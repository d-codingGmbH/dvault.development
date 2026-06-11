using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation of one provider capability type mapping.
/// </summary>
public sealed record DataVaultProviderTypeMappingExplain(
    DataVaultLogicalPropertyKind LogicalPropertyKind,
    string ModelClrTypeName,
    string StoreType,
    DataVaultProviderValueFormat ValueFormat);
