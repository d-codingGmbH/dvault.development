[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB817Q8RAXCQH5QQR5RFY34'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB817Q8RAXCQH5QQR5RFY34`.
- Optimistic claim succeeded (`expectedRevision=06EYW44H4NS79BAKYRAZ5FAGV4`, `currentRevision=06EYW48KZ736ZG8AH8FQJ7DB6R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB817Q8RAXCQH5QQR5RFY34-task-enable-xml-docs-warnings-for-public-and-pro' from source '70932187ba0b523ed3bfdbf60d897197d815d424'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB817Q8RAXCQH5QQR5RFY34-task-enable-xml-docs-warnings-for-public-and-pro` as `70b0e1d5f0be`.

Open questions / Risiken
- Risky assumption: Assuming the existing `GenerateDocumentationFile` plus `WarningsAsErrors ... CS1591` settings are sufficient without extra analyzers; the current ticket is correct only if developers verify missing-doc failures and do not rely on property presence alone.
- Risky assumption: Assuming package-shipped XML docs will be proven from real pack artifacts in `bin/packages/` or package contents, not inferred from `PackageOutputPath`/`GenerateDocumentationFile` settings.
- Split recommendation: No split recommended; the downstream API approval/snapshot testing work is already separated into ticket `06EXB81FSWAA6N1HMYQ0CM4S8G`.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9502`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `15dc4d5f1812455ead7f11ff2c45e622`
- completed-at-utc: `<redacted>-03T13:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB817Q8RAXCQH5QQR5RFY34/runs/20260503T134603805Z-15dc4d5f1812455ead7f11ff2c45e622.json`