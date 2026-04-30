[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7FPZRCFC33RF2M5SXZTK4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FPZRCFC33RF2M5SXZTK4`.
- Optimistic claim succeeded (`expectedRevision=06EXYCZC8E4WWQJGP4ZHKD92D8`, `currentRevision=06EXYG6HMJ32X6NNY64BTFZE3R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7FPZRCFC33RF2M5SXZTK4-task-add-modelbuilder-extension-for-dvault-conve' from source '7a41ff442b1c2f2744f01339770a45bbdd2e112f'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7FPZRCFC33RF2M5SXZTK4-task-add-modelbuilder-extension-for-dvault-conve` as `76d187ad921d`.

Open questions / Risiken
- Blocking finding: The contract requires the EF `ModelBuilder` extension to `apply/record` the default DVault conventions, but the repository currently has no EF Core package reference, no EF-model metadata contract, and no existing test/assertion surface that makes that behavi...
- Required PO action: Amend the delivery contract to name the exact EF-model artifact this ticket must create or mutate to prove `ModelBuilder.UseDataVault()` is wired to `DataVaultConventions.Default` (for example, specific DVault-owned model annotation key/value(s) or another ...
- Required PO action: State explicitly whether this ticket does or does not perform EF model metadata translation for hubs, links, satellites, keys, indexes, and technical columns, so its boundary is clean against blocked ticket `06EXB7FYXNBPMH8VGQCGP2R41R`.
- Required PO action: Update the acceptance criteria and test expectations to match that chosen contract, so dev does not have to guess between a near-no-op fluent wrapper and prematurely implementing downstream metadata work.
- Risky assumption: Assumes the EF Core package chosen for the `net10.0` baseline exposes a public, provider-neutral `ModelBuilder` construction path suitable for focused unit tests; the repo currently has no EF Core reference to verify that locally.
- Risky assumption: Assumes a root-namespace `ModelBuilder.UseDataVault()` overload will remain unambiguous beside `DCoding.Data.DVault.Modeling.DataVaultModelBuilderExtensions.UseDataVault()` purely through parameter-type distinction.
- Split recommendation: Keep the existing split: this ticket for the public EF `ModelBuilder` entry point, and `06EXB7FYXNBPMH8VGQCGP2R41R` for richer EF model metadata translation. The needed change is scope clarification, not another child ticket.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8631`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `0f234d8f450f4abab7169f899c42c0f9`
- completed-at-utc: `<redacted>-30T16:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FPZRCFC33RF2M5SXZTK4/runs/20260430T164325109Z-0f234d8f450f4abab7169f899c42c0f9.json`