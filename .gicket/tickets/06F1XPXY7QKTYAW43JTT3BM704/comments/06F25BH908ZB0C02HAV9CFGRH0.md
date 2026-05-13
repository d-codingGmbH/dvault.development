[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPXY7QKTYAW43JTT3BM704'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPXY7QKTYAW43JTT3BM704`.
- Optimistic claim succeeded (`expectedRevision=06F1XTPPXTE8KNK8FGB2KJZDBR`, `currentRevision=06F259E832NCS6ZY9RYGMKMZH4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPXY7QKTYAW43JTT3BM704': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPXY7QKTYAW43JTT3BM704': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPXY7QKTYAW43JTT3BM704-task-implement-first-read-helper-api-slice-and-t' from source 'bf07771987b6f56ff0547f4c4d8018d82a6bcc39'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If the typed helper is implemented by post-processing raw records instead of sharing the projection pipeline, provider-strategy behavior can drift from raw reads.
- The exact-name projector contract creates a public naming surface; documentation, API snapshots, and diagnostics must stay synchronized to avoid confusing breaks.
- The ticket title is broad enough to invite PIT or bridge expansion; the implementation needs to stay anchored to latest/as-of satellite projections to preserve scope.
- Split recommendation: Split PIT-backed typed read helpers into a separate ticket if work extends beyond latest/as-of satellite projections.
- Split recommendation: Split bridge traversal typed helpers and any bridge-specific diagnostics into a separate ticket.
- Split recommendation: Split reflection-based DTO binding or additional non-string projection accessor families into a separate ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8981`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f221311bc89e474d83343d4467b3335f`
- completed-at-utc: `<redacted>-13T18:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPXY7QKTYAW43JTT3BM704/runs/20260513T185138969Z-f221311bc89e474d83343d4467b3335f.json`