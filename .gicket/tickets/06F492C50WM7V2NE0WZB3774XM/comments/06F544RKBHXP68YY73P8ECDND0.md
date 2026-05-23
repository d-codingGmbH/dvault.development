[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F53V0N1SQQQ16RJM1RJ0W2VR`, `currentRevision=06F5435S9BAG9GSX5N19FQFHNM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source 'ebfc09d558bb514f59b1138be9b7a15d69b869d3'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `aa8d9c60b475`.

Open questions / Risiken
- Blocking finding: The persisted delivery contract is factually stale against current source: it says no existing public performance-stage record type is evidenced, but the repo already exposes and snapshots those ReadShape performance records.
- Blocking finding: The ticket does not identify a net-new delta from already shipped and tested behavior. Current source, release notes, public API snapshot, and tests already cover the same ReadShape performance/index/provider diagnostics the story asks developers to add.
- Required PO action: Rewrite the story as a concrete delta from the current ReadShape baseline, naming the exact missing fields or behaviors that are not already present in src/DCoding.Data.DVault/DataVaultDiagnostics.cs and its existing tests/docs.
- Required PO action: If no source-backed gap remains, close or reclassify the ticket as duplicate/obsolete, or convert it into a narrower follow-up with a genuinely new outcome.
- Required PO action: Update acceptance criteria and definition of done so they do not treat already-shipped ReadShape, support-bundle, public API snapshot, and registry-equivalence behavior as new development work.
- Risky assumption: It assumes the branch lacks performance-stage ReadShape records even though DataVaultDiagnostics.cs, the public API snapshot, and tests already expose them.
- Risky assumption: It assumes support-bundle and API snapshot work is still future scope even though DataVaultSupportBundleExporter.ExportJson(...) and the approved API snapshot already cover the existing ReadShape model.
- Risky assumption: It assumes registry-backed latest-satellite and bridge equivalence still needs to be introduced even though existing unit and integration tests already assert it.
- Split recommendation: No split until PO identifies a real net-new delta. If only documentation wording or a separate telemetry/summary concept remains, ticket that independently from core read-shape diagnostics.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8044`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `da36ec9bcbbe4d78af20a761db6ad258`
- completed-at-utc: `<redacted>-22T23:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260522T234402392Z-da36ec9bcbbe4d78af20a761db6ad258.json`