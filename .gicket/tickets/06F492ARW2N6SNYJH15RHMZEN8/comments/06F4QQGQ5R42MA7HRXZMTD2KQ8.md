[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492ARW2N6SNYJH15RHMZEN8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492ARW2N6SNYJH15RHMZEN8`.
- Optimistic claim succeeded (`expectedRevision=06F4PFKZD1F67FT3GP9KY0FVYR`, `currentRevision=06F4QNQG1METK1P5EA2PS3MREW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in' from source '42ca4f21a0c3e0cfdd260201eb8056963eb23b30'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in` as `2f83c9f800ae`.

Open questions / Risiken
- Risky assumption: Implementation can identify generated DVault tables and technical metadata from stable symbol/annotation surfaces instead of brittle produced-name heuristics alone.
- Risky assumption: `Obviously unsafe direct generated-table write` can be detected locally with low false-positive risk and without sliding into the runtime guard space already split to ticket `06F492AYE4A3PKA2D20DDPQ37C`.
- Risky assumption: Safe shared-type read patterns must stay exempt across ordinary LINQ, `AsNoTracking()`, and compiled-query call sites, not just one documented example.
- Split recommendation: No split recommended at PO gate; the current sibling tickets already isolate runtime guard, query-shape, preflight, drift, and docs work from this analyzer slice.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9264`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c55b928f874940c99a20903aa2bd6a42`
- completed-at-utc: `<redacted>-21T18:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492ARW2N6SNYJH15RHMZEN8/runs/20260521T184826663Z-c55b928f874940c99a20903aa2bd6a42.json`