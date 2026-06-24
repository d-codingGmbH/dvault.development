using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
public sealed class PostgresPitMaintenanceServiceTests {
  private const string PostgresProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";
  private const string PostgresStrategyRegistrationDiagnostic =
      "PostgreSQL PIT maintenance expected AddDVaultPostgres to register a compatible provider strategy for a clean Npgsql-backed full rebuild request.";
  private const string PostgresOptimizedPathDiagnostic =
      "PostgreSQL PIT maintenance expected the provider INSERT SELECT path to rebuild without fallback-tracked PIT rows.";
  private const string PostgresPitMaintenanceStrategyName = "PostgresDataVaultPitMaintenanceStrategy";

  [Fact]
  public async Task AddDVaultPostgresPitRebuildsSupportedBaselineShapesWhenConfigured() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var services = new ServiceCollection();
    services.AddDVaultPostgres();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();

    await WithPostgresSchemaAsync(
        configuration.ConnectionString!,
        PostgresPitMaintenanceModelKind.Ordinary,
        async context => await AssertOrdinaryPitRebuildAsync(provider, maintenanceService, context));
    await WithPostgresSchemaAsync(
        configuration.ConnectionString!,
        PostgresPitMaintenanceModelKind.MultiActive,
        async context => await AssertMultiActivePitRebuildAsync(provider, maintenanceService, context));
    await WithPostgresSchemaAsync(
        configuration.ConnectionString!,
        PostgresPitMaintenanceModelKind.LinkParent,
        async context => await AssertLinkParentPitRebuildAsync(provider, maintenanceService, context));
  }

  [Fact]
  public async Task PostgresPitRebuildFallsBackWithNoRegisteredStrategyDiagnosticsWhenPostgresStartupIsAbsent() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();

    await WithPostgresSchemaAsync(
        configuration.ConnectionString!,
        PostgresPitMaintenanceModelKind.Ordinary,
        async context => {
          var metadata = CreateOrdinaryMetadata();
          var customerHashKey = "customer-hash-fallback";
          var statusTimestamp = Utc(2026, 5, 12, 9, 0);
          var profileTimestamp = Utc(2026, 5, 12, 10, 0);
          var request = new DataVaultPitRebuildRequest(metadata.Pit);

          AddProfileRow(context, customerHashKey, profileTimestamp, "Fallback Name", "Fallback Tier", "fallback-profile");
          AddStatusRow(context, customerHashKey, statusTimestamp, "Fallback Status", "fallback-status");
          await context.SaveChangesAsync();
          context.ChangeTracker.Clear();

          using var listener = new DataVaultActivityTestListener();
          var result = await maintenanceService.RebuildAsync(context, request);

          Assert.Equal("PitCustomerProfileStatus", result.TableName);
          Assert.Equal(1, result.ParentHashKeyCount);
          Assert.Equal(0, result.RowsDeleted);
          Assert.Equal(2, result.RowsWritten);
          Assert.Collection(
              await ReadOrdinaryPitRowsAsync(context),
              row => AssertOrdinaryPitRow(row, customerHashKey, statusTimestamp, null, statusTimestamp),
              row => AssertOrdinaryPitRow(row, customerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
          AssertPostgresFallbackActivity(
              listener,
              nameof(DataVaultPitMaintenanceStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered),
              nameof(DefaultDataVaultPitMaintenanceService),
              [
                  customerHashKey,
                  "Fallback Name",
                  "Fallback Tier",
                  "Fallback Status",
                  "fallback-profile",
                  "fallback-status",
              ]);
        });
  }

  [Fact]
  public async Task AddDVaultPostgresPitRebuildFallsBackInsideAmbientCallerTransactionWhenConfigured() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var services = new ServiceCollection();
    services.AddDVaultPostgres();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();

    await WithPostgresSchemaAsync(
        configuration.ConnectionString!,
        PostgresPitMaintenanceModelKind.Ordinary,
        async context => await AssertOrdinaryPitRebuildFallbackInsideAmbientTransactionAsync(
            provider,
            maintenanceService,
            context));
  }

  private static async Task AssertOrdinaryPitRebuildAsync(
      IServiceProvider provider,
      IDataVaultPitMaintenanceService maintenanceService,
      PostgresPitMaintenanceContext context) {
    var metadata = CreateOrdinaryMetadata();
    var customerHashKey = "customer-hash-100";
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    var secondStatusTimestamp = Utc(2026, 5, 11, 11, 0);
    var request = new DataVaultPitRebuildRequest(metadata.Pit);

    Assert.Equal(PostgresProviderName, context.Database.ProviderName);
    AssertCompatiblePostgresStrategy(provider, context, request);

    AddProfileRow(context, customerHashKey, profileTimestamp, "Alice Adams", "Gold", "profile-1");
    AddStatusRow(context, customerHashKey, statusTimestamp, "Active", "status-1");
    AddStatusRow(context, customerHashKey, secondStatusTimestamp, "Preferred", "status-2");
    context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["LoadTimestamp"] = Utc(2026, 5, 11, 8, 30),
      ["ProfileLoadTimestamp"] = null!,
      ["StatusLoadTimestamp"] = null!,
    });
    await context.SaveChangesAsync();
    context.ChangeTracker.Clear();

    using var listener = new DataVaultActivityTestListener();
    var result = await maintenanceService.RebuildAsync(context, request);

    AssertProviderPathObserved(context);
    Assert.Equal("PitCustomerProfileStatus", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(3, result.RowsWritten);
    Assert.Collection(
        await ReadOrdinaryPitRowsAsync(context),
        row => AssertOrdinaryPitRow(row, customerHashKey, statusTimestamp, null, statusTimestamp),
        row => AssertOrdinaryPitRow(row, customerHashKey, profileTimestamp, profileTimestamp, statusTimestamp),
        row => AssertOrdinaryPitRow(row, customerHashKey, secondStatusTimestamp, profileTimestamp, secondStatusTimestamp));
    AssertPostgresSelectedActivity(
        listener,
        [
            customerHashKey,
            "Alice Adams",
            "Gold",
            "Active",
            "Preferred",
            "profile-1",
            "status-1",
            "status-2",
        ]);
  }

  private static async Task AssertOrdinaryPitRebuildFallbackInsideAmbientTransactionAsync(
      IServiceProvider provider,
      IDataVaultPitMaintenanceService maintenanceService,
      PostgresPitMaintenanceContext context) {
    var metadata = CreateOrdinaryMetadata();
    var customerHashKey = "customer-ambient-transaction";
    var staleTimestamp = Utc(2026, 5, 14, 8, 30);
    var profileTimestamp = Utc(2026, 5, 14, 10, 0);
    var statusTimestamp = Utc(2026, 5, 14, 11, 0);
    var request = new DataVaultPitRebuildRequest(metadata.Pit);

    AddProfileRow(context, customerHashKey, profileTimestamp, "Ambra Allen", "Silver", "profile-ambient");
    AddStatusRow(context, customerHashKey, statusTimestamp, "Active", "status-ambient");
    context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["LoadTimestamp"] = staleTimestamp,
      ["ProfileLoadTimestamp"] = null!,
      ["StatusLoadTimestamp"] = null!,
    });
    await context.SaveChangesAsync();
    context.ChangeTracker.Clear();

    await using var transaction = await context.Database.BeginTransactionAsync();
    AssertPostgresStrategyDeclinesCurrentTransaction(provider, context, request);

    using var listener = new DataVaultActivityTestListener();
    var result = await maintenanceService.RebuildAsync(context, request);

    Assert.Equal("PitCustomerProfileStatus", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(2, result.RowsWritten);
    Assert.Collection(
        await ReadOrdinaryPitRowsAsync(context),
        row => AssertOrdinaryPitRow(row, customerHashKey, profileTimestamp, profileTimestamp, statusSnapshotTimestamp: null),
        row => AssertOrdinaryPitRow(row, customerHashKey, statusTimestamp, profileTimestamp, statusTimestamp));
    AssertPostgresFallbackActivity(
        listener,
        nameof(DataVaultPitMaintenanceStrategyFallbackCauseKind.CurrentTransactionSavepointUnavailable),
        PostgresPitMaintenanceStrategyName,
        [
            customerHashKey,
            "Ambra Allen",
            "Silver",
            "Active",
            "profile-ambient",
            "status-ambient",
        ]);

    await transaction.RollbackAsync();
  }

  private static async Task AssertMultiActivePitRebuildAsync(
      IServiceProvider provider,
      IDataVaultPitMaintenanceService maintenanceService,
      PostgresPitMaintenanceContext context) {
    var metadata = CreateMultiActiveMetadata();
    var customerHashKey = "customer-hash-600";
    var profileBeforeTuple = Utc(2026, 5, 11, 8, 0);
    var billingContact = Utc(2026, 5, 11, 9, 0);
    var shippingContact = Utc(2026, 5, 11, 10, 0);
    var profileAfterTuple = Utc(2026, 5, 11, 11, 0);
    var request = new DataVaultPitRebuildRequest(metadata.Pit);

    Assert.Equal(PostgresProviderName, context.Database.ProviderName);
    AssertCompatiblePostgresStrategy(provider, context, request);

    AddProfileRow(context, customerHashKey, profileBeforeTuple, "Frank First", "Silver", "profile-before");
    AddContactRow(context, customerHashKey, billingContact, "billing", "billing@example.test", "contact-billing");
    AddContactRow(context, customerHashKey, shippingContact, "shipping", "shipping@example.test", "contact-shipping");
    AddProfileRow(context, customerHashKey, profileAfterTuple, "Frank Final", "Gold", "profile-after");
    await context.SaveChangesAsync();
    context.ChangeTracker.Clear();

    var result = await maintenanceService.RebuildAsync(context, request);

    AssertProviderPathObserved(context);
    Assert.Equal("PitCustomerContactProfile", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(0, result.RowsDeleted);
    Assert.Equal(4, result.RowsWritten);
    Assert.Collection(
        await ReadMultiActivePitRowsAsync(context),
        row => AssertMultiActivePitRow(row, customerHashKey, "billing", billingContact, billingContact, profileBeforeTuple),
        row => AssertMultiActivePitRow(row, customerHashKey, "billing", profileAfterTuple, billingContact, profileAfterTuple),
        row => AssertMultiActivePitRow(row, customerHashKey, "shipping", shippingContact, shippingContact, profileBeforeTuple),
        row => AssertMultiActivePitRow(row, customerHashKey, "shipping", profileAfterTuple, shippingContact, profileAfterTuple));
  }

  private static async Task AssertLinkParentPitRebuildAsync(
      IServiceProvider provider,
      IDataVaultPitMaintenanceService maintenanceService,
      PostgresPitMaintenanceContext context) {
    var metadata = CreateLinkParentMetadata();
    var linkHashKey = "customer-order-hash-100";
    var stateTimestamp = Utc(2026, 5, 11, 9, 0);
    var fulfillmentTimestamp = Utc(2026, 5, 11, 10, 0);
    var secondStateTimestamp = Utc(2026, 5, 11, 11, 0);
    var request = new DataVaultPitRebuildRequest(metadata.Pit);

    Assert.Equal(PostgresProviderName, context.Database.ProviderName);
    AssertCompatiblePostgresStrategy(provider, context, request);

    AddLinkStateRow(context, linkHashKey, stateTimestamp, "Packed", "state-1");
    AddLinkFulfillmentRow(context, linkHashKey, fulfillmentTimestamp, "Dock 12", "fulfillment-1");
    AddLinkStateRow(context, linkHashKey, secondStateTimestamp, "Shipped", "state-2");
    context.Set<Dictionary<string, object>>("PitCustomerOrderStateFulfillment").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerOrderHashKey"] = linkHashKey,
      ["LoadTimestamp"] = Utc(2026, 5, 11, 8, 30),
      ["StateLoadTimestamp"] = null!,
      ["FulfillmentLoadTimestamp"] = null!,
    });
    await context.SaveChangesAsync();
    context.ChangeTracker.Clear();

    var result = await maintenanceService.RebuildAsync(context, request);

    AssertProviderPathObserved(context);
    Assert.Equal("PitCustomerOrderStateFulfillment", result.TableName);
    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(3, result.RowsWritten);
    Assert.Collection(
        await ReadLinkParentPitRowsAsync(context),
        row => AssertLinkParentPitRow(row, linkHashKey, stateTimestamp, stateTimestamp, null),
        row => AssertLinkParentPitRow(row, linkHashKey, fulfillmentTimestamp, stateTimestamp, fulfillmentTimestamp),
        row => AssertLinkParentPitRow(row, linkHashKey, secondStateTimestamp, secondStateTimestamp, fulfillmentTimestamp));
  }

  private static async Task WithPostgresSchemaAsync(
      string connectionString,
      PostgresPitMaintenanceModelKind modelKind,
      Func<PostgresPitMaintenanceContext, Task> execute) {
    var schemaName = "dvault_test_" + Guid.NewGuid().ToString("N");
    var options = CreatePostgresOptions(connectionString);

    await using var context = new PostgresPitMaintenanceContext(options, schemaName, modelKind);
    await context.Database.ExecuteSqlRawAsync("CREATE SCHEMA " + QuoteIdentifier(schemaName) + ";");

    try {
      await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());
      await execute(context);
    }
    finally {
      await context.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS " + QuoteIdentifier(schemaName) + " CASCADE;");
    }
  }

  private static DbContextOptions<PostgresPitMaintenanceContext> CreatePostgresOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<PostgresPitMaintenanceContext>();

    NpgsqlProviderReflection.UseNpgsql(optionsBuilder, connectionString);
    optionsBuilder.ReplaceService<IModelCacheKeyFactory, PostgresPitMaintenanceModelCacheKeyFactory>();

    return optionsBuilder.Options;
  }

  private static void AssertCompatiblePostgresStrategy(
      IServiceProvider provider,
      PostgresPitMaintenanceContext context,
      DataVaultPitRebuildRequest request) {
    Assert.True(
        provider.GetServices<IDataVaultProviderPitMaintenanceStrategy>().Any(strategy => strategy.CanRebuild(context, request)),
        PostgresStrategyRegistrationDiagnostic);
  }

  private static void AssertPostgresStrategyDeclinesCurrentTransaction(
      IServiceProvider provider,
      PostgresPitMaintenanceContext context,
      DataVaultPitRebuildRequest request) {
    Assert.False(
        provider.GetServices<IDataVaultProviderPitMaintenanceStrategy>().Any(strategy => strategy.CanRebuild(context, request)));
    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator.EvaluatePostgres(context, request).FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.CurrentTransactionSavepointUnavailable);
  }

  private static void AssertProviderPathObserved(PostgresPitMaintenanceContext context) {
    var trackedEntries = context.ChangeTracker.Entries().ToArray();

    Assert.True(
        trackedEntries.Length == 0,
        PostgresOptimizedPathDiagnostic + " Actual tracked entries: " + trackedEntries.Length);
  }

  private static void AssertPostgresSelectedActivity(
      DataVaultActivityTestListener listener,
      IReadOnlyList<string> sensitiveValues) {
    var activity = Assert.Single(
        listener.StoppedActivities,
        current => string.Equals(current.OperationName, DataVaultActivityTracing.PitRebuildOperation, StringComparison.Ordinal));
    var tags = GetTags(activity);

    Assert.Equal(ActivityStatusCode.Ok, activity.Status);
    Assert.Equal("success", tags[DataVaultActivityTracing.OutcomeTag]);
    Assert.Equal(PostgresProviderName, tags[DataVaultActivityTracing.ProviderTag]);
    Assert.Equal("ProviderStrategySelected", tags[DataVaultActivityTracing.StrategyStatusTag]);
    Assert.Equal(PostgresPitMaintenanceStrategyName, tags[DataVaultActivityTracing.StrategyTypeTag]);
    Assert.Contains(activity.Events, current =>
        string.Equals(current.Name, DataVaultActivityTracing.StrategySelectedEvent, StringComparison.Ordinal) &&
        string.Equals(
            Convert.ToString(GetTags(current)[DataVaultActivityTracing.StrategyTypeTag], CultureInfo.InvariantCulture),
            PostgresPitMaintenanceStrategyName,
            StringComparison.Ordinal));
    AssertActivityRedacted(activity, sensitiveValues);
  }

  private static void AssertPostgresFallbackActivity(
      DataVaultActivityTestListener listener,
      string fallbackCause,
      string expectedStrategyType,
      IReadOnlyList<string> sensitiveValues) {
    var activity = Assert.Single(
        listener.StoppedActivities,
        current => string.Equals(current.OperationName, DataVaultActivityTracing.PitRebuildOperation, StringComparison.Ordinal));
    var tags = GetTags(activity);

    Assert.Equal(ActivityStatusCode.Ok, activity.Status);
    Assert.Equal("success", tags[DataVaultActivityTracing.OutcomeTag]);
    Assert.Equal(PostgresProviderName, tags[DataVaultActivityTracing.ProviderTag]);
    Assert.Equal("ProviderNeutralFallback", tags[DataVaultActivityTracing.StrategyStatusTag]);
    Assert.Equal(expectedStrategyType, tags[DataVaultActivityTracing.StrategyTypeTag]);
    Assert.Contains(activity.Events, current =>
        string.Equals(current.Name, DataVaultActivityTracing.FallbackRecordedEvent, StringComparison.Ordinal) &&
        string.Equals(
            Convert.ToString(GetTags(current)[DataVaultActivityTracing.FallbackCauseTag], CultureInfo.InvariantCulture),
            fallbackCause,
            StringComparison.Ordinal));
    AssertActivityRedacted(activity, sensitiveValues);
  }

  private static void AssertActivityRedacted(
      Activity activity,
      IReadOnlyList<string> sensitiveValues) {
    var telemetryValues = GetTags(activity)
        .Values
        .Concat(activity.Events.SelectMany(current => GetTags(current).Values))
        .Select(value => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty)
        .ToArray();

    foreach (var value in telemetryValues) {
      Assert.DoesNotContain("SELECT ", value, StringComparison.OrdinalIgnoreCase);
      Assert.DoesNotContain("INSERT ", value, StringComparison.OrdinalIgnoreCase);
      Assert.DoesNotContain("DELETE ", value, StringComparison.OrdinalIgnoreCase);
      Assert.DoesNotContain("Host=", value, StringComparison.OrdinalIgnoreCase);
      Assert.DoesNotContain("Username=", value, StringComparison.OrdinalIgnoreCase);
      Assert.DoesNotContain("Password=", value, StringComparison.OrdinalIgnoreCase);
    }

    foreach (var sensitiveValue in sensitiveValues) {
      Assert.All(
          telemetryValues,
          value => Assert.DoesNotContain(sensitiveValue, value, StringComparison.Ordinal));
    }
  }

  private static IReadOnlyDictionary<string, object?> GetTags(Activity activity) {
    return activity.TagObjects.ToDictionary(tag => tag.Key, tag => tag.Value, StringComparer.Ordinal);
  }

  private static IReadOnlyDictionary<string, object?> GetTags(ActivityEvent activityEvent) {
    return activityEvent.Tags.ToDictionary(tag => tag.Key, tag => tag.Value, StringComparer.Ordinal);
  }

  private static void AddProfileRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      string customerName,
      string customerTier,
      string hashDiff) {
    context.Set<Dictionary<string, object>>("SatCustomerProfile").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["HashDiff"] = hashDiff,
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "crm-profile",
      ["CustomerName"] = customerName,
      ["CustomerTier"] = customerTier,
    });
  }

  private static void AddStatusRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      string statusCode,
      string hashDiff) {
    context.Set<Dictionary<string, object>>("SatCustomerStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["HashDiff"] = hashDiff,
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "crm-status",
      ["StatusCode"] = statusCode,
    });
  }

  private static void AddContactRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      string contactType,
      string emailAddress,
      string hashDiff) {
    context.Set<Dictionary<string, object>>("SatCustomerContact").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["ContactType"] = contactType,
      ["HashDiff"] = hashDiff,
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "crm-contact",
      ["EmailAddress"] = emailAddress,
    });
  }

  private static void AddLinkStateRow(
      DbContext context,
      string linkHashKey,
      DateTimeOffset loadTimestamp,
      string stateCode,
      string hashDiff) {
    context.Set<Dictionary<string, object>>("SatCustomerOrderState").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerOrderHashKey"] = linkHashKey,
      ["HashDiff"] = hashDiff,
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "order-state",
      ["StateCode"] = stateCode,
    });
  }

  private static void AddLinkFulfillmentRow(
      DbContext context,
      string linkHashKey,
      DateTimeOffset loadTimestamp,
      string location,
      string hashDiff) {
    context.Set<Dictionary<string, object>>("SatCustomerOrderFulfillment").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerOrderHashKey"] = linkHashKey,
      ["HashDiff"] = hashDiff,
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "order-fulfillment",
      ["FulfillmentLocation"] = location,
    });
  }

  private static async Task<IReadOnlyList<OrdinaryPitRow>> ReadOrdinaryPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerProfileStatus")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new OrdinaryPitRow(
            Assert.IsType<string>(row["CustomerHashKey"]),
            ReadTimestamp(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp"),
            ReadOptionalTimestamp(row, "StatusLoadTimestamp")))
        .OrderBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static async Task<IReadOnlyList<MultiActivePitRow>> ReadMultiActivePitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerContactProfile")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new MultiActivePitRow(
            Assert.IsType<string>(row["CustomerHashKey"]),
            Assert.IsType<string>(row["ContactType"]),
            ReadTimestamp(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "ContactLoadTimestamp"),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp")))
        .OrderBy(row => row.ContactType, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static async Task<IReadOnlyList<LinkParentPitRow>> ReadLinkParentPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerOrderStateFulfillment")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new LinkParentPitRow(
            Assert.IsType<string>(row["CustomerOrderHashKey"]),
            ReadTimestamp(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "StateLoadTimestamp"),
            ReadOptionalTimestamp(row, "FulfillmentLoadTimestamp")))
        .OrderBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static DateTimeOffset ReadTimestamp(object value) {
    return DataVaultLoadTimestampValueConverter.ReadProviderValue(value);
  }

  private static DateTimeOffset? ReadOptionalTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName) {
    return row.TryGetValue(columnName, out var value) && value is not null
        ? DataVaultLoadTimestampValueConverter.ReadProviderValue(value)
        : null;
  }

  private static void AssertOrdinaryPitRow(
      OrdinaryPitRow row,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(profileSnapshotTimestamp, row.ProfileSnapshotTimestamp);
    Assert.Equal(statusSnapshotTimestamp, row.StatusSnapshotTimestamp);
  }

  private static void AssertMultiActivePitRow(
      MultiActivePitRow row,
      string parentHashKey,
      string contactType,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? contactSnapshotTimestamp,
      DateTimeOffset? profileSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(contactType, row.ContactType);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(contactSnapshotTimestamp, row.ContactSnapshotTimestamp);
    Assert.Equal(profileSnapshotTimestamp, row.ProfileSnapshotTimestamp);
  }

  private static void AssertLinkParentPitRow(
      LinkParentPitRow row,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? stateSnapshotTimestamp,
      DateTimeOffset? fulfillmentSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(stateSnapshotTimestamp, row.StateSnapshotTimestamp);
    Assert.Equal(fulfillmentSnapshotTimestamp, row.FulfillmentSnapshotTimestamp);
  }

  private static PitMaintenanceMetadata CreateOrdinaryMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new PitMaintenanceMetadata(pit, model);
  }

  private static PitMaintenanceMetadata CreateMultiActiveMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var pit = new DataVaultPitMetadata(
        customer.ToReference(),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Profile"),
        ]);
    var model = new DataVaultMetadataModel([customer], [], [profile, contact], [pit]);

    return new PitMaintenanceMetadata(pit, model);
  }

  private static PitMaintenanceMetadata CreateLinkParentMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var state = new DataVaultSatelliteMetadata(
        "State",
        customerOrder.ToReference(),
        ["State Code"]);
    var fulfillment = new DataVaultSatelliteMetadata(
        "Fulfillment",
        customerOrder.ToReference(),
        ["Fulfillment Location"]);
    var pit = new DataVaultPitMetadata(customerOrder.ToReference(), ["State", "Fulfillment"]);
    var model = new DataVaultMetadataModel([customer, order], [customerOrder], [state, fulfillment], [pit]);

    return new PitMaintenanceMetadata(pit, model);
  }

  private static DataVaultMetadataModel CreateModel(PostgresPitMaintenanceModelKind modelKind) {
    return modelKind switch {
      PostgresPitMaintenanceModelKind.Ordinary => CreateOrdinaryMetadata().Model,
      PostgresPitMaintenanceModelKind.MultiActive => CreateMultiActiveMetadata().Model,
      PostgresPitMaintenanceModelKind.LinkParent => CreateLinkParentMetadata().Model,
      _ => throw new ArgumentOutOfRangeException(nameof(modelKind)),
    };
  }

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private static string QuoteIdentifier(string value) {
    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private sealed class PostgresPitMaintenanceContext(
      DbContextOptions<PostgresPitMaintenanceContext> options,
      string schemaName,
      PostgresPitMaintenanceModelKind modelKind) : DbContext(options) {
    public string SchemaName { get; } = schemaName;

    public PostgresPitMaintenanceModelKind ModelKind { get; } = modelKind;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.HasDefaultSchema(SchemaName);
      modelBuilder.ApplyDataVaultMetadata(
          CreateModel(ModelKind),
          DataVaultProviderCapabilityProfiles.Postgres);
    }
  }

  private sealed class PostgresPitMaintenanceModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is PostgresPitMaintenanceContext pitContext
          ? (context.GetType(), pitContext.SchemaName, pitContext.ModelKind, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private enum PostgresPitMaintenanceModelKind {
    Ordinary,
    MultiActive,
    LinkParent,
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record OrdinaryPitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp,
      DateTimeOffset? StatusSnapshotTimestamp);

  private sealed record MultiActivePitRow(
      string ParentHashKey,
      string ContactType,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ContactSnapshotTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp);

  private sealed record LinkParentPitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? StateSnapshotTimestamp,
      DateTimeOffset? FulfillmentSnapshotTimestamp);

}
