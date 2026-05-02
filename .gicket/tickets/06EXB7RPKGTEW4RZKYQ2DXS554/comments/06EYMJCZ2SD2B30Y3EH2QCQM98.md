[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RPKGTEW4RZKYQ2DXS554'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RPKGTEW4RZKYQ2DXS554`.
- Optimistic claim succeeded (`expectedRevision=06EYMFQ7ZJCJY5K86RM5VYQE2R`, `currentRevision=06EYMHE39EVFVNEWRTVKFKZ18R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil' from source '551345f154eb9c1689dadacaef20b9e2bb286c2c'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` as `eaca194f94da`.

Open questions / Risiken
- Blocking finding: Approving this ticket for dev would hand a coordination-only umbrella story to the developer path even though the persisted contract explicitly says no separate parent-owned implementation slice remains.
- Blocking finding: The contract's Implementation Notes claim an "empty diff" from `f09f90eb355182c23033b1bba082c44cc75ee9c4` to `HEAD`, but that ref range is not literally empty; it contains `.gicket` metadata changes. The intended evidence is narrower: no non-`.gicket` impleme...
- Required PO action: Persist the correct terminal workflow for this umbrella parent: either close/advance it without a dev handoff, or explicitly define a real parent-owned implementation slice if developer work is still intended.
- Required PO action: Correct the stale diff evidence in the contract so it matches observed refs and outputs, including the distinction between `.gicket` ticket metadata changes and repository implementation changes.
- Risky assumption: It assumes readers will interpret the cited "empty diff" as "no implementation diff" despite the stated ref range containing ticket-metadata changes.
- Risky assumption: It assumes the existing title/labeling around "example" will not pull this umbrella ticket back into runnable-sample scope that the contract explicitly scopes out.
- Split recommendation: Keep the existing split: the plain EF work stays in 06EXB7RYFJ3YQDB1E4QHPP8034 and the DVault work stays in 06EXB7S6DB97GVVTS2GGZ3CCX8.
- Split recommendation: If stakeholders want a runnable example, broader relationship demo, or more comparison variants, create a new follow-up ticket instead of routing this umbrella parent to dev.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9283`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `acfcad3f948f4f419a633d5454fb8813`
- completed-at-utc: `<redacted>-02T20:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/runs/20260502T200306006Z-acfcad3f948f4f419a633d5454fb8813.json`