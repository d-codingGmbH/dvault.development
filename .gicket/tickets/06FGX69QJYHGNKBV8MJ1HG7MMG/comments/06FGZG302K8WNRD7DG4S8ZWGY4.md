[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX69QJYHGNKBV8MJ1HG7MMG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX69QJYHGNKBV8MJ1HG7MMG`.
- Optimistic claim succeeded (`expectedRevision=06FGX6RSWR0CRQS7814JTT4PHW`, `currentRevision=06FGZEGC2EQGXP71A19QBA2S8C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX69QJYHGNKBV8MJ1HG7MMG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX69QJYHGNKBV8MJ1HG7MMG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife' from source '9ba47a5dc3b4ffb152dd99098bde06c0512b482a'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife` as `3f1b9b7d922b`.

Open questions / Risiken
- This ticket is already in a dependency chain: it is blocked by 06FGX67TZV1F6S949F96ZE201W and currently blocks 06FGX6B9KQME0NJ8B810239DG0, so contract drift in the finding shape or coverage rules can ripple into adjacent work.
- Fail-closed complete-boundary coverage will surface producer or baseline gaps immediately; any upstream manifest producer that omits PIT or bridge references will prevent successful validation until the evidence source is corrected.
- Provider-specific normalization or storage-fact assumptions must stay within the finite built-in profile baseline; expanding beyond that set inside this ticket would increase scope and validation ambiguity.
- Split recommendation: No split recommended for this ticket as refined; keep provider-specific live-schema enhancements, manifest generation, or broader migration orchestration in separate follow-up tickets if they arise.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `37270`
- cached-tokens: `8064`
- effective-cache-ratio: `0.2164`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8586e7e8b86446e5af44cee478bf2477`
- completed-at-utc: `<redacted>-28T19:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX69QJYHGNKBV8MJ1HG7MMG/runs/20260628T194133585Z-8586e7e8b86446e5af44cee478bf2477.json`