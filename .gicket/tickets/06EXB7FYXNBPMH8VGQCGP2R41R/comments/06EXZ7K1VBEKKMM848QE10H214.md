[gicket-bot] PO-critic review contract

Summary
- Ticket contract is now source-backed and ready for developer handoff; repository evidence matches the stated scope and the persisted contract has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes only `ModelBuilder UseDataVault(this ModelBuilder modelBuilder)`, and that method only sets the `DCoding.Data.DVault:Conventions` annotation before returning the builder; no explicit EF metadata translation entry point is currently present in the visible EF surface.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` directly defines the provider-neutral input contracts named in the contract: `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference`, plus helper metadata types for business keys, link participants, and satellite payload columns.
- `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` fixes the deterministic v1 naming/composition baseline cited by the contract, including `HubCustomer`, `SatCustomerContact`, `LinkCustomerOrder`, hub columns `[CustomerHashKey, LoadTimestamp, RecordSource, CustomerId]`, satellite columns `[CustomerHashKey, HashDiff, LoadTimestamp, RecordSource, EmailAddress]`, and link columns `[CustomerOrderHashKey, LoadTimestamp, RecordSource, CustomerHashKey, OrderHashKey]` with the expected PK/index names.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs` directly verifies the metadata contract shapes and required technical roles for hubs, links, and satellites, including that links require at least two hub endpoints and satellites can target either a hub or link parent.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not name a concrete repeated-run EF model inspection example for each of hub, link, and satellite translation paths; developers should still cover repeated equivalent input for all three kinds because determinism is an acceptance criterion.
- The contract leaves hub multi-column business-key ordering to tests rather than a spelled-out example; this is acceptable but worth exercising in implementation tests because `DataVaultHubMetadata` allows multiple business keys.

Risky assumptions
- The contract assumes developers will derive EF entity/property/key/index shape from the existing provider-neutral modeling and naming baseline instead of inventing parallel naming logic; that risk is acknowledged in the persisted `## Risks` section.
- The contract permits creation of a minimal aggregate input contract or DVault-owned annotations if needed, so scope control depends on keeping any new public EF-facing surface narrowly bounded to translation support only.

AC / test suggestions
- Inspect the EF model directly for each translated hub, link, and satellite and assert entity-kind and column-role markers without using relational provider APIs.
- Add determinism coverage that runs equivalent translation input more than once and compares entity, property, key, and index shape in declared order.
- Add an explicit opt-in test proving a bare `UseDataVault()` call still creates no entity types while the new translation path does.

Implementation watchouts
- Do not add foreign keys, navigations, or provider-specific relational annotations; the contract explicitly limits translation to provider-neutral EF metadata with primary keys and secondary indexes only.
- Any new public API surface or annotation identifiers must carry XML documentation because `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` enables `GenerateDocumentationFile` and treats `CS1591` as an error.
- Reuse the existing naming/modeling baseline under `src/DCoding.Data.DVault/Modeling` so generated EF metadata stays aligned with the deterministic outputs already asserted in `NamingPolicyTests.cs`.

Non-blocking notes
- Recent ticket comments show the automation flow already returned this ticket from an earlier PO-critic block to PO refinement and back again; the current persisted contract reflects that refinement and removes the earlier unsupported API assumption.
- The ticket remains operationally blocked by upstream ticket `06EXB7FPZRCFC33RF2M5SXZTK4` per the persisted relation evidence, but that is not a PO-refinement defect in this ticket contract itself.

Split recommendations
- No additional split recommended; the persisted contract already isolates this provider-neutral EF metadata work from downstream provider-specific tickets `06EXB7GESWZZTZG7XYAKTTKQRW` and `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment