[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEA1FF743S14XQW02H4A3W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEA1FF743S14XQW02H4A3W`.
- Optimistic claim succeeded (`expectedRevision=06F0QZ4EK1Z8SW68JK7F8YGKT0`, `currentRevision=06F0S91APN1BN9HSWMXQ21E3M4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEA1FF743S14XQW02H4A3W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEA1FF743S14XQW02H4A3W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEA1FF743S14XQW02H4A3W-task-implement-fluent-link-and-relationship-proj' from source 'd152241ae75bae8601c0c8e6f628384c59ce13d0'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- This child and the hub or satellite sibling both touch the shared code-first entry surface, so parallel delivery can create API or merge drift unless shared scaffolding stays minimal.
- Any loss of participant declaration order or drift from current naming normalization will change produced link table, key, and index names and break metadata-first equivalence.
- Repeated same-hub participants can produce duplicate participant hash-key names under the current link naming path if the code-first layer does not reject unsupported shapes early.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9531`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `dbc8ee9bd3d34dc8ac4782d2e321294f`
- completed-at-utc: `<redacted>-09T12:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEA1FF743S14XQW02H4A3W/runs/20260509T121632184Z-dbc8ee9bd3d34dc8ac4782d2e321294f.json`