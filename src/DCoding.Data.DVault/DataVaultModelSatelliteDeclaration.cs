using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelSatelliteDeclaration(
    string Name,
    DataVaultModelParentReferenceDeclaration Parent,
    IReadOnlyList<string> Payload,
    IReadOnlyList<string> DrivingKeys,
    string Path);
