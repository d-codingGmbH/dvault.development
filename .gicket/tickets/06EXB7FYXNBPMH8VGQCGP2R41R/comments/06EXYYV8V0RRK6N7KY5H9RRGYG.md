[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7FYXNBPMH8VGQCGP2R41R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FYXNBPMH8VGQCGP2R41R`.
- Optimistic claim succeeded (`expectedRevision=06EXYX8PACKWAFAJV3WBJEV1GM`, `currentRevision=06EXYXC8FA524DWCHDXHW4F0AG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7FYXNBPMH8VGQCGP2R41R-task-translate-dvault-metadata-into-ef-model-met' from source 'feb7de3027adcaeae6193ab3763e5ac46f61711a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7FYXNBPMH8VGQCGP2R41R-task-translate-dvault-metadata-into-ef-model-met` as `966f6fbc090d`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: EF :: - The implementation accepts the existing public DVault metadata types as its v1 input and produces opt-in EF model metadata only when the caller explicitly invokes the DVault translation path; a bare ModelBuilder.UseData...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: EF :: - The implementation accepts the existing public DVault metadata types as its v1 input and produces opt-in EF model metadata only when the caller explicitly invokes the DVault translation path; a bare ...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9252`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `8b2773f77fd144f1b4a858f454b28491`
- completed-at-utc: `<redacted>-30T17:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FYXNBPMH8VGQCGP2R41R/runs/20260430T174139560Z-8b2773f77fd144f1b4a858f454b28491.json`