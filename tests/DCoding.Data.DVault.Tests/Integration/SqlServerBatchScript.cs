using System.Text;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class SqlServerBatchScript {
  public static IReadOnlyList<string> SplitBatches(string script) {
    ArgumentNullException.ThrowIfNull(script);

    var batches = new List<string>();
    var batch = new StringBuilder();
    using var reader = new StringReader(script);

    while (reader.ReadLine() is { } line) {
      if (string.Equals(line.Trim(), "GO", StringComparison.OrdinalIgnoreCase)) {
        AddBatch(batches, batch);
        batch.Clear();
        continue;
      }

      batch.AppendLine(line);
    }

    AddBatch(batches, batch);

    return batches;
  }

  private static void AddBatch(List<string> batches, StringBuilder batch) {
    var batchText = batch.ToString().Trim();
    if (batchText.Length > 0) {
      batches.Add(batchText);
    }
  }
}
