[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492B9PR036PDNN52S06S9BC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492B9PR036PDNN52S06S9BC`.
- Optimistic claim succeeded (`expectedRevision=06F4V0PDPA4TVM86H8CECJSWEC`, `currentRevision=06F4V0XX2608GYRAE54V5DSZV4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea' from source '66507e775963d9dcae3bf314194ba11903ef2ec3'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea` as `c3bf9c402a32`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Revalidated, IDataVaultReadDiagnosticsService, Analyze, DataVaultDiagnosticsResult, Explain, SaveStrategy, ReadStrategy, No :: - Revalidated the ticket against current branch source. IDataVaultReadDiagnosticsService already exp...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Revalidated, IDataVaultReadDiagnosticsService, Analyze, DataVaultDiagnosticsResult, Explain, SaveStrategy, ReadStrategy, No :: - Revalidated the ticket against current branch source. IDataVaultReadDiagnostic...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9259`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `beb5aa1c4f204fc28225b2bc978d73cd`
- completed-at-utc: `<redacted>-22T02:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492B9PR036PDNN52S06S9BC/runs/20260522T023646512Z-beb5aa1c4f204fc28225b2bc978d73cd.json`