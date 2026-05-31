[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F5Q9463M0RSHAJJX0F3D1DB0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- Optimistic claim succeeded (`expectedRevision=06F7RFZT246C7GY4SD0RY8GYN4`, `currentRevision=06F7RSEPD35E6FYTYPS1EA9ZGC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope' from source '64a9ce90f482a4ca0f82812798060309f93aa202'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope` as `9e0ae55c8bc7`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Reframed :: - Reframed the story as net-new internal Activity instrumentation over source-backed save/read entry points, removing unsupported assumptions about an existing public tracing API or type.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Reframed :: - Reframed the story as net-new internal Activity instrumentation over source-backed save/read entry points, removing unsupported assumptions about an existing public tracing API or type.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9085`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `fdc2a7cac366433196a920a4a8d6bcb3`
- completed-at-utc: `<redacted>-31T05:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/runs/20260531T050844864Z-fdc2a7cac366433196a920a4a8d6bcb3.json`