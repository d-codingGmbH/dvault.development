[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC9WY4T9T6YWDHFCEMZ0VG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC9WY4T9T6YWDHFCEMZ0VG`.
- Optimistic claim succeeded (`expectedRevision=06FBSCZ0K9YQF2Y3H8XPV7B0BM`, `currentRevision=06FCWJP7Z1Q4FRN669X7QDKGNG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC9WY4T9T6YWDHFCEMZ0VG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC9WY4T9T6YWDHFCEMZ0VG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC9WY4T9T6YWDHFCEMZ0VG-task-evaluate-db2-bulk-strategy-gaps' from source '4f1fa767e62d7ad40ad6654225a45639580fbd7a'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC9WY4T9T6YWDHFCEMZ0VG-task-evaluate-db2-bulk-strategy-gaps` as `cd855d2f7f37`.

Open questions / Risiken
- The checked-in root DB2 benchmark lane is skipped, so this ticket can only close with a recommendation based on planned-path, diagnostics, smoke, and code evidence rather than measured DB2 timings.
- Reopening staged bulk or threshold tuning inside this ticket would blur the current DB2 save boundary and risk unsupported release claims.
- Mixing DB2 latest-satellite or PIT/bridge evidence work into this ticket would conflate separate backlog rows that already have independent stop conditions.
- Split recommendation: No split recommended: keep this as a bounded recommendation-only DB2 save-path evaluation ticket.
- Split recommendation: If the recommendation later changes to implementation, create a separate child ticket for the chosen DB2 save-path change rather than combining implementation with this evaluation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7691`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a781d85bbeb74533bf7afd740b2b2d28`
- completed-at-utc: `<redacted>-16T02:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC9WY4T9T6YWDHFCEMZ0VG/runs/20260616T024335517Z-a781d85bbeb74533bf7afd740b2b2d28.json`