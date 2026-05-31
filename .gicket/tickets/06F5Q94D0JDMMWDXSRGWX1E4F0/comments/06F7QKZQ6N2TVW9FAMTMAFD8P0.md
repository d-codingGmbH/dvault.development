[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q94D0JDMMWDXSRGWX1E4F0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q94D0JDMMWDXSRGWX1E4F0`.
- Optimistic claim succeeded (`expectedRevision=06F72ZSHJB0F7CAG6Y4QZ100GG`, `currentRevision=06F7QJJ693R2AP0R167Y0JW75G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q94D0JDMMWDXSRGWX1E4F0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q94D0JDMMWDXSRGWX1E4F0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q94D0JDMMWDXSRGWX1E4F0-story-add-activity-tracing-for-pit-and-bridge-ma' from source '727c4f2fbaa715301c5dbf20982d428b3e3a3bb0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q94D0JDMMWDXSRGWX1E4F0-story-add-activity-tracing-for-pit-and-bridge-ma` as `311755fe36b8`.

Open questions / Risiken
- This story remains dependency-bound by the live incoming `blocks` relation from `06F5Q93YXHSKABD2SABWY85S78`; if the shared tracing contract implementation changes tag/event helper mechanics late, maintenance instrumentation may need a small alignment pass.
- No-op coverage must stay anchored to explicit existing request/result evidence; over-eager emission on rebuild paths would violate the redaction and bounded-semantics contract.
- Split recommendation: No split recommended; the story is already bounded to four explicit maintenance entry points, fixed contract vocabularies, and existing PIT/bridge maintenance test lanes.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `35740`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0680`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `236a5228693a4cb4965dfe0e2e104dcd`
- completed-at-utc: `<redacted>-31T02:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q94D0JDMMWDXSRGWX1E4F0/runs/20260531T021447346Z-236a5228693a4cb4965dfe0e2e104dcd.json`