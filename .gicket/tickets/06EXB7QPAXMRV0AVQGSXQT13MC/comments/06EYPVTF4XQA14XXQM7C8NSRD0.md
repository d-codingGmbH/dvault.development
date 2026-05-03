[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7QPAXMRV0AVQGSXQT13MC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7QPAXMRV0AVQGSXQT13MC`.
- Optimistic claim succeeded (`expectedRevision=06EYPTB0E2HDTM0D274G51YJFC`, `currentRevision=06EYPTF5TCHCZBTSYM8ZM16AEG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7QPAXMRV0AVQGSXQT13MC-epic-examples-documentation-and-benchmarks' from source '8bf9c6eaf0cd8c025cc1b4473568afe1cb316e08'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7QPAXMRV0AVQGSXQT13MC-epic-examples-documentation-and-benchmarks` as `abe9eea61211`.

Open questions / Risiken
- Required PO action: Keep the parent epic as coordination-only; if any residual scope remains, place it on the existing child tickets or a new follow-up ticket rather than reopening parent-owned implementation work.
- Risky assumption: This contract assumes the workflow can close or advance a closure-only epic without forcing a `dev` phase; the current runtime success path (`po-critic` -> `dev`) conflicts with the no-parent-implementation contract.
- Split recommendation: No additional split is needed at the epic level; keep future standalone `examples/`, provider-specific docs, or benchmark-publication work as separate follow-up tickets or epics.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9054`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `44d09cfd273e4b43a54d78f6c6d1b3ef`
- completed-at-utc: `<redacted>-03T01:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7QPAXMRV0AVQGSXQT13MC/runs/20260503T012353107Z-44d09cfd273e4b43a54d78f6c6d1b3ef.json`