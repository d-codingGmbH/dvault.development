[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F9XD26D2MHVAKZ2GCZ67BEFC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9XD26D2MHVAKZ2GCZ67BEFC`.
- Optimistic claim succeeded (`expectedRevision=06FA3B4779CFZM51EGTX97X2NW`, `currentRevision=06FA3BB7VGDW0XFJPV9C8N8MWW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma' from source 'e794e677a6560dbf97a7bc2b2c01532a7bc76414'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma` as `57cdf7fe0a10`.

Open questions / Risiken
- Risky assumption: Assuming the repo-root `benchmark-summary.*` files are the baseline would be wrong; the current root triplet is a lightweight SQLite-plus-skipped-external rollup, not the required all-provider v0.32 evidence set.
- Risky assumption: Assuming `--scale` also covers PIT/bridge verification would be wrong; BenchmarkRunner.cs restricts scale mode to customer-profile scale scenarios, so the read verification must be recorded separately.
- Risky assumption: Assuming a transient provider outage can be silently worked around would be wrong; the contract requires skipped/failed provider rows or equivalent recorded operational evidence instead of disappearing lanes.
- Risky assumption: Assuming this ticket may tune thresholds or provider behavior would be wrong; scope-out is evidence capture only.
- Split recommendation: No split is required now; keep this ticket as the baseline-evidence prerequisite for story `06F9XD1T3TJK7NEBYNVT2JEPZW` and tasks `06F9XD2M71D1XFT7FJX62KD8HM`, `06F9XD2TGEYEG6S0AK86YF295M`, and `06F9XD33MNNVHHW232TC7T1CN8`.
- Split recommendation: If the cleanup-verification rerun expands beyond a bounded smoke/read proof, split that expansion into a separate validation-only follow-up instead of widening this baseline ticket.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9078`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `5ed419c438914a0aad14798d76b74001`
- completed-at-utc: `<redacted>-07T10:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9XD26D2MHVAKZ2GCZ67BEFC/runs/20260607T105016445Z-5ed419c438914a0aad14798d76b74001.json`