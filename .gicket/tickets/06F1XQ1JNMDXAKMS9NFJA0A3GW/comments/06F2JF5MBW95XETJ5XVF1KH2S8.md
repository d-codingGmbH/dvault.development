[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F1XQ1JNMDXAKMS9NFJA0A3GW\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests\u0027 and commit \u00272196be4e2e6e\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests\u0027 from source \u00272196be4e2e6e\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests\u0027.",
    "Evidence: git branch --show-current returned ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests.",
    "Evidence: git log --first-parent shows 2196be4e2e6e as a lease-claim metadata commit and later handoff/test metadata commits; git diff --name-only 2196be4e2e6e..HEAD over src/tests/DVault.slnx/Directory.Build.props/README/docs/examples returned no output, so the reviewed branch tip has the same relevant tree as the claimed source ref.",
    "Evidence: git diff --name-status develop..HEAD over repo implementation paths lists new src/DCoding.Data.DVault.Analyzers files, new tests/DCoding.Data.DVault.Tests/Analyzers files, DVault.slnx, and Directory.Build.props.",
    "Evidence: git diff --name-status develop..HEAD also lists M README.md, D docs/production-adoption-checklist.md, D examples/DCoding.Data.DVault.PostgresQuickstart/README.md, and M examples/README.md; git diff --stat for those files shows 246 deletions.",
    "Evidence: DataVaultCodeFirstAnalyzer.cs declares SupportedDiagnostics for CodeFirstDiagnosticCatalog.UnsupportedSelector and DuplicateMember, registers invocation and lambda syntax analysis, and reports DMV1901/DMV1902 diagnostics for BusinessKey, Payload, and DrivingKey.",
    "Evidence: CodeFirstDiagnosticCatalog.cs defines DMV1901 and DMV1902 metadata in category CodeFirst; CodeFirstAnalyzerDiagnosticMetadata.cs creates DiagnosticDescriptor values with remediation text in the description.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj keeps RunAnalyzers=false but references the analyzer project and Roslyn assemblies; DataVaultCodeFirstAnalyzerTests.cs invokes compilation.WithAnalyzers(...).GetAnalyzerDiagnosticsAsync(...).",
    "Evidence: DVault.slnx contains project entries for src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj and tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj.",
    "Evidence: git diff --check develop..HEAD over src/DCoding.Data.DVault.Analyzers, tests/DCoding.Data.DVault.Tests/Analyzers, DVault.slnx, and Directory.Build.props produced no output.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F1XPS7KGKBP5SVMQPJC49J2G-story-establish-stable-dvault-diagnostic-codes\u0027.",
    "Evidence: Ticket history references implementation commit \u00272196be4e2e6e\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: The repository contains a working analyzer implementation path for DVault that follows existing layout conventions and can be exercised by automated tests without relying on the current RunAnalyzers=false test-project defaults. (src/DCoding.Data.DVault.Analyzers and tests/DCoding.Data.DVault.Tests/Analyzers exist, DVault.slnx includes both projects, and DataVaultCodeFirstAnalyzerTests.cs executes the analyzer through CSharpCompilation.WithAnalyzers instead of relying on normal project analyzer execution.).",
    "AC check passed: DMV1901 in category CodeFirst reports a documented diagnostic when BusinessKey(...), Payload(...), or DrivingKey(...) receives an unsupported selector shape such as anonymous-object, method-call, nested-member, or collection selectors, and does not report on valid direct readable scalar members. (DataVaultCodeFirstAnalyzer.cs reports CodeFirstDiagnosticCatalog.UnsupportedSelector for unsupported direct lambda selector bodies, and tests cover DMV1901 true positives for BusinessKey anonymous object, Payload method call, DrivingKey nested member, and Payload collection selectors plus a valid direct scalar non-finding guard.).",
    "AC check passed: DMV1902 in category CodeFirst reports a documented diagnostic when the same logical member name is declared more than once within the same relevant BusinessKey(...), Payload(...), or DrivingKey(...) fluent scope, and does not report when distinct members are declared once each. (DataVaultCodeFirstAnalyzer.cs reports CodeFirstDiagnosticCatalog.DuplicateMember for repeated logical member names in the same hub or satellite builder lambda scope, and tests cover BusinessKey, Payload, and DrivingKey duplicate true positives plus distinct-member and separate-satellite non-finding guards.).",
    "AC check passed: The analyzer project defines the metadata for DMV1901 and DMV1902 locally, using descriptor or catalog entries that mirror the established DVault diagnostic fields and catalog style without depending on non-public core-package catalog types. (CodeFirstDiagnosticCatalog.cs defines analyzer-local DMV1901 and DMV1902 descriptors in category CodeFirst, and CodeFirstAnalyzerDiagnosticMetadata.cs carries id, category, title, message, explanation, and remediation fields without referencing the core package internal catalog.).",
    "AC check passed: Automated tests cover at least one true-positive and one false-positive guard for each rule, using code samples that map back to the documented Code-First contract and current runtime-validation behavior. (DataVaultCodeFirstAnalyzerTests.cs contains true-positive tests for both rules and false-positive guards for valid direct scalar declarations and separate satellite scopes, using BusinessKey, Payload, and DrivingKey samples that match docs/plans/fluent-code-first-api-contract.md.).",
    "AC check passed: The solution and project layout for any new analyzer and analyzer-test projects match repository conventions and are added to DVault.slnx. (DVault.slnx includes src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj and tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj under the existing src/tests layout.).",
    "DoD check passed: Any new analyzer and test project scaffolding is limited to what is required for this ticket and follows the repository\u0027s existing net10.0, nullable, implicit-usings, and solution-layout conventions. (The new analyzer and analyzer test projects use net10.0, nullable enable, implicit usings, existing test runner properties, and Directory.Build.props adds the analyzer test project to the shared test output path convention.).",
    "DoD check passed: Diagnostic metadata for DMV1901 and DMV1902 is documented in source through analyzer-local catalog or descriptor definitions that mirror the established DVault contract fields, with no undocumented one-off ids, categories, titles, messages, or remediation text. (CodeFirstDiagnosticCatalog.cs documents DMV1901 and DMV1902 locally with stable ids, CodeFirst category, titles, messages, and remediation text surfaced through DiagnosticDescriptor descriptions.).",
    "DoD check passed: The test suite proves both findings and non-findings for the targeted Code-First misuse patterns, so the first analyzer slice is demonstrably low-noise. (The analyzer tests assert findings for unsupported selector shapes and duplicates, and non-findings for valid direct scalar declarations, separate satellite scopes, and selector variables outside the first direct-lambda slice.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: DMV1901 and DMV1902 are implemented, compile, and are covered by repeatable automated tests in the repository. (DMV1901 and DMV1902 are implemented and covered by repository tests, but this read-only session could not run dotnet test/build to directly confirm compile and repeatable execution.).",
    "DoD check failed: No broader analyzer backlog items, shared diagnostics-contract extraction, packaging polish, or non-trivial code-fix work are pulled into this ticket. (git diff develop..HEAD still includes unrelated documentation/example removals: README.md, docs/production-adoption-checklist.md, examples/README.md, and examples/DCoding.Data.DVault.PostgresQuickstart/README.md. These are outside the minimal analyzer implementation and test scaffolding required for this ticket.).",
    "Blocking: unrelated README/docs/example deletions remain in the branch diff, despite the ticket being limited to analyzer implementation and analyzer-test scaffolding.",
    "Executable verification remains unconfirmed in this read-only review; dotnet test DVault.slnx --nologo and bash tools/check-format.sh require deterministic verification in a writable/supported environment after the diff is cleaned."
  ],
  "evidence": [
    "git branch --show-current returned ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests.",
    "git log --first-parent shows 2196be4e2e6e as a lease-claim metadata commit and later handoff/test metadata commits; git diff --name-only 2196be4e2e6e..HEAD over src/tests/DVault.slnx/Directory.Build.props/README/docs/examples returned no output, so the reviewed branch tip has the same relevant tree as the claimed source ref.",
    "git diff --name-status develop..HEAD over repo implementation paths lists new src/DCoding.Data.DVault.Analyzers files, new tests/DCoding.Data.DVault.Tests/Analyzers files, DVault.slnx, and Directory.Build.props.",
    "git diff --name-status develop..HEAD also lists M README.md, D docs/production-adoption-checklist.md, D examples/DCoding.Data.DVault.PostgresQuickstart/README.md, and M examples/README.md; git diff --stat for those files shows 246 deletions.",
    "DataVaultCodeFirstAnalyzer.cs declares SupportedDiagnostics for CodeFirstDiagnosticCatalog.UnsupportedSelector and DuplicateMember, registers invocation and lambda syntax analysis, and reports DMV1901/DMV1902 diagnostics for BusinessKey, Payload, and DrivingKey.",
    "CodeFirstDiagnosticCatalog.cs defines DMV1901 and DMV1902 metadata in category CodeFirst; CodeFirstAnalyzerDiagnosticMetadata.cs creates DiagnosticDescriptor values with remediation text in the description.",
    "tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj keeps RunAnalyzers=false but references the analyzer project and Roslyn assemblies; DataVaultCodeFirstAnalyzerTests.cs invokes compilation.WithAnalyzers(...).GetAnalyzerDiagnosticsAsync(...).",
    "DVault.slnx contains project entries for src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj and tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj.",
    "git diff --check develop..HEAD over src/DCoding.Data.DVault.Analyzers, tests/DCoding.Data.DVault.Tests/Analyzers, DVault.slnx, and Directory.Build.props produced no output.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
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
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F1XPS7KGKBP5SVMQPJC49J2G-story-establish-stable-dvault-diagnostic-codes\u0027.",
    "Ticket history references implementation commit \u00272196be4e2e6e\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Restore README.md, docs/production-adoption-checklist.md, examples/README.md, and examples/DCoding.Data.DVault.PostgresQuickstart/README.md to the develop baseline unless a separate ticket authorizes those changes.",
    "After cleanup, run the policy verification commands in the supported environment: dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests",
  "commitSha": "2196be4e2e6e"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F1XQ1JNMDXAKMS9NFJA0A3GW`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests`