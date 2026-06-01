[gicket-bot] PO-critic review contract

Summary
- Ticket is refined to an evidence-backed pre-development contract for support-bundle-driven typed bridge helpers, with no unresolved Open Questions and no PO blocker for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F7Y0HJ1ZPY7ND9N8RVS92H4C/description.md contains the authoritative Delivery Contract with 6 acceptance-criteria bullets, 4 Definition of Done bullets, and `## Open Questions` set to `- none`.
- .gicket/relations/4C/Z0/06F7Y0HJ1ZPY7ND9N8RVS92H4C--06F7Y0HZKHBHMYX9EYDYFRYXZ0--blocks.json records this ticket blocks downstream docs task `06F7Y0HZKHBHMYX9EYDYFRYXZ0`, and .gicket/tickets/06F7Y0HZKHBHMYX9EYDYFRYXZ0/ticket.json keeps that documentation work as a separate `todo` task.
- docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md states the implemented typed read-model generator baseline is support-bundle-driven and satellite-only, and that PIT/bridge helper generation is additive; docs/releases/v0.24.0.md and src/DCoding.Data.DVault.Analyzers/README.md repeat the same satellite-only baseline.
- src/DCoding.Data.DVault/DataVaultBridgeTraversalEndpoint.cs defines the closed traversal vocabulary `From`, `To`, `Ancestor`, and `Descendant`; src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs requires `maximumDepth` for hierarchy bridges, rejects depth on many-to-many bridges, and accepts zero-or-greater depth.
- src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs currently routes support-bundle bridge entities into `ReportUnsupportedSupportBundleBridge(...)`, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs asserts current bridge outcomes `DMV1964` and `DMV1969`; the ticket correctly scopes work as replacing the current bridge skip-only path for supported shapes.
- src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs already fixes the existing helper conventions this story must reuse: namespace suffix `DVault.GeneratedReadModels`, row type `{ProducedName}ReadModel`, extension type `{ProducedName}ReadExtensions`, and compatibility constants `ProducedTableName`, `MetadataSourceKind`, `MetadataSourceFingerprint`, `{MemberName}ProducedColumnName`, and `{MemberName}MappedName`.
- git diff --name-only d8c926189f1b7d02c5872fcdc04077e273a427a4..HEAD lists only .gicket/tickets/06F7Y0HJ1ZPY7ND9N8RVS92H4C comment/description/event/ticket files, so branch history is consistent with a pre-development refinement handoff rather than unfinished implementation work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract implies, but does not spell out with an explicit example, that hierarchy `maximumDepth=0` remains valid because current runtime validation in `src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs` accepts zero-or-greater depth.
- The contract calls for helper isolation on diagnostics, but it does not illustrate a concrete mixed support-bundle example where one bridge entity is skipped with `DMV1964`/`DMV1967`/`DMV1969` while unrelated satellite generation still succeeds.

Risky assumptions
- This story assumes representative support-bundle export continues to include the `readShape.bridge` facts documented in `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md`; if endpoint order, filter endpoint, or depth facts are absent or redacted, helper generation must stay suppressed.
- This story assumes bridge-derived generated member names can fit the existing helper naming pattern without new collisions beyond the existing `DMV1965` diagnostic path.

AC / test suggestions
- Add approval coverage proving many-to-many emits both `Read{ProducedName}FromAsync` and `Read{ProducedName}ToAsync`, while hierarchy emits only bounded `Read{ProducedName}AncestorAsync` and `Read{ProducedName}DescendantAsync` overloads.
- Add runtime-equivalence coverage for hierarchy `maximumDepth=0`, `maximumDepth=1`, and omitted-depth rejection to lock the current `DataVaultBridgeReadRequest` boundary.
- Add mixed-bundle coverage where a valid satellite helper still generates when one bridge entity surfaces `DMV1964`, `DMV1967`, or `DMV1969`.

Implementation watchouts
- Current generator source in `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs` is satellite-only and bridge-diagnostic-first; the dev change should stay narrowly scoped to supported bridge shapes and must not broaden into raw `dvault.model.v1` parsing, source-visible inference, or provider-specific behavior.
- Generated bridge helpers should delegate through the existing bridge read surface anchored by `src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs` and `src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs`, not introduce a new runtime read primitive.
- Hierarchy helpers must preserve the existing inclusive bounded-depth semantics exactly; emitting an unbounded overload or silently widening traversal behavior would contradict the current runtime contract already enforced by `DataVaultBridgeReadRequest`.

Non-blocking notes
- .gicket/tickets/06F7Y0HJ1ZPY7ND9N8RVS92H4C/comments/06F88K2YGDVDQVH9RASW2R57G4.md records the refinement contract coverage as 6 acceptance-criteria items, 4 Definition of Done items, and 4 implementation notes.

Split recommendations
- No split recommended; the ticket is already bridge-only, the upstream contract story `06F7Y0GT7A5QT77TADMRZBVYN8` is done, and downstream documentation is already separated into `06F7Y0HZKHBHMYX9EYDYFRYXZ0`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment