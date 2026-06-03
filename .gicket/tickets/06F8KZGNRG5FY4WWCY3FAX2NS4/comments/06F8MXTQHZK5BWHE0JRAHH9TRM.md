[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F8KZGNRG5FY4WWCY3FAX2NS4' for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZGNRG5FY4WWCY3FAX2NS4`.
- Optimistic claim succeeded (`expectedRevision=06F8MVY674HHTNMKHR8VSJ514W`, `currentRevision=06F8MW8CC2JZAKM3SR1837C5KR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault' and commit '9b1c8684ba6f' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault' from source '9b1c8684ba6f'.
- Interactive tester tool loop completed review for branch 'ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault'.
- Evidence: `git diff --name-only develop...9b1c8684ba6f` lists repository code changes in `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs`, `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs`, and `tests/DCoding.Data.DVault.Tests/Analyzers/D...
- Evidence: `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:24-56` adds warning descriptors `DMV1912`, `DMV1913`, and `DMV1914`.
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:119-139` emits DMV1913 and DMV1914 directly from `GetContextLifecycleShape(...)` without additional same-scope safe-lane proof.
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268` sets variable shape by collecting every instance field/property reference anywhere in `OnModelCreating(...)` once `ApplyDataVaultMetadata(...)` is present.
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:436-495` treats only factory-self method calls as opaque when evaluating custom `IModelCacheKeyFactory` return values.
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:571-585` classifies registrations by method name (`AddDbContext`/`AddDbContextPool`) and generic DbContext type alone.
- 35 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: DMV1912 is implemented as a warning in the existing EfCore analyzer category and reports only when source-visible DVault model-shape variation depends on instance or selected metadata state and the visible model-cache-key path does not include that varying sta...
- AC check failed: DMV1913 is implemented as a warning and reports only when source-visible UseModel(...) applies a compiled or runtime model to a DVault context with visibly variable realized model shape and the same visible source scope does not prove one fixed shape or the do...
- AC check failed: DMV1914 is implemented as a warning and reports only when source-visible AddDbContextPool<TContext>(...) is used for a DVault context whose realized model shape visibly varies beyond one fixed options-only shape. (DMV1914 is wired as a warning, but `src/DCodin...
- AC check failed: The implementation keeps UseDataVaultMetadata(...) registration paths, safe fixed-shape ApplyDataVaultMetadata(...) paths, documented read-only generated-table query patterns, safe compiled-query use, and visibly sufficient custom cache-key examples non-diagno...
- AC check failed: The implementation skips ambiguous cases instead of guessing, including helper-expanded registrations, cross-assembly inference, opaque custom IModelCacheKeyFactory logic, and runtime-only tenant or DI state. (The contract requires ambiguous helper/opaque case...
- DoD check failed: DataVaultEfCoreMisuseAnalyzer emits the new diagnostics only from direct source-visible evidence and preserves existing DMV1910 and DMV1911 behavior. (The analyzer does not emit only from sufficiently direct source-visible evidence because `src/DCoding.Data.D...
- High: `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:199-268` broadens DMV1912/DMV1913/DMV1914 to any context member reference anywhere in `OnModelCreating(...)`, even when that member is not part of the DVault model-shaping path. That violates the high-co...
- High: `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:119-128` never checks the documented same-scope design-model-to-runtime-model safe lane from `docs/architecture/dvault-ef-compiled-compatibility.md:93`, so a variable-capable context can be flagged even ...
- Medium: `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:436-495` and `:571-585` still guess through ambiguous cases: helper-based cache-key construction is treated as omission unless the helper lives on the factory type, and any generic method named `AddDbC...

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Narrow variable-shape detection so it only tracks members that directly participate in DVault model projection, naming, schema, provider, profile, or caller-owned metadata selection instead of scanning every instance-member reference in `OnModelCreating(...)`.
- Rework DMV1913 and DMV1914 to validate the actual EF registration symbols and the local source scope, including the documented non-diagnostic `UseModel(runtimeModel)` design-model-to-runtime-model lane.
- Treat helper-based or otherwise indirect cache-key computation as opaque/no-diagnostic, then add regression tests for unrelated `OnModelCreating(...)` member reads, helper-built `IModelCacheKeyFactory` keys, safe `UseModel(runtimeModel)` scope, and non-EF helper methods named ...

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8727`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `76e721c9aaa74fa3acdeb4a9eb21c4c4`
- completed-at-utc: `<redacted>-02T22:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZGNRG5FY4WWCY3FAX2NS4/runs/20260602T223217545Z-76e721c9aaa74fa3acdeb4a9eb21c4c4.json`