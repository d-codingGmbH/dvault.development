[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZP0VKMXGE0JXPZRD1RQDG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZP0VKMXGE0JXPZRD1RQDG`.
- Optimistic claim succeeded (`expectedRevision=06F9EG26M0XX3T2NYPRX80A3NG`, `currentRevision=06F9EG98P6J0W7CT0MDHCG9N7M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' from source 'de31cd3100f24bacd77ef8160db5c128e0bb3c7e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag` as `550b801565e4`.

Open questions / Risiken
- Blocking finding: The epic Definition of Done is not met on documentation surfaces: the required current release-note surface does not exist (`docs/releases/v0.30.0.md` is missing), README still points to v0.29.0 as the current baseline, and the required freshness/troubleshoot...
- Blocking finding: Tracking coverage is inconsistent: child task `06F8KZQAWZ7QRGB68KB21C9B0R` is already `done`, but its own acceptance criteria and DoD are not satisfied by current repository evidence, so remaining work is no longer represented by an active delivery ticket.
- Required PO action: Reopen `06F8KZQAWZ7QRGB68KB21C9B0R` or create one bounded replacement documentation ticket for the still-missing repo work: README freshness/recovery wording, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` troubleshooting guidance, and a new `...
- Required PO action: Update the epic's tracking state so remaining documentation work is represented by an active child/follow-up instead of leaving the epic with all children marked done.
- Required PO action: Before resubmitting this epic for closure-style review, provide landed repo evidence that the documentation carrier's acceptance criteria are satisfied and, if closure is intended, reconcile or explicitly supersede the stale incoming `blocks` relation from ...
- Risky assumption: Assuming child `06F8KZQAWZ7QRGB68KB21C9B0R` is satisfied because `ticket.json` says `done`, even though its own contract and current repo state still show the three documentation gaps as open.
- Risky assumption: Assuming the stale relation file `.gicket/relations/0R/DG/06F8KZQAWZ7QRGB68KB21C9B0R--06F8KZP0VKMXGE0JXPZRD1RQDG--blocks.json` is harmless housekeeping because epic `ticket.json` says `is-blocked=false` and comment `.gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/...
- Split recommendation: No further split beyond a single bounded documentation carrier. Prefer reopening `06F8KZQAWZ7QRGB68KB21C9B0R`; if that is not acceptable, create one replacement task and keep the epic tracking-only.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8909`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b5a32ee4c0c0427e95e96683245845e8`
- completed-at-utc: `<redacted>-05T10:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/runs/20260605T101827908Z-b5a32ee4c0c0427e95e96683245845e8.json`