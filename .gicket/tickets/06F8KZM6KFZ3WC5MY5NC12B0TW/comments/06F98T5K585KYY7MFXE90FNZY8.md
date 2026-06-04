[gicket-bot] Run report (outcome: po-refinement-clarification)

Summary
- PO refinement processed ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'. Ticket requires clarification handoff to role 'po' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F98SNJY2XSKBFDPC7XCYVS2R`, `currentRevision=06F98STKNFD118BV7KM212YZ1M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source 'a188c36d7ee4b2307f85592dd9f421cc97dfbeda'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP7` on branch `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` as `8048fc57e55c`.

Open questions / Risiken
- Returning ready_for_po_critic without the required bounded verification step would violate the interactive PO refinement instructions.
- Open question: Can the runtime execute the requested bounded ticket/comment/relation reads so the refinement can be finalized on verified live state?
- Split recommendation: No split recommendation yet; the supplied snapshot already indicates the existing four completed child tickets cover the epic scope.

Next steps
- Collect missing answers and hand off to role 'po' after clarification.
- Re-run PO refinement after open questions are resolved.

Prompt cache usage
- prompt-tokens: `97455`
- cached-tokens: `55552`
- effective-cache-ratio: `0.5700`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9c9a8e241bc74dfe88e54cdadccc308f`
- completed-at-utc: `<redacted>-04T20:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T205229903Z-9c9a8e241bc74dfe88e54cdadccc308f.json`