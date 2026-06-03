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
    "Selected verification source branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027 and commit \u00279b1c8684ba6f\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027 from source \u00279b1c8684ba6f\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027.",
    "Evidence: \u0060git diff --name-only develop...9b1c8684ba6f\u0060 lists repository code changes in \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0060 plus ticket metadata.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:24-56\u0060 adds warning descriptors \u0060DMV1912\u0060, \u0060DMV1913\u0060, and \u0060DMV1914\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:119-139\u0060 emits DMV1913 and DMV1914 directly from \u0060GetContextLifecycleShape(...)\u0060 without additional same-scope safe-lane proof.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268\u0060 sets variable shape by collecting every instance field/property reference anywhere in \u0060OnModelCreating(...)\u0060 once \u0060ApplyDataVaultMetadata(...)\u0060 is present.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:436-495\u0060 treats only factory-self method calls as opaque when evaluating custom \u0060IModelCacheKeyFactory\u0060 return values.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:571-585\u0060 classifies registrations by method name (\u0060AddDbContext\u0060/\u0060AddDbContextPool\u0060) and generic DbContext type alone.",
    "Evidence: \u0060docs/architecture/dvault-ef-compiled-compatibility.md:91-97\u0060 requires helper-based opaque cache-key logic to skip, requires the documented \u0060UseModel(runtimeModel)\u0060 design-model-to-runtime-model lane to stay non-diagnostic, and limits DMV1914 to direct \u0060AddDbContextPool\u003CTContext\u003E(...)\u0060 registrations.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:182-333\u0060 adds one positive and one safe case for each new rule, but no regression covering helper-built cache keys or the documented design-model-to-runtime-model \u0060UseModel(runtimeModel)\u0060 lane.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/modeling, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027.",
    "Evidence: Ticket history references implementation commit \u00279b1c8684ba6f\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "DoD check passed: EfCoreMisuseDiagnosticCatalog exposes contiguous DMV1912 through DMV1914 descriptors with warning severity and remediation text aligned to the lifecycle contract. (\u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:24-56\u0060 exposes contiguous warning descriptors \u0060DMV1912\u0060 through \u0060DMV1914\u0060 with remediation text, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:12-46\u0060 asserts the supported IDs and metadata.).",
    "DoD check passed: Targeted analyzer tests cover at least one positive and one non-diagnostic safe case for each new rule, while the larger regression-fixture expansion remains in the sibling fixture story. (\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:182-333\u0060 adds at least one positive and one non-diagnostic safe case for each of DMV1912, DMV1913, and DMV1914.).",
    "DoD check passed: The implementation leaves runtime packages and runtime behavior unchanged. (\u0060git diff --name-only develop...9b1c8684ba6f\u0060 shows repository code changes only in \u0060src/DCoding.Data.DVault.Analyzers/*\u0060, \u0060tests/DCoding.Data.DVault.Tests/Analyzers/*\u0060, and ticket metadata; no runtime package or runtime-library files were modified.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: DMV1912 is implemented as a warning in the existing EfCore analyzer category and reports only when source-visible DVault model-shape variation depends on instance or selected metadata state and the visible model-cache-key path does not include that varying state. (\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268\u0060 treats any instance field/property reference anywhere in \u0060OnModelCreating(...)\u0060 as model-shape variation once \u0060ApplyDataVaultMetadata(...)\u0060 is present, and \u0060:436-495\u0060 does not skip helper-based cache-key construction, so DMV1912 does not report only on proven model-shape/cache-key mismatches.).",
    "AC check failed: DMV1913 is implemented as a warning and reports only when source-visible UseModel(...) applies a compiled or runtime model to a DVault context with visibly variable realized model shape and the same visible source scope does not prove one fixed shape or the documented safe design-model-to-runtime-model lane. (DMV1913 is wired as a warning, but \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:119-128\u0060 reports on any variable-shape context type and never checks the documented same-scope design-model-to-runtime-model safe lane required by \u0060docs/architecture/dvault-ef-compiled-compatibility.md:93\u0060.).",
    "AC check failed: DMV1914 is implemented as a warning and reports only when source-visible AddDbContextPool\u003CTContext\u003E(...) is used for a DVault context whose realized model shape visibly varies beyond one fixed options-only shape. (DMV1914 is wired as a warning, but \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:131-139,571-585\u0060 reports from type-level shape plus a method-name match for \u0060AddDbContextPool\u0060, not only from proven direct EF pooling registrations whose visible model shape varies beyond one fixed options-only shape.).",
    "AC check failed: The implementation keeps UseDataVaultMetadata(...) registration paths, safe fixed-shape ApplyDataVaultMetadata(...) paths, documented read-only generated-table query patterns, safe compiled-query use, and visibly sufficient custom cache-key examples non-diagnostic. (Read-only query, compiled-query, and cache-key happy-path tests were added, but \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268\u0060 can still flag fixed-shape \u0060ApplyDataVaultMetadata(...)\u0060 paths when unrelated context members are read inside \u0060OnModelCreating(...)\u0060, so the non-diagnostic safe-lane contract is not preserved.).",
    "AC check failed: The implementation skips ambiguous cases instead of guessing, including helper-expanded registrations, cross-assembly inference, opaque custom IModelCacheKeyFactory logic, and runtime-only tenant or DI state. (The contract requires ambiguous helper/opaque cases to skip, but \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:436-495\u0060 treats only self-factory calls as opaque and \u0060:571-585\u0060 infers registrations by method name alone, so helper-expanded and opaque cases are still guessed instead of skipped.).",
    "DoD check failed: DataVaultEfCoreMisuseAnalyzer emits the new diagnostics only from direct source-visible evidence and preserves existing DMV1910 and DMV1911 behavior. (The analyzer does not emit only from sufficiently direct source-visible evidence because \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268\u0060 over-approximates variable shape and \u0060:571-585\u0060 over-approximates registrations; this item is conjunctive even though DMV1910/DMV1911 code remains present.).",
    "High: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268\u0060 broadens DMV1912/DMV1913/DMV1914 to any context member reference anywhere in \u0060OnModelCreating(...)\u0060, even when that member is not part of the DVault model-shaping path. That violates the high-confidence direct-evidence boundary and can false-positive safe fixed-shape contexts.",
    "High: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:119-128\u0060 never checks the documented same-scope design-model-to-runtime-model safe lane from \u0060docs/architecture/dvault-ef-compiled-compatibility.md:93\u0060, so a variable-capable context can be flagged even when the visible \u0060UseModel(runtimeModel)\u0060 lane proves one fixed realized shape.",
    "Medium: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:436-495\u0060 and \u0060:571-585\u0060 still guess through ambiguous cases: helper-based cache-key construction is treated as omission unless the helper lives on the factory type, and any generic method named \u0060AddDbContextPool\u0060 is treated as a pool registration even if it is not the direct EF entrypoint."
  ],
  "evidence": [
    "\u0060git diff --name-only develop...9b1c8684ba6f\u0060 lists repository code changes in \u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0060 plus ticket metadata.",
    "\u0060src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:24-56\u0060 adds warning descriptors \u0060DMV1912\u0060, \u0060DMV1913\u0060, and \u0060DMV1914\u0060.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:119-139\u0060 emits DMV1913 and DMV1914 directly from \u0060GetContextLifecycleShape(...)\u0060 without additional same-scope safe-lane proof.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268\u0060 sets variable shape by collecting every instance field/property reference anywhere in \u0060OnModelCreating(...)\u0060 once \u0060ApplyDataVaultMetadata(...)\u0060 is present.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:436-495\u0060 treats only factory-self method calls as opaque when evaluating custom \u0060IModelCacheKeyFactory\u0060 return values.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:571-585\u0060 classifies registrations by method name (\u0060AddDbContext\u0060/\u0060AddDbContextPool\u0060) and generic DbContext type alone.",
    "\u0060docs/architecture/dvault-ef-compiled-compatibility.md:91-97\u0060 requires helper-based opaque cache-key logic to skip, requires the documented \u0060UseModel(runtimeModel)\u0060 design-model-to-runtime-model lane to stay non-diagnostic, and limits DMV1914 to direct \u0060AddDbContextPool\u003CTContext\u003E(...)\u0060 registrations.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:182-333\u0060 adds one positive and one safe case for each new rule, but no regression covering helper-built cache keys or the documented design-model-to-runtime-model \u0060UseModel(runtimeModel)\u0060 lane.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/modeling, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault\u0027.",
    "Ticket history references implementation commit \u00279b1c8684ba6f\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Narrow variable-shape detection so it only tracks members that directly participate in DVault model projection, naming, schema, provider, profile, or caller-owned metadata selection instead of scanning every instance-member reference in \u0060OnModelCreating(...)\u0060.",
    "Rework DMV1913 and DMV1914 to validate the actual EF registration symbols and the local source scope, including the documented non-diagnostic \u0060UseModel(runtimeModel)\u0060 design-model-to-runtime-model lane.",
    "Treat helper-based or otherwise indirect cache-key computation as opaque/no-diagnostic, then add regression tests for unrelated \u0060OnModelCreating(...)\u0060 member reads, helper-built \u0060IModelCacheKeyFactory\u0060 keys, safe \u0060UseModel(runtimeModel)\u0060 scope, and non-EF helper methods named \u0060AddDbContextPool\u0060; after that, rerun \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 through the supported verification path."
  ],
  "branchName": "ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault",
  "commitSha": "9b1c8684ba6f"
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