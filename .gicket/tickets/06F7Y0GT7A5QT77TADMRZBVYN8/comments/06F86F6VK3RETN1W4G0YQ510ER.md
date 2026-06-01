[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0GT7A5QT77TADMRZBVYN8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0GT7A5QT77TADMRZBVYN8`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0Y988EJZTG2DP5J50T2T8`, `currentRevision=06F86CZNV66TH68RFGKK9FS4WG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0GT7A5QT77TADMRZBVYN8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0GT7A5QT77TADMRZBVYN8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0GT7A5QT77TADMRZBVYN8-story-define-support-bundle-driven-typed-pit-and' from source 'b2348c94f0b385f26bc08fa83068eed9482384cb'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0GT7A5QT77TADMRZBVYN8-story-define-support-bundle-driven-typed-pit-and` as `8684fd5b680d`.

Open questions / Risiken
- Live relation state still shows incoming blocker tickets `06F7Y0F650KM61BQXMEQPZ86DR` and `06F7Y0FZXX5J0G7G15681HVEBR`; implementation should verify whether those blockers remain semantically active or need relation cleanup before development starts.
- PIT helper support is only safe when the support-bundle export path can carry the bounded PIT read-shape facts needed for parent identity, canonical driving-key families, and segment snapshot references without reintroducing raw-model parsing.
- Bridge helper ergonomics must not drift into broader graph traversal or provider-specific behavior; the contract needs to stay constrained to the existing many-to-many and hierarchy read-service semantics.
- Split recommendation: No bounded child-ticket split was materialized in this pass; keep the contract-definition story unified and split later engineering delivery into PIT and bridge implementation tickets only if development capacity or test volume warrants it.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `78809`
- cached-tokens: `41216`
- effective-cache-ratio: `0.5230`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a1b6763b0d4f41059f5f57018da3b963`
- completed-at-utc: `<redacted>-01T12:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0GT7A5QT77TADMRZBVYN8/runs/20260601T125104212Z-a1b6763b0d4f41059f5f57018da3b963.json`