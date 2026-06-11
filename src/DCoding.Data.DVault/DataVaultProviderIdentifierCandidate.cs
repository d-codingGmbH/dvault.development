using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed record DataVaultProviderIdentifierCandidate(
    DataVaultProviderIdentifierKind Kind,
    string LogicalName,
    string? MetadataName,
    string Scope,
    string Path);
