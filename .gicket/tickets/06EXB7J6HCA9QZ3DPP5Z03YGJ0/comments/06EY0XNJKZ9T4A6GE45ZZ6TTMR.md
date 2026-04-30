[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7J6HCA9QZ3DPP5Z03YGJ0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.
- Optimistic claim succeeded (`expectedRevision=06EY0RWR6G53XX6FXZT47NSKTR`, `currentRevision=06EY0W545YCV3BY1M3P3QX79N4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7J6HCA9QZ3DPP5Z03YGJ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7J6HCA9QZ3DPP5Z03YGJ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7J6HCA9QZ3DPP5Z03YGJ0-task-define-provider-capability-abstraction' from source '812e87ffbcf54b3fb5ffe9019c204db85aeacfb2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7J6HCA9QZ3DPP5Z03YGJ0-task-define-provider-capability-abstraction` as `7fb6b6621426`.

Open questions / Risiken
- If the implementation expands beyond the explicit no-function/no-concurrency baseline, it can reopen the speculative provider-matrix problem the critic flagged.
- If tests do not exercise `DataVaultEfMetadataTranslator` as the first consumer, the abstraction could still land as dormant infrastructure.
- Future provider tickets that require non-text native mappings may need additive contract growth; that is acceptable only if kept versioned and provider-neutral at the logical boundary.
- Split recommendation: No split recommended: the ticket is now bounded to one concrete consumer path, one Sqlite profile, explicit none/unsupported baselines for speculative categories, and one small test surface.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `77157`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0315`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4ce332d0867045c49a0f73a9c7c366dd`
- completed-at-utc: `<redacted>-30T22:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7J6HCA9QZ3DPP5Z03YGJ0/runs/20260430T221607972Z-4ce332d0867045c49a0f73a9c7c366dd.json`