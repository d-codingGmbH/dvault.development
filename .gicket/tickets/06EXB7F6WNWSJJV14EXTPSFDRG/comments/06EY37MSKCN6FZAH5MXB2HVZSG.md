[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7F6WNWSJJV14EXTPSFDRG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Optimistic claim succeeded (`expectedRevision=06EY36B9ZPAY0T68AZBEGM162G`, `currentRevision=06EY36F07Q07VMCXXSMMQC195W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc' from source 'abaf12ed62614210af15ec26f8102332ddc40f41'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc` as `da57261484fe`.

Open questions / Risiken
- Blocking finding: The parent epic does not expose a live developer-owned slice anymore. Its own contract says the bounded delivery path is the four listed child tickets, and direct ticket reads show all four are already done.
- Blocking finding: Approving this ticket would hand a tracking/orchestration epic to dev even though the repo already contains the referenced EF surface, explicit save surface, SQLite tests, and Postgres opt-in contract. That is a workflow/status problem, not an implementation-...
- Required PO action: Reframe 06EXB7F6WNWSJJV14EXTPSFDRG explicitly as a tracking/closure item or move it onto the appropriate completion path instead of sending it to dev.
- Required PO action: If any implementation is still intended, identify the specific still-open child ticket or create one; do not hand the current parent epic to dev while the listed child delivery path is already complete.
- Required PO action: Align ticket-level workflow state with that decision by updating the parent epic status/labels/comment handoff so automation does not dispatch a completed umbrella back to development.
- Split recommendation: No additional split is needed for the current scope; the four-child decomposition on 06EXB7F6WNWSJJV14EXTPSFDRG remains sufficient.
- Split recommendation: If residual work exists after PO cleanup, reopen or create a concrete child ticket instead of treating the parent epic as a generic dev handoff.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `95036`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0256`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `a78ba6a44e9a451884d6b37a3507d3a3`
- completed-at-utc: `<redacted>-01T03:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/runs/20260501T033920203Z-a78ba6a44e9a451884d6b37a3507d3a3.json`