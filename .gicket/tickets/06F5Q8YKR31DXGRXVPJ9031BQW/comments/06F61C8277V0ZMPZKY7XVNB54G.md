[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q8YKR31DXGRXVPJ9031BQW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8YKR31DXGRXVPJ9031BQW`.
- Optimistic claim succeeded (`expectedRevision=06F5Q98AAZJC8AG15GF3E5SBA4`, `currentRevision=06F61AE22BZKTX0MDZZ3P35EP8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q8YKR31DXGRXVPJ9031BQW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q8YKR31DXGRXVPJ9031BQW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q8YKR31DXGRXVPJ9031BQW-story-define-provider-staging-spi-and-transactio' from source '1aa6b9b82fe8b839f0585905453a3da129d69969'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q8YKR31DXGRXVPJ9031BQW-story-define-provider-staging-spi-and-transactio` as `bf698baeebff`.

Open questions / Risiken
- Late churn in lifecycle or transaction rules will cascade into the five downstream tickets already blocked by this story.
- If oversized-batch and schema-limitation gates are under-specified, provider packages may diverge in when they reject equivalent request shapes.
- If the implementation leaks provider-specific staging abstractions into public namespaces, the project could accidentally take on a long-term public API support burden.
- Split recommendation: No additional split is recommended in this refinement pass; the live relation graph already shows this story serving as the contract/architecture blocker for five downstream tickets.
- Split recommendation: If later evidence shows one provider needs materially different staging cleanup or transaction semantics, create a provider-specific follow-up ticket instead of widening this shared contract story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `24031`
- cached-tokens: `2432`
- effective-cache-ratio: `0.1012`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2efd43e8447b4220ac4b433b8f5aab80`
- completed-at-utc: `<redacted>-25T19:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8YKR31DXGRXVPJ9031BQW/runs/20260525T195113708Z-2efd43e8447b4220ac4b433b8f5aab80.json`