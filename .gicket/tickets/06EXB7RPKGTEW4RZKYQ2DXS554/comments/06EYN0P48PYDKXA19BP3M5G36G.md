[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RPKGTEW4RZKYQ2DXS554'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RPKGTEW4RZKYQ2DXS554`.
- Optimistic claim succeeded (`expectedRevision=06EYMYPTENYY0ZF1CZWTC8K35M`, `currentRevision=06EYMYXRZM020K3M1H5T09094G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil' from source '122d0c7eb34958571415d0feaeb5af530da8a4e7'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` as `0b0c4c335873`.

Open questions / Risiken
- Blocking finding: Approving this ticket for dev would contradict the persisted contract: the parent is explicitly coordination-only and has no remaining parent-owned repository work to hand to a developer.
- Blocking finding: The parent's current ticket state still advertises active downstream work (`todo`, `critic-needed`, `blocked/dev`, `blocked/test`), so the automation surface is not aligned with the intended non-dev next step.
- Required PO action: Change the parent ticket's workflow outcome so PO-critic success closes or otherwise advances the umbrella without routing it to `dev`.
- Required PO action: Update the parent status/labels to remove the misleading dev/test/critic blockers once the non-dev advance path is chosen.
- Required PO action: If stakeholders actually want new work on the parent, add a new explicit parent-owned scope before any developer handoff instead of relying on the current umbrella contract.
- Risky assumption: Automation or manual triage will honor the description-level instruction to close/advance the parent instead of following the configured `po-critic.on-success: dev` path.
- Risky assumption: The `area/examples` story framing will not cause the parent to be reopened as a third implementation ticket despite the contract saying the implementation lives only in the two done child tickets.
- Split recommendation: No further split is needed for the current umbrella ticket.
- Split recommendation: If a runnable example or documentation sample is desired later, create a new follow-up ticket instead of reopening this parent for development.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9447`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `34f7e2ec488b4500bd02a22f22b2bbfa`
- completed-at-utc: `<redacted>-02T21:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/runs/20260502T210531077Z-34f7e2ec488b4500bd02a22f22b2bbfa.json`