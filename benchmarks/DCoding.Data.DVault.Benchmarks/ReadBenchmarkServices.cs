using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class ReadBenchmarkServices {
  public static ServiceProvider CreateProvider(DataVaultBenchmarkStrategy strategy) {
    return CreateProvider(strategy, BenchmarkHashKeyVariant.Default);
  }

  public static ServiceProvider CreateProvider(
      DataVaultBenchmarkStrategy strategy,
      BenchmarkHashKeyVariant hashKeyVariant) {
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, strategy, hashKeyVariant);

    return services.BuildServiceProvider(validateScopes: true);
  }

  public static void AssertReadStrategySelection(
      DataVaultBenchmarkStrategy strategy,
      string scenarioName,
      DataVaultDiagnosticsResult diagnostics) {
    var expectedStrategyName = DataVaultBenchmarkHelpers.GetProviderReadStrategyName(strategy, scenarioName);
    if (expectedStrategyName is not null) {
      DataVaultBenchmarkHelpers.AssertProviderReadStrategySelected(diagnostics, expectedStrategyName);
    }
  }

  public static async Task<IReadOnlyList<string>> SeedCustomerProfileHistoryAsync<TContext>(
      DbContextOptions<TContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      CustomerProfileBulkScenarioDefinition scenario,
      CancellationToken cancellationToken)
      where TContext : CustomerProfileReadContext {
    await using var context = (TContext)Activator.CreateInstance(
        typeof(TContext),
        options,
        providerCapabilities)!;
    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            scenario.BaseTimestamp,
            scenario.RecordSource,
            Enumerable.Range(0, scenario.CustomerCount)
                .Select(customerIndex => new DataVaultHubSaveOperation(
                    ScenarioContracts.CustomerHub,
                    [new("Customer Id", scenario.CreateBusinessKey(customerIndex))]))
                .ToArray(),
            []),
        cancellationToken).ConfigureAwait(false);
    var customerHashKeys = hubResult.SavedRecords
        .Select((record, customerIndex) => new {
          CustomerIndex = customerIndex,
          record.HashKey,
        })
        .OrderBy(value => value.CustomerIndex)
        .Select(value => value.HashKey)
        .ToArray();
    var satelliteRequests = Enumerable.Range(0, scenario.ChangeCount)
        .Select(changeIndex => new DataVaultSaveRequest(
            scenario.BaseTimestamp.AddMinutes(changeIndex),
            scenario.RecordSource,
            [],
            [],
            Enumerable.Range(0, scenario.CustomerCount)
                .Select(customerIndex => {
                  var customerProfileEvent = scenario.CreateEvent(customerIndex, changeIndex);
                  return new DataVaultSatelliteSaveOperation(
                      ScenarioContracts.CustomerProfileSatellite,
                      customerHashKeys[customerIndex],
                      [
                          new("customer_name", customerProfileEvent.CustomerName),
                          new("customer_status", customerProfileEvent.CustomerStatus),
                      ],
                      customerProfileEvent.HashDiff);
                })
                .ToArray()))
        .ToArray();

    DataVaultBenchmarkHelpers.AssertStableHashKey(
        customerHashKeys[scenario.SampleCustomerIndex],
        providerCapabilities,
        "Seeded read benchmark customer hash key must use the active stable-hash shape.");

    await saveService
        .SaveAsync(context, new DataVaultBulkSaveRequest(satelliteRequests), cancellationToken)
        .ConfigureAwait(false);

    return customerHashKeys;
  }
}
