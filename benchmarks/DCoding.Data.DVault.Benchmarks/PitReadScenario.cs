using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class PitReadScenario {
  public static readonly DateTimeOffset PitTimestamp = new(2026, 4, 29, 10, 11, 0, TimeSpan.Zero);
  public static readonly DateTimeOffset AsOf = new(2026, 4, 29, 10, 12, 0, TimeSpan.Zero);

  public static PitReadMetadata Metadata { get; } = CreateMetadata();

  private static PitReadMetadata CreateMetadata() {
    var status = new DataVaultSatelliteMetadata(
        "Status",
        ScenarioContracts.CustomerHub.ToReference(),
        ["status_code"]);
    var pit = new DataVaultPitMetadata(
        ScenarioContracts.CustomerHub.ToReference(),
        [ScenarioContracts.CustomerProfileSatellite.Name, status.Name]);
    var model = new DataVaultMetadataModel(
        [ScenarioContracts.CustomerHub],
        [],
        [ScenarioContracts.CustomerProfileSatellite, status],
        [pit]);

    return new PitReadMetadata(status, pit, model);
  }
}
