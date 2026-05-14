[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ25KK4VY4MYJSDG9V4BZM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ25KK4VY4MYJSDG9V4BZM`.
- Optimistic claim succeeded (`expectedRevision=06F1XTQEN3FQGSJC75CDER3D8G`, `currentRevision=06F2HDB8B6RF546530BRWZ7MY0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ25KK4VY4MYJSDG9V4BZM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ25KK4VY4MYJSDG9V4BZM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample' from source '67f54b6168949878ad8dfeb9abc359f7f49081a1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample` as `572a7739a04d`.

Open questions / Risiken
- Container guidance can drift from README if it implies DVault provisions databases by default; documentation must preserve the opt-in boundary.
- Hardcoded ports or credentials can conflict with developer machines; the sample should make overrides and cleanup clear.
- Podman and Docker networking differ on some hosts, so the sample should call out connection-string adjustment rather than hiding runtime-specific assumptions.
- Split recommendation: No new split is recommended. This task is already the bounded first-provider sample under parent story 06F1XQ1VWEX0WPAXE78FHSWJ8G, while the full provider matrix remains out of scope.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9089`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b1de8d46be3b436fac121b90754d36ee`
- completed-at-utc: `<redacted>-14T23:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ25KK4VY4MYJSDG9V4BZM/runs/20260514T230237154Z-b1de8d46be3b436fac121b90754d36ee.json`