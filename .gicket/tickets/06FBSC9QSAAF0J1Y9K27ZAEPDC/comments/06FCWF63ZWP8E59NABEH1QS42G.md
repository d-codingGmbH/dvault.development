[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC9QSAAF0J1Y9K27ZAEPDC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC9QSAAF0J1Y9K27ZAEPDC`.
- Optimistic claim succeeded (`expectedRevision=06FBSCYZ083EQ6DS5QQFJA7PSR`, `currentRevision=06FCWD2C2X0TFETRCM37MMCGRC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC9QSAAF0J1Y9K27ZAEPDC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC9QSAAF0J1Y9K27ZAEPDC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC9QSAAF0J1Y9K27ZAEPDC-task-evaluate-oracle-bulk-strategy-gaps' from source '0740ed91cd5abc1ab7068a11383bdccd7dd304a5'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC9QSAAF0J1Y9K27ZAEPDC-task-evaluate-oracle-bulk-strategy-gaps` as `7c29bd76c26a`.

Open questions / Risiken
- The current recommendation is intentionally bounded by existing repository evidence. Without a new provider-configured benchmark rerun, the product should not broaden Oracle timing claims beyond the documented direct-path cases.
- Oracle workloads outside the current gate, especially over 10000 satellite operations or multi-active satellite batches, still rely on fallback behavior; that remains a known capability boundary rather than a resolved optimization.
- The v0.32 completed Oracle evidence still showed conventional EF outperforming the retained direct Oracle path in the recorded large-batch comparisons, so any future product messaging must avoid implying a universal Oracle performance win.
- Split recommendation: No split is needed for this ticket. Keep it as a bounded evaluation that concludes with a keep-as-is recommendation, and open a separate follow-up only if new Oracle workload evidence justifies staged bulk or threshold changes.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7276`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `081d8dd16cf14398ba3252fdc793347f`
- completed-at-utc: `<redacted>-16T02:22:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC9QSAAF0J1Y9K27ZAEPDC/runs/20260616T022229236Z-081d8dd16cf14398ba3252fdc793347f.json`