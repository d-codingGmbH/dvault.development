# Provider Read Parity Outcomes And Benchmarks Refinement

Status: ticket-bound refinement note
Ticket: `06FBSCHBJEYYERDPA7JN34Y8PG`

## Purpose

Define the bounded documentation scope for the current provider-read parity baseline so live docs distinguish implemented strategy registration, completed timing evidence, provider-neutral fallback boundaries, and deferred or unmeasured provider lanes.

## Verified Repository Baseline

- `docs/plans/provider-optimization-evidence-matrix.md` is the canonical row lookup surface for provider read row identity, evidence posture, artifact source, and claim boundaries.
- `docs/plans/provider-optimization-gap-matrix.md` is the canonical follow-up backlog surface for remaining read evidence gaps and defer lanes.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` and `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md` already fix the bounded read fallback posture: unsupported providers or shapes, incomplete read-shape evidence, and stale PIT or bridge maintenance fall back to the provider-neutral read path.
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` now all register `IDataVaultProviderReadStrategy` for their provider-specific latest-satellite read candidate paths, while PIT and bridge registrations remain in place.
- The root benchmark artifact triplet (`benchmark-summary.md`, `.csv`, `.json`) keeps SQLite completed latest-satellite, PIT, and bridge timing rows, while PostgreSQL, SQL Server, MySQL, Oracle, and DB2 read rows remain skipped placeholders when their connection-string environment variables are unset. Those skipped rows preserve planned strategy facts such as `selectedStrategy` and `plannedReadStrategy`, but they are not completed timing evidence.
- The checked-in provider-configured smoke-read bundle under `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/` already contains completed PostgreSQL, SQL Server, MySQL, and Oracle `pit-as-of-read` and `bridge-traversal-read` rows with their provider read strategies selected.
- The current evidence matrix baseline is finite and should be ratified rather than reopened:
  - SQLite is the only completed-timing optimized `latest-satellite-read` lane.
  - PostgreSQL, SQL Server, MySQL, Oracle, and DB2 `latest-satellite-read` rows are implemented strategy or planned-strategy guidance rows in the root triplet, but they are still unmeasured because the checked-in rows are skipped placeholders.
  - PostgreSQL, SQL Server, MySQL, and Oracle `pit-as-of-read` and `bridge-traversal-read` rows are completed-timing evidence when cited through the v0.32 smoke-read bundle.
  - DB2 `pit-as-of-read` and `bridge-traversal-read` remain defer-lane evidence gaps with skipped-placeholder root rows plus diagnostics-only and smoke-only posture.
- Local `.gicket` relation state still includes historical incoming `blocks` links from done provider-specific closure tickets into this documentation ticket. Because the source tickets are `done` and the current ticket `is-blocked` flag is `false`, treat those links as closure-housekeeping context rather than as an active refinement blocker.

## Required Documentation Surfaces

- `docs/performance-profiles.md`
- `docs/architecture/dvault-v1-pit-bridge-boundary.md`
- `docs/releases/v0.41.0.md`

The evidence matrix and gap matrix remain the row-level fact sources. This ticket should cite them and align the live docs around them, not replace them with copied benchmark prose.

## Required Content Boundary

- Separate strategy availability from measured timing:
  - provider strategy registration, diagnostics selection, and skipped-placeholder benchmark guidance prove bounded parity outcomes
  - only completed-timing rows with preserved artifact triplets and run context may be described as measured provider wins
- Document the latest-satellite posture exactly as the current repository proves it:
  - SQLite has the only completed-timing optimized latest-satellite row
  - PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite rows may be documented as diagnostics-gated provider-specific strategy candidates with skipped-placeholder root rows
  - no current doc should promote those non-SQLite latest-satellite rows into completed external-provider timing evidence
- Document PIT and bridge posture exactly as the current repository proves it:
  - SQLite root rows are completed timing
  - PostgreSQL, SQL Server, MySQL, and Oracle PIT and bridge rows are completed timing only when cited through the checked-in v0.32 smoke-read bundle
  - DB2 PIT and bridge remain defer-lane rows with diagnostics-only, smoke-only, and skipped-placeholder evidence, not completed timing
- Preserve the finite fallback and caveat posture already fixed in code and architecture docs:
  - provider mismatch, unsupported satellite-parent or multi-active latest-satellite shapes, unsupported PIT or bridge shapes, incomplete read-shape evidence, and stale PIT or bridge maintenance fall back to provider-neutral reads
  - PIT and bridge reads consume explicitly maintained read models and do not imply automatic maintenance or scheduling
- Preserve the DB2 boundary as narrower than the PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge timing baseline:
  - DB2 strategy registration and smoke or diagnostics posture may be documented
  - completed DB2 latest-satellite, PIT, or bridge timing claims remain out of scope until a provider-configured benchmark artifact lane exists
- Keep historical incoming `blocks` links out of `open_questions`; if relation cleanup is later desired, treat it as housekeeping after documentation delivery rather than as a documentation-scope blocker.

## Acceptance Boundary

- `docs/performance-profiles.md` clearly distinguishes measured provider-read evidence from implemented-but-unmeasured latest-satellite guidance and from DB2 defer-lane posture.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` remains aligned with the current provider registrations, explicit-maintenance requirement, and finite provider-neutral fallback causes without implying new public read APIs or automatic maintenance.
- `docs/releases/v0.41.0.md` records the accepted provider-read posture change set without inventing new benchmark runs or promoting skipped, diagnostics-only, or smoke-only rows into timing claims.
- The live docs cite the evidence matrix and gap matrix consistently:
  - PostgreSQL, SQL Server, MySQL, and Oracle PIT or bridge outcomes are documented as completed timing only through the preserved v0.32 smoke-read artifact bundle
  - PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite outcomes are documented as strategy-registration or diagnostics-gated parity outcomes unless and until a completed provider-configured timing lane exists
  - DB2 PIT and bridge remain explicitly deferred from completed timing claims
- No update in this ticket reruns benchmarks, changes benchmark schemas, widens supported read shapes, changes provider code, or claims completed DB2 timing.
