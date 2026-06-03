[gicket-bot] tracking-epic-closure-v1

Summary
- Closed tracking-only epic '06F8KYYJEM7HF4AFRAQA81F4S8' because PO-critic verified that the ticket is already satisfied and no developer or tester execution remains.
- PO-critic closure audit approved that the ticket is satisfied without developer or tester execution.

Evidence
- ticket: `06F8KYYJEM7HF4AFRAQA81F4S8`
- parentOf child evidence was not required for this closure-only ticket.

PO-critic audit evidence
- `.gicket/tickets/06F8KYYJEM7HF4AFRAQA81F4S8/description.md` contains `## Open Questions` followed by `- none`, so the persisted delivery contract has no unresolved open question.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `d39aed8422efbe9aec1b8e767eaee045af1326c9`; `git -C /mnt/c/Projects/DVault diff --name-status d39aed8422efbe9aec1b8e767eaee045af1326c9 HEAD` returned no paths; `git -C /mnt/c/Projects/DVault show --stat --format=fuller HEAD` shows the tip is the po-critic lease-claim commit touching `.gicket/...` metadata only.
- `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs` exposes `DMV1910` through `DMV1914` in `SupportedDiagnostics` and explicitly analyzes `UseModel(...)`, `AddDbContextPool<TContext>(...)`, and model-cache coverage.
- `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs` defines `DMV1912`, `DMV1913`, and `DMV1914` in category `EfCore` with lifecycle messages for missing cache-key discriminators, unsafe `UseModel(...)`, and unsafe `AddDbContextPool<TContext>(...)`.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs` directly registers `IModelCacheKeyFactory` and builds a `DataVaultModelCacheKey` from `SourceKind` and `Fingerprint`, which is direct source evidence for the registry-backed `UseDataVaultMetadata(...)` safe lane described by the ticket.
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` includes positive and negative lifecycle cases such as `ReportsMissingCacheKeyWhenDirectCodeFirstDeclarationUsesContextState`, `DoesNotReportMissingCacheKeyForMetadataFirstRegistryBackedOptions`, `ReportsUnsafeUseModelForVisibleVariableDataVaultShape`, `DoesNotReportUseModelForVisibleDesignRuntimeModelLane`, `ReportsUnsafeDbContextPoolForVisibleVariableDataVaultShape`, and `DoesNotReportDbContextPoolForOptionsOnlyFixedDataVaultShape`.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs` contains `CompiledModelKeepsDataVaultMetadataAnnotationsAfterRuntimeModelInitialization`, `ModelDriftPreflightComparesCompiledRuntimeModelAgainstExplicitSnapshotModelWithoutDatabaseConnection`, and `CompiledQueryReadsGeneratedSharedTypeProjectionWithDeterministicValuesThroughSqlite`; `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` also contain completed SQLite rows for `compiled-model-startup`, `compiled-query-hub-read`, and `dbcontext-pooling-dvault-operation`.
- `docs/releases/v0.27.0.md`, `docs/architecture/dvault-ef-compiled-compatibility.md`, `README.md`, `docs/production-adoption-checklist.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, and the root `dvault-ef-compiled-compatibility.md` all repeat the same analyzer-only lifecycle boundary, safe non-diagnostic lanes, and no-runtime-change posture.

PO-critic non-blocking notes
- The current branch tip is only the po-critic lease claim commit; the empty diff against `d39aed8422efbe9aec1b8e767eaee045af1326c9` means this review is evaluating already-landed repository evidence rather than in-flight epic changes.

PO-critic closure watchouts
- Do not let downstream work reinterpret this epic as runtime enforcement or provider-specific lifecycle proof; the landed repo surfaces consistently frame `DMV1912` through `DMV1914` as analyzer-only source-visible guardrails.
- Publication is still a separate release activity: `docs/releases/v0.27.0.md` and `docs/manual-nuget-publication.md` both keep final package push and approval outside this epic.

<!-- gicket-semantic-idempotency-key: bot-closure:06f8kyyjem7hf4afraqa81f4s8:tracking-epic:done:done -->