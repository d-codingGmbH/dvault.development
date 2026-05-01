[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7F6WNWSJJV14EXTPSFDRG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Optimistic claim succeeded (`expectedRevision=06EY3C7TKKF4967Y5AC8F956TW`, `currentRevision=06EY3CBD488PK89DNEXM7DKBWR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc' from source '44379985fcc9badcaef360d4a22689d32b8c87d6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc` as `5d539af59db5`.

Open questions / Risiken
- Blocking finding: There is no implementation delta left to hand to a developer on this branch: HEAD is only the PO-critic claim commit, there are no non-`.gicket` changes relative to `develop`, and all four child stories named as the bounded delivery path are already `done`.
- Required PO action: Make the post-critic route explicit at ticket level for this closure epic so automation does not send it back to a developer role with no remaining implementation slice.
- Split recommendation: No additional split is recommended; the four existing `parentOf` child stories already cover the bounded delivery path.
- Split recommendation: If workflow-governance cleanup is needed for closure-only epics, track that under a separate follow-up ticket instead of reopening `06EXB7F6WNWSJJV14EXTPSFDRG`.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9476`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `5a68c015d0dd4922a1591eebdd4be477`
- completed-at-utc: `<redacted>-01T04:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/runs/20260501T040610237Z-5a68c015d0dd4922a1591eebdd4be477.json`