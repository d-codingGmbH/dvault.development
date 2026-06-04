[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F98DBN1E6HGXJ6SP3JD137RW`, `currentRevision=06F98DGCHP99MTP7MT7EQ5W12G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source '269a8a4be3a0b37f159b72c7302c5838fe5e8201'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` as `f8e712adcad5`.

Open questions / Risiken
- Blocking finding: This epic owns no residual developer slice; the correct next step is closure-path/status cleanup, not a dev handoff. Under the allowed decision enum, that requires `return_to_po` even though child coverage is otherwise sufficient.
- Required PO action: Move the ticket onto the correct closure/completion path if the workflow cannot represent a closure-only epic on the normal `po-critic -> dev` route.
- Required PO action: Keep any future provider-expansion or physical-naming-override work as new scope on follow-up tickets or epics rather than reopening this parent.
- Risky assumption: That the historical child-to-parent `blocks` relation in `.gicket/relations/68/TW/06F8KZNNS76TD9Z7ESB173FZ68--06F8KZM6KFZ3WC5MY5NC12B0TW--blocks.json` is harmless housekeeping and will not reopen parent routing.
- Split recommendation: No new split is needed for the completed epic.
- Split recommendation: If provider baselines expand beyond SQLite, Oracle, PostgreSQL, SQL Server, and MySQL, or if a consumer-facing physical naming override API is wanted later, track that as a new epic or follow-up ticket instead of reopening 06F8KZM6KFZ3WC5MY5NC12B0TW.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8740`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `9b8c1d81c477463b9025c9addb75e453`
- completed-at-utc: `<redacted>-04T20:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T200441070Z-9b8c1d81c477463b9025c9addb75e453.json`