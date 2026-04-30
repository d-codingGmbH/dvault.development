[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7FF1J9NR2849WKDR8DKPG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FF1J9NR2849WKDR8DKPG`.
- Optimistic claim succeeded (`expectedRevision=06EY0W2AK3AG3NFZ4KJ61G6VVC`, `currentRevision=06EY0XSZ3N5457562F6DVG7V8R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7FF1J9NR2849WKDR8DKPG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7FF1J9NR2849WKDR8DKPG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7FF1J9NR2849WKDR8DKPG-story-integrate-with-ef-core-model-building' from source '20ea6fa07e948ad57f21654cf59c934b666b2f08'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If story-level wording drifts back toward executable developer scope, automation can hand duplicate work to developers even though the child tickets are already done.
- Downstream tickets that currently use this story as a blocker may need relation hygiene after the umbrella advances to avoid stale workflow dependencies.
- Split recommendation: No additional split is recommended; the only concrete implementation slices are already separated as 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R, and both are done.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `65577`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0371`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f94f3e22991c443f8f14ae9a2f188257`
- completed-at-utc: `<redacted>-30T22:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FF1J9NR2849WKDR8DKPG/runs/20260430T221951855Z-f94f3e22991c443f8f14ae9a2f188257.json`