[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492B9PR036PDNN52S06S9BC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492B9PR036PDNN52S06S9BC`.
- Optimistic claim succeeded (`expectedRevision=06F4T9Z8G6XX4GK2527854F3VR`, `currentRevision=06F4TWR11F3TF4TY53WBRQHV3M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea' from source 'd2ddf804d26defe95757e1cca7da3019007d89ba'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea` as `b896e2a06a7b`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: DataVaultDiagnosticsResult, ReadStrategy :: - DataVaultDiagnosticsResult already exists but currently only exposes ReadStrategy for read-bound data; the query-shape payload in this ticket is a new additive member/model that mus...
- Blocking finding: Unsupported inferred API claim: Implement, DataVaultDiagnosticsResult, DataVaultReadStrategyDiagnostics :: - Implement the read-shape payload as a fresh additive model/property on DataVaultDiagnosticsResult; do not overload DataVaultReadStrategyDiagnostics or...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: DataVaultDiagnosticsResult, ReadStrategy :: - DataVaultDiagnosticsResult already exists but currently only exposes ReadStrategy for read-bound data; the query-shape payload in this ticket is a new additive m...
- Risky assumption: Existing API/type assumption lacks source evidence: Implement, DataVaultDiagnosticsResult, DataVaultReadStrategyDiagnostics :: - Implement the read-shape payload as a fresh additive model/property on DataVaultDiagnosticsResult; do not overload DataVaultReadSt...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.5411`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `4824ed0c72114c1e9b1a2056f8593082`
- completed-at-utc: `<redacted>-22T02:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492B9PR036PDNN52S06S9BC/runs/20260522T021905396Z-4824ed0c72114c1e9b1a2056f8593082.json`