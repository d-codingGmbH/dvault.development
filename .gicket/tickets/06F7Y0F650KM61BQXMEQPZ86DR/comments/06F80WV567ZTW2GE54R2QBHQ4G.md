[gicket-bot] PO refinement contract

Summary
- Refined 06F7Y0F650KM61BQXMEQPZ86DR as the bounded v0.24 documentation and release-note rollup over the already-completed async helper, benchmark-evidence, and EF-safety analyzer child work; no new split or relation write was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Existing child-ticket evidence already fixes the release surface: 06F7Y0DZ3AJSG99YN00CAVX3JR landed additive async-source helpers over the existing IDataVaultSaveService async chunked save boundary, 06F7Y0EVNY2M0113A6VWBNDCPR landed async benchmark evidence inside the existing customer-profile-streaming-save family, and 06F7Y0E81P65F9HEPNN72Z0NBW landed EF safety analyzer guidance for caller-owned model-shape risks.
- The documentation task is a rollup only: it must describe async streaming, typed helper convenience, benchmark evidence, and EF model-cache safety without reopening API shape, benchmark schema, or analyzer design.
- Docs must preserve the settled boundary that DVault is not an ingestion platform and does not provide provider-native async execution, background orchestration, automatic model-cache-key completeness checks, or automatic compiled-model/pooling safety.
- Repository docs already show the current baseline on v0.23.0; this task should move the public baseline to v0.24.0 and keep v0.23.0 and earlier notes as historical feature-introduction sources rather than parallel current baselines.
- No bounded child-ticket creation, relation change, description write, attachment, or planning-document write was required during this refinement run.

Scope In
- Update the user-facing documentation surfaces named in the ticket: root README, docs/production-adoption-checklist.md, src/DCoding.Data.DVault.Analyzers/README.md, performance guidance, and v0.24.0 release notes.
- Document the save-path selection guidance across materialized DataVaultBulkSaveRequest, synchronous DataVaultChunkedSaveRequest, and the landed async IAsyncEnumerable<DataVaultSaveChunk> path, including ordering, cancellation, caller-owned transaction behavior, and provider-neutral limits.
- Document the landed typed async helper story as convenience over the existing explicit async chunked save boundary, not as a second persistence abstraction or ingestion subsystem.
- Document the EF safety guidance around registry-backed UseDataVaultMetadata as the safe built-in baseline, caller-owned IModelCacheKeyFactory when extra tenant/schema/naming/provider/profile state changes the realized model shape, and the fixed-model requirements for UseModel(...) and AddDbContextPool<TContext>(...).
- Update release-note and performance-guidance text to cite the shared benchmark artifact triplet and the async-source evidence inside the existing customer-profile-streaming-save scenario family.

Scope Out
- No product-code, analyzer, or benchmark-harness implementation changes.
- No new provider-native async execution claims, ingestion pipeline guidance, scheduler/background worker guidance, or SaveChanges interception expansion beyond the existing documented boundary.
- No new benchmark artifact schema, file naming contract, or provider-specific async benchmark matrix.
- No runtime model-cache guard, cache-stress harness, or proof that a custom IModelCacheKeyFactory includes every caller-owned discriminator.
- No package publication, automation, or release-approval claims beyond the existing manual publication boundary.

Open questions
- none

Follow-up questions
- After the v0.24.0 doc rollup lands, do we want a later follow-up that adds provider-specific tenant or schema examples for model-cache-key customization, or should the public guidance stay provider-neutral?
- Once more benchmark evidence accumulates, should a later docs pass add recommended chunk-size ranges or profile-based tuning heuristics beyond the initial provider-neutral selection guidance?
- If release history needs before/after benchmark lineage beyond the root triplet, should a future task add a ticket-labelled benchmark artifact bundle instead of keeping v0.24.0 citations only on the shared benchmark-summary surfaces?

Risks
- If the docs cite async streaming without the benchmark run-context caveats from the shared artifact contract, they can overstate throughput or provider-specific behavior.
- If README, checklist, analyzer README, and release notes are not updated together, the repo can expose conflicting guidance about whether v0.23.0 or v0.24.0 is the current public baseline.
- If the EF safety guidance blurs the line between DVault-owned registry-backed isolation and caller-owned discriminators, readers may incorrectly assume DVault proves custom IModelCacheKeyFactory completeness or makes pooled or compiled dynamic-model contexts safe.
- If the release notes or analyzer README use provisional diagnostic IDs instead of the implemented catalog, the documentation will drift from the actual analyzer surface.

Split recommendations
- No new split recommended; keep this ticket as the bounded documentation and release-note rollup over completed child tickets 06F7Y0DZ3AJSG99YN00CAVX3JR, 06F7Y0EVNY2M0113A6VWBNDCPR, and 06F7Y0E81P65F9HEPNN72Z0NBW.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment