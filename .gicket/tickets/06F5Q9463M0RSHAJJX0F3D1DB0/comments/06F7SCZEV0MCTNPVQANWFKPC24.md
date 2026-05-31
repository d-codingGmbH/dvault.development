[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F5Q9463M0RSHAJJX0F3D1DB0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- Optimistic claim succeeded (`expectedRevision=06F7SASA3MBBYM5Q6H1XJ96KTC`, `currentRevision=06F7SB2920WG7MDDEBRVSQ1XPG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope' from source '45e1129653c46c118ec77a33b250f81e1350a085'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope` as `13995ff2614c`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Instrument :: - Instrument latest-satellite and PIT explicit read boundaries plus the existing public helper surfaces for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridge reads so each exec...
- Blocking finding: Unsupported inferred API claim: Do :: - Do not assume or require any pre-existing public tracing API; if a holder/helper is added by this story it stays internal unless an intentional public API change is snapshot-reviewed.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Instrument :: - Instrument latest-satellite and PIT explicit read boundaries plus the existing public helper surfaces for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridg...
- Risky assumption: Existing API/type assumption lacks source evidence: Do :: - Do not assume or require any pre-existing public tracing API; if a holder/helper is added by this story it stays internal unless an intentional public API change is snapshot-reviewed.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7721`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f527b817a69943d6bda65166662a0f20`
- completed-at-utc: `<redacted>-31T06:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/runs/20260531T062347412Z-f527b817a69943d6bda65166662a0f20.json`