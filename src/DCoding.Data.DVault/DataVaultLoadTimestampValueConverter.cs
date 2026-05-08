using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal static class DataVaultLoadTimestampValueConverter {
  public static object ToProviderValue(
      DbContext dbContext,
      string tableName,
      string columnName,
      DateTimeOffset timestamp) {
    var property = FindProperty(dbContext, tableName, columnName);

    return ToProviderParameterValue(property, timestamp);
  }

  public static object ToProviderValue(IProperty? property, DateTimeOffset timestamp) {
    var valueFormat = property?.FindAnnotation(DataVaultAnnotationNames.ProviderValueFormat)?.Value;
    if (valueFormat is not DataVaultProviderValueFormat providerValueFormat) {
      return timestamp.ToUniversalTime();
    }

    return ToProviderValue(providerValueFormat, property!.ClrType, timestamp);
  }

  public static object ToProviderParameterValue(IProperty? property, DateTimeOffset timestamp) {
    var valueFormat = property?.FindAnnotation(DataVaultAnnotationNames.ProviderValueFormat)?.Value;
    if (valueFormat is not DataVaultProviderValueFormat providerValueFormat) {
      return timestamp.ToUniversalTime();
    }

    return ToProviderParameterValue(providerValueFormat, timestamp);
  }

  public static object ToProviderValue(
      DataVaultProviderValueFormat valueFormat,
      Type? modelClrType,
      DateTimeOffset timestamp) {
    var utcTimestamp = timestamp.ToUniversalTime();

    return valueFormat switch {
      DataVaultProviderValueFormat.UtcTicks => utcTimestamp.UtcDateTime.Ticks,
      DataVaultProviderValueFormat.Iso8601UtcText when modelClrType == typeof(string) => FormatIso8601UtcText(utcTimestamp),
      _ => utcTimestamp,
    };
  }

  public static object ToProviderParameterValue(DataVaultProviderValueFormat valueFormat, DateTimeOffset timestamp) {
    var utcTimestamp = timestamp.ToUniversalTime();

    return valueFormat switch {
      DataVaultProviderValueFormat.UtcTicks => utcTimestamp.UtcDateTime.Ticks,
      DataVaultProviderValueFormat.Iso8601UtcText => FormatIso8601UtcText(utcTimestamp),
      _ => utcTimestamp,
    };
  }

  public static DateTimeOffset ReadProviderValue(object? value) {
    if (TryReadProviderValue(value, out var timestamp)) {
      return timestamp;
    }

    throw new InvalidOperationException("Data Vault load timestamp value is null or has an unsupported storage shape.");
  }

  public static bool TryReadProviderValue(object? value, out DateTimeOffset timestamp) {
    if (value is DateTimeOffset dateTimeOffset) {
      timestamp = dateTimeOffset.ToUniversalTime();
      return true;
    }

    if (value is DateTime dateTime) {
      if (dateTime.Kind == DateTimeKind.Unspecified) {
        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
      }

      timestamp = new DateTimeOffset(dateTime).ToUniversalTime();
      return true;
    }

    if (TryReadTicks(value, out var ticks)) {
      timestamp = new DateTimeOffset(new DateTime(ticks, DateTimeKind.Utc));
      return true;
    }

    if (value is string text &&
        DateTimeOffset.TryParse(
            text,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out timestamp)) {
      timestamp = timestamp.ToUniversalTime();
      return true;
    }

    timestamp = DateTimeOffset.MinValue;
    return false;
  }

  public static string FormatIso8601UtcText(DateTimeOffset timestamp) {
    return timestamp.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
  }

  private static bool TryReadTicks(object? value, out long ticks) {
    switch (value) {
      case long longValue:
        ticks = longValue;
        return true;
      case int intValue:
        ticks = intValue;
        return true;
      case short shortValue:
        ticks = shortValue;
        return true;
      case byte byteValue:
        ticks = byteValue;
        return true;
      case decimal decimalValue when decimalValue == decimal.Truncate(decimalValue) &&
          decimalValue >= long.MinValue &&
          decimalValue <= long.MaxValue:
        ticks = (long)decimalValue;
        return true;
      default:
        ticks = 0;
        return false;
    }
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
