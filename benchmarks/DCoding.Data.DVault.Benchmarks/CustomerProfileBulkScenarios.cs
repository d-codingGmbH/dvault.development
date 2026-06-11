using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class CustomerProfileBulkScenarios {
  private const int CustomerCount = 100;
  private const int SampleCustomerIndex = 42;
  private static readonly DateTimeOffset BaseTimestamp = new(2026, 4, 29, 10, 0, 0, TimeSpan.Zero);
  private static readonly int[] ScaleCustomerCounts = [10, 100, 1000, 10000];

  public static readonly CustomerProfileBulkScenarioDefinition InsertOnly = new(
      "customer-profile-bulk-insert-only",
      "100 customers, 1 profile state each",
      "0% repeat-change history",
      CustomerCount,
      1,
      SampleCustomerIndex,
      "bulk-insert-benchmark",
      BaseTimestamp);

  public static readonly CustomerProfileBulkScenarioDefinition ChangeHeavy = new(
      "customer-profile-bulk-history",
      "100 customers, 10 profile states each",
      "90% repeat-change history",
      CustomerCount,
      10,
      SampleCustomerIndex,
      "bulk-history-benchmark",
      BaseTimestamp);

  public static IReadOnlyList<CustomerProfileBulkScenarioDefinition> ScaleMatrix { get; } =
  [
      .. ScaleCustomerCounts.Select(customerCount => CreateScale(customerCount, changeCount: 1)),
      .. ScaleCustomerCounts.Select(customerCount => CreateScale(customerCount, changeCount: 10)),
  ];

  private static CustomerProfileBulkScenarioDefinition CreateScale(int customerCount, int changeCount) {
    var changeRatio = changeCount == 1
        ? "0% repeat-change history"
        : (((changeCount - 1) * 100) / changeCount).ToString(CultureInfo.InvariantCulture) + "% repeat-change history";

    return new CustomerProfileBulkScenarioDefinition(
        "customer-profile-scale-" +
        customerCount.ToString(CultureInfo.InvariantCulture) +
        "x" +
        changeCount.ToString(CultureInfo.InvariantCulture),
        customerCount.ToString(CultureInfo.InvariantCulture) +
        " customers, " +
        changeCount.ToString(CultureInfo.InvariantCulture) +
        " profile state" +
        (changeCount == 1 ? string.Empty : "s") +
        " each",
        changeRatio,
        customerCount,
        changeCount,
        Math.Min(SampleCustomerIndex, customerCount - 1),
        "scale-" +
        customerCount.ToString(CultureInfo.InvariantCulture) +
        "x" +
        changeCount.ToString(CultureInfo.InvariantCulture) +
        "-benchmark",
        BaseTimestamp);
  }
}
