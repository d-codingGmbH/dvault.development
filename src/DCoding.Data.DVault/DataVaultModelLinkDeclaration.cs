using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelLinkDeclaration(
    string Name,
    IReadOnlyList<DataVaultModelLinkParticipantDeclaration> Participants,
    string Path);
