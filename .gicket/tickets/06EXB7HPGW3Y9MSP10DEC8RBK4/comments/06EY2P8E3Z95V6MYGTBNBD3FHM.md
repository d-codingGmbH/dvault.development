[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7HPGW3Y9MSP10DEC8RBK4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7HPGW3Y9MSP10DEC8RBK4`.
- Optimistic claim succeeded (`expectedRevision=06EY2MNBK2JSDVHXBD3B81DXF8`, `currentRevision=06EY2MQRGSV6GXNMXT4VVVX298`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7HPGW3Y9MSP10DEC8RBK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7HPGW3Y9MSP10DEC8RBK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7HPGW3Y9MSP10DEC8RBK4-task-persist-satellite-rows-only-when-hash-diff' from source '73ef76ec775ce1de7dfb24e3f8a553a059332c17'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7HPGW3Y9MSP10DEC8RBK4-task-persist-satellite-rows-only-when-hash-diff` as `0f55018dc7ff`.

Open questions / Risiken
- Because HashDiff is caller-supplied in v1, inconsistent field selection or normalization across callers can cause false changed or unchanged outcomes until a higher-level domain contract is introduced.
- The current provider baseline does not declare multi-writer concurrency support, so unchanged-row suppression and latest-version comparison remain based on deterministic pre-insert lookup rather than provider-neutral conflict handling.
- If downstream callers immediately need single-call parent creation plus satellite historization without precomputed parent hash keys, a follow-up convenience API may be needed even though this ticket intentionally keeps the save-service contract explicit.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `69888`
- effective-cache-ratio: `0.5854`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3c11c3fde7d64ca1bec44466b3e33483`
- completed-at-utc: `<redacted>-01T02:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/runs/20260501T022322479Z-3c11c3fde7d64ca1bec44466b3e33483.json`