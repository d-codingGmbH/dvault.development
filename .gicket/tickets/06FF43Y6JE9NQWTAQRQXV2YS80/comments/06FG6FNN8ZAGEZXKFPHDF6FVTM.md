[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FF43Y6JE9NQWTAQRQXV2YS80'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43Y6JE9NQWTAQRQXV2YS80`.
- Optimistic claim succeeded (`expectedRevision=06FG6D7X47RRMFZ15D2KRYRRX8`, `currentRevision=06FG6DJTRPXASAPR247QKC0KZC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same' from source 'dfd5fb2040ca254270caacc371387109d408992f'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same` as `2dc066097784`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Refined :: - Refined this as a bounded additive support-bundle explain-contract ticket: repeated same-hub links already exist in metadata/model artifacts and explicit save operations, but the support bundle still needs explicit...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Refined :: - Refined this as a bounded additive support-bundle explain-contract ticket: repeated same-hub links already exist in metadata/model artifacts and explicit save operations, but the support bundle ...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8803`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `10912b471638469d83b3f76fbb28fe2c`
- completed-at-utc: `<redacted>-26T09:24:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43Y6JE9NQWTAQRQXV2YS80/runs/20260626T092429122Z-10912b471638469d83b3f76fbb28fe2c.json`