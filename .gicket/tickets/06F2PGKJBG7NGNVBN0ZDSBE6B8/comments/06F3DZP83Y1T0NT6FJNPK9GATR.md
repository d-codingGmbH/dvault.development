[gicket-bot] PO-critic review contract

Summary
- Return to PO: the refined contract does not identify a concrete remaining gap, and the repository already contains direct link-parent satellite projection coverage for the contract the ticket describes.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git -C /mnt/c/Projects/DVault diff --name-status develop..HEAD` lists only `.gicket/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/**` files; there are no `src/**` or `tests/**` changes on the ticket branch.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:424-454` already contains `ApplyDataVaultMetadataTranslatesLinkParentSatellites`, which asserts `ParentReferenceKind == Link`, `ParentReferenceName == "CustomerOrder"`, and the `SatCustomerOrderState` PK/index shape.
- `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs:48-51` already snapshots `SatCustomerOrderState`, including `PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp` and `IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:161-168` already verifies `DataVaultSatelliteMetadata` retains a link parent via `DataVaultMetadataReference.Link("CustomerOrder")`.
- `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:67-69` builds satellites only from `_hubs`, and `src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs:18-23` exposes only `Participant<TEntity>()`; no visible code-first link-parent satellite declaration surface was found.
- The persisted contract at `.gicket/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/description.md:41-42` says `## Open Questions` -> `none`, and `.gicket/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/description.md:14` says no recent human comments were supplied.
- Inspecting `.gicket/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/comments/*.md` showed bot workflow comments only; no human clarification comment explains what gap remains beyond the existing tests.

Blocking findings
- The acceptance criteria describe repository state that already exists in the current tree, but the ticket does not name the uncovered regression, missing assertion, or missing suite delta that still requires developer work.
- The contract points to code-first test patterns without source-backed evidence that link-parent satellites are declarable through the code-first surface, so the intended developer target is ambiguous.

Required PO actions
- Decide whether this ticket should be closed as no-work-required/already covered, or rewritten around a concrete uncovered gap that is not already satisfied by `ApplyDataVaultMetadataTranslatesLinkParentSatellites` and the existing snapshot coverage.
- If work is still intended, update the acceptance criteria to name the exact missing contract and the exact target suite instead of restating the already-present baseline.
- If code-first coverage is intended, provide source-backed justification for that surface; otherwise narrow the ticket to the metadata-first / EF projection path that is actually supported.

Open issues ledger
- critic-item-1 [required-po-action] Decide whether this ticket should be closed as no-work-required/already covered, or rewritten around a concrete uncovered gap that is not already satisfied by `ApplyDataVaultMetadataTranslatesLinkParentSatellites` and the existing snapshot coverage.
- critic-item-2 [required-po-action] If work is still intended, update the acceptance criteria to name the exact missing contract and the exact target suite instead of restating the already-present baseline.
- critic-item-3 [required-po-action] If code-first coverage is intended, provide source-backed justification for that surface; otherwise narrow the ticket to the metadata-first / EF projection path that is actually supported.
- critic-item-4 [blocking-finding] The acceptance criteria describe repository state that already exists in the current tree, but the ticket does not name the uncovered regression, missing assertion, or missing suite delta that still requires developer work.
- critic-item-5 [blocking-finding] The contract points to code-first test patterns without source-backed evidence that link-parent satellites are declarable through the code-first surface, so the intended developer target is ambiguous.

Missing examples / edge cases
- A concrete failing example that the current `SatCustomerOrderState` tests do not already catch.
- Whether the intended missing delta is EF metadata translation, live-schema/snapshot parity, diagnostics, or another projection layer.
- What broader coverage, if any, is intentionally deferred once the minimal missing assertion is identified.

Risky assumptions
- That link-parent satellite projection is currently untested, despite direct existing unit and snapshot coverage.
- That code-first suites are an appropriate target for this ticket, despite no visible code-first link-parent satellite builder API.
- That a developer can infer the remaining scope without a human clarification comment or a delta-oriented acceptance criterion.

AC / test suggestions
- Rewrite the acceptance criteria around one named delta, not the already-satisfied baseline; for example, require one specific uncovered assertion or one specific uncovered suite only.
- Name the intended verification command or suite explicitly in the contract once the real missing surface is identified.

Implementation watchouts
- Without narrowing the gap, a developer is likely to duplicate the existing `SatCustomerOrderState` assertions instead of adding new signal.
- Do not route work into code-first link tests unless PO first confirms a supported link-parent satellite declaration path.

Non-blocking notes
- The contract has no unresolved `Open Questions`, so the return decision is based on scope clarity and repo mismatch, not on an open-question policy failure.
- The branch is cleanly bounded to ticket metadata/workflow files, so this is a ticket-definition issue rather than a hidden in-flight code change issue.

Split recommendations
- No split is needed if PO closes the ticket as already covered. If PO identifies more than one real uncovered layer, split the minimal missing test surface from any broader diagnostics/parity follow-up.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment