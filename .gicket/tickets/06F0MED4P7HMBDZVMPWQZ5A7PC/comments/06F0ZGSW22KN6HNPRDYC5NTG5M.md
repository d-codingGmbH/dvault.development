[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0MED4P7HMBDZVMPWQZ5A7PC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MED4P7HMBDZVMPWQZ5A7PC`.
- Optimistic claim succeeded (`expectedRevision=06F0ZCGGWD8FTQY7QMZH20THS4`, `currentRevision=06F0ZFHKY940D6896SRPD5BGG4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MED4P7HMBDZVMPWQZ5A7PC-task-implement-data-vault-model-validation-and-e' from source '1a4e53c92e8bc56510453283bc51b29fc5f307db'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0MED4P7HMBDZVMPWQZ5A7PC-task-implement-data-vault-model-validation-and-e` as `e5401d5c7611`.

Open questions / Risiken
- Risky assumption: Assuming fallback-cause reporting can stay accurate if it duplicates provider CanSave logic instead of sharing extracted gates.
- Risky assumption: Assuming provider optimization thresholds will remain stable without synchronized diagnostics-test updates.
- Split recommendation: No split recommended; the refined contract is now sufficiently bounded for implementation inside 06F0MED4P7HMBDZVMPWQZ5A7PC.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9137`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `33b08c7e079d4cfba94de52ec1953869`
- completed-at-utc: `<redacted>-10T02:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MED4P7HMBDZVMPWQZ5A7PC/runs/20260510T024153210Z-33b08c7e079d4cfba94de52ec1953869.json`