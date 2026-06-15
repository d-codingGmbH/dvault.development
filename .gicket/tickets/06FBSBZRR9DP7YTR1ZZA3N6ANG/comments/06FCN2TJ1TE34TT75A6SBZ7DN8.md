[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi' for ticket '06FBSBZRR9DP7YTR1ZZA3N6ANG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSBZRR9DP7YTR1ZZA3N6ANG`.
- Optimistic claim succeeded (`expectedRevision=06FCN1JGCWWYXFZJBTGCM52100`, `currentRevision=06FCN1S26G8CEDQQCP7XBCZ6NC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi' from source 'ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi'.
- Interactive tester tool loop completed review for branch 'ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi'.
- Evidence: `git diff --name-only develop...ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi` listed only `.gicket/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/**` paths; none of the named README/docs/src/tests evidence files changed on the parent branch.
- Evidence: `.gicket/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/description.md` contains the persisted delivery-contract block, states the parent is closure/tracking reconciliation only, names all six completed child tickets, and lists the authoritative evidence surfaces.
- Evidence: `git ls-files` returned all named evidence paths: README.md, CHANGELOG.md, hash-key-footprint.md, docs/getting-started.md, docs/plans/hash-key-storage-profile-contract.md, docs/releases/v0.36.0.md, src/DCoding.Data.DVault/DataVaultOptions.cs, src/DCoding.Data.DVault/...
- Evidence: `git log --all --grep='06FBSBZRR9DP7YTR1ZZA3N6ANG\|06FBSBZY1XEJYK1DRV4RV2ZN88\|06FBSC03KAGDABNFGPK9D95QKR\|06FBSC08W24BJGFZ87RSFS21WC\|06FBSC0EJHAY200E7PXNRGV7XR\|06FBSC0MNH0YAWQ4NY2WSC8KJG\|06FBSC0TMZBXVVECGQGESWPCY4' --max-count=30 --oneline` showed child AUTO-INTE...
- Evidence: Targeted reads confirmed `UseBinaryFirstProfile()` in `src/DCoding.Data.DVault/DataVaultOptions.cs`, `UseDataVaultBinaryFirstProfile()` in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, new-project binary-first guidance with no automatic migration in R...
- Evidence: Ticket status at verification time is 'todo'.
- 54 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator; no developer rework is indicated for this closure-only parent ticket.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7793`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `6719476d104e4f949d9d69d548339441`
- completed-at-utc: `<redacted>-15T09:09:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/runs/20260615T090948679Z-6719476d104e4f949d9d69d548339441.json`