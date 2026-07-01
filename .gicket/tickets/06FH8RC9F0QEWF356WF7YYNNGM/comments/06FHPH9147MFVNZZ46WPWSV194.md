[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8RC9F0QEWF356WF7YYNNGM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RC9F0QEWF356WF7YYNNGM`.
- Optimistic claim succeeded (`expectedRevision=06FHPFF6DT84GT3B8CBZGDKPCW`, `currentRevision=06FHPFVQN9RQG03X5VAP7FENWW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8RC9F0QEWF356WF7YYNNGM-task-close-selected-provider-save-strategy-parit' from source 'aadca0bd3f2a09e52d2a423e85491bc079694c3c'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8RC9F0QEWF356WF7YYNNGM-task-close-selected-provider-save-strategy-parit` as `aad7b0ee2851`.

Open questions / Risiken
- Risky assumption: Developer handoff must treat this as a save-only parity ticket; adjacent PIT/read/provider-maintenance code already in the repository is not authorization to widen scope.
- Risky assumption: Reviewers and implementers must use the root `sqlserver-threshold-decision.md` as the live SQL Server authority and not fall back to the older historical diagnostics bundle's superseded 50-operation minimum.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9127`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `dbe892050afc4703803c1b40e041331c`
- completed-at-utc: `<redacted>-01T01:22:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RC9F0QEWF356WF7YYNNGM/runs/20260701T012223133Z-dbe892050afc4703803c1b40e041331c.json`