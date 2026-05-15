[gicket-bot] PO refinement contract

Summary
- Refined the ticket using the persisted ticket contract, referenced repository documents, and branch state snapshot evidence. The critic's two process blockers are addressed by tying the next review to the visible local repository evidence in this prompt envelope rather than prompt seed alone.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The next PO-critic pass should verify against the declared local evidence surfaces now included in this run: persisted ticket contract text, referenced repository documents, and repository branch state snapshot. Those surfaces expose the analyzer project boundary, project metadata, Roslyn references, diagnostics catalog, analyzer implementation, README guidance context, solution/project layout summary, and test-root layout, which are the verification points the prior critic requested.
- critic-item-2: `answered` - The refinement is not based on prompt seed context alone. It uses the persisted delivery contract in the ticket snapshot plus direct repository evidence embedded in the branch snapshot and referenced repository documents. The remaining delivery contract keeps implementation scope bounded to package/build/test/docs readiness because the branch evidence already shows the v1 analyzer project and DMV1901/DMV1902 code-first rule slice are present, while packability remains an explicit implementation concern due to IsPackable=false.

Clarifications
- The v1 analyzer package boundary is src/DCoding.Data.DVault.Analyzers with root namespace DCoding.Data.DVault.Analyzers.
- The initial diagnostic baseline is DMV1901 for unsupported Code-First selector shape and DMV1902 for duplicate Code-First member declaration.
- DMV1901 and DMV1902 are CodeFirst category warning diagnostics with centralized descriptor metadata in CodeFirstDiagnosticCatalog.
- Current branch evidence shows IsPackable=false, so this story must either enable analyzer asset packing or document the deliberate package-boundary rationale for deferring packability.
- The completed child ticket 06F1XQ1JNMDXAKMS9NFJA0A3GW remains historical completed scope for the first analyzer rules/tests slice and does not block this ticket.

Scope In
- Finish analyzer package foundation readiness for DCoding.Data.DVault.Analyzers after the integrated DMV1901/DMV1902 rule slice.
- Make the analyzer project buildable and pack-ready, or document why packability remains deferred for this foundation boundary.
- Preserve DMV1901 and DMV1902 ids, CodeFirst category, warning severity, and centralized descriptor metadata.
- Keep coverage for unsupported BusinessKey, Payload, and DrivingKey selector shapes, duplicate declarations in the same applicable builder lambda scope, valid direct scalar selectors, separate satellite scopes, and selector variables outside the first direct-lambda slice.
- Add or update normal Roslyn analyzer package installation and suppression guidance.

Scope Out
- No new analyzer rule family beyond the first Code-First selector and duplicate-member slice unless required for package readiness.
- No Visual Studio, Rider, or other IDE extension outside normal Roslyn analyzer packaging conventions.
- No runtime behavior changes in DCoding.Data.DVault or provider packages.
- No EF design-time CLI shim, migration interception, or Microsoft.EntityFrameworkCore.Design dependency in the DVault runtime package.
- No broad heuristic analysis for metadata-first, model-first, or indirect patterns without high confidence.

Open questions
- none

Follow-up questions
- Should future analyzer tickets add diagnostics for missing business keys, suspicious satellite names, metadata-first/model-first artifact mistakes, or provider-specific governance checks after this Code-First foundation is accepted?
- Should the coordinated NuGet package family documented in README add a public DCoding.Data.DVault.Analyzers package id once packability is enabled?
- Should analyzer documentation live in README installation guidance or a dedicated docs/architecture or docs/quality page when the package is published?

Risks
- Analyzer packaging may still be incomplete if IsPackable remains false without a documented package-boundary rationale.
- Roslyn analyzer distribution details can be under-specified unless pack output is inspected to confirm analyzer assemblies land under analyzer assets rather than only normal library references.
- Broader future rules such as missing business keys can become noisy unless limited to high-confidence fluent scopes.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment