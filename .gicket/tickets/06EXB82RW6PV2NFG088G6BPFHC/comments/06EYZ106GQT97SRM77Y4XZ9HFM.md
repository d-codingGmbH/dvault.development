[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06EXB82RW6PV2NFG088G6BPFHC-task-add-ci-workflow-for-build-tests-formatting\u0027 at commit \u0027dcf6bdc5625a\u0027.",
  "implementationReference": {
    "branchName": "ticket/06EXB82RW6PV2NFG088G6BPFHC-task-add-ci-workflow-for-build-tests-formatting",
    "commitSha": "dcf6bdc5625a",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "A CI workflow runs the repository-local validation flow from the repository root using the current baseline commands: \u0060bash tools/check-format.sh\u0060, \u0060dotnet build DVault.slnx\u0060, \u0060dotnet test DVault.slnx\u0060 with the default provider boundary, \u0060dotnet pack DVault.slnx --configuration Release --nologo\u0060, and \u0060bash tools/verify-packages.sh\u0060.",
      "satisfied": true,
      "reason": ".github/workflows/ci.yml adds a validate job that runs bash tools/check-format.sh, dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo --filter \u0022Category!=ProviderIntegration.ExternalOptIn\u0022, dotnet pack DVault.slnx --configuration Release --nologo, and bash tools/verify-packages.sh from the repository root."
    },
    {
      "expectation": "The default workflow completes without external database services or secrets while still running required SQLite integration coverage and default-run provider smoke coverage; external-provider live-database tests run only when explicitly configured or enabled.",
      "satisfied": true,
      "reason": "The workflow leaves DVAULT_TEST_POSTGRES_CONNECTION_STRING empty by default, excludes Category=ProviderIntegration.ExternalOptIn in its test step, and the integration test project only restores Npgsql when that environment variable is set, while tagged SQLite RequiredLocal and DefaultProviderSmoke tests remain in the default run surface."
    },
    {
      "expectation": "The workflow blocks on \u0060bash tools/check-format.sh\u0060, so governed documentation and configuration text, \u0060dotnet format\u0060 verification, and one-member-per-file enforcement are automated rather than manual review steps.",
      "satisfied": true,
      "reason": "The workflow has a dedicated blocking format/docs step before build, test, and pack, and tools/check-format.sh still enforces governed text rules, calls tools/check-one-member-per-file.sh, and performs dotnet format verification against DVault.slnx with a documented constrained-environment fallback."
    },
    {
      "expectation": "The package-validation step fails on missing or unexpected package artifacts, missing symbols or generated XML docs, missing packaged README content, incorrect nuspec metadata, or provider-to-core dependency version drift across the six-packable-package matrix.",
      "satisfied": true,
      "reason": "tools/verify-packages.sh runs the package-verification project, and tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs rejects missing or unexpected artifacts, missing symbols, missing XML docs, missing packaged README content, nuspec metadata drift, and provider-to-core dependency version drift across the six expected packages."
    },
    {
      "expectation": "Failure output identifies the concrete repository-local command or step developers can rerun to reproduce the problem outside CI.",
      "satisfied": true,
      "reason": "The workflow step names include the concrete rerunnable commands, and the package verifier emits repository-local rerun guidance such as running dotnet pack DVault.slnx --configuration Release --nologo before bash tools/verify-packages.sh when bin/packages is missing."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The workflow file and any supporting filters, scripts, or docs updates needed for the CI flow are added consistently with the shared formatting and implementation standards.",
      "satisfied": true,
      "reason": "The branch delta against develop adds the workflow file plus the supporting formatting script and formatting documentation updates, with no extra repository delivery files beyond that CI support surface."
    },
    {
      "expectation": "Repository automation exercises distinct blocking stages for formatting and docs validation, build, tests, pack, and package verification.",
      "satisfied": true,
      "reason": ".github/workflows/ci.yml separates formatting/docs validation, build, test, pack, and package verification into distinct blocking steps."
    },
    {
      "expectation": "Default CI behavior stays within the existing SQLite-required and external-provider-opt-in test contract and does not require live external database infrastructure.",
      "satisfied": true,
      "reason": "Default CI behavior stays on the SQLite-required and external-provider-opt-in boundary through the ExternalOptIn test filter, the empty default Postgres environment variable, and the conditional external-provider package reference."
    },
    {
      "expectation": "Any optional environment switches or configuration needed to enable external-provider jobs are documented where developers or maintainers will discover them.",
      "satisfied": true,
      "reason": "README.md documents the opt-in DVAULT_TEST_POSTGRES_CONNECTION_STRING flow and the default skip behavior for external-provider integration coverage, which keeps the current optional external-provider configuration discoverable."
    }
  ],
  "evidence": [
    "git diff --name-only develop...dcf6bdc5625a shows the repository change set is .github/workflows/ci.yml, docs/formatting.md, and tools/check-format.sh.",
    "git ls-files confirms tracked DVault.slnx, .github/workflows/ci.yml, tools/check-format.sh, tools/verify-packages.sh, and tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs.",
    ".github/workflows/ci.yml defines step names and run lines for bash tools/check-format.sh, dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo --filter \u0022Category!=ProviderIntegration.ExternalOptIn\u0022, dotnet pack DVault.slnx --configuration Release --nologo, and bash tools/verify-packages.sh.",
    "tools/check-format.sh invokes bash tools/check-one-member-per-file.sh and runs dotnet format whitespace DVault.slnx --verify-no-changes --no-restore before its documented folder fallback.",
    "tools/verify-packages.sh shells into tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj for package validation.",
    "tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs validates exact .nupkg and .snupkg counts, rejects unexpected files, checks README and XML doc entries, validates nuspec metadata, and enforces provider dependency alignment with DCoding.Data.DVault.",
    "tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj only includes Npgsql.EntityFrameworkCore.PostgreSQL when $(DVAULT_TEST_POSTGRES_CONNECTION_STRING) is non-empty.",
    "rg over tests/DCoding.Data.DVault.Tests shows actual tests tagged with ProviderIntegration.RequiredLocal, ProviderSmoke.Default, and ProviderIntegration.ExternalOptIn, including SQLite required-local tests and Postgres external opt-in tests.",
    "README.md documents the default local test command with --filter \u0022Category!=ProviderIntegration.ExternalOptIn\u0022 and the opt-in DVAULT_TEST_POSTGRES_CONNECTION_STRING commands for Postgres-backed runs.",
    ".gitignore ignores bin/, the six packable src/*.csproj files write PackageOutputPath to ../../bin/packages/, and the verifier treats bin/packages as generated output rather than tracked repository content.",
    "bash -n tools/check-format.sh tools/verify-packages.sh exited 0 in the read-only review session.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/quality, automation/bot-ready, backlog/initial-dvault, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EXB82RW6PV2NFG088G6BPFHC-task-add-ci-workflow-for-build-tests-formatting\u0027.",
    "Ticket history references implementation commit \u0027dcf6bdc5625a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 2 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Proceed to integrator."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06EXB82RW6PV2NFG088G6BPFHC`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06EXB82RW6PV2NFG088G6BPFHC-task-add-ci-workflow-for-build-tests-formatting' at commit 'dcf6bdc5625a'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06EXB82RW6PV2NFG088G6BPFHC-task-add-ci-workflow-for-build-tests-formatting`
- implementation-commit: `dcf6bdc5625a`
- implementation-pr: `<none>`
- implementation-change: `<none>`