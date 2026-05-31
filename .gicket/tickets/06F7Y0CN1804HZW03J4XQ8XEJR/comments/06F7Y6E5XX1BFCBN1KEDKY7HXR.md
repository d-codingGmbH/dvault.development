[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F7Y0CN1804HZW03J4XQ8XEJR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0CN1804HZW03J4XQ8XEJR`.
- Optimistic claim succeeded (`expectedRevision=06F7Y4KPP1Y72XT9BEXJACCB68`, `currentRevision=06F7Y4WCK3YNB79MBGM8M61Q6C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F7Y0CN1804HZW03J4XQ8XEJR-story-define-async-streaming-save-contract-and-b' from source 'c12dc600b68a780d0109e3b0327a39eb60cf3d76'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F7Y0CN1804HZW03J4XQ8XEJR-story-define-async-streaming-save-contract-and-b` as `cbbd5496a88d`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Refined, IAsyncEnumerable :: - Refined the ticket to an additive `IDataVaultSaveService` async streaming overload over `IAsyncEnumerable<DataVaultSaveChunk>`, reusing the existing chunked-save ordering, cancellation, retained-s...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Refined, IAsyncEnumerable :: - Refined the ticket to an additive `IDataVaultSaveService` async streaming overload over `IAsyncEnumerable<DataVaultSaveChunk>`, reusing the existing chunked-save ordering, canc...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8672`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `4ad2f00e9b6944b382e87ead9371ef97`
- completed-at-utc: `<redacted>-31T17:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0CN1804HZW03J4XQ8XEJR/runs/20260531T173416042Z-4ad2f00e9b6944b382e87ead9371ef97.json`