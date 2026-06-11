namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes a link table name request.
/// </summary>
public sealed record DataVaultLinkNameContext(string? RelationshipName, IReadOnlyList<string> ParticipantNames);
