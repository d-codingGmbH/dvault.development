[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCH0M358R5J3RGFB6GRDM4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCH0M358R5J3RGFB6GRDM4`.
- Optimistic claim succeeded (`expectedRevision=06FD6EA4TH0F5FMEEC1ZDYHG54`, `currentRevision=06FDT625HEGS47ZPEG2PF58D2C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCH0M358R5J3RGFB6GRDM4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCH0M358R5J3RGFB6GRDM4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps' from source '7b6efb6f628b57c7c0f17d91717be9c6a46b2588'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps` as `114390329f23`.

Open questions / Risiken
- Completed Oracle timing evidence still depends on a reachable configured Oracle test environment; until that run is checked in, the repository only preserves skipped placeholder rows for Oracle.
- If matrix or guidance docs are updated without matching artifact-backed verifier coverage, the repository could overstate Oracle timing claims relative to the evidence contract.
- Delivery sequencing may still depend on the existing live blocks relations even though PO clarification is complete.
- Split recommendation: No additional split is recommended; the repository already bounds this ticket to two Oracle read scenarios plus the required evidence, docs, and verifier updates.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `62149`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1215`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `13d8af8bb652486ea56af2897ffaf7cd`
- completed-at-utc: `<redacted>-18T23:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCH0M358R5J3RGFB6GRDM4/runs/20260618T234441061Z-13d8af8bb652486ea56af2897ffaf7cd.json`