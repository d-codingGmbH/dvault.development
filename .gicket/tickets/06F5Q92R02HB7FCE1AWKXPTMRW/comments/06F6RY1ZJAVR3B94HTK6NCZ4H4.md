[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q92R02HB7FCE1AWKXPTMRW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Optimistic claim succeeded (`expectedRevision=06F5Q99D62YS5A8N14W6YD4FGC`, `currentRevision=06F6RVPF7YJTR9KAZRAQZJ3P90`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q92R02HB7FCE1AWKXPTMRW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q92R02HB7FCE1AWKXPTMRW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q92R02HB7FCE1AWKXPTMRW-story-generate-typed-pit-and-bridge-read-project' from source '091e8a2c9e8583718d6f9477a86b87c11d70efde'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q92R02HB7FCE1AWKXPTMRW-story-generate-typed-pit-and-bridge-read-project` as `95e37751e099`.

Open questions / Risiken
- PIT scope can drift past the repo-proven baseline if implementation widens link-parent, multi-active, or registry-backed behavior beyond the landed contract.
- Bridge helpers will produce unstable public API if exact produced endpoint names or hierarchy-depth semantics are normalized incorrectly or if unbounded traversal slips into the generated surface.
- Because PIT/bridge diagnostics share the reserved `DMV196x` range with sibling generator work, internal diagnostic catalogs and messages need coordination to avoid fragmented behavior across the typed-read generator stories.
- Split recommendation: No additional split is recommended now. The repository already treats this as the bounded PIT/bridge child of the typed read-model generator contract, separate from satellite generation in `06F5Q92AHG0ZCTVQGC6NAYVP9C` and broader analyzer/code-fix follow-...
- Split recommendation: If implementation later proves shared generator plumbing or cross-shape test vectors too large, create additive cleanup or test-vector follow-ups after this bounded PIT/bridge slice rather than reopening the current ticket scope.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9329`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `cf992c9603804818ba10109de6b39bd6`
- completed-at-utc: `<redacted>-28T02:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92R02HB7FCE1AWKXPTMRW/runs/20260528T024440463Z-cf992c9603804818ba10109de6b39bd6.json`