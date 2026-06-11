using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelLinkParticipantDeclaration(
    string Hub,
    string? Role,
    string Path);
