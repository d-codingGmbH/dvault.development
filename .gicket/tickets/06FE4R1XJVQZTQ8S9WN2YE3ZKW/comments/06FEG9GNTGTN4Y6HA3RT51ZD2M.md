[gicket-bot] PO-critic review contract

Summary
- Persisted contract is bounded, repo-backed, and ready for evidence-only developer handoff on the SQLite `sha256-v1`/`HexString` baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FE4R1XJVQZTQ8S9WN2YE3ZKW/description.md` contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` set to `- none`.
- `git log --oneline -n 8` on `ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff` shows only PO workflow commits after `develop`, and `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06FE4R1XJVQZTQ8S9WN2YE3ZKW/*`, so no implementation or benchmark artifact changes have landed on this branch yet.
- `src/DCoding.Data.DVault/DefaultStableHashNormalizer.cs`, `src/DCoding.Data.DVault/BuiltInStableHashService.cs`, and `src/DCoding.Data.DVault/DefaultDataVaultSaveService.cs` exist, directly matching the scoped hotspot surfaces named in Scope In.
- `benchmark-summary.json` context records `hashKeyVariants[0] = sha256-v1-hex` with `stableHashAlgorithmId=sha256-v1`, `digestByteLength=32`, and `storageProfile=HexString`; `docs/plans/hash-key-storage-profile-contract.md` defines `HexString` as the default profile and `sha256-v1` as 32 bytes / 64 hex characters.
- `benchmark-summary.md` contains completed SQLite rows for `customer-profile-bulk-history`, four `customer-profile-streaming-save` variants, `order-product-fulfillment-history`, and `latest-satellite-read`, which covers the hub-only, chunked/replay, link-bearing, and latest-state workload shapes cited in the contract.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` requires every persisted evidence set to keep `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` from one run and states that provider-evidence additions do not replace that triplet.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` enumerates the checked-in scenario rows and verifies the repository `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` stay in sync, so the triplet artifact contract is already enforced in-repo.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` documents `--latest-indexes` as the lane that seeds 100 customers with 20 existing profile states each and compares unchanged replay vs changed replay saves across index variants, matching the ticket's latest-hash-diff replay evidence requirement.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The developer will prove provider-neutral/common-path allocations separately whenever the SQLite optimized lane would otherwise hide `DefaultDataVaultSaveService` costs; the contract allows a mirrored common-path measurement if needed.
- The developer will keep caller-owned satellite `HashDiff` generation out of the hotspot ranking and measure only DVault-owned latest-hash-diff lookup/filter and replay-dedup work.
- The existing whole-scenario allocation rows are assumed to be baseline context only; ticket closure still depends on additive method-level or step-level hotspot evidence.

AC / test suggestions
- Require the final evidence bundle to name the exact exercised rows or modes, including `customer-profile-bulk-history`, the chosen `customer-profile-streaming-save` row(s), `order-product-fulfillment-history`, and the `--latest-indexes` unchanged/changed replay lane.
- Require the hotspot summary to separate provider-neutral fallback measurements from any SQLite-optimized comparison row instead of treating optimized SQLite as the common-path baseline.
- Require chunk size, processed chunk count, and retained-state high-water to be visible whenever chunked continuity-state evidence is part of the replay allocation ranking.

Implementation watchouts
- The current checked-in benchmark triplet reports whole-scenario `meanAllocatedBytes`, not the ranked per-method hotspot output this ticket promises; developers still need supplemental hotspot artifacts.
- Because `git diff develop..HEAD` is metadata-only, the developer handoff starts from a refinement-only branch and must add the actual benchmark/profiler evidence on top of the current repo baseline.
- `DefaultStableHashNormalizer` sorts normalized fields and materializes normalized text, and `BuiltInStableHashService` materializes UTF-8 bytes plus lowercase hex output; those steps should be measured directly before choosing optimization order.
- If `--latest-indexes` does not isolate the provider-neutral replay filter path well enough, the developer should mirror that workload with an explicit common-path measurement rather than infer equivalence from an optimized lane.

Non-blocking notes
- The ticket comments are workflow/refinement/handoff records only; no unresolved design debate or contradictory acceptance guidance was found in the local comment set.
- The root benchmark baseline already includes materialized, chunked, and async-source streaming-save rows, which gives enough repository-backed shapes to keep the ticket evidence-only without reopening scope.
- The downstream implementation work is already represented by ticket `06FE4R261S2FSQ786S4F4JE90R`, so approving this ticket does not imply combining optimization changes into the same delivery slice.

Split recommendations
- No additional PO split is needed before development; keep this ticket evidence-only and land runtime allocation reductions in follow-up implementation tickets.
- If the final ranking cleanly separates canonicalization/hash-generation hotspots from replay/save-preparation hotspots, split those optimization follow-ups rather than broadening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment