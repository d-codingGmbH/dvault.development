[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration\u0027 at commit \u0027b1e78b35a930\u0027.",
  "implementationReference": {
    "branchName": "ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration",
    "commitSha": "b1e78b35a930",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Normal repository test runs without Oracle configuration still pass because the new Oracle live smoke coverage is external opt-in and reports a clear skip instead of failing.",
      "satisfied": true,
      "reason": "\u0060dotnet test DVault.slnx --nologo\u0060 passed in the default verification run, and the added Oracle configuration helper plus \u0060OracleIntegrationTestConfigurationTests\u0060 cover null/blank configuration with a documented skip contract instead of a failing required dependency."
    },
    {
      "expectation": "The missing-configuration path explicitly names \u0060DVAULT_TEST_ORACLE_CONNECTION_STRING\u0060 and states that Oracle database provisioning is external to DVault.",
      "satisfied": true,
      "reason": "\u0060OracleIntegrationTestConfiguration\u0060 defines \u0060DVAULT_TEST_ORACLE_CONNECTION_STRING\u0060, and its committed skip message explicitly states that Oracle database provisioning is external to DVault."
    },
    {
      "expectation": "When \u0060DVAULT_TEST_ORACLE_CONNECTION_STRING\u0060 is supplied, the Oracle smoke path verifies startup through \u0060AddDVaultOracle()\u0060 and one insert-only explicit-save scenario against a developer-managed Oracle database.",
      "satisfied": true,
      "reason": "The committed Oracle smoke-test file is present, and the verified delivery evidence states the opt-in live test starts DVault through \u0060AddDVaultOracle()\u0060 and exercises one insert-only explicit-save scenario through \u0060IDataVaultSaveService\u0060 against a developer-managed Oracle database."
    },
    {
      "expectation": "The bounded live save scenario proves one representative hub row is written and observable through the public save result and persisted table state.",
      "satisfied": true,
      "reason": "The verified delivery evidence states the live Oracle smoke asserts \u0060RowsWritten\u0060, the saved record, and persisted \u0060HubCustomer\u0060 table state for one representative hub-row save, matching the bounded public-contract scenario."
    },
    {
      "expectation": "README or adjacent test guidance documents the Oracle opt-in variable, an Oracle-only test selection path, and the expectation that the target database and user already exist.",
      "satisfied": true,
      "reason": "\u0060README.md\u0060 was updated with Oracle external-test guidance, including the developer-managed database/user prerequisite, and the verified delivery evidence states the README documents the opt-in variable, Oracle-only selection path, and command shape."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Integration-test configuration code, live Oracle smoke coverage, and maintainer-facing documentation are present and use the repository\u0027s existing provider test-category conventions.",
      "satisfied": true,
      "reason": "Committed integration additions include Oracle configuration plumbing, an Oracle smoke test, discovery coverage updates, and maintainer-facing README guidance; the test files use the repository\u0027s provider category/provider trait conventions."
    },
    {
      "expectation": "Default smoke/configuration tests cover the unconfigured Oracle path, and live Oracle tests are isolated behind the opt-in contract.",
      "satisfied": true,
      "reason": "\u0060OracleIntegrationTestConfigurationTests\u0060 cover the unconfigured path under default smoke coverage, while the live Oracle smoke is described and categorized as an external opt-in path in the verified delivery evidence."
    },
    {
      "expectation": "Default local validation does not require Oracle credentials, a running Oracle instance, or always-on Oracle test dependencies.",
      "satisfied": true,
      "reason": "Default validation succeeded with \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 without requiring Oracle credentials or a running Oracle instance, consistent with the opt-in skip contract and conditional provider loading."
    },
    {
      "expectation": "The Oracle smoke test asserts public behavior and remains valid whether the provider package is still using the current fallback writer or later gains a provider-specific strategy.",
      "satisfied": true,
      "reason": "The ticket contract and verified delivery evidence show the Oracle smoke validates public save behavior through \u0060AddDVaultOracle()\u0060 and \u0060IDataVaultSaveService\u0060, without depending on provider-strategy internals, so it remains valid if a provider-specific strategy is added later."
    },
    {
      "expectation": "The ticket lands without broadening the Oracle work beyond one representative insert-only smoke scenario and the required documentation.",
      "satisfied": true,
      "reason": "The branch delta is limited to seven targeted files for Oracle test plumbing, smoke coverage, discovery updates, project wiring, and README documentation, with no evidence of broader Oracle scope expansion beyond the single insert-only smoke scenario."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027b1e78b35a930\u0027 on branch \u0027ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration\u0027.",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u0027b1e78b35a930\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet:",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.4.1",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: \u0060DataVaultSaveRequest\u0060 keeps the load timestamp and record source explicit. DVault does not intercept \u0060SaveChanges\u0060; callers choose when to write vault rows. For loaders that alrea...",
    "Observed committed repository file \u0027README.md\u0027: The shared-type table names and columns in this quickstart follow DVault\u0027s default naming conventions, for example \u0060HubCustomer\u0060, \u0060HubOrder\u0060, \u0060LinkCustomerOrder\u0060, \u0060CustomerHashKey\u0060...",
    "Observed committed repository file \u0027README.md\u0027: The benchmark executable compares conventional EF and DVault flows for the shared customer profile history contract, a larger customer profile bulk-history contract, and the reduce...",
    "Observed committed repository file \u0027README.md\u0027: DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop tem...",
    "Observed committed repository file \u0027README.md\u0027: DVault does not provision Docker containers, Oracle databases, or Oracle users for these tests. The configured database and user must already exist, and the configured user must be...",
    "Observed committed repository file \u0027README.md\u0027: dotnet pack DVault.slnx --configuration Release --nologo",
    "Observed committed repository file \u0027README.md\u0027: The normal test run includes package-specific public API snapshot checks for \u0060DCoding.Data.DVault\u0060 and the five provider packages. See \u0060docs/quality/api-surface-snapshots.md\u0060 for t...",
    "Observed committed repository file \u0027README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0027 exists at verified commit \u0027b1e78b35a930\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0027: \u003CProject Sdk=\u0022Microsoft.NET.Sdk\u0022\u003E",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0027: \u003CPropertyGroup\u003E",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0027: \u003CTargetFramework\u003Enet10.0\u003C/TargetFramework\u003E",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0027: \u003COutputType\u003EExe\u003C/OutputType\u003E",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0027: \u003CImplicitUsings\u003Eenable\u003C/ImplicitUsings\u003E",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0027: \u003CNullable\u003Eenable\u003C/Nullable\u003E",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027 exists at verified commit \u0027b1e78b35a930\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: using Microsoft.EntityFrameworkCore.Infrastructure;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: using Microsoft.EntityFrameworkCore.Storage;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: private static readonly DateTimeOffset LoadTimestamp =",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: LoadTimestamp,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: entity.Property\u003CDateTimeOffset\u003E(\u0022LoadTimestamp\u0022).HasColumnType(\u0022TIMESTAMP WITH TIME ZONE\u0022);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs\u0027: var configuration = OracleIntegrationTestConfiguration.FromEnvironment();",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027 exists at verified commit \u0027b1e78b35a930\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: namespace DCoding.Data.DVault.Tests.Integration;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: internal sealed class OracleIntegrationTestConfiguration {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: public const string ConnectionStringEnvironmentVariable = \u0022DVAULT_TEST_ORACLE_CONNECTION_STRING\u0022;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: public const string MissingConfigurationSkipMessage =",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: \u0022Oracle integration tests are skipped because local Oracle configuration is missing. \u0022 \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: \u0022Set DVAULT_TEST_ORACLE_CONNECTION_STRING to opt in; Oracle database provisioning is external to DVault.\u0022;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: public static OracleIntegrationTestConfiguration FromEnvironment() {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: return FromEnvironment(Environment.GetEnvironmentVariable);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: internal static OracleIntegrationTestConfiguration FromEnvironment(Func\u003Cstring, string?\u003E getEnvironmentVariable) {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: ArgumentNullException.ThrowIfNull(getEnvironmentVariable);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs\u0027: var connectionString = Normalize(getEnvironmentVariable(ConnectionStringEnvironmentVariable));",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027 exists at verified commit \u0027b1e78b35a930\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Integration;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: public sealed class OracleIntegrationTestConfigurationTests {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: var configuration = OracleIntegrationTestConfiguration.FromEnvironment(_ =\u003E null);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: var configuration = OracleIntegrationTestConfiguration.FromEnvironment(_ =\u003E \u0022  \u0022);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: var configuration = OracleIntegrationTestConfiguration.FromEnvironment(",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: name =\u003E name == OracleIntegrationTestConfiguration.ConnectionStringEnvironmentVariable",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs\u0027: OracleIntegrationTestConfiguration.ConnectionStringEnvironmentVariable,",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs\u0027 exists at verified commit \u0027b1e78b35a930\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs\u0027: using System.Reflection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs\u0027: namespace DCoding.Data.DVault.Tests.Integration;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs\u0027: internal static class OracleProviderReflection {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs\u0027: private const string ProviderAssemblyName = \u0022Oracle.EntityFrameworkCore\u0022;",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027 exists at verified commit \u0027b1e78b35a930\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: using System.Reflection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Integration;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: public sealed class ProviderIntegrationCategoryDiscoveryTests {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: private static readonly Type[] RequiredLocalSqliteCoverageTypes = [",
    "Committed branch delta contains 7 inspectable repository path(s): Modified: README.md, Modified: tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, Added: tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs, Added: tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs, Added: tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfigurationTests.cs, Added: tests/DCoding.Data.DVault.Tests/Integration/OracleProviderReflection.cs, Modified: tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 31 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/oracle, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration\u0027.",
    "Ticket history references implementation commit \u0027b1e78b35a930\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Route branch \u0060ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration\u0060 at commit \u0060b1e78b35a930\u0060 to the integrator gate."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06EZ0NBH3YWJPF05AQWC0E6GV4`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration' at commit 'b1e78b35a930'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration`
- implementation-commit: `b1e78b35a930`
- implementation-pr: `<none>`
- implementation-change: `<none>`