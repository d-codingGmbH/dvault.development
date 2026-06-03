[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZJAKN7Q2QXXP9PRK2V94G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZJAKN7Q2QXXP9PRK2V94G`.
- Optimistic claim succeeded (`expectedRevision=06F8M00YEH5NF7B2NDH1EZF7AR`, `currentRevision=06F8WQ6FQNWCE06HH96GW9W5Z4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZJAKN7Q2QXXP9PRK2V94G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZJAKN7Q2QXXP9PRK2V94G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r' from source '71779ff5b2907077b5af3a1dc55989f9088f1fdf'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r` as `70f0e9279c17`.

Open questions / Risiken
- The ticket still has a live incoming blocks relation from 06F8KZHZ27SDTNCFNMFDQRVCKM, so delivery sequencing may still depend on upstream work even though PO refinement is complete.
- Provider-specific SQL paths can drift from provider-neutral semantics unless parity tests cover bounded multi-active PIT behavior and hierarchy bridge traversal depth rules.
- If stale-maintenance or read-shape evidence checks fail open instead of failing closed to fallback, optimized reads could return incorrect PIT or bridge results.
- Split recommendation: No split was materialized; keep the story whole if implementation stays limited to existing PIT and bridge shapes and fallback safety rules.
- Split recommendation: If provider-specific SQL, tests, and benchmark evidence expand beyond that boundary, split next by provider rather than by public API surface.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `27177`
- cached-tokens: `7552`
- effective-cache-ratio: `0.2779`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `693e2fd562c7405a91feb1aec12cd650`
- completed-at-utc: `<redacted>-03T17:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZJAKN7Q2QXXP9PRK2V94G/runs/20260603T170054395Z-693e2fd562c7405a91feb1aec12cd650.json`