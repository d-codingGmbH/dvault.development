<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the persisted ticket refinement now answers both PO-critic items: same-length stable-hash `algorithmId` drift (`sha1-v1` versus `sha256-160-v1`) is explicitly fail-closed, and the reviewed support-bundle preflight baseline is named as the authoritative comparison surface. The description update was already persisted before this verification turn; no new bounded writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Persisted ticket description revision `06FBCX58QQPZQY6G5W84MV487G` already contains the amended Delivery Contract required by the PO-critic return; this turn verified the persisted text against repository evidence and relation state.
- Repository evidence confirms `sha1-v1` and `sha256-160-v1` are distinct built-in stable-hash ids that share the same 20-byte / 40-hex width, so digest length alone is not a safe compatibility key.
- Relation context remains consistent: this ticket is a child of `06F9GF5A8V7G3PAKGRXNYEBW5C` and blocks `06F9GF5N4N3Q685XQPKTM5EC00`.
- The visible provider baseline remains six built-in profiles: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`.

### Scope In
- Define the model-level hash-key storage profile contract that separates logical lowercase-hex hash keys from physical provider storage.
- Cover DVault-owned hash-key columns and hash-key-reference columns across hubs, links, satellites, PITs, and bridges.
- Define machine-readable EF metadata, diagnostics, support-bundle, provider-capability, and guardrail facts for storage profile, `algorithmId`, and digest length.
- Define compatibility behavior for the current built-in stable-hash ids, including same-length drift cases such as `sha1-v1` versus `sha256-160-v1`.

### Scope Out
- Changing public or logical hash-key surfaces from canonical lowercase hex strings to `byte[]` or provider-specific runtime types.
- Automatic rehashing, dual-write, repair, backfill, or migration tooling for callers who intentionally change persisted algorithm or storage profile.
- Provider-side SQL hashing or changes to the separate `content_hash` contract.
- Implementing DB2 live-schema reading under this ticket.

## Acceptance Criteria
- Logical hash-key values remain canonical lowercase hexadecimal strings at API, request, metadata, and diagnostics boundaries regardless of HexString or Binary physical storage.
- The contract defines a bounded model-level storage-profile vocabulary with HexString as default and Binary as explicit opt-in, applied consistently to every DVault-owned hash-key and hash-key-reference column in scope.
- Storage sizing binds to the active stable-hash algorithm's fixed digest byte length for the whole model and explicitly covers `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- Provider capability profiles and translated EF metadata expose storage profile, provider store type, logical property kind, CLR projection or conversion behavior, declared digest length, and active `algorithmId` for all six visible built-in provider profiles.
- Explain and support-bundle diagnostics expose `algorithmId`, `digestByteLength`, `digestEncoding`, and selected hash-key storage facts without raw hash values, and the reviewed support-bundle artifact is the authoritative preflight baseline for algorithm or storage drift checks.
- Migration and preflight guardrails fail closed when a DVault-owned hash-key or hash-key-reference column changes storage profile, stable-hash `algorithmId`, digest length, provider store type, or equivalent persisted shape without an intentional contract change; specifically, `sha1-v1` to `sha256-160-v1` must be rejected even though both are 20-byte / 40-hex digests.

## Definition of Done
- Provider capability profile tests cover the six visible built-in profiles for default HexString storage and digest-length sizing.
- EF translation tests prove DVault-owned hash-key and hash-key-reference properties carry authoritative storage annotations, `algorithmId`, and diagnostics facts required by the contract.
- Diagnostics and support-bundle tests cover `algorithmId` plus `digestByteLength` exposure, verify that no raw hash values are emitted, and prove the reviewed support-bundle preflight baseline distinguishes `sha1-v1` from `sha256-160-v1` when width and store type are unchanged.
- Migration or preflight guardrail tests cover unsupported HexString-to-Binary transitions, digest-length mismatches, same-length `algorithmId` drift, and provider-shape mismatches for DVault-owned hash-key columns.
- Final contract documentation is published on an approved planning or equivalent authoritative handoff surface and aligned with the v0.35.0 stable-hash guidance baseline.

## Implementation Notes
- Use the existing single-algorithm-per-model boundary (`DataVaultOptions.UseStableHashAlgorithm`, DVault conventions, and stable-hash explain surfaces) to derive one fixed hash-key digest length per translated model.
- Binary storage remains a persistence-only concern; existing save/read/request surfaces stay string-based for hash keys and hash-key references.
- Same-length algorithm swaps cannot be detected from column width or store type alone, so the reviewed support-bundle preflight baseline must carry `algorithmId` together with digest length, storage profile, provider store type, and logical property kind.
- DB2 live-schema reads remain unsupported and should stay fail-closed rather than gain a hidden special case under this ticket.
- The ticket description update answering the PO-critic return was already persisted before this turn; no relation, attachment, child-ticket, or planning-document writes were needed in this verification pass.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket extend the same storage-profile abstraction to caller-supplied HashDiff columns after the hash-key contract lands?
- Should a later ticket add reviewed migration tooling or data-move guidance for consumers who deliberately convert persisted HexString columns to Binary outside the fail-closed default posture?
- Should a later ticket bring DB2 live-schema reading to parity so runtime drift checks can validate hash-key storage shape against the catalog?

## Risks
- This ticket still blocks `06F9GF5N4N3Q685XQPKTM5EC00`, so delayed contract landing continues to hold downstream work.
- If storage-profile facts are not applied consistently across keys and hash-key references, joins, indexes, and cross-table comparisons can drift even when logical hash values still appear valid at the API boundary.
- Changing `algorithmId` or converting persisted text columns to binary remains caller-owned compatibility work; the safe default is rejection rather than inferred migration.
- Provider-scope documentation drift remains possible because older planning text predates the current six-profile baseline.

## Split Recommendations
- If delivery scope must shrink, split provider-profile plus EF-annotation/storage-profile work from migration, live-schema, and explain/preflight guardrail work while keeping this ticket as the contract parent.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define logical hash-key representation versus physical database storage. Cover HexString default behavior, Binary opt-in behavior, fixed digest byte lengths, EF model annotations, provider capabilities, diagnostics, unsupported migrations, and compatibility with variable hash algorithms from v0.35.0.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Implemented the hash-key storage profile contract on the ticket branch and published its planning-document discoverability.
- The branch defines `HexString` as the default hash-key storage profile and `Binary` as explicit opt-in provider profile projection while keeping logical hash-key boundaries as canonical lowercase hexadecimal strings.
- Provider, EF metadata, diagnostics/support-bundle, migration guardrail, and test surfaces now carry storage profile, stable-hash `algorithmId`, digest byte length, digest encoding, provider store type, value format, and conversion behavior.
- Repaired the SQLite provider integration test so it now expects `LowercaseHexText` hash-key mapping metadata and uses a canonical lowercase-hex fixture while still proving SQLite stores the value as raw TEXT.

### Repository Surfaces
- `docs/plans/hash-key-storage-profile-contract.md` is the durable planning contract for logical lowercase-hex hash keys versus provider physical storage.
- `docs/plans/README.md` now lists the contract in the current planning index.
- `docs/production-adoption-checklist.md` now points adopters to the hash-key storage profile contract and treats storage-profile drift as caller-owned compatibility work.
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageProfile.cs`, provider capability/profile mapping, EF translation, diagnostics, support-bundle, model-cache, and migration guardrail code carry the machine-readable contract facts.
- `tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs` now aligns the SQLite raw-storage integration assertion with the lowercase-hex hash-key value-format contract.
- Unit and integration tests cover six built-in provider profiles, EF annotations for hash keys and hash-key references, diagnostics/support-bundle redaction and same-width algorithm distinction, migration guardrail rejection of `sha1-v1` to `sha256-160-v1` drift, and SQLite raw TEXT persistence for lowercase-hex hash keys.

### Validation
- `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --filter FullyQualifiedName~SqliteProviderCapabilityProfileTests` passed. Microsoft.Testing.Platform ignored the VSTest filter and ran the full integration project: net8.0 had 187 passing and 23 skipped tests; net10.0 had 200 passing and 23 skipped tests.
- `dotnet build DVault.slnx --nologo` passed with 0 errors.
- `dotnet test DVault.slnx --nologo` passed.
- `bash tools/check-format.sh` passed.

### Notes
- Build and test output still contains pre-existing warnings, including NuGet vulnerability-cache warnings caused by the sandbox read-only `/home/davidullrich/.local/share/NuGet/http-cache` path, nullable warnings in existing unit tests, and existing analyzer warnings in integration tests.
- External-provider integration tests remain opt-in and were skipped where local provider connection-string environment variables were absent.
- No product clarification is needed for this ticket.
<!-- gicket-bot:developer-delivery:v1:end -->

## Developer Delivery

### Summary
- Rework pass completed without additional repository source edits because the current ticket branch already contains the hash-key storage profile contract implementation and tests.
- Refreshed validation evidence addresses the tester return: build, no-build full solution test, and formatting checks were rerun on the current branch.
- The branch defines `HexString` as the default hash-key storage profile and `Binary` as explicit opt-in while keeping logical hash-key boundaries as canonical lowercase hexadecimal strings.
- Provider, EF metadata, diagnostics/support-bundle, model-cache, and migration guardrail surfaces carry storage profile, stable-hash `algorithmId`, digest byte length, digest encoding, provider store type, value format, and conversion behavior facts.
- The SQLite integration assertion remains aligned with `LowercaseHexText` metadata and a canonical lowercase-hex fixture while proving SQLite stores the hash key as raw TEXT.

### Repository Surfaces
- `docs/plans/hash-key-storage-profile-contract.md` is the durable planning contract and explicitly names the reviewed support bundle as the authoritative preflight baseline for algorithm or storage drift checks.
- `docs/plans/README.md` lists the contract in the current planning index.
- `docs/production-adoption-checklist.md` points adopters to the hash-key storage profile contract and treats storage-profile drift as caller-owned compatibility work.
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageProfile.cs` defines the bounded `HexString`/`Binary` vocabulary.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs`, `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs`, `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`, `src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs`, and `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs` carry the implementation facts through provider mapping, EF annotations, diagnostics/support-bundle explain output, and guardrail comparison.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs` cover the acceptance criteria and Definition of Done surfaces.

### Validation
- `dotnet build DVault.slnx --nologo` passed with 0 errors. Output still includes existing warning classes, including NuGet vulnerability-cache warnings from the read-only `/home/davidullrich/.local/share/NuGet/http-cache` path and existing analyzer/nullable/xUnit warnings.
- `timeout 600s dotnet test DVault.slnx --nologo --no-build` passed. Observed summaries included integration net8.0: 187 succeeded, 23 skipped; integration net10.0: 200 succeeded, 23 skipped; unit net8.0: 524 succeeded; unit net10.0: 542 succeeded.
- `bash tools/check-format.sh` passed, including one-member-per-file validation for 647 C# files and dotnet whitespace verification.
- `git diff --name-only -- . ':!.gicket'` and `git diff --cached --name-only -- . ':!.gicket'` returned no output after verification, so this rework pass did not introduce unstaged or staged non-ticket source edits.

### Tester Rework Resolution
- The prior tester return did not identify a repository code defect; it stated that acceptance criteria, Definition of Done expectations, or checklist gates were not fully confirmed.
- This rework pass materially addresses that return by rerunning the policy validation commands on the current branch and persisting the fresh validation evidence in this developer delivery block.
- No product clarification is needed.

### Notes
- External-provider live integration tests remain opt-in and were skipped where local provider connection-string environment variables were absent.
- DB2 live-schema reading remains intentionally unsupported under this ticket; the contract keeps that path fail-closed.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Rework pass completed without repository source edits because the current ticket branch already contains the hash-key storage profile contract implementation, documentation, and tests.
- Refreshed validation evidence directly addresses the tester return: build, solution tests, and formatting checks were rerun on the current branch.
- The branch defines `HexString` as the default hash-key storage profile and `Binary` as explicit opt-in while keeping logical hash-key boundaries as canonical lowercase hexadecimal strings.
- Provider, EF metadata, diagnostics/support-bundle, model-cache, and migration guardrail surfaces carry storage profile, stable-hash `algorithmId`, digest byte length, digest encoding, provider store type, value format, and conversion behavior facts.
- The SQLite integration assertion remains aligned with `LowercaseHexText` metadata and a canonical lowercase-hex fixture while proving SQLite stores the hash key as raw TEXT.

### Repository Surfaces
- `docs/plans/hash-key-storage-profile-contract.md` is the durable planning contract for logical lowercase-hex hash keys versus provider physical storage, including same-width `sha1-v1` to `sha256-160-v1` drift rejection.
- `docs/plans/README.md` lists the contract in the current planning index.
- `docs/production-adoption-checklist.md` points adopters to the hash-key storage profile contract and treats post-persistence algorithm, truncation, or storage-profile changes as caller-owned compatibility work.
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageProfile.cs`, provider capability/profile mapping, EF translation, diagnostics, support-bundle, model-cache, and migration guardrail code carry the machine-readable contract facts.
- `tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs` verifies SQLite raw TEXT persistence for canonical lowercase-hex hash keys.
- Unit tests cover six built-in provider profiles, EF annotations for hash keys and hash-key references, diagnostics/support-bundle redaction and same-width algorithm distinction, and migration guardrail rejection of `sha1-v1` to `sha256-160-v1` drift.

### Validation
- `dotnet build DVault.slnx --nologo` passed with 0 errors.
- `timeout 600s dotnet test DVault.slnx --nologo --no-build` passed: integration net8.0 had 187 passing and 23 skipped tests; integration net10.0 had 200 passing and 23 skipped tests; unit net8.0 had 524 passing tests; unit net10.0 had 542 passing tests.
- `timeout 600s bash tools/check-format.sh` passed; the one-member-per-file check reported 647 C# files.

### Notes
- Build output still contains pre-existing warning classes, including NuGet vulnerability-cache warnings caused by the read-only local cache path, existing nullable warnings, existing xUnit analyzer warnings, and existing DMV1913 analyzer warnings in integration tests.
- External-provider live integration tests remain opt-in and were skipped where local provider connection-string environment variables were absent.
- No product clarification is needed for this ticket.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Rework pass completed without additional repository source edits because the current ticket branch already contains the hash-key storage profile contract implementation and tests.
- Refreshed tester-facing evidence addresses the return: build, no-build full solution tests, and formatting checks were rerun on the current branch.
- The branch defines `HexString` as the default hash-key storage profile and `Binary` as explicit opt-in while keeping logical hash-key boundaries as canonical lowercase hexadecimal strings.
- Provider profiles, EF translation, diagnostics/support-bundle, model-cache, and migration guardrails carry storage profile, stable-hash `algorithmId`, digest byte length, digest encoding, provider store type, value format, and conversion behavior facts.
- Same-width stable-hash drift is fail-closed: `sha1-v1` to `sha256-160-v1` is rejected even when digest width and store type are unchanged.

### Repository Surfaces
- `docs/plans/hash-key-storage-profile-contract.md` is the durable planning contract for logical lowercase-hex hash keys versus provider physical storage.
- `docs/plans/README.md` lists the hash-key storage profile contract in the current planning index.
- `docs/production-adoption-checklist.md` points adopters to the contract and treats algorithm, truncation, and hash-key storage-profile changes as caller-owned compatibility work.
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageProfile.cs`, provider capability/profile mapping, EF translation, diagnostics, support-bundle, model-cache, and migration guardrail code carry the machine-readable contract facts.
- `tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs` asserts SQLite hash-key mappings use `LowercaseHexText` and persist canonical lowercase-hex values as raw TEXT.
- Unit and integration tests cover six built-in provider profiles, EF annotations for hash keys and hash-key references, diagnostics/support-bundle redaction and same-width algorithm distinction, migration guardrail rejection of `sha1-v1` to `sha256-160-v1` drift, and SQLite raw TEXT persistence for lowercase-hex hash keys.

### Tester Rework Closure
- The tester return did not identify a missing source change; it reported that persisted acceptance criteria, Definition-of-Done expectations, or checklist gates were not fully confirmed.
- Current branch HEAD is `d56301cd3a8ee67950c249e38655aa816382fb24`.
- `git diff --name-only 2575cbbb0ef3..HEAD -- docs src tests` returned no files, so the repository implementation surfaces still match the prior developer handoff for docs/src/tests.
- `git diff --name-only -- docs src tests` and `git diff --cached --name-only -- docs src tests` returned no files after validation.

### Validation
- `dotnet build DVault.slnx --nologo` passed with 0 errors.
- `timeout 600s dotnet test DVault.slnx --nologo --no-build` passed: integration net8.0 had 187 succeeded and 23 skipped; integration net10.0 had 200 succeeded and 23 skipped; unit net8.0 had 524 succeeded; unit net10.0 had 542 succeeded.
- `timeout 600s bash tools/check-format.sh` passed, including the one-member-per-file check for 647 C# files.

### Notes
- Build output still contains known warning classes, including NuGet vulnerability-cache warnings caused by the read-only `/home/davidullrich/.local/share/NuGet/http-cache` path, existing xUnit analyzer warnings in integration tests, and existing nullable warnings in unit tests.
- External-provider live integration tests remain opt-in and were skipped where local provider connection-string environment variables were absent.
- No product clarification is needed.
<!-- gicket-bot:developer-delivery:v1:end -->