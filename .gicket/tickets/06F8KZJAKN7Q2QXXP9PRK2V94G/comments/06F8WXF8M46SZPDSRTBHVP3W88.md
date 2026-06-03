[gicket-bot] PO-critic review contract

Summary
- Contract is ready for dev: scope stays inside the existing PIT/bridge read boundary, `## Open Questions` is `none`, and repository evidence already shows where PostgreSQL/SQL Server read-strategy work fits in the current provider-selection and fallback pipeline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZJAKN7Q2QXXP9PRK2V94G/description.md:11-29` scopes the story to existing `IDataVaultReadService` PIT/bridge boundaries, existing PostgreSQL/SQL Server provider packages, safe fallback, and explicit exclusions for API, maintenance, and generator-surface expansion.
- `.gicket/tickets/06F8KZJAKN7Q2QXXP9PRK2V94G/description.md:31-53` persists 6 acceptance criteria and `## Open Questions` = `none`.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md:10-12,57-63,92-99` states PIT/bridge reads consume already-maintained rows, `AddDVaultSqlite()` is the only repository-proven optimized PIT/bridge read path today, non-SQLite providers fall back to provider-neutral reads, and non-SQLite optimized PIT/bridge read claims are unsupported in v1.
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-33` registers `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy` for SQLite, while `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-24` and `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-24` currently register provider behavior plus save strategy only.
- `src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:123-214` and `src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs:189-250` already contain provider-specific PIT/bridge selection, provider-neutral fallback, and telemetry/fallback-cause plumbing that this story can extend without new public read APIs.
- `.gicket/relations/KM/4G/06F8KZHZ27SDTNCFNMFDQRVCKM--06F8KZJAKN7Q2QXXP9PRK2V94G--blocks.json:1-10` still persists a `blocks` relation from `06F8KZHZ27SDTNCFNMFDQRVCKM` to this ticket, but `.gicket/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/ticket.json:1-24` shows that upstream ticket is `done`.
- `git -C /mnt/c/Projects/DVault diff --name-only develop..HEAD -- ':(exclude).gicket/**'` returned no paths, and `git -C /mnt/c/Projects/DVault log --oneline -n 4` shows only PO/PO-critic claim and handoff commits on this branch, so this is still a normal pre-development ticket-quality gate rather than an implementation-review branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete example names a bounded multi-active hub-parent PIT parity case on PostgreSQL and SQL Server, even though the risk section calls out semantic drift there.
- No concrete example names a hierarchy bridge `maximumDepth` parity/fallback case on PostgreSQL and SQL Server.
- No concrete example names what specific stale-maintenance or incomplete-evidence condition should trigger fail-closed fallback first in provider-candidate tests.

Risky assumptions
- The existing provider-read evidence contract already defined by done ticket `06F8KZHZ27SDTNCFNMFDQRVCKM` is sufficient for PostgreSQL/SQL Server candidate gating without adding new public diagnostics fields.
- `Stale-maintenance signals` can be enforced through existing diagnostics/read-shape evidence and provider-strategy gates without widening maintenance orchestration or read APIs.
- Any provider-specific supported-shape exclusions discovered during implementation can be documented as a support-matrix limitation without forcing a PO re-split of this story.

AC / test suggestions
- Keep explicit parity coverage for bounded multi-active PIT behavior and hierarchy bridge depth semantics on both providers, since those are the highest-risk drift areas already named in the ticket.
- Make at least one candidate-selection test and one provider-neutral fallback test name the exact evidence/gate that qualified or disqualified the PostgreSQL/SQL Server strategy.
- If either provider cannot support the full published shape matrix, capture that as an explicit documentation/assertion outcome rather than leaving it implicit in failing or skipped tests.

Implementation watchouts
- Current repository wiring already supports provider-specific PIT/bridge dispatch and provider-neutral fallback in `src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:123-214` and `src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs:189-250`; new provider paths need to preserve those fallback and telemetry semantics.
- SQLite is the only package currently registering PIT/bridge read strategies in `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-33`; PostgreSQL and SQL Server provider packages currently expose save-only registrations, so dev work must add read-strategy registration without widening the public API boundary.
- This branch currently has no non-`.gicket` diff against `develop`, so the next handoff is purely pre-development and should be judged on ticket clarity, not on missing implementation artifacts.

Non-blocking notes
- none

Split recommendations
- No split is needed before dev handoff; the story remains bounded to existing PIT/bridge shapes, existing telemetry/diagnostics surfaces, and the existing provider-support architecture.
- If implementation expands into materially different provider limitations or benchmark/documentation work, split next by provider package rather than by new public API surface.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment