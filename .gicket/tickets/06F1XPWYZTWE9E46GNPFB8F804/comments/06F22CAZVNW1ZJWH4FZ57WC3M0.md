[gicket-bot] PO-critic review contract

Summary
- Ticket is sufficiently refined for developer handoff: the contract is bounded to a SQLite-first live-schema drift slice, `Open Questions` is `none`, and the repository already contains the design-time drift baseline, SQLite schema fixtures, opt-in external-provider lanes, and the docs that must be narrowed.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract at `.gicket/tickets/06F1XPWYZTWE9E46GNPFB8F804/description.md` contains explicit Scope In/Out, 6 acceptance criteria, 5 Definition of Done items, and `## Open Questions` -> `none`.
- Existing public drift baseline is in source: `src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs` exposes `Compare(...)` overloads for metadata/import-vs-EF metadata, and `src/DCoding.Data.DVault/DataVaultModelDriftDifference.cs` already defines machine-readable `Severity`, `Code`, `ElementKind`, `PropertyPath`, `ExpectedValue`, and `ActualValue`.
- Repository already has SQLite-first schema evidence: `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` asserts deterministic table names, ordered columns, named primary keys, secondary indexes, and `ForeignKeyCount(...) == "0"` for bridge tables; `tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs` provides an opt-in external schema lane.
- Provider lane boundaries and opt-in configuration contracts already exist in `tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs`, `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs`, and README sections for `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, and `DVAULT_TEST_MYSQL_CONNECTION_STRING`.
- Current docs still state that live drift is not yet implemented: `README.md:450` says `no live database drift introspection`, `docs/model-first-governance.md:136,214` says drift comparison `does not inspect a live database`, and `docs/releases/v0.7.0.md:49,83` repeats the same boundary; this matches the ticket's documentation-update DoD.
- Branch history confirms this branch is still ticket-refinement-only: `git diff --stat 1095f9f2297c5cdd2ca043e03b9e9326fcc93f17..01e8aa02f2a648cbe6cd13de7a7332b14f759db5` shows only `.gicket/tickets/06F1XPWYZTWE9E46GNPFB8F804/**` changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not state whether a renamed table/column/index/primary key should surface as a dedicated rename code or as the existing missing-plus-unexpected pattern.
- The contract requires unavailable-provider handling but does not give one concrete example assertion for an opt-in external lane when the environment variable contract is absent.

Risky assumptions
- Assumes live-schema normalization can reuse current drift conventions without introducing provider-specific casing or ordering false positives.
- Assumes unsupported-provider and unavailable-environment outcomes can be distinguished cleanly within existing diagnostics/drift conventions without needing a separate public result type.
- Assumes the current SQLite/Postgres schema-test helpers can be extracted into a provider-neutral live snapshot abstraction without widening scope into general catalog diffing.

AC / test suggestions
- Document in tests how a rename is represented, for example as a stable missing-plus-unexpected pair if no dedicated rename code is introduced.
- Require one required-local SQLite assertion for a matching live snapshot with zero blocking drift and one intentional mismatch assertion with stable `Code` and `ElementKind` values.
- Require one classified unavailable-or-unsupported outcome assertion for a requested external live comparison when the relevant `DVAULT_TEST_*` contract is not configured.

Implementation watchouts
- Do not change the semantics of existing design-time `DataVaultModelDriftReporter.Compare(...)` overloads without also reconciling the current docs and tests that say drift is design-time-only.
- Public API additions will trip `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` and the approved public API snapshot files.
- Required-local vs external opt-in categorization is enforced by `ProviderIntegrationCategoryDiscoveryTests`; any new live-schema tests need to fit those boundaries.
- Keep the v1 surface bounded to tables, ordered columns, named primary keys, and secondary indexes; the repo's current bridge schema evidence explicitly excludes foreign-key comparison.

Non-blocking notes
- Sibling ticket `.gicket/tickets/06F1XPWNAWWMDBRK315S66P7AM/ticket.json` is still `todo`, but the persisted contract keeps ModelSnapshot work out of scope for this ticket.
- Observed ticket comments are bot claim/refinement/handoff metadata only; no human objection or clarification comments were present.

Split recommendations
- No split is required for developer handoff; the ticket is already bounded as a SQLite-first live-schema abstraction with explicit unsupported-provider handling and documentation work.
- If first-class live readers are later needed for Postgres, SQL Server, Oracle, or MySQL, keep them in separate follow-up tickets instead of widening this slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment