[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB6ZMBB97J1Z5TBS29QMGPR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6ZMBB97J1Z5TBS29QMGPR`.
- Optimistic claim succeeded (`expectedRevision=06EXD8RXNKJF5K4KBWJ7YJ0P1G`, `currentRevision=06EXDGMRRDPQPANQTPH2X089ZC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup' from source 'd440cc11db2a0c7a69003d6d53bf41df0b285f71'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup` as `8493e26ed6d3`.

Open questions / Risiken
- Blocking finding: The ticket is not ready for developer handoff now because its own contract says implementation must wait for a public startup API and runnable test scaffold, and direct repository evidence shows those prerequisites are still absent.
- Blocking finding: Compatibility depends on public AddDVault and UseDataVault APIs, but only planning/ticket text exists locally; no source implementation or project containing those public members is available to target.
- Required PO action: Keep this ticket out of dev until the prerequisite public API implementation, solution/library project, DVault test project, and standard test command exist, or explicitly rescope the ticket to include that larger setup work.
- Required PO action: Add or verify concrete blocking/dependency relation(s) from this ticket to the prerequisite API implementation and repository test-scaffold work, not only a prose sequencing note.
- Risky assumption: Assuming a developer can implement only the smoke test despite the absent source/test scaffold would likely turn this into hidden architecture and repository setup work.
- Risky assumption: Assuming the API-shape planning document is sufficient compatibility evidence would violate the requirement to verify public API/type availability from source when implementation depends on it.
- Split recommendation: No split is needed for the smoke-test body after prerequisites exist.
- Split recommendation: If no existing ticket owns the solution/library/test scaffold and public API implementation, create or link those prerequisite tickets before sending this task to dev.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.5490`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `64387660c1314a22a181a383c6bf164c`
- completed-at-utc: `<redacted>-29T01:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/runs/20260429T010645472Z-64387660c1314a22a181a383c6bf164c.json`