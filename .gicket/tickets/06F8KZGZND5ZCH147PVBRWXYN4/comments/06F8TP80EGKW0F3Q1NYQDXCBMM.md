[gicket-bot] PO refinement contract

Summary
- Repository evidence supports one bounded regression-test story: extend EF lifecycle analyzer fixtures for code-first unsafe model-shape cases plus metadata-first and model-first non-diagnostic baselines; stale blocks relation cleanup from ticket 06F8KZGNRG5FY4WWCY3FAX2NS4 to this ticket was already queued for replay.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current branch still matches scratch source ref 762b610ef6a278348cf9238e6227a455abb26650, so this story is refining planned coverage rather than describing in-flight implementation.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs already exercises DMV1912 through DMV1914 for direct ApplyDataVaultMetadata(...) code-first lanes and several documented safe lanes, but it does not yet include metadata-first UseDataVaultMetadata(...) or model-first UseDataVaultMetadata(DataVaultModelImportResult) lifecycle fixtures.
- README.md and tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs already establish that UseDataVaultMetadata(), explicit registry selection, and successful model-first imports participate in DVault metadata-source isolation and are the built-in non-diagnostic baseline for lifecycle rules.
- Queued mutation mutation-85850a0a7eb2c034 removes stale relation 06F8KZGNRG5FY4WWCY3FAX2NS4--06F8KZGZND5ZCH147PVBRWXYN4--blocks on ticket 06F8KZGNRG5FY4WWCY3FAX2NS4's owner branch; that cleanup should be treated as part of this refinement contract.

Scope In
- Add analyzer regression fixtures for DMV1912, DMV1913, and DMV1914 around unsafe caller-owned model-shape variation in direct ApplyDataVaultMetadata(...), direct UseModel(...), and direct AddDbContextPool<TContext>(...) lanes.
- Add explicit metadata-first non-diagnostic fixtures for UseDataVaultMetadata(), UseDataVaultMetadata(DataVaultMetadataModel), and UseDataVaultMetadata(DataVaultMetadataRegistry) because DVault already isolates those registry-backed sources in the EF model cache.
- Add explicit model-first non-diagnostic fixtures for UseDataVaultMetadata(DataVaultModelImportResult) because successful dvault.model.v1 imports resolve to the same registry-backed projection and built-in cache isolation baseline.
- Add regression coverage for documented safe fixed-shape lanes, including visible design-model-to-runtime-model UseModel(...) flow, fixed options-only AddDbContextPool<TContext>(...) flow, read-only compiled queries over generated shared-type tables, metadata-interceptor opt-in, and opaque cache-key helpers that the analyzer must skip.

Scope Out
- No expansion of analyzer semantics beyond the documented high-confidence lifecycle contract that relies on direct source-visible evidence.
- No new diagnostics for pooled factories, cross-assembly/helper-based inference, raw dvault.model.v1 parsing, or literal metadata-model inspection inside the analyzer.
- No runtime behavior changes, provider-specific lifecycle rules, or unrelated product-code edits outside the minimum analyzer/test adjustments needed for this regression slice.

Open questions
- none

Follow-up questions
- Should src/DCoding.Data.DVault.Analyzers/README.md be aligned with the v0.27 lifecycle contract now that repository code and tests already reserve DMV1912 through DMV1914, or is that documentation update intentionally tracked elsewhere?

Risks
- The analyzer currently keys off direct ApplyDataVaultMetadata(...), UseModel(...), and AddDbContextPool<TContext>(...) source evidence, so overly ambitious fixtures could accidentally require unsupported inference instead of validating the documented high-confidence boundary.
- Metadata-first and model-first baselines are safe because DVault-owned UseDataVaultMetadata(...) isolation is already proven elsewhere; fixtures must preserve that distinction so they do not imply raw model or metadata parsing by the analyzer.
- The stale blocks relation removal is queued for replay on another ticket's owner branch and may remain visibly present until that replay completes, even though the intended contract has already been cleaned up.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment