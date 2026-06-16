[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSCFKWGQMBEF5Q96AZ5Q0X0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCFKWGQMBEF5Q96AZ5Q0X0`.
- Optimistic claim succeeded (`expectedRevision=06FD1RY92KRT82MBZXZ00C1HVW`, `currentRevision=06FD1S4TJWFC7DPRV0AAZ0GGBG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap' from source '69a8f93a63df75995e11e110426dae2deff5192d'.
- Interactive PO-critic tool loop fell back to legacy critic review after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap` as `1d655ad5272a`.

Open questions / Risiken
- Risky assumption: The existing SQL Server latest-hash-diff query helpers in SqlServerDataVaultSaveStrategy can be reused for read-path semantics without introducing behavioral drift from the provider-neutral latest-satellite pipeline.
- Risky assumption: Existing read diagnostics and fallback vocabularies are sufficient for the new SQL Server latest-satellite path, so no additional public diagnostics surface is needed.
- Risky assumption: If DVAULT_TEST_SQLSERVER_CONNECTION_STRING stays unset, skipped-placeholder benchmark artifacts with corrected planned/selected path tokens will be accepted as sufficient evidence for this ticket.
- Split recommendation: No split recommended; the current contract is already bounded to one provider and to existing read-strategy, diagnostics, benchmark, and documentation surfaces.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8254`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e9c749a55cea48928ad667f100268895`
- completed-at-utc: `<redacted>-16T14:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCFKWGQMBEF5Q96AZ5Q0X0/runs/20260616T145200321Z-e9c749a55cea48928ad667f100268895.json`