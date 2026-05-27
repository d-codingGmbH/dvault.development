using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal static class DataVaultPitMaintenanceShapeValidator {
  public static void ValidateSupportedShape(DataVaultPitMetadata pit) {
    ArgumentNullException.ThrowIfNull(pit);

    if (pit.Parent.Kind != DataVaultMetadataReferenceKind.Hub &&
        pit.Parent.Kind != DataVaultMetadataReferenceKind.Link) {
      throw PitMaintenanceFailure(
          pit.Name,
          "declares parent '" + pit.Parent.Name + "' as " + pit.Parent.Kind +
          "; supported PIT maintenance requires a hub or link parent");
    }

    if (pit.Satellites.Count == 0) {
      throw PitMaintenanceFailure(pit.Name, "must declare at least one attached satellite");
    }

    var satelliteNames = new HashSet<string>(StringComparer.Ordinal);
    foreach (var satelliteReference in pit.Satellites) {
      if (!satelliteNames.Add(satelliteReference.SatelliteName)) {
        throw PitMaintenanceFailure(
            pit.Name,
            "declares duplicate satellite reference '" + satelliteReference.SatelliteName + "'");
      }

    }
  }

  private static InvalidOperationException PitMaintenanceFailure(string pitName, string detail) {
    return new InvalidOperationException("PIT metadata '" + pitName + "' " + detail + ".");
  }
}
