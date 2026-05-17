<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Confirmed the ticket is already covered by existing repository tests and should stay on the closure/no-work-required path rather than reopening development.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Existing unit coverage already asserts the link-parent translation contract for satellite State over link CustomerOrder, including ParentReferenceKind=Link, ParentReferenceName=CustomerOrder, produced table SatCustomerOrderState, and the expected primary-key and index shape.
- Existing shared live-schema fixture coverage already includes SatCustomerOrderState in the deterministic schema snapshot surface, so no new snapshot-style test pass is needed for this ticket.
- The supported v1 baseline for link-parent satellites remains metadata-first; the current fluent code-first builder surface does not declare link-parent satellites.
- This ticket is closure-only from the current evidence: no child tickets, relation edits, attachments, or planning documents were materialized in this run.
- The provided ticket snapshot shows no recent human comments and no provided relation context that reopens scope or justifies a split.

### Scope In
- Confirm whether link-parent satellite projection coverage already exists in the repository.
- Ratify the supported baseline for this topic as metadata-first EF projection rather than fluent code-first declaration.
- Refine the ticket as already covered/no-work-required and keep any future feature intent separate from this closure ticket.

### Scope Out
- Adding new product code, tests, or documentation for the already-covered link-parent satellite projection behavior.
- Reopening this ticket for fluent code-first link-parent satellite declaration support.
- Broad test hardening, provider-matrix expansion, diagnostics work, or release-note changes beyond confirming the already-covered baseline.

## Acceptance Criteria
- Repository evidence confirms existing unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs already translates a link-parent satellite and asserts ParentReferenceKind=Link, ParentReferenceName=CustomerOrder, SatCustomerOrderState, and the expected key/index shape.
- Repository evidence confirms existing shared snapshot coverage in tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs and tests/DCoding.Data.DVault.Tests/Shared/LiveSchemaReaderContractFixture.cs already includes SatCustomerOrderState with the expected deterministic schema surface.
- The refined contract explicitly treats link-parent satellite declarations as metadata-first, not fluent code-first, for the current DVault baseline.
- Any future request for fluent code-first link-parent satellite support is tracked as a separate feature ticket, not by reopening this closure ticket.

## Definition of Done
- The ticket contract records this work as already covered/no-work-required based on existing repository evidence.
- No repository code, test, documentation, attachment, relation, or planning-document changes are required under this ticket from the current evidence.
- Any later fluent code-first link-parent satellite request or broader coverage hardening is tracked outside this ticket as separate follow-up work.

## Implementation Notes
- Primary coverage evidence is the existing translation test in tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs and the shared snapshot contract in tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs backed by tests/DCoding.Data.DVault.Tests/Shared/LiveSchemaReaderContractFixture.cs.
- Source and release-note evidence narrow the supported declaration surface away from fluent code-first for link-parent satellites: src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs only exposes Satellite(...) on hubs, src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs only materializes satellites from hub declarations, and docs/releases/v0.6.0.md keeps link-parent satellite code-first declarations outside the bounded surface.
- No bounded planning writes were needed because current repository evidence already justifies closure without a split.

## Open Questions
- none

## Follow-Up Questions
- If the roadmap now wants fluent code-first declaration support for link-parent satellites, should a separate feature ticket be created for that capability?
- If broader provider-specific or additional scenario hardening beyond the current translation test plus shared snapshot fixture is still desired, should that be tracked as a separate test-hardening ticket?

## Risks
- If the original human intent was a new fluent code-first capability instead of confirming existing projection coverage, closing this ticket will not deliver that future feature and a separate feature ticket will be needed.

## Split Recommendations
- No split on this closure ticket. Keep it no-work-required/already covered. If needed later, open a separate feature ticket for fluent code-first link-parent satellite support and a separate hardening ticket for any broader coverage expansion.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add focused tests for link-parent satellite projection.

## Scope
- Refine and complete the work for "Test link-parent satellite metadata projection" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.