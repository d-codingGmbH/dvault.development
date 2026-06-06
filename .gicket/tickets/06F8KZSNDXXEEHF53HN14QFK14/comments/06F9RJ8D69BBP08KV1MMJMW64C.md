[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZSNDXXEEHF53HN14QFK14'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZSNDXXEEHF53HN14QFK14`.
- Optimistic claim succeeded (`expectedRevision=06F9JFCE1J3EPD0GPRMVJJQ9KW`, `currentRevision=06F9RFHSM99F96T0FW0W43XF4M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZSNDXXEEHF53HN14QFK14': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZSNDXXEEHF53HN14QFK14': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with' from source 'a1df9a89dd9340d951b4b9df066decd6391872f0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with` as `7571fb5141d5`.

Open questions / Risiken
- The implementation can drift into a broad tutorial rewrite or new sample family unless it stays anchored to the existing quickstart pair and shared flow.
- Diagnostics or observability snippets can accidentally over-promise raw SQL visibility, hosted tooling, or automatic maintenance behavior that the current contracts explicitly exclude.
- Requiring live PostgreSQL validation for the default success path would break the repository's SQLite-first no-infrastructure example posture.
- Split recommendation: No immediate split is recommended; the repository already has a bounded shared quickstart surface and the code-plus-README work fits one task.
- Split recommendation: If scope expands to separate provider-specific scenarios or dedicated observability walkthroughs, create follow-up tickets rather than enlarging this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9213`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6222256e8bc345b6970f110230e07dcf`
- completed-at-utc: `<redacted>-06T09:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZSNDXXEEHF53HN14QFK14/runs/20260606T093453718Z-6222256e8bc345b6970f110230e07dcf.json`