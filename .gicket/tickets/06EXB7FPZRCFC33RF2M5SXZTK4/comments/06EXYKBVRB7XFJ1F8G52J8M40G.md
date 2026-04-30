[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7FPZRCFC33RF2M5SXZTK4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FPZRCFC33RF2M5SXZTK4`.
- Optimistic claim succeeded (`expectedRevision=06EXYHHEJ8GQ83BRPT2SBW1634`, `currentRevision=06EXYHKS90HD1NMHGRZCTGRKRG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7FPZRCFC33RF2M5SXZTK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7FPZRCFC33RF2M5SXZTK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7FPZRCFC33RF2M5SXZTK4-task-add-modelbuilder-extension-for-dvault-conve' from source 'b4dc68abfb55c6db71c75344537df749f14786ca'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7FPZRCFC33RF2M5SXZTK4-task-add-modelbuilder-extension-for-dvault-conve` as `77e76a1e3ec7`.

Open questions / Risiken
- The new EF Core package reference must stay aligned with the repository's net10.0 baseline to avoid restore or build drift.
- Once shipped, annotation key DCoding.Data.DVault:Conventions becomes a public contract and should not be renamed casually because tests and downstream EF work may rely on it.
- There is still a namespace and overload-resolution risk alongside the existing Modeling.DataVaultModelBuilderExtensions.UseDataVault, so the EF extension must remain typed specifically for Microsoft.EntityFrameworkCore.ModelBuilder in the root namespace.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7581`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6a91b023601e4f1f852027ac8e86c531`
- completed-at-utc: `<redacted>-30T16:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FPZRCFC33RF2M5SXZTK4/runs/20260430T165129748Z-6a91b023601e4f1f852027ac8e86c531.json`