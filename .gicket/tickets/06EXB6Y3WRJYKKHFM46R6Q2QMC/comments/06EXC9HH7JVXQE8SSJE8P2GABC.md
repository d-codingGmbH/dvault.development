[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB6Y3WRJYKKHFM46R6Q2QMC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6Y3WRJYKKHFM46R6Q2QMC`.
- Optimistic claim succeeded (`expectedRevision=06EXBF7PS09P5T2BMKB7R3P8Z4`, `currentRevision=06EXC8XVQBB5384ADDFD6E4SB8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB6Y3WRJYKKHFM46R6Q2QMC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB6Y3WRJYKKHFM46R6Q2QMC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB6Y3WRJYKKHFM46R6Q2QMC-task-add-test-projects-and-shared-test-utilities' from source '8698a526da108ac00c6ccd132fea0128d362ac89'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If the repository does not yet have a solution or central test entry point, the developer may need to add the smallest conventional .NET wiring needed for dotnet test discovery.
- Package versions for the test framework and Sqlite provider should be kept consistent with any central package management conventions discovered during implementation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `18840`
- cached-tokens: `2432`
- effective-cache-ratio: `0.1291`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `db3fdbb24e4e473ea6f6da4ce78a999b`
- completed-at-utc: `<redacted>-28T22:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6Y3WRJYKKHFM46R6Q2QMC/runs/20260428T221159784Z-db3fdbb24e4e473ea6f6da4ce78a999b.json`