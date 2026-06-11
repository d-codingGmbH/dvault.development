using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation of one Data Vault parent metadata reference.
/// </summary>
public sealed record DataVaultParentReferenceExplain(
    DataVaultMetadataReferenceKind Kind,
    string Name);
