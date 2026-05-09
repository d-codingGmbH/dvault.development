[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MEANEV00QSYHMSGWX1X0R4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEANEV00QSYHMSGWX1X0R4`.
- Optimistic claim succeeded (`expectedRevision=06F0XGY999RTG33HZ63TYR4E54`, `currentRevision=06F0XH79KA8ZCS3FM2YM3GK02R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEANEV00QSYHMSGWX1X0R4-story-introduce-data-vault-model-registry' from source '97be0094f4189fff928c9be0795811c10545e1a7'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MEANEV00QSYHMSGWX1X0R4-story-introduce-data-vault-model-registry` as `bd920f127e06`.

Open questions / Risiken
- Blocking finding: The contract says code-first declarations normalize to `DataVaultMetadataModel` and frames that as already-established baseline behavior (`description.md:22,34,47`), but direct public source evidence does not expose a caller-usable code-first-to-`DataVaultMet...
- Blocking finding: The contract uses `current point-in-time metadata` (`description.md:13,33`) without naming which public lookup family is in scope, but the registry publicly exposes two separate families: `PointInTimeTables`/`TryGetPointInTimeTable` and `Pits`/`TryGetPit` in ...
- Required PO action: Rewrite the code-first compatibility wording in the delivery contract so it matches the public source surface: either limit the claim to internal normalization during EF translation, or explicitly require a new public export/registration path if that is the...
- Required PO action: Replace `current point-in-time metadata` with the concrete in-scope public type/API names: `DataVaultPointInTimeMetadata`/`TryGetPointInTimeTable`, `DataVaultPitMetadata`/`TryGetPit`, or both.
- Required PO action: After the two scope statements above are corrected, recheck the acceptance criteria and Definition of Done so dev/test do not infer unplanned public API work from the parent story.
- Risky assumption: Assuming internal code-first normalization implies an existing public code-first-to-registry contract; direct source evidence does not currently support that.
- Risky assumption: Assuming `current point-in-time metadata` is self-explanatory even though the public registry surface splits point-in-time tables and PIT metadata into different lookup APIs.
- Split recommendation: Keep the current three-child split under `06F0MEANEV00QSYHMSGWX1X0R4`; repository relations and child statuses support it.
- Split recommendation: Keep broader parity/regression breadth on `06F0MEAD1BAA5QEVM3F9QJA38G` and do not let parent-story wording pull that scope back in.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9221`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `5c101a235904482b868c45f78dc822ad`
- completed-at-utc: `<redacted>-09T22:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEANEV00QSYHMSGWX1X0R4/runs/20260509T221127412Z-5c101a235904482b868c45f78dc822ad.json`