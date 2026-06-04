[gicket-bot] Run report (outcome: po-refinement-clarification)

Summary
- PO refinement processed ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'. Ticket requires clarification handoff to role 'po' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F98FC7YX2725089CCJWA47TR`, `currentRevision=06F98FHFVM7R04SNEEHCGQ0EZ4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source '26c85fdc91a9d3c0f9dd0afad0a7785a9837a2e2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP7` on branch `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` as `6d89148b04c2`.

Open questions / Risiken
- If runtime workflow cleanup does not move this ticket onto a closure/completion path, automation or humans could still misroute it toward dev despite no remaining implementation scope.
- If future provider-expansion or physical-naming-override requests are attached to this parent epic, the repository could reopen already-completed scope and blur release traceability.
- Open question: Which runtime closure/completion path should this closure-only epic use so it can be finished without re-entering the normal po-critic -> dev route?
- Split recommendation: No new split is recommended; the existing four child tickets already cover and complete the epic scope on develop.
- Split recommendation: Any future provider-expansion or physical-naming-override work should be created as new follow-up tickets or epics rather than as children under this closure-only roll-up.

Next steps
- Collect missing answers and hand off to role 'po' after clarification.
- Re-run PO refinement after open questions are resolved.

Prompt cache usage
- prompt-tokens: `57263`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0425`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ab8dafa6359f49989407fe4a5a90cdf8`
- completed-at-utc: `<redacted>-04T20:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T201323937Z-ab8dafa6359f49989407fe4a5a90cdf8.json`