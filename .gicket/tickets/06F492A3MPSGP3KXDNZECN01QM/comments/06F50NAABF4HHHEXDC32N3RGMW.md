[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492A3MPSGP3KXDNZECN01QM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492A3MPSGP3KXDNZECN01QM`.
- Optimistic claim succeeded (`expectedRevision=06F4NV09S4GDJZR2V165EWTCZM`, `currentRevision=06F50KZBX9GQPVNDGJBATWDTGC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492A3MPSGP3KXDNZECN01QM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492A3MPSGP3KXDNZECN01QM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight' from source '2a30493fc4f1af75f76c9d1eb6b40c82eb07055b'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Future doc or feature work could blur the consumer-owned design-time boundary and imply unsupported DVault-owned CLI or automatic workflow behavior.
- Provider-specific and live-schema operational coverage beyond the current bounded baseline remains environment-dependent and should not be treated as implicit default evidence.
- Split recommendation: No further split is recommended; the epic is already decomposed into nine persisted child tickets and the current split matches the repository evidence.
- Split recommendation: Keep any future tutorial or sample-app expansion as a new follow-up ticket instead of reopening this epic.
- Split recommendation: Keep performance-evidence and reporting expansion under epic `06F492BTNHRPBC7D24E13ECFKM` and story `06F492BZPP5YT9SJSPDHQBGF3R`, not under this EF safety and preflight epic.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7894`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `696e5317f53a418294a3f089ddb046cc`
- completed-at-utc: `<redacted>-22T15:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492A3MPSGP3KXDNZECN01QM/runs/20260522T153707411Z-696e5317f53a418294a3f089ddb046cc.json`