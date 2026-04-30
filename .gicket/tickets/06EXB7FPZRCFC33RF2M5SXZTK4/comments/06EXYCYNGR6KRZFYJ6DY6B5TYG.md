[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7FPZRCFC33RF2M5SXZTK4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FPZRCFC33RF2M5SXZTK4`.
- Optimistic claim succeeded (`expectedRevision=06EXNNNMNYVPBN0KAF8R3Q89B4`, `currentRevision=06EXYC8EZNJ97MFYFCB301HXSR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7FPZRCFC33RF2M5SXZTK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7FPZRCFC33RF2M5SXZTK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7FPZRCFC33RF2M5SXZTK4-task-add-modelbuilder-extension-for-dvault-conve' from source 'c5e38e99b7a08f396605add7dcb82a6eaa7e400d'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- EF Core package version selection must stay aligned with the net10.0 baseline to avoid introducing restore/build drift.
- There is a naming collision risk with the existing Modeling.DataVaultModelBuilderExtensions.UseDataVault method; placing the EF extension in the root namespace and typing it for Microsoft.EntityFrameworkCore.ModelBuilder should keep overload resolution clear.
- Because provider-specific persistence remains out of scope, tests should assert provider-neutral convention application rather than expecting generated SQL or migrations.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `40540`
- cached-tokens: `12160`
- effective-cache-ratio: `0.3000`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6f90a4a4fb22405894f7798f86e99e74`
- completed-at-utc: `<redacted>-30T16:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FPZRCFC33RF2M5SXZTK4/runs/20260430T162328758Z-6f90a4a4fb22405894f7798f86e99e74.json`