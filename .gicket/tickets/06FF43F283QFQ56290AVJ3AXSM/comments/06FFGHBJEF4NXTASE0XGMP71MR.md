[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43F283QFQ56290AVJ3AXSM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43F283QFQ56290AVJ3AXSM`.
- Optimistic claim succeeded (`expectedRevision=06FF44JT9FTTD2WES9VF2S7GW0`, `currentRevision=06FFGF6CV7HCZH347PZJHKJTJR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43F283QFQ56290AVJ3AXSM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43F283QFQ56290AVJ3AXSM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43F283QFQ56290AVJ3AXSM-task-document-provider-pit-expansion-decision-ma' from source 'f7744c740beb860f2ed583f2bbf373256f9bc535'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Oracle has completed read-side PIT evidence but still lacks PIT maintenance implementation, diagnostics, SQL parity proof, and rollback-clean failure proof; the matrix must not over-promote Oracle from read timing to write-side feasibility.
- MySQL and DB2 accepted lanes remain conditional on rollback-clean delete-plus-insert behavior and provider-specific savepoint limits; overstating ambient-transaction support would widen scope beyond current evidence.
- Live incoming blocks relations from the done MySQL, Oracle, and DB2 evaluation tickets can confuse downstream readers unless the matrix explicitly treats them as historical completion context rather than active blockers.
- Split recommendation: Reuse existing MySQL implementation ticket 06FFDG522514HX2J17GT9VE77W as the only MySQL follow-up child for this matrix.
- Split recommendation: Create one DB2 implementation ticket limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy.
- Split recommendation: Do not create an Oracle implementation child ticket from this matrix; keep Oracle on provider-neutral PIT maintenance until a separate Oracle-specific reopen proves SQL parity and rollback-clean behavior.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9371`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `badb02861acb48efa8fd91b6e050e1f8`
- completed-at-utc: `<redacted>-24T06:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43F283QFQ56290AVJ3AXSM/runs/20260624T061601388Z-badb02861acb48efa8fd91b6e050e1f8.json`