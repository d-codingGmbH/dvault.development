[gicket-bot] PO refinement contract

Summary
- Refined the ticket as a bounded v1 diagnostic-catalog slice around existing shipped diagnostics and one importer/projection validation-path integration.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already shows stable shipped diagnostic ids on the model-artifact path, including DMV1002 and DMV1801, plus category-scoped diagnostics such as parse and projection, so v1 should catalog current diagnostics rather than invent a new id family.
- The first wired validation path should be the existing model-artifact importer/projection diagnostics path because current tests already pin code, category, logical source path, and JSON pointer behavior there.
- No child tickets, relation changes, or planning attachments were needed; the ticket remains the existing child of 06F1XPS7KGKBP5SVMQPJC49J2G.

Scope In
- Add a core-package diagnostic definition model that captures the stable fields needed for DVault diagnostic discovery and focused validation tests.
- Add one centrally discoverable catalog seeded with the existing shipped diagnostics used by the model-artifact importer/projection path.
- Refactor one existing validation/emission path to resolve diagnostics from the central catalog instead of scattered inline literals.
- Add focused unit tests for deterministic catalog discoverability, id uniqueness, severity/category validity, and documentation coverage.

Scope Out
- No analyzer package, CLI, or new external diagnostics delivery surface.
- No broad rewrite of every validator or diagnostic emitter in the repository.
- No requirement in this ticket to migrate provider save/read strategy diagnostics or all diagnostics-service outputs unless they are part of the chosen first integration path.

Open questions
- none

Follow-up questions
- After the importer/projection path is catalog-backed, should later tickets migrate diagnostics service analysis, code-first validation, and provider strategy diagnostics to the same catalog?
- Once the internal catalog shape is stable, should a later documentation artifact publish the full supported diagnostic list for consumers?

Risks
- Existing diagnostics may currently be emitted from multiple call sites, so centralizing them without altering stable ids or visible behavior depends on careful regression coverage.
- If documentation coverage is defined too loosely, the first catalog can satisfy tests without creating a durable human-facing diagnostic contract.

Split recommendations
- No split recommended; the current scope is already a bounded first slice with one shared catalog model, focused tests, and one existing validation-path integration.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment