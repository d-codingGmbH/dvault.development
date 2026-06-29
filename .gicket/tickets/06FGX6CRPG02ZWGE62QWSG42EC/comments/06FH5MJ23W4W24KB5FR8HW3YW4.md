[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FGX6CRPG02ZWGE62QWSG42EC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX6CRPG02ZWGE62QWSG42EC`.
- Optimistic claim succeeded (`expectedRevision=06FH5JNNGW9HRNGQ04VKE094P4`, `currentRevision=06FH5K0TVM2TC3NCMZD8J383H0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati' from source '80f02a54abf465f944678b475bcae12fff0a2f41'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati` as `82347cbcbbdb`.

Open questions / Risiken
- Risky assumption: Assumes implementation will align documentation to the source-backed manifest shape already asserted in `DataVaultDesignTimeCommandTests.cs` (`schemaVersion`/`dryRun`/`source`/`target`/`comparison`/`entries`) instead of the older abstract field names currentl...
- Risky assumption: Assumes any root `README.md` wording change either preserves the packaged README assertions enforced in `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:533-619` or updates those assertions intentionally.
- Risky assumption: Assumes `docs/releases/v0.49.0.md` remains the intended current public release-notes baseline, with `docs/releases/v0.43.0.md` treated only as historical context.
- Split recommendation: No split recommended; repository evidence shows the exporter, validator, preflight lane, and most surrounding docs already exist, so the remaining work is a bounded docs-alignment task across the migration guide, README, and current release notes.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9188`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `06886005c90c40a48aa25e78604ca338`
- completed-at-utc: `<redacted>-29T09:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX6CRPG02ZWGE62QWSG42EC/runs/20260629T095957211Z-06886005c90c40a48aa25e78604ca338.json`