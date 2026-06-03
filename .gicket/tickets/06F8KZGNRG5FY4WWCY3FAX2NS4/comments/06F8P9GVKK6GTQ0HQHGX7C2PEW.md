[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F8KZGNRG5FY4WWCY3FAX2NS4\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027 and commit \u0027d027853b4ba5\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027 from source \u0027d027853b4ba5\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027.",
    "Evidence: \u0060git diff --name-only develop...d027853b4ba5\u0060 touches \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0060 plus \u0060.gicket/...\u0060 metadata.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:24-56\u0060 adds DMV1912, DMV1913, and DMV1914 descriptors.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:138-151\u0060 emits DMV1912 from direct \u0060AddDbContext*\u0060 registration analysis.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:334-447\u0060 derives registration variation by treating non-\u0060DbContextOptionsBuilder\u0060 parameters and unresolved locals as varying symbols.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:178-196\u0060 reuses that registration shape when emitting DMV1914.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:582-609\u0060 only recognizes declaration initializers with constant values as fixed source-visible state.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:182-639\u0060 adds new positive and non-diagnostic analyzer tests for DMV1912-DMV1914.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:959-976\u0060 only stubs single-parameter \u0060AddDbContext\u0060 and \u0060AddDbContextPool\u0060 overloads, so DI/service-provider registration cases are not exercised.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/modeling, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027.",
    "Evidence: Ticket history references implementation commit \u0027d027853b4ba5\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 2 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "DoD check passed: EfCoreMisuseDiagnosticCatalog exposes contiguous DMV1912 through DMV1914 descriptors with warning severity and remediation text aligned to the lifecycle contract. (\u0060EfCoreMisuseDiagnosticCatalog.cs:24-56\u0060 adds contiguous DMV1912-DMV1914 descriptors with warning severity and remediation text, and \u0060DataVaultEfCoreMisuseAnalyzerTests.cs:13-40\u0060 asserts the supported descriptor set.).",
    "DoD check passed: Targeted analyzer tests cover at least one positive and one non-diagnostic safe case for each new rule, while the larger regression-fixture expansion remains in the sibling fixture story. (\u0060DataVaultEfCoreMisuseAnalyzerTests.cs:182-639\u0060 adds positive and non-diagnostic tests for DMV1912, DMV1913, and DMV1914, although those tests do not cover the false-positive paths called out in the findings.).",
    "DoD check passed: The implementation leaves runtime packages and runtime behavior unchanged. (\u0060git diff --name-only develop...d027853b4ba5\u0060 shows code changes only in the analyzer/catalog/test files plus ticket metadata; no runtime package or runtime-behavior files changed.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: DMV1912 is implemented as a warning in the existing EfCore analyzer category and reports only when source-visible DVault model-shape variation depends on instance or selected metadata state and the visible model-cache-key path does not include that varying state. (\u0060DataVaultEfCoreMisuseAnalyzer.cs:138-151\u0060 reports DMV1912 from \u0060AddDbContext*\u0060 registration shape alone, without any cache-key-coverage check, and \u0060DataVaultEfCoreMisuseAnalyzer.cs:400-447\u0060 also treats runtime-DI/helper inputs as varying. That breaks the rule\u0027s direct high-confidence boundary.).",
    "AC check failed: DMV1913 is implemented as a warning and reports only when source-visible UseModel(...) applies a compiled or runtime model to a DVault context with visibly variable realized model shape and the same visible source scope does not prove one fixed shape or the documented safe design-model-to-runtime-model lane. (DMV1913 depends on \u0060GetContextLifecycleShape(...)\u0060, but \u0060DataVaultEfCoreMisuseAnalyzer.cs:582-609\u0060 only recognizes declaration initializers as fixed source-visible state. Fixed expression-bodied or getter-backed discriminators would still look variable, so \u0060UseModel(...)\u0060 is not limited to truly variable visible shapes.).",
    "AC check failed: DMV1914 is implemented as a warning and reports only when source-visible AddDbContextPool\u003CTContext\u003E(...) is used for a DVault context whose realized model shape visibly varies beyond one fixed options-only shape. (DMV1914 reuses the same lifecycle-shape heuristics (\u0060DataVaultEfCoreMisuseAnalyzer.cs:178-196\u0060, \u0060334-447\u0060, \u0060582-609\u0060), so pooled-context diagnostics can also fire for runtime-DI registration state or fixed getter-backed members instead of only real variable shapes.).",
    "AC check failed: The implementation keeps UseDataVaultMetadata(...) registration paths, safe fixed-shape ApplyDataVaultMetadata(...) paths, documented read-only generated-table query patterns, safe compiled-query use, and visibly sufficient custom cache-key examples non-diagnostic. (Safe lanes are not consistently preserved: fixed-shape \u0060ApplyDataVaultMetadata(...)\u0060 or schema members expressed as getter-backed constants are treated as varying by \u0060IsFixedSourceVisibleStateMember(...)\u0060, and there is no test covering that safe form.).",
    "AC check failed: The implementation skips ambiguous cases instead of guessing, including helper-expanded registrations, cross-assembly inference, opaque custom IModelCacheKeyFactory logic, and runtime-only tenant or DI state. (The implementation does not skip all ambiguous cases required by the contract; opaque helper-expanded registration locals and non-\u0060DbContextOptionsBuilder\u0060 lambda parameters are converted into varying symbols in \u0060AddVaryingSourceReferences(...)\u0060 (\u0060DataVaultEfCoreMisuseAnalyzer.cs:400-447\u0060).).",
    "DoD check failed: DataVaultEfCoreMisuseAnalyzer emits the new diagnostics only from direct source-visible evidence and preserves existing DMV1910 and DMV1911 behavior. (The analyzer does not stay within the promised high-confidence direct-evidence boundary because the new registration and fixed-member heuristics can report ambiguous DI/helper cases and fixed-shape members.).",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:334-447\u0060, \u0060138-151\u0060, and \u0060178-196\u0060: \u0060AddDbContext*\u0060 registration analysis turns non-options lambda parameters and opaque helper-derived locals into diagnostics, so runtime-only DI/service-provider state can incorrectly trigger DMV1912 and DMV1914 instead of being skipped as ambiguous.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:582-609\u0060: fixed-state detection ignores expression-bodied or getter-backed constants, so fixed visible shapes can be treated as variable and falsely trigger DMV1912-DMV1914.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:959-976\u0060: the new unit-test harness covers only single-parameter registration overloads and misses both DI-state and fixed-member false-positive paths."
  ],
  "evidence": [
    "\u0060git diff --name-only develop...d027853b4ba5\u0060 touches \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0060 plus \u0060.gicket/...\u0060 metadata.",
    "\u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:24-56\u0060 adds DMV1912, DMV1913, and DMV1914 descriptors.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:138-151\u0060 emits DMV1912 from direct \u0060AddDbContext*\u0060 registration analysis.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:334-447\u0060 derives registration variation by treating non-\u0060DbContextOptionsBuilder\u0060 parameters and unresolved locals as varying symbols.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:178-196\u0060 reuses that registration shape when emitting DMV1914.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:582-609\u0060 only recognizes declaration initializers with constant values as fixed source-visible state.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:182-639\u0060 adds new positive and non-diagnostic analyzer tests for DMV1912-DMV1914.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:959-976\u0060 only stubs single-parameter \u0060AddDbContext\u0060 and \u0060AddDbContextPool\u0060 overloads, so DI/service-provider registration cases are not exercised.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/modeling, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027.",
    "Ticket history references implementation commit \u0027d027853b4ba5\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 2 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Tighten \u0060AddDbContext*\u0060 registration analysis so runtime-DI/service-provider inputs and opaque helper-expanded locals are skipped instead of converted into varying symbols.",
    "Extend fixed-state detection to recognize expression-bodied/getter-backed constant context members, or conservatively skip them rather than diagnosing them as variable.",
    "Add analyzer tests for service-provider registration overloads, helper-derived registration conditions, and fixed getter-backed discriminator members across DMV1912, DMV1913, and DMV1914."
  ],
  "branchName": "ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault",
  "commitSha": "d027853b4ba5"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F8KZGNRG5FY4WWCY3FAX2NS4`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault`