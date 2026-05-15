[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGG8ZKSYGC8863118H56G8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGG8ZKSYGC8863118H56G8`.
- Optimistic claim succeeded (`expectedRevision=06F2QAGNA3GAA9NQRSQ6Q2G534`, `currentRevision=06F2QAPPZGZ1A2ZZF5666PC0RW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers' from source 'd33454ad873f6569a82fdc373e159169831902f9'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` as `84efa3299b2e`.

Open questions / Risiken
- Blocking finding: This closure-only/handoff state is unsupported by the repository: compared with `develop`, the branch contains ticket/comment updates only and no product or test implementation for the claimed provider-reader work.
- Blocking finding: Acceptance criteria 1, 2, and 5 are not satisfied because `DataVaultLiveSchemaReader.ReadAsync(...)` still dispatches only to SQLite and classifies every non-SQLite provider as `UnsupportedProvider`.
- Blocking finding: Acceptance criterion 3 and Definition of Done items 1-2 are not satisfied because there is no direct Postgres/SQL Server/Oracle/MySQL `DataVaultLiveSchemaReader.ReadAsync(...)` integration coverage on the branch; only SQLite live-schema tests are present.
- Required PO action: Return this ticket to PO refinement and remove the unsupported implication that the current branch is ready for closure/handoff without developer implementation.
- Required PO action: If implementation exists elsewhere, attach the exact branch/ref/commit and related test evidence; otherwise keep this as actual developer work and do not resend to PO-critic until non-ticket `src/` and `tests/` evidence is present.
- Required PO action: If release scope is intended to remain SQLite-only, narrow this ticket contract accordingly and move first-class PostgreSQL/SQL Server/Oracle/MySQL live-schema readers into a separate implementation ticket instead of asserting them on this branch.
- Risky assumption: Assuming the presence of `ExternalProviderLiveSchemaFixture` means provider reader support already exists; repository search shows `DataVaultLiveSchemaReader.ReadAsync(...)` is only exercised in SQLite tests.
- Risky assumption: Assuming built-in provider capability selection automatically gives live-schema reader support; `DataVaultLiveSchemaReader.cs:31-34` bypasses those non-SQLite profiles and returns `UnsupportedProvider` instead.
- Risky assumption: Assuming the ticket title and PO contract are enough to prove closure readiness; branch history and diff show only ticket-state commits after `develop`.
- Split recommendation: Do not split purely to paper over the missing implementation evidence; first correct the unsupported closure-only/handoff framing.
- Split recommendation: If this is re-opened as real developer work, keeping one bounded ticket still looks reasonable because the shared contract/fixture baseline already exists; split by provider only if provider-specific catalog behavior or external setup becomes independentl...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9239`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c7a644fdb59e44cb901993f148929daf`
- completed-at-utc: `<redacted>-15T12:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGG8ZKSYGC8863118H56G8/runs/20260515T124943030Z-c7a644fdb59e44cb901993f148929daf.json`