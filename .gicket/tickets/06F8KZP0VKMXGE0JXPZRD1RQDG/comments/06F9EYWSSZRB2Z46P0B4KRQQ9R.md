[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZP0VKMXGE0JXPZRD1RQDG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZP0VKMXGE0JXPZRD1RQDG`.
- Optimistic claim succeeded (`expectedRevision=06F9EWVBWD50A7CG4DNFVS5W0R`, `currentRevision=06F9EX2EZMCXCDB2VGSWRX6ZR4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' from source 'b34dcb91ef13a60dc265d5ed45726b6bc88f000a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag` as `c76852feb9c7`.

Open questions / Risiken
- Risky assumption: Approval assumes the queued create-ticket replay `mutation-d16ba25963e2af83` will materialize cleanly and become the visible active carrier before delivery tracking or closure automation depends on a concrete replacement-ticket ULID.
- Risky assumption: Approval also assumes developers will treat the stale incoming `blocks` relation as closure-stage housekeeping only and will not reopen analyzer/generator scope that the epic explicitly marked out of scope.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9286`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d9e549e4b0904f8b8bd8486f88b92d91`
- completed-at-utc: `<redacted>-05T11:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/runs/20260605T111200460Z-d9e549e4b0904f8b8bd8486f88b92d91.json`