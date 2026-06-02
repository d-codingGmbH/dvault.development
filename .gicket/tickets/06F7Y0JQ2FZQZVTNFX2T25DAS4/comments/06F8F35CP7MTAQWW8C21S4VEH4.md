[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F7Y0JQ2FZQZVTNFX2T25DAS4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0JQ2FZQZVTNFX2T25DAS4`.
- Optimistic claim succeeded (`expectedRevision=06F8F0WRWGM1C3583KEVXK3Y1C`, `currentRevision=06F8F16TZ2SCN43DDEY2Y8WS5R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F7Y0JQ2FZQZVTNFX2T25DAS4-story-define-provider-performance-tuning-diagnos' from source '04cbc803c22991f50b7d2360d9e8ca3b357903b9'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F7Y0JQ2FZQZVTNFX2T25DAS4-story-define-provider-performance-tuning-diagnos` as `54da0fe1ebf1`.

Open questions / Risiken
- Risky assumption: Treating MySQL as having one undifferentiated minimum gate would be unsafe; `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` has `MinimumMySqlOptimizedBatchOperationCount = 50` and `MinimumMySqlStagedBatchOperationCount = 60`.
- Risky assumption: Treating non-SQLite provider read optimization as repository-proven would be unsafe; `docs/performance-profiles.md` says SQLite is the only repository-proven optimized latest-satellite, PIT, or bridge read path in the checked-in artifact set.
- Risky assumption: Treating the historical `06F7Y0HZKHBHMYX9EYDYFRYXZ0 -> 06F7Y0JQ2FZQZVTNFX2T25DAS4` `blocks` relation as active would be unsafe; the related ticket is `done`, the current ticket has `isBlocked: false`, and `git log --all --grep '06F7Y0HZKHBHMYX9EYDYFRYXZ0' -n ...
- Split recommendation: Keep this ticket contract-only; implementation of provider eligibility, threshold, and recommendation diagnostics stays in `06F7Y0JZKTVBGGQ9Q4EBC2PCDG` and benchmark-artifact verification stays in `06F7Y0K95VW0PX21F6R2YGP8DM`.
- Split recommendation: If the team later wants new benchmark profiles, provider-specific read thresholds, or transport, reporting, or exporter surfaces, open separate follow-up tickets instead of widening this v1 contract.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9231`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `4291ff9ec9e2470a8aafaa352f536d2c`
- completed-at-utc: `<redacted>-02T08:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0JQ2FZQZVTNFX2T25DAS4/runs/20260602T085643948Z-4291ff9ec9e2470a8aafaa352f536d2c.json`