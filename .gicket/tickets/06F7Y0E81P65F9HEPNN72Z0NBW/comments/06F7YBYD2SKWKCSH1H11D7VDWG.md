[gicket-bot] closure-only-ticket-closure-v1

Summary
- Closed closure-only ticket '06F7Y0E81P65F9HEPNN72Z0NBW' because PO-critic verified that the ticket is already satisfied and no developer or tester execution remains.
- PO-critic closure audit approved that the ticket is satisfied without developer or tester execution.

Evidence
- ticket: `06F7Y0E81P65F9HEPNN72Z0NBW`
- parentOf child evidence was not required for this closure-only ticket.

PO-critic audit evidence
- .gicket/tickets/06F7Y0E81P65F9HEPNN72Z0NBW/description.md contains a full delivery contract with `PO Handoff` = `ready_for_po_critic`, `## Open Questions` = `none`, and explicit acceptance criteria for `AddDbContextPool<TContext>(...)`, `UseModel(...)`, and missing `ReplaceService<IModelCacheKeyFactory,...>` diagnostics.
- The latest substantive ticket comment, `.gicket/tickets/06F7Y0E81P65F9HEPNN72Z0NBW/comments/06F7Y9Q9PEXX9J7T12B6PDXVJ0.md`, restates the bounded analyzer scope and `ready_for_po_critic`; later comments are orchestration/lease records only.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs` already registers `DataVaultModelCacheKeyFactory`, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs` directly proves app-default, explicit-registry, and import-result `UseDataVaultMetadata(...)` participate in the EF model cache key while caller-owned model-shape state can require a custom `IModelCacheKeyFactory`.
- `docs/architecture/dvault-ef-compiled-compatibility.md`, `README.md` (`Isolate EF model cache entries`), and `benchmark-summary.md` already define the fixed-model `UseModel(...)` and `AddDbContextPool<TContext>(...)` boundary the new analyzer guidance should reference.
- `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs`, `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs`, and `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` show an existing in-place EF misuse analyzer with only `DMV1910` and `DMV1911`, matching the ticket's extension plan.
- `git log --oneline --graph -n 8` shows only PO/PO-critic claim and handoff commits on this branch, and `git diff --name-only develop...3796846256fd7f926190da5f14cdd3f1366fd9f5` lists only `.gicket/tickets/06F7Y0E81P65F9HEPNN72Z0NBW/...` metadata files, so no implementation evidence is being overstated at this pre-dev gate.

PO-critic non-blocking notes
- The branch currently carries ticket metadata only, which is expected at this gate; absence of code changes or test results is not a PO blocker for a normal pre-development story when the delivery contract is otherwise clear.

PO-critic closure watchouts
- Keep the work inside `DataVaultEfCoreMisuseAnalyzer`, `EfCoreMisuseDiagnosticCatalog`, and `DataVaultEfCoreMisuseAnalyzerTests`; current tests explicitly assert only `DMV1910` and `DMV1911`, so the new diagnostics must be additive without regressing those rules.
- Use direct source-visible evidence only; the current analyzer is syntax/operation based and the contract explicitly rules out whole-application DI inference or proving custom factory completeness.
- Do not report ordinary registry-backed `UseDataVaultMetadata()` alone as unsafe; `DataVaultMetadataRegistrationIntegrationTests.cs` already proves the built-in metadata-source fingerprint isolation contract.

<!-- gicket-semantic-idempotency-key: bot-closure:06f7y0e81p65f9hepnn72z0nbw:closure-only-ticket:done:doing-done -->