[gicket-bot] PO refinement contract

Summary
- Verified ticket 06EXB76NNRDP7WH1F2R5VYYPMR, comments, relations, attachments, source/test layout, and the stable hashing contract. No child tickets, relations, attachments, or planning documents were created; the refined scope is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The source of truth for this ticket is docs/plans/stable-hashing-contract.md: sha256-v1, SHA-256, UTF-8 input bytes without BOM, lowercase 64-character hex digest, and deterministic AlgorithmId propagation.
- The approved v1 scalar normalization baseline covers null, string, boolean, integer, decimal, timestamp, and guid values plus structured field ordering. Null stable values are encoded as n: and are distinct from empty service input and empty string values.
- The current repository evidence shows the owning library under src/DCoding.Data.DVault and the active test layout under tests/DCoding.Data.DVault.Tests with Unit, Integration, and Shared projects.
- No human ticket comments add product requirements, and the ticket has no attachments. Existing relations are parentOf from 06EXB765S2X2MR2K18ZBV8RC38 and blocks 06EXB80FPE3REH11RQ1YR6BW1G.
- The stable-hashing contract does not approve a separate binary scalar value encoding. For this ticket, binary coverage means hash byte materialization: normalized .NET strings are encoded as UTF-8 without BOM and digests are returned as lowercase hexadecimal.

Scope In
- Unit tests for the default stable hash service using the published vectors for empty input, empty string value, null value, repeated deterministic text, ordered structured value with null, and culture-invariant decimal/timestamp.
- Tests that repeated hashing of the same normalized input returns the same AlgorithmId and digest.
- Tests that null service input fails fast with ArgumentNullException while empty normalized input remains valid.
- Tests for required canonical normalization behavior: string NFC normalization, CRLF/CR to LF conversion, invariant number and timestamp formatting, null field inclusion, and ordinal field-path sorting.
- Tests that unsupported value types and invalid supported values fail before hashing.
- Tests that culture-sensitive cases stay stable when CurrentCulture is changed to a non-invariant culture.
- Tests that binary materialization uses UTF-8 bytes without BOM and produces 64 lowercase hexadecimal SHA-256 digest text.

Scope Out
- Password hashing, encryption, MACs, signatures, key management, salts, or other security-specific hashing behavior.
- Persisted storage schema, migrations, provider-specific physical types, or Data Vault entity-field participation decisions.
- A new binary scalar input encoding such as byte array, stream, or base64 canonicalization unless a separate contract explicitly approves it.
- Advanced hash replacement UI/API design beyond what is needed to test AlgorithmId and digest propagation where the implementation already exposes replacement through DI or options.
- Renaming or restructuring existing test roots outside tests/DCoding.Data.DVault.Tests.

Open questions
- none

Follow-up questions
- Should a later contract ticket add a first-class binary scalar normalization rule for byte arrays or streams? Current evidence does not define one, so it should not block this ticket.
- When persisted hashes are introduced, confirm where AlgorithmId and canonicalization version are stored for compatibility and migration planning.
- Future entity-specific hashing tickets should identify participating fields and add vectors for those model-specific canonical inputs.

Risks
- The ticket title mentions binary normalization, but the approved stable-hashing contract currently defines UTF-8 byte materialization for normalized string input and no standalone binary scalar encoding; the refined scope keeps that bounded to avoid unapproved behavior.
- Culture tests can leak process-global state if CurrentCulture and CurrentUICulture are not restored.

Split recommendations
- No split is recommended for the current bounded test scope.
- Create a separate follow-up ticket only if product wants a new binary scalar canonical encoding beyond UTF-8 materialization of normalized strings.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment