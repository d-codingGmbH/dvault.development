[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FBSBZRR9DP7YTR1ZZA3N6ANG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSBZRR9DP7YTR1ZZA3N6ANG`.
- Optimistic claim succeeded (`expectedRevision=06FCMKEX74C15NG9JFTDC0ZGEC`, `currentRevision=06FCMMV8Y8WMDY93BMD5K940Q4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi' from source '7413d67d8916803ac015acd7110c53d06aa945e1'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail downgraded mistaken tracking-parent closure blockers because the ticket is a direct implementation handoff.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FBSBZRR9DP7YTR1ZZA3N6ANG-story-define-binary-first-new-project-hash-profi` as `457c18381dce`.

Open questions / Risiken
- Blocking finding: Ticket state is inconsistent with its own maintenance comment and with current child completion: the parent is still `todo`/`critic-needed`, but the comment says it should have been marked `tracking/waiting-on-children`, and all observed split child tickets a...
- Required PO action: Decide whether `06FBSBZRR9DP7YTR1ZZA3N6ANG` is still an executable story or is now a tracking/closure ticket for already-completed child work.
- Required PO action: If the parent remains open, replace the one-line description with an authoritative delivery contract that names the exact remaining parent-level outcome, the authoritative repository artifact(s), and how child-ticket completion rolls up into parent acceptance.
- Required PO action: If the parent is closure-only, move it out of the developer handoff path and update its ticket state accordingly instead of sending it to a developer queue.
- Risky assumption: Assuming the parent can still be treated as fresh developer work even though the observed split child tickets are already `done`.
- Risky assumption: Assuming the maintenance comment's `tracking/waiting-on-children` state is already reflected in the current persisted ticket state when the current ticket.json does not show that label.
- Risky assumption: Assuming the repository's landed API/docs/tests are sufficient to close the parent without first stating, at the parent ticket level, whether any parent-specific acceptance remains.
- Split recommendation: No further split is needed. The split already exists and the observed child tickets are done; the needed action is parent-ticket reconciliation, not new child creation.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8437`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `863b5f96fa1249048bd1c2312e233b83`
- completed-at-utc: `<redacted>-15T08:17:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/runs/20260615T081721750Z-863b5f96fa1249048bd1c2312e233b83.json`