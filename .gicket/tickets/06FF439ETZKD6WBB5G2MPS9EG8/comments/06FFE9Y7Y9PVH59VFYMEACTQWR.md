[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF439ETZKD6WBB5G2MPS9EG8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF439ETZKD6WBB5G2MPS9EG8`.
- Optimistic claim succeeded (`expectedRevision=06FFE4S0DXMJY6FHVGCG11X30M`, `currentRevision=06FFE8NDDYKYG1KS2B0G6GBXY8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi' from source 'f414a5fcb483481e59806fadc63c85406ba771d8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi` as `dd7f60b1b13d`.

Open questions / Risiken
- Risky assumption: The scope-in bullet at `description.md:18-20` groups `latest-satellite` with PIT/bridge read evidence; implementation must keep the maintained-row prerequisite scoped to PIT/bridge reads rather than implying latest-satellite uses maintained read-model rows.
- Risky assumption: The ticket assumes the existing v0.45.0 release note, architecture note, performance guide, evidence matrix, and gap matrix are sufficient citation surfaces, so no new benchmark artifacts or taxonomy changes are needed.
- Risky assumption: The ticket assumes only documentation alignment is needed because the branch diff from `develop` to `HEAD` contains ticket metadata only and no partial repo implementation to reconcile.
- Split recommendation: No split recommended; the persisted contract and current branch evidence bound this to one documentation-alignment task across `docs/performance-profiles.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md`.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8500`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `71de20a3b067443cac9c078c07dea10b`
- completed-at-utc: `<redacted>-24T01:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF439ETZKD6WBB5G2MPS9EG8/runs/20260624T010359980Z-71de20a3b067443cac9c078c07dea10b.json`