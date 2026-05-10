[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEBV90FB8TQMRXJNH078BM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEBV90FB8TQMRXJNH078BM`.
- Optimistic claim succeeded (`expectedRevision=06F0QH16VSQMN66DFETN4FEHRG`, `currentRevision=06F17TZ18DJM0ZF46SDD8BEM2W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEBV90FB8TQMRXJNH078BM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEBV90FB8TQMRXJNH078BM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEBV90FB8TQMRXJNH078BM-story-add-typed-explicit-save-and-read-helpers' from source 'e6c444c78fb27fc992cafc1552a1fb5b15de9ed7'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Because IDataVaultSatelliteMapper<TSource> supports link-parent and multi-active outputs while typed save convenience helpers do not, examples and docs must make that boundary explicit to avoid caller confusion.
- Typed read projection reserves the technical names ParentHashKey, HashDiff, LoadTimestamp, and RecordSource; metadata that reuses those names as payload or driving-key names will fail fast and must stay outside this v1 surface.
- Consumers may expect a broader typed query layer once projections exist, but this story intentionally remains limited to explicit latest/as-of satellite reads by parent hash key.
- Split recommendation: If the team wants one-call orchestration that chains hub save results into satellite writes, capture that as a separate convenience-layer story rather than expanding this ticket beyond the existing explicit save boundary.
- Split recommendation: If typed save support is needed for link-parent or multi-active satellites, split that into a follow-on story with its own acceptance tests and diagnostics instead of widening this v1 ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8316`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d334b391c1bc4010999004f48ea9f45b`
- completed-at-utc: `<redacted>-10T22:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEBV90FB8TQMRXJNH078BM/runs/20260510T221622392Z-d334b391c1bc4010999004f48ea9f45b.json`