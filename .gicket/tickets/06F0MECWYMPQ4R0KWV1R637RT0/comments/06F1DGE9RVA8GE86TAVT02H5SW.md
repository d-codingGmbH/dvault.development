[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MECWYMPQ4R0KWV1R637RT0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MECWYMPQ4R0KWV1R637RT0`.
- Optimistic claim succeeded (`expectedRevision=06F0QH1BBYH8EMF23Z1N6QMEH4`, `currentRevision=06F1DF5YXZ8QN7EG87NC3NWY8G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MECWYMPQ4R0KWV1R637RT0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MECWYMPQ4R0KWV1R637RT0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MECWYMPQ4R0KWV1R637RT0-story-add-developer-diagnostics-and-starter-exam' from source '8f218e18973518a2c006ab9e4536b2bd18bac3f6'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Final package publication remains outside this story and still depends on release-operator validation and approval.
- Diagnostics fallback-cause reporting can drift from runtime dispatch behavior if provider strategy gates change without updating shared tests and docs.
- Reviewers may confuse README v0.6.0 install guidance with pre-tag MinVer artifact evidence; the completed docs child separates those concerns, but the distinction should remain visible through release validation.
- Provider capability auto-registration and provider-specific save-strategy registration are separate surfaces; future provider docs should keep that distinction explicit.
- Split recommendation: No new split is recommended. The story already has the needed persisted child split and all three children are done: diagnostics 06F0MED4P7HMBDZVMPWQZ5A7PC, quickstart examples 06F0MEDBFZ25YA1M7RJ71Z7ZCM, and README/release docs 06F0MEDJC732GDD77H60R259P0.
- Split recommendation: Future work should be split only for concrete new scope such as CLI diagnostics, Code-First-to-registry bridging, additional provider quickstarts, or post-tag package publication defects; none blocks this PO-critic handoff.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9078`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `fb9af4e2d023499ebf2aaaa11113dade`
- completed-at-utc: `<redacted>-11T11:17:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MECWYMPQ4R0KWV1R637RT0/runs/20260511T111738170Z-fb9af4e2d023499ebf2aaaa11113dade.json`