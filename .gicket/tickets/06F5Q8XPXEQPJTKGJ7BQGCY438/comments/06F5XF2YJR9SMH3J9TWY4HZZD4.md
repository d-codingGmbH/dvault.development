[gicket-bot] PO-critic review contract

Summary
- Contract is clear, grounded in current repo evidence, and has no unresolved open questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q8XPXEQPJTKGJ7BQGCY438/description.md:32-54` defines concrete acceptance criteria and definition-of-done items, and `:53-54` shows `## Open Questions` = `none`.
- `git log --oneline --decorate -n 8` on `ticket/06F5Q8XPXEQPJTKGJ7BQGCY438-story-explain-streaming-fallback-and-remediation` shows HEAD `dedb3fa142c3290cd591e7ecf303829b7a8ceead` with only PO/PO-critic workflow commits after scratch source `0b1caa35633951259af2b6b13dac2283ba55e298`.
- `git diff --name-only 0b1caa35633951259af2b6b13dac2283ba55e298..HEAD` lists only `.gicket/tickets/06F5Q8XPXEQPJTKGJ7BQGCY438/**`, confirming this branch is still ticket-refinement only and not implementation evidence.
- `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs:59-136,209-237` already exposes chunked-save telemetry members (`ChunkCount`, `ProcessedChunkCount`, retained-state counts, `ChunkedStateFallbackCauseKinds`, `UnsupportedShapeKinds`) as the public bounded lane named by the ticket.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:54-108` defines the finite save fallback vocabulary, and `:745-799` shows `IDataVaultDiagnosticsService.Analyze(...)` overloads for `DataVaultSaveRequest` and `DataVaultBulkSaveRequest` but no `DataVaultChunkedSaveRequest` overload.
- `docs/architecture/dvault-v1-streaming-explicit-save-contract.md:45-47,68-72` fixes transaction ownership, the `10000` retained-state limit, the finite `RetainedSatelliteSeriesLimitReached` / `RetainedSatelliteSeriesLimitExceeded` classifications, and `DataVaultSaveTelemetrySummary` as the chunked diagnostics surface.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:<redacted>,<redacted>` emits one chunked telemetry summary per attempt and records the retained-state fallback/unsupported-shape enums into telemetry.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:<redacted>` already covers chunked success, failure, cancellation, and retained-state-limit telemetry scenarios, matching the test families requested by the ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The acceptance criteria name the fallback families but do not explicitly call out `UnknownOrUnregisteredProviderName` and `StrategyDeclined`; keep those cases in scope if the public explanation surface maps every currently exposed enum.
- Implementation notes say one chunked attempt can aggregate multiple strategy selections and suppress a single `SelectedStrategyName`; a mixed-strategy aggregate case is worth exercising so the explanation output does not read like a per-chunk trace.

Risky assumptions
- Consumers who need the new guidance will have `AddDVaultTelemetry()` or a custom `IDataVaultTelemetryObserver` configured; the default `AddDVault()` path remains telemetry-free.
- Provider-strategy gate changes will keep the current finite enum vocabulary stable enough for explanation/remediation mapping without silent drift.
- Per-attempt aggregate guidance is acceptable even when different chunks contribute different fallback causes.

AC / test suggestions
- Keep at least one focused assertion per finite save fallback enum family instead of only asserting generic provider-neutral fallback.
- Add a mixed-chunk aggregation assertion where different chunks contribute different fallback causes or provider strategy outcomes and the output stays deterministic and redacted.
- If the public surface changes beyond current summary members, require the public API snapshot and consumer-facing docs to move in lockstep with the opt-in telemetry requirement.

Implementation watchouts
- Do not introduce a second free-form chunked diagnostics channel; current repo evidence points to additive explanation/remediation over `DataVaultSaveTelemetrySummary` because `IDataVaultDiagnosticsService` does not analyze `DataVaultChunkedSaveRequest`.
- Keep remediation text aggregate and low-cardinality because chunked telemetry is emitted once per attempt and retained-state fallbacks must not expose raw hash keys, payload values, or per-parent listings.
- Transaction wording must stay aligned with the contract: caller owns transaction boundaries, and all-or-nothing across chunks requires opening the transaction before invoking the save service.

Non-blocking notes
- The current branch is still pre-development: the diff from scratch source `0b1caa35633951259af2b6b13dac2283ba55e298` to HEAD contains only `.gicket` ticket metadata changes.
- The story still blocks downstream docs task `06F5Q8Y3WW9FFV7HA289VHCEAM` via `.gicket/relations/38/AM/06F5Q8XPXEQPJTKGJ7BQGCY438--06F5Q8Y3WW9FFV7HA289VHCEAM--blocks.json`, so that documentation follow-up remains sequenced behind implementation.
- Local ticket comments are bot-authored workflow comments only; there is no human discussion to reconcile before dev handoff.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment