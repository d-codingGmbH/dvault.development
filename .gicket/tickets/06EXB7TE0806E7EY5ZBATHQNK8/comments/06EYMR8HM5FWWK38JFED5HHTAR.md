[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7TE0806E7EY5ZBATHQNK8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7TE0806E7EY5ZBATHQNK8`.
- Optimistic claim succeeded (`expectedRevision=06EYMH9YV164A4CYPFHRG6GXZM`, `currentRevision=06EYMNG4VFJYZC0QTPBGVJ3MCW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7TE0806E7EY5ZBATHQNK8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7TE0806E7EY5ZBATHQNK8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7TE0806E7EY5ZBATHQNK8-task-add-benchmark-project-for-scenario-comparis' from source '2418c4ad567988091a8a7688bb38991eff95f0a3'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7TE0806E7EY5ZBATHQNK8-task-add-benchmark-project-for-scenario-comparis` as `80266464373c`.

Open questions / Risiken
- Benchmark numbers will still vary by developer machine; this ticket should establish deterministic relative comparison coverage, not a cross-machine performance gate.
- If the benchmark implementation drifts from the ratified order contract or accidentally times the broader reuse scenario, the reported comparison will stop representing the intended workload.
- If the conventional EF order baseline omits the shared timestamps or record-source values, cross-suite comparison drift can reappear even when both benchmarks still run.
- Split recommendation: No split recommended; the ticket remains one bounded benchmark-project task, and the order under-specification is resolved by the explicit shared contract rather than a child ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `86989`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0280`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `488e9bb086764f4aa8e2b41b3b13944f`
- completed-at-utc: `<redacted>-02T20:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7TE0806E7EY5ZBATHQNK8/runs/20260502T202842659Z-488e9bb086764f4aa8e2b41b3b13944f.json`