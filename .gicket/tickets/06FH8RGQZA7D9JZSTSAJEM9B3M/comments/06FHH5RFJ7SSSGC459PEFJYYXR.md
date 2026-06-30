[gicket-bot] PO-critic review contract

Summary
- The prior PO-critic blocker is resolved: the ticket now points to the existing core `DataVaultProviderNativeEncryptionBoundaryFact` source, keeps `## Open Questions` empty, and is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The current ticket description PO Summary explicitly says the earlier PO-critic finding was resolved by correcting the provider-native boundary diagnostics contract to `src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs`.
- `gicket-read-ticket-comments` includes the later PO refinement comment with `critic-item-1`, `critic-item-2`, and `critic-item-3` marked `answered`, followed by `decision: ready_for_po_critic`.
- `git status --short --branch` returned `## HEAD (no branch)` and `git rev-parse HEAD` returned `3c264a9e77bb38c81f1bc336953f6ee6eb8e597b`, matching the prompt branch-head identity and showing a clean scratch review surface.
- `src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs` directly defines the core public record `DataVaultProviderNativeEncryptionBoundaryFact(...)`.
- `src/DCoding.Data.DVault.Privacy/IDataVaultEncryptedPayloadKeyProvider.cs`, `src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadConversionDirection.cs`, and `src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs` directly verify the caller-owned alias-driven conversion seam, explicit `Encrypt`/`Decrypt` directions, and fail-closed alias/provider checks.
- `docs/plans/dvault-model-v1-schema-contract.md` directly defines additive `personalData[].encryptedPayloadAlias` metadata on satellite payload fields and rejects provider-specific storage, SQL, algorithm, migration, or DDL fields.
- `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` directly keeps provider-native encryption guidance-only and unmanaged in the shared DVault surface.
- `docs/releases/v0.50.0.md` directly states that `DataVaultPrivacyDiagnostics` carries provider-native encryption boundary facts and that `DataVaultProviderNativeEncryptionBoundaryFact` records SQL Server, PostgreSQL, Oracle, MySQL, SQLite, and DB2 native encryption as guidance-only and unmanaged.
- `git grep -n` found `personal-data-privacy-proof-missing` in `src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:1910` and matching unit assertions in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:468,498,523,545,566,589,591`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A later implementation ticket should keep the fail-closed case for marked payloads with alias metadata but no observable converter coverage explicit, rather than silently downgrading to ordinary payload handling.
- A later provider-specific ticket should choose one exact provider-native capability example instead of widening this shared boundary ticket.

Risky assumptions
- The current contract correctly treats `personalData[].encryptedPayloadAlias` as the approved downstream contract target; implementers should not assume that metadata is already landed as branch code just because the contract is now approved.
- Implementers must treat `DataVaultProviderNativeEncryptionBoundaryFact` as evidence-only boundary reporting, not as approval for shared runtime provider-native encryption behavior.

AC / test suggestions
- Keep direct coverage for missing alias registration, missing or marker-only key-provider posture, declined conversion, and the distinction between `personal-data-privacy-proof-missing` and `personal-data-privacy-coverage-unusable`.
- Keep direct coverage or equivalent checked-in evidence for provider-native boundary fact reporting across the finite baseline: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.

Implementation watchouts
- Do not widen this story into provider-native encrypted DDL, SQL crypto functions, capability probing, runtime dispatch, or key-store integration; the reviewed docs keep those out of scope.
- Use `src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs` as the source-backed boundary-fact carrier and do not reintroduce the nonexistent privacy-package path.
- Treat the release-note `DataVaultPrivacyDiagnostics` lane as redacted diagnostics evidence only, not as approval for a shared cross-provider native crypto feature.

Non-blocking notes
- As a normal pre-development ticket, missing implementation changes on the branch are not a PO blocker once the contract and source-backed compatibility anchors are clear.

Split recommendations
- Keep future provider-native encryption work split to one provider and one exact capability per ticket, with its own provider package surface, fallback rules, tests, and evidence.
- Split broader privacy workflow APIs such as read-helper redaction, pseudonymization flows, or retention metadata review into separate tickets instead of widening this contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment