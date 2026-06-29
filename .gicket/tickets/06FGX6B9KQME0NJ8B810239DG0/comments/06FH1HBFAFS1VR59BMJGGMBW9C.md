[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX6B9KQME0NJ8B810239DG0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX6B9KQME0NJ8B810239DG0`.
- Optimistic claim succeeded (`expectedRevision=06FGX6SAHWC9FM9CGK421E6QVC`, `currentRevision=06FH1FBMNQDAAPM7MSSCQBZJGC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX6B9KQME0NJ8B810239DG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX6B9KQME0NJ8B810239DG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre' from source '2f8029987e58e77cbe2380c19ef8767d9deabfbf'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Adding a new public preflight/diagnostics lane changes structured output and display text; omitted-input and serialization compatibility need explicit regression tests.
- If developers fold manifest findings into migration-guardrail issues, the implementation will violate the ticket's required separation between manifest validation and EF migration guardrails.
- If diagnostics/support-bundle projection reuses raw manifest text instead of the structured validation result, it risks breaching the redaction boundary documented for hash-key migration planning.
- Split recommendation: No split recommended: the repository already contains the validator, manifest command, and aggregate preflight scaffolding, so the remaining work is a bounded integration and test task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8148`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d195daf7a60449f4abc39c8a15b2a3ce`
- completed-at-utc: `<redacted>-29T00:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX6B9KQME0NJ8B810239DG0/runs/20260629T002642381Z-d195daf7a60449f4abc39c8a15b2a3ce.json`