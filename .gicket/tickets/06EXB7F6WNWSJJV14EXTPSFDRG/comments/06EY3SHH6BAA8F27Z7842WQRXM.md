[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7F6WNWSJJV14EXTPSFDRG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Optimistic claim succeeded (`expectedRevision=06EY3RER18KPBST4CVXETM56EM`, `currentRevision=06EY3RJDC3MSPGX13S1DSS95Q4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc' from source 'd5f071ace490057874f3e2a8c742dc6557c7b264'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc` as `bcae15a7c00b`.

Open questions / Risiken
- Blocking finding: The latest PO pass did not actually persist the required routing cleanup before re-entering PO-critic; `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/comments/06EY3RCKPTSZ7ABZABF1SYBJ30.md` defers the label cleanup to downstream automation instead of leaving the...
- Required PO action: Persist an explicit closure-compatible status/label end state for this parent epic instead of relying on a later developer-oriented handoff to clean up the routing metadata.
- Risky assumption: That runtime will clear stale blocked routing labels later even though the latest persisted `ticket.json` still carries them.
- Risky assumption: That a PO-critic success path ending in `dev` is safe for a closure-only parent epic with no remaining developer-owned work.
- Split recommendation: No additional split is needed for the parent epic itself.
- Split recommendation: If workflow-governance cleanup or first-class Postgres runtime/provider support is approved later, create separate follow-up tickets instead of reopening `06EXB7F6WNWSJJV14EXTPSFDRG`.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7162`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `610385a9ec014df6ad1e0fb99a421158`
- completed-at-utc: `<redacted>-01T04:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/runs/20260501T045732075Z-610385a9ec014df6ad1e0fb99a421158.json`