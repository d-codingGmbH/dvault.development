[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RPKGTEW4RZKYQ2DXS554'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RPKGTEW4RZKYQ2DXS554`.
- Optimistic claim succeeded (`expectedRevision=06EYN6BJ1XQYW7FZ52K5XVMZHW`, `currentRevision=06EYN6JR77K3XZ1WA9XTZVAN4R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil' from source 'c91bdfadcffeb3e4d780779297b7216c1a143e44'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` as `1ad0555a94a6`.

Open questions / Risiken
- Blocking finding: The persisted delivery contract defines 06EXB7RPKGTEW4RZKYQ2DXS554 as a coordination-only umbrella with no parent-owned implementation slice, so approving it for developer handoff would misroute a ticket that has no remaining developer work.
- Required PO action: Update the parent ticket's status and handoff metadata so it follows a coordination-only completion or closure path instead of the po-critic-to-dev route.
- Required PO action: Keep the parent scoped as umbrella coordination only; do not create or reopen a parent-owned src/ or tests/ implementation slice.
- Risky assumption: Assuming downstream automation or reviewers will infer 'no developer work remains' from the narrative alone despite the current status and labels still signaling dev/test blockage.
- Risky assumption: Assuming this umbrella can safely enter a developer queue without being reopened as a third implementation ticket.
- Split recommendation: No further split is recommended for this umbrella; keep implementation ownership with child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- Split recommendation: If stakeholders still want a runnable example, broader relationship demo, or additional history variants, create separate follow-up tickets instead of reopening this parent for developer work.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8522`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `3c7681172c364e548d0166cc008444bc`
- completed-at-utc: `<redacted>-02T21:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/runs/20260502T213421764Z-3c7681172c364e548d0166cc008444bc.json`