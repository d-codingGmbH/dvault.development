[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FF43Y6JE9NQWTAQRQXV2YS80'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43Y6JE9NQWTAQRQXV2YS80`.
- Optimistic claim succeeded (`expectedRevision=06FG775QS6EY49S7EVXES6RBQW`, `currentRevision=06FG7A1R76FN55Q8P78YXH9MZR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same' from source 'b23fb33030b3552d5e23b7abbe0df575ef77bfaf'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same` as `d9408d1340bb`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Claiming :: - Claiming or relying on an already-existing public participant explain type that is not visible in current branch evidence.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Claiming :: - Claiming or relying on an already-existing public participant explain type that is not visible in current branch evidence.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9016`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `1b287de2c73645a2a639a2b202fbb16c`
- completed-at-utc: `<redacted>-26T11:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43Y6JE9NQWTAQRQXV2YS80/runs/20260626T112728970Z-1b287de2c73645a2a639a2b202fbb16c.json`