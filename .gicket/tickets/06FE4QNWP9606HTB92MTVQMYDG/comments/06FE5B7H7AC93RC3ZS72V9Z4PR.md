[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FE4QNWP9606HTB92MTVQMYDG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QNWP9606HTB92MTVQMYDG`.
- Optimistic claim succeeded (`expectedRevision=06FE58XWSGM9HVN5KERGMJGKJM`, `currentRevision=06FE594GAY384GSGAJAPAT5NBM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4QNWP9606HTB92MTVQMYDG-story-define-v0-42-provider-evidence-and-tuning' from source 'c971352f087cbdb94dca921e5b5b80d99fee656a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FE4QNWP9606HTB92MTVQMYDG-story-define-v0-42-provider-evidence-and-tuning` as `56a59ab83a0e`.

Open questions / Risiken
- Risky assumption: Later tooling will treat the current `blocks` + `relates` graph as authoritative until a separate relation-normalization ticket intentionally changes the live relation model.
- Split recommendation: No additional split recommended; the story already has `3` active `blocks` relations and `10` active `relates` relations to downstream tickets, and the contract now states that live model explicitly.

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
- run-id: `f63a32420e9243ca92674ee0037bdd55`
- completed-at-utc: `<redacted>-20T01:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QNWP9606HTB92MTVQMYDG/runs/20260620T013725302Z-f63a32420e9243ca92674ee0037bdd55.json`