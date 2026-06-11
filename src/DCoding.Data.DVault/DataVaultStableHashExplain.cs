using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable stable-hash compatibility metadata for the active hash service.
/// </summary>
public sealed record DataVaultStableHashExplain(
    string AlgorithmId,
    int DigestByteLength,
    string DigestEncoding);
