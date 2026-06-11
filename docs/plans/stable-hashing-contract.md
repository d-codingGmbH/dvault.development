# Stable Hashing Contract

Status: v1 design contract
Ticket: 06EXB76DNVSRBD12T4W03AWQZC
Milestone: Foundation and architecture

## Purpose

Stable hashes identify normalized modeling and data values across repeated runs, machines, and runtime versions. They are deterministic data identity values, not a security boundary.

This contract is not a password hashing policy, encryption design, message authentication code, digital signature, key-management scheme, or secret-rotation framework. Callers that need those behaviors must use a separate security-specific component.

## Public Contract Responsibilities

The public boundary is a small hash service abstraction plus a replaceable registration point. When source roots are introduced, the .NET-facing shape should stay equivalent to:

- `IStableHashService.AlgorithmId`: stable, non-empty, versioned identifier for the implementation, digest algorithm, truncation policy, and compatibility version.
- `IStableHashService.ComputeHash(string normalizedInput)`: hashes a normalized text payload and returns the algorithm identifier plus the digest value.
- `StableHashDigest.AlgorithmId`: copied from the service that produced the digest.
- `StableHashDigest.Value`: canonical lowercase hexadecimal digest text without a prefix.
- `StableHashDigest.DigestByteLength`: the byte length represented by the canonical hexadecimal value.

`AlgorithmId` is authoritative for digest semantics. Equal `AlgorithmId` values and equal normalized input bytes must produce equal digest bytes and equal canonical hexadecimal text. Different algorithms, truncation lengths, or compatibility versions must use different `AlgorithmId` values and must not masquerade as `sha256-v1`.

Digest text is serialized as lowercase hexadecimal without a `0x` prefix. Validation is algorithm-aware: `sha256-v1` remains exactly 32 digest bytes and 64 hexadecimal characters, while approved non-default ids may use shorter digest lengths. Unknown caller-supplied algorithm ids are accepted only when the digest is whole-byte lowercase hexadecimal text; the caller owns the stability and collision profile for that custom id.

The hash service consumes already-normalized text. Model-specific code is responsible for turning domain values into the canonical text described below before calling the service. This keeps domain field choices out of the shared hashing service while still making the digest algorithm replaceable.

Required behavior:

- `normalizedInput` must be a .NET string that represents the exact canonical text to hash.
- Empty normalized input is valid and hashes the zero-length UTF-8 byte sequence.
- A null input object is invalid and must fail fast with `ArgumentNullException`.
- A model normalizer that receives an unsupported value type must fail with `NotSupportedException` and include the field path or value type in the diagnostic message.
- A model normalizer that receives a supported type in an invalid state, such as a non-finite floating point value, must fail before hashing with `ArgumentOutOfRangeException` or `ArgumentException`.
- The service must not silently skip fields, coerce unsupported values, use current culture, or fall back to platform-default encoding.

## Default v1 Implementation

Default algorithm:

- `AlgorithmId`: `sha256-v1`
- Digest algorithm: SHA-256
- Input bytes: UTF-8 encoding of `normalizedInput`, without a byte order mark
- Output: 32 digest bytes serialized as 64 lowercase hexadecimal characters
- Determinism: identical normalized input bytes always produce identical `AlgorithmId` and `Value`

The implementation must not use process-local salts, random values, timestamps, culture-specific formatting, machine identifiers, current directory values, serializer defaults, dictionary iteration order, or any other platform-specific side effects.

## Non-Default Algorithm Id Candidates

The following ids define the bounded v1 candidate set for callers that deliberately replace the default service. They are not registered by `AddDVault()` and are not enabled automatically by model configuration.

| AlgorithmId | Digest algorithm and truncation | Digest bytes | Hex characters | Status |
| --- | --- | ---: | ---: | --- |
| `sha256-v1` | SHA-256, full digest | 32 | 64 | Default registered compatibility baseline |
| `sha1-v1` | SHA-1, full digest | 20 | 40 | Non-default opt-in candidate |
| `sha256-128-v1` | SHA-256 truncated to the first 128 bits | 16 | 32 | Non-default opt-in candidate |
| `sha256-160-v1` | SHA-256 truncated to the first 160 bits | 20 | 40 | Non-default opt-in candidate |

SHA-1 and truncated SHA-256 ids are deterministic identity trade-offs for non-adversarial Data Vault key hashing only. They are not password hashing, message authentication, signature, encryption, or adversarial collision-defense policies. A caller that has regulatory, cryptographic, or adversarial-collision requirements must use a separate security-specific component and must not cite these ids as a compliance control.

The truncated SHA-256 candidates use the leading bytes of the `sha256-v1` digest for the same normalized input, then serialize those bytes as canonical lowercase hexadecimal text. They must use their own `AlgorithmId` values and must never be stored, compared, or advertised as `sha256-v1`.

## Normalization Rules

Normalization happens before bytes are passed to the digest algorithm. The shared contract defines the canonical text rules; later domain-specific tickets decide which entity fields participate in a given model hash.

General rules:

- Normalize string values to Unicode normalization form C before length calculation or hashing.
- Preserve case, leading and trailing whitespace, and internal whitespace unless a domain-specific contract explicitly says otherwise.
- Convert CRLF and CR line endings in text values to LF (`\n`) before length calculation.
- Use invariant culture for every formatted value.
- Use ASCII type tags so null, empty string, and textual sentinel values cannot collide.
- Use LF (`\n`) between structured fields and no trailing LF in the normalized input used for the test vectors below.

