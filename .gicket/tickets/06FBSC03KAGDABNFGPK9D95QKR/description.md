<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already ratifies `sha256-v1` plus `HexString` as the existing-project default across `AddDVault()`, `UseDataVault()`, built-in provider capability profiles, diagnostics, docs, and approved API snapshots; this ticket should stay bounded to regression coverage that proves only explicit binary-profile selection changes hash-key storage. No child tickets, relation edits, description updates, attachments, or planning documents were materialized; the live `blocks` relation to `06FBSC0TMZBXVVECGQGESWPCY4` remains unchanged.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `DataVaultConventions.Default`, `services.AddDVault()`, and `modelBuilder.UseDataVault()` already resolve to `sha256-v1`, 32 digest bytes, and `DataVaultHashKeyStorageProfile.HexString` in the visible source and tests.
- Built-in provider profiles already default hash-key and participant-reference mappings to lowercase-hex text plus `none-string-model`; binary mapping is explicit opt-in through `WithHashKeyStorageProfile(..., DataVaultHashKeyStorageProfile.Binary, ...)`.
- Repository docs already state that logical hash-key values stay lowercase hexadecimal strings, `HexString` is the compatible default, and `Binary` is explicit opt-in physical storage only.
- No bounded planning writes were applied during refinement; the existing `blocks` relation to `06FBSC0TMZBXVVECGQGESWPCY4` stays live.

### Scope In
- Add regression coverage for existing-project default startup and model paths so `AddDVault()`, `UseDataVault()`, and default metadata translation keep `HexString`-compatible hash storage.
- Add regression coverage proving that explicit binary-profile selection is the only supported path that flips hash-key storage behavior away from the compatibility default.
- Protect both `HashKey` and `ParticipantReference` mappings so a partial default flip cannot pass unnoticed.
- Keep approved public API snapshot coverage aligned with any public selector or helper involved in the binary-profile story.

### Scope Out
- Changing the existing-project default storage profile away from `HexString`.
- Automatic migration, backfill, dual-write, repair, or rehash behavior for persisted hashes.
- New provider-footprint or performance claims beyond the checked-in SQLite-local evidence bundle.
- Unrelated stable-hash algorithm-selection changes or broader storage-profile redesign.

## Acceptance Criteria
- Regression tests fail if the default `AddDVault()` path stops resolving `sha256-v1`, 32 digest bytes, and `DataVaultHashKeyStorageProfile.HexString` for existing-project setup.
- Regression tests fail if the default `UseDataVault()` or default `ApplyDataVaultMetadata(...)` paths stop projecting `HexString`-compatible hash-key and participant-reference mappings, including the expected provider value format and conversion behavior.
- Regression tests fail if explicit binary-profile opt-in does not project `Binary`, `LowercaseHexBinary`, and `lowercase-hex-string-to-bytes`, or if the same mapping facts appear without explicit selection.
- Approved public API snapshot tests cover any public binary-profile selection surface so accidental surface drift or silent default changes require intentional review.

## Definition of Done
- Existing unit, integration, and snapshot suites are updated in the repository’s current coverage areas for default conventions, metadata translation/provider mapping, and public API approval.
- The completed tests prove both sides of the contract: existing-project defaults stay `HexString`, and explicit binary selection is the only path that yields binary storage mappings.
- Coverage asserts persisted-compatibility facts that matter for regressions: storage profile, algorithm id, digest byte length, provider store type, provider value format, and conversion behavior.
- Changed validation lanes pass, including the affected snapshot/behavior tests under `dotnet test DVault.slnx --nologo` or an equivalent targeted subset used by the implementer.

