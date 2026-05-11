[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEH660Y5QTNR5P8JPS2QXC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEH660Y5QTNR5P8JPS2QXC`.
- Optimistic claim succeeded (`expectedRevision=06F0QH40X30EWBG5YD5ERYMQM0`, `currentRevision=06F1GDD4G9ZVTXSNRRFKZRFPBR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEH660Y5QTNR5P8JPS2QXC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEH660Y5QTNR5P8JPS2QXC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEH660Y5QTNR5P8JPS2QXC-task-implement-provider-neutral-pit-snapshot-rea' from source 'c8b0b02f396558067c9da490d906ce9d0672af43'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEH660Y5QTNR5P8JPS2QXC-task-implement-provider-neutral-pit-snapshot-rea` as `31a9d7647889`.

Open questions / Risiken
- If generated PIT entities do not already expose the expected metadata annotations or snapshot-reference columns consistently, implementation may spill into separate modeling/projection work rather than staying a pure read-service task.
- Joining multiple satellites through PIT snapshot references may surface provider-neutral EF translation edge cases across timestamp storage modes, so the failure-mode and timestamp-option test matrix needs to stay explicit.
- The current release-note baseline still says PIT-backed read APIs are not delivered, so public API completion here must stay coordinated with the next release packaging/documentation pass.
- Split recommendation: No additional split is recommended now; the existing contract already bounds v1 to one hub-parent PIT read shape and leaves provider-specific optimization, PIT maintenance, bridge traversal, and multi-active cases for later work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `29699`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0819`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f1e0d0700a23447db5d4ebae87a458ea`
- completed-at-utc: `<redacted>-11T18:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEH660Y5QTNR5P8JPS2QXC/runs/20260511T181225709Z-f1e0d0700a23447db5d4ebae87a458ea.json`