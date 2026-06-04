[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZNBGB8FPW6TK5A8SAJMVC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZNBGB8FPW6TK5A8SAJMVC`.
- Optimistic claim succeeded (`expectedRevision=06F967C5VKXBG49781Z9PC7TF4`, `currentRevision=06F967H4X5C7KM1ECC297D2XQ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZNBGB8FPW6TK5A8SAJMVC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZNBGB8FPW6TK5A8SAJMVC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua' from source '1de44a520119c170a866a38a79162bb77e2d39c9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua` as `198381aa8207`.

Open questions / Risiken
- Provider package upgrades can change emitted EF migration operation shapes, so tests must lock the bounded supported operation set or provider-specific drift may evade guardrails.
- Tightening timestamp nullability and store-type checks can surface previously tolerated migrations as blocking findings, which is correct but may require clearer upgrade guidance for consumers.
- The broader provider identifier contract mentions future constraint classes, so developers may try to widen this implementation beyond the current primary-key and secondary-index baseline unless the narrowed story boundary stays explicit.
- Split recommendation: No PO split is required after narrowing scope; the visible repository baseline keeps provider-specific migration guardrails bounded to one implementation story plus the already-separate documentation task.
- Split recommendation: If implementation later needs DVault-owned unique-constraint modeling, diagnostics, or migration-operation support, queue that as a separate follow-up ticket instead of widening this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8640`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b12746755aac418a875d47e79a4cb786`
- completed-at-utc: `<redacted>-04T15:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/runs/20260604T150017561Z-b12746755aac418a875d47e79a4cb786.json`