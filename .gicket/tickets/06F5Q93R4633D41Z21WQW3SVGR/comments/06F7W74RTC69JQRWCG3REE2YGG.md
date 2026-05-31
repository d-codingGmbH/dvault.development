[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The stale incoming `blocks` relation was materially addressed by queuing removal of relation `06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks` as outbox `mutation-3848c5922287e32c` on source-ticket branch `ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan`; this contract now treats closure as waiting on replay plus any remaining child completion, not on the already-done source ticket itself.
- critic-item-2: `answered` - The relation is not intentionally retained. No replacement blocker against done ticket `06F5Q93H60W6X8FJ88PWTR6NG4` was created; any remaining wait state is child-ticket completion and queued replay completion, not an active `blocks` dependency from a completed ticket.
- critic-item-3: `answered` - The persisted `.gicket/relations/...--blocks.json` file still appears in the current scratch snapshot because branch ownership routed the mutation to the source-ticket owner branch, but that finding is now explicitly superseded by durable replay `mutation-3848c5922287e32c`; the remaining issue is operational replay completion, not unresolved PO scope.

Clarifications
- Queued relation cleanup was materialized: remove relation `06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks` via outbox `mutation-3848c5922287e32c` for replay on `ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan`.
- This epic remains a tracking-only five-child v0.23.0 coordination ticket; no new child tickets, attachments, or planning documents were needed.
- Repository evidence continues to align with the existing split across `docs/architecture/dvault-v1-activity-tracing-contract.md`, `src/DCoding.Data.DVault/DataVaultActivityTracing.cs`, `docs/performance-profiles.md`, `benchmark-summary.md`, `benchmark-summary.json`, `docs/releases/v0.23.0.md`, and `README.md`.
- Recent comments and closure evidence amendments remain empty, so no human-side scope change was introduced in this pass.

Scope In
- Track the authoritative Activity tracing contract and redaction rules through child ticket `06F5Q93YXHSKABD2SABWY85S78` and the landed contract document `docs/architecture/dvault-v1-activity-tracing-contract.md`.
- Track listener-driven save/read Activity spans and PIT/bridge maintenance spans using one shared `DCoding.Data.DVault` tracing vocabulary.
- Track benchmark-backed adopter guidance for four practical performance profiles using `docs/performance-profiles.md` and the checked-in benchmark artifact triplet.
- Track coordinated v0.23.0 public-doc consistency across `README.md`, `docs/releases/v0.23.0.md`, `docs/performance-profiles.md`, and the production adoption checklist.

Scope Out
- No exporter, collector, dashboard, alerting, scheduler, hosted worker, database/container provisioning, credential management, or package-publication automation work.
- No raw business keys, hash keys, payload values, record sources, SQL text, query plans, connection strings, provider messages, exception messages, or stack traces in tracing outputs.
- No provider-strategy redesign, benchmark-harness redesign, public persistence API rewrite, or change to `AddDVault()` default telemetry-free behavior.

Open questions
- none

Follow-up questions
- After outbox `mutation-3848c5922287e32c` replays on `ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan`, confirm the live relation graph no longer contains the stale incoming `blocks` edge.
- Before runtime closes the epic, confirm whether any of the five child tickets need reopen work despite the landed repository surfaces.

Risks
- Until queued replay lands, repository snapshots can still show `.gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json`, which can confuse closure checks even though the PO cleanup action has been materialized.
- Two child-ticket reads previously hit the structured result bytes cap, so live child status should still be confirmed before final closure if runtime does not already enforce that.
- If a contract or tracing child reopens with scope beyond the fixed tracing vocabulary or evidence-bound performance posture, the epic may need re-scoping rather than simple closure.

Split recommendations
- No additional split recommended; the existing five-child decomposition remains bounded and matches the landed repository surfaces.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment