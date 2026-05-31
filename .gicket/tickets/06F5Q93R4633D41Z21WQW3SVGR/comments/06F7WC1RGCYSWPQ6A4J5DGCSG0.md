[gicket-bot] PO refinement contract

Summary
- Queued stale-blocker removal exists, but the epic still has a live incoming `blocks` relation, so it remains in PO clarification until replay or equivalent cleanup is directly evidenced and PO-critic is rerun.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `cannot_answer` - Not satisfied in the current evidence. The remove-relation mutation was queued as outbox `mutation-3848c5922287e32c`, but there is no direct evidence that replay or equivalent cleanup has landed, and the live graph still shows the incoming `blocks` edge.
- critic-item-2: `cannot_answer` - Not ready yet. PO-critic should be rerun only after a fresh relation read verifies that the stale incoming `blocks` edge has been removed from the live graph.
- critic-item-3: `answered` - Confirmed. Direct local evidence still shows a live incoming `blocks` relation into the epic, so the epic does not meet its own closure condition yet.

Clarifications
- Queued cleanup exists but is not closure evidence: `gicket-remove-relation` queued outbox `mutation-3848c5922287e32c` to source-ticket branch `ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan`.
- Current live relation evidence still shows one incoming `blocks` edge into the epic: `06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks`.
- No new child tickets, attachments, planning documents, or relation writes beyond the already queued removal were justified in this pass.
- Repository evidence still supports the existing five-child split across `docs/architecture/dvault-v1-activity-tracing-contract.md`, `src/DCoding.Data.DVault/DataVaultActivityTracing.cs`, `docs/performance-profiles.md`, `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, `docs/releases/v0.23.0.md`, and `README.md`.

Scope In
- Track the authoritative Activity tracing contract and redaction rules through child ticket `06F5Q93YXHSKABD2SABWY85S78` and `docs/architecture/dvault-v1-activity-tracing-contract.md`.
- Track save/read and PIT/bridge maintenance tracing against the shared `DCoding.Data.DVault` tracing vocabulary evidenced by `src/DCoding.Data.DVault/DataVaultActivityTracing.cs`.
- Track benchmark-backed adopter guidance for four practical performance profiles through `docs/performance-profiles.md` and the checked-in `benchmark-summary.md` / `benchmark-summary.csv` / `benchmark-summary.json` triplet.
- Track coordinated v0.23.0 documentation consistency across `README.md`, `docs/releases/v0.23.0.md`, `docs/performance-profiles.md`, and child ticket `06F5Q94SQ086B2DZ1AKFDXGV94`.

Scope Out
- No exporter, collector, dashboard, alerting, scheduler, hosted worker, database or container provisioning, credential management, or package-publication automation work.
- No raw business keys, hash keys, payload values, record sources, SQL text, query plans, connection strings, provider messages, exception messages, or stack traces in tracing outputs.
- No provider-strategy redesign, benchmark-harness redesign, public persistence API rewrite, or change to `AddDVault()` default telemetry-free behavior.
- No direct product-code or documentation edits in this epic; authoritative implementation and doc changes stay in the existing child tickets.

Open questions
- Has outbox `mutation-3848c5922287e32c` replayed on `ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan`, or has equivalent cleanup landed so the live graph no longer contains `06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks`?
- After relation cleanup lands, does a fresh `gicket-read-ticket-relations` read for `06F5Q93R4633D41Z21WQW3SVGR` show no incoming `blocks` relation so the epic can return to PO-critic against the cleaned graph?

Follow-up questions
- Before runtime closes the epic, confirm none of the five child tickets need reopen work despite the landed repository surfaces.
- If runtime does not already enforce live child-status checks, confirm the two previously bytes-capped child reads against current persisted ticket state before final closure.

Risks
- The live relation graph still contains an incoming `blocks` edge, so any closure or PO-critic pass run now will fail against the epic's own closure condition.
- Because replay targets another ticket branch, the queued outbox may remain unlanded until that branch processes pending mutations.
- If a child ticket reopens with scope beyond the fixed tracing vocabulary or evidence-bound performance posture, the epic may need re-scoping rather than simple closure.

Split recommendations
- No additional split recommended; the existing five-child decomposition remains bounded and matches the visible repository surfaces.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment