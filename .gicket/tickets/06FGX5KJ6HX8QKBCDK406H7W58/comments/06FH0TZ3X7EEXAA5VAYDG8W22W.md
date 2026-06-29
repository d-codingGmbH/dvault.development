[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FGX5KJ6HX8QKBCDK406H7W58'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5KJ6HX8QKBCDK406H7W58`.
- Optimistic claim succeeded (`expectedRevision=06FH0S7K60RKHXGJW6TZ70B1A0`, `currentRevision=06FH0SHKXK0FW180PZCKQ2KYGG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FGX5KJ6HX8QKBCDK406H7W58-task-update-analyzer-compatibility-documentation' from source 'd863a3e51f8ced2b6844fdc17eabb510f8cf0ebd'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FGX5KJ6HX8QKBCDK406H7W58-task-update-analyzer-compatibility-documentation` as `f128fc4ad304`.

Open questions / Risiken
- Blocking finding: The ticket requires touched cross-references to stop carrying stale `v0.49.0` labels, but it does not say whether this ticket must also create or update `docs/releases/v0.50.0.md` and `CHANGELOG.md`, or whether those links should remain deferred to the separa...
- Blocking finding: A separate release-note ticket already exists and is still `todo` / `needs-po`, so the current ticket cannot safely assume the target `v0.50.0` release artifact is available; that dependency needs explicit ownership in this ticket.
- Required PO action: Decide ownership for `v0.50.0` release-note/changelog alignment: either add `CHANGELOG.md` and `docs/releases/v0.50.0.md` to this ticket's in-scope surfaces, or explicitly state that README/package-compatibility/manual-publication must not retarget those li...
- Required PO action: Add one acceptance criterion that defines the exact intended target for README/package-compatibility release-note references during this ticket.
- Required PO action: If the work stays split, add an explicit dependency note naming ticket `06FGX6DSX1SRQ1Y22DP53629S8` so the developer is not forced to guess the correct cross-reference behavior.
- Risky assumption: Assumes a `v0.50.0` release-note/changelog artifact either already exists or can be referenced without clarifying which ticket owns it.
- Risky assumption: Assumes a developer will infer the correct fallback behavior for stale `v0.49.0` links without breaking scope boundaries.
- Split recommendation: Keep the release-note/package-validation work separate only if this ticket explicitly states how versioned README/package-compatibility links should behave before ticket `06FGX6DSX1SRQ1Y22DP53629S8` lands.
- Split recommendation: If PO wants the developer to update versioned release-note links now, merge that ownership into this ticket instead of leaving it implicit.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8992`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `a2315f33ff46410a8133eafc38c49b20`
- completed-at-utc: `<redacted>-28T22:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5KJ6HX8QKBCDK406H7W58/runs/20260628T224853988Z-a2315f33ff46410a8133eafc38c49b20.json`