[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZSNDXXEEHF53HN14QFK14'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZSNDXXEEHF53HN14QFK14`.
- Optimistic claim succeeded (`expectedRevision=06F9RJD56JWMSRS9RJZF136JJ8`, `currentRevision=06F9RJKWF0NKJAGP16GX969PYG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with' from source '4a7463f32bdd281810e90fb9c9503e297fe7fce8'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with` as `c196a0f90a10`.

Open questions / Risiken
- Risky assumption: Implementation will keep any diagnostics snippet limited to redacted facts such as strategy status, selected strategy name, fallback presence, or read-shape category, without printing raw SQL or provider messages.
- Risky assumption: v0.31 guardrail wording can be added in examples/README.md without this ticket also owning the separate release-note file docs/releases/v0.31.0.md.
- Risky assumption: SQLite remains the default proof path and PostgreSQL remains optional; developers should not reinterpret the contract as requiring live PostgreSQL validation for normal success.
- Split recommendation: No split recommended; the example plus README work remains bounded to one shared quickstart surface.
- Split recommendation: If a later request needs provider-specific or observability-heavy samples, create a follow-up ticket instead of broadening 06F8KZSNDXXEEHF53HN14QFK14.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9396`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e7a791e3c3bc473682d0a875daabe571`
- completed-at-utc: `<redacted>-06T09:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZSNDXXEEHF53HN14QFK14/runs/20260606T094439883Z-e7a791e3c3bc473682d0a875daabe571.json`