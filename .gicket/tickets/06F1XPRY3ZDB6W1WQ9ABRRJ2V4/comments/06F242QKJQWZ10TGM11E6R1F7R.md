[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XPRY3ZDB6W1WQ9ABRRJ2V4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`.
- Optimistic claim succeeded (`expectedRevision=06F240Q9KK7ZJYEDBPS5W1D7ZC`, `currentRevision=06F2413GA9G84N2XRX6AHG7RVR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails' from source 'e62cebf080b0cc814d7f381a7b393ecd6f6c2d2c'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails` as `8ed3ca537429`.

Open questions / Risiken
- Blocking finding: The parent epic no longer owns direct developer work: its contract and latest PO comment both say the only unmet deliverable is the follow-up docs ticket 06F23Z08K0W49K5JMEHP60WZC0. Approving this parent epic for dev would hand off a tracking ticket instead o...
- Required PO action: Keep the parent epic as a tracking ticket and do not hand it off directly to dev while the remaining repository artifact is owned by 06F23Z08K0W49K5JMEHP60WZC0.
- Required PO action: Move ticket 06F23Z08K0W49K5JMEHP60WZC0 through PO refinement and PO-critic so the developer handoff lands on the ticket that actually owns docs/releases/v0.8.0.md.
- Required PO action: After the follow-up is routed, keep the parent epic blocked on that ticket rather than implying direct implementation remains on this branch.
- Split recommendation: Keep the current docs-only follow-up split 06F23Z08K0W49K5JMEHP60WZC0.
- Split recommendation: Do not reopen the four done implementation stories on this epic branch.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9149`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `831cc35a9227467ca0a8c1846c6e3395`
- completed-at-utc: `<redacted>-13T15:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/runs/20260513T155322852Z-831cc35a9227467ca0a8c1846c6e3395.json`