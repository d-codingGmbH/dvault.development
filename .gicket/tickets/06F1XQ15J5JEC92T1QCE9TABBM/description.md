<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket using the persisted ticket contract, referenced repository documents, and branch state snapshot evidence. The critic's two process blockers are addressed by tying the next review to the visible local repository evidence in this prompt envelope rather than prompt seed alone.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 analyzer package boundary is src/DCoding.Data.DVault.Analyzers with root namespace DCoding.Data.DVault.Analyzers.
- The initial diagnostic baseline is DMV1901 for unsupported Code-First selector shape and DMV1902 for duplicate Code-First member declaration.
- DMV1901 and DMV1902 are CodeFirst category warning diagnostics with centralized descriptor metadata in CodeFirstDiagnosticCatalog.
- Current branch evidence shows IsPackable=false, so this story must either enable analyzer asset packing or document the deliberate package-boundary rationale for deferring packability.
- The completed child ticket 06F1XQ1JNMDXAKMS9NFJA0A3GW remains historical completed scope for the first analyzer rules/tests slice and does not block this ticket.

### Scope In
- Finish analyzer package foundation readiness for DCoding.Data.DVault.Analyzers after the integrated DMV1901/DMV1902 rule slice.
- Make the analyzer project buildable and pack-ready, or document why packability remains deferred for this foundation boundary.
- Preserve DMV1901 and DMV1902 ids, CodeFirst category, warning severity, and centralized descriptor metadata.
- Keep coverage for unsupported BusinessKey, Payload, and DrivingKey selector shapes, duplicate declarations in the same applicable builder lambda scope, valid direct scalar selectors, separate satellite scopes, and selector variables outside the first direct-lambda slice.
- Add or update normal Roslyn analyzer package installation and suppression guidance.

### Scope Out
- No new analyzer rule family beyond the first Code-First selector and duplicate-member slice unless required for package readiness.
- No Visual Studio, Rider, or other IDE extension outside normal Roslyn analyzer packaging conventions.
- No runtime behavior changes in DCoding.Data.DVault or provider packages.
- No EF design-time CLI shim, migration interception, or Microsoft.EntityFrameworkCore.Design dependency in the DVault runtime package.
- No broad heuristic analysis for metadata-first, model-first, or indirect patterns without high confidence.

## Acceptance Criteria
- src/DCoding.Data.DVault.Analyzers builds as the Roslyn analyzer package boundary and has package metadata suitable for packing, or includes a documented reason for any remaining packability switch.
- SupportedDiagnostics exposes at least DMV1901 and DMV1902 with CodeFirst category, warning severity, clear title, explanation, and remediation text.
- DMV1901 reports unsupported BusinessKey(...), Payload(...), and DrivingKey(...) selector shapes only when the analyzer can identify a first direct lambda argument that is not one readable scalar member on the configured entity type.
- DMV1902 reports duplicate logical member declarations within the same applicable builder lambda scope and does not report duplicates across separate satellite scopes.
- Analyzer tests cover positive diagnostics and non-reporting cases for valid direct scalar selectors, separate scopes, and selector variables intentionally outside the first direct-lambda slice.
- Documentation or package guidance explains installation through normal Roslyn analyzer package conventions and how a consumer suppresses diagnostics when intentionally accepting a pattern.

## Definition of Done
- Analyzer project and analyzer test project are included in the repository-level solution or documented build/test entry point used for this package foundation.
- Relevant analyzer tests pass for the Code-First analyzer coverage.
- Package metadata is present for the analyzer package boundary, including package id, description, repository metadata where the repo convention expects it, and analyzer asset packing behavior if IsPackable is enabled.
- The implementation does not introduce runtime DVault behavior changes or provider-specific requirements.
- Repository formatting and one-member-per-file policy remain satisfied for touched files.
- Relation context remains reflected: parent epic 06F1XQ0T5WQWN1AES5Z3E0RMSR, done child 06F1XQ1JNMDXAKMS9NFJA0A3GW, and done blockers 06F1XPS7KGKBP5SVMQPJC49J2G and 06F1XPX99KQRB09GRQG50Z75FM.

## Implementation Notes
- Continue from the existing conservative syntax-node analyzer over invocation expressions and builder lambda scopes unless a broader Roslyn operation analyzer is needed for correctness.
- Use the existing CodeFirstDiagnosticCatalog shape for descriptors and keep diagnostic metadata centralized there.
- Use README Code-First and planning-document conventions as the v1 semantic baseline: BusinessKey, Payload, and DrivingKey selectors are repeated direct readable scalar member selectors.
- The analyzer project currently has IsPackable=false; remaining development should either make the analyzer package packable with analyzer asset metadata or explicitly document why this foundation ticket only establishes the boundary and defers packability.
- Keep Microsoft.CodeAnalysis and Microsoft.CodeAnalysis.CSharp reference behavior compatible with Roslyn analyzer distribution and avoid turning compiler assemblies into normal consumer package dependencies.
- Do not add EF design-time CLI integration or Microsoft.EntityFrameworkCore.Design dependencies to DVault runtime packages for this analyzer foundation story.

## Open Questions
- none

## Follow-Up Questions
- Should future analyzer tickets add diagnostics for missing business keys, suspicious satellite names, metadata-first/model-first artifact mistakes, or provider-specific governance checks after this Code-First foundation is accepted?
- Should the coordinated NuGet package family documented in README add a public DCoding.Data.DVault.Analyzers package id once packability is enabled?
- Should analyzer documentation live in README installation guidance or a dedicated docs/architecture or docs/quality page when the package is published?

## Risks
- Analyzer packaging may still be incomplete if IsPackable remains false without a documented package-boundary rationale.
- Roslyn analyzer distribution details can be under-specified unless pack output is inspected to confirm analyzer assemblies land under analyzer assets rather than only normal library references.
- Broader future rules such as missing business keys can become noisy unless limited to high-confidence fluent scopes.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Warn developers about common DVault modeling mistakes while they write EF Core configuration code.

## Scope In

- Create an analyzer package project or documented package boundary.
- Implement initial rules for missing business keys, suspicious satellite configuration, duplicate metadata names, or unsupported declaration patterns.
- Reuse stable diagnostic ids where possible.
- Add tests and packaging metadata.

## Scope Out

- No IDE extension outside Roslyn conventions.
- No analyzer for every DVault rule in the first release.
- No runtime behavior changes.

## Acceptance Criteria

- Analyzer package builds and packs with metadata.
- At least two useful diagnostics are covered by tests.
- Rules avoid noisy false positives.
- Docs show installation and suppression guidance.

## Implementation Notes

- Prefer high-confidence rules over broad heuristics.

## Open Questions

- none