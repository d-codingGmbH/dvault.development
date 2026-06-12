[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The persisted delivery contract is authoritative, bounded, and has no open questions; repository evidence matches the proposed implementation seam and downstream ticket split.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F9GF5N4N3Q685XQPKTM5EC00/description.md` is the authoritative delivery contract; its `## Open Questions` section says `none`, and its acceptance criteria explicitly cover Binary-profile conversion for both `HashKey` and `ParticipantReference`, invalid-input handling, comparer/snapshot behavior, HexString default preservation, and tests.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` already sets `HashKeyStorageProfile`, `StableHashAlgorithmId`, `StableHashDigestByteLength`, and `HashKeyConversionBehavior` annotations and applies `LowercaseHexStringToBytesConverter` when the provider value format is `LowercaseHexBinary`, which matches the ticket's chosen implementation seam.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs` already projects `HexString` and `Binary` mappings for `DataVaultLogicalPropertyKind.HashKey` and `ParticipantReference` using `algorithmId`, `digestByteLength`, and conversion behavior metadata; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs` already covers hex sizing for built-in algorithms and binary opt-in store types.
- All current local comments for this ticket are workflow/bot comments under `.gicket/tickets/06F9GF5N4N3Q685XQPKTM5EC00/comments/`; none reopen scope or add unresolved PO questions.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete example for whether non-canonical uppercase hex at the EF string boundary should fail or normalize back to lowercase would remove one remaining implementation choice.
- The acceptance criteria references 'built-in digest sizes' generically; naming the current built-ins (`sha256-v1`, `sha1-v1`, `sha256-128-v1`, `sha256-160-v1`) would make the intended test matrix more explicit.

Risky assumptions
- Assumes 'invalid hex payloads' is enough guidance even though the currently visible `LowercaseHexStringToBytesConverter` in `DataVaultEfMetadataTranslator.cs` uses `Convert.FromHexString(...)`; if canonical lowercase enforcement is required at the EF boundary, developers will need explicit validation/tests.
- Assumes comparer/snapshot work stays inside the provider-neutral EF metadata/conversion layer and does not force scope creep into provider-specific mappings or broader schema/save/read integration already split into tickets `06F9GF5TNAXBCKN5BD9CKD7WVG` and `06F9GF60BKEW0CC9FCZRPVX0SR`.

AC / test suggestions
- Add explicit Binary-profile tests for both `HashKey` and `ParticipantReference` across `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- Add negative tests for odd-length hex, non-hex characters, and digest-length mismatch against the active `algorithmId`/`digestByteLength`.
- Add EF change-tracking equality/snapshot tests proving equivalent canonical strings, nulls, and round-trip persistence remain stable under the binary profile.

Implementation watchouts
- Keep the implementation anchored in `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` and `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs`, as the ticket contract and current repo already point there.
- A repository search returned no binary-profile hits in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs`, or `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs`; coverage expansion there is likely part of completing this story.
- Do not pull provider-specific store-type behavior or full schema/save/read integration into this story; those are already split to tickets `06F9GF5TNAXBCKN5BD9CKD7WVG` and `06F9GF60BKEW0CC9FCZRPVX0SR`.

Non-blocking notes
- The downstream provider-specific mapping ticket `06F9GF5TNAXBCKN5BD9CKD7WVG` and follow-up test ticket `06F9GF60BKEW0CC9FCZRPVX0SR` are both still `todo`, which matches the contract's staged split.

Split recommendations
- No split recommended; the current story is already bounded to the provider-neutral conversion/comparer layer and leaves provider-specific mappings and broader integration coverage to separate tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment