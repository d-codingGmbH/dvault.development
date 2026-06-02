[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0KGY29HHGZWHC470KVJBG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0KGY29HHGZWHC470KVJBG`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0ZENQVA3AA3DJS5F91CC0`, `currentRevision=06F8F5CKQ3R3AA2K014EG7SPJC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0KGY29HHGZWHC470KVJBG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0KGY29HHGZWHC470KVJBG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru' from source '9793831010f78474729df3f7259ea4386ea15520'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru` as `6b9cee0c456a`.

Open questions / Risiken
- Complex provider-specific scaffolding can decompose one logical rename into multiple migration operations, so some legitimate changes may still be classified as suspicious unless the migration preserves enough continuity evidence.
- Broader structure coverage across tables, columns, indexes, and named constraints increases the test matrix needed to prove provider-neutral behavior.
- The live dependency chain around the current `blocks` relations remains a delivery-sequencing risk even though refinement itself is ready.
- Split recommendation: No split recommended: strengthening destructive-change classification, diagnostics, and tests on the existing `guardrail --migration` surface is one cohesive story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `27547`
- cached-tokens: `7552`
- effective-cache-ratio: `0.2741`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `258b002d90da430585ab1f3d1999acc2`
- completed-at-utc: `<redacted>-02T09:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0KGY29HHGZWHC470KVJBG/runs/20260602T091537078Z-258b002d90da430585ab1f3d1999acc2.json`