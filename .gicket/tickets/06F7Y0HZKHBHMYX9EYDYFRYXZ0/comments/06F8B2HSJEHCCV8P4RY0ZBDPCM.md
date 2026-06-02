[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0HZKHBHMYX9EYDYFRYXZ0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0HZKHBHMYX9EYDYFRYXZ0`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0Z0YD8896JV3F671CMERR`, `currentRevision=06F8AZ26JBTYTFM8S90P8G8HXW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0HZKHBHMYX9EYDYFRYXZ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0HZKHBHMYX9EYDYFRYXZ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0HZKHBHMYX9EYDYFRYXZ0-task-update-v0-25-0-read-plan-and-typed-helper-d' from source '1a0cef747b495f8a768505498ecd76058b3045cf'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Multiple current docs still repeat the old satellite-only story; partial edits will leave contradictory guidance across README, analyzer README, checklist, and architecture pages.
- Read-plan examples can accidentally violate the repository's redaction boundary or imply SQL/plan inspection if they include raw request values or overly concrete provider behavior.
- Bridge helper docs can overstate support unless they keep the bounded endpoint vocabulary and required maximumDepth rule explicit.
- Split recommendation: Keep this ticket doc-only. If the team wants runnable consumer samples, benchmark refreshes, or new generator/runtime behavior, handle those as separate follow-up tickets rather than expanding this documentation refinement scope.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8387`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `add18592e0654cbd9d714f5f6191c01f`
- completed-at-utc: `<redacted>-01T23:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0HZKHBHMYX9EYDYFRYXZ0/runs/20260601T233448965Z-add18592e0654cbd9d714f5f6191c01f.json`