[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is explicit, `## Open Questions` is `none`, and repository evidence already shows the manifest, vectors, tests, and shared hash-key integration points this story is meant to ratify.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract in `.gicket/tickets/06F5Q934MSKVCQAHPCWEM29CZW/description.md:27-57` defines 4 acceptance criteria, 4 DoD items, and `## Open Questions` = `none` at lines 45-46.
- The authoritative manifest already exists at `docs/plans/stable-hashing-contract.md:33-135`, documenting `sha256-v1`, UTF-8 without BOM, lowercase hex output, NFC normalization, LF line endings, invariant formatting, ordinal structured ordering, null encoding, and published compatibility vectors at lines 108-115.
- Regression coverage already exists in `tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs:24-180` and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs:9-109`, including field-order independence, culture independence, invalid field paths, unsupported `byte[]`, invalid values, null handling, and UTF-8 no-BOM hashing.
- Default DI registration exposes overridable `IStableHashService` and `IStableHashNormalizer` in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-33`; override-preservation tests exist at `StableHashServiceTests.cs:85-100` and `StableHashNormalizerTests.cs:183-195`.
- Shared hub/link hash-key computation uses `_stableHashNormalizer.NormalizeFields(...)` and `_stableHashService.ComputeHash(...)` in `src/DCoding.Data.DVault/DataVaultSaveService.cs:<redacted>`, `<redacted>`, and `<redacted>`.
- Satellite saves still persist caller-supplied `operation.HashDiff` rather than deriving it from payload fields in `src/DCoding.Data.DVault/DataVaultSaveService.cs:<redacted>`, matching the refined scope-out.
- Branch history shows no docs/src/tests changes for this ticket handoff: `git diff --name-only f51dcdd1fef5ce9001e2a0e7d99ab07d020ed20f..3b28aca1a3e0dfcc94d449cac0e524c9454e857f` returns only `.gicket/tickets/06F5Q934MSKVCQAHPCWEM29CZW/**` paths.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The story title says `Add`, but the observed branch contains only ticket metadata changes; approval assumes developer handoff is for ratifying/protecting the existing manifest/tests rather than creating a new manifest file.
- The contract centers shared hashing on the existing normalizer/service path; any provider-native or future hash-diff producer will still need to honor `docs/plans/stable-hashing-contract.md` even if it uses a different execution path.

AC / test suggestions
- If development touches the vector set, keep `docs/plans/stable-hashing-contract.md:108-115` and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs:9-29` byte-for-byte aligned to avoid doc/test drift.
- A non-blocking future enhancement would be a published example for NFC-plus-CRLF normalization or explicit `DateTimeOffset` UTC conversion to help non-.NET consumers validate interop.

Implementation watchouts
- Do not expand this story into automatic satellite `hashDiff` generation; `DataVaultSaveService.cs:<redacted>` shows that `hashDiff` remains caller-supplied.
- Do not introduce a separate JSON/YAML manifest unless scope changes; the persisted contract explicitly designates `docs/plans/stable-hashing-contract.md` as the v1 source of truth.
- Preserve override behavior for `IStableHashService` and `IStableHashNormalizer` when touching DI or provider-specific save paths.

Non-blocking notes
- The manifest file header still references historical ticket `06EXB76DNVSRBD12T4W03AWQZC` at `docs/plans/stable-hashing-contract.md:3-5`; the current ticket text is still clear that this story ratifies that existing artifact.
- The PO refinement comment keeps the downstream `blocks` relation as a risk note rather than a handoff blocker (`.gicket/tickets/06F5Q934MSKVCQAHPCWEM29CZW/comments/06F6RB34A4QC603T4P5SBCPJS8.md:33-38`).

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment