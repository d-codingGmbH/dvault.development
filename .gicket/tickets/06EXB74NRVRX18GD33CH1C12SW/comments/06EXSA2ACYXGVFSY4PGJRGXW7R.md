[gicket-bot] PO-critic review contract

Summary
- Persisted contract is ready for developer handoff; no unresolved Open Questions, and repository evidence confirms the relevant modeling namespace, technical metadata baseline, and test surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted Delivery Contract has PO Handoff decision ready_for_po_critic and ## Open Questions contains '- none'.
- repository-list-directory src/DCoding.Data.DVault/Modeling listed DataVaultMetadata.cs, DataVaultModel.cs, DataVaultModelBuilder.cs, DataVaultModelConcept.cs, DefaultDataVaultNamingPolicy.cs, DefaultNamingPolicy.cs, and IDataVaultNamingPolicy.cs.
- git rev-parse HEAD returned 5941e2a0b3a79c8409bedbdf379170933d0f9b13 on branch ticket/06EXB74NRVRX18GD33CH1C12SW-story-model-data-vault-building-blocks; git log shows c295c732 handoff po->po-critic and a429328 handoff dev->po in branch history.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj directly shows TargetFramework net10.0, GenerateDocumentationFile true, and WarningsAsErrors including CS1591.
- src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs directly defines the closed role enum values HashKey, HashDiff, LoadTimestamp, and RecordSource with XML summaries.
- src/DCoding.Data.DVault/Modeling/DataVaultModelConcept.cs directly defines Hub, Link, Satellite, HashKey, HashDiff, LoadTimestamp, and RecordSource concepts.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs directly defines public DataVaultHubMetadata, DataVaultLinkMetadata, DataVaultSatelliteMetadata, business-key, participant, payload, and reference metadata classes; constructors wire required technical metadata through TechnicalMetadataColumnContract.ForRole.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs covers hub business keys, link endpoints, satellite hub/link parents, provider-neutral CLR contracts, validation failures, and required technical roles.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- None identified as a PO blocker; dev should preserve coverage for link with exactly two participants, satellite with link parent, null/empty metadata collections, and provider-token absence.

Risky assumptions
- Hash key and hash diff metadata could be mistaken for hash computation; the ticket contract keeps hash algorithms and normalization out of scope.
- The parent story spans hub, link, and satellite concepts, so implementation needs scope discipline to avoid drifting into provider persistence or schema generation.

AC / test suggestions
- Keep acceptance tied to documented public/protected APIs that compile under net10.0 with CS1591 enforced.
- Unit tests should assert hub business-key metadata, link participant metadata, satellite payload metadata, required technical roles, naming/default behavior, and provider-neutral behavior without a database provider.

Implementation watchouts
- Use src/DCoding.Data.DVault/Modeling and namespace DCoding.Data.DVault.Modeling unless nearby source establishes a narrower placement.
- Reuse TechnicalMetadataColumnRole and TechnicalMetadataColumnContract instead of creating competing technical role concepts.
- Do not introduce Sqlite/Postgres/EF provider APIs, schema generation, migrations, generated columns, sequences, triggers, hash computation, or advanced public options beyond a minimal provider-neutral shape.
- Run bash tools/check-format.sh and dotnet test/build; if sandbox MSBuild IPC limitations recur, record the exact command and diagnostic separately from source correctness.

Non-blocking notes
- Previous dev comments report sandbox-specific dotnet/MSBuild IPC issues; the PO contract already clarifies they do not alter scope.
- git status --short showed dirty bot metadata under .gicket/.gicket-bot paths, not product source, tests, or docs paths; this does not block PO handoff.

Split recommendations
- No PO split is required before dev; comment evidence records existing parentOf child tickets 06EXB74XQJFKGSKVJ6THQWJY8W and 06EXB755X9TGQW2EG1G30GJG28.
- If dev finds the parent too broad, split by hub/business-key metadata, link/participant metadata, and satellite/payload metadata while keeping the shared technical metadata role set common.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment