using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DCoding.Data.DVault;

internal static class DataVaultSupportBundleRedactor {
  private const string RedactedValue = "<redacted>";

  private static readonly Regex KeyValueSecretPattern = new(
      @"(?<prefix>\b(?:password|pwd|passphrase|secret|token|access_token|refresh_token|client_secret|accountkey|account_key|sharedaccesskey|shared_access_key|sas|api_key|apikey|credential|credentials|user id|userid|uid|username)\s*=\s*)(?<value>[^;,]+)",
      RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

  private static readonly Regex UriCredentialPattern = new(
      @"://[^/\s:@]+:[^/\s@]+@",
      RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

  public static void Redact(JsonNode node) {
    ArgumentNullException.ThrowIfNull(node);

    RedactNode(node);
  }

  private static void RedactNode(JsonNode node) {
    if (node is JsonObject jsonObject) {
      foreach (var property in jsonObject.ToArray()) {
        if (property.Value is null) {
          continue;
        }

        if (TryGetString(property.Value, out var value)) {
          jsonObject[property.Key] = RedactString(value);
        }
        else {
          RedactNode(property.Value);
        }
      }

      return;
    }

    if (node is JsonArray jsonArray) {
      for (var index = 0; index < jsonArray.Count; index++) {
        var item = jsonArray[index];
        if (item is null) {
          continue;
        }

        if (TryGetString(item, out var value)) {
          jsonArray[index] = RedactString(value);
        }
        else {
          RedactNode(item);
        }
      }
    }
  }

  private static bool TryGetString(JsonNode node, out string value) {
    if (node is JsonValue jsonValue && jsonValue.TryGetValue<string>(out var stringValue)) {
      value = stringValue;
      return true;
    }

    value = string.Empty;
    return false;
  }

  private static string RedactString(string value) {
    var redacted = KeyValueSecretPattern.Replace(
        value,
        match => match.Groups["prefix"].Value + RedactedValue);
    return UriCredentialPattern.Replace(redacted, "://" + RedactedValue + "@");
  }
}
