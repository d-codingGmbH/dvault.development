[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZR38EDSVZBCTC0XYR4R80'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZR38EDSVZBCTC0XYR4R80`.
- Optimistic claim succeeded (`expectedRevision=06F9KF2RV2GT21FQMV9PZFFMYG`, `currentRevision=06F9KF9VVT4YAZXBQ1KHDZ9X7R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZR38EDSVZBCTC0XYR4R80-story-define-performance-decision-tree-contract' from source '8e400acc252a1d53c471767ff946b3ca24e602d2'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZR38EDSVZBCTC0XYR4R80-story-define-performance-decision-tree-contract` as `b2873e2e8e36`.

Open questions / Risiken
- Risky assumption: The developer will treat the existing `## Profile Selection` table as supporting input and not leave it as a competing authoritative decision model after the new v0.31.0 section is added.
- Risky assumption: The developer will keep this story scoped to `docs/performance-profiles.md` even though repo-wide current-baseline statements are mixed today: `docs/performance-profiles.md` still says v0.28.0, `docs/production-adoption-checklist.md:9` says v0.29.0, and `READ...
- Risky assumption: Downstream task `06F8KZRSTHAGSP6GPGFBFQGY08` will own practical examples and any optional checklist pointer, so this story can stay contract-only.
- Split recommendation: No split recommended. Keep the contract definition in `06F8KZR38EDSVZBCTC0XYR4R80` and leave practical examples, optional checklist pointer work, and release-doc follow-through to the existing downstream tickets under epic `06F8KZQNH8CCMTJW9P95W1N388`.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9106`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `0a6307e1c68a4ead9272a175a7c116a5`
- completed-at-utc: `<redacted>-05T21:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZR38EDSVZBCTC0XYR4R80/runs/20260605T215217665Z-0a6307e1c68a4ead9272a175a7c116a5.json`