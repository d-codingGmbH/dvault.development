[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F98QHCT7ZJ852PWXCKTW1PMC`, `currentRevision=06F98QPBWQ3TWWACQKVY3VDR04`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source '49e47bfca4823d3fff0ad7e023f916c0bf8cee6f'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` as `3425ac1d3fcb`.

Open questions / Risiken
- Blocking finding: Child coverage is sufficient, but the parent ticket is explicitly closure-only with no residual developer slice. Under the allowed outcome enum, approve_for_dev would misroute the ticket to dev and contradict both the persisted delivery contract and the lates...
- Required PO action: Keep the closure evidence anchored to the four done child tickets and landed develop commits ef35f304c, d23b0e481, fa1f7a1f1, and 826b80b9f when performing the final closure/completion cleanup.
- Risky assumption: This review assumes the lingering .gicket/relations/68/TW/06F8KZNNS76TD9Z7ESB173FZ68--06F8KZM6KFZ3WC5MY5NC12B0TW--blocks.json relation is historical housekeeping, because .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/comments/06F98QE2D09YXATYSWFS4BKFWR.md droppe...
- Risky assumption: This review assumes the workflow can represent a PO-side closure disposition after PO updates the ticket; the current runtime path po-critic.on-success=dev does not match the ticket's closure-only contract.
- Split recommendation: No new split is needed for this epic.
- Split recommendation: Any future provider-expansion or physical-naming-override work should be tracked on new follow-up tickets or epics, not reopened under 06F8KZM6KFZ3WC5MY5NC12B0TW.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9210`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `40eca274b4164c38824a5197370dc2d0`
- completed-at-utc: `<redacted>-04T20:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T204938878Z-40eca274b4164c38824a5197370dc2d0.json`