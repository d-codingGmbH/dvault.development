[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FH8QAVJFXANVQFXGPYVAFXSR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8QAVJFXANVQFXGPYVAFXSR`.
- Optimistic claim succeeded (`expectedRevision=06FHC67NB40XPCXSBZS91FGS30`, `currentRevision=06FHC6JPDB465C7EBJH3TDGP2W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp' from source '407586bd0e19211c45699a1272d4e24b1040b733'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp` as `c120d5653f75`.

Open questions / Risiken
- Blocking finding: The parent mixes an already-landed 8.50.0/10.50.0 repository baseline with future 8.51.0/10.51.0 release wording, so approving it for developer handoff would leave no direct developer scope on this ticket while the real remaining scope sits on an unrefined se...
- Required PO action: Either narrow this parent contract to the already-landed 8.50.0/10.50.0 analyzer-host baseline and remove the 8.51.0/10.51.0 landing condition from this ticket, or keep that future roll-forward in scope and wait until ticket 06FH8RP1SBVZ7K3K48ERGZSMQC is re...
- Required PO action: Refine ticket 06FH8RP1SBVZ7K3K48ERGZSMQC into a delivery-contract-quality follow-up if it remains the intended carrier for the 8.51.0/10.51.0 release-note, changelog, install-guidance, and package-validation updates.
- Required PO action: Clean up or explicitly defer the stale child-to-parent blocks/relation noise before re-submitting this tracking parent so the live relation graph matches the done child state.
- Risky assumption: Assumes the stale incoming child relation state will not mislead automation or closure logic.
- Risky assumption: Assumes no further PO clarification is needed even though the repository evidence still stops at the 8.50.0/10.50.0 baseline.
- Split recommendation: No new implementation split is needed; the current child split is adequate. The remaining issue is scope hygiene between this tracking parent and follow-up ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8324`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `57c7640575a94bff9a7dcb26338a301e`
- completed-at-utc: `<redacted>-30T01:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/runs/20260630T012545066Z-57c7640575a94bff9a7dcb26338a301e.json`