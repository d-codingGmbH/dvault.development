using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkHashKeyFootprintRow(
    string Variant,
    string Provider,
    string StableHashAlgorithmId,
    int DigestByteLength,
    int HexCharacterLength,
    string StorageProfile,
    string HashKeyStoreType,
    string ParticipantReferenceStoreType,
    string HashKeyValueFormat,
    string ParticipantReferenceValueFormat,
    int HashKeyPayloadBytes,
    int ParentHashReferencePayloadBytes,
    int TwoColumnHashReferenceIndexPayloadBytes,
    int CompletedRows,
    int SkippedRows,
    int FailedRows);
