[gicket-bot] PO-critic review contract

Summary
- Persisted contract is bounded, source-backed, and has no unresolved Open Questions; ticket 06FE4R0TBG8JP5WA2SHXKH438M is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4R0TBG8JP5WA2SHXKH438M/description.md contains a delivery contract with PO handoff decision ready_for_po_critic and ## Open Questions set to - none.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs directly define and emit ProviderStorageType, ProviderValueFormat, HashKeyStorageProfile, StableHashAlgorithmId, StableHashDigestByteLength, StableHashDigestEncoding, and HashKeyConversionBehavior, matching the ticket's required manifest facts.
- src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs and src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs show an existing consumer-owned design-time preflight boundary, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs already verifies deterministic support-bundle export and a dry-run sql-artifact manifest pattern.
- docs/hash-key-storage-migration.md, docs/plans/hash-key-storage-profile-contract.md, and docs/architecture/dvault-dotnet-ef-design-time-workflow.md already document the caller-owned migration posture, finite v1 stable-hash baseline, required compatibility facts, and the consumer-owned preflight entrypoint referenced by the ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly say whether a selected model boundary with zero in-scope HashKey or ParticipantReference columns should emit an empty manifest or fail closed.
- The contract does not explicitly define the expected outcome for a no-op comparison where source and target storage profiles do not actually differ.
- The fail-closed posture is clear, but the contract does not spell out whether a partially flipped model boundary should be reported as supported scoped migration evidence or as drift.

Risky assumptions
- Developers will fit the artifact into the existing consumer-owned preflight surface without needing a PO-level decision on exact command naming or schema naming.
- Equivalent persisted-shape drift can be derived from the existing metadata and support-bundle vocabulary without reopening the product contract.

AC / test suggestions
- Add a golden-file style test that runs the dry-run twice against the same evidence and asserts byte-for-byte identical output.
- Add coverage that the manifest includes both HashKey and ParticipantReference entries across hubs, links, satellites, PITs, and bridges.
- Add fail-closed coverage for same-width algorithm drift such as sha1-v1 to sha256-160-v1, plus store-type, value-format, and conversion-behavior drift.
- Add an explicit side-effect guard that proves the dry-run path does not invoke migration application, DDL, or DML.

Implementation watchouts
- Keep the entrypoint consumer-owned as documented in docs/architecture/dvault-dotnet-ef-design-time-workflow.md; do not add a DVault-owned dotnet ef shim or IDesignTimeServices surface.
- Reuse the existing annotation vocabulary already emitted by DataVaultEfMetadataTranslator instead of inventing a second manifest terminology set.
- Preserve the public hash-key boundary as lowercase hexadecimal string values even when Binary storage is selected; tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs already assert string CLR type plus binary provider conversion.
- Make deterministic ordering explicit because CI review depends on stable table and property ordering.

Non-blocking notes
- Current ticket comments under .gicket/tickets/06FE4R0TBG8JP5WA2SHXKH438M/comments are bot claim, refinement, handoff, and run-report entries; no human clarification comment was observed.
- The upstream dependency is already resolved by done ticket 06FE4R0H98K42XJY1NEDQX8KB4, while downstream ticket 06FE4R2EGQ444EGPKZBRZCDEV8 is still todo and blocked by this ticket, which is consistent with approving this ticket for development now.

Split recommendations
- No split recommended; the persisted delivery contract already bounds this as one preflight-artifact task, and the parent story 06FE4R089MT3BYRCVH7Q4EX6CG is already done.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment