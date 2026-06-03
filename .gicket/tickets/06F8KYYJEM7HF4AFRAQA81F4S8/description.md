<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this epic to the already-bounded v0.27.0 release contract: `DMV1912` through `DMV1914` are the new EF lifecycle analyzer guardrails, the safe non-diagnostic lanes are explicit, and no blocking PO clarification remains.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This epic advances the v0.27.0 analyzer/documentation baseline by adding lifecycle diagnostics `DMV1912`, `DMV1913`, and `DMV1914`; `DMV1910` and `DMV1911` remain carried-forward EF misuse context, not fresh scope.
- The lifecycle slice is analyzer-only and source-visible. It must not add runtime guards, runtime behavior changes, compiled-model generation, or whole-application/cross-assembly inference.
- Safe non-diagnostic baselines are already fixed in repo docs: registry-backed `UseDataVaultMetadata(...)`, fixed-shape `ApplyDataVaultMetadata(...)` with visible cache-key coverage, fixed-shape `UseModel(runtimeModel)`, stable direct compiled queries, and options-only fixed-shape `AddDbContextPool<TContext>(...)`.
- Ambiguous, helper-expanded, pooled-factory, generated-artifact-inspection, provider-SQL-specific, and cross-assembly cases are explicit skip/non-goal territory for this ticket.

### Scope In
- Define and deliver `DMV1912` for source-visible caller-owned DVault model-shape variation whose visible `IModelCacheKeyFactory` path omits the same discriminator.
- Define and deliver `DMV1913` for direct `UseModel(...)` compiled-model selection on a visibly variable-shape DVault context.
- Define and deliver `DMV1914` for direct `AddDbContextPool<TContext>(...)` registration of a visibly variable-shape DVault context.
- Document one consistent v0.27.0 lifecycle guardrail story across release notes, README/adoption guidance, analyzer package guidance, and the compiled-compatibility architecture note.
- Back the boundary with analyzer tests and SQLite compiled-compatibility evidence for fixed-shape runtime-model and compiled-query support.

### Scope Out
- Any runtime guard, runtime behavior change, or save/read pipeline change.
- Generated compiled-model artifacts, custom `dotnet ef` tooling, or design-time service generation owned by DVault.
- Whole-application DI inference, cross-assembly inference, arbitrary helper expansion, or generated compiled-model artifact inspection.
- Provider-specific lifecycle guarantees, provider-specific SQL validation, or pooled-factory diagnostics.
- Benchmark reruns, package publication, release approval, or package-publication automation.

## Acceptance Criteria
- The ticket contract defines `DMV1912`, `DMV1913`, and `DMV1914` as `EfCore` warning diagnostics for direct source-visible evidence only.
- `DMV1912` reports only when visible DVault model-shape variation depends on caller-owned discriminators and the visible model-cache-key path omits those discriminators; registry-backed `UseDataVaultMetadata(...)` paths and visibly complete custom cache keys remain non-diagnostic.
- `DMV1913` reports only when direct `UseModel(...)` applies a compiled/runtime EF model to a DVault context whose realized model shape is visibly variable; fixed-shape contexts and the documented design-model-to-runtime-model lane remain non-diagnostic.
- `DMV1914` reports only when direct `AddDbContextPool<TContext>(...)` targets a DVault context whose visible model shape varies beyond one fixed options-only shape; fixed-shape options-only pooling remains non-diagnostic.
- Stable direct compiled queries over generated shared-type tables remain documented non-diagnostic read patterns and are not treated as compiled-model selection.
- Ambiguous or opaque cases are skipped rather than guessed, including helper-expanded, cross-assembly, pooled-factory, and provider-SQL-specific scenarios.
- Repository documentation for v0.27.0 tells one consistent story about lifecycle scope, safe lanes, validation evidence, and non-goals.

## Definition of Done
- The v0.27.0 release note, compiled-compatibility architecture note, root README, production adoption checklist, and analyzer README all reflect the same lifecycle guardrail boundary.
- Analyzer descriptor/catalog and tests cover positive and negative cases for `DMV1912` through `DMV1914`, while existing `DMV1910` and `DMV1911` context remains intact.
- SQLite compiled-compatibility evidence remains present for runtime-model initialization, fixed-shape `UseModel(runtimeModel)`, deterministic compiled-query reads, and explicit model-cache-key handling.
- The repository validation baseline for this release remains `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh`.
- No runtime API or runtime-behavior expansion is required beyond the analyzer/documentation boundary.

## Implementation Notes
- Use `docs/releases/v0.27.0.md` as the release-boundary source of truth and keep its language aligned with `docs/architecture/dvault-ef-compiled-compatibility.md`, `README.md`, `docs/production-adoption-checklist.md`, and `src/DCoding.Data.DVault.Analyzers/README.md`.
- Treat `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs` and `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` as the concrete repository evidence for the diagnostic catalog and false-positive guardrails.
- Ratify registry-backed `UseDataVaultMetadata(...)` and fixed-shape `UseModel(runtimeModel)` as the visible v1 safe defaults instead of reopening those decisions.
- Keep analyzer logic syntax/semantic-bound: direct visible instance-member reads, direct branches, direct `ReplaceService<IModelCacheKeyFactory, ...>()` paths, direct returned cache-key shapes, and direct `UseModel(...)` or `AddDbContextPool<TContext>(...)` calls.
- Use `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs` as the bounded proof for compiled compatibility and deterministic compiled-query reads; no new provider matrix is required for this ticket.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket extend lifecycle diagnostics beyond direct `AddDbContextPool<TContext>(...)` to pooled factories or other pooling entrypoints?
- Should a later ticket add separate guardrails for helper-expanded or cross-assembly lifecycle patterns that this analyzer slice intentionally skips?
- Does the product want future runtime enforcement or provider-specific lifecycle guarantees, or should those remain permanently outside the analyzer-only posture?

## Risks
- Because the analyzer intentionally skips ambiguous and indirect code paths, some real lifecycle misuse can remain undiagnosed until a future ticket broadens the contract.
- The safe pooled and compiled-model baselines still depend on consumer-owned model-cache-key discipline when model shape varies.
- This release note is a documentation baseline only; final package publication still depends on a separate approval record outside this ticket.

## Split Recommendations
- If delivery breadth re-expands, keep this epic limited to `DMV1912` through `DMV1914` plus bounded docs/tests, and move pooled-factory, helper-expansion, cross-assembly, or runtime-guard ideas into separate follow-on tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Plan and deliver high-confidence analyzer diagnostics for DVault EF lifecycle safety. The release must stay inside Roslyn analyzer and documentation boundaries, avoid whole-application DI inference, and keep runtime behavior unchanged.