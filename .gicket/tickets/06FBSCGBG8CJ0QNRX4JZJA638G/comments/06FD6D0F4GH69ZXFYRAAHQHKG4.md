[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCGBG8CJ0QNRX4JZJA638G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCGBG8CJ0QNRX4JZJA638G`.
- Optimistic claim succeeded (`expectedRevision=06FD1D2XT8YV494TQF58WM25A0`, `currentRevision=06FD68M0WK7WVJ5KB6ANM8PAV0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCGBG8CJ0QNRX4JZJA638G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCGBG8CJ0QNRX4JZJA638G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps' from source '919f4d5fb5e4a8a0efb155f4c09d3ea7df0bfbb2'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The five child-description updates are durable queued mutations on their owner branches; until replay completes, the canonical child branches may briefly lag the parent contract text.
- External-provider PIT and bridge evidence still depends on configured connection strings and benchmark infrastructure, so the active-provider child tickets can still stall after refinement.
- A downstream ticket could over-claim performance if it treats skipped-placeholder or diagnostics-only rows as completed timing evidence without new benchmark artifact triplets.
- Split recommendation: Use existing child 06FBSCGGN528A2NC6TTA5A99X0 for PostgreSQL provider-configured PIT and bridge timing evidence against PostgresDataVaultReadStrategy.
- Split recommendation: Use existing child 06FBSCGNY2R6PC7P4Y91RD0HVR for SQL Server provider-configured PIT and bridge timing evidence against SqlServerDataVaultReadStrategy.
- Split recommendation: Use existing child 06FBSCGVAZ5G8NP1TRXFNEP6DW for MySQL provider-configured PIT and bridge timing evidence against MySqlDataVaultReadStrategy.
- Split recommendation: Use existing child 06FBSCH0M358R5J3RGFB6GRDM4 for Oracle provider-configured PIT and bridge timing evidence against OracleDataVaultReadStrategy.
- Split recommendation: Use existing child 06FBSCH65R88BT6PS7XV32NQ1M only as deferred DB2 planning until explicit DB2 evidence scope and environment-backed benchmark work are approved.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `66816`
- effective-cache-ratio: `0.5129`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `22e5b03f781e4d37b9c4b345510c7542`
- completed-at-utc: `<redacted>-17T01:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/runs/20260617T013104861Z-22e5b03f781e4d37b9c4b345510c7542.json`