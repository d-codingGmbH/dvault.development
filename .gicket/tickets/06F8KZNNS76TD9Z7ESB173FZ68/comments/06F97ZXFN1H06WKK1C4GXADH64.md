[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZNNS76TD9Z7ESB173FZ68'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZNNS76TD9Z7ESB173FZ68`.
- Optimistic claim succeeded (`expectedRevision=06F97XHFX6ESFZ9M4957EJY814`, `currentRevision=06F97XPKKX3SVCE9040HSPZ1QM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZNNS76TD9Z7ESB173FZ68-task-update-v0-29-0-provider-schema-guardrail-do' from source '38be2f42712935d87ec8cc6099f2f57ace1cd3e8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZNNS76TD9Z7ESB173FZ68-task-update-v0-29-0-provider-schema-guardrail-do` as `b47073a8a46a`.

Open questions / Risiken
- Risky assumption: Current provider profile names, diagnostic names, and guardrail terminology in `DataVaultDiagnostics` / `DataVaultMigrationOperationDiagnostics` will remain stable between refinement and implementation.
- Risky assumption: Updating `README.md`, `docs/production-adoption-checklist.md`, and the new `docs/releases/v0.29.0.md` will be enough for discoverability; `docs/model-first-governance.md` still calls `docs/releases/v0.26.0.md` the current public baseline.
- Split recommendation: No split recommended; the current contract already bounds the work to the missing `docs/releases/v0.29.0.md` plus coordinated public-doc updates for the provider schema guardrail slice.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8707`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `33f46822f1074b6ca8dcaed171ef5d8f`
- completed-at-utc: `<redacted>-04T18:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZNNS76TD9Z7ESB173FZ68/runs/20260604T185747936Z-33f46822f1074b6ca8dcaed171ef5d8f.json`