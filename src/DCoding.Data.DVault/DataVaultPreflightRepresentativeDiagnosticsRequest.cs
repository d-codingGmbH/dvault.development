using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one caller-owned representative diagnostics request to evaluate during aggregate preflight.
/// </summary>
public sealed class DataVaultPreflightRepresentativeDiagnosticsRequest {
  /// <summary>
  /// Initializes a new representative diagnostics request.
  /// </summary>
  /// <param name="name">The stable caller-owned name for the representative request.</param>
  /// <param name="createDiagnostics">The caller-owned diagnostics factory that evaluates the configured DbContext.</param>
  public DataVaultPreflightRepresentativeDiagnosticsRequest(
      string name,
      Func<DbContext, DataVaultDiagnosticsResult> createDiagnostics) {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    ArgumentNullException.ThrowIfNull(createDiagnostics);

    Name = name;
    CreateDiagnostics = createDiagnostics;
  }

  /// <summary>
  /// Gets the stable caller-owned name for the representative request.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the caller-owned diagnostics factory that evaluates the configured DbContext.
  /// </summary>
  public Func<DbContext, DataVaultDiagnosticsResult> CreateDiagnostics { get; }
}