Stable scalar encodings:

- Null: `n:`
- String: `s:<utf8-byte-count>:<normalized-text>`
- Boolean: `b:true` or `b:false`
- Integer: `i:<base-10-digits>` with `-` only for negative values and no grouping separators
- Decimal: `d:<canonical-decimal>` using invariant digits and `.` as the separator; callers must define the domain scale before hashing persisted decimal values
- Timestamp: `t:<utc-roundtrip>` in UTC with the round-trip pattern, for example `2026-04-28T00:00:00.0000000Z`
- Guid: `g:<lowercase-d-format-guid>`

Structured values:

- Represent a structured value as one field per line: `<field-path>=<stable-scalar-encoding>`.
- Sort field paths by ordinal string comparison before joining lines.
- Include fields with null values using `n:`; do not omit them.
- Treat missing fields as invalid unless the domain-specific contract defines an explicit absent-field encoding.
- Do not serialize arbitrary objects, dictionaries, or records directly through a general serializer for hashing. Map the fields deliberately, then apply the ordering rule.

Example canonical structured input:

```text
active=b:true
name=s:5:Alice
nickname=n:
score=i:42
```

## Replacement and Registration

Consumers must obtain the hash service through options or dependency injection instead of constructing the default implementation inside model code.

Expected registration shape when application source exists:

- Register `IStableHashService` as the public service dependency.
- Register the default `sha256-v1` implementation when no caller override is supplied.
- Allow options to provide an alternate `IStableHashService` instance or factory.
- Keep normalization rules independent from the concrete hash service so a replacement receives the same canonical text.

Replacement rules:

- A replacement implementation must expose a stable `AlgorithmId`.
- If it is compatible with the default implementation, it must produce the same lowercase SHA-256 digest for the same normalized input and use `sha256-v1`.
- If it intentionally produces different digest values, it must use a different `AlgorithmId`.
- It must preserve deterministic behavior across repeated runs and must return canonical lowercase hexadecimal digest text whose byte length matches the selected `AlgorithmId`.
- Model code must depend only on the abstraction and must not branch on the concrete implementation type.

## Test Vectors

All default vectors use `AlgorithmId` `sha256-v1`, UTF-8 input bytes, and lowercase SHA-256 output.

| Case | Normalized input | Expected digest |
| --- | --- | --- |
| Empty service input | empty string | `e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855` |
| Empty string stable value | `s:0:` | `68531113e40fffcea6caa4b72302c47015bb82b9e9ff2ceb9f2c6953e5f9a2b0` |
| Null stable value | `n:` | `1f8dc03d51e3ddcc59b608508bba5c34aecac15f1b250390a629a4231ad80a9a` |
| Repeated deterministic text | `s:21:dvault:stable-hash:v1` | `eb99c3da5f4b0e5f6137357a0134b1d8d92133d1137ebe0606daae281a6a4281` |
| Ordered structured value with null | `active=b:true\nname=s:5:Alice\nnickname=n:\nscore=i:42` | `d2fb098dce221d02fc6561aabacfe9418c331fd576b3518c01e70cf6ba7ea115` |
| Culture-invariant decimal and timestamp | `amount=d:1234.50\ntimestamp=t:2026-04-28T00:00:00.0000000Z` | `1a84b2aacf8d30fe82e26bf2c21e2948a9ebf43780e6667718191c5ef8abb83a` |

Implementation tests derived from these vectors should assert:

- Hashing the same normalized input twice returns the same digest and algorithm identifier.
- Empty input and null stable values are distinct and match their vectors.
- Structured input produces the same normalized text and digest regardless of the source object's field or dictionary iteration order.
- Culture-sensitive values do not change under a non-invariant current culture.
- Invalid or unsupported values fail before hashing.

Alternate implementation substitution example:

- Configure a test implementation through options or dependency injection with `AlgorithmId` `test-double-v1`.
- For normalized input `s:21:dvault:stable-hash:v1`, the test implementation returns digest `00000000000000000000000000000001`.
- A consuming model test must observe `AlgorithmId` `test-double-v1` and the configured digest without changing model code.

## Compatibility Notes

Persisted hashes should store or otherwise retain the algorithm identifier once storage is introduced. This ticket does not define a migration framework, but changing the default algorithm, scalar encodings, field ordering, or culture formatting after hashes are persisted will require compatibility work.

Adopting a different algorithm id or truncation policy after hub keys, link keys, hash diffs, or other stable-hash values are persisted is caller-owned compatibility work. DVault does not automatically rehash, backfill, migrate, or reconcile existing stored values when callers replace the stable hash service.

Stable model and key hashing remain separate from the persistence content hash tuple contract. `DataVaultConventions.PersistenceContentHashAlgorithm` continues to describe the storage-integrity policy value `sha-256`, and the persistence `content_hash` value remains fixed to the storage policy's lowercase SHA-256 shape. The non-default stable-hash ids above do not weaken or broaden `content_hash` storage semantics.

Domain-specific entity hashing remains out of scope for this contract. Future entity tickets should reference this document, select their participating fields explicitly, and add focused vectors for those entity-specific canonical inputs.
