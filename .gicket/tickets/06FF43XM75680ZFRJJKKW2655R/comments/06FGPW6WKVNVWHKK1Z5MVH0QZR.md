[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FF43XM75680ZFRJJKKW2655R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43XM75680ZFRJJKKW2655R`.
- Optimistic claim succeeded (`expectedRevision=06FGPSJYV8N43QG726JHW8VAYG`, `currentRevision=06FGPSXGB9JAENCWT2716TCNS4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43XM75680ZFRJJKKW2655R-story-define-repeated-same-hub-generator-parity' from source 'c8d9664199a0b4ddc445174d40fdd7c04c54537b'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FF43XM75680ZFRJJKKW2655R-story-define-repeated-same-hub-generator-parity` as `5699b92296fb`.

Open questions / Risiken
- Blocking finding: The ticket's own persisted contract is factually inconsistent about description changes. The branch diff, handoff commit, and PO run report all show that `description.md` was updated in this pass, but the current contract still claims no description update or...
- Blocking finding: The persisted follow-up and risk text is stale after that rewrite. The live `description.md` already contains the aggregate contract block, yet the ticket still asks whether a later pass should write that contract into the ticket body and still describes the ...
- Required PO action: Rewrite the delivery-contract history text so it matches the persisted ticket state: the description was updated in this PO pass and the aggregate contract is already in the ticket body.
- Required PO action: Remove or restate the stale follow-up and risk wording so the handoff surface no longer contains mutually exclusive statements about the same description update.
- Risky assumption: This review assumes the unreadable duplicate relation `06FF43Z97VRFNMVKPZ13CKPN1C` is historical noise only, because the real replacement child `06FF43YPV3WYDQHEGZSW4T296C` is locally present and `done`.
- Split recommendation: No additional split recommended once the ticket text is reconciled; the existing child-ticket breakdown already covers the bounded work.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9363`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e5b0d43688d542ce955cf56427840714`
- completed-at-utc: `<redacted>-27T23:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43XM75680ZFRJJKKW2655R/runs/20260627T233613723Z-e5b0d43688d542ce955cf56427840714.json`