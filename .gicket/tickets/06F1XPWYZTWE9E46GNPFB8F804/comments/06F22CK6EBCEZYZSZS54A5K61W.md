[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F1XPWYZTWE9E46GNPFB8F804'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPWYZTWE9E46GNPFB8F804`.
- Optimistic claim succeeded (`expectedRevision=06F22ATHDGK95H2KHYTCG21RQC`, `currentRevision=06F22B57CAK6KBZRYVXT5KXX3C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPWYZTWE9E46GNPFB8F804-task-add-live-database-schema-drift-abstraction' from source '01e8aa02f2a648cbe6cd13de7a7332b14f759db5'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F1XPWYZTWE9E46GNPFB8F804-task-add-live-database-schema-drift-abstraction` as `36397dff4cd5`.

Open questions / Risiken
- Risky assumption: Assumes live-schema normalization can reuse current drift conventions without introducing provider-specific casing or ordering false positives.
- Risky assumption: Assumes unsupported-provider and unavailable-environment outcomes can be distinguished cleanly within existing diagnostics/drift conventions without needing a separate public result type.
- Risky assumption: Assumes the current SQLite/Postgres schema-test helpers can be extracted into a provider-neutral live snapshot abstraction without widening scope into general catalog diffing.
- Split recommendation: No split is required for developer handoff; the ticket is already bounded as a SQLite-first live-schema abstraction with explicit unsupported-provider handling and documentation work.
- Split recommendation: If first-class live readers are later needed for Postgres, SQL Server, Oracle, or MySQL, keep them in separate follow-up tickets instead of widening this slice.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9286`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `8cf0bc0c564c4bf598500813e93412a0`
- completed-at-utc: `<redacted>-13T11:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPWYZTWE9E46GNPFB8F804/runs/20260513T115651012Z-8cf0bc0c564c4bf598500813e93412a0.json`