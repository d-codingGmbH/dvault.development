[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSCF61N0TYPYH7008TRD6VR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCF61N0TYPYH7008TRD6VR`.
- Optimistic claim succeeded (`expectedRevision=06FD1397BR7GKA535YQC6WMYKW`, `currentRevision=06FD13FNV0QK5X5F312Q2KBGCR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCF61N0TYPYH7008TRD6VR-story-define-provider-read-parity-acceptance-cri' from source 'd368c58b601bb393ed3e7e070ccaef5d41cde70f'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSCF61N0TYPYH7008TRD6VR-story-define-provider-read-parity-acceptance-cri` as `03a509f9792a`.

Open questions / Risiken
- Risky assumption: Approval assumes the items under `## Follow-Up Questions` remain informational and do not need PO answers before downstream tickets apply the documented gates, because `## Open Questions` is explicitly `none` in `description.md:54-59`.
- Split recommendation: No new split recommended. Keep `06FBSCGBG8CJ0QNRX4JZJA638G` as the PIT/bridge audit lane and the five latest-satellite gap tasks `06FBSCFDFFYQXBK17RT3E8W4CM`, `06FBSCFKWGQMBEF5Q96AZ5Q0X0`, `06FBSCFVT3SBHKMDGNEXWVWFXG`, `06FBSCG18KBRT1FTHDRX073EF4`, and `0...

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9501`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `dac69513e45549eb94df3b18283aa11f`
- completed-at-utc: `<redacted>-16T13:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCF61N0TYPYH7008TRD6VR/runs/20260616T131820079Z-dac69513e45549eb94df3b18283aa11f.json`