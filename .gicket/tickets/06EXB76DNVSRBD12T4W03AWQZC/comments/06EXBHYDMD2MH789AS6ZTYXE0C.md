[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB76DNVSRBD12T4W03AWQZC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB76DNVSRBD12T4W03AWQZC`.
- Optimistic claim succeeded (`expectedRevision=06EXBF7AFNCATNRY2T6CMJQSK4`, `currentRevision=06EXBHC7D94FKFENY925KM0PK0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB76DNVSRBD12T4W03AWQZC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB76DNVSRBD12T4W03AWQZC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB76DNVSRBD12T4W03AWQZC-task-design-stable-hashing-contract' from source '8f2fe8fefa2da85209c500bd2dbc1ac3978f832d'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Changing the default algorithm or normalization after hashes are persisted would create compatibility work, so the v1 contract should be treated as stable once consumed.
- Ambiguous structured input normalization can cause non-reproducible hashes across runtimes if field ordering, null handling, or culture-specific formatting are not made explicit.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `18794`
- cached-tokens: `12160`
- effective-cache-ratio: `0.6470`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c6b20f366eda413690708d4911185969`
- completed-at-utc: `<redacted>-28T20:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB76DNVSRBD12T4W03AWQZC/runs/20260428T202853966Z-c6b20f366eda413690708d4911185969.json`