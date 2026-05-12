[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MEGPPETJD4ZDEN5ESGR7JW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEGPPETJD4ZDEN5ESGR7JW`.
- Optimistic claim succeeded (`expectedRevision=06F1H2H28N2A5VMAWCB3TE8TA0`, `currentRevision=06F1H2Z0XHDXV0C7XB46G1YJYW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers' from source 'a39f8e93edcef04edd04069d13d9a4e920f4a910'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers` as `dc1073a73f97`.

Open questions / Risiken
- Blocking finding: The delivery contract is internally inconsistent for hierarchy bridge depth. Acceptance criterion .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/description.md:40 makes maximum depth sound optional, but the actual public request type requires a bounded maximumDep...
- Required PO action: Rewrite the hierarchy bridge acceptance criterion so it explicitly matches intended behavior: either require non-negative bounded maximumDepth for hierarchy requests or change the code/tests in a separate implementation ticket if optional depth was the true...
- Required PO action: Reconcile the parent story workflow with observed delivery state. If this is an umbrella or completion story, update status and routing expectations instead of sending it to dev for new implementation work.
- Required PO action: Clarify release-note or changelog ownership if this story still owns scope-consistency work; docs/releases/v0.6.0.md:46-47 still describes PIT-backed reads and bridge helpers as not delivered.
- Risky assumption: Assuming docs/releases/v0.6.0.md is historical release context only; if it is still a live acceptance reference for this story, it conflicts with the current source and test state.
- Risky assumption: Assuming there is no remaining developer-owned work outside the four done child tickets; the repository and branch history do not show any new code delta on this parent story branch.
- Split recommendation: No additional split recommended; the story is already decomposed into four child tickets, and status or routing cleanup is the remaining need.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9347`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `ba3b7d4f2009463db544ec45c5b51cf1`
- completed-at-utc: `<redacted>-11T19:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/runs/20260511T194532873Z-ba3b7d4f2009463db544ec45c5b51cf1.json`