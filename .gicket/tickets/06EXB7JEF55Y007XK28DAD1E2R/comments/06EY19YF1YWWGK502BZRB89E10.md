[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB7JEF55Y007XK28DAD1E2R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7JEF55Y007XK28DAD1E2R`.
- Optimistic claim succeeded (`expectedRevision=06EY18SCZZ1QRV1XQSMZB2XTS8`, `currentRevision=06EY18WYZXX600ATV2BSC34E9W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit' from source '14f5b073269236b1038469a1947b676fd4f9af92'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit` as `63c02d821877`.

Open questions / Risiken
- Risky assumption: The implementation can add meaningful Postgres-backed tests without widening the public API, even though the public `ApplyDataVaultMetadata` path remains SQLite-default and the built-in profiles currently stop at `Sqlite`.
- Risky assumption: Developers will choose a clear environment-variable naming contract during implementation; the ticket intentionally fixes the mechanism class (`environment variables`) but not the exact variable names or minimum key set.
- Risky assumption: Documentation will make it unmistakable that this ticket adds local optional test coverage only and does not imply supported runtime Postgres provider behavior.
- Split recommendation: No split recommended; the contract remains bounded to local opt-in test gating, clear skip diagnostics, documentation, and preserving the default no-Postgres validation path.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9410`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `00fd2812715548f589547fe247749bfc`
- completed-at-utc: `<redacted>-30T23:09:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7JEF55Y007XK28DAD1E2R/runs/20260430T230946503Z-00fd2812715548f589547fe247749bfc.json`