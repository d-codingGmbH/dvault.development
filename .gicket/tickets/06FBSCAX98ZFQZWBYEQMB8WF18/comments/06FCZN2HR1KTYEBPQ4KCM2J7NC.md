[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCAX98ZFQZWBYEQMB8WF18'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCAX98ZFQZWBYEQMB8WF18`.
- Optimistic claim succeeded (`expectedRevision=06FBSCZCKCQ9T05S4MV768HADM`, `currentRevision=06FCZK5HEN236B5NSGY35RVWSW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCAX98ZFQZWBYEQMB8WF18': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCAX98ZFQZWBYEQMB8WF18': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCAX98ZFQZWBYEQMB8WF18-task-document-provider-bulk-outcomes-and-benchma' from source 'fd1689f01531da1e6f054b663b1cec05fbc27ceb'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCAX98ZFQZWBYEQMB8WF18-task-document-provider-bulk-outcomes-and-benchma` as `5cffa574991c`.

Open questions / Risiken
- Future doc edits can accidentally overclaim external-provider performance if they cite skipped root rows without the evidence-matrix posture or the linked provider-specific artifact bundle.
- Provider thresholds in the v0.32 evidence bundles are run-context-bound; copying their numbers without preserving hardware, runtime, iteration, warmup, and provider-configuration context would create misleading guidance.
- Split recommendation: No split recommended; current repository evidence already bounds this work to documentation alignment and claim hygiene across existing bulk-save evidence surfaces.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7148`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f15cfb8f8a4e434c975505486e9b6d27`
- completed-at-utc: `<redacted>-16T09:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCAX98ZFQZWBYEQMB8WF18/runs/20260616T094738589Z-f15cfb8f8a4e434c975505486e9b6d27.json`