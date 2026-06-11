using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelBridgeDeclaration(
    string Name,
    string Kind,
    string Source,
    IReadOnlyDictionary<string, DataVaultModelBridgeEndpointDeclaration> Endpoints,
    string Path);
