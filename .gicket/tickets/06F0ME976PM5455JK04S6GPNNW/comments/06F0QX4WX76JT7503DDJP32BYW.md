[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0ME976PM5455JK04S6GPNNW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0ME976PM5455JK04S6GPNNW`.
- Optimistic claim succeeded (`expectedRevision=06F0QP95NP9G8ZQW5NKGBY9BYW`, `currentRevision=06F0QVT42T77KKFMR5HQE6GZC4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co' from source 'f321de1153b9de4bf06e5f1c907935f9dc6033d9'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co` as `96496824acbf`.

Open questions / Risiken
- Blocking finding: The persisted parent Definition of Done requires the child implementation tickets to reference this contract as their boundary, but the current child ticket content does not reference the parent ticket id, parent story id, or `docs/plans/06F0ME976PM5455JK04S6...
- Blocking finding: The hub/satellite child ticket does not currently carry the parent contract's reserved `DrivingKey(...)` / multi-active opt-in scope, and the parity child also does not mention validating that covered multi-active shape. That leaves part of the approved paren...
- Required PO action: Update the three child implementation tickets so they explicitly reference ticket `06F0ME976PM5455JK04S6GPNNW` and/or `docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md` as the authoritative boundary.
- Required PO action: Amend child scope/acceptance text so the hub/satellite implementation ticket explicitly includes `DrivingKey(...)` multi-active opt-in selector capture and validation, and the parity ticket explicitly includes parity coverage for that covered shape.
- Risky assumption: Assuming relation files alone satisfy the parent DoD's requirement that child tickets `reference this contract as their boundary`.
- Risky assumption: Assuming developers will infer multi-active `DrivingKey(...)` scope from the parent design note even though the assigned child ticket text does not currently mention it.
- Risky assumption: Assuming parity coverage will automatically include all covered Code-First shapes without the parity ticket naming those shapes explicitly.
- Split recommendation: No additional split is needed. Keep the existing three-child decomposition, but return this ticket to PO until the child tickets are updated to carry the parent contract boundary explicitly.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9119`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7399d8a682ee4fe9bb42f777621d14d7`
- completed-at-utc: `<redacted>-09T08:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0ME976PM5455JK04S6GPNNW/runs/20260509T085720086Z-7399d8a682ee4fe9bb42f777621d14d7.json`