[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RPKGTEW4RZKYQ2DXS554'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RPKGTEW4RZKYQ2DXS554`.
- Optimistic claim succeeded (`expectedRevision=06EYM98MB8YVN7H1WS1Z85K10C`, `currentRevision=06EYM9DFK7DHS6SHA00FW3NTA8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil' from source '94c31711497311b40c6280ed708c6653255646ae'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` as `d7a4fd088a28`.

Open questions / Risiken
- Blocking finding: Branch-history evidence shows no parent-owned repository implementation delta beyond ticket metadata. Handing this parent story to a developer would duplicate or no-op work unless PO defines a new remaining slice.
- Required PO action: Correct the parent ticket workflow so it matches the observed state: either close/advance `06EXB7RPKGTEW4RZKYQ2DXS554` as an umbrella story or restate a concrete remaining parent-owned slice before sending it to `dev`.
- Required PO action: If the parent must stay open, add explicit ticket-level scope describing what work remains that is not already owned and completed by child tickets `06EXB7RYFJ3YQDB1E4QHPP8034` and `06EXB7S6DB97GVVTS2GGZ3CCX8`.
- Split recommendation: No further split is needed.
- Split recommendation: Prefer a PO workflow/status correction on the existing parent story over creating another developer slice for already integrated work.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9210`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `be4ac38b15ca4b36b468ce2351226968`
- completed-at-utc: `<redacted>-02T19:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/runs/20260502T192815944Z-be4ac38b15ca4b36b468ce2351226968.json`