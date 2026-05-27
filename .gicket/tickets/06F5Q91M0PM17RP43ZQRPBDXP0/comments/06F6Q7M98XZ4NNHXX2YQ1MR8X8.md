[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the delivery contract is bounded, Open Questions are resolved, and the cited PIT/bridge repository evidence is present.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q91M0PM17RP43ZQRPBDXP0/description.md records PO handoff decision ready_for_po_critic and Open Questions as none.
- README.md:323-367 already defines the PIT/bridge boundary: explicit IDataVaultPitMaintenanceService and IDataVaultBridgeMaintenanceService, SQLite-optimized read dispatch only via AddDVaultSqlite(), provider-neutral fallback elsewhere, bounded link-parent and shared-driving-key multi-active PIT support, and RebuildBridgeAsync(...) vs MaintainBridgeAsync(...).
- README.md:713-751, docs/production-adoption-checklist.md:9, and docs/model-first-governance.md:3-5 still treat docs/releases/v0.20.0.md as the current public baseline.
- git ls-files returned docs/releases/v0.20.0.md but not docs/releases/v0.21.0.md; docs/releases/v0.20.0.md:1-28 exists and carries the current coordinated release-note baseline.
- ls docs/architecture lists only dvault-dotnet-ef-design-time-workflow.md, dvault-ef-compiled-compatibility.md, dvault-v1-explicit-save-service.md, dvault-v1-streaming-explicit-save-contract.md, dvault-v1-typed-row-mapper-contract.md, and mvp-data-vault-concepts.md; there is no PIT/bridge boundary note yet.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs:11-43 and DataVaultPitMaintenanceServiceSqliteTests.cs:82-120,433-507 exercise SQLite PIT reads/maintenance, multi-active PIT behavior, fallback diagnostics, and the registry-backed rebuild/parent-maintenance requests named in the contract.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs:33-85 and DataVaultBridgeMaintenanceServiceSqliteTests.cs:33-65,156-180 exercise SQLite bridge read strategy selection, provider-neutral fallback, append-only incremental maintenance, and rebuild after shrink/delete scenarios.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:112-170 verifies read-strategy and read-shape diagnostics for SQLite reads, matching the ticket's diagnostics-evidence requirement.
- benchmark-summary.csv:19-22 and artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.csv:19-22 contain PIT as-of and bridge traversal rows for both provider-neutral fallback and SQLite-optimized paths; docs/plans/performance-evidence-benchmark-artifact-contract.md:12-18,53-88 defines the shared artifact contract and requires those scenarios.
- rg -n DataVaultRegistryPitAsOfReadRequest src tests README docs returned no matches, while README.md:346 names only DataVaultRegistryPitRebuildRequest and DataVaultRegistryPitParentMaintenanceRequest.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Keep the runtime link-parent PIT support vs model-first hub-parent-only PIT artifact distinction explicit, because README.md:325-346 and docs/production-adoption-checklist.md:115 separate those boundaries.
- Call out destructive hierarchy shrink/delete as the case that requires RebuildBridgeAsync(...), not MaintainBridgeAsync(...), matching README.md:367 and DataVaultBridgeMaintenanceServiceSqliteTests.cs:156-180.

Risky assumptions
- Assumes this story intentionally rolls the documentation baseline to v0.21.0 without needing additional capability work, because the cited PIT/bridge diagnostics/benchmark story 06F5Q91DR1555RSBQT7KDST684 is already done and the repository proof points exist.
- Assumes README installation version snippets are either intentionally out of scope or will be handled consistently during implementation; the contract explicitly calls out baseline-reference updates but not every 0.20.0 package-version literal.
- Assumes no registry-backed PIT as-of read API should be documented until direct source evidence exists; the repo search found no DataVaultRegistryPitAsOfReadRequest symbol.

AC / test suggestions
- Acceptance review should verify every new or updated doc link resolves to existing repo assets: tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs, DataVaultPitMaintenanceServiceSqliteTests.cs, DataVaultBridgeReadServiceSqliteTests.cs, DataVaultBridgeMaintenanceServiceSqliteTests.cs, DataVaultDiagnosticsIntegrationTests.cs, benchmark-summary.{md,csv,json}, artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.{md,csv,json}, and docs/plans/performance-evidence-benchmark-artifact-contract.md.
- Doc QA should diff the finished wording against README.md:323-367 and docs/production-adoption-checklist.md:55-58,115-116 so the new v0.21.0 note and architecture entrypoint do not widen claims beyond explicit maintenance, SQLite-only optimization, provider-neutral fallback, and non-delete-aware bridge behavior.

Implementation watchouts
- The new architecture note should centralize PIT/bridge boundaries rather than split them across existing design-time or compiled-compatibility notes, matching the current gap in docs/architecture.
- Do not infer a registry-backed PIT as-of read API, automatic maintenance, delete-aware bridge maintenance, or non-SQLite optimized PIT/bridge reads from benchmarks or prose; the current proof set only supports the narrower README/test boundary.

Non-blocking notes
- This is a clean pre-development handoff: git log develop..ticket/06F5Q91M0PM17RP43ZQRPBDXP0-task-update-v0-21-0-pit-and-bridge-completeness shows only PO and PO-critic workflow commits.
- No ticket comment reviewed in .gicket/tickets/06F5Q91M0PM17RP43ZQRPBDXP0/comments introduced a new unresolved PO question after the durable refinement contract was written.

Split recommendations
- No split is needed for the current bounded roll-forward: v0.21.0 release note, baseline-reference updates, and one dedicated PIT/bridge architecture entrypoint.
- If stakeholders want delete-aware bridge maintenance, registry-backed PIT as-of reads, or non-SQLite optimized PIT/bridge reads, keep those as additive follow-up tickets instead of widening this documentation story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment