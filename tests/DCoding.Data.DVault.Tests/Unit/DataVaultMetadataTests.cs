using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultMetadataTests
{
    [Fact]
    public void HubMetadataRetainsIdentifyingProperties()
    {
        var hub = new DataVaultHubMetadata("Customer", ["CustomerId", "SourceSystem"]);

        Assert.Equal("Customer", hub.Name);
        Assert.Equal(["CustomerId", "SourceSystem"], hub.BusinessKeyNames);
        Assert.Equal(["CustomerId", "SourceSystem"], hub.BusinessKeyColumns.Select(column => column.ColumnName));
        Assert.Equal(TechnicalMetadataColumnRole.HashKey, hub.HashKeyMetadata.Role);
        Assert.Equal(TechnicalMetadataColumnRole.LoadTimestamp, hub.LoadTimestampMetadata.Role);
        Assert.Equal(TechnicalMetadataColumnRole.RecordSource, hub.RecordSourceMetadata.Role);
        AssertRequiredRoles(
            hub.TechnicalMetadataColumns,
            TechnicalMetadataColumnRole.HashKey,
            TechnicalMetadataColumnRole.LoadTimestamp,
            TechnicalMetadataColumnRole.RecordSource);

        var reference = hub.ToReference();
        Assert.Equal(DataVaultMetadataReferenceKind.Hub, reference.Kind);
        Assert.Equal("Customer", reference.Name);
    }

    [Fact]
    public void LinkMetadataRetainsAtLeastTwoHubEndpoints()
    {
        var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
        var order = new DataVaultHubMetadata("Order", ["OrderId"]);

        var link = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);

        Assert.Equal("CustomerOrder", link.Name);
        Assert.Equal(2, link.Endpoints.Count);
        Assert.All(link.Endpoints, endpoint => Assert.Equal(DataVaultMetadataReferenceKind.Hub, endpoint.Kind));
        Assert.Equal("Customer", link.Endpoints[0].Name);
        Assert.Equal("Order", link.Endpoints[1].Name);
        Assert.Equal(2, link.Participants.Count);
        Assert.Equal("Customer", link.Participants[0].HubReference.Name);
        Assert.Equal("Order", link.Participants[1].HubReference.Name);
        Assert.All(link.Participants, participant =>
        {
            Assert.Equal(DataVaultMetadataReferenceKind.Hub, participant.HubReference.Kind);
            Assert.Equal(TechnicalMetadataColumnRole.HashKey, participant.HashKeyMetadata.Role);
        });
        Assert.Equal(TechnicalMetadataColumnRole.HashKey, link.HashKeyMetadata.Role);
        Assert.Equal(TechnicalMetadataColumnRole.LoadTimestamp, link.LoadTimestampMetadata.Role);
        Assert.Equal(TechnicalMetadataColumnRole.RecordSource, link.RecordSourceMetadata.Role);
        AssertRequiredRoles(
            link.TechnicalMetadataColumns,
            TechnicalMetadataColumnRole.HashKey,
            TechnicalMetadataColumnRole.LoadTimestamp,
            TechnicalMetadataColumnRole.RecordSource);

        var reference = link.ToReference();
        Assert.Equal(DataVaultMetadataReferenceKind.Link, reference.Kind);
        Assert.Equal("CustomerOrder", reference.Name);
    }

    [Fact]
    public void SatelliteMetadataRetainsHubParentAndDescriptiveAttributes()
    {
        var parent = DataVaultMetadataReference.Hub("Customer");

        var satellite = new DataVaultSatelliteMetadata(
            "CustomerContact",
            parent,
            ["EmailAddress", "PhoneNumber"]);

        Assert.Equal("CustomerContact", satellite.Name);
        Assert.Equal(DataVaultMetadataReferenceKind.Hub, satellite.Parent.Kind);
        Assert.Equal("Customer", satellite.Parent.Name);
        Assert.Equal(["EmailAddress", "PhoneNumber"], satellite.DescriptiveAttributeNames);
        Assert.Equal(["EmailAddress", "PhoneNumber"], satellite.PayloadColumns.Select(column => column.ColumnName));
        Assert.Equal(TechnicalMetadataColumnRole.HashDiff, satellite.HashDiffMetadata.Role);
        Assert.Equal(TechnicalMetadataColumnRole.LoadTimestamp, satellite.LoadTimestampMetadata.Role);
        Assert.Equal(TechnicalMetadataColumnRole.RecordSource, satellite.RecordSourceMetadata.Role);
        AssertRequiredRoles(
            satellite.TechnicalMetadataColumns,
            TechnicalMetadataColumnRole.HashDiff,
            TechnicalMetadataColumnRole.LoadTimestamp,
            TechnicalMetadataColumnRole.RecordSource);
    }

    [Fact]
    public void SatelliteMetadataRetainsLinkParent()
    {
        var parent = DataVaultMetadataReference.Link("CustomerOrder");

        var satellite = new DataVaultSatelliteMetadata("OrderStatus", parent, ["Status"]);

        Assert.Equal(DataVaultMetadataReferenceKind.Link, satellite.Parent.Kind);
        Assert.Equal("CustomerOrder", satellite.Parent.Name);
        Assert.Equal(["Status"], satellite.DescriptiveAttributeNames);
    }

    [Fact]
    public void MetadataAbstractionsUseProviderNeutralClrContracts()
    {
        var metadataTypes = new[]
        {
            typeof(DataVaultBusinessKeyMetadata),
            typeof(DataVaultHubMetadata),
            typeof(DataVaultLinkMetadata),
            typeof(DataVaultLinkParticipantMetadata),
            typeof(DataVaultSatelliteMetadata),
            typeof(DataVaultSatellitePayloadMetadata),
        };
        var providerTokens = new[] { "Sqlite", "Postgres", "Npgsql", "Migration", "Sequence", "Trigger" };

        foreach (var metadataType in metadataTypes)
        {
            Assert.DoesNotContain(providerTokens, token => metadataType.FullName!.Contains(token, StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(metadataType.GetProperties(), property =>
                providerTokens.Any(token => property.PropertyType.FullName!.Contains(token, StringComparison.OrdinalIgnoreCase)));
        }
    }

    [Fact]
    public void RequiredMetadataNamesRejectNullEmptyAndWhitespace()
    {
        foreach (var invalidName in new string?[] { null, "", " " })
        {
            ThrowsArgumentException(() => new DataVaultHubMetadata(invalidName!, ["CustomerId"]));
            ThrowsArgumentException(() => new DataVaultHubMetadata("Customer", [invalidName!]));
            ThrowsArgumentException(() => DataVaultMetadataReference.Hub(invalidName!));
            ThrowsArgumentException(() => DataVaultMetadataReference.Link(invalidName!));
            ThrowsArgumentException(() => new DataVaultLinkMetadata(
                invalidName!,
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]));
            ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
                invalidName!,
                DataVaultMetadataReference.Hub("Customer"),
                ["EmailAddress"]));
            ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
                "CustomerContact",
                DataVaultMetadataReference.Hub("Customer"),
                [invalidName!]));
            ThrowsArgumentException(() => new DataVaultBusinessKeyMetadata(invalidName!));
            ThrowsArgumentException(() => new DataVaultSatellitePayloadMetadata(invalidName!));
        }
    }

    [Fact]
    public void RequiredMetadataCollectionsRejectNullAndEmpty()
    {
        ThrowsArgumentException(() => new DataVaultHubMetadata("Customer", null!));
        ThrowsArgumentException(() => new DataVaultHubMetadata("Customer", []));
        ThrowsArgumentException(() => new DataVaultLinkMetadata("CustomerOrder", null!));
        ThrowsArgumentException(() => new DataVaultLinkMetadata("CustomerOrder", []));
        ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
            "CustomerContact",
            DataVaultMetadataReference.Hub("Customer"),
            null!));
        ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
            "CustomerContact",
            DataVaultMetadataReference.Hub("Customer"),
            []));
    }

    [Fact]
    public void LinkMetadataRequiresAtLeastTwoHubEndpoints()
    {
        ThrowsArgumentException(() => new DataVaultLinkMetadata(
            "CustomerOrder",
            [DataVaultMetadataReference.Hub("Customer")]));
        ThrowsArgumentException(() => new DataVaultLinkMetadata(
            "CustomerOrder",
            [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Link("OrderPayment")]));
        ThrowsArgumentException(() => new DataVaultLinkParticipantMetadata(DataVaultMetadataReference.Link("OrderPayment")));
    }

    [Fact]
    public void SatelliteMetadataRequiresParentRelationship()
    {
        ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
            "CustomerContact",
            null!,
            ["EmailAddress"]));
    }

    private static void ThrowsArgumentException(Action action)
    {
        var exception = Record.Exception(action);

        Assert.IsAssignableFrom<ArgumentException>(exception);
    }

    private static void AssertRequiredRoles(
        IReadOnlyList<TechnicalMetadataColumnContract> contracts,
        params TechnicalMetadataColumnRole[] expectedRoles)
    {
        Assert.Equal(expectedRoles, contracts.Select(contract => contract.Role));
        Assert.All(contracts, contract =>
        {
            Assert.Equal(TechnicalMetadataColumnRequiredness.RequiredWhenDeclared, contract.RequirednessExpectation);
            Assert.False(string.IsNullOrWhiteSpace(contract.DefaultEffectiveColumnName));
            Assert.Equal(contract.DefaultEffectiveColumnName, contract.EffectiveColumnName);
        });
    }
}
