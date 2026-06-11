using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelArtifact(
    string SchemaVersion,
    DataVaultModelArtifactNaming Naming,
    DataVaultLoadTimestampStorage LoadTimestampStorage,
    IReadOnlyList<DataVaultModelHubDeclaration> Hubs,
    IReadOnlyList<DataVaultModelLinkDeclaration> Links,
    IReadOnlyList<DataVaultModelSatelliteDeclaration> Satellites,
    IReadOnlyList<DataVaultModelPitDeclaration> Pits,
    IReadOnlyList<DataVaultModelBridgeDeclaration> Bridges);
