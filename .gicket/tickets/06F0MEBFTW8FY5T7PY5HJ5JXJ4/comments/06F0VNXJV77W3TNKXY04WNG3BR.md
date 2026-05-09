[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEBFTW8FY5T7PY5HJ5JXJ4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEBFTW8FY5T7PY5HJ5JXJ4`.
- Optimistic claim succeeded (`expectedRevision=06F0QH0VBWSCTSNZR2959EEMTG`, `currentRevision=06F0VM0SJTQEM6H7BZTGJWJXXR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEBFTW8FY5T7PY5HJ5JXJ4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEBFTW8FY5T7PY5HJ5JXJ4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEBFTW8FY5T7PY5HJ5JXJ4-task-refactor-save-and-read-services-to-consume' from source '9bb29cdde7ae136832c69dedf9ff5d6d987b409c'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEBFTW8FY5T7PY5HJ5JXJ4-task-refactor-save-and-read-services-to-consume` as `689b0da5289d`.

Open questions / Risiken
- The live ticket still has incoming blocks relations from 06F0MEAXT99V0P115P0WEJD4P0 and 06F0MEB634X6CTBZ00W108G3FG, so implementation sequencing depends on those upstream tickets or later relation cleanup.
- If registry-backed calls accidentally diverge from the explicit validation path, ordinary and advanced callers could see inconsistent diagnostics or write ordering; regression tests need to pin this down.
- Split recommendation: No additional split is recommended now; the current ticket is already bounded to registry-backed metadata consumption, while typed save-helper and typed read-projection work is already separated into neighboring tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `28088`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0866`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1d21384cf2c545c8a7471ec3d70adea3`
- completed-at-utc: `<redacted>-09T17:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEBFTW8FY5T7PY5HJ5JXJ4/runs/20260509T174459658Z-1d21384cf2c545c8a7471ec3d70adea3.json`