using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record CustomerProfileBulkScenarioDefinition(
    string ScenarioName,
    string DatasetSize,
    string ChangeRatio,
    int CustomerCount,
    int ChangeCount,
    int SampleCustomerIndex,
    string RecordSource,
    DateTimeOffset BaseTimestamp) {
  public int TotalChangeCount => CustomerCount * ChangeCount;

  public IEnumerable<CustomerProfileBulkEvent> CreateEvents() {
    return Enumerable.Range(0, CustomerCount)
        .SelectMany(customerIndex => Enumerable.Range(0, ChangeCount)
            .Select(changeIndex => CreateEvent(customerIndex, changeIndex)));
  }

  public CustomerProfileBulkEvent CreateEvent(int customerIndex, int changeIndex) {
    var customerBusinessKey = CreateBusinessKey(customerIndex);
    var customerNumber = customerIndex.ToString("0000", CultureInfo.InvariantCulture);
    var changeNumber = changeIndex.ToString("00", CultureInfo.InvariantCulture);

    return new CustomerProfileBulkEvent(
        customerBusinessKey,
        "Customer " + customerNumber + " v" + changeNumber,
        changeIndex == 0 ? "prospect" : "active",
        BaseTimestamp.AddMinutes(changeIndex),
        RecordSource,
        "profile-" + customerNumber + "-" + changeNumber);
  }

  public string CreateBusinessKey(int customerIndex) {
    return "C-BULK-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture);
  }
}