## Implementation Notes
- Use the existing regression anchors instead of inventing a new test harness: `StableHashServiceTests`, `DefaultNamingPolicyTests`, `DataVaultEfMetadataTranslationTests`, `DataVaultProviderCapabilityProfileTests`, `SqliteDataVaultSchemaTests`, `DataVaultDiagnosticsTests`, and `ApiSurfaceSnapshotTests` already cover nearby behavior.
- The visible default baseline is already encoded in `DataVaultConventions.Default`, `DataVaultModelBuilderExtensions.UseDataVault()`, and the built-in hash mappings in `DataVaultProviderCapabilityProfiles`; refinement should ratify that baseline rather than reopen it.
- Keep the assertions at the compatibility-fact level instead of only checking enum presence or store width, because the repository contract treats storage profile, algorithm id, digest length, provider value format, and conversion behavior as persisted compatibility facts.
- If downstream work introduces a public greenfield/new-project binary helper, this ticket’s regression coverage should exercise that exact helper as the sole explicit path allowed to change the default hash-key storage profile.
- No description update, attachment, child-ticket split, or planning-document write was justified from the current evidence.

## Open Questions
- none

## Follow-Up Questions
- When the downstream new-project binary-profile ticket lands, should adopter-facing docs explicitly label that helper as greenfield-only and link back to the compatibility and migration caveats?
- Does the downstream binary-profile work need a separate provider-matrix smoke lane proving the explicit helper selects the same binary mapping facts across every built-in provider profile, beyond this default-preservation ticket?

## Risks
- If coverage only exercises one entry point, another default path could still drift; `AddDVault()`, `UseDataVault()`, and default metadata translation all need protection.
- Snapshot approval alone can hide behavioral drift if reviewers accept changed baselines without matching runtime mapping assertions.
- Only asserting primary hash-key columns would miss regressions on participant references, which are part of the same persisted-compatibility contract.
- The live `blocks` relation to `06FBSC0TMZBXVVECGQGESWPCY4` remains until this regression coverage is delivered.

## Split Recommendations
- No split recommended; the work is already bounded to extending existing unit, integration, and snapshot suites around one compatibility default.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add regression coverage proving that current setup paths keep HexString-compatible hash storage unless the new-project binary profile is explicitly selected. Acceptance: package/API snapshots and model mapping tests catch accidental default changes.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added regression coverage for existing-project hash compatibility defaults across `AddDVault()`, EF `UseDataVault()`, default `ApplyDataVaultMetadata(...)`, and SQLite model projection.
- The new assertions cover `sha256-v1`, 32 digest bytes, `DataVaultHashKeyStorageProfile.HexString`, `LowercaseHexText`, `TEXT`, `lowercase-hex-no-prefix`, `none-string-model`, and no binary value converter on default hash-key and participant-reference mappings.
- Existing explicit binary-profile coverage remains the only path asserting `Binary`, `LowercaseHexBinary`, and `lowercase-hex-string-to-bytes`.

### Repository Artifacts
- `tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs`
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs`

### Verification
- `dotnet test tests/DCoding.Data.DVault.Tests/Modeling/DCoding.Data.DVault.Tests.Modeling.csproj --no-restore --nologo --filter FullyQualifiedName~ModelingConventionCoverageTests -p:UseSharedCompilation=false -p:MinVerSkip=true` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --no-restore --nologo --filter FullyQualifiedName~DataVaultEfMetadataTranslationTests -p:UseSharedCompilation=false -p:MinVerSkip=true` passed; Microsoft Testing Platform ignored the VSTest filter and ran the unit suite for both target frameworks.
- `dotnet artifacts/bin/DCoding.Data.DVault.Tests.Integration/Debug/net10.0/DCoding.Data.DVault.Tests.Integration.dll --no-progress --no-ansi --filter-class DCoding.Data.DVault.Tests.Integration.SqliteDataVaultSchemaTests` passed.
- `dotnet artifacts/bin/DCoding.Data.DVault.Tests.Integration/Debug/net8.0/DCoding.Data.DVault.Tests.Integration.dll --no-progress --no-ansi --filter-class DCoding.Data.DVault.Tests.Integration.SqliteDataVaultSchemaTests` passed.
- `dotnet format whitespace --folder --verify-no-changes --verbosity minimal --include tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` passed.

### Validation Note
- `bash tools/check-format.sh` timed out after 25 seconds in the sandbox while entering its git-based discovery path; touched-file whitespace verification passed separately.
<!-- gicket-bot:developer-delivery:v1:end -->