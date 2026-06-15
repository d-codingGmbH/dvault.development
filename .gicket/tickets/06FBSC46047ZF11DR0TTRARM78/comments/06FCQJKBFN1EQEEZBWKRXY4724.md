[gicket-bot] PO-critic review contract

Summary
- Delivery contract is concrete, bounded to the existing benchmark and evidence surface, and has no open questions; repository evidence cleanly explains the current DB2 gap and the expected documentation and verifier outcomes for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSC46047ZF11DR0TTRARM78/description.md:30-53 defines six acceptance criteria, four Definition of Done items, and Open Questions = none.
- The ticket comment files under .gicket/tickets/06FBSC46047ZF11DR0TTRARM78/comments are bot lease, handoff, and refinement records; no substantive human discussion was found.
- git -C /mnt/c/Projects/DVault log --oneline --max-count=5 shows only PO and PO-critic workflow commits on the ticket branch, and git -C /mnt/c/Projects/DVault diff --stat d25fd6cb8df3ae6d58c6046f56d5503d33aa7358..HEAD returned no diff, confirming this is still a pre-development review state.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExternalProviderDefinitions.cs:6-37 defines only Postgres, SQL Server, MySQL, and Oracle external providers; there is no DB2 external provider definition yet.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs:38-45 and 70-75 build optionalProviders and provider strategy mappings for four external providers only, and lines 156-176 list valid provider help text as all, sqlite, postgres, sqlserver, mysql, and oracle only.
- benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj:13-29 has conditional package and project references for SQL Server, PostgreSQL, Oracle, and MySQL, but no IBM DB2 benchmark dependency or DB2 project reference.
- benchmark-summary.json:17-42 lists exactly four optional providers (PostgreSQL, SQL Server, MySQL, Oracle), and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:544-565 asserts optionalProviders.Length == 4.
- docs/local-validation.md:65-71 already documents the DB2 external opt-in test lane via DVAULT_TEST_DB2_CONNECTION_STRING, and tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs:12-29 shows dedicated DB2 external smoke and integration coverage exists today.
- docs/architecture/dvault-v1-explicit-save-service.md:103-105 still states that no DB2 benchmark lane exists in the current harness, which matches the contract's documentation-alignment problem statement.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly call out the existing optional-provider dependency-unavailable skip branch for cases where the DB2 package was not restored before run; this is a small clarity gap, but the implementation notes already point back to the shared optional-provider contract.

Risky assumptions
- The ticket assumes DB2 should reuse the current optional-provider artifact contract exactly, including deterministic skipped rows and unchanged triplet schema and file names.
- The ticket assumes initial developer validation may be limited to skipped-placeholder behavior when no reachable DB2 instance is available, and that completed DB2 timing evidence is not required for this handoff.
- The ticket assumes the outgoing blocks relation to 06FBSC4BEBGSVVTJSQXM1Z74CC is downstream coordination only and not an input dependency for this task.

AC / test suggestions
- Add an explicit verification point that --provider db2 appears in benchmark usage and help text and selects the DB2 lane without widening the provider vocabulary beyond the existing pattern.
- Verify the root artifact triplet and verifier coverage move from four to five optional-provider entries, and that DB2 skipped rows preserve deterministic executionStatus, skipReason, iterations = 0, blank or null metrics, and persistedOutcome = not executed.
- Verify DB2 read-row expectations keep latest-satellite provider-neutral while PIT and bridge rows can carry the planned or selected Db2DataVaultReadStrategy tokens defined by the existing capability boundary.

Implementation watchouts
- This ticket is bounded to the existing benchmark and evidence contract; it should not expand into DB2-specific artifact filenames, schema changes, provisioning promises, or a DB2 latest-satellite optimization claim.
- Documentation updates need a full contradiction sweep because current public docs already say DB2 has no benchmark lane; landing only README or only one canonical doc would leave public guidance inconsistent.
- DB2 benchmark enablement depends on the same opt-in and conditional dependency posture as other external providers, plus a DB2 temp external-database path; missing either would produce documentation that outruns the executable harness.

Non-blocking notes
- The persisted delivery contract is concrete for a pre-development task: it names the missing repository surfaces, the non-goals, and the bounded DB2 capability posture.
- The branch contains no implementation diff yet, which is consistent with a PO-critic gate on a pre-development ticket and not a blocker by itself.
- The ticket's comment history is operational and bot-generated rather than substantive product discussion, so repository files are the main evidence surface for this review.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment