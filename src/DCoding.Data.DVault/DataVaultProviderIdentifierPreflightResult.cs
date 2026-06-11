using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed record DataVaultProviderIdentifierPreflightResult(
    DataVaultProviderIdentifierProjectionSet ProjectionSet,
    IReadOnlyList<DataVaultProviderIdentifierPreflightIssue> Issues);
