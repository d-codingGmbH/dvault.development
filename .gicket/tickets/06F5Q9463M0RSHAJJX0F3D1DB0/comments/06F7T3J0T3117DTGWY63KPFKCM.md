[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F5Q9463M0RSHAJJX0F3D1DB0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- Optimistic claim succeeded (`expectedRevision=06F7T1MYCM01BJW8SWQPAWS2X4`, `currentRevision=06F7T1XZSV9PGQJEA82PHX0ZXG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope' from source '8fbda48e40ca1131f9c80c8f92a28c2983ce6596'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope` as `0505112e564c`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: No :: - No existing public ActivitySource or tracing holder is evidenced on the current branch, so the contract now treats the tracing holder/helper as net-new internal code unless a deliberate public API addition is made and s...
- Blocking finding: Unsupported inferred API claim: Do :: - Do not assume or require any pre-existing public tracing API; any new tracing holder/helper stays internal unless an intentional public API addition is made and the approved public API snapshot is updated in the same ch...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: No :: - No existing public ActivitySource or tracing holder is evidenced on the current branch, so the contract now treats the tracing holder/helper as net-new internal code unless a deliberate public API ad...
- Risky assumption: Existing API/type assumption lacks source evidence: Do :: - Do not assume or require any pre-existing public tracing API; any new tracing holder/helper stays internal unless an intentional public API addition is made and the approved public API snapshot is up...

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
- run-id: `02b5e4f489a645098fc593a06d1727b2`
- completed-at-utc: `<redacted>-31T08:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/runs/20260531T080226635Z-02b5e4f489a645098fc593a06d1727b2.json`