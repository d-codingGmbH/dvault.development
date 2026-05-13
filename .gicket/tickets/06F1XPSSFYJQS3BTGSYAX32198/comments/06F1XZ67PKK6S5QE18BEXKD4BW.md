[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XPSSFYJQS3BTGSYAX32198'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPSSFYJQS3BTGSYAX32198`.
- Optimistic claim succeeded (`expectedRevision=06F1XX6BG0KV0V2ME7S2BFSF4G`, `currentRevision=06F1XXFAE4F55M8ZBNZWQ72MSM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPSSFYJQS3BTGSYAX32198-task-implement-diagnostic-catalog-infrastructure' from source '11070e49750931bcb90506836b8527519eb6ad21'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XPSSFYJQS3BTGSYAX32198-task-implement-diagnostic-catalog-infrastructure` as `ab21a96ee34e`.

Open questions / Risiken
- Blocking finding: The acceptance criteria require documentation coverage on each catalog entry, but neither the ticket contract nor repository sources define what fields or quality bar satisfy that coverage. With only generic XML-doc guidance in `docs/plans/shared-implementati...
- Blocking finding: The contract cites `DMV1002` and `DMV1801` as examples, but direct source inspection shows 18 currently emitted importer/projection-path codes across `DataVaultModelArtifactParser.cs` and `DataVaultModelImportResult.cs` (`DMV1001`-`DMV1801`). The ticket does ...
- Required PO action: Define the minimum documentation contract for one catalog entry in ticket language: required fields, where they live, and what the new tests must enforce.
- Required PO action: State the exact v1 seed rule for this ticket, for example 'catalog every diagnostic currently emitted by `DataVaultModelArtifactParser` and `DataVaultModelImportResult`' or provide an explicit smaller in-scope code list.
- Risky assumption: Assumes the first-slice catalog only needs current error diagnostics; the reviewed model-artifact path evidence did not show warning/info severities, so a broader catalog shape would otherwise be guessed.
- Split recommendation: No split needed after the two contract gaps above are resolved; the implementation slice itself remains appropriately small.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9480`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `9365f6bc29164dc7b72d6743bfee045b`
- completed-at-utc: `<redacted>-13T01:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPSSFYJQS3BTGSYAX32198/runs/20260513T013902448Z-9365f6bc29164dc7b72d6743bfee045b.json`