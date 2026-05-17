[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGJSXP18VKKV52QZA4NP30/description.md contains the authoritative delivery contract with explicit Scope In, Scope Out, Acceptance Criteria, Definition of Done, Implementation Notes, and `## Open Questions` set to `none`.
- .gicket/tickets/06F2PGJN1XCV8F7NWH567SQSKM/ticket.json shows the upstream contract ticket is `done`, and .gicket/relations/KM/30/06F2PGJN1XCV8F7NWH567SQSKM--06F2PGJSXP18VKKV52QZA4NP30--blocks.json records that contract as the blocker satisfied for this implementation ticket.
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj keeps the work inside the existing optional analyzer package (`IncludeBuildOutput=false`, `SuppressDependenciesWhenPacking=true`), and src/DCoding.Data.DVault.Analyzers/README.md says the package supplies analyzer assets only with `PrivateAssets="all"` guidance rather than a runtime reference.
- `rg -n "IIncrementalGenerator|ISourceGenerator|Generator" src/DCoding.Data.DVault.Analyzers -g '!**/bin/**' -g '!**/obj/**'` returned no matches, which matches the contract statement that the repository currently has analyzer/code-fix tooling but no source-generator implementation yet.
- src/DCoding.Data.DVault/IDataVaultLinkMapper.cs documents the unique-participant-hub-name-only v1 link boundary, src/DCoding.Data.DVault/IDataVaultSatelliteMapper.cs documents that the runtime satellite mapper contract spans both hub-parent and link-parent satellites, and src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs limits the existing typed save helper to ordinary hub-parent satellites only.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs, tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs show that analyzer tests, package-verification tests, and SQLite integration baselines already exist in the repository for this ticket to extend.
- `git diff --name-only develop...HEAD` lists only .gicket ticket/comment/event files, and `git show --stat --name-only 07130ad655eb` plus `git show --stat --name-only 565980654573f7a6304d882473f1dcd1a6ac2989` show PO handoff and PO-critic claim commits only, which is consistent with a pre-development handoff rather than missing implementation evidence.
- `test -f docs/releases/v0.12.0.md` returned `missing`, and .gicket/tickets/06F2PGJYY6S97B4Z8044D34K5C/ticket.json shows the documentation follow-up ticket is still `todo`, matching this ticket's explicit scope-out for broader v0.12 docs/release-note work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not include one concrete multi-active hub-parent satellite declaration plus expected generated-helper usage, so driving-key ordering and helper-consumption behavior need to be pinned down in implementation tests.
- The contract does not show one explicit negative declaration example for excluded link-parent satellites and the expected compile-time diagnostic outcome.
- The contract does not show one explicit negative declaration example for same-hub repeated-participant or ordinary self-link mappings, even though those shapes are intentionally excluded from v1.
- The contract does not include an example showing how generated multi-active satellite output should be consumed when the current `SaveOrdinaryHubSatelliteAsync(...)` helper rejects driving-key values.

Risky assumptions
- An implementer could assume the existing typed satellite save helper already covers multi-active generated output, but src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs rejects driving-key values and non-ordinary hub-parent targets.
- An implementer could widen generator support into link-parent satellites because src/DCoding.Data.DVault/IDataVaultSatelliteMapper.cs covers both hub-parent and link-parent satellites at the runtime contract level even though this ticket excludes link-parent generation.
- An implementer could widen link generation into repeated-participant or self-link shapes because DVault metadata can represent them elsewhere, even though src/DCoding.Data.DVault/IDataVaultLinkMapper.cs limits the typed-mapper v1 boundary to unique participant hub names by `StringComparer.Ordinal`.
- An implementer could try to fold README or `docs/releases/v0.12.0.md` work into this ticket unless the separate ownership of 06F2PGJYY6S97B4Z8044D34K5C is kept explicit during dev handoff.

AC / test suggestions
- Add generated-source golden tests for one hub, one unique-participant link, one ordinary hub-parent satellite, and one multi-active hub-parent satellite declaration, each asserting exact logical names, logical member order, and the emitted `DataVaultRegistry*SaveOperation` shape.
- Add compile-time diagnostic tests for malformed declarations and for excluded shapes, especially link-parent satellites and repeated-participant or self-link mappings.
- Add runtime/API tests and public API snapshot updates for any new consumer declaration surface or helper contract introduced in `src/DCoding.Data.DVault`.
- Add at least one SQLite end-to-end proof that generated helpers work through the existing registry-backed save pipeline with caller-supplied `loadTimestamp` and `recordSource`.

Implementation watchouts
- Keep generator implementation inside `src/DCoding.Data.DVault.Analyzers`, and keep consumer-authored declaration types plus any shared generated-helper contracts in `src/DCoding.Data.DVault`; do not introduce a new package family or analyzer-to-runtime dependency that breaks analyzer packaging.
- Do not execute `ApplyDataVaultMetadata(...)`, `DataVaultMetadataModel`, model-artifact import, or design-time command hosts during compilation; the contract requires compile-time-inspectable C# input only.
- Generated output may assemble existing `DataVaultRegistry*SaveOperation` objects or implement existing mapper interfaces, but it must not hide `SaveAsync`, `SaveChanges`, `loadTimestamp`, or `recordSource` ownership.
- Multi-active hub-parent support needs an explicit path that respects the current runtime boundary without assuming `SaveOrdinaryHubSatelliteAsync(...)` is sufficient for satellites with driving keys.

Non-blocking notes
- Current comment history under .gicket/tickets/06F2PGJSXP18VKKV52QZA4NP30/comments consists of PO refinement, handoff, lease, and orchestration comments only; no later comment introduces a new unresolved product question.
- The branch is still metadata-only at review time, which is expected for this pre-development quality gate and not a reason to return the ticket to PO.
- The separate documentation ticket 06F2PGJYY6S97B4Z8044D34K5C remains open, but the current contract correctly keeps broader release-note and README closure out of this implementation ticket.

Split recommendations
- No additional split is required before developer handoff; the existing separation between contract ticket 06F2PGJN1XCV8F7NWH567SQSKM, implementation ticket 06F2PGJSXP18VKKV52QZA4NP30, and documentation ticket 06F2PGJYY6S97B4Z8044D34K5C is still sufficient.
- If implementation grows, split follow-on work by excluded shape families or later save-helper ergonomics instead of widening the initial v1 generator slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment