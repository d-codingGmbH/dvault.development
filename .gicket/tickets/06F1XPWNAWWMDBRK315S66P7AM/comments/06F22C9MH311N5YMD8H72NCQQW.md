[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the persisted contract has no open questions, the requested adapter scope is bounded to existing DVault EF metadata and dvault.model.v1, and direct source evidence confirms the required annotation/design-time/model-first baselines exist. Developer should still treat the stale incoming block relation files and missing unsupported-gap precedent as watchouts.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPWNAWWMDBRK315S66P7AM/description.md:47-48 shows ## Open Questions with '- none'.
- git rev-parse HEAD returned fd3c21d2b08d8790c633ea4ab45affd767c5aaf0, matching the scratch source ref; git log shows fd3c21d2 as the po-critic lease claim and 936fb04c as the PO handoff commit.
- docs/model-first-governance.md:17-19 fixes exact schemaVersion dvault.model.v1 and stable categories hubs, links, satellites, pits, and bridges; docs/model-first-governance.md:136-157 documents DataVaultModelDriftReporter.Compare as design-time EF metadata comparison with no live database inspection.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md:5-13 fixes the consumer-owned IDesignTimeDbContextFactory/preflight boundary and explicitly says DVault does not provide EF design services, a custom dotnet ef shim, CLI interception, or Microsoft.EntityFrameworkCore.Design in the package.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:15-80 defines ProducedName, EntityKind, MetadataName, ParentReferenceKind, ParentReferenceName, Ordinal, PropertyRole, ProviderLogicalPropertyKind, MetadataSourceKind, and MetadataSourceFingerprint; line 115 defines BridgeDepth.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:313,370,442 project Bridge and Pit table kinds; lines 332-337 project hierarchy TraversalDepth with BridgeDepth; lines 572,605,729,731 set parent reference, index ordinal, property ordinal, and provider logical property annotations.
- src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:20,33,52,76,93 exposes Compare overloads; lines 639-649 create an internal snapshot from IReadOnlyModel entity metadata; lines 742-743 read SqlServer/Npgsql include annotations, so provider-specific metadata handling needs careful classification.
- rg for unsupported/gap terms in src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelDriftReporterTests.cs returned no matches, so unsupported-gap behavior appears to be new work rather than existing precedent.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add or require test examples where dvault.model.v1 asks for metadata not recoverable from the current EF projection surface and the adapter emits an explicit unsupported-gap result.
- Include at least one test that starts from a ModelSnapshot-derived IReadOnlyModel path, not only an already-built DbContext/current ModelBuilder path.
- Keep representative PIT and hierarchy bridge examples with TraversalDepth/BridgeDepth coverage in the acceptance tests.

Risky assumptions
- The contract assumes the current annotation surface is sufficient for the comparison shape; source confirms the named annotations exist, but unsupported-gap behavior still has to define what happens when a dvault.model.v1 field cannot be recovered.
- Existing DataVaultModelDriftReporter already has snapshot comparison concepts; developers may duplicate public surface unless the implementation intentionally extends or composes the existing drift reporter.
- Incoming block relation files remain present even though both source tickets are done; automation may still need relation cleanup outside this PO-critic decision.

AC / test suggestions
- Assert deterministic ordering by produced name and ordinal for entities, properties, primary keys, indexes, and supported constraints.
- Assert exact match and bounded drift for hubs, links, satellites, PITs, many-to-many bridges, and hierarchy bridges with TraversalDepth.
- Assert provider-specific/out-of-scope EF metadata is either ignored by contract or surfaced as unsupported, without creating false matches.
- Assert no live database, migration generation, migration operation diffing, or EF CLI shim behavior is introduced.

Implementation watchouts
- Prefer extending the existing DataVaultModelDriftReporter/CreateSnapshot(IReadOnlyModel) path over adding a parallel drift contract.
- Use DataVaultAnnotationNames and DataVaultPropertyRole/DataVaultTableKind values as the authority for extraction; do not infer defaults for missing contract fields.
- Be explicit about how EF Core ModelSnapshot is converted to design-time IReadOnlyModel metadata while staying outside live database and migration-generation flows.
- Treat provider index include annotations already read by the current reporter as provider-specific surface that must not mask unsupported dvault.model.v1 gaps.

Non-blocking notes
- The persisted contract is ready enough for dev: no open questions, bounded scope, clear scope-out, concrete DoD, and direct repository evidence for the required upstream surfaces.
- The two incoming blockers are still represented as relation files, but both source tickets are currently done, so this is a sequencing/automation cleanup note rather than a PO refinement blocker.

Split recommendations
- No split recommended; adapter extraction, comparison behavior, and focused tests remain one bounded drift task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment