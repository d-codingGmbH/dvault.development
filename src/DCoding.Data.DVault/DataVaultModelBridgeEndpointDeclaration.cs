using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelBridgeEndpointDeclaration(
    string Hub,
    string? Role,
    string Path);
