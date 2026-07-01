[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8REKX113JRZQ42HEB1NVZ8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8REKX113JRZQ42HEB1NVZ8`.
- Optimistic claim succeeded (`expectedRevision=06FH8SE170PKM41SVKHGB4H4AW`, `currentRevision=06FHQ8CW2DZE60TCY2VGE971N0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8REKX113JRZQ42HEB1NVZ8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8REKX113JRZQ42HEB1NVZ8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8REKX113JRZQ42HEB1NVZ8-task-record-provider-parity-benchmark-evidence-a' from source '08d137c3b825b229ef8444d6f16520f244b958bf'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The current ticket description still reads like a request to run or collect benchmarks, which could duplicate the already checked-in closure bundle if the scope is not ratified.
- Because the repository-root benchmark-summary files still show skipped optional-provider rows, reviewers can misread placeholders as missing evidence unless the closure bundle and matrices stay explicit.
- The accepted DB2 PIT maintenance lane is not yet materialized as a child ticket, so that future work can get lost between documentation closure and later delivery.
- Historical block relations from done tickets can confuse workflow history until relation cleanup happens.
- Split recommendation: Do not split the current ticket further for save, latest-satellite, PIT, or bridge work; those implementation lanes are already handled by sibling tickets.
- Split recommendation: Create at most one additional child only if the team wants to pursue DB2 PIT maintenance now, and limit it to IDataVaultProviderPitMaintenanceStrategy push-down for IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...).
- Split recommendation: Keep Oracle PIT maintenance, MySQL PIT maintenance timing evidence, bridge-maintenance push-down, staged DB2 bulk, and provider-native chunk execution as separate later tickets rather than enlarging this documentation ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9220`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `621e17d78f1249d5b53660a509b88471`
- completed-at-utc: `<redacted>-01T03:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8REKX113JRZQ42HEB1NVZ8/runs/20260701T031145781Z-621e17d78f1249d5b53660a509b88471.json`