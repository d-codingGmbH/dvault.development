using System.Collections.ObjectModel;
using System.Text;

namespace DCoding.Data.DVault.Modeling;

internal static class InvariantStringExtensions {
  public static string ToStringInvariant(this int value) {
    return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
  }
}
