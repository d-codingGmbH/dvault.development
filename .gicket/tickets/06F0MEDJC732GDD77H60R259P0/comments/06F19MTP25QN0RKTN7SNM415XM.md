[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MEDJC732GDD77H60R259P0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEDJC732GDD77H60R259P0`.
- Optimistic claim succeeded (`expectedRevision=06F19K5TJM1W8HN1H1M55SF8TW`, `currentRevision=06F19KCM1GT3NBME1FR03QCHMR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u' from source 'eac8d9e1a73432bb8b73f8e29ec85b56057d84cb'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u` as `09247cbccc5b`.

Open questions / Risiken
- Blocking finding: The persisted contract is still conditional on runner capability, but this PO-critic run only has configured success -> dev and failure -> po; there is no observed ticket field, label, assignee, or runtime guarantee that success will assign a network/cache-en...
- Blocking finding: Approving for dev through the current success path would risk violating description.md:13-15 and 44-45, because the ticket itself says normal dev is acceptable only with an explicit capable-runner guarantee and otherwise requires release-validation with a com...
- Blocking finding: Required package-validation pass evidence is still absent from the ticket history; searches found requests and failures, not recorded successful dotnet pack plus verify-packages output.
- Required PO action: Add or obtain an explicit ticket-level routing/assignment guarantee that PO-critic success will land on a network/cache-enabled mutable dev runner, or route the ticket to release-validation with a complete NuGet cache before requesting PO-critic approval ag...
- Required PO action: Do not request repository edits merely to work around sandbox network/cache restrictions.
- Risky assumption: Assuming generic dev handoff will automatically use a capable runner is risky because prior dev evidence showed network-restricted/cache-incomplete execution.
- Risky assumption: Assuming release-validation will happen is not enough unless ticket metadata/routing actually sends the ticket there.
- Risky assumption: Treating the current or prior network-restricted/cache-incomplete failures as pass evidence would violate the persisted contract.
- Split recommendation: No split recommended now. Split only if capable-runner output proves a real non-environmental packaging defect that needs separate remediation.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9173`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f2051101ba794f45bfbc3a5160e189cf`
- completed-at-utc: `<redacted>-11T02:17:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEDJC732GDD77H60R259P0/runs/20260511T021734340Z-f2051101ba794f45bfbc3a5160e189cf.json`