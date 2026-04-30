namespace DCoding.Data.DVault;

/// <summary>
/// Produces canonical stable-hash text for supported scalar values and structured fields.
/// </summary>
public interface IStableHashNormalizer
{
    /// <summary>
    /// Normalizes one supported scalar value to its canonical stable-hash text representation.
    /// </summary>
    /// <param name="value">The scalar value to normalize.</param>
    /// <returns>The canonical stable-hash text representation.</returns>
    string NormalizeValue(object? value);

    /// <summary>
    /// Normalizes structured fields to sorted canonical stable-hash text lines.
    /// </summary>
    /// <param name="fields">The deliberately mapped field paths and scalar values to normalize.</param>
    /// <returns>The canonical structured text with fields sorted by ordinal path and joined by line feed.</returns>
    string NormalizeFields(IEnumerable<KeyValuePair<string, object?>> fields);
}
