[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the ticket is repository-backed, pre-development, and already refined to a concrete documentation-only scope with no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCHBJEYYERDPA7JN34Y8PG/description.md` records PO handoff `ready_for_po_critic`, scopes work to `docs/performance-profiles.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md`, and `docs/releases/v0.40.0.md`, and sets `## Open Questions` to `- none`.
- `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06FBSCHBJEYYERDPA7JN34Y8PG/...` plus `docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md`; the three live docs in scope are not yet changed on this branch, so this is still a pre-development handoff rather than a post-dev review.
- `docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md` persists the bounded scope and names the evidence matrix, gap matrix, root benchmark triplet, and `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/` as the authoritative sources.
- Provider registration is directly backed by code: `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:31-33`, `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:24-26`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:24-26`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:28-30`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:24-26`, and `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs:24-26` each register provider read strategies, including `IDataVaultProviderReadStrategy`.
- `benchmark-summary.csv` line 19 is the completed SQLite optimized `latest-satellite-read`, lines 21 and 23 are the completed SQLite optimized PIT/bridge rows, while lines 42, 45, 48, 51, 54, 55, and 56 keep PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite plus DB2 PIT/bridge rows as `skipped` placeholders with unset connection-string reasons.
- `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.csv` lines 34-35, 39-40, 45-46, and 50-51 provide completed PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge rows with provider read strategies selected.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers will treat the `## Follow-Up Questions` as later backlog routing and will not expand this ticket into provider-configured latest-satellite timing collection or DB2 PIT/bridge benchmark activation.
- The checked-in v0.32.0 smoke-read bundle will remain the authoritative PIT/bridge timing source for PostgreSQL, SQL Server, MySQL, and Oracle until a later ticket explicitly supersedes it.

AC / test suggestions
- Use the evidence matrix row identity (`scenario`, `provider`, `baseline`, `posture`) as a doc-review checklist across all three live docs so each statement lands in one of three buckets: completed timing, skipped-placeholder strategy guidance, or diagnostics/smoke-only posture.
- Require each PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge timing statement in the live docs to cite the v0.32.0 smoke-read bundle rather than the root quick baseline.
- Run a final text review across the three scoped docs for `latest-satellite`, `DB2`, `pit-as-of-read`, and `bridge-traversal-read` to ensure no wording implies completed non-SQLite latest-satellite timing or completed DB2 PIT/bridge timing.

Implementation watchouts
- Do not turn root skipped rows that carry `selectedStrategy` or `plannedReadStrategy` into measured wins; in `benchmark-summary.csv` they are placeholders when provider connection strings are unset.
- Do not use the v0.32.0 smoke-read bundle to claim latest-satellite parity; for this ticket its authoritative completed timing use is PIT/bridge only.
- Keep `docs/releases/v0.40.0.md` aligned with `docs/performance-profiles.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md` without implying benchmark reruns, new public APIs, or automatic PIT/bridge maintenance.
- Historical incoming `blocks` relation cleanup stays out of scope for this documentation ticket.

Non-blocking notes
- Current repository text already carries much of the posture in `docs/performance-profiles.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md`; the main branch-level delta before dev work is the ticket contract and refinement note.
- `docs/releases/v0.40.0.md` currently mentions PostgreSQL latest-satellite registration but does not yet serve as the full cross-provider read-parity summary, which matches the stated need for this ticket rather than exposing a PO refinement gap.
- `git rev-parse HEAD` matches the provided scratch-source ref `9a69159dbb6671a949b5eda1d4277e1ad5e8d4ed`, so the review used the expected snapshot.

Split recommendations
- No split is needed for developer handoff. If later work is opened, keep it split between provider-configured latest-satellite timing collection and DB2 PIT/bridge evidence activation, as the current contract already recommends.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment