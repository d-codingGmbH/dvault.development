[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RC9F0QEWF356WF7YYNNGM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RC9F0QEWF356WF7YYNNGM`.
- Optimistic claim succeeded (`expectedRevision=06FH8SFE0NKZ55DDHF2J0D0RE8`, `currentRevision=06FHPDB8MWTYXWY1YPYJT8ZASM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RC9F0QEWF356WF7YYNNGM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RC9F0QEWF356WF7YYNNGM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RC9F0QEWF356WF7YYNNGM-task-close-selected-provider-save-strategy-parit' from source '684cd29a52d5387bd646d15e1cc074e5ee58708b'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RC9F0QEWF356WF7YYNNGM-task-close-selected-provider-save-strategy-parit` as `be5568a35e8a`.

Open questions / Risiken
- The current ticket description still reads like a fresh implementation discovery task; without this refinement, downstream work could rerun already closed save evidence or reopen settled thresholds.
- Because the repository already contains closed save evidence rows, implementers may overreach into read or PIT-maintenance work unless the save-only boundary stays explicit.
- Future work can accidentally widen DB2 or Oracle scope if staged bulk or provider-native chunk execution is treated as implied parity rather than as separate evidence-gated follow-up.
- Split recommendation: Do not split this ticket further by provider; the current repository evidence and shared save-gate surface keep PostgreSQL, SQL Server, MySQL, Oracle, and DB2 within one bounded save-parity task.
- Split recommendation: Keep read-path work in sibling ticket 06FH8RDS25081N5S181C7TQGTG and documentation or evidence publication work in 06FH8REKX113JRZQ42HEB1NVZ8.
- Split recommendation: If the team wants to pursue DB2 PIT full-rebuild maintenance, open one separate child limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) through IDataVaultProviderPitMaintenanceStrategy.
- Split recommendation: Any future Oracle staged bulk, staged DB2 bulk, provider-native chunk execution, or maintenance-evidence expansion should be separate later tickets rather than enlarging this save task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8891`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7558ea8533b84c2fbdfa037bed8b44f8`
- completed-at-utc: `<redacted>-01T01:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RC9F0QEWF356WF7YYNNGM/runs/20260701T011339102Z-7558ea8533b84c2fbdfa037bed8b44f8.json`