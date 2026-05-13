[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPV0YJ8Z9HQVT6BYR397Q8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPV0YJ8Z9HQVT6BYR397Q8`.
- Optimistic claim succeeded (`expectedRevision=06F1XTP6CXJ6AK69BGPKFDH7GW`, `currentRevision=06F1YWAG2AR476Y8VYT8WGBTHW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPV0YJ8Z9HQVT6BYR397Q8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPV0YJ8Z9HQVT6BYR397Q8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu' from source '18deacfbc7a91ec3faf6b45d4aa41da51141f3df'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu` as `a098a48de1c6`.

Open questions / Risiken
- Migration-specific diagnostic ids and severities must align with the existing catalog; mismatches can create snapshot or contract churn in downstream diagnostics tests.
- Determinism can regress if finding order or rendered location/remediation text depends on non-stable enumeration or provider details.
- Some operations are context-sensitive rather than categorically safe or unsafe, so overly blunt rules could under-report or over-report invariant risk.
- Implementation sequencing still depends on upstream ticket 06F1XPS7KGKBP5SVMQPJC49J2G because it currently blocks this ticket in live relation state.
- Split recommendation: No split recommended; current scope is already tightly bounded to one provider-neutral validator pass, six operation fixtures, and deterministic diagnostics coverage.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `36227`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0671`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `19a9c941aabd469aa5444af929b2fd5e`
- completed-at-utc: `<redacted>-13T03:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/runs/20260513T035715754Z-19a9c941aabd469aa5444af929b2fd5e.json`