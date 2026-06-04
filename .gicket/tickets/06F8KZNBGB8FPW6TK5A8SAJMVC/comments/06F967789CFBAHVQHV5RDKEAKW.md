[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZNBGB8FPW6TK5A8SAJMVC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZNBGB8FPW6TK5A8SAJMVC`.
- Optimistic claim succeeded (`expectedRevision=06F964Y5HCV702GATDJASANHWR`, `currentRevision=06F9653HQH54NAHMDV1C8SEK58`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua' from source '529c5a744b71d95460555c3fb6a22950cbf613c0'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua` as `234002eb0f14`.

Open questions / Risiken
- Blocking finding: The contract makes unique-constraint compatibility part of the story, but local source evidence does not identify a concrete DVault constraint surface or EF migration operation to target. Today the repo exposes only primary-key/foreign-key constraint kinds, d...
- Required PO action: Either narrow the ticket from `unique-constraint surfaces` to the generated index/primary-key surfaces already evidenced in the repo, or add direct source-backed evidence naming the exact in-scope EF/Core operation types and baseline objects for unique-cons...
- Risky assumption: The contract assumes provider-specific uniqueness behavior can be implemented against an existing public/source surface, but the current repo does not show a unique-constraint API or migration-operation path.
- Risky assumption: The contract assumes the separate documentation task is effectively blocked even though its persisted ticket state is not blocked yet.
- Risky assumption: The contract assumes provider packages will surface the in-scope uniqueness behavior through a bounded, known set of operation shapes without citing the exact shapes.
- Split recommendation: Keep provider-specific index/timestamp guardrails in one implementation story.
- Split recommendation: If PO wants to preserve unique-constraint behavior but it requires new explicit constraint surfaces or separate provider research, split that uniqueness-surface lane into a follow-up instead of leaving it underspecified in this story.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9485`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f4c269f786274dba9c8e08dd77e8ac7f`
- completed-at-utc: `<redacted>-04T14:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/runs/20260604T145005763Z-f4c269f786274dba9c8e08dd77e8ac7f.json`