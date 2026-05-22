<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to a bounded compile-time analyzer slice: add high-confidence EF Core misuse diagnostics in `DCoding.Data.DVault.Analyzers`, keep the existing epic/docs relations unchanged, and exclude runtime/preflight/query-shape work already owned by sibling tickets.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified local ticket evidence: the story remains under epic `06F492A3MPSGP3KXDNZECN01QM` and still blocks documentation task `06F492BNDPWS9P4EDSV0W7G6VM`; no relation cleanup was needed.
- Verified repository baseline: `DCoding.Data.DVault.Analyzers` currently ships only Code-First and mapping diagnostics (`DMV1901`, `DMV1902`, `DMV1950`-`DMV1955`), so this story is the first bounded analyzer slice for EF Core misuse diagnostics.
- Verified public DVault boundary: `IDataVaultSaveService` is the default write lane, `UseDataVaultSaveChangesMetadataInterceptor(...)` is explicit opt-in and metadata-only, and direct `Set<Dictionary<string, object>>(...)` queries are documented read patterns.
- No child tickets, description updates, attachments, or planning documents were materialized in this refinement pass; the live ticket contract is kept consistent with the existing persisted relations.

### Scope In
- Add compile-time diagnostics in `DCoding.Data.DVault.Analyzers` for statically obvious consumer-side EF Core misuse that violates documented DVault model or write boundaries.
- Cover the bounded misuse families already named by the story when they are high-confidence in source: unsupported/generated-table `DbSet` exposure, obviously unsafe direct generated-table write patterns, statically obvious missing DVault metadata registration, and obvious bypasses of DVault technical metadata conventions.
- Add analyzer tests in `tests/DCoding.Data.DVault.Tests/Analyzers` for both positive findings and documented safe patterns.
- Add a bounded code fix only where the remediation is mechanical and low-risk; otherwise rely on precise diagnostic and remediation text.

### Scope Out
- Runtime interception, runtime blocking, or runtime warn-only guard behavior for `SaveChanges`; that is already carved into `06F492AYE4A3PKA2D20DDPQ37C`.
- Preflight aggregation, drift, migration, provider-capability, or query-shape diagnostics beyond analyzer-local misuse detection; those are already covered by sibling tickets in the same epic.
- Whole-application DI inference or cross-project proof that arbitrary `DbContext` construction paths call `UseDataVaultMetadata(...)`; this story only needs statically obvious cases.
- Flagging documented read-only generated-table query patterns such as `Set<Dictionary<string, object>>(...)` plus LINQ or compiled-query reads.
- Broad release-note or documentation rollout beyond the analyzer inputs needed by downstream task `06F492BNDPWS9P4EDSV0W7G6VM`.

## Acceptance Criteria
- The analyzer package adds one or more new stable DMV diagnostics for EF Core misuse patterns that are statically obvious and specific to documented DVault invariants.
- The initial rule set includes at least unsupported/generated-table `DbSet` exposure and obviously unsafe direct generated-table write patterns; any missing-registration or technical-metadata rules are limited to cases that are unambiguous from source.
- Diagnostics do not fire on documented safe read/query usage of generated shared-type tables, including `AsNoTracking()` and compiled-query read patterns over `Set<Dictionary<string, object>>(...)`.
- Each new diagnostic ships with clear message, description, and remediation text; code fixes are included only where the fix is mechanical and safe.
- Analyzer tests cover positive findings, non-findings for supported patterns, and regression cases around the explicit `IDataVaultSaveService` boundary and opt-in `UseDataVaultSaveChangesMetadataInterceptor(...)` lane.

## Definition of Done
- New EF misuse diagnostics are implemented in `src/DCoding.Data.DVault.Analyzers` and follow the existing DMV catalog conventions used by the package.
- Repository analyzer tests prove the intended trigger and non-trigger boundaries for every added diagnostic.
- The refined implementation keeps the analyzer package as optional developer tooling rather than turning it into a complete DVault model validator.
- Downstream documentation work can consume the final diagnostic ids and remediation text without reopening the analyzer scope.

## Implementation Notes
- Use the existing analyzer/test baseline in `src/DCoding.Data.DVault.Analyzers` and `tests/DCoding.Data.DVault.Tests/Analyzers`; no new analyzer host or project split is needed.
- Treat the current public contract as authoritative: `IDataVaultSaveService` is the default write boundary, `UseDataVaultSaveChangesMetadataInterceptor(...)` only fills missing `LoadTimestamp` and `RecordSource`, and `UseDataVaultMetadata(...)` / `ApplyDataVaultMetadata(...)` remain the visible metadata registration surfaces.
- Prefer symbol and API-shape evidence over table-name heuristics alone, because DVault produced names and provider projections can vary and documented read access to generated tables is allowed.
- Sibling ticket boundaries are already present and should stay untouched: runtime guard mode `06F492AYE4A3PKA2D20DDPQ37C`, query-shape diagnostics `06F492B9PR036PDNN52S06S9BC`, preflight aggregator `06F492BG6BZYYFMBE5WK7CB024`, drift preflight `06F492AE2C8XBDXDH4V2JPTJDR`, and docs `06F492BNDPWS9P4EDSV0W7G6VM`.
- The current `blocks` relation from this story to `06F492BNDPWS9P4EDSV0W7G6VM` is still appropriate because documentation should publish the final analyzer ids and supported misuse slice after implementation lands.

## Open Questions
- none

## Follow-Up Questions
- After the high-confidence v1 rule set lands, do we want a later analyzer phase for broader DI or `DbContext` composition patterns that require multi-file inference rather than local source certainty?
- Once runtime guard mode exists, should a later story align analyzer suppressions and guard-mode messaging for advanced generated-row tracking scenarios that deliberately bypass `IDataVaultSaveService`?

## Risks
- False positives will be the main failure mode if rules try to infer arbitrary app composition instead of staying on statically obvious misuse.
- Advanced consumer flows that intentionally track generated DVault rows through EF can resemble unsafe direct writes; diagnostics must distinguish the documented opt-in metadata-interceptor lane from clearly unsupported patterns.
- String-only table-name detection is brittle because DVault supports provider-aware produced names and documented direct read access to shared-type tables.

## Split Recommendations
- No additional child-ticket split is recommended at PO refinement time; the existing sibling tickets already separate runtime guard, preflight, drift, query-shape, and documentation work, so this story can stay a single compile-time analyzer slice.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add high-confidence analyzer diagnostics for DVault-specific EF Core mistakes, such as unsupported DbSet exposure, missing model registration, unsafe direct writes, or bypassed technical metadata conventions. Prefer precise diagnostics and useful fixes over broad noisy rules.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Implemented the bounded EF Core misuse analyzer slice in `DCoding.Data.DVault.Analyzers`.
- Added high-confidence diagnostics for unsupported generated shared-type DbSet exposure and direct generated-table write calls.
- Kept runtime guard behavior, broader DI/model inference, and query-shape validation out of scope.

### Diagnostic IDs
- `DMV1910`: reports non-private `DbContext` properties or fields typed as `DbSet<Dictionary<string, object>>` because generated DVault shared-type tables should not be exposed on the context surface. Remediation: keep generated tables off the public DbContext surface and use `context.Set<Dictionary<string, object>>(producedName)` only for documented read-only query shapes.
- `DMV1911`: reports direct mutating EF calls such as `Add(...)`, `AddRange(...)`, `AddAsync(...)`, `AddRangeAsync(...)`, `Attach(...)`, `Remove(...)`, and `Update(...)` on generated shared-type sets. Remediation: use `IDataVaultSaveService` for hub, link, and satellite writes; reserve direct `Set<Dictionary<string, object>>(...)` access for read-only queries.

### Test Coverage
- Positive coverage for generated `DbSet<Dictionary<string, object>>` exposure on `DbContext`.
- Positive coverage for direct generated-table write methods.
- Non-finding coverage for private generated-set caches, ordinary entity DbSets, documented `AsNoTracking()` reads, compiled-query reads, `IDataVaultSaveService` writes, and `UseDataVaultSaveChangesMetadataInterceptor(...)` registration.

### Verification
- `dotnet test tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj --nologo` passed: 38 total, 38 succeeded.
- `dotnet build DVault.slnx --nologo` passed with existing warning noise, including NU1900 from the sandbox read-only NuGet HTTP cache and unrelated existing EF/xUnit warnings.
- `dotnet test DVault.slnx --nologo` passed; external-provider integration tests were skipped where local provider connection strings were not configured.
- `bash tools/check-format.sh` passed.

<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Reworked the bounded EF Core misuse analyzer slice in `DCoding.Data.DVault.Analyzers` after tester rework.
- Kept `DMV1910` and `DMV1911` as the shipped EF misuse diagnostic IDs, but narrowed both rules to source-visible generated DVault table evidence instead of any `DbSet<Dictionary<string, object>>`.
- Preserved documented read/query usage and added explicit non-finding coverage for arbitrary non-DVault shared-type dictionary sets and the visible `UseDataVaultSaveChangesMetadataInterceptor(...)` opt-in lane.

### Diagnostic IDs
- `DMV1910`: reports non-private `DbContext` properties or fields typed as `DbSet<Dictionary<string, object>>` only when the member source visibly resolves a DVault generated table through `Set<Dictionary<string, object>>(producedName)`. Remediation: keep generated DVault tables off the public DbContext surface and use `context.Set<Dictionary<string, object>>(producedName)` only for documented read-only query shapes.
- `DMV1911`: reports direct mutating EF calls such as `Add(...)`, `AddRange(...)`, `AddAsync(...)`, `AddRangeAsync(...)`, `Attach(...)`, `AttachRange(...)`, `Remove(...)`, `RemoveRange(...)`, `Update(...)`, and `UpdateRange(...)` on source-visible generated shared-type sets. The rule skips arbitrary non-DVault dictionary shared-type sets and a local source scope that visibly opts into `UseDataVaultSaveChangesMetadataInterceptor(...)`. Remediation: use `IDataVaultSaveService` for ordinary hub, link, and satellite writes; reserve direct generated-table access for documented read-only queries or explicit opt-in metadata-interceptor scenarios.

### Test Coverage
- Positive coverage for generated `DbSet<Dictionary<string, object>>` exposure on `DbContext`.
- Positive coverage for direct generated-table write methods.
- Non-finding coverage for private generated-set caches, ordinary entity DbSets, arbitrary non-DVault dictionary shared-type members, arbitrary non-DVault dictionary shared-type writes, documented `AsNoTracking()` reads, compiled-query reads, `IDataVaultSaveService` writes, and visible `UseDataVaultSaveChangesMetadataInterceptor(...)` opt-in usage.

### Verification
- `dotnet test tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj --nologo` passed: 41 total, 41 succeeded.
- `dotnet build DVault.slnx --nologo` passed with existing warning noise, including NU1900 from the sandbox read-only NuGet HTTP cache and unrelated existing EF/xUnit/nullability warnings.
- `dotnet test DVault.slnx --nologo` passed; external-provider integration tests were skipped where local provider connection strings were not configured.
- `bash tools/check-format.sh` passed.

<!-- gicket-bot:developer-delivery:v1:end -->