[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0DZ3AJSG99YN00CAVX3JR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0DZ3AJSG99YN00CAVX3JR`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0X7BZ1GEWJGFKS2WFMJ4C`, `currentRevision=06F7ZJNT2GF5BG09E7KVT8S6KW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0DZ3AJSG99YN00CAVX3JR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0DZ3AJSG99YN00CAVX3JR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0DZ3AJSG99YN00CAVX3JR-story-add-typed-async-chunk-mapper-helpers-for-e' from source '6f5827073cb9c697da99379b7853825df8c626aa'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The public API can become noisy if both adapter-style and direct save-style helpers are added without a tight naming and documentation story.
- Typed async helper expectations may be misread as broader than the current typed helper contract, especially for generated satellite mappers that target unsupported convenience shapes.
- A careless implementation could accidentally pre-buffer the full async source or hide chunk defaults, which would violate the landed async chunked save contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9038`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7f5b91384b924b928f0684b5330bc588`
- completed-at-utc: `<redacted>-31T21:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0DZ3AJSG99YN00CAVX3JR/runs/20260531T210200234Z-7f5b91384b924b928f0684b5330bc588.json`