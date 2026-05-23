[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492D05THPGQVT3B3K7853A0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492D05THPGQVT3B3K7853A0`.
- Optimistic claim succeeded (`expectedRevision=06F5DV22NC7V866MK75TJ715DR`, `currentRevision=06F5DVVG8XSE72JS4PS2THY0G4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492D05THPGQVT3B3K7853A0-task-update-v0-18-0-documentation-and-release-no' from source 'e2de538f7a2c0eb69836c44fb105c19f7d2233e0'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492D05THPGQVT3B3K7853A0-task-update-v0-18-0-documentation-and-release-no` as `1a6582da1b14`.

Open questions / Risiken
- Risky assumption: Assumes a forward-looking cross-reference to the final approval record is acceptable before the final approval artifact itself exists, because the contract explicitly authorizes the pending-approval placeholder path.
- Risky assumption: Assumes the existing README read-shape diagnostics section is the intended canonical local source for the `request-bound read-shape diagnostics surface` referenced by the acceptance criteria.
- Split recommendation: No split recommended; the scope already fits one documentation and release-note rollup, and the delivery contract explicitly says historical done-ticket blocks do not require a split.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `91776`
- effective-cache-ratio: `0.6458`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f6b2066ca360434da8d144dc081c2ba7`
- completed-at-utc: `<redacted>-23T22:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492D05THPGQVT3B3K7853A0/runs/20260523T222734778Z-f6b2066ca360434da8d144dc081c2ba7.json`