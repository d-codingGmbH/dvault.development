<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this into a bounded v0.27 documentation-alignment task: roll the coordinated docs baseline forward, keep DMV1912-DMV1914 lifecycle guidance consistent across public docs, and preserve the analyzer-only no-runtime-change posture.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is documentation alignment for already-landed analyzer and test behavior; it does not reopen analyzer implementation or runtime design.
- Treat `docs/architecture/dvault-ef-compiled-compatibility.md` as the authoritative lifecycle and compiled-compatibility source; the root `dvault-ef-compiled-compatibility.md` remains a validation-facing entrypoint, not a second contract document.
- The v0.27 analyzer slice is the existing EfCore misuse surface for `DMV1910` through `DMV1914`, with `DMV1912` through `DMV1914` limited to high-confidence, source-visible lifecycle misuse around caller-owned cache keys, `UseModel(...)`, and direct `AddDbContextPool<TContext>(...)` usage.
- Registry-backed `UseDataVaultMetadata(...)` paths, the documented fixed-shape `UseModel(runtimeModel)` lane, stable direct EF compiled queries over generated shared-type tables, and fixed-shape options-only pooling remain documented non-diagnostic baselines.
- Related contract, implementation, and fixture tickets `06F8KZGC4NY41PRYB2RP00ZA1M`, `06F8KZGNRG5FY4WWCY3FAX2NS4`, and `06F8KZGZND5ZCH147PVBRWXYN4` are already `done` and should be treated as completed evidence, not blockers for this ticket.

### Scope In
- Roll the current coordinated documentation baseline from v0.26.0 to v0.27.0 in the root README, production checklist, and release-note surface.
- Align `src/DCoding.Data.DVault.Analyzers/README.md`, README install snippets, and other versioned examples with the v0.27.0 documentation baseline while preserving the existing no-publication disclaimer.
- Document the lifecycle analyzer boundary for `DMV1912`, `DMV1913`, and `DMV1914` alongside the carried-forward analyzer catalog and limitations.
- Keep EF compiled-model, compiled-query, and `DbContext` pooling guidance consistent across README, checklist, compiled-compatibility note, and release notes.
- Add the v0.27 validation-evidence and non-goal story for EF lifecycle analyzer guardrails, including the explicit no-runtime-change posture.

### Scope Out
- Analyzer implementation changes, descriptor changes, severity changes, or new analyzer test coverage.
- New runtime guards, interceptors, compiled-model generators, provider-specific lifecycle behavior, or whole-application inference.
- Broadening diagnostics beyond the documented direct `ApplyDataVaultMetadata(...)`, `UseModel(...)`, and direct `AddDbContextPool<TContext>(...)` evidence lanes.
- NuGet publication records, package hashes, release approval records, or release-automation work.
- Benchmark reruns or new performance/provider claims unrelated to the lifecycle analyzer documentation rollout.

## Acceptance Criteria
- A new `docs/releases/v0.27.0.md` records `v0.27.0 - EF Core Lifecycle Analyzer Guardrails` as the current coordinated documentation baseline and explicitly states the analyzer-only, no-runtime-change posture.
- The root README and `docs/production-adoption-checklist.md` identify v0.27.0 as the current public baseline, retain earlier release notes as historical records, and use aligned `0.27.0` package examples without claiming package publication.
- Public docs consistently describe the relevant analyzer surface as `DMV1910` and `DMV1911` for generated shared-type-table misuse plus `DMV1912` through `DMV1914` for source-visible EF lifecycle misuse, while preserving the carried-forward `DMV1950` through `DMV1955` and `DMV1960` through `DMV1969` references where those ranges are already in scope.
- README, analyzer README, checklist, and release notes all state the same safe-lane boundaries: registry-backed `UseDataVaultMetadata(...)`, fixed-shape `UseModel(runtimeModel)`, stable direct EF compiled queries, and options-only pooling for one fixed metadata/model shape remain supported and non-diagnostic.
- README, analyzer README, compiled-compatibility guidance, checklist, and release notes all state the same non-goals: no runtime guard, no runtime behavior change, no compiled-model generator, no provider-specific lifecycle guarantee, and no cross-assembly or whole-application inference.
- Validation and evidence sections cite the landed repository surfaces for this story, at minimum `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs`, plus the authoritative architecture note and analyzer README where appropriate.
- The root `dvault-ef-compiled-compatibility.md` entrypoint remains consistent with the authoritative architecture note and does not fork the lifecycle contract into conflicting parallel prose.

## Definition of Done
- `docs/releases/v0.27.0.md` exists and the targeted documentation surfaces tell one consistent v0.27 story.
- No targeted surface still presents v0.26.0 as the current coordinated baseline after the v0.27 roll-forward; earlier v0.26.0 and older sections remain historical rather than being silently rewritten as current guidance.
- Targeted docs use working repo-relative references to the analyzer README, compiled-compatibility note, README sections, and cited validation files.
- Documentation text does not claim runtime behavior changes, published package availability, or provider guarantees beyond the landed repository evidence.
- A repo text review of the touched docs shows the lifecycle diagnostics are described only within the bounded `DMV1912` through `DMV1914` contract and the no-runtime-change posture is preserved throughout.

## Implementation Notes
- `docs/architecture/dvault-ef-compiled-compatibility.md` already contains the accepted lifecycle contract wording; reuse that contract text instead of inventing new rule semantics in sibling docs.
- `src/DCoding.Data.DVault.Analyzers/README.md` and the top-level README are already partially aligned with `DMV1912` through `DMV1914`; treat them as partially landed surfaces that mainly need version and baseline consistency, not a fresh contract rewrite.
- `docs/production-adoption-checklist.md` still names v0.26.0 as current and still limits its analyzer-package bullet to `DMV1910`/`DMV1911` plus `DMV196x`; rolling that checklist forward is part of this ticket.
- The current README still frames v0.26.0 as the current coordinated release section and current limitations section; add a v0.27 current section and demote v0.26 material to historical context instead of rewriting history.
- Use `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` as the analyzer evidence source for supported IDs and safe/unsafe lanes, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs` as the fixed-shape compiled-model and compiled-query evidence source.
- Keep the root `dvault-ef-compiled-compatibility.md` file lightweight unless a small wording refresh is needed for validation references; the detailed lifecycle rules belong in the architecture note.
- The v0.27 release note should follow the existing release-note structure: package scope, boundary shift from the previous baseline, validation evidence, documentation updates, and limitations/non-goals, with explicit exclusions for runtime guards, provider-specific lifecycle behavior, whole-application inference, benchmark reruns, package publication, and release automation.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- The repository is already partially updated for `DMV1912` through `DMV1914`, so the main delivery risk is leaving a mixed baseline where README or checklist sections still present v0.26.0 or the older `DMV1910`/`DMV1911`-only story as current.
- A new v0.27 release note can easily over-claim runtime or provider behavior unless it mirrors the accepted contract and existing test evidence exactly.
- Versioned install snippets can imply package availability if the existing no-publication disclaimer is weakened or removed during the v0.27 roll-forward.
- The root compiled-compatibility entrypoint can drift from the architecture note if both files are expanded independently instead of keeping one authoritative source.

## Split Recommendations
- No further split is recommended; contract, implementation, fixtures, and documentation are already separated across sibling tickets, and this ticket is now a bounded documentation-alignment task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update the analyzer README, EF compiled compatibility note, README, production checklist, and release notes to describe the v0.27 analyzer boundary, diagnostics, limitations, and no-runtime-change posture.