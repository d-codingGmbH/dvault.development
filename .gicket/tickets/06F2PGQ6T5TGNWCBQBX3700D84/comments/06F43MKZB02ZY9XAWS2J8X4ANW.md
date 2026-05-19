[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGQ6T5TGNWCBQBX3700D84'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGQ6T5TGNWCBQBX3700D84`.
- Optimistic claim succeeded (`expectedRevision=06F2PNN2BWVZWNQ47JYEDGSN5C`, `currentRevision=06F43JV7G69GPFZH8FM5BT81K8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGQ6T5TGNWCBQBX3700D84': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGQ6T5TGNWCBQBX3700D84': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGQ6T5TGNWCBQBX3700D84-story-explain-save-and-read-strategy-decisions' from source 'f4b5b7fa2693fdb65090e73f433bda9e134930a1'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Explanation logic can drift from real runtime dispatch if it duplicates strategy-selection rules instead of reusing the same gate evaluation behavior.
- Documentation drift is likely unless README and release-note updates explicitly catch up with the existing read-diagnostics surface as well as the save-diagnostics surface.
- Scope can sprawl into telemetry, support-bundle, and release-wrap work unless the existing downstream blocks relations continue to own those deliverables.
- Split recommendation: No additional split is recommended. The live relation set already separates downstream telemetry, support-bundle, and v0.16 documentation work into tickets 06F2PGQBGNZPEEJE4KBET4JG24, 06F2PGQJ7THHNSYYBFFPBG4174, and 06F2PGQQJB5FJGDB16M2G7CPCM.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9354`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `393a7acde508448fa58e1c48972b6dc9`
- completed-at-utc: `<redacted>-19T19:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGQ6T5TGNWCBQBX3700D84/runs/20260519T195934598Z-393a7acde508448fa58e1c48972b6dc9.json`