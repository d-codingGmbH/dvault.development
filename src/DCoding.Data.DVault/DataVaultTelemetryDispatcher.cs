using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultTelemetryDispatcher {
  public static IReadOnlyList<IDataVaultTelemetryObserver> CreateObservers(IEnumerable<IDataVaultTelemetryObserver>? observers) {
    return observers?.ToArray() ?? Array.Empty<IDataVaultTelemetryObserver>();
  }

  public static void RecordSave(
      IReadOnlyList<IDataVaultTelemetryObserver> observers,
      DataVaultSaveTelemetrySummary summary) {
    foreach (var observer in observers) {
      try {
        observer.RecordSave(summary);
      }
      catch (Exception) {
      }
    }
  }

  public static void RecordRead(
      IReadOnlyList<IDataVaultTelemetryObserver> observers,
      DataVaultReadTelemetrySummary summary) {
    foreach (var observer in observers) {
      try {
        observer.RecordRead(summary);
      }
      catch (Exception) {
      }
    }
  }
}
