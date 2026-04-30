using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed class DefaultStableHashNormalizer : IStableHashNormalizer
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    public static DefaultStableHashNormalizer Instance { get; } = new();

    public string NormalizeValue(object? value)
    {
        return NormalizeValue(value, fieldPath: null);
    }

    public string NormalizeFields(IEnumerable<KeyValuePair<string, object?>> fields)
    {
        ArgumentNullException.ThrowIfNull(fields);

        var normalizedFields = new List<KeyValuePair<string, string>>();
        var fieldPaths = new HashSet<string>(StringComparer.Ordinal);

        foreach (var field in fields)
        {
            var fieldPath = RequireFieldPath(field.Key);
            if (!fieldPaths.Add(fieldPath))
            {
                throw new ArgumentException("Stable hash structured fields must not contain duplicate field paths.", nameof(fields));
            }

            normalizedFields.Add(new KeyValuePair<string, string>(fieldPath, NormalizeValue(field.Value, fieldPath)));
        }

        normalizedFields.Sort((left, right) => string.CompareOrdinal(left.Key, right.Key));

        return string.Join(
            "\n",
            normalizedFields.Select(field => field.Key + "=" + field.Value));
    }

    private static string NormalizeValue(object? value, string? fieldPath)
    {
        return value switch
        {
            null => "n:",
            string stringValue => NormalizeString(stringValue, fieldPath),
            bool boolValue => boolValue ? "b:true" : "b:false",
            byte byteValue => "i:" + byteValue.ToString(CultureInfo.InvariantCulture),
            sbyte sbyteValue => "i:" + sbyteValue.ToString(CultureInfo.InvariantCulture),
            short shortValue => "i:" + shortValue.ToString(CultureInfo.InvariantCulture),
            ushort ushortValue => "i:" + ushortValue.ToString(CultureInfo.InvariantCulture),
            int intValue => "i:" + intValue.ToString(CultureInfo.InvariantCulture),
            uint uintValue => "i:" + uintValue.ToString(CultureInfo.InvariantCulture),
            long longValue => "i:" + longValue.ToString(CultureInfo.InvariantCulture),
            ulong ulongValue => "i:" + ulongValue.ToString(CultureInfo.InvariantCulture),
            decimal decimalValue => "d:" + decimalValue.ToString(CultureInfo.InvariantCulture),
            DateTime dateTimeValue => NormalizeDateTime(dateTimeValue, fieldPath),
            DateTimeOffset dateTimeOffsetValue => NormalizeDateTimeOffset(dateTimeOffsetValue),
            Guid guidValue => "g:" + guidValue.ToString("D", CultureInfo.InvariantCulture).ToLowerInvariant(),
            _ => throw UnsupportedValue(value, fieldPath),
        };
    }

    private static string NormalizeString(string value, string? fieldPath)
    {
        var normalizedText = value
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Normalize(NormalizationForm.FormC);

        try
        {
            return "s:" + Utf8NoBom.GetByteCount(normalizedText).ToString(CultureInfo.InvariantCulture) + ":" + normalizedText;
        }
        catch (EncoderFallbackException exception)
        {
            throw InvalidValue("Stable hash string values must contain valid Unicode scalar text.", fieldPath, exception);
        }
    }

    private static string NormalizeDateTime(DateTime value, string? fieldPath)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw InvalidValue("Stable hash DateTime values must use DateTimeKind.Utc.", fieldPath);
        }

        return "t:" + value.ToString("O", CultureInfo.InvariantCulture);
    }

    private static string NormalizeDateTimeOffset(DateTimeOffset value)
    {
        return "t:" + value.UtcDateTime.ToString("O", CultureInfo.InvariantCulture);
    }

    private static string RequireFieldPath(string fieldPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldPath);

        if (fieldPath.Contains('\n', StringComparison.Ordinal) ||
            fieldPath.Contains('\r', StringComparison.Ordinal) ||
            fieldPath.Contains('=', StringComparison.Ordinal))
        {
            throw new ArgumentException("Stable hash field paths must not contain line breaks or equals signs.", nameof(fieldPath));
        }

        return fieldPath;
    }

    private static Exception UnsupportedValue(object value, string? fieldPath)
    {
        var valueType = value.GetType().FullName ?? value.GetType().Name;
        if (fieldPath is null)
        {
            return new NotSupportedException("Stable hash normalization does not support value type '" + valueType + "'.");
        }

        return new NotSupportedException(
            "Stable hash field '" + fieldPath + "' does not support value type '" + valueType + "'.");
    }

    private static ArgumentException InvalidValue(string message, string? fieldPath, Exception? innerException = null)
    {
        if (fieldPath is null)
        {
            return new ArgumentException(message, innerException);
        }

        return new ArgumentException("Stable hash field '" + fieldPath + "' is invalid. " + message, innerException);
    }
}
