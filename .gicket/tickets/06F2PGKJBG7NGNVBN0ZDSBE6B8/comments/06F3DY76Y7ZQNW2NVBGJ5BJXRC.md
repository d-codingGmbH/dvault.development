[gicket-bot] PO refinement contract

Summary
- Refined the ticket as a focused unit-test task for existing link-parent satellite metadata projection coverage; no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already shows existing unit test homes under tests/DCoding.Data.DVault.Tests/Unit for link declarations, metadata translation, schema parity, and EF metadata projection; this ticket should extend those suites instead of adding new test infrastructure.
- The visible provider-neutral annotation contract already exposes DataVaultAnnotationNames.ParentReferenceKind and DataVaultAnnotationNames.ParentReferenceName, so the bounded v1 assertion surface is the projected parent-link identity for a satellite.
- No recent human comments were supplied, and no repository evidence justified a split or ticket-relation change in this run.

Scope In
- Add focused automated coverage for a satellite declared against a link parent through the existing metadata translation and projection test paths.
- Assert that the projected satellite metadata preserves the correct parent-link kind and name and stays aligned with current naming and annotation conventions.
- Capture verification steps for the targeted unit suite or suites that own this contract.

Scope Out
- New runtime behavior for link-parent satellites or broad refactoring outside existing test helpers.
- Provider-specific SQL, external-database fixture, or integration-suite expansion unless an existing unit-level contract cannot express the regression.
- README, release-note, package, or public API changes for this test-only ticket.

Open questions
- none

Follow-up questions
- If this regression exposes a similar gap in diagnostics or design-time explain coverage, should that broader matrix be tracked as a separate follow-up ticket instead of widening this task?

Risks
- Covering only one projection layer could miss a parallel regression if both metadata-model translation and EF-model projection currently maintain their own assertions; add the minimum adjacent coverage needed to close that gap.
- Existing test helpers may encode hub-parent assumptions, so a small amount of test-only helper reshaping may be needed to express a link-parent satellite case without changing product code.

Split recommendations
- No split recommended; the repository already has bounded unit test surfaces for this contract, so the work remains a single focused testing ticket.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment