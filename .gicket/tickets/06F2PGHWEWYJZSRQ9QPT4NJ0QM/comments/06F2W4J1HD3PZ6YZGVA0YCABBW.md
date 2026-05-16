[gicket-bot] PO refinement contract

Summary
- Ratified this task as a closure-style refinement of the already-visible high-confidence analyzer slice: the local ticket store shows one parent story relation and one satisfied blocker, repository evidence already contains `DMV1901` and `DMV1902` plus tests, docs, and package assets, and no split or relation write is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Local ticket-store evidence shows `06F2PGHWEWYJZSRQ9QPT4NJ0QM` is a `parentOf` child of story `06F2PGHQ2GATEM13M5QK1MSX1G` and is blocked only by done epic `06F2PGFT8Z406HFBJGQSY7YRJ0`; no child tickets, attachment files, or extra relations are present for this task.
- The live ticket metadata places this task in milestone `v0.12.0 - Analyzer rule and code-fix expansion` and release `v0.12.0 - Analyzer and Generator Ergonomics`.
- Ticket comments are automation-only claim and lease comments; there is no human clarification to incorporate.
- Repository evidence already shows the intended high-confidence slice in `src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstAnalyzer.cs` and `src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs`: `DMV1901` for unsupported direct selector shapes and `DMV1902` for duplicate member declarations.
- Analyzer behavior is already bounded to `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)` on the first direct lambda slice, with intentional non-reporting for selector variables outside that slice and duplicate detection limited to one relevant fluent scope.
- Tests and package docs already exist in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs`, `src/DCoding.Data.DVault.Analyzers/README.md`, and `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`.
- No child tickets, relation rewrites, attachments, or planning documents were materialized in this refinement pass.

Scope In
- The existing high-confidence analyzer slice limited to `DMV1901` and `DMV1902` for Code-First `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)` declarations.
- Analyzer packaging and consumer guidance for the standalone `DCoding.Data.DVault.Analyzers` package, including analyzer asset packing and project-local usage guidance.
- Unit-test coverage for supported diagnostics, true positives, and false-positive guards on valid direct readable scalar selectors, separate satellite scopes, and selector variables outside the first direct-lambda slice.
- Repository consistency between analyzer source, tests, package README, and the already-recorded public description of this slice.

Scope Out
- Code fixes, refactorings, or automatic remediation behavior; no `CodeFixProvider` surface exists in the repository today.
- Broader model validation, metadata-first or model-first JSON diagnostics, provider diagnostics, migration guardrails, or generated SQL analysis.
- Dataflow-heavy or indirect selector analysis such as selector variables, nested flow reasoning, or whole-model duplicate validation beyond the current direct-lambda, high-confidence boundary.
- New package shape work or wider `v0.12.0` release-note aggregation; broader release documentation belongs with downstream documentation work.

Open questions
- none

Follow-up questions
- Should downstream documentation ticket `06F2PGJYY6S97B4Z8044D34K5C` explicitly carry the already-implemented `DMV1901` and `DMV1902` baseline into the eventual `v0.12.0` release-note set once that file is created?
- Should later analyzer tickets expand beyond the first direct-lambda slice into low-noise handling for indirect selectors or dataflow-backed selector variables?
- Should downstream story `06F2PGJBRXFCP038CN6XVAYSZM` attach mechanical code fixes to `DMV1901` and `DMV1902`, or reserve fixes for later diagnostics with clearer one-step remediations?

Risks
- Because the repository already contains this analyzer slice while the ticket is still planned under the `v0.12.0` graph, later work could accidentally reopen or over-expand a slice that is already historically documented unless the two-diagnostic boundary stays explicit.
- No `docs/releases/v0.12.0.md` file exists yet, so future release-note work could omit or duplicate the analyzer baseline if documentation ownership is not kept with the downstream docs ticket.
- If a developer treats this task as an invitation to add code fixes, broader model validation, or dataflow analysis, the current high-confidence, low-noise analyzer contract will expand beyond the intended scope.

Split recommendations
- No additional split is recommended; the current task is already a bounded implementation slice and adjacent work is already separated into analyzer documentation task `06F2PGJ28KVSZAAFRA40D94128`, downstream code-fix story `06F2PGJBRXFCP038CN6XVAYSZM`, and release-documentation task `06F2PGJYY6S97B4Z8044D34K5C`.
- Do not create child tickets for broader diagnostics, code fixes, or generator work from this task; keep those as separate follow-on scopes.
- Do not materialize relation cleanup from this pass; the live relation state is already coherent for a closure-style refinement of this ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment