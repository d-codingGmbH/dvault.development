[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q916BXE2N372SWMH1X776G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q916BXE2N372SWMH1X776G`.
- Optimistic claim succeeded (`expectedRevision=06F5Q98XDTJAK85TBWXF47E43C`, `currentRevision=06F6HTPRT439XPF9HTH12AG4SC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q916BXE2N372SWMH1X776G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q916BXE2N372SWMH1X776G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q916BXE2N372SWMH1X776G-story-add-delete-aware-bridge-maintenance-for-to' from source 'd519d7b65e8a450f636a4168d594f61ff4bdd0ca'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q916BXE2N372SWMH1X776G-story-add-delete-aware-bridge-maintenance-for-to` as `4c49f30d2ab8`.

Open questions / Risiken
- The current implementation loads source-link rows and bridge rows into memory before reconciling; the new delete-aware path keeps that whole-bridge cost profile unless a later optimization ticket changes it.
- This story changes a public service contract and multiple published docs, so compatibility depends on keeping `MaintainBridgeAsync(...)` behavior stable while introducing the new delete-aware path additively.
- Split recommendation: No split recommended; repository evidence shows one cohesive service, test, and documentation change centered on the existing whole-bridge desired-row computation and public maintenance boundary.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.5694`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `e09b277b3698471394f31ebf3919f544`
- completed-at-utc: `<redacted>-27T10:24:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q916BXE2N372SWMH1X776G/runs/20260527T102427380Z-e09b277b3698471394f31ebf3919f544.json`