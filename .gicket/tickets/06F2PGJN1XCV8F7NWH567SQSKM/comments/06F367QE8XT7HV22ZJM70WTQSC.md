[gicket-bot] PO-critic review contract

Summary
- The persisted contract is sufficiently bounded for pre-development handoff: package placement, supported target shapes, runtime boundary, validation ownership, and downstream ticket routing are explicit, and the remaining gaps are example-level rather than PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGJN1XCV8F7NWH567SQSKM/description.md contains the authoritative delivery contract with explicit Scope In, Scope Out, Acceptance Criteria, Definition of Done, Implementation Notes, and an Open Questions section set to none.
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj defines the existing packable analyzer package boundary, and src/DCoding.Data.DVault.Analyzers/README.md states consumers install it with PrivateAssets=all and that it is not a runtime reference.
- find src -maxdepth 1 -mindepth 1 -type d lists only the existing package roots, rg -n for ISourceGenerator, IIncrementalGenerator, or [Generator] under src/DCoding.Data.DVault.Analyzers returned no matches, and rg --files src/DCoding.Data.DVault.Analyzers lists analyzer and code-fix files only, so there is no separate generator package or current generator implementation on this branch.
- src/DCoding.Data.DVault/IDataVaultHubMapper.cs, src/DCoding.Data.DVault/IDataVaultLinkMapper.cs, src/DCoding.Data.DVault/IDataVaultSatelliteMapper.cs, and src/DCoding.Data.DVault/DataVaultSaveService.cs expose the current runtime boundary: typed mappers return DataVaultRegistryHubSaveOperation, DataVaultRegistryLinkSaveOperation, and DataVaultRegistrySatelliteSaveOperation; the link mapper remarks limit v1 to unique participant hub names; DataVaultRegistrySaveRequest requires caller-supplied loadTimestamp and recordSource.
- .gicket/relations/KM/30/06F2PGJN1XCV8F7NWH567SQSKM--06F2PGJSXP18VKKV52QZA4NP30--blocks.json shows this contract ticket blocks implementation ticket 06F2PGJSXP18VKKV52QZA4NP30, and git show --stat --name-only 44bf8769d, fc42924c2, and e6eb88799 changed only .gicket ticket/comment/event files, which is consistent with a pre-development contract handoff rather than missing code work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete example declaration is included for one multi-active hub-parent satellite mapping, so ordered driving-key and payload binding behavior should be pinned down by implementation tests.
- No explicit negative example shows the expected compile-time diagnostic for excluded link-parent satellites.
- No explicit negative example shows the expected behavior for same-hub repeated-participant or ordinary self-link mappings, which the ticket excludes from v1.

Risky assumptions
- Because src/DCoding.Data.DVault/IDataVaultSatelliteMapper.cs says the runtime satellite mapper contract covers both hub-parent and link-parent satellites, an implementer could accidentally widen generator support past the ticket's hub-parent-only v1 scope.
- Because src/DCoding.Data.DVault/IDataVaultLinkMapper.cs documents only unique participant hub names, an implementer could incorrectly assume same-hub or self-link generation is allowed just because metadata can represent those shapes elsewhere in DVault.
- Because docs/releases currently stops at v0.11.0, an implementer could try to fold v0.12.0 release-note work into this ticket even though the contract delegates that follow-through to 06F2PGJYY6S97B4Z8044D34K5C.

AC / test suggestions
- Add contract tests for one hub, one unique-participant link, one ordinary hub-parent satellite, and one multi-active hub-parent satellite declaration that each generate the expected DataVaultRegistry*SaveOperation shape.
- Add diagnostic tests that malformed declarations fail at compile time, while missing required mapped values still fail in the existing operation constructors or IDataVaultSaveService pipeline.
- Add negative tests that link-parent satellites and same-hub repeated-participant or self-link mappings are diagnosed or rejected as out of scope for v1.

Implementation watchouts
- Keep generator work inside DCoding.Data.DVault.Analyzers and preserve analyzer-only packaging; do not introduce a new generator package family.
- Do not execute ApplyDataVaultMetadata(...), DataVaultMetadataModel, DataVaultModelArtifactImporter, or a design-time command host during compilation.
- Generated helpers may construct DataVaultRegistry*SaveOperation objects or implement the existing typed mapper interfaces, but they must not hide SaveAsync, SaveChanges, loadTimestamp, or recordSource ownership.
- Because the runtime satellite mapper surface already spans link-parent satellites, generator diagnostics need an explicit guard so v1 support stays limited to hub-parent satellites.

Non-blocking notes
- docs/releases currently contains v0.5.0 through v0.11.0 only; the contract correctly keeps v0.12.0 documentation outside this ticket and routes it to 06F2PGJYY6S97B4Z8044D34K5C.
- The latest ticket comments are operational handoff and relation-follow-up comments only; none introduce a new unresolved product question after the persisted contract was written.
- The current branch history for this ticket is ticket-metadata-only, which is expected for a pre-development contract task and not a reason to return it to PO.

Split recommendations
- No additional split is needed for this contract ticket.
- If implementation grows, split follow-on work by excluded shape families such as link-parent satellites, repeated-participant or self-link handling, or higher-level save wrappers instead of widening v1.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment