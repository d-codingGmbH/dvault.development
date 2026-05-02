using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault.Benchmarks;

internal static class ScenarioContracts {
  public const string CustomerBusinessKey = "C-100";
  public const string OrderBusinessKey = "O-1000";
  public const string ProductBusinessKey = "SKU-COFFEE";
  public const string ProductName = "Coffee subscription";

  public static readonly CustomerProfileEvent[] CustomerProfileEvents =
  [
      new(
          CustomerBusinessKey,
          "Alice Adams",
          "prospect",
          new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
          "crm-import",
          "profile-hash-1"),
      new(
          CustomerBusinessKey,
          "Alice Baker",
          "active",
          new DateTimeOffset(2026, 4, 29, 11, 30, 0, TimeSpan.Zero),
          "crm-change",
          "profile-hash-2"),
  ];

  public static readonly OrderRelationshipEvent OrderRelationship = new(
      OrderBusinessKey,
      ProductBusinessKey,
      ProductName,
      new DateTimeOffset(2026, 5, 1, 9, 30, 0, TimeSpan.Zero),
      "order-entry");

  public static readonly OrderFulfillmentEvent[] MeasuredOrderFulfillmentEvents =
  [
      new(
          "Backordered",
          "NORTH-1",
          new DateTimeOffset(2026, 5, 1, 10, 0, 0, TimeSpan.Zero),
          "warehouse-allocation",
          "fulfillment-backordered-north-1"),
      new(
          "Allocated",
          "NORTH-1",
          new DateTimeOffset(2026, 5, 1, 10, 45, 0, TimeSpan.Zero),
          "warehouse-allocation",
          "fulfillment-allocated-north-1"),
  ];

  public static readonly OrderFulfillmentEvent UnchangedOrderFulfillmentReplay = new(
      "Allocated",
      "NORTH-1",
      new DateTimeOffset(2026, 5, 1, 11, 15, 0, TimeSpan.Zero),
      "warehouse-replay",
      "fulfillment-allocated-north-1");

  public static readonly DataVaultHubMetadata CustomerHub = new("Customer", ["Customer Id"]);
  public static readonly DataVaultSatelliteMetadata CustomerProfileSatellite = new(
      "Profile",
      CustomerHub.ToReference(),
      ["customer_name", "customer_status"]);

  public static readonly DataVaultHubMetadata OrderHub = new("Order", ["Order Id"]);
  public static readonly DataVaultHubMetadata ProductHub = new("Product", ["Sku"]);
  public static readonly DataVaultLinkMetadata OrderProductLink = new(
      "OrderProduct",
      [OrderHub.ToReference(), ProductHub.ToReference()]);
  public static readonly DataVaultSatelliteMetadata OrderFulfillmentSatellite = new(
      "Fulfillment",
      OrderProductLink.ToReference(),
      ["Allocation Status", "Warehouse Code"]);

  public static DataVaultMetadataModel CreateCustomerProfileDataVaultModel() {
    return new DataVaultMetadataModel([CustomerHub], [], [CustomerProfileSatellite]);
  }

  public static DataVaultMetadataModel CreateOrderProductDataVaultModel() {
    return new DataVaultMetadataModel([OrderHub, ProductHub], [OrderProductLink], [OrderFulfillmentSatellite]);
  }
}

internal sealed record CustomerProfileEvent(
    string CustomerBusinessKey,
    string CustomerName,
    string CustomerStatus,
    DateTimeOffset ChangedAtUtc,
    string RecordSource,
    string HashDiff);

internal sealed record OrderRelationshipEvent(
    string OrderBusinessKey,
    string ProductBusinessKey,
    string ProductName,
    DateTimeOffset CreatedAtUtc,
    string RecordSource);

internal sealed record OrderFulfillmentEvent(
    string AllocationStatus,
    string WarehouseCode,
    DateTimeOffset ChangedAtUtc,
    string RecordSource,
    string HashDiff);
