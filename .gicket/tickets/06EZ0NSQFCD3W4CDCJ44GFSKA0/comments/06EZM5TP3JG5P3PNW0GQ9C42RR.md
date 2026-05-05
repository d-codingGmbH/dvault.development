[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NSQFCD3W4CDCJ44GFSKA0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NSQFCD3W4CDCJ44GFSKA0`.
- Optimistic claim succeeded (`expectedRevision=06EZM4R53XG8J4JWXM74P2E690`, `currentRevision=06EZM51VC96ETG0D5P0GVDF6KC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NSQFCD3W4CDCJ44GFSKA0-task-add-api-snapshot-guardrails-for-deferred-ca' from source '4aeba57e5965ad114d771427c99a2d3934cdad94'.
- Interactive PO-critic tool loop fell back to legacy critic review after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NSQFCD3W4CDCJ44GFSKA0-task-add-api-snapshot-guardrails-for-deferred-ca` as `a2b71efe5233`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Refine :: - Refine or extend the existing API snapshot approval test and approved snapshot files so newly exported deferred-capability contracts are covered in the affected packable package.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Refine :: - Refine or extend the existing API snapshot approval test and approved snapshot files so newly exported deferred-capability contracts are covered in the affected packable package.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `71808`
- effective-cache-ratio: `0.6429`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `77e15d5355ed4cd2b8081c47cb61b603`
- completed-at-utc: `<redacted>-05T21:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/runs/20260505T214205767Z-77e15d5355ed4cd2b8081c47cb61b603.json`