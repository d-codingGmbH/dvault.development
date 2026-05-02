[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7TE0806E7EY5ZBATHQNK8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7TE0806E7EY5ZBATHQNK8`.
- Optimistic claim succeeded (`expectedRevision=06EYMBSMZE94H61KXWDMQ6B4QC`, `currentRevision=06EYMBYKPQ4CMJ8VFDQW3KXYFG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7TE0806E7EY5ZBATHQNK8-task-add-benchmark-project-for-scenario-comparis' from source '66b93e067ee0ab273d7a345dee40e2f0790bce8b'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7TE0806E7EY5ZBATHQNK8-task-add-benchmark-project-for-scenario-comparis` as `9e899867cd03`.

Open questions / Risiken
- Blocking finding: The order benchmark comparison is under-specified. The delivery contract says current repository evidence already fixes the order baseline, but the observed conventional EF and DVault order scenarios do not share one explicit deterministic event contract or o...
- Blocking finding: Acceptance criterion 3 requires shared business keys, timestamps, record-source values, and scenario shape across benchmark suites. The observed conventional EF order test does not expose the same timestamp/record-source driven history sequence that the DVaul...
- Required PO action: Amend this ticket in place with an explicit order comparison contract, equivalent in precision to the customer-profile contract, naming the exact order/product business keys, timestamps, record sources, dataset size, and operation sequence that both the con...
- Required PO action: State whether the order benchmark includes the unchanged replay case from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:206-226 or excludes it from the measured workload.
- Required PO action: Clarify whether the conventional EF order benchmark should model the broader 2-order/3-line reuse scenario from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-72 or a reduced single-relationship workload that directly matc...
- Risky assumption: Assuming the developer can infer a fair order comparison workload from the current tests without introducing scenario drift.
- Risky assumption: Assuming one shared deterministic benchmark setup can be extracted for the order scenario even though the observed conventional EF and DVault order tests are shaped differently.
- Risky assumption: Assuming relative benchmark results remain meaningful if the two order implementations do not process the same explicit business-event contract.
- Split recommendation: No implementation split is required if Product updates this ticket in place with a precise order comparison contract before developer handoff.
- Split recommendation: If Product cannot express the order comparison contract concisely in this ticket, split a small contract/refinement follow-up first and keep benchmark implementation blocked on that clarification.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9335`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `90a64e61683c44dd94ff4dfc3811405d`
- completed-at-utc: `<redacted>-02T19:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7TE0806E7EY5ZBATHQNK8/runs/20260502T194028857Z-90a64e61683c44dd94ff4dfc3811405d.json`