using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed record DataVaultProviderIdentifierProjection(
    DataVaultProviderIdentifierCandidate Candidate,
    string PhysicalName,
    bool IsDerived);
