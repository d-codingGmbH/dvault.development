[gicket-bot] PO-critic review contract

Summary
- Contract is locally verified, has no open questions, and is ready for developer handoff as a documentation-only v0.24.0 rollup over already-landed repo surface.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract at .gicket/tickets/06F7Y0F650KM61BQXMEQPZ86DR/description.md sets PO handoff to `ready_for_po_critic` and `## Open Questions` to `none`.
- Local source matches the named async API surface: src/DCoding.Data.DVault/DataVaultSaveService.cs:50-60 defines `SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)`; src/DCoding.Data.DVault/DataVaultSaveServiceAsyncExtensions.cs:29-43 defines `SaveAsync<TSource>(...)`; src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs:229-334 and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:<redacted> expose async `SaveHubsAsync`, `SaveLinksAsync`, and `SaveOrdinaryHubSatellitesAsync`.
- Local EF-safety baselines match the contract references: README.md:509-560 covers DVault-owned registry isolation plus caller-owned `IModelCacheKeyFactory` duties, and docs/architecture/dvault-ef-compiled-compatibility.md:83-97 bounds `UseModel(...)` and `AddDbContextPool<TContext>(...)` to one fixed realized model shape.
- Implemented EF misuse analyzer IDs are locally verified as DMV1910 and DMV1911 only in src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:6-19 and src/DCoding.Data.DVault.Analyzers/README.md:5-10.
- Repo drift is concrete and matches the ticket scope: README.md:25 and 768-781, docs/production-adoption-checklist.md:9 and 107, docs/performance-profiles.md:3-5, and src/DCoding.Data.DVault.Analyzers/README.md:20 still point at v0.23.0, while docs/releases/v0.24.0.md is missing.
- Benchmark evidence for the async-source wording already exists in benchmark-summary.md:42-44, while docs/performance-profiles.md:83-109 still mixes v0.24 async wording with a v0.23.0 header and omits the async-source row from its supporting table.
- `git diff --name-status develop...HEAD` shows only .gicket ticket metadata/comments/events changes on this branch, not product docs or source files; `git log --oneline --max-count=12 -- ...` shows PO/PO-critic workflow commits, which is consistent with a pre-development handoff.
- Comment evidence is aligned: .gicket/tickets/06F7Y0F650KM61BQXMEQPZ86DR/comments/06F814SPJCZSRPYT5DWEF55244.md says PO refinement made the ticket ready for PO-critic, and .gicket/tickets/06F7Y0EVNY2M0113A6VWBNDCPR/comments/06F80DP4GA73YDET2D50YYS90M.md says the broader v0.24 adopter-doc rewrite stays on this ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Include one explicit chooser example for `DataVaultBulkSaveRequest` vs `DataVaultChunkedSaveRequest` vs `IAsyncEnumerable<DataVaultSaveChunk>` so adopters can distinguish when each path is preferred.
- Include one explicit reminder example that `UseModel(...)` and `AddDbContextPool<TContext>(...)` are fixed-model guidance only and do not replace caller-owned `IModelCacheKeyFactory` duties when tenant/schema/naming/profile state changes model shape.

Risky assumptions
- Assuming existing prose in docs/performance-profiles.md is already authoritative would be risky; benchmark-summary.md:42-44 carries the current async-source row and docs/performance-profiles.md:107-109 still reflects older supporting-row content.
- Assuming model-cache or pooling diagnostic IDs exist would be wrong; related ticket 06F7Y0E81P65F9HEPNN72Z0NBW is `done` with `closure/no-work-required`, and the implemented EF misuse catalog exposes DMV1910/DMV1911 only.

AC / test suggestions
- Add a doc-level verification pass that README.md, docs/production-adoption-checklist.md, docs/performance-profiles.md, src/DCoding.Data.DVault.Analyzers/README.md, and docs/releases/v0.24.0.md all name v0.24.0 as the coordinated baseline and keep the seven-package/manual-publication wording aligned.
- Verify the final performance-guidance and release-note prose cite the root benchmark triplet and explicitly include the `customer-profile-streaming-save` async-source row with its run-context caveats.
- Verify any analyzer-ID references in updated docs are limited to DMV1910 and DMV1911, with model-cache/pooling safety kept as guidance-only links back to README.md and docs/architecture/dvault-ef-compiled-compatibility.md.

Implementation watchouts
- Do not let documentation imply provider-native async execution, background continuation, or a second persistence subsystem; the verified source surface is additive over the existing explicit `IDataVaultSaveService` boundary.
- Do not treat `UseModel(...)` or `AddDbContextPool<TContext>(...)` as generally safe for variable model shapes; the local guidance is fixed-model-only unless the application owns the full cache-key discriminator set.
- Create docs/releases/v0.24.0.md in the same pass as the README/checklist/analyzer/performance updates to avoid preserving the current v0.23/v0.24 split.

Non-blocking notes
- This branch is still at pre-dev state: no repository docs or source files have changed beyond .gicket ticket metadata, so the next role is expected to do the first content-update pass.
- The ticket is well-bounded as documentation-only work over already-landed repo surface; no code, analyzer catalog, or benchmark harness changes are required by the verified contract.

Split recommendations
- No split recommended. Related benchmark and analyzer comments already route the broader v0.24 adopter-doc rollup to this ticket, and the current contract keeps scope bounded to documentation surfaces only.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment