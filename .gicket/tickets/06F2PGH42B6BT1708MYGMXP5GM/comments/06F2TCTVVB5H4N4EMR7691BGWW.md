[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F2PGH42B6BT1708MYGMXP5GM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGH42B6BT1708MYGMXP5GM`.
- Optimistic claim succeeded (`expectedRevision=06F2TB7SAYP8Y075TB9N88KRT0`, `currentRevision=06F2TBJ8SE2HA5THRW7FDD0A2G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGH42B6BT1708MYGMXP5GM-task-add-migration-guardrail-rule-coverage' from source 'c470db378329bccb4ea480544201206a6194bf62'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F2PGH42B6BT1708MYGMXP5GM-task-add-migration-guardrail-rule-coverage` as `0e77974868ec`.

Open questions / Risiken
- Risky assumption: The implementation will compare only provider-neutral structural invariants from `DataVaultDiagnosticsResult.Explain.Entities`, even though `DataVaultPropertyExplain` also exposes provider profile, store type, and value-format fields that the ticket says to i...
- Risky assumption: EF `CreateTableOperation` metadata is assumed to be sufficient to emit deterministic `migration/CreateTable/...` member paths without changing the public diagnostics issue shape.
- Split recommendation: No new split is needed beyond the existing contract: keep RenameTableOperation and absence-based drift inference in later follow-ups, and keep broad v0.11 documentation work in `06F2PGHA0EXJRGDHM4GQM7NPYR`.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9126`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7372abd9e9ad445e80f2fbeb046a8142`
- completed-at-utc: `<redacted>-15T19:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGH42B6BT1708MYGMXP5GM/runs/20260515T195320548Z-7372abd9e9ad445e80f2fbeb046a8142.json`