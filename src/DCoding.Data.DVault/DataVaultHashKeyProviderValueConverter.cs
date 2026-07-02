using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal static class DataVaultHashKeyProviderValueConverter {
  public static object ToProviderParameterValue(
      DbContext dbContext,
      string tableName,
      string columnName,
      object value) {
    var property = FindProperty(dbContext, tableName, columnName);

    return ToProviderParameterValue(property, value);
  }

  public static object ToProviderParameterValue(IProperty? property, object value) {
    if (!UsesBinaryHashKeyStorage(property)) {
      return value;
    }

    return value switch {
      string text => ConvertCanonicalHexToBytes(text, GetDigestByteLength(property!)),
      byte[] => value,
      _ => throw new FormatException(
          "Data Vault binary hash-key conversion requires canonical lowercase hexadecimal string model values."),
    };
  }

  public static object ReadProviderValue(
      DbContext dbContext,
      string tableName,
      string columnName,
      object value) {
    var property = FindProperty(dbContext, tableName, columnName);

    return ReadProviderValue(property, value);
  }

  public static object ReadProviderValue(IProperty? property, object value) {
    if (!UsesBinaryHashKeyStorage(property)) {
      return value;
    }

    return value switch {
      byte[] bytes => ConvertBytesToCanonicalHex(bytes, GetDigestByteLength(property!)),
      _ => throw new FormatException(
          "Data Vault binary hash-key conversion expected provider bytes for the active stable hash digest."),
    };
  }

  public static byte[] ConvertCanonicalHexToBytes(string value, int digestByteLength) {
    ArgumentNullException.ThrowIfNull(value);

    var expectedHexLength = digestByteLength * 2;
    if (value.Length != expectedHexLength) {
      throw new FormatException(
          "Data Vault binary hash-key conversion expected " +
          expectedHexLength.ToString(CultureInfo.InvariantCulture) +
          " lowercase hexadecimal characters for a " +
          digestByteLength.ToString(CultureInfo.InvariantCulture) +
          "-byte stable hash digest.");
    }

    for (var index = 0; index < value.Length; index++) {
      var character = value[index];
      var isLowercaseHex =
          character is >= '0' and <= '9' ||
          character is >= 'a' and <= 'f';
      if (!isLowercaseHex) {
        throw new FormatException(
            "Data Vault binary hash-key conversion requires canonical lowercase hexadecimal values without prefixes.");
      }
    }

    var bytes = new byte[digestByteLength];
    for (var index = 0; index < bytes.Length; index++) {
      bytes[index] = (byte)((ReadLowerHexNibble(value[index * 2]) << 4) | ReadLowerHexNibble(value[(index * 2) + 1]));
    }

    return bytes;
  }

  public static string ConvertBytesToCanonicalHex(byte[] value, int digestByteLength) {
    ArgumentNullException.ThrowIfNull(value);

    if (value.Length != digestByteLength) {
      throw new FormatException(
          "Data Vault binary hash-key conversion expected " +
          digestByteLength.ToString(CultureInfo.InvariantCulture) +
          " provider bytes for the active stable hash digest.");
    }

    return string.Create(
        digestByteLength * 2,
        value,
        static (chars, bytes) => {
          const string LowerHexDigits = "0123456789abcdef";

          for (var index = 0; index < bytes.Length; index++) {
            var current = bytes[index];
            chars[index * 2] = LowerHexDigits[current >> 4];
            chars[(index * 2) + 1] = LowerHexDigits[current & 0x0f];
          }
        });
  }

  private static int ReadLowerHexNibble(char character) {
    if (character is >= '0' and <= '9') {
      return character - '0';
    }

    if (character is >= 'a' and <= 'f') {
      return character - 'a' + 10;
    }

    throw new FormatException(
        "Data Vault binary hash-key conversion requires canonical lowercase hexadecimal values without prefixes.");
  }

  private static bool UsesBinaryHashKeyStorage(IProperty? property) {
    return property?.FindAnnotation(DataVaultAnnotationNames.ProviderValueFormat)?.Value is
        DataVaultProviderValueFormat.LowercaseHexBinary;
  }

  private static int GetDigestByteLength(IProperty property) {
    if (property.FindAnnotation(DataVaultAnnotationNames.StableHashDigestByteLength)?.Value is int digestByteLength &&
        digestByteLength > 0) {
      return digestByteLength;
    }

    throw new InvalidOperationException(
        "Binary hash-key conversion requires a declared stable-hash digest byte length.");
  }

  private static IProperty? FindProperty(DbContext dbContext, string tableName, string columnName) {
    return FindEntityType(dbContext, tableName)?.FindProperty(columnName);
  }

  private static IEntityType? FindEntityType(DbContext dbContext, string tableName) {
    try {
      return dbContext.Model.GetEntityTypes().FirstOrDefault(entity =>
          string.Equals(entity.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string, tableName, StringComparison.Ordinal) ||
          string.Equals(entity.GetTableName(), tableName, StringComparison.Ordinal) ||
          string.Equals(entity.Name, tableName, StringComparison.Ordinal));
    }
    catch (InvalidOperationException) {
      return null;
    }
  }
}
