[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKJBG7NGNVBN0ZDSBE6B8`.
- Optimistic claim succeeded (`expectedRevision=06F3E1S8J0R4PCMKFY3PKS1XKC`, `currentRevision=06F3E1Y7NP03HK2W6TSHRGJDDM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project' from source 'f2ecf6cb2329c1a055b7a442ea8b79c3fd31f1fe'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project` as `b5b5b07b44f2`.

Open questions / Risiken
- Required PO action: Move the ticket out of the dev handoff path and into the project's closed/no-work-required state; remove the current dev/test blocking labels when doing so.
- Required PO action: If the actual product intent is new fluent code-first link-parent satellite support, open a separate feature ticket instead of reopening this already-covered closure ticket.
- Risky assumption: This assessment assumes the intended outcome is to confirm existing metadata-first/EF projection coverage, not to deliver a new code-first declaration capability.
- Risky assumption: This assessment assumes the workflow allows Product Owner follow-up to close a no-work-required ticket without sending it through developer implementation.
- Split recommendation: No split for this ticket itself. If needed later, create separate tickets for fluent code-first link-parent satellite declarations and for broader provider-specific test hardening.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8921`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `81628c390e7e45a3a3b2a6d8396957c9`
- completed-at-utc: `<redacted>-17T17:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/runs/20260517T174621792Z-81628c390e7e45a3a3b2a6d8396957c9.json`