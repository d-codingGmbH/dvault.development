[gicket-bot] PO-critic review contract

Summary
- Contract is clear, bounded, and unblocked for developer handoff; no unresolved PO refinement question remains.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4R261S2FSQ786S4F4JE90R/events/06FEJ31NEAB48DEREQREFQB0N0.json records a <redacted>-21T07:19:11Z TicketEdited event that set `description-markdown`, matching the persisted contract revision cited in the ticket.
- git -C /mnt/c/Projects/DVault log --oneline --decorate -n 12 shows HEAD `a749c27e5` on branch `ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation`, with the PO->PO-critic handoff commit `aef3fa157` above `develop` at `a7427ddac`.
- git -C /mnt/c/Projects/DVault diff --name-only develop..HEAD lists only `.gicket/tickets/06FE4R261S2FSQ786S4F4JE90R/...` metadata paths, confirming this is still a pre-development ticket-quality branch rather than a partially implemented code branch.
- .gicket/relations/KW/0R/06FE4R1XJVQZTQ8S9WN2YE3ZKW--06FE4R261S2FSQ786S4F4JE90R--blocks.json exists, and .gicket/tickets/06FE4R1XJVQZTQ8S9WN2YE3ZKW/ticket.json shows the hotspot prerequisite ticket is `done`.
- .gicket/relations/CG/0R/06FE4R089MT3BYRCVH7Q4EX6CG--06FE4R261S2FSQ786S4F4JE90R--relates.json exists, and .gicket/tickets/06FE4R089MT3BYRCVH7Q4EX6CG/ticket.json shows the parent story is `done`.
- .gicket/relations/0R/V8/06FE4R261S2FSQ786S4F4JE90R--06FE4R2EGQ444EGPKZBRZCDEV8--blocks.json exists, and .gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/ticket.json keeps the downstream docs ticket in `todo`, matching the stated dependency direction.
- artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-<redacted>/allocation-hotspots.md shows required provider `SQLite local temporary files`, provider filter `sqlite`, hash key variant `sha256-v1-hex`, and ranks `DefaultDataVaultSaveService.AddSatellitesAsync` #1/#2, `DefaultDataVaultSaveService.FilterSatellitePlansAsync` #3/#5, `DefaultDataVaultSaveService.LoadLatestSatelliteHashDiffsAsync` #4/#6, `DefaultStableHashNormalizer.NormalizeFields` #8, and `BuiltInStableHashService.ComputeHash` #12.
- artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-<redacted>/benchmark-summary.json records `iterations=3`, `warmupIterations=1`, `providerFilter=sqlite`, `stableHashAlgorithmId=sha256-v1`, and `storageProfile=HexString` for the authoritative hotspot baseline.
- docs/plans/performance-evidence-benchmark-artifact-contract.md requires the benchmark triplet `benchmark-summary.md/.csv/.json` and states that targeted allocation work must improve or hold the targeted metric, with required SQLite non-target allocation regressions above 5% failing by default.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers and testers will keep the refreshed before/after evidence on the same SQLite `sha256-v1` `HexString` baseline and comparable run inputs; the root `benchmark-summary.json` is a broader `providerFilter=all` snapshot and is not the authoritative hotspot baseline for this ticket.
- Any future documentation will avoid generalizing wins to PostgreSQL, SQL Server, MySQL, Oracle, or DB2 unless those lanes are explicitly rerun; the checked-in root `benchmark-summary.json` still shows optional-provider rows skipped when connection strings are unset.
- The planned allocation reductions can stay low-risk without changing stable hash outputs, lowercase hex behavior, replay dedupe semantics, or provider strategy-selection boundaries.

AC / test suggestions
- Keep explicit regression checks for unchanged satellite replay suppressing duplicate writes and changed replay persisting the expected new state across retained chunks.
- Retain bit-for-bit stable hash vector coverage for `sha256-v1` `HexString`, including canonicalization order, UTF-8 handling, and lowercase hex output.
- Persist comparable before/after benchmark triplets under one explicit artifact label and keep `allocation-hotspots.*` additive if hotspot profiling is rerun.

Implementation watchouts
- Current branch history is ticket-metadata only versus `develop`; developer work still needs to add code and refreshed evidence without widening scope into docs or provider-specific SQL tuning.
- Do not use the general root benchmark summary as proof of optional-provider behavior for this ticket; the authoritative baseline is the SQLite-only hotspot artifact bundle from `06FE4R1XJVQZTQ8S9WN2YE3ZKW`.
- Preserve the caller-owned `HashDiff` boundary and avoid public hash/storage contract changes while removing intermediate materialization.

Non-blocking notes
- The outgoing blocks relation to docs ticket `06FE4R2EGQ444EGPKZBRZCDEV8` is appropriate downstream routing and not a blocker on developer handoff for this implementation task.

Split recommendations
- No immediate split is warranted; keep one bounded implementation ticket and only carve out a later follow-up if secondary stable-hash micro-optimizations naturally separate after the dominant replay/save-preparation reductions land.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment