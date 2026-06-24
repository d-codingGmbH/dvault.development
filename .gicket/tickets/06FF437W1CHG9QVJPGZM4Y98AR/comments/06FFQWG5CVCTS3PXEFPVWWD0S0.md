[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c\u0027 at commit \u0027ca31eff4e1d5\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c",
    "commitSha": "ca31eff4e1d5",
    "pullRequestReference": null,
    "changeReference": null,
    "branchOwnerProvenance": {
      "ticketId": "06FF437W1CHG9QVJPGZM4Y98AR",
      "ownerBranch": "ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c",
      "sourceCommitSha": "ca31eff4e1d5",
      "baseBranch": "develop",
      "producingRole": "test",
      "producingRunId": "7f69a393384d4f79ad66dc21c207d64b",
      "producingInstanceId": "hp-ai-2026-001.1"
    }
  },
  "acceptanceCriteria": [
    {
      "expectation": "The contract states that pit-full-rebuild-maintenance is the only PIT maintenance timing scenario and explicitly rejects pit-as-of-read and bridge-traversal-read as maintenance timing proof.",
      "satisfied": true,
      "reason": "The evidence matrix and benchmark artifact contract both state that pit-full-rebuild-maintenance is the only PIT maintenance timing scenario and explicitly exclude pit-as-of-read and bridge-traversal-read as maintenance proof."
    },
    {
      "expectation": "The contract records the supported provider-maintenance lanes exactly as the repository proves them: PostgreSQL clean ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active full rebuilds; SQL Server clean ordinary hub-parent full rebuilds only; MySQL clean ordinary hub-parent full rebuilds only on MySql.EntityFrameworkCore; Oracle deferred; DB2 future-only and provider-neutral today.",
      "satisfied": true,
      "reason": "Performance profiles, release notes, service-registration evidence, and root PIT maintenance placeholder rows align on the exact accepted lanes: PostgreSQL ordinary/shared-driving-key multi-active/link-parent non-multi-active full rebuilds, SQL Server clean ordinary hub-parent only, MySQL clean ordinary hub-parent only on MySql.EntityFrameworkCore, Oracle deferred, and DB2 provider-neutral today with a future-only lane."
    },
    {
      "expectation": "The contract requires maintenance evidence to cite the artifact triplet, run context, provider, baseline or comparator, maintenanceScope=FullRebuild, selected or planned maintenance strategy, execution path or fallback posture, and bounded fallback causes.",
      "satisfied": true,
      "reason": "The benchmark artifact contract and evidence matrix require the benchmark-summary.md/benchmark-summary.csv/benchmark-summary.json triplet, run context, provider, baseline or comparator, maintenanceScope=FullRebuild, selected strategy or fallback posture, execution path, and bounded fallback causes for PIT maintenance claims."
    },
    {
      "expectation": "The contract points to the bounded fallback vocabularies already proved in code and tests: DataVaultPitMaintenanceStrategyFallbackCauseKind for the shared strategy seam and SqlServerPitMaintenanceFallbackCauseKind for the SQL Server service-replacement lane.",
      "satisfied": true,
      "reason": "The evidence matrix explicitly names DataVaultPitMaintenanceStrategyFallbackCauseKind and SqlServerPitMaintenanceFallbackCauseKind as the bounded fallback vocabularies, and both enums are present in the inspected source."
    },
    {
      "expectation": "The contract keeps bridge maintenance, automatic scheduling, parent-maintenance push-down expansion, and unsupported PIT shapes outside this ticket\u0027s accepted claim surface.",
      "satisfied": true,
      "reason": "The contract surfaces keep bridge maintenance push-down, automatic scheduling or read-time refresh, parent-maintenance expansion, and unsupported PIT shapes outside the accepted claim surface."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The canonical evidence and documentation surfaces all describe the same maintenance boundary without promoting read rows or source/test-backed availability into benchmark-backed maintenance wins.",
      "satisfied": true,
      "reason": "The evidence matrix, benchmark artifact contract, performance profiles, and v0.47 release notes all describe the same maintenance boundary and explicitly prevent read rows or source/test-backed availability from being promoted into benchmark-backed maintenance wins."
    },
    {
      "expectation": "BenchmarkScenarioExecutionTests and the evidence matrix remain aligned on provider-evidence manifest mapping for PIT maintenance rows, including workloadShape=pit-full-rebuild-maintenance and readShape=null.",
      "satisfied": true,
      "reason": "BenchmarkScenarioExecutionTests asserts PIT maintenance manifest rows with scenario/workloadShape pit-full-rebuild-maintenance and readShape=null, matching the evidence matrix and the preserved root placeholder artifact rows."
    },
    {
      "expectation": "Service registration and capability tests continue to prove the accepted provider boundary: PostgreSQL and MySQL register IDataVaultProviderPitMaintenanceStrategy, SQL Server replaces IDataVaultPitMaintenanceService, and Oracle and DB2 do not register PIT maintenance today.",
      "satisfied": true,
      "reason": "Inspected service registrations match the accepted provider boundary: PostgreSQL and MySQL register IDataVaultProviderPitMaintenanceStrategy, SQL Server replaces IDataVaultPitMaintenanceService, Oracle and DB2 add no PIT maintenance registration, and deterministic verification reported dotnet test DVault.slnx --nologo success."
    },
    {
      "expectation": "The parent story relation state remains consistent with delivery: the existing child tickets stay done and no active blocks relations remain on the parent.",
      "satisfied": true,
      "reason": "All 13 related child tickets inspected under .gicket/tickets/*/ticket.json are in status done, and the parent event pack records matching TicketRelationRemoved events for each previously added blocks relation, so no active parent blocks relations remain."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027ca31eff4e1d5\u0027 on branch \u0027ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c\u0027.",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: # Performance Evidence And Benchmark Artifact Contract",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Status: v1 contract",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Ticket: \u006006F492BZPP5YT9SJSPDHQBGF3R\u0060",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: ## Purpose",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: This document is the shared DVault performance-evidence contract. Performance tuning, release-note, and documentation work must reuse this contract instead of inventing ticket-spec...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: The current benchmark harness remains the v1 baseline. It is extended by contract, documentation, and artifact-field tests rather than replaced.",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: The before and after sets must use the same scenario mode, provider filter, iteration count, warmup count, load-timestamp storage setting, and provider configuration unless the cla...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - load-timestamp storage",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - optional provider connection-string environment variable names",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Every persisted benchmark evidence set must contain these files from one benchmark execution:",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Before/after evidence must store two comparable artifact sets under one explicit scenario, ticket, or release label. For example:",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - OS description",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - .NET runtime description",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - .NET runtime version",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: PIT full-rebuild maintenance timing rows use the same row fields and must use scenario \u0060pit-full-rebuild-maintenance\u0060. They must not reuse \u0060pit-as-of-read\u0060 or \u0060bridge-traversal-rea...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: ## Provider Evidence Manifest Alignment",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: The provider-evidence manifest row contract is documented in [Provider Optimization Evidence Matrix](provider-optimization-evidence-matrix.md) with schema version \u0060dvault.provider-...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: When a benchmark-backed provider-evidence manifest row is populated from the artifact triplet, map the existing row identity fields directly: \u0060scenarioName\u0060, \u0060provider\u0060, \u0060baselineN...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Completed benchmark rows map to manifest \u0060evidencePosture=completed-timing\u0060 only when they keep their artifact triplet and run context. Skipped optional-provider rows map to \u0060evide...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: PIT full-rebuild maintenance rows map to the same provider-evidence manifest with \u0060workloadShape=pit-full-rebuild-maintenance\u0060 and \u0060readShape=null\u0060. Provider-specific completed row...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: The required local baseline is SQLite temporary files. A standard local evidence set must include:",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Streaming-save evidence must reuse the artifact fields above. The materialized, synchronous chunked, and async-source rows must use the same logical explicit save requests and comp...",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: When the claim depends on scale behavior, include the scale matrix mode. When the claim depends on latest-satellite lookup/index behavior, include the latest-index matrix mode.",
    "Observed hinted repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: When the claim compares stable-hash algorithm width or physical hash-key storage profile, keep supplemental footprint sidecars beside the same benchmark artifact triplet under the ...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: # Provider Optimization Evidence Matrix",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: Status: v1 evidence contract",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: Ticket: 06FBSC3N7ZFVQW3AV2JJ8T7Q7W",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: ## Purpose",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: This document is the canonical lookup surface for DVault provider optimization evidence rows. Later tickets should cite these matrix rows by scenario, provider, baseline, and evide...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: The matrix reuses the existing benchmark artifact contract vocabulary from [Performance Evidence And Benchmark Artifact Contract](performance-evidence-benchmark-artifact-contract.m...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: ## Evidence Postures",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: | \u0060completed-timing\u0060 | A checked-in benchmark row completed and may support a timing claim only with its artifact triplet and run context. |",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: | \u0060skipped-placeholder\u0060 | A checked-in optional-provider row is present with \u0060executionStatus=skipped\u0060, a skip reason, \u0060iterations=0\u0060, blank or null metrics, deterministic executio...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: | \u0060diagnostics-only\u0060 | Repository diagnostics, capability profiles, and provider registration prove a bounded strategy candidate or fallback condition, but no benchmark timing row ...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: | \u0060storage-footprint\u0060 | Hash-key storage sidecars record physical storage footprint facts and scoped benchmark rows for hash-key variants. They are not successful provider timing c...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: For the v0.42 provider performance evidence and tuning baseline, downstream work must apply these gates before promoting a provider claim:",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - Treat only \u0060completed-timing\u0060 rows with a preserved provider-configured artifact triplet and run context as measured timing evidence.",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - Keep \u0060skipped-placeholder\u0060, \u0060diagnostics-only\u0060, \u0060smoke-only\u0060, and \u0060storage-footprint\u0060 rows out of measured timing claims.",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - Treat \u0060pit-full-rebuild-maintenance\u0060 rows as the only PIT full-rebuild maintenance timing row family. Do not promote \u0060pit-as-of-read\u0060 or \u0060bridge-traversal-read\u0060 rows into mainten...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - Require every completed PIT maintenance timing claim to cite the scenario, provider, baseline or comparator, selected provider strategy or provider-neutral fallback posture, boun...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - Fall back to provider-neutral save or read behavior when the matching \u0060DVAULT_TEST_*\u0060 connection string is unset, provider diagnostics do not select the expected strategy, the pr...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - Do not promote maintained-bridge read rows into write-side bridge-maintenance push-down claims. Bridge maintenance push-down needs its own source seam, diagnostics vocabulary, pa...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: | Oracle | Direct optimized batching starts at 50-plus total operations and no more than 10000 satellite operations; read commands prefetch LOB payloads. | Closed with the 2026-06-...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: | DB2 | Clean-context set-based save with the measured 1000-row command cap. | Closed with the 2026-06-23 configured DB2 closure rows; do not claim staged DB2 bulk, provider-native...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - v0.32.0 smoke-read provider-configured PIT/bridge evidence for PostgreSQL, SQL Server, MySQL, and Oracle: [benchmark-summary.md](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVA...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - MySQL latest-satellite provider-configured evidence for ticket \u006006FE4QQ9VF7B74E60CXEHSS5XW\u0060: [benchmark-summary.md](../../artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-la...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - SQL Server bulk-threshold provider-configured evidence: [sqlserver-threshold-decision.md](../../sqlserver-threshold-decision.md), [benchmark-summary.md](../../artifacts/benchmark...",
    "Observed hinted repository file \u0027docs/plans/provider-optimization-evidence-matrix.md\u0027: - DB2 hotspot provider-configured evidence for clean-context save plus supported latest-satellite/PIT/bridge reads: [benchmark-summary.md](../../artifacts/benchmarks/06FE4QR3DD7EFZ...",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: Restored C:\\Projects\\DVault\\src\\DCoding.Data\\DCoding.Data.csproj (in 108 ms).",
    "Observed stdout: Restored C:\\Projects\\DVault\\tools\\DCoding.Data.DVault.PackageVerification\\DCoding.Data.DVault.PackageVerification.csproj (in 108 ms).",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 704 C# files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/performance, area/provider-support, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027develop\u0027.",
    "Ticket history references implementation commit \u0027d890851ec8a3\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Latest developer delivery outcome declares \u0027no_repository_change_required\u0027.",
    "Developer delivery outcome reason: The ticket contract names concrete repository validation paths, and the current branch already satisfies them. Creating a documentation-only repository artifact would duplicate the existing canonical surfaces rather than implementing missing contract content..",
    "Developer delivery outcome targets role \u0027test\u0027.",
    "Observed behavior: developer explicitly documented a ticket-only or external outcome that does not require a new repository diff.",
    "Developer delivery evidence: Branch inspection: git rev-parse --abbrev-ref HEAD returned ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c; git rev-parse --short HEAD returned ddc7a7f2a.",
    "Developer delivery evidence: docs/plans/provider-optimization-evidence-matrix.md lines 10, 29, 140, and 340-349 define pit-full-rebuild-maintenance as the maintenance row family, keep PIT/bridge read rows out of maintenance timing, require workloadShape=pit-full-rebuild-maintenance and readShape=null, and list provider-neutral/PostgreSQL/SQL Server/MySQL maintenance contract rows and boundaries.",
    "Developer delivery evidence: docs/plans/performance-evidence-benchmark-artifact-contract.md lines 78 and 88 require the benchmark-summary.md/csv/json triplet, run context, maintenanceScope=FullRebuild, selected strategy or fallback posture, bounded fallback causes, workloadShape=pit-full-rebuild-maintenance, and readShape=null.",
    "Developer delivery evidence: benchmark-summary.md lines 69-70 and 76-77 plus benchmark-summary.json lines 708, 727, 841, and 860 preserve pit-full-rebuild-maintenance rows with maintenanceScope=FullRebuild for PostgreSQL and SQL Server root skipped placeholders.",
    "Developer delivery evidence: docs/performance-profiles.md lines 67-84 and docs/releases/v0.47.0.md lines 17-38 align the accepted provider boundary and state that read rows are not provider maintenance timing evidence.",
    "Developer delivery evidence: Fresh grep of service collection extensions returned Postgres registration at src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:27, MySQL registration at src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:31, and SQL Server replacement at src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:22; Oracle and DB2 extension files were included and had no PIT maintenance registration hits.",
    "Developer delivery evidence: dotnet test DVault.slnx --nologo --filter FullyQualifiedName~BenchmarkScenarioExecutionTests exited 0. Integration summaries passed for net8.0 and net10.0; unit summaries passed for net8.0 and net10.0.",
    "Developer delivery evidence: bash tools/check-format.sh exited 0: one-member-per-file check passed for 704 C# files and formatting check passed.",
    "Developer delivery evidence: The ticket contract does not expose explicit repository-relative validation paths, so the existing branch state must be verified by tester evidence rather than developer workspace path validation.",
    "Developer verification hint: Inspect docs/plans/provider-optimization-evidence-matrix.md for pit-full-rebuild-maintenance, workloadShape=pit-full-rebuild-maintenance, readShape=null, and the PostgreSQL/SQL Server/MySQL maintenance rows.",
    "Developer verification hint: Inspect docs/plans/performance-evidence-benchmark-artifact-contract.md for the completed maintenance claim requirements and provider-evidence manifest mapping.",
    "Developer verification hint: Inspect benchmark-summary.md and benchmark-summary.json for root pit-full-rebuild-maintenance placeholder rows and maintenanceScope=FullRebuild execution details.",
    "Developer verification hint: Run dotnet test DVault.slnx --nologo --filter FullyQualifiedName~BenchmarkScenarioExecutionTests.",
    "Developer verification hint: Run bash tools/check-format.sh.",
    "Developer verification hint: Treat this handoff as branch-state verification: confirm the current ticket branch already satisfies the contract using repository, branch, commit, and ticket evidence."
  ],
  "findings": [
    "Committed branch delta against base branch \u0027develop\u0027 did not contain non-ticket repository paths to inspect.",
    "Developer verification hint references repository path \u0027PostgreSQL/SQL\u0027, but that path is absent from the verified committed repository state.",
    "Developer verification hint references repository path \u0027Server/MySQL\u0027, but that path is absent from the verified committed repository state.",
    "Developer verification hint references repository path \u0027tools/check-format.sh.\u0027, but that path is absent from the verified committed repository state."
  ],
  "nextSteps": [
    "Hand off to integrator for the final accept/rework decision; current tester evidence does not indicate developer rework."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FF437W1CHG9QVJPGZM4Y98AR`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c' at commit 'ca31eff4e1d5'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c`
- implementation-commit: `ca31eff4e1d5`
- implementation-pr: `<none>`
- implementation-change: `<none>`