[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7TP9PF2XFRQ9MG7CJQR10'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7TP9PF2XFRQ9MG7CJQR10`.
- Optimistic claim succeeded (`expectedRevision=06EYNZN6PN6TN5EZ7JENMJTSXM`, `currentRevision=06EYNZRS4PVQHCR06AZ46EA3BG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7TP9PF2XFRQ9MG7CJQR10': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7TP9PF2XFRQ9MG7CJQR10': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7TP9PF2XFRQ9MG7CJQR10-task-emit-benchmark-artifacts-suitable-for-docum' from source 'de422d91c136651be48b7705e955aa321b798185'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7TP9PF2XFRQ9MG7CJQR10-task-emit-benchmark-artifacts-suitable-for-docum` as `9d5c4bcef580`.

Open questions / Risiken
- Benchmark numbers vary by machine, so docs can mislead if artifact context is incomplete or stripped when results are copied.
- If generated benchmark outputs are later committed without a refresh policy, docs may cite stale measurements even when the artifact format is correct.
- Split recommendation: No split recommended; artifact emission and benchmark-documentation updates share one bounded surface in the existing benchmark runner and benchmark README.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `62539`
- cached-tokens: `10624`
- effective-cache-ratio: `0.1699`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `272ae77de8404f16a7cea5bfd9e55cba`
- completed-at-utc: `<redacted>-02T23:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7TP9PF2XFRQ9MG7CJQR10/runs/20260502T232735689Z-272ae77de8404f16a7cea5bfd9e55cba.json`