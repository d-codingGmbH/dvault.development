[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEGPPETJD4ZDEN5ESGR7JW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEGPPETJD4ZDEN5ESGR7JW`.
- Optimistic claim succeeded (`expectedRevision=06F1H4VWY4QTZA0T792GT0JF9W`, `currentRevision=06F1H53B615NP7STYWGB7K3F4C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEGPPETJD4ZDEN5ESGR7JW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEGPPETJD4ZDEN5ESGR7JW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers' from source '4338cbbe21387b187adada102418f5039d09c72d'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers` as `970b05de6c42`.

Open questions / Risiken
- Consumers may still read docs/releases/v0.6.0.md and conclude PIT-backed reads and bridge helpers are absent until downstream docs/release work updates the consumer-facing notes.
- Hierarchy bridge reads depend on precomputed rows and a required bounded maximumDepth; they do not imply arbitrary recursive traversal or automatic closure maintenance.
- Consumers may expect PIT or bridge helpers to populate PIT/bridge maintenance tables; the read-only boundary must stay explicit in diagnostics and follow-up documentation.
- Split recommendation: No further split is recommended. The parent story is already decomposed into four done child tickets, and the remaining docs/release and benchmark work already exists as downstream tickets rather than missing child scope.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9209`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1152dd6a0276454e9c0b78d175bccea8`
- completed-at-utc: `<redacted>-11T19:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/runs/20260511T195534046Z-1152dd6a0276454e9c0b78d175bccea8.json`