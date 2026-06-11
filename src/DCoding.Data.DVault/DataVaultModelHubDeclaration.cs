using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelHubDeclaration(
    string Name,
    IReadOnlyList<string> BusinessKeys,
    string Path);
