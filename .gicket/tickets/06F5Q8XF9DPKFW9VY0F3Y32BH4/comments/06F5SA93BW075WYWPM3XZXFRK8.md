[gicket-bot] PO-critic review contract

Summary
- Contract is sufficiently refined for developer handoff: scope, split boundaries, diagnostics expectations, and test baseline are explicit; only non-blocking edge-case examples remain to pin.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q8XF9DPKFW9VY0F3Y32BH4/description.md lines 30-52 define six acceptance criteria, four Definition of Done items, and ## Open Questions is explicitly none.
- .gicket/tickets/06F5Q8X261DQHG7N1445NGXB5W/ticket.json shows the contract predecessor ticket is done, and .gicket/relations/5W/H4/06F5Q8X261DQHG7N1445NGXB5W--06F5Q8XF9DPKFW9VY0F3Y32BH4--blocks.json records that it blocks this story.
- docs/architecture/dvault-v1-streaming-explicit-save-contract.md lines 8-21 define the additive chunked IDataVaultSaveService.SaveAsync(..., DataVaultChunkedSaveRequest, ...) boundary, and lines 62-68 assign bounded retained-state diagnostics to follow-on implementation stories rather than the contract ticket.
- tests/DCoding.Data.DVault.Tests/Unit/StreamingExplicitSaveContractSnapshotTests.cs lines 16-30 pin the chunked contract fixture, and lines 118-156 assert the architecture document and baseline compatibility evidence remain present.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs lines <redacted> already carry executable chunked-contract fixtures for bulk-order equivalence, cancellation before later chunks, current-transaction participation, repeated hub/link reuse, and satellite hash-diff continuity across chunks.
- src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs lines 4-131 and src/DCoding.Data.DVault/DataVaultDiagnostics.cs lines 34-94 and 320-365 show existing bounded telemetry and finite fallback-cause vocabulary named in the implementation notes: DataVaultSaveTelemetrySummary, DataVaultSaveStrategyDiagnosticsStatus, and DataVaultSaveStrategyFallbackCauseKind.
- git log develop..ticket/06F5Q8XF9DPKFW9VY0F3Y32BH4-story-bound-streaming-hash-state-and-memory-diag shows only PO and PO-critic claim and handoff commits (20a394517, 3551f8905, 25273bf47, 59a5910ed), and git diff --stat develop..ticket/... changes only .gicket ticket metadata/comments with no src/ or tests/ edits, so this is still a pre-development refinement branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete example pins a multi-active satellite continuity case where the same parent spans chunks and driving-key inputs arrive in different but canonically equivalent orders.
- No concrete unsupported or unbounded retained-state example names the expected rejection or bounded-fallback cause kind.
- The diagnostics AC mentions total and processed chunk counts, but it does not explicitly say how empty chunks contribute to each count.

Risky assumptions
- Repository search for DataVaultChunkedSaveRequest and DataVaultSaveChunk hits docs/tests only and not src/, so this story assumes sequencing with the actual chunked API implementation rather than an already-landed public type.
- The team can classify unsupported memory-sensitive shapes into a finite cause set without reopening sibling remediation story 06F5Q8XPXEQPJTKGJ7BQGCY438.

AC / test suggestions
- Add one explicit acceptance-test example for multi-active continuity across chunk boundaries with canonical driving-key normalization.
- Add one explicit acceptance-test example for an unsupported shape and name the exact finite fallback or rejection cause kind expected in diagnostics.
- Clarify the expected total chunk count versus processed chunk count values when the request contains empty chunks.

Implementation watchouts
- Reuse the existing bounded telemetry and fallback vocabulary instead of inventing a parallel diagnostics surface.
- Keep retained state strictly per attempt and release it on success, failure, and cancellation before any caller-owned DbContext reuse.
- Preserve the ordered-bulk semantic baseline already modeled by SaveChunkedContractAsync in ExplicitDataVaultSaveServiceSqliteTests.cs when the real chunked API lands.
- Do not emit raw hash keys, payload values, or unbounded per-parent listings in any diagnostic summary.

Non-blocking notes
- .gicket/relations/H4/38/06F5Q8XF9DPKFW9VY0F3Y32BH4--06F5Q8XPXEQPJTKGJ7BQGCY438--blocks.json and .gicket/relations/H4/7W/06F5Q8XF9DPKFW9VY0F3Y32BH4--06F5Q8XXSBGW1B8RDRMGVF557W--blocks.json confirm this story is the upstream source of bounded state and diagnostic facts for the remediation and benchmark follow-on stories.
- The refined contract already narrows scope away from contract redesign, generic execution-pipeline ownership, remediation prose, benchmark evidence, and advanced tuning hooks, which keeps the handoff boundary crisp.

Split recommendations
- No additional split is recommended; the current epic split across contract 06F5Q8X261DQHG7N1445NGXB5W, execution 06F5Q8X8Q72TQ5B7F2JSAJWPR8, remediation 06F5Q8XPXEQPJTKGJ7BQGCY438, and benchmark 06F5Q8XXSBGW1B8RDRMGVF557W is still coherent.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment