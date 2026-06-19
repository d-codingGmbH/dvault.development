[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSCFDFFYQXBK17RT3E8W4CM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCFDFFYQXBK17RT3E8W4CM`.
- Optimistic claim succeeded (`expectedRevision=06FD1GN7DJFVF4TRMH0ZW9QPJ8`, `currentRevision=06FD1GVQ5R0G83T6RRKXFVPMWG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap' from source 'bc542c0dca115ebca056ed141d048d8ef85c7dce'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap` as `3198a33cf6bb`.

Open questions / Risiken
- Risky assumption: Assuming PIT/bridge candidate registration implies PostgreSQL latest-satellite coverage would be wrong; current repo evidence limits `PostgresDataVaultReadStrategy` to PIT/bridge lanes.
- Risky assumption: Assuming skipped-placeholder guidance rows can support a PostgreSQL performance claim would be wrong; the contract and evidence matrices treat them as non-timing evidence only.
- Split recommendation: No split recommended; the current ticket already isolates PostgreSQL latest-satellite from PIT/bridge work, and downstream ticket `06FBSCHBJEYYERDPA7JN34Y8PG` already exists to publish the final outcome.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9405`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `86684edfea674b8d8069e09f4ee2e9be`
- completed-at-utc: `<redacted>-16T14:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCFDFFYQXBK17RT3E8W4CM/runs/20260616T141651083Z-86684edfea674b8d8069e09f4ee2e9be.json`