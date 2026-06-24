[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF43F283QFQ56290AVJ3AXSM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43F283QFQ56290AVJ3AXSM`.
- Optimistic claim succeeded (`expectedRevision=06FFGHGW995WQ41142Y5V8FHJM`, `currentRevision=06FFGHS8VVY3PHM4GXWMCGX9M0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43F283QFQ56290AVJ3AXSM-task-document-provider-pit-expansion-decision-ma' from source '58719a99caabe17e5b3fc480932eadad6a9f7257'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF43F283QFQ56290AVJ3AXSM-task-document-provider-pit-expansion-decision-ma` as `8a8a34cb80e0`.

Open questions / Risiken
- Risky assumption: The future DB2 follow-up may be referenced descriptively without a persisted ticket id during this ticket's implementation; that matches the current contract, but downstream docs will need a concrete id once the child is created.
- Split recommendation: Reuse existing MySQL follow-up ticket 06FFDG522514HX2J17GT9VE77W for the ordinary hub-parent lane.
- Split recommendation: Create one separate DB2 implementation ticket limited to `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` push-down through `IDataVaultProviderPitMaintenanceStrategy`.
- Split recommendation: Keep Oracle deferred and do not create an Oracle implementation child from this matrix.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6315`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7163255b05d14db58c69c34a18934a28`
- completed-at-utc: `<redacted>-24T06:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43F283QFQ56290AVJ3AXSM/runs/20260624T062709980Z-7163255b05d14db58c69c34a18934a28.json`