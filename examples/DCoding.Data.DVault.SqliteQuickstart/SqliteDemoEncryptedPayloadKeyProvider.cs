using System.Text;
using DCoding.Data.DVault.Privacy;

namespace DCoding.Data.DVault.SqliteQuickstart;

public sealed class SqliteDemoEncryptedPayloadKeyProvider : IDataVaultEncryptedPayloadKeyProvider {
  public DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
      DataVaultEncryptedPayloadConversionRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return request.Direction switch {
      DataVaultEncryptedPayloadConversionDirection.Encrypt => DataVaultEncryptedPayloadConversionResult.Approved(
          "demo-encrypted:" +
          request.EncryptedPayloadAlias +
          ":" +
          Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Value))),
      DataVaultEncryptedPayloadConversionDirection.Decrypt => Decrypt(request),
      _ => DataVaultEncryptedPayloadConversionResult.Declined("unsupported-conversion-direction"),
    };
  }

  private static DataVaultEncryptedPayloadConversionResult Decrypt(
      DataVaultEncryptedPayloadConversionRequest request) {
    var prefix = "demo-encrypted:" + request.EncryptedPayloadAlias + ":";
    if (!request.Value.StartsWith(prefix, StringComparison.Ordinal)) {
      return DataVaultEncryptedPayloadConversionResult.Declined("alias-mismatch");
    }

    var providerPayload = request.Value[prefix.Length..];
    return DataVaultEncryptedPayloadConversionResult.Approved(
        Encoding.UTF8.GetString(Convert.FromBase64String(providerPayload)));
  }
}
