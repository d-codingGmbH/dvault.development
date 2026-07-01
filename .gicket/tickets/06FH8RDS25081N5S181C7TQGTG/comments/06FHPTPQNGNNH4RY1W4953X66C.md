[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RDS25081N5S181C7TQGTG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RDS25081N5S181C7TQGTG`.
- Optimistic claim succeeded (`expectedRevision=06FH8SFTP0XN0TN9CN7V5AN0E0`, `currentRevision=06FHPRPXEY4WPMKD3KDFCMZGVC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RDS25081N5S181C7TQGTG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RDS25081N5S181C7TQGTG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RDS25081N5S181C7TQGTG-task-close-selected-provider-latest-satellite-pi' from source '6b6b5b01973b7a2f5d0d07a3ca2796290ea33b3b'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If downstream work treats remaining fallback boundaries as open implementation gaps, this ticket will sprawl into PIT maintenance, bridge maintenance, or save-path work that already belongs elsewhere.
- The root `benchmark-summary.*` files still show skipped external-provider read rows; without explicit closure-bundle citation, reviewers can misread intentional placeholders as missing evidence.
- The stale inbound `blocks` relation from the done matrix-refresh ticket can confuse workflow history until it is cleaned up.
- Split recommendation: Do not split this ticket further; save-path work and documentation work already have separate bounded children.
- Split recommendation: If the team wants additional implementation after this ticket, create one separate DB2 PIT maintenance child limited to `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` push-down through `IDataVaultProviderPitMaintenanceStrategy`.
- Split recommendation: Any Oracle PIT maintenance reopen, MySQL PIT maintenance timing evidence, bridge-maintenance push-down, or DB2 staged-bulk follow-up should stay in separate later tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9189`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8425c358fc34484999b5a079dd139618`
- completed-at-utc: `<redacted>-01T02:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RDS25081N5S181C7TQGTG/runs/20260701T020334695Z-8425c358fc34484999b5a079dd139618.json`