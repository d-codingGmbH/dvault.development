[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the persisted contract is specific, has no open questions, and is grounded in existing repository cache-key, fingerprint, and custom `IModelCacheKeyFactory` patterns.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket` for `06F492AKGMKPCRJYF4Z1EC9WY4` shows `## Open Questions` = `none` and PO handoff = `ready_for_po_critic`; `gicket-read-ticket-comments` returned 10 automation comments and no later discussion reopening scope.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs` builds the DVault EF model-cache key from `(ContextType, designTime, SourceKind, Fingerprint)` and resolves the fingerprint through `DataVaultMetadataSourceAnnotations.CreateFingerprint(...)`.
- `src/DCoding.Data.DVault/DataVaultMetadataSourceAnnotations.cs` defines source kinds `app-default-registry`, `dbcontext-registry`, `model-metadata`, `model-registry`, and `model-artifact`, records `MetadataSourceKind` and `MetadataSourceFingerprint`, and throws on conflicting authoritative sources.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs` exposes `UseDataVaultMetadata()` overloads for app-default registry, explicit `DataVaultMetadataRegistry`, explicit `DataVaultMetadataModel`, and `UseDataVaultMetadata(DataVaultModelImportResult)`, with the import-result overload delegating to `RequireMetadataRegistry()`.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs` already contains `DbContextOptionsExplicitRegistryParticipatesInModelCacheKey` plus app-default and explicit-registry projection tests, matching the story's baseline behavior.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixtures.cs` already show consumer-owned `ReplaceService<IModelCacheKeyFactory,...>` patterns carrying caller state such as `LoadTimestampStorage`, schema name, provider profile, table prefix, and identifier overrides.
- `README.md` already documents registry-backed `UseDataVaultMetadata()` and model-first `UseDataVaultMetadata(DataVaultModelImportResult)` entry points, while an `rg -n` search for `IModelCacheKeyFactory|cache key|model cache` across `README.md` and `docs/` returned only test-file hits, confirming the ticket is targeting a real documentation gap on an existing public surface.
- `git diff --stat 9848aca88798f5fecfe22a92d2f182a3d8f61fe5..HEAD` returned no output, and `git show --stat --oneline HEAD` shows commit `9848aca88` touching only `.gicket/...` ticket metadata files; the branch is still at pre-development ticket state, which is consistent with a dev handoff review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the contract does not force one preferred consumer-owned example, so the implementation will still need to choose among existing repository patterns such as load-timestamp storage, schema selection, or naming/prefix overrides.

Risky assumptions
- Assumes the developer will keep the custom-key proof on a repository-supported local baseline, as requested in the contract, instead of relying only on external-provider schema fixtures.
- Assumes the documentation update will explicitly distinguish registry-backed isolation from caller-owned `OnModelCreating` variability so readers do not infer automatic protection for tenant, profile, or schema-dependent state.
- Assumes the model-first import lane can be covered through the existing `UseDataVaultMetadata(DataVaultModelImportResult)` surface without reopening metadata-source selection architecture.

AC / test suggestions
- Keep one proof lane for app-default versus explicit registry selection and one for `UseDataVaultMetadata(DataVaultModelImportResult)` so the registry-backed boundary is demonstrated across both supported entry points.
- For the consumer-owned customization example, prefer a deterministic in-repo discriminator already used by tests, such as `LoadTimestampStorage`, schema name, or naming/prefix state, and assert that distinct models do not leak entities or annotations across cache entries.
- In the documentation acceptance proof, require one concrete discriminator list such as `tenantId`, `schemaName`, `loadTimestampStorage`, or naming/profile state, plus an explicit statement that direct `ApplyDataVaultMetadata(...)` is safe only when the remaining model shape is stable for that context type and design-time flag.

Implementation watchouts
- Do not broaden the story into automatic tenant or profile discovery; the contract explicitly keeps arbitrary caller-owned state outside DVault's built-in cache-key responsibility.
- Reuse the existing authoritative-source and fingerprint behavior rather than inventing a second cache-selection mechanism, or the work risks conflicting with current `DataVaultMetadataSourceAnnotations` diagnostics.
- If a test uses external-provider schema variation, pair it with a SQLite-friendly or provider-agnostic custom-key proof so the normal CI lane still validates the contract.

Non-blocking notes
- Current public docs already expose the registry-backed and model-first entry points, so there is a clear public guidance surface to update.
- Current branch HEAD is ticket-metadata-only; absence of src/docs/test changes is expected at this pre-development gate and is not a PO blocker.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment