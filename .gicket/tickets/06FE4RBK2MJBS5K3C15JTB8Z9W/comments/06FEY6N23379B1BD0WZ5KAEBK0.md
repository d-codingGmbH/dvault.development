[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FE4RBK2MJBS5K3C15JTB8Z9W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RBK2MJBS5K3C15JTB8Z9W`.
- Optimistic claim succeeded (`expectedRevision=06FEY4Z3NMFB4HNVK8X186EE4R`, `currentRevision=06FEY57T1F686R87CATHSTM84R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4RBK2MJBS5K3C15JTB8Z9W-task-add-privacy-extension-example-and-documenta' from source '3815d99ed22c750ddf5b72fbc9ea8fa16ff20de0'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FE4RBK2MJBS5K3C15JTB8Z9W-task-add-privacy-extension-example-and-documenta` as `7879348c794c`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: DataVaultPrivacyOptions, RegisterEncryptedPayloadAlias, IDataVaultEncryptedPayloadKeyProvider :: - The v1 scope is documentation and example work around the already-shipped privacy proof surface, not new privacy runtime archite...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: DataVaultPrivacyOptions, RegisterEncryptedPayloadAlias, IDataVaultEncryptedPayloadKeyProvider :: - The v1 scope is documentation and example work around the already-shipped privacy proof surface, not new pri...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8821`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7c32303ebee440508d77a39a223a8782`
- completed-at-utc: `<redacted>-22T11:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RBK2MJBS5K3C15JTB8Z9W/runs/20260622T113240596Z-7c32303ebee440508d77a39a223a8782.json`