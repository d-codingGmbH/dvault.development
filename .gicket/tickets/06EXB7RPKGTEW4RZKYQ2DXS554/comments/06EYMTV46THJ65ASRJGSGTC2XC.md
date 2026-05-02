[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RPKGTEW4RZKYQ2DXS554'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RPKGTEW4RZKYQ2DXS554`.
- Optimistic claim succeeded (`expectedRevision=06EYMM0PBQ72DDSNSVEKKCF6BR`, `currentRevision=06EYMSS9NHXR4NPPRVYNSC30W4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil' from source '40738eb0dbc827c21e4cd6bf90c4d4ffdab0ccf9'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` as `1e613356cea4`.

Open questions / Risiken
- Blocking finding: The ticket is not ready for developer handoff because its own persisted contract says the parent owns no separate implementation slice and must close or advance after PO-critic rather than go to dev.
- Blocking finding: The live ticket metadata has not been reconciled with that contract: it remains `todo` and still carries dev/test blocking and critic-routing labels instead of a terminal or advance state.
- Required PO action: If a developer handoff is actually desired, rewrite the delivery contract so the parent owns a concrete implementation/test slice; otherwise keep the coordination-only contract and close or advance the parent umbrella.
- Risky assumption: Assuming future reviewers will not reopen this umbrella as a third implementation ticket despite the legacy story framing and `area/examples` label.
- Split recommendation: No further split recommended; keep the current umbrella-plus-two-children structure and resolve the parent through workflow/status cleanup rather than new implementation tickets.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8895`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e97723a0171e4d0983218e82cf06b925`
- completed-at-utc: `<redacted>-02T20:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/runs/20260502T203959018Z-e97723a0171e4d0983218e82cf06b925.json`