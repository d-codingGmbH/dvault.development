using System.Reflection;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

internal readonly record struct DataVaultDbContextMetadataSourceKey(
    string SourceKind,
    string Fingerprint) {
  public static DataVaultDbContextMetadataSourceKey None { get; } = new("<none>", "<none>");
}
