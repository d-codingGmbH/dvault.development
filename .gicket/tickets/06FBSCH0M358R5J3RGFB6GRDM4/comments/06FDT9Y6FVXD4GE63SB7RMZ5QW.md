[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSCH0M358R5J3RGFB6GRDM4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCH0M358R5J3RGFB6GRDM4`.
- Optimistic claim succeeded (`expectedRevision=06FDT7ZQNZXY9TQ9PCGSD24CW8`, `currentRevision=06FDT880BB7SZ6FG3KAS5PN100`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps' from source 'c669fc274795802d575699220f4fa776a2891c66'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps` as `dff4c964c6bf`.

Open questions / Risiken
- Risky assumption: Assumes the existing smoke-read Oracle bundle can only satisfy this ticket if documentation and evidence posture are updated together; otherwise a new provider-configured bundle will be needed.
- Risky assumption: Assumes Oracle latest-satellite remains out of scope even if the smoke-read bundle contains a completed latest-satellite-read row, because providerSpecificReadStrategy=not registered for latest satellite reads is still the documented boundary in benchmark det...
- Risky assumption: Assumes the live blocks chain 06FBSCGBG8CJ0QNRX4JZJA638G -> 06FBSCH0M358R5J3RGFB6GRDM4 -> 06FBSCHBJEYYERDPA7JN34Y8PG is housekeeping rather than a dev-start gate, because the delivery contract says live relations remain unchanged and Follow-Up Questions treat...
- Split recommendation: No split recommended; the ticket is already tightly bounded to Oracle PIT/bridge evidence closure plus doc and verifier alignment.
- Split recommendation: Do not separate doc-only work from verifier/evidence alignment, because the repository currently has conflicting signals between the canonical skipped root matrix and the existing smoke-read Oracle bundle.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9135`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `43703f59c57c44c299a96873e11271c8`
- completed-at-utc: `<redacted>-18T23:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCH0M358R5J3RGFB6GRDM4/runs/20260618T235351994Z-43703f59c57c44c299a96873e11271c8.json`