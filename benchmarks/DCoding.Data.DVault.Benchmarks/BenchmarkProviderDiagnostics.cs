using System.Data.Common;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkProviderDiagnostics {
  public static string NormalizeExceptionMessage(Exception exception) {
    var message = exception.GetBaseException().Message
        .Replace("\r", " ", StringComparison.Ordinal)
        .Replace("\n", " ", StringComparison.Ordinal)
        .Trim();

    const int maximumMessageLength = 240;
    if (message.Length <= maximumMessageLength) {
      return message;
    }

    return message[..maximumMessageLength] + "...";
  }
}
