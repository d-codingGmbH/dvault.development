[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F5Q8XXSBGW1B8RDRMGVF557W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8XXSBGW1B8RDRMGVF557W`.
- Optimistic claim succeeded (`expectedRevision=06F5XK25BK4WBSBBT0732G6YCM`, `currentRevision=06F5Y15FVPFKJG5N74JMF77X6C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q8XXSBGW1B8RDRMGVF557W-story-add-streaming-save-benchmark-evidence' from source '62311ce817042fcfef5412d71a822ba1c7200485'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F5Q8XXSBGW1B8RDRMGVF557W-story-add-streaming-save-benchmark-evidence` as `8ab6795ae8a4`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: No :: - No new public streaming/chunked save API, telemetry type, or retained-state contract design; those surfaces already exist in source, docs, and tests.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: No :: - No new public streaming/chunked save API, telemetry type, or retained-state contract design; those surfaces already exist in source, docs, and tests.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9279`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `461d7adf39b443c08bbf5c6f5fa11ed3`
- completed-at-utc: `<redacted>-25T12:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8XXSBGW1B8RDRMGVF557W/runs/20260525T121032660Z-461d7adf39b443c08bbf5c6f5fa11ed3.json`