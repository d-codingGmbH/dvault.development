[gicket-bot] PO refinement contract

Summary
- Repository evidence shows link-parent satellite metadata projection is already covered; this ticket should be treated as no-work-required/already covered, with no child tickets, relation changes, attachments, or planning documents materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Close this ticket as no-work-required/already covered. The repository already has direct EF metadata projection coverage for a link-parent satellite in ApplyDataVaultMetadataTranslatesLinkParentSatellites, and the shared live-schema snapshot contract already includes SatCustomerOrderState.
- critic-item-2: `answered` - No new developer acceptance criteria are needed. Rewrite the contract as closure criteria that name the already-satisfied contract and suites: DataVaultEfMetadataTranslationTests.ApplyDataVaultMetadataTranslatesLinkParentSatellites and LiveSchemaReaderContractFixtureTests.ExpectedSqliteSnapshotDefinesDeterministicLiveSchemaContractSurface.
- critic-item-3: `answered` - Do not target code-first coverage here. Repository docs and source show link-parent satellite declarations are outside the supported code-first surface, so the only supported surface is metadata-first and EF projection, which is already covered.
- critic-item-4: `answered` - Confirmed. The previous acceptance criteria merely restated repository state that already exists; no uncovered regression, missing assertion, or missing suite delta is visible in the current tree for this ticket.
- critic-item-5: `answered` - Confirmed. Code-first is the wrong target surface for this ticket: Satellite(...) exists only on the hub builder, code-first satellite materialization comes only from hub declarations, and release docs explicitly keep link-parent satellite declarations metadata-first.

Clarifications
- Existing repository tests already assert the exact link-parent projection contract: ParentReferenceKind=Link, ParentReferenceName=CustomerOrder, produced table SatCustomerOrderState, and the expected key and index shape.
- Shared live-schema fixture coverage already includes the same CustomerOrder/State link-parent satellite contract, so this ticket does not need another snapshot-style test pass.
- The supported baseline for link-parent satellites is metadata-first rather than code-first; the historical code-first routing context should not reopen code-first test work on this ticket.
- Recent comments were empty in the provided ticket snapshot, and no provided relation context justified a split or relation change.
- No child tickets, relation changes, attachments, or planning documents were materialized in this run.

Scope In
- Verify whether an uncovered link-parent satellite metadata projection gap still exists.
- Close or reframe the ticket based on current repository evidence.
- Ratify the supported surface for this topic as metadata-first and EF projection rather than code-first.

Scope Out
- Adding new product or test code for already-covered link-parent satellite projection behavior.
- Expanding the code-first API to declare link-parent satellites.
- Broad provider-matrix, diagnostics, or feature work beyond confirming the already-covered contract.

Open questions
- none

Follow-up questions
- If the product roadmap now wants fluent code-first declaration support for link-parent satellites, should that be tracked as a separate feature ticket rather than through this closure-only test ticket?
- If broader provider-specific hardening beyond the current EF translation test plus shared snapshot fixture is still desired, should that be tracked as a separate test-hardening ticket?

Risks
- The only material risk is intent mismatch: if the original human intent was new code-first capability rather than projection-test coverage, closing this ticket will not deliver that feature.
- If this ticket is kept open instead of closed, the historical code-first routing context may keep sending reviewers toward an unsupported surface.

Split recommendations
- No split on this ticket. Close it as already covered. If needed later, open separate tickets for code-first link-parent satellite declarations and for any broader coverage expansion.

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