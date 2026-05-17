[gicket-bot] PO refinement contract

Summary
- Epic scope is already fully materialized and completed: the direct child split is done, repository evidence matches the v0.12 analyzer/code-fix/source-generator/docs baseline, and no new planning artifacts or relation changes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Local ticket-store evidence shows four direct parentOf children under this epic and all four are done: 06F2PGHQ2GATEM13M5QK1MSX1G (Expand Code-First analyzer diagnostics), 06F2PGJBRXFCP038CN6XVAYSZM (Add code fixes for common DVault analyzer findings), 06F2PGJGDGMXHPT1VP0ASQ5HJ4 (Add source-generated metadata helper foundation), and 06F2PGJYY6S97B4Z8044D34K5C (Update v0.12.0 documentation and release notes).
- Nested child work is also already closed: analyzer story 06F2PGHQ2GATEM13M5QK1MSX1G has done tasks 06F2PGHWEWYJZSRQ9QPT4NJ0QM and 06F2PGJ28KVSZAAFRA40D94128, and generator story 06F2PGJGDGMXHPT1VP0ASQ5HJ4 has done tasks 06F2PGJN1XCV8F7NWH567SQSKM and 06F2PGJSXP18VKKV52QZA4NP30.
- Repository evidence matches that split: src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstAnalyzer.cs and DataVaultCodeFirstCodeFixProvider.cs cover analyzer/code-fix ergonomics, DataVaultMappingSourceGenerator.cs and DataVaultMappingDiagnosticCatalog.cs cover generated mapper helpers and DMV1950-DMV1955, and tests/DCoding.Data.DVault.Tests/Analyzers plus typed-mapper tests cover the shipped behavior.
- Public documentation is already aligned on the current baseline: README.md, src/DCoding.Data.DVault.Analyzers/README.md, and docs/releases/v0.12.0.md describe the optional analyzer package, bounded code fixes, generated mapper helpers, and the preserved explicit save boundary.
- Epic comments currently contain only automation claim/lease records; there are no human comments or attachment-driven clarifications to incorporate.
- Outgoing blocks relations to later Code-First parity tickets 06F2PGK4QJ0YGXK5479W83Z2J0, 06F2PGKAQVVF8GEZVVC8SHFASG, 06F2PGKJBG7NGNVBN0ZDSBE6B8, 06F2PGKV9AFAMKGJEKKZ3AXHGC, and 06F2PGM1HQ5W1M2H8T50MZ3EEC remain consistent as downstream sequencing for later release work; no relation cleanup was justified in this pass.
- No child tickets, relation writes, attachments, or planning documents were created during this refinement run.

Scope In
- Closure-only epic roll-up of the shipped v0.12 analyzer and generator ergonomics slice across analyzer diagnostics, bounded code fixes, generated mapper helpers, and release-ready documentation.
- Direct child-ticket completion status and existing nested split as the authoritative planning structure for this epic.
- The public v0.12 behavior baseline: DMV1901/DMV1902, bounded code fixes, DMV1950-DMV1955, compile-time mapping attributes, generated IDataVault*Mapper<TSource> helpers, and the explicit IDataVaultSaveService boundary.
- Epic-level confirmation that downstream work can rely on the current ergonomics baseline without reopening this release scope.

Scope Out
- New analyzer diagnostics, broader code-fix automation, or dataflow-heavy/full-model validation beyond the shipped v0.12 slice.
- Generated support for later Code-First parity shapes such as link-parent satellites, effectivity satellites, same-as links, or dependent child key modeling.
- A new runnable generator-based sample app, broader post-release docs expansion, or unrelated package/runtime refactoring.
- Relation-graph reshaping beyond the already-materialized child split and still-valid downstream blocks sequencing.

Open questions
- none

Follow-up questions
- Should the later Code-First parity epic 06F2PGK4QJ0YGXK5479W83Z2J0 treat the current analyzer/generator baseline as fixed and add new generator/analyzer support only through separate follow-on tickets per shape?
- Should a later docs/examples ticket add a runnable end-to-end consumer sample that uses the compile-time mapping attributes and generated mappers?
- If future releases add more DMV195x coverage or broader generated shapes, should root-level docs grow a compact capability table while keeping suppression and detailed rule guidance package-local?

Risks
- If later work reopens this epic instead of using the already-linked downstream tickets, the clean v0.12 release boundary will blur into later Code-First parity scope.
- If future analyzer/generator changes are not kept aligned across README.md, src/DCoding.Data.DVault.Analyzers/README.md, and docs/releases, the public release narrative can drift from the shipped package behavior.
- If later documentation presents generated helpers as a new metadata authority or hidden persistence layer, adopters may misinterpret the preserved explicit-save boundary.

Split recommendations
- No additional split is recommended; the existing direct and nested child-ticket structure is already sufficient and completed.
- Keep later Code-First parity expansion in the already-linked downstream epic 06F2PGK4QJ0YGXK5479W83Z2J0 and its child tickets instead of widening this v0.12 epic.
- If future ergonomics work adds new generated shapes, deeper analyzer rules, or runnable examples, create new follow-on tickets rather than reopening this epic.

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