[gicket-bot] PO-critic review contract

Summary
- Authoritative contract is concrete, bounded, and aligned with the verified repo baseline; no unresolved PO questions remain, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NBAP31G489S3YXXYY54WM/description.md contains the authoritative delivery contract with 5 acceptance-criteria items, 4 definition-of-done items, and `## Open Questions` set to `none`.
- .gicket/tickets/06EZ0NBAP31G489S3YXXYY54WM/comments/06EZ3Z72PX8CAYBV4DEZCEDXJW.md records PO handoff `decision: ready_for_po_critic`; the local comment set for this ticket is workflow/handoff only and does not contain unresolved design debate.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs currently exposes only `DataVaultProviderCapabilityProfiles.Sqlite`, so the Oracle-profile addition is a concrete additive change against a verified baseline.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs currently exposes only the default `UseDataVault()` and `ApplyDataVaultMetadata(DataVaultMetadataModel)` path, and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs hardcodes `DataVaultProviderCapabilityProfiles.Sqlite`; this directly matches the ticket's provider-aware model-configuration requirement while preserving the default path.
- src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs currently just calls `services.AddDVault();`, and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs currently asserts `AddDVaultOracle()` registers no provider strategy; the ticket explicitly updates this verified compatibility-only Oracle baseline.
- src/DCoding.Data.DVault/DataVaultSaveService.cs dispatches registered `IDataVaultProviderSaveStrategy` instances by descending `Priority`, first `CanSave` winner, else provider-neutral fallback; tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs already codify those fallback and tie-break semantics referenced by the contract.
- README.md and docs/architecture/dvault-v1-explicit-save-service.md currently describe Oracle as compatibility-only, which gives a clear, directly observed before-state for the implementation and expectation updates.
- `git show --stat --oneline --no-patch 5800f1af6a1f3bde705c509b3b28718163da4dd1` returned `[06EZ0NBAP31G489S3YXXYY54WM] lease claim po-critic`, and `git diff --name-only 5800f1af6a1f3bde705c509b3b28718163da4dd1..ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil` returned no files, so the review surface matches the claimed scratch ref.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Whole-batch fallback when an ordered bulk request mixes Oracle-supported and Oracle-unsupported operations.
- Behavior when Oracle services are registered but the active `DbContext.Database.ProviderName` is not the Oracle provider.
- Satellite-only or mixed hub/link/satellite batches if the first Oracle optimized path intentionally supports only a subset of shapes.
- Provider-aware model translation assertions proving the new Oracle path changes `ProviderProfile` and `ProviderStorageType` annotations while the existing default path still emits the current SQLite baseline.

Risky assumptions
- The exact Oracle native store-type and value-format baseline can be chosen during implementation without further product input.
- The first Oracle optimized path may support only a narrow subset of save batches, provided `CanSave` rejects unsupported whole batches deterministically.
- Oracle-runtime correctness beyond unit/smoke coverage is intentionally deferred and remains acceptable for this task.

AC / test suggestions
- Add explicit Oracle capability-profile tests alongside `DataVaultProviderCapabilityProfileTests`, including unsupported SQL-function and concurrency baselines plus all logical property kinds.
- Add Oracle strategy-selection tests mirroring `DataVaultSaveStrategySelectionTests` for registration, provider gating, tracked-change rejection, and whole-batch fallback.
- Add negative dependency verification for `DCoding.Data.DVault.Oracle` so package or nuspec checks prove it does not pull in `Sqlite`, `Postgres`, `MySql`, `SqlServer`, or non-Oracle EF provider packages.
- Update public API snapshot coverage for both core and Oracle packages if the provider-aware model-configuration path or capability surface becomes public.

Implementation watchouts
- Do not break the existing no-argument `AddDVault()`, `UseDataVault()`, or `ApplyDataVaultMetadata()` SQLite-default path verified in README.md and src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs.
- Keep dispatch semantics unchanged: `DefaultDataVaultSaveService` sorts by descending `Priority` and falls back only after every strategy rejects the batch.
- The current Oracle package surface is intentionally narrow in src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj, so dependency creep needs active verification.
- Current tests and docs still treat Oracle as compatibility-only, so Oracle implementation work will need coordinated expectation updates where those baselines intentionally change.

Non-blocking notes
- The sibling task `.gicket/tickets/06EZ0NBH3YWJPF05AQWC0E6GV4` already exists for Oracle opt-in integration configuration and smoke tests, which keeps this ticket bounded to core/Oracle capability and strategy work.
- The local comment thread for 06EZ0NBAP31G489S3YXXYY54WM contains workflow and handoff comments only; there is no recorded human objection or unresolved discussion in the current ticket thread.

Split recommendations
- Keep shared profile/model-selection work in `src/DCoding.Data.DVault` separate from Oracle strategy implementation in `src/DCoding.Data.DVault.Oracle`, matching the persisted contract.
- Keep opt-in Oracle integration configuration and environment-specific smoke coverage in task `06EZ0NBH3YWJPF05AQWC0E6GV4` instead of expanding this task's scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment