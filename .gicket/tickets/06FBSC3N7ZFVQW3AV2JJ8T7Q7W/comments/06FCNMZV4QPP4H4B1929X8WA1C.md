[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSC3N7ZFVQW3AV2JJ8T7Q7W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC3N7ZFVQW3AV2JJ8T7Q7W`.
- Optimistic claim succeeded (`expectedRevision=06FCNJPZNH89RBTHBBQVCY4Y2W`, `currentRevision=06FCNJXKS28ZM2BJVPV9M518JC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSC3N7ZFVQW3AV2JJ8T7Q7W-story-define-provider-optimization-evidence-matr' from source 'f068b34f0bacaf553928b53ab8507f1bad5e8808'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSC3N7ZFVQW3AV2JJ8T7Q7W-story-define-provider-optimization-evidence-matr` as `b1a6b3ef4851`.

Open questions / Risiken
- Risky assumption: The implementation will cite mixed-version source baselines by evidence boundary instead of flattening them into one release claim: `docs/performance-profiles.md:1-35` is v0.32.0 guidance, `docs/releases/v0.34.0.md:41-43` carries the DB2 posture, and `hash-ke...
- Split recommendation: If the work expands into new measured evidence, split DB2 benchmark-lane work away from the documentation-only matrix story.
- Split recommendation: Keep any future automated matrix generation or provider-specific hash-key expansion as separate tooling/evidence tickets rather than enlarging this handoff story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9175`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7ce4e02cf61c4b5687e80e1e585ca978`
- completed-at-utc: `<redacted>-15T10:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC3N7ZFVQW3AV2JJ8T7Q7W/runs/20260615T102910559Z-7ce4e02cf61c4b5687e80e1e585ca978.json`