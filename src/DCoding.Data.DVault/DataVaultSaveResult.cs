using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Summarizes rows persisted by an explicit DVault save request.
/// </summary>
public sealed class DataVaultSaveResult {
  /// <summary>
  /// Initializes a new save result.
  /// </summary>
  /// <param name="rowsWritten">The row count inserted by the explicit service invocation.</param>
  /// <param name="savedRecords">The generated hub, link, and satellite hash-key summaries.</param>
  public DataVaultSaveResult(int rowsWritten, IEnumerable<DataVaultSavedRecord> savedRecords) {
    ArgumentNullException.ThrowIfNull(savedRecords);

    RowsWritten = rowsWritten;
    SavedRecords = savedRecords.ToArray();
  }

  /// <summary>
  /// Gets the row count inserted by the explicit service invocation.
  /// </summary>
  public int RowsWritten { get; }

  /// <summary>
  /// Gets the generated hub, link, and satellite hash-key summaries.
  /// </summary>
  public IReadOnlyList<DataVaultSavedRecord> SavedRecords { get; }
}
