[gicket-bot] PO refinement contract

Summary
- Verified ticket, comments, relations, and repository evidence; the authoritative description update is already applied at revision 06FEJ31NEAB48DEREQREFQB0N0, and the ticket is refined as bounded low-risk allocation work on the SQLite sha256-v1 HexString baseline with no further planning writes required.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The authoritative hotspot baseline is artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-<redacted>/ from done ticket 06FE4R1XJVQZTQ8S9WN2YE3ZKW; the ranked order starts with DefaultDataVaultSaveService.AddSatellitesAsync, then DefaultDataVaultSaveService.FilterSatellitePlansAsync and LoadLatestSatelliteHashDiffsAsync, then DefaultStableHashNormalizer.NormalizeFields, then BuiltInStableHashService.ComputeHash.
- The default v1 evidence lane remains SQLite local temporary files with sha256-v1 and HexString; optional providers in the checked-in benchmark summary are skipped placeholders unless separately rerun as explicit regression evidence.
- Satellite HashDiff generation remains caller-owned and out of scope; this ticket only targets DVault-owned canonicalization, digest generation, save-plan materialization, and latest-hash-diff replay filtering allocations.
- Verified live relation context: incoming relates from done story 06FE4R089MT3BYRCVH7Q4EX6CG is historical alignment context, incoming blocks from done hotspot ticket 06FE4R1XJVQZTQ8S9WN2YE3ZKW is completed prerequisite context, and outgoing blocks to docs ticket 06FE4R2EGQ444EGPKZBRZCDEV8 should stay in place because that docs task depends on measured results from this implementation.
- No child tickets, relation changes, attachments, or planning documents were materialized in this turn; the durable refinement change was the already-applied description update.

Scope In
- Implement low-risk allocation reductions in the provider-neutral/common hot paths surfaced by the hotspot report, especially satellite replay and save-preparation work in DefaultDataVaultSaveService.AddSatellitesAsync, FilterSatellitePlansAsync, and LoadLatestSatelliteHashDiffsAsync.
- Reduce avoidable allocation materialization in stable-hash canonicalization and digest generation without changing the public contract, especially DefaultStableHashNormalizer.NormalizeFields and BuiltInStableHashService.ComputeHash.
- Refresh comparable before/after benchmark evidence with the existing benchmark harness and artifact contract so targeted allocation improvements are reviewable.
- Preserve current persisted outcomes, published stable-hash vectors, chunked continuity behavior, and provider strategy-selection behavior while reducing allocations.

Scope Out
- Changing stable-hash algorithm ids, published digest vectors, canonical text rules, hash-key storage-profile contracts, or the public lowercase-hex hash-key boundary.
- Reassigning satellite HashDiff ownership to DVault or adding an internal payload-hash generator.
- Provider-specific SQL/save/read tuning, new optional-provider performance claims, or broad non-default hash-key variant validation beyond follow-up regression checks.
- Benchmark harness replacement, artifact-schema redesign, or documentation and release-note updates already owned by ticket 06FE4R2EGQ444EGPKZBRZCDEV8.
- High-risk structural rewrites whose correctness depends on new persistence semantics rather than low-risk allocation reductions.

Open questions
- none

Follow-up questions
- After the bounded low-risk reductions land, do any remaining stable-hash canonicalization or digest micro-optimizations warrant a separate post-v0.43 follow-up instead of extending this ticket?
- Once refreshed evidence is available, do any configured optional-provider lanes need explicit reruns before docs or release notes generalize the result beyond the required SQLite baseline?
- Should a future release promote any focused hotspot lane into the default benchmark workflow, or should it remain opt-in alongside --allocation-hotspots?

Risks
- Allocation-focused edits in canonicalization or digest generation can accidentally change published hash outputs if normalization ordering, UTF-8 handling, or lowercase-hex materialization semantics drift.
- Replay-filter reductions can regress unchanged-versus-changed satellite behavior if latest-hash-diff lookup or retained chunk state semantics change with the allocation work.
- A win measured only on the required SQLite sha256-v1 HexString lane should not be overgeneralized to provider-specific or non-default hash-key variants without follow-up evidence.

Split recommendations
- No immediate split is required because the hotspot ranking already gives one bounded optimization order inside this ticket.
- If implementation naturally separates into a second round after the dominant replay/save-preparation reductions land, prefer a later follow-up ticket for secondary stable-hash canonicalization and digest micro-optimizations rather than widening this task mid-flight.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment