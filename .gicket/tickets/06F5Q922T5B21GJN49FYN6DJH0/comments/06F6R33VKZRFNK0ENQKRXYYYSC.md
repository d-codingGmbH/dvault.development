[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q922T5B21GJN49FYN6DJH0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q922T5B21GJN49FYN6DJH0`.
- Optimistic claim succeeded (`expectedRevision=06F5Q9995SVEAC2KXQWD21D6DG`, `currentRevision=06F6R0W6HFXG0DDZ76ZJ336E68`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q922T5B21GJN49FYN6DJH0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q922T5B21GJN49FYN6DJH0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q922T5B21GJN49FYN6DJH0-story-define-typed-read-model-generator-contract' from source 'c0bc8cc2911705cb714151fc656f839accb7ef95'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q922T5B21GJN49FYN6DJH0-story-define-typed-read-model-generator-contract` as `0b30ed2a860a`.

Open questions / Risiken
- If the contract blurs the boundary between generated stable helpers and dynamic IDataVaultReadService requests, downstream stories can drift into unsupported arbitrary query compilation.
- PIT and bridge support is bounded by the repository's existing architecture notes; over-promising link-parent, tuple-filter, provider-specific, or maintenance-coupled behavior would create delivery churn.
- Model-first and compiled-model inputs only stay safe if the contract ties generation to one authoritative metadata source and fingerprint; otherwise stale generated code and mismatched produced names become likely.
- Split recommendation: No additional split is needed now. Keep this story focused on the authoritative v1 contract and let 06F5Q92AHG0ZCTVQGC6NAYVP9C cover latest or as-of satellite projector implementation while 06F5Q92R02HB7FCE1AWKXPTMRW covers PIT or bridge projector impleme...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `23691`
- cached-tokens: `2432`
- effective-cache-ratio: `0.1027`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `20ab9ef36cf74059bf38e5cfe90b0b34`
- completed-at-utc: `<redacted>-28T00:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q922T5B21GJN49FYN6DJH0/runs/20260528T004657938Z-20ab9ef36cf74059bf38e5cfe90b0b34.json`