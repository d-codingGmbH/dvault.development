using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelParentReferenceDeclaration(
    string Kind,
    string Name,
    string Path);
