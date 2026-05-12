[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0MEFHKF04B746X7GJKRVT04'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEFHKF04B746X7GJKRVT04`.
- Optimistic claim succeeded (`expectedRevision=06F1SKCWSZ846ERX9NRKSBBG40`, `currentRevision=06F1SKVYSHJMEXCT9NMNBGG2PG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' from source '3c9f0d257db09fd6a9b947816a5b3640a9b01cb6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry` as `ec25fed3c8f2`.

Open questions / Risiken
- Risky assumption: Assuming loadTimestampStorage can always be inferred from registry provider profiles without an explicit model-level property is risky unless empty, imported, custom, and mixed profile sets are covered.
- Risky assumption: Assuming all bridge metadata is exportable is risky; role-bearing hierarchy shapes are representable only when the source metadata carries enough participant role information.
- Risky assumption: Assuming Code-First coverage implies a new public raw fluent export API would contradict the refined contract and release documentation.
- Split recommendation: No split recommended for this ticket as refined. Keep direct Code-First-to-registry/export APIs and legacy PointInTimeTables migration helpers as later tickets.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9428`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c0f90cfcd57e443fa799b4fa0f2a2f16`
- completed-at-utc: `<redacted>-12T15:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEFHKF04B746X7GJKRVT04/runs/20260512T154441682Z-c0f90cfcd57e443fa799b4fa0f2a2f16.json`