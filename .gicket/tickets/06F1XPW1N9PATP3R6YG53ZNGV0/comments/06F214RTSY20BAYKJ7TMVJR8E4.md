[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F1XPW1N9PATP3R6YG53ZNGV0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPW1N9PATP3R6YG53ZNGV0`.
- Optimistic claim succeeded (`expectedRevision=06F212MDSH755869X89NSSAXF0`, `currentRevision=06F212Y55SNZBCEYG0H2AQAMP4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w' from source '9242c5a1b065409e46ccdc85de059f48bd7cfb06'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w` as `4e630335aa0c`.

Open questions / Risiken
- Risky assumption: The reproduction command filter is intentionally unspecified; the implementation should choose a stable class-level or namespace-level filter rather than a brittle single-test name.
- Risky assumption: The design-time-only constraint depends on stopping at model building and drift comparison; opening a connection or initializing a database would exceed scope.
- Split recommendation: No split is required for this ticket as currently bounded to unit-test coverage plus governance-doc updates.
- Split recommendation: A runnable quickstart under `examples/`, CI gating, or a broader invalid-model matrix should stay separate follow-up tickets.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9113`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `810412f1342e4b619e981276f2fbff62`
- completed-at-utc: `<redacted>-13T09:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPW1N9PATP3R6YG53ZNGV0/runs/20260513T090251650Z-810412f1342e4b619e981276f2fbff62.json`