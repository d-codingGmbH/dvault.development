using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultPitMaintenanceServiceTests {
  [Fact]
  public void PitParentMaintenanceRequestDeduplicatesParentHashKeysOrdinally() {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

    var request = new DataVaultPitParentMaintenanceRequest(
        pit,
        ["customer-hash", "CUSTOMER-HASH", "customer-hash"]);

    Assert.Same(pit, request.Pit);
    Assert.Equal(["customer-hash", "CUSTOMER-HASH"], request.ParentHashKeys);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("   ")]
  public void PitParentMaintenanceRequestRejectsNullEmptyOrWhitespaceParentHashKeys(string? parentHashKey) {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

    Assert.ThrowsAny<ArgumentException>(() => new DataVaultPitParentMaintenanceRequest(
        pit,
        [parentHashKey!]));
  }

  [Fact]
  public void AddDVaultRegistersPitMaintenanceServiceBesideSaveAndReadServices() {
    using var provider = new ServiceCollection()
        .AddDVault()
        .BuildServiceProvider(validateScopes: true);

    Assert.IsType<DefaultDataVaultSaveService>(provider.GetRequiredService<IDataVaultSaveService>());
    Assert.IsType<DefaultDataVaultReadService>(provider.GetRequiredService<IDataVaultReadService>());
    Assert.IsType<DefaultDataVaultPitMaintenanceService>(provider.GetRequiredService<IDataVaultPitMaintenanceService>());
  }

  [Fact]
  public async Task EmptyParentMaintenanceRequestIsNoOpWithoutModelValidation() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new EmptyPitModelContext(new DbContextOptionsBuilder<EmptyPitModelContext>().Options);

    var result = await service.MaintainParentsAsync(
        context,
        new DataVaultPitParentMaintenanceRequest(CreateCustomerProfilePit(), []));

    Assert.Equal("CustomerProfile", result.Pit.Name);
    Assert.Equal("PitCustomerProfile", result.TableName);
    Assert.Equal(0, result.ParentHashKeyCount);
    Assert.Equal(0, result.RowsDeleted);
    Assert.Equal(0, result.RowsWritten);
    Assert.True(result.IsNoOp);
  }

  [Fact]
  public async Task RegistryBackedPitMaintenanceDelegatesRebuildByNameAndClrMapping() {
    var metadata = CreateCustomerProfileStatusMetadata();
    var registry = CreateCustomerProfileStatusRegistry(metadata);
    var service = new RecordingPitMaintenanceService();
    await using var context = CreateRegistryContext(registry);

    await service.RebuildAsync(
        context,
        new DataVaultRegistryPitRebuildRequest(metadata.Pit.Name));
    await service.RebuildAsync(
        context,
        new DataVaultRegistryPitRebuildRequest(typeof(CustomerProfileStatusPitMapping)));

    Assert.Equal([metadata.Pit, metadata.Pit], service.RebuildRequests);
  }

  [Fact]
  public async Task RegistryBackedPitMaintenanceDelegatesParentMaintenanceByNameAndClrMapping() {
    var metadata = CreateCustomerProfileStatusMetadata();
    var registry = CreateCustomerProfileStatusRegistry(metadata);
    var service = new RecordingPitMaintenanceService();
    await using var context = CreateRegistryContext(registry);

    await service.MaintainParentsAsync(
        context,
        new DataVaultRegistryPitParentMaintenanceRequest(metadata.Pit.Name, ["customer-hash", "customer-hash"]));
    await service.MaintainParentsAsync(
        context,
        new DataVaultRegistryPitParentMaintenanceRequest(typeof(CustomerProfileStatusPitMapping), ["other-hash"]));

    Assert.Equal([metadata.Pit, metadata.Pit], service.ParentRequests.Select(request => request.Pit));
    Assert.Equal(["customer-hash"], service.ParentRequests[0].ParentHashKeys);
    Assert.Equal(["other-hash"], service.ParentRequests[1].ParentHashKeys);
  }

  [Fact]
  public async Task RegistryBackedPitMaintenanceFailsBeforeDelegationWhenRegistryOrLookupIsMissing() {
    var metadata = CreateCustomerProfileStatusMetadata();
    var registryWithoutClrMapping = DataVaultMetadataRegistry.Create(metadata.Model);
    var emptyRegistry = DataVaultMetadataRegistry.Create(new DataVaultMetadataModel([metadata.Customer], [], []));
    var service = new RecordingPitMaintenanceService();
    await using var contextWithoutRegistry = new EmptyPitModelContext(
        new DbContextOptionsBuilder<EmptyPitModelContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);
    await using var contextWithoutPit = CreateRegistryContext(emptyRegistry);
    await using var contextWithoutClrMapping = CreateRegistryContext(registryWithoutClrMapping);

    var noRegistryException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            contextWithoutRegistry,
            new DataVaultRegistryPitRebuildRequest(metadata.Pit.Name)));
    var missingNameException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            contextWithoutPit,
            new DataVaultRegistryPitRebuildRequest("MissingPit")));
    var missingClrException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            contextWithoutClrMapping,
            new DataVaultRegistryPitRebuildRequest(typeof(CustomerProfileStatusPitMapping))));

    Assert.Contains("UseDataVaultMetadata()", noRegistryException.Message, StringComparison.Ordinal);
    Assert.Contains("PIT metadata 'MissingPit'", missingNameException.Message, StringComparison.Ordinal);
    Assert.Contains("PIT metadata mapped to CLR type", missingClrException.Message, StringComparison.Ordinal);
    Assert.Empty(service.RebuildRequests);
  }

  [Fact]
  public async Task RegistryBackedPitParentMaintenanceDelegatesSupportedLinkParentPitBeforeEmptyNoOp() {
    var linkParentPit = CreateCustomerOrderStatePit();
    var registry = DataVaultMetadataRegistry.Create(linkParentPit.Model);
    var service = new RecordingPitMaintenanceService();
    await using var context = CreateRegistryContext(registry);

    var result = await service.MaintainParentsAsync(
        context,
        new DataVaultRegistryPitParentMaintenanceRequest(linkParentPit.Pit.Name, []));

    var request = Assert.Single(service.ParentRequests);
    Assert.Same(linkParentPit.Pit, request.Pit);
    Assert.Empty(request.ParentHashKeys);
    Assert.Equal("CustomerOrderState", result.Pit.Name);
    Assert.True(result.IsNoOp);
  }

  [Fact]
  public async Task PitMaintenanceAcceptsLinkParentPitShapeAndValidatesGeneratedEntityBeforeQuery() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new EmptyPitModelContext(
        new DbContextOptionsBuilder<EmptyPitModelContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);
    var linkParentPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"]);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            context,
            new DataVaultPitRebuildRequest(linkParentPit)));

    Assert.Contains("PIT metadata 'CustomerOrderState'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("generated PIT table/entity 'PitCustomerOrderState'", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task PitMaintenanceRejectsMissingMultiActivePitGeneratedEntityBeforeQuery() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new EmptyPitModelContext(
        new DbContextOptionsBuilder<EmptyPitModelContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);
    var multiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Profile", isMultiActive: true)]);
    var multiActiveException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            context,
            new DataVaultPitRebuildRequest(multiActivePit)));

    Assert.Contains("PIT metadata 'CustomerProfile'", multiActiveException.Message, StringComparison.Ordinal);
    Assert.Contains("generated PIT table/entity 'PitCustomerProfile'", multiActiveException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task PitMaintenanceRejectsContradictingMultiActiveReferenceBeforeQuery() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new ContradictingMultiActiveReferenceContext(
        new DbContextOptionsBuilder<ContradictingMultiActiveReferenceContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);
    var multiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Profile", isMultiActive: true)]);

    var multiActiveException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            context,
            new DataVaultPitRebuildRequest(multiActivePit)));

    Assert.Contains("PIT metadata 'CustomerProfile'", multiActiveException.Message, StringComparison.Ordinal);
    Assert.Contains("declares IsMultiActive=True", multiActiveException.Message, StringComparison.Ordinal);
    Assert.Contains("no driving keys", multiActiveException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task PitMaintenanceFailsBeforeWriteWhenGeneratedSnapshotReferencePropertyIsMissing() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new MissingPitSnapshotPropertyContext(
        new DbContextOptionsBuilder<MissingPitSnapshotPropertyContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            context,
            new DataVaultPitRebuildRequest(CreateCustomerProfilePit())));

    Assert.Contains("PIT metadata 'CustomerProfile'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("satellite snapshot reference property", exception.Message, StringComparison.Ordinal);
    Assert.Contains("metadata name 'Profile'", exception.Message, StringComparison.Ordinal);
  }

  private static DataVaultPitMetadata CreateCustomerProfilePit() {
    return new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
  }

  private static PitMaintenanceMetadata CreateCustomerProfileStatusMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new PitMaintenanceMetadata(customer, pit, model);
  }

  private static PitMaintenanceMetadata CreateCustomerOrderStatePit() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var state = new DataVaultSatelliteMetadata(
        "State",
        customerOrder.ToReference(),
        ["State Code"]);
    var pit = new DataVaultPitMetadata(customerOrder.ToReference(), ["State"]);
    var model = new DataVaultMetadataModel([customer, order], [customerOrder], [state], [pit]);

    return new PitMaintenanceMetadata(customer, pit, model);
  }

  private static DataVaultMetadataRegistry CreateCustomerProfileStatusRegistry(PitMaintenanceMetadata metadata) {
    return DataVaultMetadataRegistry.Create(
        metadata.Model,
        [],
        [DataVaultMetadataClrMapping.Pit<CustomerProfileStatusPitMapping>(metadata.Pit.Name)]);
  }

  private static EmptyPitModelContext CreateRegistryContext(DataVaultMetadataRegistry registry) {
    var optionsBuilder = new DbContextOptionsBuilder<EmptyPitModelContext>();
    optionsBuilder
        .UseSqlite("Data Source=:memory:")
        .UseDataVaultMetadata(registry);

    return new EmptyPitModelContext(optionsBuilder.Options);
  }

  private sealed class EmptyPitModelContext(DbContextOptions<EmptyPitModelContext> options) : DbContext(options) {
  }

  private sealed class RecordingPitMaintenanceService : IDataVaultPitMaintenanceService {
    public List<DataVaultPitMetadata> RebuildRequests { get; } = [];

    public List<ParentMaintenanceCall> ParentRequests { get; } = [];

    public Task<DataVaultPitMaintenanceResult> RebuildAsync(
        DbContext dbContext,
        DataVaultPitRebuildRequest request,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(request);

      RebuildRequests.Add(request.Pit);

      return Task.FromResult(new DataVaultPitMaintenanceResult(
          request.Pit,
          "Pit" + request.Pit.Name,
          parentHashKeyCount: 0,
          rowsDeleted: 0,
          rowsWritten: 0));
    }

    public Task<DataVaultPitMaintenanceResult> MaintainParentsAsync(
        DbContext dbContext,
        DataVaultPitParentMaintenanceRequest request,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(request);

      ParentRequests.Add(new ParentMaintenanceCall(request.Pit, request.ParentHashKeys));

      return Task.FromResult(new DataVaultPitMaintenanceResult(
          request.Pit,
          "Pit" + request.Pit.Name,
          request.ParentHashKeys.Count,
          rowsDeleted: 0,
          rowsWritten: 0));
    }
  }

  private sealed class CustomerProfileStatusPitMapping {
  }

  private sealed class MissingPitSnapshotPropertyContext(DbContextOptions<MissingPitSnapshotPropertyContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      ConfigurePitEntity(modelBuilder, includeSnapshotReference: false);
    }

    public static void ConfigurePitEntity(
        ModelBuilder modelBuilder,
        bool includeSnapshotReference) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("PitCustomerProfile", entityBuilder => {
        entityBuilder.ToTable("PitCustomerProfile");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Pit);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "CustomerProfile");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceKind, DataVaultMetadataReferenceKind.Hub);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceName, "Customer");

        var parentHashKey = entityBuilder.IndexerProperty<string>("CustomerHashKey");
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.HashKey);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Customer");

        var loadTimestamp = entityBuilder.IndexerProperty<DateTimeOffset>("LoadTimestamp");
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.LoadTimestamp);
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "LoadTimestamp");

        if (includeSnapshotReference) {
          var profileSnapshot = entityBuilder.IndexerProperty<DateTimeOffset?>("ProfileLoadTimestamp");
          profileSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.SnapshotReference);
          profileSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.LoadTimestamp);
          profileSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Profile");
        }

        entityBuilder.HasKey("CustomerHashKey", "LoadTimestamp");
      });
    }
  }

  private sealed class ContradictingMultiActiveReferenceContext(DbContextOptions<ContradictingMultiActiveReferenceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      MissingPitSnapshotPropertyContext.ConfigurePitEntity(modelBuilder, includeSnapshotReference: true);
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("SatCustomerProfile", entityBuilder => {
        entityBuilder.ToTable("SatCustomerProfile");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Satellite);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Profile");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceKind, DataVaultMetadataReferenceKind.Hub);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceName, "Customer");

        var parentHashKey = entityBuilder.IndexerProperty<string>("CustomerHashKey");
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.HashKey);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Customer");

        var loadTimestamp = entityBuilder.IndexerProperty<DateTimeOffset>("LoadTimestamp");
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.LoadTimestamp);

        entityBuilder.HasKey("CustomerHashKey", "LoadTimestamp");
      });
    }
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultHubMetadata Customer,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record ParentMaintenanceCall(
      DataVaultPitMetadata Pit,
      IReadOnlyList<string> ParentHashKeys);
}
