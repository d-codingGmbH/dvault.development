[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZTNG44XDPMVTVCV4WJSHG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZTNG44XDPMVTVCV4WJSHG`.
- Optimistic claim succeeded (`expectedRevision=06F9XHMZ6MWEV6WVQ7T67XN660`, `currentRevision=06F9XHW4KYEZRG9TSY4PWFB27G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a' from source 'a242ac0a133a10697759d972a334330a46f74dd1'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a` as `74e36b078f14`.

Open questions / Risiken
- Blocking finding: The README index entry required by the Acceptance Criteria and Definition of Done is not yet durable in branch history. It exists only as an unstaged scratch-worktree diff, so a fresh checkout of HEAD still misses the docs/plans/README.md entry for provider-s...
- Required PO action: Persist the docs/plans/README.md index addition into the owner-branch committed state, or remove the claim that the contract is already indexed there.
- Required PO action: Reconcile the Delivery Contract and Implementation Notes with the actual write history so the ticket accurately states whether the description and planning index were updated.
- Required PO action: Re-run PO handoff after the repository state and the persisted ticket narrative agree.
- Risky assumption: Assuming a scratch-worktree-only README edit is enough to satisfy the ticket's durable planning-surface claim.
- Risky assumption: Assuming reviewers will ignore the contradictory note that no ticket description updates were materialized.
- Split recommendation: No new split is needed; the current parent and child tickets still separate architecture contract, evidence rules, prototype, and rollout documentation.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8884`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `45adae3bfd764eaab39d71eed4a5c934`
- completed-at-utc: `<redacted>-06T21:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/runs/20260606T212100798Z-45adae3bfd764eaab39d71eed4a5c934.json`