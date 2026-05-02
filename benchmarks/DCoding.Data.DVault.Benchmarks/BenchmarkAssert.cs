namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkAssert {
  public static void Equal<T>(T expected, T actual, string description) {
    if (!EqualityComparer<T>.Default.Equals(expected, actual)) {
      throw new InvalidOperationException(
          description + " Expected '" + expected + "' but found '" + actual + "'.");
    }
  }

  public static void True(bool condition, string description) {
    if (!condition) {
      throw new InvalidOperationException(description);
    }
  }

  public static T Single<T>(IEnumerable<T> values, string description) {
    var materializedValues = values.ToArray();
    if (materializedValues.Length != 1) {
      throw new InvalidOperationException(
          description + " Expected exactly one row but found " + materializedValues.Length + ".");
    }

    return materializedValues[0];
  }
}
