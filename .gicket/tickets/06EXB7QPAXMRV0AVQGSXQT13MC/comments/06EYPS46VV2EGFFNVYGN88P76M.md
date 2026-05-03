[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7QPAXMRV0AVQGSXQT13MC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7QPAXMRV0AVQGSXQT13MC`.
- Optimistic claim succeeded (`expectedRevision=06EYPQQ1BR6QGHAFDJQQRF0A5G`, `currentRevision=06EYPQV1E7MB5VPWF496S2RZQG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7QPAXMRV0AVQGSXQT13MC-epic-examples-documentation-and-benchmarks' from source '0abcfead118f2429f82d6c31e0fef59f8937a062'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7QPAXMRV0AVQGSXQT13MC-epic-examples-documentation-and-benchmarks` as `82c0191c4d5b`.

Open questions / Risiken
- Blocking finding: The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.
- Required PO action: Resolve the tracking-epic closure audit findings before this parent ticket can be closed.
- Risky assumption: Assuming the contract-level resolution is enough even though `README.md:137` still advertises `examples/` as future runnable examples and `examples/.gitkeep` is the only file under that directory.
- Risky assumption: Assuming downstream workflow consumers respect the coordination-only boundary and do not reopen new epic-level implementation work after approval.
- Split recommendation: No additional split is recommended at the epic level; the existing four child stories already cover docs, example scenarios, and benchmarks.
- Split recommendation: Keep any future standalone `examples/` tree, provider-specific documentation, or broader benchmark publication as follow-up tickets/epics rather than enlarging this MVP epic.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9195`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `591a3755ea3243869e714fca128c8b72`
- completed-at-utc: `<redacted>-03T01:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7QPAXMRV0AVQGSXQT13MC/runs/20260503T011206406Z-591a3755ea3243869e714fca128c8b72.json`