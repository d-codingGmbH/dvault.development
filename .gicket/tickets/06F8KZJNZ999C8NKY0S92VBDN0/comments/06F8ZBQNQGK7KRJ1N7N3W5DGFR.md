[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZJNZ999C8NKY0S92VBDN0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZJNZ999C8NKY0S92VBDN0`.
- Optimistic claim succeeded (`expectedRevision=06F8Z9WK2BBH0KNS9QKTVFDZ6W`, `currentRevision=06F8ZA37MYKJN9CDFMNKCA64MC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZJNZ999C8NKY0S92VBDN0-story-add-mysql-and-oracle-pit-bridge-read-strat' from source '99e1523e50d574186f0c8b619d4c82091c9fe88e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZJNZ999C8NKY0S92VBDN0-story-add-mysql-and-oracle-pit-bridge-read-strat` as `1856bb9b8116`.

Open questions / Risiken
- Risky assumption: Assumes a deliberate-decline outcome can satisfy this story without widening into README/performance-profile work, because broad documentation follow-up is already split to 06F8KZKFTCC0YXAPRTXA53DNEC.
- Risky assumption: Assumes one provider may ship a candidate while the other ships a decline within this same story, because the acceptance criteria are phrased per provider and split-by-provider remains optional.
- Risky assumption: Assumes benchmark-row work remains downstream and non-blocking for this story, because 06F8KZK2MSFQP9G2DBM61ZVGD4 already exists as a separate task.
- Split recommendation: Keep the story whole while the work stays inside MySQL/Oracle read-strategy registration, gate evaluation, diagnostics, parity coverage, or explicit decline evidence.
- Split recommendation: Split by provider only if one provider becomes decline-only or needs materially different validation effort than the other.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8995`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `597d943c352b43a3bc42ef379bc24d87`
- completed-at-utc: `<redacted>-03T22:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZJNZ999C8NKY0S92VBDN0/runs/20260603T225108600Z-597d943c352b43a3bc42ef379bc24d87.json`