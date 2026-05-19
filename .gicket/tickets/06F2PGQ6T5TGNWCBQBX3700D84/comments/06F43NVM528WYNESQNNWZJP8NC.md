[gicket-bot] PO-critic review contract

Summary
- Authoritative delivery contract is bounded, repository-backed, and has no unresolved PO questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGQ6T5TGNWCBQBX3700D84/description.md:7-16 and :53-54 set PO handoff to `ready_for_po_critic`, bind the story to the existing diagnostics surface, and show `## Open Questions` -> `- none`.
- .gicket/tickets/06F2PGP7HM8F39K3J0H5JHB3B4/ticket.json shows the inbound blocking epic is `done`, matching the contract statement that it is historical context rather than an active blocker.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:320-387 exposes `DataVaultDiagnosticsResult.ReadStrategy` and `ToDisplayString()` output that already renders both save-strategy and read-strategy status and selected strategy name.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted> and :<redacted> directly implement read-strategy candidate ordering, provider-neutral fallback aggregation, and request-shape-specific causes for latest/as-of, PIT, and bridge reads.
- src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs:8-55, src/DCoding.Data.DVault/DataVaultPitAsOfReadRequest.cs:8-37, and src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs:8-141 define the exact read request shapes named in the contract.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:694-745, :908-945, and :<redacted> snapshot the public read/save strategy diagnostics records plus `IDataVaultDiagnosticsService` and `IDataVaultReadDiagnosticsService`.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:11-19 and :234-363 already verify `NotEvaluated` defaults and material fallback causes such as `UnsupportedSatelliteParent`, `UnsupportedPitShape`, `DirtyDbContext`, and provider-threshold declines.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-33 and src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs:10-42 show the current concrete provider-specific read-strategy registration is SQLite for latest-satellite, PIT, and bridge reads.
- `git diff --name-only f4b5b7fa2693fdb65090e73f433bda9e134930a1..HEAD -- .gicket/tickets/06F2PGQ6T5TGNWCBQBX3700D84 src docs README.md tests` returned only `.gicket/tickets/06F2PGQ6T5TGNWCBQBX3700D84/...` paths, so the branch currently contains ticket/refinement changes only, which is normal for pre-development handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the contract asks for representative fallback causes but does not name a concrete bridge-negative example; the current repo gate logic exposes `UnsupportedBridgeShape` for projection-feature or unsupported-bridge cases in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted>`.
- Non-blocking: the contract does not name the positive read-strategy provider, but the current concrete selected-strategy lane is SQLite per `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-33`.

Risky assumptions
- Developers need to follow the authoritative delivery-contract block over the legacy release-note text in `.gicket/tickets/06F2PGQ6T5TGNWCBQBX3700D84/description.md:85-89`; otherwise scope could drift into the downstream v0.16 documentation ticket.
- Read-strategy coverage should assume only currently registered provider-specific read implementations and not infer new non-SQLite optimized read support from the broader save-strategy provider matrix.

AC / test suggestions
- Use SQLite as the positive read-strategy selection case and include at least one provider-neutral fallback case for latest/as-of, PIT, and bridge requests.
- Assert candidate ordering by descending `Priority` and registration-order tie breaks, matching `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:959-965` and the dispatch behavior exercised in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:9-66`.
- Snapshot `DataVaultDiagnosticsResult.ToDisplayString()` output so both strategy-status strings and selected-strategy names stay locked when the public diagnostics text changes.

Implementation watchouts
- Reuse the existing gate-evaluation logic when producing explanation causes; the drift risk named in the contract is real, and the repository's current read-side truth lives in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted>`.
- Keep the work additive on `DataVaultDiagnosticsResult`, `IDataVaultDiagnosticsService`, and `IDataVaultReadDiagnosticsService`; the contract explicitly says to complete the existing surface rather than add a parallel explain artifact.
- Do not change dispatch semantics, provider thresholds, or fallback behavior; the story is bounded to observational diagnostics only.

Non-blocking notes
- `README.md:438` currently documents request-bound save-strategy diagnostics, while `README.md:490` already mentions SQLite optimized read dispatch; keeping those user-facing docs aligned is implementation follow-through, not a PO blocker.
- The ticket comment history under `.gicket/tickets/06F2PGQ6T5TGNWCBQBX3700D84/comments/` is operational/bot-only; no reviewer or stakeholder comment introduces unresolved scope or acceptance conflicts.

Split recommendations
- No split recommended; outbound `blocks` relations already separate telemetry `06F2PGQBGNZPEEJE4KBET4JG24`, support bundle `06F2PGQJ7THHNSYYBFFPBG4174`, and v0.16 documentation `06F2PGQQJB5FJGDB16M2G7CPCM`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment