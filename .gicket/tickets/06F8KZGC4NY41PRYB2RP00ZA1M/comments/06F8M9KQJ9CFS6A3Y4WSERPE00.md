[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZGC4NY41PRYB2RP00ZA1M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZGC4NY41PRYB2RP00ZA1M`.
- Optimistic claim succeeded (`expectedRevision=06F8M7Q8X9ZK8DN8AP8W98S8QC`, `currentRevision=06F8M8294TQSZEAKM9Q2RXTH44`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract' from source '0ce2436f57931acda6026b56de993fbd66dae7ad'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract` as `05c9b180b412`.

Open questions / Risiken
- Risky assumption: The contract assumes diagnostics stay high-confidence and skip opaque or indirect cache-key computations; if implementation starts inferring through helpers or cross-assembly abstractions, false positives will likely follow.
- Risky assumption: The contract assumes `UseModel(...)` is only diagnosable when the same visible source scope proves variable model shape; the documented fixed-model compiled-compatibility lane must remain non-diagnostic.
- Risky assumption: The pooling rule is intentionally bounded to `AddDbContextPool<TContext>(...)`; any extension to `AddPooledDbContextFactory<TContext>` or other entrypoints would need a separate ticket-level decision.
- Split recommendation: No further split is recommended; the persisted contract already separates contract 06F8KZGC4NY41PRYB2RP00ZA1M from implementation 06F8KZGNRG5FY4WWCY3FAX2NS4, fixtures 06F8KZGZND5ZCH147PVBRWXYN4, and docs 06F8KZHAB717MJJNAWWK7S0A5W.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8685`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `0471e9f7d0cd4fb785bdec79b7ada76a`
- completed-at-utc: `<redacted>-02T21:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZGC4NY41PRYB2RP00ZA1M/runs/20260602T210357323Z-0471e9f7d0cd4fb785bdec79b7ada76a.json`