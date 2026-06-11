using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation of one translated Data Vault key.
/// </summary>
public sealed record DataVaultKeyExplain(
    string Name,
    IReadOnlyList<string> PropertyNames) {
  /// <summary>
  /// Gets the provider-neutral produced name preserved on the EF metadata item.
  /// </summary>
  public string ProducedName { get; init; } = string.Empty;
}
