using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BridgeReadScenario {
  public static BridgeReadMetadata Metadata { get; } = CreateMetadata();

  private static BridgeReadMetadata CreateMetadata() {
    var bridge = DataVaultBridgeMetadata.Hierarchy(
        "SalesRegionHierarchy",
        DataVaultMetadataReference.Hub("SalesRegion"),
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        DataVaultMetadataReference.Hub("SalesRegion"),
        ancestorParticipantOrdinal: 0,
        descendantParticipantOrdinal: 1);

    return new BridgeReadMetadata(bridge);
  }
}
