[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZSCGZBKAC4YZH5SY3NX68'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZSCGZBKAC4YZH5SY3NX68`.
- Optimistic claim succeeded (`expectedRevision=06F9JF7W9Q59WGS1X3QT2YCHA8`, `currentRevision=06F9M7CA6ERXK9BCAHJZ5QVF1C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZSCGZBKAC4YZH5SY3NX68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZSCGZBKAC4YZH5SY3NX68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZSCGZBKAC4YZH5SY3NX68-task-add-opentelemetry-examples-for-dvault-activ' from source '661ef50887011b7e7cee636a912faeba1ac83459'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZSCGZBKAC4YZH5SY3NX68-task-add-opentelemetry-examples-for-dvault-activ` as `25022731a1f3`.

Open questions / Risiken
- The root `README.md` already contains detailed tracing and telemetry prose; if `examples/README.md` repeats too much contract detail, the docs can drift unless the new section stays compact and link-first.
- `examples/README.md` currently shows `0.16.0` package-version examples while the root README installation baseline is `0.30.0`; touching that file without care could preserve stale version guidance even though version alignment is not the main scope of this ticket.
- An overly concrete OpenTelemetry snippet could accidentally imply DVault-owned package, exporter, or backend responsibilities; the wording must keep all such integration choices explicitly application-owned.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8892`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `88877683988843b898a85b94a8583488`
- completed-at-utc: `<redacted>-05T23:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZSCGZBKAC4YZH5SY3NX68/runs/20260605T233553730Z-88877683988843b898a85b94a8583488.json`