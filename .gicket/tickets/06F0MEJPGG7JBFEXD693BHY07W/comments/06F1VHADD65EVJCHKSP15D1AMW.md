[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEJPGG7JBFEXD693BHY07W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEJPGG7JBFEXD693BHY07W`.
- Optimistic claim succeeded (`expectedRevision=06F0QH2W0837HB7YNWGPMR0KQC`, `currentRevision=06F1VGD21K0KC4C67T05B1TY64`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEJPGG7JBFEXD693BHY07W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEJPGG7JBFEXD693BHY07W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEJPGG7JBFEXD693BHY07W-task-update-docs-and-release-notes-for-v0-7-0-mo' from source '9ab5e5db807665ee759627a350b43113cdaf2de0'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The main documentation risk is overstating PIT or bridge capabilities beyond the implemented provider-neutral read behavior; examples should stay narrowly tied to current code.
- Benchmark wording can become misleading if it summarizes planned optimizations instead of measured evidence already present in the branch.
- Release-note compatibility text should avoid making v0.6.0 historical limitations sound like current v0.7.0 behavior.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25425`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0957`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ba5e3a92753b446b931b6b7f5cb2c21d`
- completed-at-utc: `<redacted>-12T19:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEJPGG7JBFEXD693BHY07W/runs/20260512T195849556Z-ba5e3a92753b446b931b6b7f5cb2c21d.json`