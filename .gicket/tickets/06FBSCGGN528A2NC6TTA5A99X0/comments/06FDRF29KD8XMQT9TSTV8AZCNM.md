[gicket-bot] PO-critic review contract

Summary
- Ticket is bounded and repository-backed: the runtime candidate, completed PostgreSQL PIT/bridge artifact lane, and fail-closed fallback gates are directly evidenced, and the persisted delivery contract has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCGGN528A2NC6TTA5A99X0/description.md` contains a persisted Delivery Contract with `## Open Questions` = `none`, explicit Scope In/Out, and acceptance criteria limited to PostgreSQL PIT/bridge evidence reclassification rather than new API or strategy work.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` registers `PostgresDataVaultReadStrategy` for both `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy`, matching the claimed implementation baseline.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` verifies fail-closed PIT/bridge fallback for incomplete read-shape evidence and stale maintenance, and `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs` emits `StaleReadModelMaintenance` for dirty-context PIT/bridge reads.
- The root quick baseline keeps PostgreSQL PIT/bridge as placeholders: `benchmark-summary.md` rows for `pit-as-of-read` and `bridge-traversal-read` are `skipped` with `not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.` and still name `plannedReadStrategy=PostgresDataVaultReadStrategy`.
- The cited provider-configured artifact lane already has completed PostgreSQL rows: `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md` shows `pit-as-of-read` and `bridge-traversal-read` as `completed` with `selectedStrategy=PostgresDataVaultReadStrategy`.
- Current canonical docs still show the gap this ticket is meant to close: `docs/plans/provider-optimization-gap-matrix.md` still lists PostgreSQL `P2.01` and `P3.01` as evidence gaps, `docs/plans/provider-optimization-evidence-matrix.md` still marks those rows `skipped-placeholder`, and `docs/performance-profiles.md:17` still says PostgreSQL PIT/bridge rows remain evidence-gap recommendations until provider-configured triplets exist.
- Branch history is still pre-development: `git log --oneline --max-count=6` on `ticket/06FBSCGGN528A2NC6TTA5A99X0-task-close-postgresql-pit-and-bridge-read-gaps` shows only `.gicket` handoff/claim commits, and `git diff --stat f01ed4f56..70ccff9a4` changes ticket metadata only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not name every live guidance surface explicitly; implementers will need to confirm whether `docs/production-adoption-checklist.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md` also need the PostgreSQL PIT/bridge evidence story updated so they do not keep SQLite-only/skipped-placeholder wording.

Risky assumptions
- Assumes the v0.32.0 smoke-read bundle is the accepted canonical evidence source for PostgreSQL PIT/bridge without needing any newer artifact lane.

AC / test suggestions
- Add or update a verifier assertion that PostgreSQL `pit-as-of-read` and `bridge-traversal-read` move from root `skipped-placeholder` guidance to the cited provider-configured artifact lane without changing PostgreSQL `latest-satellite-read` or other providers in the same pass.
- Require any touched docs/tests to keep `selectedStrategy=PostgresDataVaultReadStrategy` for PIT/bridge and `providerSpecificReadStrategy=not registered for latest satellite reads` for PostgreSQL latest-satellite.

Implementation watchouts
- Do not close the ticket by rewriting the root `benchmark-summary.*` triplet into completed PostgreSQL timing evidence; the contract and current repo evidence point to the provider-configured artifact bundle instead.
- Do not accidentally promote SQL Server, MySQL, Oracle, or DB2 PIT/bridge rows just because the same artifact bundle contains them.
- Preserve the existing fail-closed boundary for incomplete read-shape evidence, stale maintenance, unsupported PIT/bridge shapes, and provider-neutral fallback.

Non-blocking notes
- The current branch contains refined ticket metadata only; no repository code or documentation changes for the feature have landed yet.

Split recommendations
- No split recommended; the repository evidence supports one bounded PostgreSQL evidence-reclassification task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment