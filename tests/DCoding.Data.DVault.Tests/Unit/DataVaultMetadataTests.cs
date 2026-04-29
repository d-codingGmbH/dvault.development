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
}
