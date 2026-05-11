[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MEDJC732GDD77H60R259P0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEDJC732GDD77H60R259P0`.
- Optimistic claim succeeded (`expectedRevision=06F19GSMGYY97EXPJF6CXCG0X8`, `currentRevision=06F19H0CWV9NJD8C71HFQK5H2C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u' from source 'a7e88a54f281cc4524055bb17a5d36bdb9084160'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u` as `a2baea1e601d`.

Open questions / Risiken
- Blocking finding: The ticket is not ready for unconditional developer handoff through the current PO-critic success path. The contract permits dev only if the next dev runner is network/cache-enabled and mutable, but the latest dev evidence shows the runner was network-restric...
- Blocking finding: The PO contract also names release-validation as the fallback lane, but the current PO-critic role path provided to this run has success -> dev and failure -> po. Without a ticket-level routing change or explicit capable-dev assignment, approving would likely...
- Required PO action: Keep the ticket out of tester until successful dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh output is recorded from the capable lane.
- Required PO action: Do not request docs, product-code, package metadata, provider behavior, or release automation edits merely to work around the sandbox limitation.
- Risky assumption: Assuming a normal dev handoff will automatically land on a capable runner is risky because the immediately preceding dev run did not.
- Risky assumption: Assuming release-validation is available is not enough unless the ticket metadata/routing actually sends this ticket there.
- Risky assumption: Treating the current no-network/cache-incomplete restore failure as package-validation evidence would violate the persisted contract.
- Split recommendation: No split recommended now; split only if capable-runner output proves a real non-environmental packaging defect that needs separate remediation.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9405`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c0f2373d6303426c881c79e2e88840f1`
- completed-at-utc: `<redacted>-11T02:07:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEDJC732GDD77H60R259P0/runs/20260511T020747122Z-c0f2373d6303426c881c79e2e88840f1.json`