[gicket-bot] PO-critic review contract

Summary
- Refined contract now closes the earlier PO-critic gaps and is specific enough for developer handoff on the existing explicit save-service boundary.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/description.md:11-18 and 36-63 now fix SatelliteOperations, explicit ParentHashKey, caller-supplied HashDiff, satellite SavedRecords behavior, and record `## Open Questions` as `none`.
- .gicket/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/comments/06EY2P7CHAGEVKMSRA90HPNDAW.md:6-16 records PO handoff `ready_for_po_critic` and marks critic-item-1 through critic-item-6 as answered.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:10-68 shows the existing public boundary is the explicit IDataVaultSaveService/DataVaultSaveRequest surface with request-level LoadTimestamp and RecordSource plus HubOperations and LinkOperations, which matches the ticket's additive extension point.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:166-230 shows DataVaultSaveResult/DataVaultSavedRecord already provide the result surface the ticket is extending, and src/DCoding.Data.DVault/Modeling/DataVaultModel.cs:447-461 already defines DataVaultTableKind.Satellite.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:273-336 and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:164-227 show public satellite metadata already exists and translated satellite tables use parent hash key, HashDiff, LoadTimestamp, and RecordSource with primary key (parentHashKey, LoadTimestamp).
- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs:54-71 and 170-190 verify both hub-parent and link-parent satellite schema shapes already exist in the SQLite baseline.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11-166 proves the current explicit save-service baseline and hub/link idempotent reuse behavior that the ticket says must not regress.
- docs/plans/stable-hashing-contract.md:22-23 and 47-74 plus docs/architecture/mvp-data-vault-concepts.md:58-63 support the refined contract decision that callers own HashDiff construction and SQLite-oriented tests may use explicit text HashDiff values.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No explicit acceptance example covers a changed satellite save that reuses the same ParentHashKey and the same request LoadTimestamp as an existing row, even though the source-backed satellite primary key is (parentHashKey, LoadTimestamp) in src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:212-218 and tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs:54-71.
- The acceptance text requires ParentHashKey to work for hub or link parents, but the save-service-specific examples still focus on hub/link baseline rather than an explicit link-parent satellite save path.

Risky assumptions
- Caller/domain code will supply stable HashDiff values consistently across producers; the ticket acknowledges this risk and docs/plans/stable-hashing-contract.md:47-74 keeps payload field selection outside the shared hash service.
- Caller-provided LoadTimestamp values are usable for historization ordering per parent, because the existing satellite schema keys history by parent hash key plus load timestamp.

AC / test suggestions
- Add one SQLite save-service test for the first satellite insert when no prior row exists.
- Add one explicit save-service test for a link-parent satellite in addition to a hub-parent satellite, since repository metadata and schema already support both parent kinds.
- Add an A->B->A timeline test with increasing timestamps to lock the 'compare only to the current latest row' rule from .gicket/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/description.md:15-16 and 39-41.

Implementation watchouts
- The current implementation is still hub/link only in src/DCoding.Data.DVault/DataVaultSaveService.cs:247-275, so the work must remain an additive extension of the explicit save-service boundary and not introduce an implicit SaveChanges path.
- Satellite translation uses parent hash key, HashDiff, LoadTimestamp, and RecordSource with only a non-unique parent index and PK (parentHashKey, LoadTimestamp) in src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:164-227, so latest-row lookup semantics matter.
- The current docs baseline in docs/architecture/dvault-v1-explicit-save-service.md:8-27 keeps provider-specific conflict handling and upserts out of scope, so the ticket should stay on deterministic pre-insert lookup behavior.

Non-blocking notes
- The earlier blocking PO-critic assessment is preserved in .gicket/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/comments/06EY2MKDGPJMCKA0NCGCBYJ1KM.md:20-58, and the newer refinement comment directly resolves those same blocker themes.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment