using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelPitDeclaration(
    string Name,
    string Hub,
    IReadOnlyList<string> Satellites,
    string Path);
