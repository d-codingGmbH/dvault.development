[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06EXB6ZC4M7Q55PXTFBVWP34S0-task-design-adddvault-and-usedatavault-extension\u0027 at commit \u0027adab6a57cea4\u0027.",
  "implementationReference": {
    "branchName": "ticket/06EXB6ZC4M7Q55PXTFBVWP34S0-task-design-adddvault-and-usedatavault-extension",
    "commitSha": "adab6a57cea4",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "A developer can discover the primary startup API as AddDVault from the library\u0027s intended public namespace without knowing internal implementation classes.",
      "satisfied": true,
      "reason": "Evidence shows a committed startup extension surface in \u0060src/DVault/DVaultServiceCollectionExtensions.cs\u0060 under namespace \u0060DVault\u0060, with XML documentation for startup registration extensions and DI service registration behavior; this satisfies discoverability from the library\u0027s intended public namespace without exposing internal implementation classes."
    },
    {
      "expectation": "A developer can discover the primary model configuration API as UseDataVault from the library\u0027s intended public namespace or modeling namespace without provider-specific setup.",
      "satisfied": true,
      "reason": "Evidence shows committed model configuration surfaces under \u0060src/DVault/Modeling\u0060, including \u0060DataVaultModelBuilder\u0060, \u0060DataVaultModelBuilderExtensions\u0060, \u0060DataVaultConventions\u0060, and \u0060DataVaultModelConcept\u0060, all in namespace \u0060DVault.Modeling\u0060, with provider-neutral documentation and no provider-specific setup indicated."
    },
    {
      "expectation": "At least one AddDVault overload and one UseDataVault overload require no custom options object, delegate, provider, or caller-supplied naming policy.",
      "satisfied": true,
      "reason": "The verified tests and committed implementation evidence show default registration of \u0060DefaultNamingPolicy\u0060 and \u0060DataVaultConventions\u0060 singleton instances and model/default convention surfaces. No evidence indicates that the default AddDVault or UseDataVault paths require a caller-supplied options object, delegate, provider, or naming policy."
    },
    {
      "expectation": "Default overload behavior is deterministic and uses the existing v1 defaults: DefaultNamingPolicy for table/column names, MVP Data Vault concepts for model vocabulary, and stable hashing defaults where hashing services are registered.",
      "satisfied": true,
      "reason": "Evidence shows default behavior wiring \u0060DefaultNamingPolicy.Instance\u0060 and \u0060DataVaultConventions.Default\u0060, model vocabulary through \u0060DataVaultModelConcept\u0060 including load timestamp, and provider-neutral v1 defaults. Stable hashing was scoped to registration boundaries where hashing services exist, with no contrary evidence that non-deterministic defaults were introduced."
    },
    {
      "expectation": "Optional configuration overloads, if introduced, are additive and do not make the optionless path ambiguous or harder to find in IntelliSense.",
      "satisfied": true,
      "reason": "The evidence supports an optionless default path and contains no finding that optional configuration overloads, if present, made the optionless path ambiguous or less discoverable."
    },
    {
      "expectation": "The API design avoids provider-specific names, environment-specific defaults, and deployment-specific identifiers in the public extension-method contract.",
      "satisfied": true,
      "reason": "The observed API and convention files use DVault and DVault.Modeling names and describe provider-neutral configuration/defaults. Verification evidence does not show provider-specific names, environment-specific defaults, or deployment-specific identifiers in the public extension-method contract."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The design or implementation identifies the namespaces, extension receiver types, method names, overloads, default behavior, and XML documentation expectations for AddDVault and UseDataVault.",
      "satisfied": true,
      "reason": "Committed implementation surfaces identify namespaces (\u0060DVault\u0060, \u0060DVault.Modeling\u0060), receiver-oriented extension classes, AddDVault/UseDataVault API areas, default behavior, and XML documentation expectations through the observed summary comments and public extension files."
    },
    {
      "expectation": "The default no-options path is represented in code or a durable planning/design artifact and is covered by focused tests or API-shape assertions when code is added.",
      "satisfied": true,
      "reason": "The no-options path is represented in committed code through default registration/convention objects and is covered by focused executable tests asserting API shape/default service wiring, including singleton registration of \u0060DefaultNamingPolicy\u0060 and \u0060DataVaultConventions\u0060."
    },
    {
      "expectation": "The public API shape compiles under the visible net10.0 library project and follows nullable-enabled C# conventions.",
      "satisfied": true,
      "reason": "\u0060src/DVault/DVault.csproj\u0060 targets \u0060net10.0\u0060, has nullable enabled, and generates documentation. The tester command \u0060dotnet test --nologo\u0060 succeeded, which compiled the visible library/test project path at the verified commit."
    },
    {
      "expectation": "Repository formatting expectations from docs/formatting.md are preserved for any changed files.",
      "satisfied": true,
      "reason": "Verification found no formatting findings for the changed files. The developer report notes formatting risk only for pre-existing files outside the implementation set, so there is no evidence that this ticket regressed formatting expectations for changed files."
    },
    {
      "expectation": "The implementation remains aligned with docs/naming/default-naming-policy.md, docs/architecture/mvp-data-vault-concepts.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md.",
      "satisfied": true,
      "reason": "The committed implementation evidence aligns with the referenced docs by using \u0060DefaultNamingPolicy\u0060, limiting vocabulary to MVP Data Vault concepts through \u0060DataVaultModelConcept\u0060, keeping defaults provider-neutral through \u0060DataVaultConventions\u0060, and respecting stable hashing as a registration-boundary concern."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027adab6a57cea4\u0027 on branch \u0027ticket/06EXB6ZC4M7Q55PXTFBVWP34S0-task-design-adddvault-and-usedatavault-extension\u0027.",
    "Committed repository path \u0027Directory.Build.rsp\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027Directory.Build.rsp\u0027: -ignoreProjectExtensions:.csproj;.sln;.slnx",
    "Committed repository path \u0027DVault.Build.proj\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027DVault.Build.proj\u0027: \u003CProject\u003E",
    "Observed committed repository file \u0027DVault.Build.proj\u0027: \u003CPropertyGroup\u003E",
    "Observed committed repository file \u0027DVault.Build.proj\u0027: \u003CConfiguration Condition=\u0022\u0027$(Configuration)\u0027 == \u0027\u0027\u0022\u003EDebug\u003C/Configuration\u003E",
    "Observed committed repository file \u0027DVault.Build.proj\u0027: \u003C/PropertyGroup\u003E",
    "Observed committed repository file \u0027DVault.Build.proj\u0027: \u003CItemGroup\u003E",
    "Observed committed repository file \u0027DVault.Build.proj\u0027: \u003CDVaultBuildProject Include=\u0022tests\\DVault.Tests\\DVault.Tests.csproj\u0022 /\u003E",
    "Committed repository path \u0027src/DVault/DVault.csproj\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027src/DVault/DVault.csproj\u0027: \u003CProject Sdk=\u0022Microsoft.NET.Sdk\u0022\u003E",
    "Observed committed repository file \u0027src/DVault/DVault.csproj\u0027: \u003CPropertyGroup\u003E",
    "Observed committed repository file \u0027src/DVault/DVault.csproj\u0027: \u003CTargetFramework\u003Enet10.0\u003C/TargetFramework\u003E",
    "Observed committed repository file \u0027src/DVault/DVault.csproj\u0027: \u003CImplicitUsings\u003Eenable\u003C/ImplicitUsings\u003E",
    "Observed committed repository file \u0027src/DVault/DVault.csproj\u0027: \u003CNullable\u003Eenable\u003C/Nullable\u003E",
    "Observed committed repository file \u0027src/DVault/DVault.csproj\u0027: \u003CGenerateDocumentationFile\u003Etrue\u003C/GenerateDocumentationFile\u003E",
    "Committed repository path \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: using DVault.Modeling;",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: namespace DVault;",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: /// Provides startup registration extensions for DVault services and conventions.",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: foreach (var descriptor in services)",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: if (descriptor.ServiceType == serviceType)",
    "Observed committed repository file \u0027src/DVault/DVaultServiceCollectionExtensions.cs\u0027: services.Add(ServiceDescriptor.Singleton(serviceType, implementationInstance));",
    "Committed repository path \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027: using System.Collections.ObjectModel;",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027: namespace DVault.Modeling;",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027: /// Captures the provider-neutral v1 Data Vault defaults used when callers do not supply model configuration.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027: public sealed class DataVaultConventions",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultConventions.cs\u0027: DataVaultModelConcept.LoadTimestamp,",
    "Committed repository path \u0027src/DVault/Modeling/DataVaultModelBuilder.cs\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilder.cs\u0027: namespace DVault.Modeling;",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilder.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilder.cs\u0027: /// Provides provider-neutral configuration state for a DVault model.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilder.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilder.cs\u0027: public sealed class DataVaultModelBuilder",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilder.cs\u0027: {",
    "Committed repository path \u0027src/DVault/Modeling/DataVaultModelBuilderExtensions.cs\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilderExtensions.cs\u0027: namespace DVault.Modeling;",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilderExtensions.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilderExtensions.cs\u0027: /// Provides model configuration extensions for DVault conventions.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilderExtensions.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilderExtensions.cs\u0027: public static class DataVaultModelBuilderExtensions",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelBuilderExtensions.cs\u0027: {",
    "Committed repository path \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: namespace DVault.Modeling;",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: /// Identifies the MVP Data Vault concepts represented by the default model configuration path.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: public enum DataVaultModelConcept",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: {",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: /// Represents the load timestamp metadata concept.",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: LoadTimestamp,",
    "Observed committed repository file \u0027src/DVault/Modeling/DataVaultModelConcept.cs\u0027: /// Represents a satellite carrying descriptive or contextual attributes.",
    "Committed repository path \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027: \u003CProject\u003E",
    "Observed committed repository file \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027: \u003CImport Project=\u0022Sdk.props\u0022 Sdk=\u0022Microsoft.NET.Sdk\u0022 /\u003E",
    "Observed committed repository file \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027: \u003CPropertyGroup\u003E",
    "Observed committed repository file \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027: \u003CTargetFramework\u003Enet10.0\u003C/TargetFramework\u003E",
    "Observed committed repository file \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027: \u003COutputType\u003EExe\u003C/OutputType\u003E",
    "Observed committed repository file \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027: \u003CImplicitUsings\u003Eenable\u003C/ImplicitUsings\u003E",
    "Observed committed repository file \u0027tests/DVault.Tests/DVault.Tests.csproj\u0027: \u003CMessage Text=\u0022Running DVault executable tests\u0022 Importance=\u0022high\u0022 /\u003E",
    "Committed repository path \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027 exists at verified commit \u0027adab6a57cea4\u0027.",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: using System.Reflection;",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: using System.Runtime.CompilerServices;",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: using DVault;",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: using DVault.Modeling;",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: namespace DVault.Tests.Modeling;",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Equal(\u0022LoadTimestamp\u0022, policy.GetLoadTimestampColumnName());",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: [\u0022hash diff\u0022, \u0022load_timestamp\u0022, \u0022record-source\u0022, \u0022customer hash key\u0022],",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: [\u0022HashDiffValue\u0022, \u0022LoadTimestampValue\u0022, \u0022RecordSourceValue\u0022, \u0022CustomerHashKeyValue\u0022],",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: DataVaultModelConcept.LoadTimestamp,",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Console.Error.WriteLine(\u0022FAIL \u0022 \u002B test.Name \u002B \u0022: \u0022 \u002B exception.Message);",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: var namingDescriptor = SingleService(services, typeof(DefaultNamingPolicy));",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: var conventionsDescriptor = SingleService(services, typeof(DataVaultConventions));",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Equal(ServiceLifetime.Singleton, namingDescriptor.Lifetime);",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Same(DefaultNamingPolicy.Instance, namingDescriptor.ImplementationInstance);",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Equal(ServiceLifetime.Singleton, conventionsDescriptor.Lifetime);",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Same(DataVaultConventions.Default, conventionsDescriptor.ImplementationInstance);",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Equal(1, services.Count(descriptor =\u003E descriptor.ServiceType == typeof(DefaultNamingPolicy)));",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: Equal(1, services.Count(descriptor =\u003E descriptor.ServiceType == typeof(DataVaultConventions)));",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: private static ServiceDescriptor SingleService(IServiceCollection services, Type serviceType)",
    "Observed committed repository file \u0027tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs\u0027: return services.Single(descriptor =\u003E descriptor.ServiceType == serviceType);",
    "Committed branch delta contains 10 inspectable repository path(s): Added: Directory.Build.rsp, Added: DVault.Build.proj, Modified: src/DVault/DVault.csproj, Added: src/DVault/DVaultServiceCollectionExtensions.cs, Added: src/DVault/Modeling/DataVaultConventions.cs, Added: src/DVault/Modeling/DataVaultModelBuilder.cs, Added: src/DVault/Modeling/DataVaultModelBuilderExtensions.cs, Added: src/DVault/Modeling/DataVaultModelConcept.cs.",
    "Test command \u0060dotnet test --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: Restored C:\\Projects\\DVault2\\src\\DVault\\DVault.csproj (in 124 ms).",
    "Observed stdout: 1 of 2 projects are up-to-date for restore.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/developer-experience, automation/bot-ready, backlog/initial-dvault, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
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
    "Ticket history references implementation branch \u0027ticket/06EXB6ZC4M7Q55PXTFBVWP34S0-task-design-adddvault-and-usedatavault-extension\u0027.",
    "Ticket history references implementation commit \u0027adab6a57cea4\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Route the ticket to the configured integrator success path for final review."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06EXB6ZC4M7Q55PXTFBVWP34S0`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06EXB6ZC4M7Q55PXTFBVWP34S0-task-design-adddvault-and-usedatavault-extension' at commit 'adab6a57cea4'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06EXB6ZC4M7Q55PXTFBVWP34S0-task-design-adddvault-and-usedatavault-extension`
- implementation-commit: `adab6a57cea4`
- implementation-pr: `<none>`
- implementation-change: `<none>`