[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FGX6B9KQME0NJ8B810239DG0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX6B9KQME0NJ8B810239DG0`.
- Optimistic claim succeeded (`expectedRevision=06FH1HH41V6PJY8H8Y7DVMKN7M`, `currentRevision=06FH1P7CTAXY51Y9QQVEYRWM38`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre' from source '618f32e81100afbec9b73de01a902f7b812dabd2'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre` as `9780ba9fedaf`.

Open questions / Risiken
- Risky assumption: Developers must wire the current validator/exporter manifest shape, not the older conceptual field names still described in some repository docs; the done upstream ticket 06FGX69QJYHGNKBV8MJ1HG7MMG records that mismatch risk.
- Risky assumption: If this work extends diagnostics or support-bundle output, the exact projection shape is intentionally left additive; only the redaction boundary and lane separation are fixed by the contract.
- Split recommendation: No split recommended: direct repository evidence shows the validator, preflight scaffolding, migration-guardrail lane, and support-bundle redaction baseline already exist, so the remaining work is bounded integration and test wiring.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8175`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `6d3430d66cce46f4913e96dffcb259ab`
- completed-at-utc: `<redacted>-29T00:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX6B9KQME0NJ8B810239DG0/runs/20260629T005557317Z-6d3430d66cce46f4913e96dffcb259ab.json`