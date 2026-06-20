[gicket-bot] PO-critic review contract

Summary
- Ticket 06FE4QQ9VF7B74E60CXEHSS5XW is bounded, repository-backed, and has no unresolved `## Open Questions`; it is ready for developer handoff to decide whether to keep or tune the current MySQL latest-satellite path under the documented evidence rules.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git diff --name-status develop...HEAD` shows only `.gicket/tickets/06FE4QQ9VF7B74E60CXEHSS5XW/*` metadata/comment/event changes, and `git log --oneline -n 4` shows PO claim/handoff commits (`03cbc779e`, `db7166390`, `1754b3d2c`, `5b228029e`), so this branch is a pre-dev refinement branch rather than implementation work.
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-30` registers `MySqlDataVaultReadStrategy` for `IDataVaultProviderReadStrategy`, `IDataVaultProviderPitReadStrategy`, and `IDataVaultProviderBridgeReadStrategy`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:272-321` proves the MySQL latest-satellite gate accepts both `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`, and fails closed for `ProviderNameMismatch`, `UnsupportedSatelliteParent`, and `MultiActiveSatelliteUnsupported`.
- `src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs:321-351` plus `tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:239-262` lock the latest-satellite SQL shape to `ROW_NUMBER() OVER (PARTITION BY ParentHashKey ORDER BY LoadTimestamp DESC)` with parent-hash-key `IN` batching and optional `LoadTimestamp <= asOf`.
- `tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs:171-265` shows a configured MySQL context selects `MySqlDataVaultReadStrategy` and returns correct latest and as-of rows.
- `benchmark-summary.md:81-83` keeps the root MySQL latest-satellite, PIT, and bridge rows as `skipped` when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset; the latest-satellite row still records `selectedStrategy=MySqlDataVaultReadStrategy`, `plannedReadStrategy=MySqlDataVaultReadStrategy`, and `readShape=LatestSatellite`.
- `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md:74-76` shows MySQL latest-satellite completed through provider-neutral fallback with `selectedStrategy=<none>`, while MySQL PIT and bridge completed with `MySqlDataVaultReadStrategy`.
- `docs/plans/provider-optimization-gap-matrix.md:86`, `docs/plans/provider-optimization-evidence-matrix.md:239,286-288`, `docs/performance-profiles.md:30,356,392-393`, and `docs/releases/v0.42.0.md:49` consistently separate missing MySQL latest-satellite timing from closed MySQL PIT/bridge timing.
- Relation files `.gicket/relations/SR/XW/06FE4QP6FB892E7TJMB47A3MSR--06FE4QQ9VF7B74E60CXEHSS5XW--blocks.json`, `.gicket/relations/DG/XW/06FE4QNWP9606HTB92MTVQMYDG--06FE4QQ9VF7B74E60CXEHSS5XW--relates.json`, and `.gicket/relations/XW/K8/06FE4QQ9VF7B74E60CXEHSS5XW--06FE4QRMXVGJVA65ZR5MZ817K8--blocks.json` match the contract; related ticket snapshots show `06FE4QP6FB892E7TJMB47A3MSR` and `06FE4QNWP9606HTB92MTVQMYDG` are `done`, while downstream docs ticket `06FE4QRMXVGJVA65ZR5MZ817K8` is still `todo`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No checked-in configured latest-satellite evidence lane covers `Pomelo.EntityFrameworkCore.MySql`; current configured read evidence uses `MySql.EntityFrameworkCore`, while Pomelo is covered by shared gate and SQL-shape tests.

Risky assumptions
- A later evidence pass can treat one configured MySQL provider lane as representative of both provider packages because `MySqlDataVaultReadStrategy` is shared and dual-provider gating is unit-tested; the contract leaves that as a follow-up question rather than a settled rule.

AC / test suggestions
- If development claims a MySQL latest-satellite improvement, require a provider-configured before/after latest-satellite artifact triplet that also preserves provider-neutral comparison behavior and `selectedStrategy`/`plannedReadStrategy`/`readShape=LatestSatellite` tokens.
- If the SQL shape changes, rerun latest and as-of MySQL read coverage and keep the dual-provider gate assertions (`Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`) visible in the ticket evidence bundle.

Implementation watchouts
- Do not cite `benchmark-summary.md:81` as completed timing; it is a skipped placeholder with `persistedOutcome=not executed`.
- Do not reuse `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md:75-76` PIT/bridge timing or `...:74` provider-neutral latest-satellite fallback as proof of a MySQL latest-satellite win.
- Any tuned path must preserve current fallback boundaries for provider mismatch, link-parent satellites, and multi-active satellites.

Non-blocking notes
- The downstream docs dependency already exists via `.gicket/relations/XW/K8/06FE4QQ9VF7B74E60CXEHSS5XW--06FE4QRMXVGJVA65ZR5MZ817K8--blocks.json`; no extra split is needed before developer handoff.

Split recommendations
- No split needed; the contract already keeps downstream docs in `06FE4QRMXVGJVA65ZR5MZ817K8` and points to existing sibling provider follow-ups.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment