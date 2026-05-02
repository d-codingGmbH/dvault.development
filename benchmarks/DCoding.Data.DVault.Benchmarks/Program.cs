namespace DCoding.Data.DVault.Benchmarks;

internal static class Program {
  private static async Task<int> Main(string[] args) {
    if (BenchmarkOptions.IsHelpRequested(args)) {
      BenchmarkRunner.WriteUsage();
      return 0;
    }

    try {
      await BenchmarkRunner.RunAsync(BenchmarkOptions.Parse(args), CancellationToken.None).ConfigureAwait(false);
      return 0;
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      Console.Error.WriteLine(exception.Message);
      return 1;
    }
  }
}
