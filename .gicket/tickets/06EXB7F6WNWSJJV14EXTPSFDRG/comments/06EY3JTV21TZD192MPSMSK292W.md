[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7F6WNWSJJV14EXTPSFDRG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Optimistic claim succeeded (`expectedRevision=06EY3HJR184A89AYM1GNMEDBB4`, `currentRevision=06EY3HPCEM3QJZBRJ5JH8DXFSW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc' from source 'e2aab038dfb6104ba98f6eefa99f92684b740562'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc` as `8583062b7577`.

Open questions / Risiken
- Required PO action: Re-run PO handoff only after the ticket-level routing metadata no longer implies remaining developer or tester work on the parent epic.
- Risky assumption: This review assumes the stale blocked labels are the only remaining ticket-level contradiction; repository, relation, and child-ticket evidence do not suggest reopened implementation scope.
- Risky assumption: This review assumes workflow can avoid routing a closure-only epic back into implementation once the live labels are corrected; the prompt policy still lists `po-critic.on-success: dev`.
- Split recommendation: If first-class Postgres runtime/provider support is approved later, create a separate follow-up ticket or epic instead of reopening 06EXB7F6WNWSJJV14EXTPSFDRG.
- Split recommendation: If workflow governance keeps misrouting closure-only epics, create a separate governance ticket for a closure/completion route rather than using this parent epic as an executable handoff.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9372`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b5e27d09758f4879af2e5aeec23c0a59`
- completed-at-utc: `<redacted>-01T04:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/runs/20260501T042813318Z-b5e27d09758f4879af2e5aeec23c0a59.json`