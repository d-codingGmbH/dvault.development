[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XPRY3ZDB6W1WQ9ABRRJ2V4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`.
- Optimistic claim succeeded (`expectedRevision=06F23TV75J4D9XTMM8HCPX2EBR`, `currentRevision=06F23V6J91J9F36VPCKHTB8TM4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails' from source '75ca7837203a78c5b30b8dab96793a0323dcd32f'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails` as `c6ebf21f58ac`.

Open questions / Risiken
- Blocking finding: The epic contract requires release documentation for the lifecycle-guardrails slice, but the repository still has release notes only through `docs/releases/v0.7.0.md`; there is no observed `v0.8.0` lifecycle-guardrails release note or equivalent repo release-...
- Blocking finding: This closure-only epic branch does not materialize the promised repository documentation work: the branch diff from `develop` contains only `.gicket` ticket updates, so approval would depend on future documentation work rather than current repository evidence.
- Required PO action: Return the ticket to PO refinement and explicitly track the missing release-documentation deliverable before rerouting; the current closure contract is not yet backed by repository evidence.
- Required PO action: Either keep the epic open until a repo release-summary document is present for the lifecycle-guardrails slice, or revise the parent contract and create a docs-only follow-up ticket if release-note work is intentionally being deferred elsewhere.
- Required PO action: Update PO handoff text so it does not present epic closure as ready while the release-documentation acceptance criterion is still unmet.
- Risky assumption: Assuming the existing architecture and governance docs satisfy the separate release-documentation acceptance criterion even though no `v0.8.0` release-summary file exists in `docs/releases`.
- Risky assumption: Assuming the epic can close solely because the four child stories are `done`, despite the parent contract still requiring closure-only documentation evidence in the repository.
- Risky assumption: Assuming release operators will backfill the lifecycle-guardrails summary later without a tracked ticket or an updated parent contract.
- Split recommendation: If Product wants to close the tracking epic without waiting for the release-summary deliverable, split a small docs-only follow-up and revise the parent closure criteria before rerouting.
- Split recommendation: If Product keeps the current closure contract, no additional technical split is needed; the remaining work is documentation evidence aligned to release closure.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9132`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `58d87b5990d44942aa3f23fe9abcdd9c`
- completed-at-utc: `<redacted>-13T15:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/runs/20260513T152903025Z-58d87b5990d44942aa3f23fe9abcdd9c.json`