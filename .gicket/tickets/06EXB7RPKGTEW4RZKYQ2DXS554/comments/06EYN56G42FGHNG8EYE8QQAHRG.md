[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RPKGTEW4RZKYQ2DXS554'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RPKGTEW4RZKYQ2DXS554`.
- Optimistic claim succeeded (`expectedRevision=06EYN25RHC7ABT8NYGFMRSQ32C`, `currentRevision=06EYN44EZDSYJ0K4SRAH32NBTM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil' from source '00762884e6913392bb41498528bf4cde3a966d3e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` as `a601575d13a0`.

Open questions / Risiken
- Required PO action: Update the parent ticket's status, labels, and handoff metadata so it follows a coordination-only closure path instead of a developer handoff path.
- Risky assumption: Assuming downstream automation or reviewers will infer 'no developer work remains' from the narrative alone, despite the persisted ticket still looking like a normal `todo` story with dev/test blocker labels.
- Risky assumption: Assuming this umbrella can safely pass through a developer queue without being reopened as a third implementation ticket.
- Split recommendation: No further split for this umbrella. Keep child ownership with 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- Split recommendation: If stakeholders still want a runnable example, broader relationship demo, or more history variants, create a new follow-up ticket instead of routing this parent to development.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9256`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `6a58c549edd345e1a4b8f1bd85207418`
- completed-at-utc: `<redacted>-02T21:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/runs/20260502T212513658Z-6a58c549edd345e1a4b8f1bd85207418.json`