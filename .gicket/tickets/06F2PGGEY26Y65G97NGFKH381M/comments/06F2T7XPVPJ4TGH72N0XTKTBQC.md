[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface' for ticket '06F2PGGEY26Y65G97NGFKH381M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGGEY26Y65G97NGFKH381M`.
- Optimistic claim succeeded (`expectedRevision=06F2T5YAJMSD34J7GPPFAQJ67W`, `currentRevision=06F2T6AXWX08HW5HWY2XD4A94W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface' from source 'ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface'.
- Interactive tester tool loop completed review for branch 'ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface'.
- Evidence: `git rev-parse --abbrev-ref HEAD` returned `ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface`.
- Evidence: `git diff --name-only develop...ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface -- src docs tests .github tools` returned no paths; the earlier unrestricted diff output showed only `.gicket/tickets/06F2PGGEY26Y65G97NGFKH381M/**` entries.
- Evidence: `git ls-files` confirmed the presence of `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs`, `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs`, `src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaul...
- Evidence: `git ls-files 'src/*/*.csproj'` listed only `DCoding.Data.DVault`, `DCoding.Data.DVault.Analyzers`, the five provider projects, and `DCoding.Data`; no DVault-owned CLI project is present.
- Evidence: `rg -n "Microsoft\.EntityFrameworkCore\.Design" src --glob '*.csproj'` exited with no matches.
- Evidence: Repository inspection of `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` shows verb dispatch for `validate`, `export`, `drift`, and `guardrail`, usage-error exit code `2`, validate using `host.Diagnostics.Analyze(dbContext)`, drift using artifact import plus ...
- 65 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to the integrator gate.
- No tester rework is required for this ticket branch.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8675`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `814e6fc0954742e387ddfcd7a376eb65`
- completed-at-utc: `<redacted>-15T19:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGGEY26Y65G97NGFKH381M/runs/20260515T193153507Z-814e6fc0954742e387ddfcd7a376eb65.json`