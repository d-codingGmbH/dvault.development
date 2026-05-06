[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NVE88WW9PMM04NVAZHRG0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NVE88WW9PMM04NVAZHRG0`.
- Optimistic claim succeeded (`expectedRevision=06EZRGG1BCYZM004AR83HPG9KM`, `currentRevision=06EZRGT0XVDQATNGE8C6KKVK3M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar' from source '80225174e425e0b7ac625c708848150faa0319fa'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar` as `b0a2e427900e`.

Open questions / Risiken
- Blocking finding: The child's own acceptance criteria make parent refinement a prerequisite for dev handoff, but parent story 06EZ0NTV4SVAKV98C418T8A3CC is still `needs-po`; this ticket is not ready for developer handoff.
- Blocking finding: There is still no authoritative persisted or source-backed bridge surface to document; starting now would force the docs example to guess bridge names or shapes.
- Required PO action: After the parent contract is refined, re-check this child against that contract and refresh the child if the parent introduces concrete bridge naming or shape details before resubmitting to PO-critic.
- Risky assumption: Assuming parent story 06EZ0NTV4SVAKV98C418T8A3CC can become authoritative without requiring a follow-up sync on this child, even though the child risk section says one more sync pass may be needed before dev handoff.
- Split recommendation: No split recommended while this remains a bounded docs child blocked on parent story 06EZ0NTV4SVAKV98C418T8A3CC.
- Split recommendation: If hierarchy-style traversal later needs its own worked example, create a separate follow-up docs ticket after the parent bridge surface is defined.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8644`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `2a88897264e7490781593fdfa41cb7b4`
- completed-at-utc: `<redacted>-06T07:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/runs/20260506T075625124Z-2a88897264e7490781593fdfa41cb7b4.json`