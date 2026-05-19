[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGPRGN0EVGD6RY5KY9M56W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPRGN0EVGD6RY5KY9M56W`.
- Optimistic claim succeeded (`expectedRevision=06F40XJKV00YF1KYYT4K1FSCRR`, `currentRevision=06F40XNE252FNE3X4ESSQ4GAYG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt' from source 'f669a6c3b6f510e2072d8a836fd5296ec05eab6e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt` as `f029b0961623`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Visible, DataVaultBridgeReadRequest, ReadBridgeRowsAsync, ReadBridgeAsync :: - Visible source evidence in this refinement pass confirms DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and IDataVaultReadService.ReadPitR...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Visible, DataVaultBridgeReadRequest, ReadBridgeRowsAsync, ReadBridgeAsync :: - Visible source evidence in this refinement pass confirms DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and IDataVault...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7082`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b1b12642fd5d49b1a98e0c9e54ec37a3`
- completed-at-utc: `<redacted>-19T13:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/runs/20260519T135042721Z-b1b12642fd5d49b1a98e0c9e54ec37a3.json`