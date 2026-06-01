[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F7XZW80PRGN6QBMGCJVEKM3C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7XZW80PRGN6QBMGCJVEKM3C`.
- Optimistic claim succeeded (`expectedRevision=06F81TJ2R37ZVNW2R295WQHEXG`, `currentRevision=06F81TWFPJKTNXZX23KRFMJAZ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F7XZW80PRGN6QBMGCJVEKM3C-epic-async-streaming-save-and-ef-core-safety' from source '9ad37959be627015ab063f80eab158ee7b67a13d'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F7XZW80PRGN6QBMGCJVEKM3C-epic-async-streaming-save-and-ef-core-safety` as `5a561d6d0571`.

Open questions / Risiken
- Blocking finding: Stale conflicting scope remains in the same description. .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:81-84 still says Scope In includes analyzer/runtime diagnostics for EF Core model-cache, compiled-model, and DbContext pooling risks, which conf...
- Required PO action: Reclassify this epic to closure-only, done, or no-work-required, or explicitly restate a concrete remaining developer deliverable that is not already landed and reopen only the ticket or tickets that actually carry that work.
- Required PO action: Remove, archive, or explicitly neutralize the stale legacy draft section in .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:78-92 so the ticket no longer appears to promise new model-cache or pooling diagnostics.
- Risky assumption: Assuming a developer should act on this epic despite git evidence showing no repo delta beyond ticket metadata on the owner branch.
- Risky assumption: Assuming readers will ignore the legacy draft's broader analyzer/runtime-diagnostics wording because the contract block says it is non-authoritative.
- Split recommendation: Keep any future provider-native async write or provider-specific async execution claims in a separate follow-on ticket rather than reopening this epic.
- Split recommendation: Keep any future model-cache, compiled-model, or pooling analyzer/runtime guardrails in a separate follow-on ticket if guidance-only EF safety proves insufficient.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8565`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `4ca372ba598b4cdfbd66afe79079c66f`
- completed-at-utc: `<redacted>-01T02:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/runs/20260601T021149399Z-4ca372ba598b4cdfbd66afe79079c66f.json`