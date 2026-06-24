[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF43DC469VQ1N0NQ84KEV6SR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43DC469VQ1N0NQ84KEV6SR`.
- Optimistic claim succeeded (`expectedRevision=06FFDPQZR0DDHANHF4BDX44VFM`, `currentRevision=06FFDTQJDRGKWCF4BW665A19YW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43DC469VQ1N0NQ84KEV6SR-task-evaluate-oracle-pit-full-rebuild-push-down' from source '50cf61ea67e961f3788150bfe91cb6ee36ee4e8d'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF43DC469VQ1N0NQ84KEV6SR-task-evaluate-oracle-pit-full-rebuild-push-down` as `c0937534c2e4`.

Open questions / Risiken
- Risky assumption: Oracle PIT read strategy and read benchmark evidence are not sufficient proof of PIT rebuild push-down safety; the ticket correctly treats them as comparison context only.
- Risky assumption: Oracle EF Core transaction surfaces may not provide SQL Server-style rollback-clean failure behavior for full rebuilds, so an implementation recommendation could require a different seam or a defer outcome.
- Risky assumption: The existing provider-strategy seam may or may not fit Oracle cleanly; the ticket already preserves the option that Oracle could need SQL Server-style service ownership instead of a PostgreSQL-style strategy.
- Split recommendation: No split is needed before development; keep this ticket as the bounded investigation and open a separate implementation ticket only if the investigation proves a narrowly guarded Oracle full-rebuild candidate.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9310`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f50e78971b9c4972ade49104d94b8fa2`
- completed-at-utc: `<redacted>-24T00:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43DC469VQ1N0NQ84KEV6SR/runs/20260624T000427086Z-f50e78971b9c4972ade49104d94b8fa2.json`