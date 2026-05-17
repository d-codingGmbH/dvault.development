[gicket-bot] PO-critic review contract

Summary
- Repository and ticket evidence agree this is an already-covered, closure-only ticket: the link-parent satellite projection assertions and shared snapshot contract already exist, and the clarified baseline is metadata-first rather than fluent code-first.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:424-454 already contains ApplyDataVaultMetadataTranslatesLinkParentSatellites, asserting ParentReferenceKind=Link, ParentReferenceName=CustomerOrder, produced table SatCustomerOrderState, PK PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp, and index IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp.
- tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs:8-18,27-52 and tests/DCoding.Data.DVault.Tests/Shared/LiveSchemaReaderContractFixture.cs:14-20,23-39,143-158 already include the CustomerOrder/State canonical metadata and deterministic snapshot surface for SatCustomerOrderState.
- src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs:48-59 exposes Satellite(...) only on the hub builder, src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:67-68 materializes satellites only from _hubs, and docs/releases/v0.6.0.md:51 explicitly says link-parent satellite code-first declarations remain outside the bounded code-first surface.
- .gicket/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/description.md:28-45 records this as already covered/no-work-required and sets Open Questions to none.
- .gicket/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/comments/06F3E4E10AV1A6JBM4ERJFAYVW.md:10-18 answers the prior critic items by keeping the ticket on the closure/no-work-required path; git diff --name-only 8255bab49e9d45f27be9bff30118741d3e117b3d..HEAD changes only .gicket/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/* files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- If a future ticket is opened for fluent code-first link-parent satellites, write acceptance criteria against the exact public API to be added and require direct source evidence for that API surface.
- If broader provider-specific or extra scenario hardening is still wanted, scope it as a separate test-hardening ticket rather than reopening this closure-only ticket.

Implementation watchouts
- Do not treat this ticket as a request to add duplicate tests or code-first API work; the supported baseline evidenced here is metadata-first link-parent projection, and any fluent code-first expansion belongs on a separate feature ticket.
- If automation still routes the ticket into a dev lane, it should be handled as a closure/no-op confirmation rather than new repository work.

Non-blocking notes
- none

Split recommendations
- No split for this ticket itself. If needed later, open one separate feature ticket for fluent code-first link-parent satellite declarations and one separate hardening ticket for broader coverage expansion.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment