[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZSYCVZ21MS983501BZG18'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZSYCVZ21MS983501BZG18`.
- Optimistic claim succeeded (`expectedRevision=06F9JF7Y7ZJFSXXJBEENNXVR4C`, `currentRevision=06F9S3Q9ZA5AY2JPYJG2J8VT3R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZSYCVZ21MS983501BZG18': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZSYCVZ21MS983501BZG18': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release' from source '11dde99f99424d213efacf92c39db202b04cd3c7'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release` as `35e9067845c9`.

Open questions / Risiken
- The release note can over-specify future provider-specific SQL artifact workflow unless it keeps v0.32 as a short non-goal boundary only.
- Partial navigation updates can leave conflicting current-baseline claims if README.md, examples/README.md, and docs/production-adoption-checklist.md are not aligned together when touched.
- Copying too much from the example or observability docs can accidentally over-promise raw SQL visibility, hosted observability, automatic maintenance, or runtime routing that the existing contracts explicitly exclude.
- Split recommendation: No immediate split is needed; the remaining work is one release note plus small baseline-link adjustments.
- Split recommendation: If someone wants a repo-wide version-sweep or a dedicated v0.32 artifact-lane explainer, create separate follow-up tickets rather than enlarging this v0.31 release-doc task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8806`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `394cc6a26ece498cbd322d5c56ceb867`
- completed-at-utc: `<redacted>-06T10:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZSYCVZ21MS983501BZG18/runs/20260606T105926830Z-394cc6a26ece498cbd322d5c56ceb867.json`