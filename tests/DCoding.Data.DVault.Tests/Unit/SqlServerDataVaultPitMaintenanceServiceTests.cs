using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class SqlServerDataVaultPitMaintenanceServiceTests {
  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerPitMaintenanceCandidateGateAcceptsOnlyCleanSqlServerOrdinaryHubParentRebuilds() {
    var ordinaryPit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var linkParentPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"]);
    var multiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true)]);

    var accepted = SqlServerDataVaultPitMaintenanceService.EvaluateRebuildCandidate(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: false,
        ordinaryPit);
    var wrongProvider = SqlServerDataVaultPitMaintenanceService.EvaluateRebuildCandidate(
        KnownProviderNames.Sqlite,
        hasPendingTrackedChanges: false,
        ordinaryPit);
    var dirtyContext = SqlServerDataVaultPitMaintenanceService.EvaluateRebuildCandidate(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: true,
        ordinaryPit);
    var linkParent = SqlServerDataVaultPitMaintenanceService.EvaluateRebuildCandidate(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: false,
        linkParentPit);
    var multiActive = SqlServerDataVaultPitMaintenanceService.EvaluateRebuildCandidate(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: false,
        multiActivePit);
    var callerTransactionWithoutSavepoints = SqlServerDataVaultPitMaintenanceService.EvaluateRebuildCandidate(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: false,
        ordinaryPit,
        hasCurrentTransactionWithoutSavepoints: true);

    Assert.True(accepted.CanRebuild);
    Assert.Empty(accepted.FallbackCauses);
    Assert.Contains(
        wrongProvider.FallbackCauses,
        cause => cause.Kind == SqlServerPitMaintenanceFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        dirtyContext.FallbackCauses,
        cause => cause.Kind == SqlServerPitMaintenanceFallbackCauseKind.DirtyDbContext);
    Assert.Contains(
        linkParent.FallbackCauses,
        cause => cause.Kind == SqlServerPitMaintenanceFallbackCauseKind.UnsupportedPitParent);
    Assert.Contains(
        multiActive.FallbackCauses,
        cause => cause.Kind == SqlServerPitMaintenanceFallbackCauseKind.MultiActivePitUnsupported);
    Assert.Contains(
        callerTransactionWithoutSavepoints.FallbackCauses,
        cause => cause.Kind == SqlServerPitMaintenanceFallbackCauseKind.CurrentTransactionSavepointUnavailable);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerPitMaintenanceRebuildSqlUsesSetBasedInsertSelectWithSnapshotLookups() {
    var metadata = CreateCustomerContactProfileMetadata();
    var options = new DbContextOptionsBuilder<PitMaintenanceCommandContext>()
        .UseSqlite("Data Source=:memory:")
        .Options;
    using var context = new PitMaintenanceCommandContext(options, metadata.Model);
    var projection = DefaultDataVaultPitMaintenanceService.CreatePitProjection(context, metadata.Pit);

    var insertCommandText = SqlServerDataVaultPitMaintenanceService.CreateSqlServerPitRebuildInsertCommandText(
        context,
        projection);
    var parentCountCommandText = SqlServerDataVaultPitMaintenanceService.CreateSqlServerPitParentCountCommandText(
        context,
        projection);

    Assert.Contains("INSERT INTO [PitCustomerContactProfile]", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("WITH [__dvault_pit_timestamps] AS", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("SELECT TOP(1) [snapshot0].[LoadTimestamp]", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("SELECT TOP(1) [snapshot1].[LoadTimestamp]", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("FROM [SatCustomerContact] AS [sat0] UNION SELECT", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("FROM [SatCustomerProfile] AS [sat1]", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("ORDER BY [snapshot0].[LoadTimestamp] DESC", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("ORDER BY [snapshot1].[LoadTimestamp] DESC", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("SELECT COUNT(1) FROM", parentCountCommandText, StringComparison.Ordinal);
    Assert.Contains("UNION", parentCountCommandText, StringComparison.Ordinal);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public async Task SqlServerPitMaintenanceRebuildFallsBackToProviderNeutralPathWhenProviderDoesNotMatch() {
    var metadata = CreateCustomerContactProfileMetadata();
    var service = new SqlServerDataVaultPitMaintenanceService();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitMaintenanceCommandContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var customerHashKey = "customer-fallback";
    var staleTimestamp = Utc(2026, 5, 4, 9, 30);
    var contactTimestamp = Utc(2026, 5, 4, 10, 0);
    var profileTimestamp = Utc(2026, 5, 4, 10, 30);

    await using var context = new PitMaintenanceCommandContext(options, metadata.Model);
    await context.Database.EnsureCreatedAsync();
    AddPitRow(context, customerHashKey, staleTimestamp, contactTimestamp: null, profileTimestamp: null);
    AddContactRow(context, customerHashKey, contactTimestamp);
    AddProfileRow(context, customerHashKey, profileTimestamp);
    await context.SaveChangesAsync();

    using var listener = new PitMaintenanceActivityListener();
    var result = await service.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));
    var rows = await ReadPitRowsAsync(context);

    Assert.Equal("PitCustomerContactProfile", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(2, result.RowsWritten);
    Assert.Collection(
        rows,
        row => AssertPitRow(row, customerHashKey, contactTimestamp, contactTimestamp, profileTimestamp: null),
        row => AssertPitRow(row, customerHashKey, profileTimestamp, contactTimestamp, profileTimestamp));

    AssertPitRebuildFallbackCause(listener, nameof(SqlServerPitMaintenanceFallbackCauseKind.ProviderNameMismatch));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public async Task SqlServerPitMaintenanceRebuildFallsBackToProviderNeutralPathWhenContextIsDirty() {
    var metadata = CreateCustomerContactProfileMetadata();
    var service = new SqlServerDataVaultPitMaintenanceService();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitMaintenanceCommandContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var customerHashKey = "customer-dirty-fallback";
    var staleTimestamp = Utc(2026, 5, 4, 9, 30);
    var unsavedPitTimestamp = Utc(2026, 5, 4, 9, 45);
    var contactTimestamp = Utc(2026, 5, 4, 10, 0);
    var profileTimestamp = Utc(2026, 5, 4, 10, 30);

    await using var context = new PitMaintenanceCommandContext(options, metadata.Model);
    await context.Database.EnsureCreatedAsync();
    AddPitRow(context, customerHashKey, staleTimestamp, contactTimestamp: null, profileTimestamp: null);
    AddContactRow(context, customerHashKey, contactTimestamp);
    AddProfileRow(context, customerHashKey, profileTimestamp);
    await context.SaveChangesAsync();
    AddPitRow(context, customerHashKey, unsavedPitTimestamp, contactTimestamp: null, profileTimestamp: null);
    Assert.True(context.ChangeTracker.HasChanges());

    using var listener = new PitMaintenanceActivityListener();
    var result = await service.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));
    var rows = await ReadPitRowsAsync(context);

    Assert.Equal("PitCustomerContactProfile", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(2, result.RowsWritten);
    Assert.Collection(
        rows,
        row => AssertPitRow(row, customerHashKey, contactTimestamp, contactTimestamp, profileTimestamp: null),
        row => AssertPitRow(row, customerHashKey, profileTimestamp, contactTimestamp, profileTimestamp));

    AssertPitRebuildFallbackCause(listener, nameof(SqlServerPitMaintenanceFallbackCauseKind.DirtyDbContext));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public async Task PitMaintenanceRebuildUsesProviderNeutralPathWhenProviderSpecificRegistrationIsOmitted() {
    var metadata = CreateCustomerContactProfileMetadata();
    var services = new ServiceCollection();
    services.AddDVault();
    using var provider = services.BuildServiceProvider(validateScopes: true);
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitMaintenanceCommandContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var customerHashKey = "customer-no-provider-strategy";
    var staleTimestamp = Utc(2026, 5, 4, 9, 30);
    var contactTimestamp = Utc(2026, 5, 4, 10, 0);
    var profileTimestamp = Utc(2026, 5, 4, 10, 30);

    Assert.Empty(provider.GetServices<IDataVaultProviderPitMaintenanceStrategy>());

    await using var context = new PitMaintenanceCommandContext(options, metadata.Model);
    await context.Database.EnsureCreatedAsync();
    AddPitRow(context, customerHashKey, staleTimestamp, contactTimestamp: null, profileTimestamp: null);
    AddContactRow(context, customerHashKey, contactTimestamp);
    AddProfileRow(context, customerHashKey, profileTimestamp);
    await context.SaveChangesAsync();

    var result = await maintenanceService.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));
    var rows = await ReadPitRowsAsync(context);

    Assert.IsType<DefaultDataVaultPitMaintenanceService>(maintenanceService);
    Assert.Equal("PitCustomerContactProfile", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(2, result.RowsWritten);
    Assert.Collection(
        rows,
        row => AssertPitRow(row, customerHashKey, contactTimestamp, contactTimestamp, profileTimestamp: null),
        row => AssertPitRow(row, customerHashKey, profileTimestamp, contactTimestamp, profileTimestamp));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public async Task SqlServerPitMaintenanceRebuildFallsBackToProviderNeutralPathForUnsupportedMultiActivePit() {
    var metadata = CreateCustomerContactProfileMultiActiveMetadata();
    var service = new SqlServerDataVaultPitMaintenanceService();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<MultiActivePitMaintenanceCommandContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var customerHashKey = "customer-multiactive-fallback";
    var contactType = "billing";
    var staleTimestamp = Utc(2026, 5, 4, 9, 30);
    var contactTimestamp = Utc(2026, 5, 4, 10, 0);
    var profileTimestamp = Utc(2026, 5, 4, 10, 30);

    await using var context = new MultiActivePitMaintenanceCommandContext(options);
    await context.Database.EnsureCreatedAsync();
    AddMultiActivePitRow(
        context,
        customerHashKey,
        contactType,
        staleTimestamp,
        contactTimestamp: null,
        profileTimestamp: null);
    AddMultiActiveContactRow(context, customerHashKey, contactType, contactTimestamp);
    AddProfileRow(context, customerHashKey, profileTimestamp);
    await context.SaveChangesAsync();

    using var listener = new PitMaintenanceActivityListener();
    var result = await service.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));
    var rows = await ReadMultiActivePitRowsAsync(context);

    Assert.Equal("PitCustomerContactProfile", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(2, result.RowsWritten);
    Assert.Collection(
        rows,
        row => AssertMultiActivePitRow(row, customerHashKey, contactType, contactTimestamp, contactTimestamp, profileTimestamp: null),
        row => AssertMultiActivePitRow(row, customerHashKey, contactType, profileTimestamp, contactTimestamp, profileTimestamp));

    AssertPitRebuildFallbackCause(listener, nameof(SqlServerPitMaintenanceFallbackCauseKind.MultiActivePitUnsupported));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public async Task SqlServerPitMaintenanceMaintainParentsFallsBackToProviderNeutralNoOp() {
    var metadata = CreateCustomerContactProfileMetadata();
    var service = new SqlServerDataVaultPitMaintenanceService();
    await using var context = new DbContext(new DbContextOptionsBuilder().Options);

    using var listener = new PitMaintenanceActivityListener();
    var result = await service.MaintainParentsAsync(
        context,
        new DataVaultPitParentMaintenanceRequest(metadata.Pit, []));

    Assert.Equal("PitCustomerContactProfile", result.TableName);
    Assert.True(result.IsNoOp);

    var activity = Assert.Single(listener.StoppedActivities.Where(current =>
        string.Equals(current.OperationName, DataVaultActivityTracing.PitMaintainParentsOperation, StringComparison.Ordinal)));
    var tags = GetTags(activity);
    Assert.Equal("ProviderNeutralFallback", tags[DataVaultActivityTracing.StrategyStatusTag]);
    Assert.Contains(activity.Events, current =>
        string.Equals(current.Name, DataVaultActivityTracing.FallbackRecordedEvent, StringComparison.Ordinal) &&
        string.Equals(
            Convert.ToString(GetTags(current)[DataVaultActivityTracing.FallbackCauseTag], CultureInfo.InvariantCulture),
            nameof(SqlServerPitMaintenanceFallbackCauseKind.MaintainParentsUnsupported),
            StringComparison.Ordinal));
  }

  private static PitMaintenanceMetadata CreateCustomerContactProfileMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Tier"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Contact", "Profile"]);
    var model = new DataVaultMetadataModel([customer], [], [contact, profile], [pit]);

    return new PitMaintenanceMetadata(pit, model);
  }

  private static PitMaintenanceMetadata CreateCustomerContactProfileMultiActiveMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Tier"]);
    var pit = new DataVaultPitMetadata(
        customer.ToReference(),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Profile"),
        ]);
    var model = new DataVaultMetadataModel([customer], [], [contact, profile], [pit]);

    return new PitMaintenanceMetadata(pit, model);
  }

  private static void AddContactRow(
      DbContext context,
      string customerHashKey,
      DateTimeOffset loadTimestamp) {
    context.Set<Dictionary<string, object>>("SatCustomerContact").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["HashDiff"] = "contact-" + loadTimestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "sqlserver-pit-fallback-test",
      ["EmailAddress"] = "fallback@example.test",
    });
  }

  private static void AddProfileRow(
      DbContext context,
      string customerHashKey,
      DateTimeOffset loadTimestamp) {
    context.Set<Dictionary<string, object>>("SatCustomerProfile").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["HashDiff"] = "profile-" + loadTimestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "sqlserver-pit-fallback-test",
      ["Tier"] = "Gold",
    });
  }

  private static void AddPitRow(
      DbContext context,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? contactTimestamp,
      DateTimeOffset? profileTimestamp) {
    context.Set<Dictionary<string, object>>("PitCustomerContactProfile").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["LoadTimestamp"] = loadTimestamp,
      ["ContactLoadTimestamp"] = contactTimestamp.HasValue ? contactTimestamp.Value : null!,
      ["ProfileLoadTimestamp"] = profileTimestamp.HasValue ? profileTimestamp.Value : null!,
    });
  }

  private static void AddMultiActiveContactRow(
      DbContext context,
      string customerHashKey,
      string contactType,
      DateTimeOffset loadTimestamp) {
    context.Set<Dictionary<string, object>>("SatCustomerContact").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["ContactType"] = contactType,
      ["HashDiff"] = "contact-" + loadTimestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "sqlserver-pit-fallback-test",
      ["EmailAddress"] = contactType + "@example.test",
    });
  }

  private static void AddMultiActivePitRow(
      DbContext context,
      string customerHashKey,
      string contactType,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? contactTimestamp,
      DateTimeOffset? profileTimestamp) {
    context.Set<Dictionary<string, object>>("PitCustomerContactProfile").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["ContactType"] = contactType,
      ["LoadTimestamp"] = loadTimestamp,
      ["ContactLoadTimestamp"] = contactTimestamp.HasValue ? contactTimestamp.Value : null!,
      ["ProfileLoadTimestamp"] = profileTimestamp.HasValue ? profileTimestamp.Value : null!,
    });
  }

  private static async Task<IReadOnlyList<PitRowSnapshot>> ReadPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerContactProfile")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new PitRowSnapshot(
            Assert.IsType<string>(row["CustomerHashKey"]),
            ReadRequiredTimestamp(row, "LoadTimestamp"),
            ReadOptionalTimestamp(row, "ContactLoadTimestamp"),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp")))
        .OrderBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static async Task<IReadOnlyList<MultiActivePitRowSnapshot>> ReadMultiActivePitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerContactProfile")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new MultiActivePitRowSnapshot(
            Assert.IsType<string>(row["CustomerHashKey"]),
            Assert.IsType<string>(row["ContactType"]),
            ReadRequiredTimestamp(row, "LoadTimestamp"),
            ReadOptionalTimestamp(row, "ContactLoadTimestamp"),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp")))
        .OrderBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static DateTimeOffset ReadRequiredTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName) {
    Assert.True(DataVaultLoadTimestampValueConverter.TryReadProviderValue(row[columnName], out var timestamp));

    return timestamp;
  }

  private static DateTimeOffset? ReadOptionalTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName) {
    if (!row.TryGetValue(columnName, out var value) || value is null or DBNull) {
      return null;
    }

    Assert.True(DataVaultLoadTimestampValueConverter.TryReadProviderValue(value, out var timestamp));

    return timestamp;
  }

  private static void AssertPitRow(
      PitRowSnapshot row,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? contactTimestamp,
      DateTimeOffset? profileTimestamp) {
    Assert.Equal(customerHashKey, row.ParentHashKey);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(contactTimestamp, row.ContactLoadTimestamp);
    Assert.Equal(profileTimestamp, row.ProfileLoadTimestamp);
  }

  private static void AssertMultiActivePitRow(
      MultiActivePitRowSnapshot row,
      string customerHashKey,
      string contactType,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? contactTimestamp,
      DateTimeOffset? profileTimestamp) {
    Assert.Equal(customerHashKey, row.ParentHashKey);
    Assert.Equal(contactType, row.ContactType);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(contactTimestamp, row.ContactLoadTimestamp);
    Assert.Equal(profileTimestamp, row.ProfileLoadTimestamp);
  }

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private static void AssertPitRebuildFallbackCause(
      PitMaintenanceActivityListener listener,
      string fallbackCause) {
    var activity = Assert.Single(listener.StoppedActivities.Where(current =>
        string.Equals(current.OperationName, DataVaultActivityTracing.PitRebuildOperation, StringComparison.Ordinal)));
    var tags = GetTags(activity);
    Assert.Equal("ProviderNeutralFallback", tags[DataVaultActivityTracing.StrategyStatusTag]);
    Assert.Contains(activity.Events, current =>
        string.Equals(current.Name, DataVaultActivityTracing.FallbackRecordedEvent, StringComparison.Ordinal) &&
        string.Equals(
            Convert.ToString(GetTags(current)[DataVaultActivityTracing.FallbackCauseTag], CultureInfo.InvariantCulture),
            fallbackCause,
            StringComparison.Ordinal));
  }

  private static IReadOnlyDictionary<string, object?> GetTags(Activity activity) {
    return activity.TagObjects.ToDictionary(tag => tag.Key, tag => tag.Value, StringComparer.Ordinal);
  }

  private static IReadOnlyDictionary<string, object?> GetTags(ActivityEvent activityEvent) {
    return activityEvent.Tags.ToDictionary(tag => tag.Key, tag => tag.Value, StringComparer.Ordinal);
  }

  private sealed class PitMaintenanceCommandContext(
      DbContextOptions<PitMaintenanceCommandContext> options,
      DataVaultMetadataModel metadataModel) : DbContext(options) {
    public DataVaultMetadataModel MetadataModel { get; } = metadataModel;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(MetadataModel);
    }
  }

  private sealed class MultiActivePitMaintenanceCommandContext(
      DbContextOptions<MultiActivePitMaintenanceCommandContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateCustomerContactProfileMultiActiveMetadata().Model);
    }
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record PitRowSnapshot(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ContactLoadTimestamp,
      DateTimeOffset? ProfileLoadTimestamp);

  private sealed record MultiActivePitRowSnapshot(
      string ParentHashKey,
      string ContactType,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ContactLoadTimestamp,
      DateTimeOffset? ProfileLoadTimestamp);

  private sealed class PitMaintenanceActivityListener : IDisposable {
    private readonly ActivityListener _listener;
    private readonly List<Activity> _stoppedActivities = [];

    public PitMaintenanceActivityListener() {
      _listener = new ActivityListener {
        ShouldListenTo = source => string.Equals(source.Name, "DCoding.Data.DVault", StringComparison.Ordinal),
        Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
        ActivityStopped = activity => _stoppedActivities.Add(activity),
      };

      ActivitySource.AddActivityListener(_listener);
    }

    public IReadOnlyList<Activity> StoppedActivities => _stoppedActivities;

    public void Dispose() {
      _listener.Dispose();
    }
  }
}
