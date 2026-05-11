[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEJ0NE80R7CNS982S3PKVR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEJ0NE80R7CNS982S3PKVR`.
- Optimistic claim succeeded (`expectedRevision=06F0QH3SPXTJE3M0RXFKCV0P6G`, `currentRevision=06F1H2KRP62ABZG2F93R04PZYR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEJ0NE80R7CNS982S3PKVR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEJ0NE80R7CNS982S3PKVR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEJ0NE80R7CNS982S3PKVR-task-benchmark-latest-pit-and-bridge-reads-acros' from source '59e2e990af6f32d87dd4196a21fb2e4bdcaece8c'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Non-SQLite providers may produce sparse measured data on machines without local configuration, so the summary must distinguish measured results from deterministic skips.
- PIT and bridge read baselines are provider-neutral and may expose fixture/setup costs if the benchmark does not separate seeding from measured operations.
- Provider differences in timestamp storage or query translation can make raw numbers hard to compare unless scenario labels and fixture sizes are consistent.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25420`
- cached-tokens: `12160`
- effective-cache-ratio: `0.4784`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6ff822ad8e9a4e5499fabdbfe0dfd3d6`
- completed-at-utc: `<redacted>-11T19:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEJ0NE80R7CNS982S3PKVR/runs/20260511T193950869Z-6ff822ad8e9a4e5499fabdbfe0dfd3d6.json`