using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Privacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultEfMetadataTranslationTests {
  private const string EncryptedPayloadAlias = "CustomerContactEmailEncrypted";
  private const string EncryptedPayloadPlaintext = "alice@example.test";

  [Fact]
  public void ApplyDataVaultMetadataIsExplicitRootNamespaceTranslationExtension() {
    var method = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "ApplyDataVaultMetadata" &&
            methodInfo.GetParameters().Length == 2 &&
            methodInfo.GetParameters()[1].ParameterType == typeof(DataVaultMetadataModel));
    var parameters = method.GetParameters();

    Assert.Equal("DCoding.Data.DVault", method.DeclaringType?.Namespace);
    Assert.Equal(typeof(ModelBuilder), parameters[0].ParameterType);
    Assert.Equal(typeof(DataVaultMetadataModel), parameters[1].ParameterType);
    Assert.Equal(typeof(ModelBuilder), method.ReturnType);
    Assert.True(method.IsDefined(typeof(ExtensionAttribute), inherit: false));
  }

  [Fact]
  public void ApplyDataVaultMetadataCodeFirstIsExplicitRootNamespaceTranslationExtension() {
    var method = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "ApplyDataVaultMetadata" &&
            methodInfo.GetParameters().Length == 2 &&
            methodInfo.GetParameters()[1].ParameterType == typeof(Action<DataVaultCodeFirstModelBuilder>));
    var parameters = method.GetParameters();

    Assert.Equal("DCoding.Data.DVault", method.DeclaringType?.Namespace);
    Assert.Equal(typeof(ModelBuilder), parameters[0].ParameterType);
    Assert.Equal(typeof(Action<DataVaultCodeFirstModelBuilder>), parameters[1].ParameterType);
    Assert.Equal(typeof(ModelBuilder), method.ReturnType);
    Assert.True(method.IsDefined(typeof(ExtensionAttribute), inherit: false));
  }

  [Fact]
  public void ApplyDataVaultMetadataWithProviderProfileIsExplicitRootNamespaceTranslationExtension() {
    var method = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "ApplyDataVaultMetadata" &&
            methodInfo.GetParameters().Length == 3 &&
            methodInfo.GetParameters()[1].ParameterType == typeof(DataVaultMetadataModel) &&
            methodInfo.GetParameters()[2].ParameterType == typeof(DataVaultProviderCapabilityProfile));
    var parameters = method.GetParameters();

    Assert.Equal("DCoding.Data.DVault", method.DeclaringType?.Namespace);
    Assert.Equal(typeof(ModelBuilder), parameters[0].ParameterType);
    Assert.Equal(typeof(DataVaultMetadataModel), parameters[1].ParameterType);
    Assert.Equal(typeof(DataVaultProviderCapabilityProfile), parameters[2].ParameterType);
    Assert.Equal(typeof(ModelBuilder), method.ReturnType);
    Assert.True(method.IsDefined(typeof(ExtensionAttribute), inherit: false));
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsNullArguments() {
    var metadataModel = CreateMetadataModel();
    ModelBuilder? modelBuilder = null;

    var modelBuilderException = Assert.Throws<ArgumentNullException>(() => modelBuilder!.ApplyDataVaultMetadata(metadataModel));
    var metadataException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().ApplyDataVaultMetadata((DataVaultMetadataModel)null!));
    var configureModelException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().ApplyDataVaultMetadata((Action<DataVaultCodeFirstModelBuilder>)null!));
    var profileException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().ApplyDataVaultMetadata(metadataModel, null!));

    Assert.Equal("modelBuilder", modelBuilderException.ParamName);
    Assert.Equal("metadataModel", metadataException.ParamName);
    Assert.Equal("configureModel", configureModelException.ParamName);
    Assert.Equal("providerCapabilities", profileException.ParamName);
  }

  [Fact]
  public void ApplyDataVaultMetadataCreatesProviderNeutralHubLinkAndSatelliteMetadata() {
    var model = CreateTranslatedModel();

    Assert.Equal(3, model.GetEntityTypes().Count());

    AssertHub(FindEntity(model, "HubCustomer"));
    AssertLink(FindEntity(model, "LinkCustomerOrder"));
    AssertSatellite(FindEntity(model, "SatCustomerContact"));
  }

  [Fact]
  public void ApplyDataVaultMetadataCreatesProviderNeutralBridgeMetadata() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateBridgeMetadataModel());

    var model = modelBuilder.Model;
    var orderHub = FindEntity(model, "HubOrder");
    var manyToManyBridge = FindEntity(model, "BridgeCustomerOrder");
    var hierarchyBridge = FindEntity(model, "BridgeSalesRegionHierarchy");

    Assert.Equal(6, model.GetEntityTypes().Count());
    AssertHub(FindEntity(model, "HubCustomer"));
    AssertOrderHub(orderHub);
    AssertLink(FindEntity(model, "LinkCustomerOrder"));
    AssertSatellite(FindEntity(model, "SatCustomerContact"));
    AssertManyToManyBridge(manyToManyBridge);
    AssertHierarchyBridge(hierarchyBridge);
  }

  [Fact]
  public void ApplyDataVaultMetadataMapsProducedNamesToRelationalMetadata() {
    var model = CreateTranslatedModel();

    AssertRelationalEntity(
        FindEntity(model, "HubCustomer"),
        "HubCustomer",
        ["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId"],
        "PkHubCustomerCustomerHashKey",
        "IxHubCustomerBusinessKeyCustomerId");
    AssertRelationalEntity(
        FindEntity(model, "LinkCustomerOrder"),
        "LinkCustomerOrder",
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        "PkLinkCustomerOrderCustomerOrderHashKey",
        "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey");
    AssertRelationalEntity(
        FindEntity(model, "SatCustomerContact"),
        "SatCustomerContact",
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        "PkSatCustomerContactCustomerHashKeyLoadTimestamp",
        "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp");
  }

  [Fact]
  public void ApplyDataVaultMetadataMapsBridgeProducedNamesToRelationalMetadata() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateBridgeMetadataModel());

    AssertRelationalEntityWithIndexes(
        FindEntity(modelBuilder.Model, "BridgeCustomerOrder"),
        "BridgeCustomerOrder",
        ["CustomerHashKey", "OrderHashKey"],
        "PkBridgeCustomerOrderCustomerHashKeyOrderHashKey",
        [
            (
                "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
                ["OrderHashKey", "CustomerHashKey"]),
        ]);
    AssertRelationalEntityWithIndexes(
        FindEntity(modelBuilder.Model, "BridgeSalesRegionHierarchy"),
        "BridgeSalesRegionHierarchy",
        ["AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey", "TraversalDepth"],
        "PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey",
        [
            (
                "IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth",
                ["AncestorSalesRegionHashKey", "TraversalDepth"]),
            (
                "IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey",
                ["DescendantSalesRegionHashKey", "AncestorSalesRegionHashKey"]),
        ]);
  }

  [Fact]
  public void ApplyDataVaultMetadataHonorsRelationalDefaultSchema() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.HasDefaultSchema("dvault_test_schema");
    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());

    Assert.All(
        modelBuilder.Model.GetEntityTypes(),
        entityType => Assert.Equal("dvault_test_schema", entityType.GetSchema()));
  }

  [Fact]
  public void ApplyDataVaultMetadataKeepsEquivalentInputDeterministic() {
    var first = CreateTranslatedModel();
    var second = CreateTranslatedModel();

    Assert.Equal(ModelShape(first), ModelShape(second));
  }

  [Fact]
  public void ApplyDataVaultMetadataPreservesDeclaredBusinessKeyOrder() {
    var modelBuilder = CreateModelBuilder();
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id", "Source System"])],
        [],
        []);

    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    var hub = FindEntity(modelBuilder.Model, "HubCustomer");

    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId", "SourceSystem"],
        PropertyNamesInOrdinalOrder(hub));
    AssertPrimaryKey(hub, "PkHubCustomerCustomerHashKey", ["CustomerHashKey"]);
    AssertIndex(hub, "IxHubCustomerBusinessKeyCustomerIdSourceSystem", ["CustomerId", "SourceSystem"], isUnique: true);
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsMultiActiveSatelliteDrivingKeysInCanonicalOrder() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type", "Region Code"]);
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(new DataVaultMetadataModel([customer], [], [contact]));

    var satellite = FindEntity(modelBuilder.Model, "SatCustomerContact");

    Assert.Equal(
        ["CustomerHashKey", "ContactType", "RegionCode", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        PropertyNamesInOrdinalOrder(satellite));
    AssertProperty(satellite, "ContactType", DataVaultPropertyRole.DrivingKey, expectedTechnicalRole: null);
    AssertProperty(satellite, "RegionCode", DataVaultPropertyRole.DrivingKey, expectedTechnicalRole: null);
    AssertPrimaryKey(
        satellite,
        "PkSatCustomerContactCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp"]);
    AssertIndex(
        satellite,
        "IxSatCustomerContactSatelliteParentCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp", "HashDiff"],
        isUnique: false);
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsSatelliteEffectivityRoleAnnotations() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code", "Effective From", "Effective To", "Is Current"],
        [],
        [],
        new DataVaultSatelliteEffectivityMetadata("Effective From", "Effective To", "Is Current"));
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(new DataVaultMetadataModel([customer], [], [status]));

    var satellite = FindEntity(modelBuilder.Model, "SatCustomerStatu");

    Assert.Equal(
        DataVaultEffectivityRole.EffectiveFrom,
        AnnotationValue<DataVaultEffectivityRole>(satellite.FindProperty("EffectiveFrom")!, DataVaultAnnotationNames.EffectivityRole));
    Assert.Equal(
        DataVaultEffectivityRole.EffectiveTo,
        AnnotationValue<DataVaultEffectivityRole>(satellite.FindProperty("EffectiveTo")!, DataVaultAnnotationNames.EffectivityRole));
    Assert.Equal(
        DataVaultEffectivityRole.CurrentFlag,
        AnnotationValue<DataVaultEffectivityRole>(satellite.FindProperty("IsCurrent")!, DataVaultAnnotationNames.EffectivityRole));
    Assert.Null(satellite.FindProperty("StatusCode")!.FindAnnotation(DataVaultAnnotationNames.EffectivityRole));
  }

  [Fact]
  public void ApplyDataVaultMetadataCreatesProviderNeutralPitMetadata() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreatePitMetadataModel());

    Assert.Equal(4, modelBuilder.Model.GetEntityTypes().Count());
    AssertPit(FindEntity(modelBuilder.Model, "PitCustomerProfileStatus"));
  }

  [Fact]
  public void ApplyDataVaultMetadataCreatesProviderNeutralLinkParentPitMetadata() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateLinkParentPitMetadataModel());

    Assert.Equal(5, modelBuilder.Model.GetEntityTypes().Count());
    AssertLinkParentPit(FindEntity(modelBuilder.Model, "PitCustomerOrderState"));
  }

  [Fact]
  public void ApplyDataVaultMetadataMapsPitProducedNamesToRelationalMetadata() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreatePitMetadataModel());

    AssertRelationalPitEntity(
        FindEntity(modelBuilder.Model, "PitCustomerProfileStatus"),
        "PitCustomerProfileStatus",
        ["CustomerHashKey", "LoadTimestamp", "ProfileLoadTimestamp", "StatusLoadTimestamp"],
        "PkPitCustomerProfileStatusCustomerHashKeyLoadTimestamp",
        "IxPitCustomerProfileStatusTraversalCustomerHashKeyLoadTimestamp");
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsMultiActivePitDrivingKeysInCanonicalOrder() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type", "Region Code"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(
        customer.ToReference(),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Status"),
        ]);
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(new DataVaultMetadataModel([customer], [], [contact, status], [pit]));

    var pitEntity = FindEntity(modelBuilder.Model, "PitCustomerContactStatus");

    Assert.Equal(
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp", "ContactLoadTimestamp", "StatusLoadTimestamp"],
        PropertyNamesInOrdinalOrder(pitEntity));
    AssertProperty(pitEntity, "ContactType", DataVaultPropertyRole.DrivingKey, expectedTechnicalRole: null);
    AssertProperty(pitEntity, "RegionCode", DataVaultPropertyRole.DrivingKey, expectedTechnicalRole: null);
    AssertPrimaryKey(
        pitEntity,
        "PkPitCustomerContactStatusCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp"]);
    AssertIndex(
        pitEntity,
        "IxPitCustomerContactStatusTraversalCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp"],
        isUnique: false);
  }

  [Fact]
  public void ApplyDataVaultMetadataPreservesPitSatelliteDeclarationOrder() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreatePitMetadataModel(["Status", "Profile"]));

    var pit = FindEntity(modelBuilder.Model, "PitCustomerStatusProfile");

    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "StatusLoadTimestamp", "ProfileLoadTimestamp"],
        PropertyNamesInOrdinalOrder(pit));
    AssertPrimaryKey(pit, "PkPitCustomerStatusProfileCustomerHashKeyLoadTimestamp", ["CustomerHashKey", "LoadTimestamp"]);
  }

  [Fact]
  public void ApplyDataVaultMetadataKeepsPitOutputDeterministic() {
    var firstModelBuilder = CreateModelBuilder();
    var secondModelBuilder = CreateModelBuilder();

    firstModelBuilder.ApplyDataVaultMetadata(CreatePitMetadataModel());
    secondModelBuilder.ApplyDataVaultMetadata(CreatePitMetadataModel());

    Assert.Equal(ModelShape(firstModelBuilder.Model), ModelShape(secondModelBuilder.Model));
  }

  [Fact]
  public void ApplyDataVaultMetadataWithOracleProfileProjectsOracleStorageAnnotations() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), DataVaultProviderCapabilityProfiles.Oracle);

    Assert.Equal(
        "oracle-v1",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.ProviderProfile)?.Value));

    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    var link = FindEntity(modelBuilder.Model, "LinkCustomerOrder");
    var satellite = FindEntity(modelBuilder.Model, "SatCustomerContact");

    AssertProviderProperty(
        hub,
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey,
        typeof(string),
        "RAW(32)",
        DataVaultProviderValueFormat.LowercaseHexBinary);
    AssertProviderProperty(
        hub,
        "LoadTimestamp",
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(string),
        "VARCHAR2(33 CHAR)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertProviderProperty(
        hub,
        "RecordSource",
        DataVaultLogicalPropertyKind.RecordSource,
        typeof(string),
        "VARCHAR2(255 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        hub,
        "CustomerId",
        DataVaultLogicalPropertyKind.BusinessKey,
        typeof(string),
        "VARCHAR2(255 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        link,
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        typeof(string),
        "RAW(32)",
        DataVaultProviderValueFormat.LowercaseHexBinary);
    AssertProviderProperty(
        satellite,
        "HashDiff",
        DataVaultLogicalPropertyKind.HashDiff,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        satellite,
        "EmailAddress",
        DataVaultLogicalPropertyKind.PayloadText,
        typeof(string),
        "CLOB",
        DataVaultProviderValueFormat.Text);
    Assert.DoesNotContain(
        satellite.GetIndexes(),
        index => index.Properties.Select(property => property.Name)
            .SequenceEqual(["CustomerHashKey", "LoadTimestamp"], StringComparer.Ordinal));
  }

  [Fact]
  public void ApplyDataVaultMetadataPreservesExplicitHashKeyProfileSizing() {
    var modelBuilder = CreateModelBuilder();
    var profile = DataVaultProviderCapabilityProfiles.SqlServer.WithStableHashAlgorithm("sha1-v1", 20);

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), profile);

    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    var hashKey = hub.FindProperty("CustomerHashKey");

    Assert.NotNull(hashKey);
    Assert.Equal("varbinary(20)", hashKey!.GetColumnType());
    Assert.Equal("varbinary(20)", AnnotationValue<string>(hashKey, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal("sha1-v1", AnnotationValue<string>(hashKey, DataVaultAnnotationNames.StableHashAlgorithmId));
    Assert.Equal(20, AnnotationValue<int>(hashKey, DataVaultAnnotationNames.StableHashDigestByteLength));
    Assert.Equal(
        DataVaultHashKeyStorageProfile.Binary,
        AnnotationValue<DataVaultHashKeyStorageProfile>(hashKey, DataVaultAnnotationNames.HashKeyStorageProfile));
  }

  [Fact]
  public void UseDataVaultNoOptionOverloadRecordsBinaryHashDefaults() {
    var modelBuilder = CreateModelBuilder();

    var result = modelBuilder.UseDataVault();

    Assert.Same(modelBuilder, result);
    Assert.Equal(
        "sqlite-v1",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.ProviderProfile)?.Value));

    var conventions = Assert.IsType<DataVaultConventions>(
        modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.Conventions)?.Value);

    Assert.Same(DataVaultConventions.Default, conventions);
    AssertBinaryHashDefaults(conventions);
  }

  [Fact]
  public void ApplyDataVaultMetadataDefaultPathProjectsBinaryHashMappings() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateBridgeMetadataModel());

    AssertSqliteBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "HubCustomer"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey);
    AssertSqliteBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "LinkCustomerOrder"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference);
    AssertSqliteBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "BridgeCustomerOrder"),
        "OrderHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference);
  }

  [Fact]
  public void ApplyDataVaultMetadataHexStringProfileProjectsCompatibilityMappings() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(
        CreateBridgeMetadataModel(),
        DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
            DataVaultHashKeyStorageProfile.HexString,
            "sha256-v1",
            32));

    AssertHexStringHashKeyCompatibilityProperty(
        FindEntity(modelBuilder.Model, "HubCustomer"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey);
    AssertHexStringHashKeyCompatibilityProperty(
        FindEntity(modelBuilder.Model, "LinkCustomerOrder"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference);
    AssertHexStringHashKeyCompatibilityProperty(
        FindEntity(modelBuilder.Model, "BridgeCustomerOrder"),
        "OrderHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference);
  }

  [Fact]
  public void ApplyDataVaultMetadataBinaryHashKeyProfileAppliesProviderNeutralConversionToKeysAndReferences() {
    var modelBuilder = CreateModelBuilder();
    var profile = DataVaultProviderCapabilityProfiles.SqlServer.WithHashKeyStorageProfile(
        DataVaultHashKeyStorageProfile.Binary,
        "sha256-128-v1",
        16);

    modelBuilder.ApplyDataVaultMetadata(CreateBridgeMetadataModel(), profile);

    AssertBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "HubCustomer"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey);
    AssertBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "LinkCustomerOrder"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference);
    AssertBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "BridgeCustomerOrder"),
        "OrderHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference);
  }

  [Fact]
  public void ApplyDataVaultMetadataBinaryFirstConventionsProfileProjectsBinaryKeysAndReferences() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.UseDataVaultBinaryFirstProfile();
    modelBuilder.ApplyDataVaultMetadata(CreateBridgeMetadataModel(), DataVaultProviderCapabilityProfiles.SqlServer);

    var conventions = Assert.IsType<DataVaultConventions>(
        modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.Conventions)?.Value);

    Assert.Equal("binary-first", conventions.ProfileName);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    AssertBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "HubCustomer"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey,
        "varbinary(32)",
        "sha256-v1",
        32);
    AssertBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "LinkCustomerOrder"),
        "OrderHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        "varbinary(32)",
        "sha256-v1",
        32);
    AssertBinaryHashKeyProperty(
        FindEntity(modelBuilder.Model, "BridgeCustomerOrder"),
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        "varbinary(32)",
        "sha256-v1",
        32);
  }

  [Theory]
  [InlineData("sha256-v1", 32)]
  [InlineData("sha1-v1", 20)]
  [InlineData("sha256-128-v1", 16)]
  [InlineData("sha256-160-v1", 20)]
  public void BinaryHashKeyConversionRoundTripsBuiltInDigestSizesAndNulls(
      string algorithmId,
      int digestByteLength) {
    var converter = GetBinaryHashKeyConverter(algorithmId, digestByteLength);
    var canonicalHash = CreateCanonicalHexDigest(digestByteLength);

    var providerValue = Assert.IsType<byte[]>(converter.ConvertToProvider(canonicalHash));
    var roundTrippedHash = Assert.IsType<string>(converter.ConvertFromProvider(providerValue));

    Assert.Equal(Enumerable.Range(0, digestByteLength).Select(value => (byte)value).ToArray(), providerValue);
    Assert.Equal(canonicalHash, roundTrippedHash);
    Assert.Null(converter.ConvertToProvider(null));
    Assert.Null(converter.ConvertFromProvider(null));
  }

  [Fact]
  public void BinaryHashKeyConversionKeepsStringModelChangeTrackingStable() {
    using var context = CreateBinaryHashKeyChangeTrackingContext();
    var link = CreateLinkEntity(
        CreateCanonicalHexDigest(16),
        CreateCanonicalHexDigest(16, seed: 16),
        CreateCanonicalHexDigest(16, seed: 32));
    var linkSet = context.Set<Dictionary<string, object>>("LinkCustomerOrder");

    linkSet.Attach(link);

    var entry = context.Entry(link);
    var participantReference = entry.Property("CustomerHashKey");
    var participantReferenceMetadata = participantReference.Metadata;
    var linkHashKeyMetadata = entry.Metadata.FindPrimaryKey()!.Properties.Single(property => property.Name == "CustomerOrderHashKey");
    var keyValueComparer = linkHashKeyMetadata.GetKeyValueComparer();
    var valueComparer = participantReferenceMetadata.GetValueComparer();
    var originalCustomerHashKey = Assert.IsType<string>(participantReference.OriginalValue);

    Assert.Equal(EntityState.Unchanged, entry.State);
    Assert.True(keyValueComparer.Equals(link["CustomerOrderHashKey"], new string(Assert.IsType<string>(link["CustomerOrderHashKey"]).ToCharArray())));
    Assert.True(keyValueComparer.Equals(null, null));
    Assert.Equal(
        link["CustomerOrderHashKey"],
        Assert.IsType<string>(keyValueComparer.Snapshot(link["CustomerOrderHashKey"])));
    Assert.NotNull(valueComparer);
    Assert.True(valueComparer!.Equals(originalCustomerHashKey, new string(originalCustomerHashKey.ToCharArray())));
    Assert.True(valueComparer.Equals(null, null));
    Assert.Equal(originalCustomerHashKey, Assert.IsType<string>(valueComparer.Snapshot(originalCustomerHashKey)));

    link["CustomerHashKey"] = new string(originalCustomerHashKey.ToCharArray());
    context.ChangeTracker.DetectChanges();

    Assert.Equal(EntityState.Unchanged, entry.State);
    Assert.False(participantReference.IsModified);

    link["CustomerHashKey"] = CreateCanonicalHexDigest(16, seed: 48);
    context.ChangeTracker.DetectChanges();

    Assert.Equal(EntityState.Modified, entry.State);
    Assert.True(participantReference.IsModified);

    using var nullContext = CreateBinaryHashKeyChangeTrackingContext();
    var nullReferenceLink = CreateLinkEntity(
        CreateCanonicalHexDigest(16, seed: 64),
        null,
        CreateCanonicalHexDigest(16, seed: 80));
    var nullReferenceSet = nullContext.Set<Dictionary<string, object>>("LinkCustomerOrder");

    nullReferenceSet.Attach(nullReferenceLink);

    var nullEntry = nullContext.Entry(nullReferenceLink);
    var nullParticipantReference = nullEntry.Property("CustomerHashKey");

    Assert.Equal(EntityState.Unchanged, nullEntry.State);
    Assert.Null(nullParticipantReference.OriginalValue);

    nullReferenceLink["CustomerHashKey"] = null!;
    nullContext.ChangeTracker.DetectChanges();

    Assert.Equal(EntityState.Unchanged, nullEntry.State);
    Assert.False(nullParticipantReference.IsModified);
  }

  [Theory]
  [InlineData("000102030405060708090a0b0c0d0e", "32 lowercase hexadecimal characters")]
  [InlineData("000102030405060708090a0b0c0d0e0f00", "32 lowercase hexadecimal characters")]
  [InlineData("000102030405060708090A0B0C0D0E0F", "canonical lowercase hexadecimal values")]
  [InlineData("000102030405060708090a0b0c0d0e0g", "canonical lowercase hexadecimal values")]
  public void BinaryHashKeyConversionRejectsInvalidModelValues(
      string value,
      string expectedMessage) {
    var converter = GetBinaryHashKeyConverter();

    var exception = Assert.Throws<FormatException>(() => converter.ConvertToProvider(value));

    Assert.Contains(expectedMessage, exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void BinaryHashKeyConversionRejectsProviderBytesWithWrongDigestLength() {
    var converter = GetBinaryHashKeyConverter();

    var exception = Assert.Throws<FormatException>(() => converter.ConvertFromProvider(new byte[15]));

    Assert.Contains("16 provider bytes", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataWithUtcTicksStorageProjectsIntegerTimestampAnnotations() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(
        CreateMetadataModel(),
        DataVaultProviderCapabilityProfiles.Sqlite,
        DataVaultLoadTimestampStorage.UtcTicks);

    Assert.Equal(
        "sqlite-v1-loadts-utc-ticks",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.ProviderProfile)?.Value));

    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    var satellite = FindEntity(modelBuilder.Model, "SatCustomerContact");

    AssertProviderProperty(
        hub,
        "LoadTimestamp",
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(long),
        "INTEGER",
        DataVaultProviderValueFormat.UtcTicks,
        "sqlite-v1-loadts-utc-ticks");
    AssertProviderProperty(
        satellite,
        "LoadTimestamp",
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(long),
        "INTEGER",
        DataVaultProviderValueFormat.UtcTicks,
        "sqlite-v1-loadts-utc-ticks");
  }

  [Fact]
  public void ApplyDataVaultMetadataWithOracleProfileProjectsPitSnapshotStorageAnnotations() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreatePitMetadataModel(), DataVaultProviderCapabilityProfiles.Oracle);

    var pit = FindEntity(modelBuilder.Model, "PitCustomerProfileStatus");

    AssertProviderProperty(
        pit,
        "ProfileLoadTimestamp",
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(string),
        "VARCHAR2(33 CHAR)",
        DataVaultProviderValueFormat.Iso8601UtcText);
  }

  [Fact]
  public void ApplyDataVaultMetadataWithMySqlProfileDoesNotPromoteSatelliteHashDiffIncludeToIndexKey() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), DataVaultProviderCapabilityProfiles.MySql);

    var satellite = FindEntity(modelBuilder.Model, "SatCustomerContact");

    AssertIndex(
        satellite,
        "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp"],
        isUnique: false);
  }

  [Theory]
  [InlineData("sqlite-v1", "TEXT")]
  [InlineData("postgres-v1", "text")]
  [InlineData("sqlserver-v1", "nvarchar(max)")]
  [InlineData("oracle-v1", "CLOB")]
  [InlineData("db2-v1", "CLOB")]
  [InlineData("mysql-pomelo-v1", "longtext")]
  public void EncryptedPayloadValueConverterBackedPayloadUsesOrdinaryPayloadTextStorageForBuiltInProviderUnitMatrix(
      string profileName,
      string expectedStorageType) {
    var payload = CreateEncryptedPayloadTranslatedPayloadProperty(SelectBuiltInProviderCapabilityProfile(profileName));

    AssertEncryptedPayloadProviderProperty(payload, profileName, expectedStorageType);

    var converter = Assert.IsType<DataVaultEncryptedPayloadValueConverter>(payload.GetValueConverter());
    var providerValue = Assert.IsType<string>(converter.ConvertToProvider(EncryptedPayloadPlaintext));

    Assert.StartsWith("encrypted:" + EncryptedPayloadAlias + ":", providerValue, StringComparison.Ordinal);
    Assert.DoesNotContain(EncryptedPayloadPlaintext, providerValue, StringComparison.Ordinal);
  }

  [Fact]
  public void EncryptedPayloadValueConverterBackedMySqlProviderNamesShareSinglePomeloProfileMapping() {
    Assert.Equal(
        DataVaultProviderCapabilityProfiles.MySql,
        DataVaultProviderCapabilityProfileSelection.Select("MySql.EntityFrameworkCore"));
    Assert.Equal(
        DataVaultProviderCapabilityProfiles.MySql,
        DataVaultProviderCapabilityProfileSelection.Select("Pomelo.EntityFrameworkCore.MySql"));

    var payload = CreateEncryptedPayloadTranslatedPayloadProperty(DataVaultProviderCapabilityProfiles.MySql);

    AssertEncryptedPayloadProviderProperty(payload, "mysql-pomelo-v1", "longtext");
  }

  [Fact]
  public void EncryptedPayloadValueConverterBackedMappingFailsDeterministicallyWhenProfileOmitsPayloadTextCapability() {
    var providerCapabilities = CreateProfileWithoutPayloadTextMapping();

    var exception = Assert.Throws<NotSupportedException>(() =>
        CreateModelBuilder().ApplyDataVaultMetadata(CreateMetadataModel(), providerCapabilities));

    Assert.Contains("broken-encrypted-payload-profile", exception.Message, StringComparison.Ordinal);
    Assert.Contains("type mapping for PayloadText", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataTranslatesLinkParentSatellites() {
    var modelBuilder = CreateModelBuilder();
    var metadataModel = new DataVaultMetadataModel(
        [],
        [],
        [
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ]);

    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    var satellite = FindEntity(modelBuilder.Model, "SatCustomerOrderState");

    Assert.Equal(DataVaultTableKind.Satellite, AnnotationValue<DataVaultTableKind>(satellite, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("State", AnnotationValue<string>(satellite, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        DataVaultMetadataReferenceKind.Link,
        AnnotationValue<DataVaultMetadataReferenceKind>(satellite, DataVaultAnnotationNames.ParentReferenceKind));
    Assert.Equal("CustomerOrder", AnnotationValue<string>(satellite, DataVaultAnnotationNames.ParentReferenceName));
    Assert.Equal(
        ["CustomerOrderHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "StateCode"],
        PropertyNamesInOrdinalOrder(satellite));
    AssertPrimaryKey(satellite, "PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp", ["CustomerOrderHashKey", "LoadTimestamp"]);
    AssertIndex(
        satellite,
        "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp",
        ["CustomerOrderHashKey", "LoadTimestamp", "HashDiff"],
        isUnique: false);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsPitWithEmptySatelliteSetWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        [],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), Array.Empty<string>())]);

    AssertPitTranslationFailure(metadataModel, "at least one attached satellite");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsPitWithDuplicateSatelliteReferencesWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Profile",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
        ],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), new[] { "Profile", "Profile" })]);

    AssertPitTranslationFailure(metadataModel, "duplicate satellite reference 'Profile'");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsPitWithSatelliteMissingFromModelWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        [],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"])]);

    AssertPitTranslationFailure(metadataModel, "satellite 'Profile' that is not declared");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsPitWithMissingHubWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [],
        [],
        [],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"])]);

    AssertPitTranslationFailure(metadataModel, "hub 'Customer' that is not declared");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsPitWithSatelliteAttachedToAnotherHubWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Profile",
                DataVaultMetadataReference.Hub("Order"),
                ["Status Code"]),
        ],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"])]);

    AssertPitTranslationFailure(metadataModel, "attached to Hub 'Order' instead of declared Hub 'Customer'");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsPitWithMissingLinkWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [],
        [],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"])]);

    AssertPitTranslationFailure(metadataModel, "link 'CustomerOrder' that is not declared");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsLinkParentPitSatelliteWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["State"])]);

    AssertPitTranslationFailure(metadataModel, "attached to Link 'CustomerOrder'");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsMultiActivePitSatelliteWithoutPartialMapping() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Profile",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
        ],
        [
            new DataVaultPitMetadata(
                DataVaultMetadataReference.Hub("Customer"),
                [new DataVaultPitSatelliteReferenceMetadata("Profile", isMultiActive: true)]),
        ]);

    AssertPitTranslationFailure(metadataModel, "declares IsMultiActive=True");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsMultiActivePitSatellitesWithIncompatibleDrivingKeys() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var preference = new DataVaultSatelliteMetadata(
        "Preference",
        customer.ToReference(),
        ["Preference Value"],
        ["Preference Type"]);
    var metadataModel = new DataVaultMetadataModel(
        [customer],
        [],
        [contact, preference],
        [
            new DataVaultPitMetadata(
                customer.ToReference(),
                [
                    new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
                    new DataVaultPitSatelliteReferenceMetadata("Preference", isMultiActive: true),
                ]),
        ]);

    AssertPitTranslationFailure(metadataModel, "do not match multi-active satellite 'Contact' driving-key names");
  }

  [Fact]
  public void ApplyDataVaultMetadataFailsDeterministicallyWhenProviderProfileOmitsRequiredMapping() {
    var providerCapabilities = new DataVaultProviderCapabilityProfile(
        "broken-test-profile",
        DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
        DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
        [
            new(
                DataVaultLogicalPropertyKind.HashKey,
                typeof(string),
                "TEXT",
                DataVaultProviderValueFormat.Text),
        ]);

    var exception = Assert.Throws<TargetInvocationException>(() =>
        InvokeTranslatorApply(CreateModelBuilder(), CreateMetadataModel(), providerCapabilities));
    var notSupportedException = Assert.IsType<NotSupportedException>(exception.InnerException);

    Assert.Contains("broken-test-profile", notSupportedException.Message, StringComparison.Ordinal);
    Assert.Contains("type mapping for LoadTimestamp", notSupportedException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsProviderSafePhysicalIdentifiers() {
    var hub = new DataVaultHubMetadata(
        "CustomerAccountWithExtremelyVerboseProviderIdentifierPreflightProjectionName",
        ["Customer Business Identifier With Extremely Verbose Provider Identifier Preflight Column Name"]);
    var metadataModel = new DataVaultMetadataModel([hub], [], []);
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(metadataModel, DataVaultProviderCapabilityProfiles.MySql);

    var entity = Assert.Single(modelBuilder.Model.GetEntityTypes());
    var producedTableName = AnnotationValue<string>(entity, DataVaultAnnotationNames.ProducedName);
    var physicalTableName = entity.GetTableName();
    var table = StoreObjectIdentifier.Table(physicalTableName!, entity.GetSchema());
    var businessKeyProperty = entity.GetProperties().Single(property =>
        string.Equals(
            AnnotationValue<string>(property, DataVaultAnnotationNames.MetadataName),
            hub.BusinessKeyColumns[0].ColumnName,
            StringComparison.Ordinal));
    var producedColumnName = AnnotationValue<string>(businessKeyProperty, DataVaultAnnotationNames.ProducedName);
    var physicalColumnName = businessKeyProperty.GetColumnName(table);
    var primaryKey = entity.FindPrimaryKey()!;
    var producedPrimaryKeyName = AnnotationValue<string>(primaryKey, DataVaultAnnotationNames.ProducedName);
    var physicalPrimaryKeyName = primaryKey.GetName();
    var index = Assert.Single(entity.GetIndexes());
    var producedIndexName = AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName);
    var physicalIndexName = index.GetDatabaseName();

    Assert.NotEqual(producedTableName, physicalTableName);
    Assert.NotEqual(producedColumnName, physicalColumnName);
    Assert.NotEqual(producedPrimaryKeyName, physicalPrimaryKeyName);
    Assert.NotEqual(producedIndexName, physicalIndexName);
    Assert.True(physicalTableName!.Length <= 64);
    Assert.True(physicalColumnName!.Length <= 64);
    Assert.True(physicalPrimaryKeyName!.Length <= 64);
    Assert.True(physicalIndexName!.Length <= 64);
    Assert.Equal(producedColumnName, businessKeyProperty.Name);
    Assert.Contains('_', physicalTableName);
    Assert.Contains('_', physicalColumnName);
    Assert.Contains('_', physicalPrimaryKeyName);
    Assert.Contains('_', physicalIndexName);
  }

  [Fact]
  public void ApplyDataVaultMetadataFailsBeforeDdlWhenProviderIdentifierCannotBeProjected() {
    var providerCapabilities = CreateMySqlProfileWithMaximumIdentifierLength(9);
    var modelBuilder = CreateModelBuilder();

    var exception = Assert.Throws<InvalidOperationException>(() =>
        modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), providerCapabilities));

    Assert.Contains("Provider identifier preflight failed", exception.Message, StringComparison.Ordinal);
    Assert.Contains("profile 'mysql-pomelo-v1-test'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("failure class 'length-limit'", exception.Message, StringComparison.Ordinal);
    Assert.Empty(modelBuilder.Model.GetEntityTypes());
  }

  [Fact]
  public void ApplyDataVaultMetadataFailsDeterministicallyWhenBridgeRequestsUnsupportedProjectionFeatures() {
    var metadataModel = new DataVaultMetadataModel(
        [],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [],
        [
            new DataVaultBridgeMetadata(
                "CustomerOrder",
                DataVaultBridgeKind.ManyToMany,
                DataVaultMetadataReference.Link("CustomerOrder"),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.From,
                        DataVaultMetadataReference.Hub("Customer"),
                        "Customer"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.To,
                        DataVaultMetadataReference.Hub("Order"),
                        "Order"),
                ],
                DataVaultBridgeProjectionFeatures.EffectivityWindow),
        ]);

    var exception = Assert.Throws<NotSupportedException>(() => CreateModelBuilder().ApplyDataVaultMetadata(metadataModel));

    Assert.Contains("CustomerOrder", exception.Message, StringComparison.Ordinal);
    Assert.Contains("EffectivityWindow", exception.Message, StringComparison.Ordinal);
    Assert.Contains("only endpoint hash-key columns and hierarchy TraversalDepth", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void MalformedHierarchyEndpointBindingsRemainMetadataValidationConcerns() {
    var exception = Assert.Throws<ArgumentException>(() => new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.From,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.To,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ChildRegion"),
        ]));

    Assert.Equal("endpoints", exception.ParamName);
    Assert.Contains(
        "A hierarchy bridge requires exactly one Ancestor endpoint and exactly one Descendant endpoint.",
        exception.Message,
        StringComparison.Ordinal);
  }

  [Fact]
  public void DataVaultMetadataModelRejectsNullCollectionsAndItems() {
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel(null!, [], []));
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel([], null!, []));
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel([], [], null!));
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel(
        [],
        [],
        [],
        (IEnumerable<DataVaultPointInTimeMetadata>)null!));
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel(
        [],
        [],
        [],
        (IEnumerable<DataVaultBridgeMetadata>)null!));
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel(
        [],
        [],
        [],
        (IEnumerable<DataVaultPitMetadata>)null!));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel([null!], [], []));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel([], [null!], []));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel([], [], [null!]));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel(
        [],
        [],
        [],
        new DataVaultPointInTimeMetadata[] { null! }));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel(
        [],
        [],
        [],
        new DataVaultBridgeMetadata[] { null! }));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel(
        [],
        [],
        [],
        new DataVaultPitMetadata[] { null! }));
  }

  private static IMutableModel CreateTranslatedModel() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());

    return modelBuilder.Model;
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
        ]);
  }

  private static DataVaultMetadataModel CreatePitMetadataModel() {
    return CreatePitMetadataModel(["Profile", "Status"]);
  }

  private static DataVaultMetadataModel CreatePitMetadataModel(string[] pitSatelliteNames) {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Profile",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
            new DataVaultSatelliteMetadata(
                "Status",
                DataVaultMetadataReference.Hub("Customer"),
                ["Status Code"]),
        ],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), pitSatelliteNames)]);
  }

  private static DataVaultMetadataModel CreateLinkParentPitMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var state = new DataVaultSatelliteMetadata(
        "State",
        customerOrder.ToReference(),
        ["State Code"]);
    var pit = new DataVaultPitMetadata(customerOrder.ToReference(), ["State"]);

    return new DataVaultMetadataModel([customer, order], [customerOrder], [state], [pit]);
  }

  private static DataVaultMetadataModel CreateBridgeMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
        ],
        [
            new DataVaultBridgeMetadata(
                "CustomerOrder",
                DataVaultBridgeKind.ManyToMany,
                DataVaultMetadataReference.Link("CustomerOrder"),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.From,
                        DataVaultMetadataReference.Hub("Customer"),
                        "Customer"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.To,
                        DataVaultMetadataReference.Hub("Order"),
                        "Order"),
                ]),
            new DataVaultBridgeMetadata(
                "SalesRegionHierarchy",
                DataVaultBridgeKind.Hierarchy,
                DataVaultMetadataReference.Link("SalesRegionParentChild"),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Ancestor,
                        DataVaultMetadataReference.Hub("SalesRegion"),
                        "ParentRegion"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Descendant,
                        DataVaultMetadataReference.Hub("SalesRegion"),
                        "ChildRegion"),
                ]),
        ]);
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }

  private static void AssertHub(IMutableEntityType hub) {
    Assert.Equal(DataVaultTableKind.Hub, AnnotationValue<DataVaultTableKind>(hub, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("Customer", AnnotationValue<string>(hub, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId"], PropertyNamesInOrdinalOrder(hub));
    AssertProperty(hub, "CustomerHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(hub, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(hub, "RecordSource", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.RecordSource);
    AssertProperty(hub, "CustomerId", DataVaultPropertyRole.BusinessKey, expectedTechnicalRole: null);
    AssertPrimaryKey(hub, "PkHubCustomerCustomerHashKey", ["CustomerHashKey"]);
    AssertIndex(hub, "IxHubCustomerBusinessKeyCustomerId", ["CustomerId"], isUnique: true);
    AssertNoRelationships(hub);
  }

  private static void AssertOrderHub(IMutableEntityType hub) {
    Assert.Equal(DataVaultTableKind.Hub, AnnotationValue<DataVaultTableKind>(hub, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("Order", AnnotationValue<string>(hub, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(["OrderHashKey", "LoadTimestamp", "RecordSource", "OrderId"], PropertyNamesInOrdinalOrder(hub));
    AssertProperty(hub, "OrderHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(hub, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(hub, "RecordSource", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.RecordSource);
    AssertProperty(hub, "OrderId", DataVaultPropertyRole.BusinessKey, expectedTechnicalRole: null);
    AssertPrimaryKey(hub, "PkHubOrderOrderHashKey", ["OrderHashKey"]);
    AssertIndex(hub, "IxHubOrderBusinessKeyOrderId", ["OrderId"], isUnique: true);
    AssertNoRelationships(hub);
  }

  private static void AssertLink(IMutableEntityType link) {
    Assert.Equal(DataVaultTableKind.Link, AnnotationValue<DataVaultTableKind>(link, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("CustomerOrder", AnnotationValue<string>(link, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        PropertyNamesInOrdinalOrder(link));
    AssertProperty(link, "CustomerOrderHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(link, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(link, "RecordSource", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.RecordSource);
    AssertProperty(link, "CustomerHashKey", DataVaultPropertyRole.ParticipantReference, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(link, "OrderHashKey", DataVaultPropertyRole.ParticipantReference, TechnicalMetadataColumnRole.HashKey);
    AssertPrimaryKey(link, "PkLinkCustomerOrderCustomerOrderHashKey", ["CustomerOrderHashKey"]);
    AssertIndex(link, "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey", ["CustomerHashKey", "OrderHashKey"], isUnique: false);
    AssertNoRelationships(link);
  }

  private static void AssertSatellite(IMutableEntityType satellite) {
    Assert.Equal(DataVaultTableKind.Satellite, AnnotationValue<DataVaultTableKind>(satellite, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("Contact", AnnotationValue<string>(satellite, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        DataVaultMetadataReferenceKind.Hub,
        AnnotationValue<DataVaultMetadataReferenceKind>(satellite, DataVaultAnnotationNames.ParentReferenceKind));
    Assert.Equal("Customer", AnnotationValue<string>(satellite, DataVaultAnnotationNames.ParentReferenceName));
    Assert.Equal(
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        PropertyNamesInOrdinalOrder(satellite));
    AssertProperty(satellite, "CustomerHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(satellite, "HashDiff", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashDiff);
    AssertProperty(satellite, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(satellite, "RecordSource", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.RecordSource);
    AssertProperty(satellite, "EmailAddress", DataVaultPropertyRole.Payload, expectedTechnicalRole: null);
    AssertPrimaryKey(satellite, "PkSatCustomerContactCustomerHashKeyLoadTimestamp", ["CustomerHashKey", "LoadTimestamp"]);
    AssertIndex(
        satellite,
        "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp", "HashDiff"],
        isUnique: false);
    AssertNoRelationships(satellite);
  }

  private static void AssertPit(IMutableEntityType pit) {
    Assert.Equal(DataVaultTableKind.Pit, AnnotationValue<DataVaultTableKind>(pit, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("CustomerProfileStatus", AnnotationValue<string>(pit, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        DataVaultMetadataReferenceKind.Hub,
        AnnotationValue<DataVaultMetadataReferenceKind>(pit, DataVaultAnnotationNames.ParentReferenceKind));
    Assert.Equal("Customer", AnnotationValue<string>(pit, DataVaultAnnotationNames.ParentReferenceName));
    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "ProfileLoadTimestamp", "StatusLoadTimestamp"],
        PropertyNamesInOrdinalOrder(pit));
    AssertProperty(pit, "CustomerHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(pit, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(pit, "ProfileLoadTimestamp", DataVaultPropertyRole.SnapshotReference, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(pit, "StatusLoadTimestamp", DataVaultPropertyRole.SnapshotReference, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertPrimaryKey(pit, "PkPitCustomerProfileStatusCustomerHashKeyLoadTimestamp", ["CustomerHashKey", "LoadTimestamp"]);
    AssertIndex(
        pit,
        "IxPitCustomerProfileStatusTraversalCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp"],
        isUnique: false);
    AssertNoRelationships(pit);
  }

  private static void AssertLinkParentPit(IMutableEntityType pit) {
    Assert.Equal(DataVaultTableKind.Pit, AnnotationValue<DataVaultTableKind>(pit, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("CustomerOrderState", AnnotationValue<string>(pit, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        DataVaultMetadataReferenceKind.Link,
        AnnotationValue<DataVaultMetadataReferenceKind>(pit, DataVaultAnnotationNames.ParentReferenceKind));
    Assert.Equal("CustomerOrder", AnnotationValue<string>(pit, DataVaultAnnotationNames.ParentReferenceName));
    Assert.Equal(
        ["CustomerOrderHashKey", "LoadTimestamp", "StateLoadTimestamp"],
        PropertyNamesInOrdinalOrder(pit));
    AssertProperty(pit, "CustomerOrderHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(pit, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(pit, "StateLoadTimestamp", DataVaultPropertyRole.SnapshotReference, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertPrimaryKey(
        pit,
        "PkPitCustomerOrderStateCustomerOrderHashKeyLoadTimestamp",
        ["CustomerOrderHashKey", "LoadTimestamp"]);
    AssertIndex(
        pit,
        "IxPitCustomerOrderStateTraversalCustomerOrderHashKeyLoadTimestamp",
        ["CustomerOrderHashKey", "LoadTimestamp"],
        isUnique: false);
    AssertNoRelationships(pit);
  }

  private static void AssertManyToManyBridge(IMutableEntityType bridge) {
    Assert.Equal(DataVaultTableKind.Bridge, AnnotationValue<DataVaultTableKind>(bridge, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("CustomerOrder", AnnotationValue<string>(bridge, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(["CustomerHashKey", "OrderHashKey"], PropertyNamesInOrdinalOrder(bridge));
    AssertProperty(bridge, "CustomerHashKey", DataVaultPropertyRole.ParticipantReference, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(bridge, "OrderHashKey", DataVaultPropertyRole.ParticipantReference, TechnicalMetadataColumnRole.HashKey);
    Assert.Equal("Customer", AnnotationValue<string>(bridge.FindProperty("CustomerHashKey")!, DataVaultAnnotationNames.MetadataName));
    Assert.Equal("Order", AnnotationValue<string>(bridge.FindProperty("OrderHashKey")!, DataVaultAnnotationNames.MetadataName));
    AssertPrimaryKey(
        bridge,
        "PkBridgeCustomerOrderCustomerHashKeyOrderHashKey",
        ["CustomerHashKey", "OrderHashKey"]);
    AssertIndex(
        bridge,
        "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
        ["OrderHashKey", "CustomerHashKey"],
        isUnique: false);
    AssertNoRelationships(bridge);
  }

  private static void AssertHierarchyBridge(IMutableEntityType bridge) {
    Assert.Equal(DataVaultTableKind.Bridge, AnnotationValue<DataVaultTableKind>(bridge, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("SalesRegionHierarchy", AnnotationValue<string>(bridge, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        ["AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey", "TraversalDepth"],
        PropertyNamesInOrdinalOrder(bridge));
    AssertProperty(
        bridge,
        "AncestorSalesRegionHashKey",
        DataVaultPropertyRole.ParticipantReference,
        TechnicalMetadataColumnRole.HashKey);
    AssertProperty(
        bridge,
        "DescendantSalesRegionHashKey",
        DataVaultPropertyRole.ParticipantReference,
        TechnicalMetadataColumnRole.HashKey);
    AssertProperty(bridge, "TraversalDepth", DataVaultPropertyRole.BridgeDepth, expectedTechnicalRole: null);
    Assert.Equal(
        "ParentRegion",
        AnnotationValue<string>(bridge.FindProperty("AncestorSalesRegionHashKey")!, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        "ChildRegion",
        AnnotationValue<string>(bridge.FindProperty("DescendantSalesRegionHashKey")!, DataVaultAnnotationNames.MetadataName));
    AssertPrimaryKey(
        bridge,
        "PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey",
        ["AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey"]);
    AssertNamedIndex(
        bridge,
        "IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth",
        ["AncestorSalesRegionHashKey", "TraversalDepth"],
        isUnique: false);
    AssertNamedIndex(
        bridge,
        "IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey",
        ["DescendantSalesRegionHashKey", "AncestorSalesRegionHashKey"],
        isUnique: false);
    AssertNoRelationships(bridge);
  }

  private static void AssertProperty(
      IMutableEntityType entityType,
      string propertyName,
      DataVaultPropertyRole expectedRole,
      TechnicalMetadataColumnRole? expectedTechnicalRole) {
    var property = entityType.FindProperty(propertyName);
    var expectedLogicalPropertyKind = GetExpectedLogicalPropertyKind(expectedRole, expectedTechnicalRole);

    Assert.NotNull(property);
    Assert.Equal(propertyName, AnnotationValue<string>(property!, DataVaultAnnotationNames.ProducedName));
    Assert.Equal(expectedRole, AnnotationValue<DataVaultPropertyRole>(property!, DataVaultAnnotationNames.PropertyRole));
    Assert.Equal(ExpectedClrType(expectedLogicalPropertyKind), property!.ClrType);
    Assert.Equal("sqlite-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal(ExpectedStorageType(expectedLogicalPropertyKind), AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(ExpectedStorageType(expectedLogicalPropertyKind), property.GetColumnType());
    Assert.Equal(ExpectedValueFormat(expectedLogicalPropertyKind), AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
    AssertHashKeyStorageAnnotations(property, expectedLogicalPropertyKind);

    if (expectedTechnicalRole is null) {
      Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.TechnicalColumnRole));
      return;
    }

    Assert.Equal(expectedTechnicalRole, AnnotationValue<TechnicalMetadataColumnRole>(property, DataVaultAnnotationNames.TechnicalColumnRole));
  }

  private static void AssertProviderProperty(
      IMutableEntityType entityType,
      string propertyName,
      DataVaultLogicalPropertyKind expectedLogicalPropertyKind,
      Type expectedClrType,
      string expectedStorageType,
      DataVaultProviderValueFormat expectedValueFormat,
      string expectedProviderProfile = "oracle-v1") {
    var property = entityType.FindProperty(propertyName);

    Assert.NotNull(property);
    Assert.Equal(expectedClrType, property!.ClrType);
    Assert.Equal(expectedProviderProfile, AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal(expectedStorageType, AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(expectedStorageType, property.GetColumnType());
    Assert.Equal(expectedValueFormat, AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
    AssertHashKeyStorageAnnotations(property, expectedLogicalPropertyKind);
  }

  private static void AssertBinaryHashKeyProperty(
      IMutableEntityType entityType,
      string propertyName,
      DataVaultLogicalPropertyKind expectedLogicalPropertyKind,
      string expectedStorageType = "varbinary(16)",
      string expectedStableHashAlgorithmId = "sha256-128-v1",
      int expectedStableHashDigestByteLength = 16) {
    var property = entityType.FindProperty(propertyName);

    Assert.NotNull(property);
    Assert.Equal(typeof(string), property!.ClrType);
    Assert.Equal("sqlserver-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal(expectedStorageType, property.GetColumnType());
    Assert.Equal(expectedStorageType, AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(DataVaultProviderValueFormat.LowercaseHexBinary, AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, AnnotationValue<DataVaultHashKeyStorageProfile>(
        property,
        DataVaultAnnotationNames.HashKeyStorageProfile));
    Assert.Equal(expectedStableHashAlgorithmId, AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashAlgorithmId));
    Assert.Equal(expectedStableHashDigestByteLength, AnnotationValue<int>(property, DataVaultAnnotationNames.StableHashDigestByteLength));
    Assert.Equal("lowercase-hex-no-prefix", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashDigestEncoding));
    Assert.Equal("lowercase-hex-string-to-bytes", AnnotationValue<string>(property, DataVaultAnnotationNames.HashKeyConversionBehavior));

    var converter = property.GetValueConverter();

    Assert.NotNull(converter);
    Assert.Equal(typeof(string), converter!.ModelClrType);
    Assert.Equal(typeof(byte[]), converter.ProviderClrType);
  }

  private static IMutableProperty CreateEncryptedPayloadTranslatedPayloadProperty(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), providerCapabilities);
    modelBuilder.SharedTypeEntity<Dictionary<string, object>>("SatCustomerContact", entityBuilder => {
      entityBuilder.IndexerProperty<string>("EmailAddress")
          .HasConversion(CreateEncryptedPayloadValueConverter());
    });

    var payload = FindEntity(modelBuilder.Model, "SatCustomerContact").FindProperty("EmailAddress");

    Assert.NotNull(payload);
    return payload!;
  }

  private static DataVaultEncryptedPayloadValueConverter CreateEncryptedPayloadValueConverter() {
    return new DataVaultEncryptedPayloadValueConverter(
        new TestPrivacyConfiguration(new TestEncryptedPayloadKeyProvider()),
        EncryptedPayloadAlias);
  }

  private static void AssertEncryptedPayloadProviderProperty(
      IMutableProperty property,
      string expectedProviderProfile,
      string expectedStorageType) {
    Assert.Equal("EmailAddress", AnnotationValue<string>(property, DataVaultAnnotationNames.ProducedName));
    Assert.Equal("Email Address", AnnotationValue<string>(property, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(DataVaultPropertyRole.Payload, AnnotationValue<DataVaultPropertyRole>(
        property,
        DataVaultAnnotationNames.PropertyRole));
    Assert.Equal(typeof(string), property.ClrType);
    Assert.Equal(expectedProviderProfile, AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(DataVaultLogicalPropertyKind.PayloadText, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal(expectedStorageType, AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(expectedStorageType, property.GetColumnType());
    Assert.Equal(DataVaultProviderValueFormat.Text, AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
    Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.TechnicalColumnRole));
    AssertHashKeyStorageAnnotations(property, DataVaultLogicalPropertyKind.PayloadText);
  }

  private static DataVaultProviderCapabilityProfile SelectBuiltInProviderCapabilityProfile(string profileName) {
    return profileName switch {
      "sqlite-v1" => DataVaultProviderCapabilityProfiles.Sqlite,
      "postgres-v1" => DataVaultProviderCapabilityProfiles.Postgres,
      "sqlserver-v1" => DataVaultProviderCapabilityProfiles.SqlServer,
      "oracle-v1" => DataVaultProviderCapabilityProfiles.Oracle,
      "db2-v1" => DataVaultProviderCapabilityProfiles.Db2,
      "mysql-pomelo-v1" => DataVaultProviderCapabilityProfiles.MySql,
      _ => throw new ArgumentOutOfRangeException(nameof(profileName), profileName, "Unsupported provider profile."),
    };
  }

  private static DataVaultProviderCapabilityProfile CreateProfileWithoutPayloadTextMapping() {
    var profile = DataVaultProviderCapabilityProfiles.Sqlite;

    return new DataVaultProviderCapabilityProfile(
        "broken-encrypted-payload-profile",
        profile.SqlFunctionSupport,
        profile.ConcurrencySupport,
        profile.TypeMappings.Where(mapping => mapping.LogicalPropertyKind != DataVaultLogicalPropertyKind.PayloadText),
        profile.MaximumIdentifierLength,
        profile.AllowsIndexesCoveredByPrimaryKey,
        profile.UnsupportedIncludedIndexColumnMode);
  }

  private static void InvokeTranslatorApply(
      ModelBuilder modelBuilder,
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var translatorType = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions).Assembly
        .GetType("DCoding.Data.DVault.DataVaultEfMetadataTranslator");

    Assert.NotNull(translatorType);

    var applyMethod = translatorType!
        .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "Apply" &&
            methodInfo.GetParameters().Length == 3);

    applyMethod.Invoke(null, [modelBuilder, metadataModel, providerCapabilities]);
  }

  private static ValueConverter GetBinaryHashKeyConverter(
      string algorithmId = "sha256-128-v1",
      int digestByteLength = 16) {
    var modelBuilder = CreateModelBuilder();
    var profile = DataVaultProviderCapabilityProfiles.SqlServer.WithHashKeyStorageProfile(
        DataVaultHashKeyStorageProfile.Binary,
        algorithmId,
        digestByteLength);

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), profile);

    var hashKey = FindEntity(modelBuilder.Model, "HubCustomer").FindProperty("CustomerHashKey");

    Assert.NotNull(hashKey);

    var converter = hashKey!.GetValueConverter();

    Assert.NotNull(converter);
    return converter!;
  }

  private static BinaryHashKeyChangeTrackingContext CreateBinaryHashKeyChangeTrackingContext() {
    var options = new DbContextOptionsBuilder<BinaryHashKeyChangeTrackingContext>()
        .UseSqlite("Data Source=:memory:")
        .Options;

    return new BinaryHashKeyChangeTrackingContext(options);
  }

  private static Dictionary<string, object> CreateLinkEntity(
      string linkHashKey,
      string? customerHashKey,
      string? orderHashKey) {
    return new Dictionary<string, object> {
      ["CustomerOrderHashKey"] = linkHashKey,
      ["LoadTimestamp"] = DateTimeOffset.UnixEpoch,
      ["RecordSource"] = "unit-test",
      ["CustomerHashKey"] = customerHashKey!,
      ["OrderHashKey"] = orderHashKey!,
    };
  }

  private static string CreateCanonicalHexDigest(int digestByteLength, int seed = 0) {
    return Convert.ToHexString(Enumerable
        .Range(0, digestByteLength)
        .Select(value => (byte)((value + seed) % 256))
        .ToArray()).ToLowerInvariant();
  }

  private sealed class BinaryHashKeyChangeTrackingContext : DbContext {
    public BinaryHashKeyChangeTrackingContext(DbContextOptions<BinaryHashKeyChangeTrackingContext> options)
        : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      var profile = DataVaultProviderCapabilityProfiles.SqlServer.WithHashKeyStorageProfile(
          DataVaultHashKeyStorageProfile.Binary,
          "sha256-128-v1",
          16);

      modelBuilder.ApplyDataVaultMetadata(CreateBridgeMetadataModel(), profile);
    }
  }

  private sealed class TestPrivacyConfiguration(IDataVaultPrivacyKeyProvider keyProvider) : IDataVaultPrivacyConfiguration {
    public IReadOnlyList<string> EncryptedPayloadAliases { get; } = [EncryptedPayloadAlias];

    public IDataVaultPrivacyKeyProvider? KeyProvider { get; } = keyProvider;
  }

  private sealed class TestEncryptedPayloadKeyProvider : IDataVaultEncryptedPayloadKeyProvider {
    public DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
        DataVaultEncryptedPayloadConversionRequest request) {
      return request.Direction switch {
        DataVaultEncryptedPayloadConversionDirection.Encrypt => DataVaultEncryptedPayloadConversionResult.Approved(
            "encrypted:" +
            request.EncryptedPayloadAlias +
            ":" +
            request.Value.Length.ToString(CultureInfo.InvariantCulture)),
        DataVaultEncryptedPayloadConversionDirection.Decrypt => DataVaultEncryptedPayloadConversionResult.Approved(
            "decrypted:" + request.EncryptedPayloadAlias),
        _ => DataVaultEncryptedPayloadConversionResult.Declined("unsupported-conversion-direction"),
      };
    }
  }

  private static DataVaultLogicalPropertyKind GetExpectedLogicalPropertyKind(
      DataVaultPropertyRole role,
      TechnicalMetadataColumnRole? technicalRole) {
    return role switch {
      DataVaultPropertyRole.BusinessKey => DataVaultLogicalPropertyKind.BusinessKey,
      DataVaultPropertyRole.ParticipantReference => DataVaultLogicalPropertyKind.ParticipantReference,
      DataVaultPropertyRole.DrivingKey => DataVaultLogicalPropertyKind.DrivingKey,
      DataVaultPropertyRole.Payload => DataVaultLogicalPropertyKind.PayloadText,
      DataVaultPropertyRole.SnapshotReference => DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
      DataVaultPropertyRole.BridgeDepth => DataVaultLogicalPropertyKind.BridgeDepth,
      DataVaultPropertyRole.Technical => technicalRole switch {
        TechnicalMetadataColumnRole.HashKey => DataVaultLogicalPropertyKind.HashKey,
        TechnicalMetadataColumnRole.HashDiff => DataVaultLogicalPropertyKind.HashDiff,
        TechnicalMetadataColumnRole.LoadTimestamp => DataVaultLogicalPropertyKind.LoadTimestamp,
        TechnicalMetadataColumnRole.RecordSource => DataVaultLogicalPropertyKind.RecordSource,
        _ => throw new InvalidOperationException("Expected a technical metadata role."),
      },
      _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported Data Vault property role."),
    };
  }

  private static Type ExpectedClrType(DataVaultLogicalPropertyKind logicalPropertyKind) {
    return logicalPropertyKind switch {
      DataVaultLogicalPropertyKind.LoadTimestamp => typeof(DateTimeOffset),
      DataVaultLogicalPropertyKind.SatelliteSnapshotReference => typeof(DateTimeOffset?),
      DataVaultLogicalPropertyKind.BridgeDepth => typeof(int),
      _ => typeof(string),
    };
  }

  private static DataVaultProviderValueFormat ExpectedValueFormat(DataVaultLogicalPropertyKind logicalPropertyKind) {
    return logicalPropertyKind switch {
      DataVaultLogicalPropertyKind.HashKey => DataVaultProviderValueFormat.LowercaseHexBinary,
      DataVaultLogicalPropertyKind.ParticipantReference => DataVaultProviderValueFormat.LowercaseHexBinary,
      DataVaultLogicalPropertyKind.LoadTimestamp => DataVaultProviderValueFormat.Iso8601UtcText,
      DataVaultLogicalPropertyKind.SatelliteSnapshotReference => DataVaultProviderValueFormat.Iso8601UtcText,
      DataVaultLogicalPropertyKind.BridgeDepth => DataVaultProviderValueFormat.NativeInteger,
      _ => DataVaultProviderValueFormat.Text,
    };
  }

  private static string ExpectedStorageType(DataVaultLogicalPropertyKind logicalPropertyKind) {
    return logicalPropertyKind switch {
      DataVaultLogicalPropertyKind.HashKey => "BLOB",
      DataVaultLogicalPropertyKind.ParticipantReference => "BLOB",
      DataVaultLogicalPropertyKind.BridgeDepth => "INTEGER",
      _ => "TEXT",
    };
  }

  private static void AssertHashKeyStorageAnnotations(
      IMutableProperty property,
      DataVaultLogicalPropertyKind logicalPropertyKind) {
    if (logicalPropertyKind is not (DataVaultLogicalPropertyKind.HashKey or DataVaultLogicalPropertyKind.ParticipantReference)) {
      Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.HashKeyStorageProfile));
      Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.StableHashAlgorithmId));
      Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.StableHashDigestByteLength));
      Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.StableHashDigestEncoding));
      Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.HashKeyConversionBehavior));
      return;
    }

    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, AnnotationValue<DataVaultHashKeyStorageProfile>(
        property,
        DataVaultAnnotationNames.HashKeyStorageProfile));
    Assert.Equal("sha256-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashAlgorithmId));
    Assert.Equal(32, AnnotationValue<int>(property, DataVaultAnnotationNames.StableHashDigestByteLength));
    Assert.Equal("lowercase-hex-no-prefix", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashDigestEncoding));
    Assert.Equal("lowercase-hex-string-to-bytes", AnnotationValue<string>(property, DataVaultAnnotationNames.HashKeyConversionBehavior));
    Assert.NotNull(property.GetValueConverter());
  }

  private static void AssertBinaryHashDefaults(DataVaultConventions conventions) {
    Assert.Equal("sha256-v1", conventions.StableHashAlgorithmId);
    Assert.Equal(32, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
  }

  private static void AssertSqliteBinaryHashKeyProperty(
      IMutableEntityType entityType,
      string propertyName,
      DataVaultLogicalPropertyKind expectedLogicalPropertyKind) {
    var property = entityType.FindProperty(propertyName);

    Assert.NotNull(property);
    Assert.Equal(typeof(string), property!.ClrType);
    Assert.Equal("sqlite-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal("BLOB", property.GetColumnType());
    Assert.Equal("BLOB", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(DataVaultProviderValueFormat.LowercaseHexBinary, AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, AnnotationValue<DataVaultHashKeyStorageProfile>(
        property,
        DataVaultAnnotationNames.HashKeyStorageProfile));
    Assert.Equal("sha256-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashAlgorithmId));
    Assert.Equal(32, AnnotationValue<int>(property, DataVaultAnnotationNames.StableHashDigestByteLength));
    Assert.Equal("lowercase-hex-no-prefix", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashDigestEncoding));
    Assert.Equal("lowercase-hex-string-to-bytes", AnnotationValue<string>(property, DataVaultAnnotationNames.HashKeyConversionBehavior));
    Assert.NotNull(property.GetValueConverter());
  }

  private static void AssertHexStringHashKeyCompatibilityProperty(
      IMutableEntityType entityType,
      string propertyName,
      DataVaultLogicalPropertyKind expectedLogicalPropertyKind) {
    var property = entityType.FindProperty(propertyName);

    Assert.NotNull(property);
    Assert.Equal(typeof(string), property!.ClrType);
    Assert.Equal("sqlite-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal("TEXT", property.GetColumnType());
    Assert.Equal("TEXT", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(DataVaultProviderValueFormat.LowercaseHexText, AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
    Assert.Equal(DataVaultHashKeyStorageProfile.HexString, AnnotationValue<DataVaultHashKeyStorageProfile>(
        property,
        DataVaultAnnotationNames.HashKeyStorageProfile));
    Assert.Equal("sha256-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashAlgorithmId));
    Assert.Equal(32, AnnotationValue<int>(property, DataVaultAnnotationNames.StableHashDigestByteLength));
    Assert.Equal("lowercase-hex-no-prefix", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashDigestEncoding));
    Assert.Equal("none-string-model", AnnotationValue<string>(property, DataVaultAnnotationNames.HashKeyConversionBehavior));
    Assert.Null(property.GetValueConverter());
  }

  private static void AssertPrimaryKey(IMutableEntityType entityType, string expectedName, string[] expectedProperties) {
    var primaryKey = entityType.FindPrimaryKey();

    Assert.NotNull(primaryKey);
    Assert.Equal(expectedName, AnnotationValue<string>(primaryKey!, DataVaultAnnotationNames.ProducedName));
    Assert.Equal(expectedProperties, primaryKey!.Properties.Select(property => property.Name));
  }

  private static void AssertIndex(IMutableEntityType entityType, string expectedName, string[] expectedProperties, bool isUnique) {
    var index = Assert.Single(entityType.GetIndexes());

    Assert.Equal(expectedName, AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName));
    Assert.Equal(expectedProperties, index.Properties.Select(property => property.Name));
    Assert.Equal(isUnique, index.IsUnique);
  }

  private static void AssertNamedIndex(IMutableEntityType entityType, string expectedName, string[] expectedProperties, bool isUnique) {
    var index = Assert.Single(
        entityType.GetIndexes(),
        index => string.Equals(
            AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName),
            expectedName,
            StringComparison.Ordinal));

    Assert.Equal(expectedProperties, index.Properties.Select(property => property.Name));
    Assert.Equal(isUnique, index.IsUnique);
  }

  private static void AssertRelationalEntity(
      IMutableEntityType entityType,
      string expectedTableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      string expectedIndexName) {
    var table = StoreObjectIdentifier.Table(expectedTableName, schema: null);

    Assert.Equal(expectedTableName, entityType.GetTableName());
    Assert.Equal(expectedColumnNames, PropertyNamesInOrdinalOrder(entityType));
    Assert.Equal(
        expectedColumnNames,
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnName(table)));
    Assert.Equal(
        Enumerable.Range(0, expectedColumnNames.Length).Select(order => (int?)order),
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnOrder()));
    Assert.Equal(expectedPrimaryKeyName, entityType.FindPrimaryKey()!.GetName());
    Assert.Equal(expectedIndexName, Assert.Single(entityType.GetIndexes()).GetDatabaseName());
  }

  private static void AssertRelationalPitEntity(
      IMutableEntityType entityType,
      string expectedTableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      string expectedIndexName) {
    var table = StoreObjectIdentifier.Table(expectedTableName, schema: null);

    Assert.Equal(expectedTableName, entityType.GetTableName());
    Assert.Equal(expectedColumnNames, PropertyNamesInOrdinalOrder(entityType));
    Assert.Equal(
        expectedColumnNames,
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnName(table)));
    Assert.Equal(expectedPrimaryKeyName, entityType.FindPrimaryKey()!.GetName());
    Assert.Equal(expectedIndexName, Assert.Single(entityType.GetIndexes()).GetDatabaseName());
  }

  private static void AssertPitTranslationFailure(DataVaultMetadataModel metadataModel, string expectedMessage) {
    var modelBuilder = CreateModelBuilder();

    var exception = Assert.Throws<NotSupportedException>(() => modelBuilder.ApplyDataVaultMetadata(metadataModel));

    Assert.Contains(expectedMessage, exception.Message, StringComparison.Ordinal);
    Assert.Empty(modelBuilder.Model.GetEntityTypes());
  }

  private static void AssertRelationalEntityWithIndexes(
      IMutableEntityType entityType,
      string expectedTableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      (string Name, string[] Properties)[] expectedIndexes) {
    var table = StoreObjectIdentifier.Table(expectedTableName, schema: null);

    Assert.Equal(expectedTableName, entityType.GetTableName());
    Assert.Equal(expectedColumnNames, PropertyNamesInOrdinalOrder(entityType));
    Assert.Equal(
        expectedColumnNames,
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnName(table)));
    Assert.Equal(
        Enumerable.Range(0, expectedColumnNames.Length).Select(order => (int?)order),
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnOrder()));
    Assert.Equal(expectedPrimaryKeyName, entityType.FindPrimaryKey()!.GetName());

    var indexesByName = entityType.GetIndexes()
        .ToDictionary(index => index.GetDatabaseName() ?? string.Empty, StringComparer.Ordinal);

    Assert.DoesNotContain(string.Empty, indexesByName.Keys);
    Assert.Equal(
        expectedIndexes.Select(index => index.Name).Order(StringComparer.Ordinal),
        indexesByName.Keys.Order(StringComparer.Ordinal));

    foreach (var expectedIndex in expectedIndexes) {
      var index = indexesByName[expectedIndex.Name];

      Assert.Equal(expectedIndex.Properties, index.Properties.Select(property => property.GetColumnName(table)));
    }
  }

  private static void AssertNoRelationships(IMutableEntityType entityType) {
    Assert.Empty(entityType.GetForeignKeys());
    Assert.Empty(entityType.GetNavigations());
    Assert.Empty(entityType.GetSkipNavigations());
  }

  private static IMutableEntityType FindEntity(IMutableModel model, string producedName) {
    var matches = model.GetEntityTypes()
        .Where(entityType => string.Equals(
            entityType.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string,
            producedName,
            StringComparison.Ordinal))
        .ToArray();

    Assert.Single(matches);
    return matches[0];
  }

  private static string[] PropertyNamesInOrdinalOrder(IMutableEntityType entityType) {
    return entityType.GetProperties()
        .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
        .Select(property => property.Name)
        .ToArray();
  }

  private static string[] ModelShape(IMutableModel model) {
    return model.GetEntityTypes()
        .Select(entityType => string.Join(
            "|",
            AnnotationValue<string>(entityType, DataVaultAnnotationNames.ProducedName),
            AnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind).ToString(),
            string.Join(",", PropertyNamesInOrdinalOrder(entityType)),
            string.Join(",", entityType.FindPrimaryKey()!.Properties.Select(property => property.Name)),
            string.Join(
                ";",
                entityType.GetIndexes()
                    .OrderBy(index => AnnotationValue<int>(index, DataVaultAnnotationNames.Ordinal))
                    .Select(index => string.Join(",", index.Properties.Select(property => property.Name)) + ":" + index.IsUnique))))
        .Order(StringComparer.Ordinal)
        .ToArray();
  }

  private static DataVaultProviderCapabilityProfile CreateMySqlProfileWithMaximumIdentifierLength(
      int maximumIdentifierLength) {
    return new DataVaultProviderCapabilityProfile(
        "mysql-pomelo-v1-test",
        DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
        DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
        DataVaultProviderCapabilityProfiles.MySql.TypeMappings,
        maximumIdentifierLength,
        unsupportedIncludedIndexColumnMode: DataVaultUnsupportedIncludedIndexColumnMode.Ignore);
  }

  private static T AnnotationValue<T>(IMutableEntityType entityType, string name) {
    return Assert.IsType<T>(RequiredAnnotation(entityType.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableProperty property, string name) {
    return Assert.IsType<T>(RequiredAnnotation(property.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableKey key, string name) {
    return Assert.IsType<T>(RequiredAnnotation(key.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableIndex index, string name) {
    return Assert.IsType<T>(RequiredAnnotation(index.FindAnnotation(name)).Value);
  }

  private static IAnnotation RequiredAnnotation(IAnnotation? annotation) {
    Assert.NotNull(annotation);

    return annotation!;
  }
}
