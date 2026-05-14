[gicket-bot] PO-critic review contract

Summary
- Approve. The bounded contract is already evidenced in the repository, and this parent now reads as a tracking/closure story whose done child covers the concrete implementation slice.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs exposes public DbContext opt-in methods UseDataVaultSaveChangesMetadataInterceptor(...), and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt snapshots both the extension methods and DataVaultSaveChangesMetadataInterceptorOptions.
- src/DCoding.Data.DVault/DataVaultSaveChangesMetadataInterceptor.cs only runs on EntityState.Added rows, filters entity kinds to Hub, Link, and Satellite, and only populates TechnicalMetadataColumnRole.LoadTimestamp and TechnicalMetadataColumnRole.RecordSource after annotation-based discovery via DataVaultAnnotationNames.PropertyRole and TechnicalColumnRole.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultSaveChangesMetadataInterceptorRegistrationTests.cs asserts the default services.AddDVault() path resolves zero ISaveChangesInterceptor instances and that interceptor registration happens only through explicit DbContext opt-in.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesMetadataInterceptorSqliteTests.cs covers both SaveChanges() and SaveChangesAsync(), verifies missing LoadTimestamp and RecordSource values are populated, preserves manual Link LoadTimestamp and Satellite RecordSource values, and proves renamed columns LoadedAtUtc and SourceSystem are discovered from DVault annotations.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- There is no explicit repository test showing Modified or Deleted DVault rows remain untouched; the Added-only guard is visible in source, so this is a non-blocking coverage gap rather than a refinement blocker.
- There is no current consumer-facing example of opting a DbContext into the interceptor, but the parent contract explicitly keeps broader README and example work out of scope.

Risky assumptions
- This approval assumes the parent should be treated as a tracking or closure-style story because the only concrete delivery slice is the done child task and the repository already contains the implementation and tests.
- This approval assumes stale repository prose can stay outside this ticket: README.md still says DVault does not intercept SaveChanges, and docs/architecture/dvault-v1-explicit-save-service.md still says an optional interceptor can be considered later.

AC / test suggestions
- If a follow-up test is added later, cover non-Added states explicitly so the Added-only boundary is protected against future interceptor expansion.
- If documentation follow-up work is created, add a minimal opt-in example using UseDataVaultSaveChangesMetadataInterceptor(...) and restate that IDataVaultSaveService remains the default write boundary.

Implementation watchouts
- Keep future expansion separate from this slice: the current implementation deliberately ignores TechnicalMetadataColumnRole.HashKey and HashDiff and only auto-populates missing LoadTimestamp and RecordSource.
- Do not let later convenience-path documentation erase the current architecture truth that AddDVault() remains interceptor-free by default.

Non-blocking notes
- The repository evidence supports the bounded contract, but README.md and docs/architecture/dvault-v1-explicit-save-service.md now lag the implemented optional interceptor surface.

Split recommendations
- No new implementation split is needed for this story; the existing done child already covers the bounded interceptor slice.
- Keep broader lineage metadata families such as batch, correlation, tenant, overwrite modes, and broader consumer docs in separate follow-up tickets as the contract already states.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment