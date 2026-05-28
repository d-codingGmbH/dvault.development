[gicket-bot] PO refinement contract

Summary
- Refined the story into a metadata-driven v1 generator contract aligned with current PIT/bridge, compiled-query, and dvault.model.v1 repository decisions; no new child tickets or relation writes are needed because the satellite and PIT/bridge implementation split already exists in 06F5Q92AHG0ZCTVQGC6NAYVP9C and 06F5Q92R02HB7FCE1AWKXPTMRW.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 read boundary: generated helpers cover stable metadata-defined latest/current/as-of satellite reads plus explicit PIT as-of and bridge traversal reads, while dynamic runtime-built requests stay on IDataVaultReadService.
- PIT and bridge reads remain explicit read-model consumers of already-maintained rows; this ticket does not reopen PIT/bridge maintenance, background orchestration, or automatic SaveChanges refresh.
- Model-first input means dvault.model.v1 metadata projected into the EF or DVault metadata surface, code-first input means projected CLR or EF metadata including compiled-model usage, and metadata-first input means direct authoritative DVault metadata; the generator contract should normalize all three into one authoritative descriptor model.
- No additional ticket split or relation cleanup is needed now because the existing downstream stories already cover the justified implementation partition between latest or as-of satellite work and PIT or bridge work.

Scope In
- Define the authoritative v1 contract for typed read-model source generation over stable DVault metadata-defined read shapes.
- Fix supported latest/current/as-of satellite, PIT as-of, and bridge traversal shapes, including the bounded PIT and bridge cases already documented in repository architecture notes.
- Define deterministic generated type and member naming, produced table and column name binding, nullability mapping, and projection semantics from authoritative metadata.
- Define analyzer and generator diagnostics for unsupported metadata, stale authoritative metadata fingerprints, and request shapes outside the bounded contract.
- Define how metadata-first, model-first, and code-first inputs participate through one authoritative metadata source boundary.

Scope Out
- Runtime compilation of arbitrary dynamic IDataVaultReadService requests or user-authored query shapes.
- Provider-specific SQL, compiled-model tooling, migrations, or cross-provider performance guarantees beyond already-documented provider-neutral behavior and the existing SQLite-proven optimized PIT and bridge path.
- Automatic PIT or bridge maintenance, background refresh, save-pipeline changes, or orchestration that mutates read tables.
- Schema changes to dvault.model.v1 or new model-first artifact dialects.
- Hash canonicalization governance work except where existing produced names or metadata fingerprints must be consumed by the generator contract.

Open questions
- none

Follow-up questions
- After the contract lands, confirm whether packaging and distribution of generator plus analyzer assets should stay in the existing analyzers surface or move into a dedicated package or story.
- When the child implementation tickets start, verify whether an additional test-vector ticket is needed for stale metadata fingerprint diagnostics across metadata-first, model-first, and compiled-model code-first flows.

Risks
- If the contract blurs the boundary between generated stable helpers and dynamic IDataVaultReadService requests, downstream stories can drift into unsupported arbitrary query compilation.
- PIT and bridge support is bounded by the repository's existing architecture notes; over-promising link-parent, tuple-filter, provider-specific, or maintenance-coupled behavior would create delivery churn.
- Model-first and compiled-model inputs only stay safe if the contract ties generation to one authoritative metadata source and fingerprint; otherwise stale generated code and mismatched produced names become likely.

Split recommendations
- No additional split is needed now. Keep this story focused on the authoritative v1 contract and let 06F5Q92AHG0ZCTVQGC6NAYVP9C cover latest or as-of satellite projector implementation while 06F5Q92R02HB7FCE1AWKXPTMRW covers PIT or bridge projector implementation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment