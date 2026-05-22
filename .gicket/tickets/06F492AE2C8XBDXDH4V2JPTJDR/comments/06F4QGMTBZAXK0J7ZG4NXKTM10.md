[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492AE2C8XBDXDH4V2JPTJDR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492AE2C8XBDXDH4V2JPTJDR`.
- Optimistic claim succeeded (`expectedRevision=06F4P4PACHAW0PNC24SCH1JY6G`, `currentRevision=06F4QF6NB8SYYXPW8F2GXKNZBM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492AE2C8XBDXDH4V2JPTJDR-story-harden-ef-model-and-snapshot-drift-preflig' from source '4b7fa1d19b23989a94f6d3ef5463e65e346bda67'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492AE2C8XBDXDH4V2JPTJDR-story-harden-ef-model-and-snapshot-drift-preflig` as `88de1fc63969`.

Open questions / Risiken
- Blocking finding: The contract requires a consumer-supplied `ModelSnapshot` input while also keeping `src/DCoding.Data.DVault` design-package-free, but the repo does not provide direct evidence for the exact EF public type/package boundary that the new API may depend on. Becau...
- Blocking finding: The delivery contract mixes multiple snapshot-input shapes: acceptance criteria say explicit `ModelSnapshot` input, clarification text refers to `snapshot type or instance`, and implementation notes talk about a `ModelSnapshot`-materialized model. That leaves...
- Required PO action: Amend the delivery contract to name one authoritative snapshot input boundary for the additive API: actual `ModelSnapshot` instance, generated snapshot-derived type, or consumer-materialized `IReadOnlyModel`; remove the conflicting alternatives.
- Required PO action: Add repo-local evidence or an explicit package-boundary statement that proves the chosen snapshot input can be supported from `src/DCoding.Data.DVault` without adding `Microsoft.EntityFrameworkCore.Design`. If the intended boundary is really a materialized ...
- Risky assumption: Assumes EF's public `ModelSnapshot` type is usable from the packages already referenced by `src/DCoding.Data.DVault`.
- Risky assumption: Assumes snapshot materialization can remain consumer-owned without forcing design-time-only dependencies into the core package.
- Risky assumption: Assumes provider/profile selection from the configured context is sufficient for all three lanes and will not create false positives when the snapshot was built differently.
- Split recommendation: Keep command aggregation and broad documentation on tickets `06F492BG6BZYYFMBE5WK7CB024` and `06F492BNDPWS9P4EDSV0W7G6VM`; no further split is needed once the snapshot-input contract is clarified.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9401`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `6738aeb108f5443d91e5c68848848905`
- completed-at-utc: `<redacted>-21T18:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492AE2C8XBDXDH4V2JPTJDR/runs/20260521T181825227Z-6738aeb108f5443d91e5c68848848905.json`