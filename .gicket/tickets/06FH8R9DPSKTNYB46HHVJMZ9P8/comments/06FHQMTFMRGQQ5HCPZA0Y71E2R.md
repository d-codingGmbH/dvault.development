[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8R9DPSKTNYB46HHVJMZ9P8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8R9DPSKTNYB46HHVJMZ9P8`.
- Optimistic claim succeeded (`expectedRevision=06FH8SNT89A302VAKEEKQNVX58`, `currentRevision=06FHQJWDRQWWZVTH01J33GCYD8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8R9DPSKTNYB46HHVJMZ9P8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8R9DPSKTNYB46HHVJMZ9P8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8R9DPSKTNYB46HHVJMZ9P8-story-close-provider-optimization-parity-gaps-fr' from source 'ca7d0b0f321d4113cf04ea6669f976c3d629fabf'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The current one-line ticket description still reads like broad implementation discovery; without this refinement, downstream reviewers can reopen already-closed save/read rows or ask for duplicate benchmark reruns.
- Because the repository-root benchmark-summary.* files still contain skipped optional-provider rows, reviewers can misread placeholders as missing evidence unless the closure bundle and evidence matrix remain the cited sources.
- The accepted DB2 PIT maintenance lane is not materialized as a child ticket in the current live relation set, so that future work can be lost if the team wants to pursue maintenance parity later.
- Split recommendation: Do not split save, read, or documentation scope any further; those lanes are already bounded by tickets 06FH8RC9F0QEWF356WF7YYNNGM, 06FH8RDS25081N5S181C7TQGTG, and 06FH8REKX113JRZQ42HEB1NVZ8.
- Split recommendation: If provider-maintenance expansion is prioritized, create one separate child limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy for DB2.
- Split recommendation: Keep Oracle PIT maintenance, maintenance timing evidence collection, bridge-maintenance push-down, staged DB2 bulk, provider-native chunk execution, and binary-storage remediation as separate later tickets rather than enlarging this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9133`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b6bce93a9e884f7eb8c880e046f72e1c`
- completed-at-utc: `<redacted>-01T03:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8R9DPSKTNYB46HHVJMZ9P8/runs/20260701T035741154Z-b6bce93a9e884f7eb8c880e046f72e1c.json`