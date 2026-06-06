[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZSYCVZ21MS983501BZG18'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZSYCVZ21MS983501BZG18`.
- Optimistic claim succeeded (`expectedRevision=06F9S5RFCAF94649WY8ZCK9134`, `currentRevision=06F9S5ZM2Z1B9V3CGRKM9QAZGW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release' from source 'c89dc8f2dd4059d1cc10d5abce38f7dbbfefcbe7'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release` as `ee4488dbe39b`.

Open questions / Risiken
- Risky assumption: The contract assumes only `README.md`, `docs/production-adoption-checklist.md`, and any intentionally touched example version text need current-baseline alignment; developers should not widen the ticket into a repo-wide version sweep.
- Risky assumption: The live relation to `06F8KZTNG44XDPMVTVCV4WJSHG` must stay a forward-boundary mention only; the ticket should not assume the v0.32 provider-specific SQL artifact contract is available for specification in v0.31.
- Split recommendation: No split is needed; the remaining work is still one coordinated v0.31.0 release note plus small baseline-link adjustments.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8838`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f428cd30846043b2acf58087b7ca527f`
- completed-at-utc: `<redacted>-06T11:07:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZSYCVZ21MS983501BZG18/runs/20260606T110726977Z-f428cd30846043b2acf58087b7ca527f.json`