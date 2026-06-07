[gicket-bot] PO-critic review contract

Summary
- Contract is clear, evidence-backed, and has no unresolved Open Questions; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/description.md:8 and :53-54 record PO handoff decision ready_for_po_critic and Open Questions as none.
- .gicket/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/description.md:12 and :42 state the architecture contract ticket 06F8KZTNG44XDPMVTVCV4WJSHG and dry-run prototype ticket 06F8KZV18BQ0GN3CE4G02ATVA0 are done, while docs ticket 06F8KZVRARQPG482YKCQ686PNM and all-provider baseline task 06F9XD26D2MHVAKZ2GCZ67BEFC remain separate follow-ons.
- git -C /mnt/c/Projects/DVault rev-parse --short HEAD returned 2544b6c03, matching the supplied scratch ref prefix, and git -C /mnt/c/Projects/DVault diff --name-only 2544b6c03ba18cd4d8a14d29a0f24c72a2bf42d6..HEAD returned no files.
- docs/plans/performance-evidence-benchmark-artifact-contract.md:16-18, :75, :94, and :115 require the benchmark-summary.md / .csv / .json triplet, keep skipped optional-provider rows visible with deterministic executionDetail and persistedOutcome, limit the optional provider matrix to PostgreSQL/SQL Server/MySQL/Oracle, and fix the regression-budget section.
- docs/performance-profiles.md:17-25, :235, and :278 tie the checked-in baseline to the root benchmark triplet, record optional PostgreSQL/SQL Server/MySQL/Oracle rows as skipped when connection strings are unset, and explicitly say the checked-in provider-native bulk rows are visibility/boundary evidence rather than measured wins.
- benchmark-summary.json:17-34 shows all optional external providers present in optionalProviders with executionStatus skipped; benchmark-summary.json:654-670 keeps the SQL Server provider-native-bulk-ingestion optimized row visible with iterations 0, transfer=SqlBulkCopy, nativeBulkBoundary=50-plus-operations, cleanupBoundary=temporary-staging-table, and persistedOutcome not executed.
- src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs:119-153 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs:147-215 prove the dry-run manifest example is one provider/one workload, uses the shared benchmark triplet, and carries semantic parity fields instead of runtime dispatch.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:355, :755, :1075, :1153, and :1319 plus tests/DCoding.Data.DVault.Tests/Unit/StreamingExplicitSaveContractSnapshotTests.cs provide the cited anchors for hash-diff continuity, chronological latest-state behavior, cancellation before later chunks, caller-owned transaction participation, and ordered chunked-save semantics.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Future PIT/bridge-focused artifact tickets should cite PIT/bridge maintenance parity evidence explicitly rather than assuming the save-oriented checklist is sufficient.

Risky assumptions
- Treating the current SQL Server dry-run example as general proof for PostgreSQL, MySQL, or Oracle would overstate the evidence; the checked-in external-provider benchmark rows are still skipped when connection strings are unset.
- Treating skipped optional-provider rows or dry-run manifests as production-readiness proof would violate the ticket contract; exact-provider diagnostics and completed benchmark evidence for the same workload are still required.

AC / test suggestions
- Require downstream implementation tickets to cite the exact benchmark row keys/baselines and the exact request-bound diagnostics source used for the proposal.
- Keep verifier coverage that the markdown/CSV/JSON triplet remains synchronized for any newly persisted evidence set.
- For PIT/bridge-specific workloads, add acceptance criteria that name the maintenance evidence anchors alongside the shared parity checklist.

Implementation watchouts
- Keep the lane one provider and one representative workload at a time; do not expand a single artifact proposal into provider-family or multi-workload claims.
- Do not change the shared benchmark artifact schema or hide skipped external-provider rows; visibility of skipped rows is part of the locked evidence contract.
- Preserve caller order, load timestamp, record source, hash key/hash diff, cancellation behavior, cleanup boundary, and caller-owned transaction behavior exactly as the cited contract and tests describe.
- Do not convert prototype or documentation evidence into runtime dispatch, automatic execution, automatic deployment, or EF migration synchronization scope.

Non-blocking notes
- The current review branch has no additional repository delta beyond the supplied scratch ref; for this pre-development refinement ticket that is a watchout, not a PO blocker.

Split recommendations
- No new split is justified; keep evidence requirements in 06F8KZVCVRPS3NAGQA7J55EAA4, dry-run manifest prototyping in 06F8KZV18BQ0GN3CE4G02ATVA0, documentation alignment in 06F8KZVRARQPG482YKCQ686PNM, and all-provider baseline capture in 06F9XD26D2MHVAKZ2GCZ67BEFC.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment