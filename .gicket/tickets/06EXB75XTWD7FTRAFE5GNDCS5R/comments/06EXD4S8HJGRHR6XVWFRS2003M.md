[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB75XTWD7FTRAFE5GNDCS5R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB75XTWD7FTRAFE5GNDCS5R`.
- Optimistic claim succeeded (`expectedRevision=06EXCZCWGW7ZMYDHGP4HZ8SQ3R`, `currentRevision=06EXD3P2WPPY98XAJQMXZA03VC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies' from source 'bd8a187ee2df1b0ad515e1e90c4a6ad67d4b4252'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies` as `30def7ab7d78`.

Open questions / Risiken
- Risky assumption: Sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM is still needs-po, so parallel implementation assumes active coordination to avoid conflicting default naming semantics.
- Risky assumption: DataVaultModelOptions is evidenced in the planning document, not current source; once source exists, developers must verify the actual public type/API before treating compatibility as established.
- Split recommendation: No split recommended; the sibling default naming policy ticket remains the separate boundary for detailed default naming rules.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9188`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `bd870a36c2db4006a8537c3b2d48da80`
- completed-at-utc: `<redacted>-29T00:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/runs/20260429T001101042Z-bd870a36c2db4006a8537c3b2d48da80.json`