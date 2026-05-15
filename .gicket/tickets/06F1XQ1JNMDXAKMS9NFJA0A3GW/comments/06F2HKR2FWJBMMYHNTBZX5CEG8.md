[gicket-bot] PO refinement contract

Summary
- Resolved the PO-critic blockers by reserving DMV1901 and DMV1902 inside a new DMV1901-DMV1999 CodeFirst analyzer band and by making analyzer-local mirrored diagnostic metadata the explicit package-boundary decision for this task.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This task now reserves DMV1901-DMV1999 as the first CodeFirst analyzer diagnostic band and assigns the two concrete rule ids up front: DMV1901 for unsupported selector shapes and DMV1902 for duplicate logical member declarations. The category convention for both rules is CodeFirst.
- critic-item-2: `answered` - The diagnostic metadata for this ticket lives in the analyzer assembly, not in the current core-package internal catalog. The analyzer project may define analyzer-local descriptor or catalog entries that mirror the established DVault diagnostic fields: id, category, title, message, and remediation guidance.
- critic-item-3: `answered` - The package-boundary decision has been copied into this task instead of being left implicit on parent story 06F1XQ15J5JEC92T1QCE9TABBM. Broader analyzer foundation work stays with the parent, but this task is explicitly allowed to ship the first two rules with analyzer-local mirrored diagnostic metadata and does not wait for a new shared public diagnostics contract.
- critic-item-4: `answered` - Stable user-visible ids are no longer left to implementation. DMV1901 is reserved for the selector-shape rule and DMV1902 for the duplicate-member rule, both inside the new DMV1901-DMV1999 CodeFirst analyzer band.
- critic-item-5: `answered` - The task no longer assumes reuse of the internal core diagnostic catalog across package boundaries. It now explicitly requires analyzer-local metadata definitions that mirror the DVault contract fields and match the established catalog style, which closes the package/catalog ownership gap without broadening this task into a shared public contract extraction.

Clarifications
- Reserve DMV1901-DMV1999 as the first Roslyn CodeFirst analyzer diagnostic band. This ticket consumes DMV1901 and DMV1902, and later CodeFirst analyzer tickets should continue at DMV1903+ unless a later PO contract says otherwise.
- DMV1901 is the selector-shape diagnostic for unsupported BusinessKey(...), Payload(...), or DrivingKey(...) selectors such as anonymous-object, method-call, nested-member, or collection selectors. Use category CodeFirst and remediation guidance that directs callers to repeated direct readable scalar member selectors.
- DMV1902 is the duplicate-member diagnostic for repeated logical member names within the same BusinessKey(...), Payload(...), or DrivingKey(...) fluent scope. Use category CodeFirst and remediation guidance that directs callers to declare each logical member name at most once per relevant scope.
- Analyzer diagnostic metadata for this ticket lives in the analyzer assembly as analyzer-local descriptor or catalog definitions that mirror the established DVault diagnostic fields: id, category, title, message, and remediation guidance.
- This task does not require the current core-package internal diagnostic catalog types to become public, shared, or directly referenced by the analyzer project.
- The package-boundary decision is now explicit in this task: parent story 06F1XQ15J5JEC92T1QCE9TABBM retains broader analyzer-foundation concerns, but this implementation task may ship the first two rules with analyzer-local mirrored metadata.

Scope In
- Create the minimal Roslyn analyzer project boundary and analyzer-test infrastructure needed for DVault within existing src/ and tests/ conventions if that scaffolding is not already present.
- Reserve the DMV1901-DMV1999 CodeFirst analyzer band and implement DMV1901 and DMV1902 within it.
- Implement DMV1901 for unsupported direct-member selector misuse in BusinessKey(...), Payload(...), and DrivingKey(...) calls inside DVault Code-First configuration.
- Implement DMV1902 for duplicate logical member declarations within the same relevant BusinessKey(...), Payload(...), or DrivingKey(...) fluent scope.
- Define analyzer-local diagnostic metadata in the analyzer project that mirrors the established DVault diagnostic fields and conventions.
- Add positive and negative analyzer test samples for each rule, including true-positive diagnostics and false-positive guards on valid direct scalar member declarations.

Scope Out
- Broader analyzer coverage such as missing business keys, link participant ordering, missing hubs, or duplicate metadata names outside the first two bounded rules.
- Model-first JSON validation, migration guardrail diagnostics, or any non-Roslyn validation surface already covered by existing DMV#### or DVM2xxx runtime diagnostics.
- NuGet packaging polish, installation docs, suppression docs, and broader analyzer-package foundation work beyond the minimal scaffolding needed to compile and test these rules.
- Extracting a new shared public diagnostics-contract package or making the current core-package internal diagnostic catalog public just to satisfy this ticket.
- Non-trivial code fixes or broad semantic or dataflow analysis that would increase false positives in the first analyzer slice.

Open questions
- none

Follow-up questions
- After the first analyzer slice ships, should parent story 06F1XQ15J5JEC92T1QCE9TABBM extract a shared public diagnostics contract if multiple analyzer assemblies need to reuse the same catalog metadata?
- After DMV1901 and DMV1902 land, should the next analyzer tickets continue consuming DMV1903+ within the same CodeFirst band or reserve a separate family for non-CodeFirst analyzers?
- Should a later ticket add code fixes for DMV1901 and DMV1902, or should the analyzer package remain diagnostics-only for v0.10.0?

Risks
- If implementation tries to reuse or expose the current internal core diagnostic catalog from src/DCoding.Data.DVault inside this ticket, the work will expand back into the parent analyzer-foundation story.
- Because current test projects disable analyzer execution, the ticket can still appear complete without real analyzer coverage unless the explicit harness is added.
- If later analyzer tickets ignore the reserved DMV1901-DMV1999 CodeFirst band, DVault diagnostic numbering will drift across analyzer work.
- If the analyzer attempts to infer builder state across locals, helper methods, or complex control flow in v1, false positives and false negatives will rise quickly.

Split recommendations
- No immediate split is required for PO-critic readiness; the task remains well-bounded once DMV1901, DMV1902, and the analyzer-local metadata decision are made explicit.
- If the team later wants a shared public diagnostics contract across multiple analyzer assemblies, create a follow-up under parent story 06F1XQ15J5JEC92T1QCE9TABBM rather than expanding this task.
- If broader analyzer coverage such as missing business keys or link-participant validation is wanted next, create a follow-up task after this first low-noise rule pair ships.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment