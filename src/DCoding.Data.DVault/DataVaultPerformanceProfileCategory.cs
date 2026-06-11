using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Closed repository-backed performance-profile category used by provider tuning diagnostics.
/// </summary>
public enum DataVaultPerformanceProfileCategory {
  /// <summary>
  /// The checked-in "Small app-local vault" performance profile.
  /// </summary>
  SmallAppLocalVault,

  /// <summary>
  /// The checked-in "Medium chunked ingestion" performance profile.
  /// </summary>
  MediumChunkedIngestion,

  /// <summary>
  /// The checked-in "Staged provider ingestion" performance profile.
  /// </summary>
  StagedProviderIngestion,

  /// <summary>
  /// The checked-in "Read-model heavy" performance profile.
  /// </summary>
  ReadModelHeavy,
}
