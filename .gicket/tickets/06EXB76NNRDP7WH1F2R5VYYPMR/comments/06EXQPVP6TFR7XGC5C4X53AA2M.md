[gicket-bot] PO-critic review contract

Summary
- PO refinement addressed the prior blocker by making this a bounded implementation-and-test handoff for the stable hash service and canonical normalizer. The persisted contract has no open questions and is ready for developer handoff, with a few non-blocking edge-case watchouts.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted .gicket/tickets/06EXB76NNRDP7WH1F2R5VYYPMR/description.md contains a Delivery Contract with PO Handoff decision ready_for_po_critic and ## Open Questions set to '- none'.
- PO refinement comment .gicket/tickets/06EXB76NNRDP7WH1F2R5VYYPMR/comments/06EXQNDAC1RCN33WYQBQTCJSMR.md explicitly answers prior critic items: the ticket is not test-only, authorizes bounded production implementation, and treats parent 06EXB765S2X2MR2K18ZBV8RC38 as broad follow-up context rather than a blocking dependency.
- docs/plans/stable-hashing-contract.md directly defines IStableHashService.AlgorithmId, ComputeHash(string normalizedInput), StableHashDigest.AlgorithmId/Value, sha256-v1, SHA-256 over UTF-8 without BOM, lowercase 64-character hex, null input ArgumentNullException, scalar encodings, structured ordinal field sorting, registration expectations, and published digest vectors.
- git grep for IStableHashService/StableHashDigest/ComputeHash/sha256-v1 found the service/digest names only in docs/plans/stable-hashing-contract.md, while source currently has only src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs exposing StableHashAlgorithmId = 'sha256-v1'; this supports the PO clarification that implementation creation is in scope.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs currently registers DefaultNamingPolicy and DataVaultConventions through AddDVault, giving a concrete existing convention-first startup path for the requested service registration behavior.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj has GenerateDocumentationFile=true and WarningsAsErrors includes CS1591, matching the contract's XML documentation DoD risk for new public APIs.
- find output shows tests/DCoding.Data.DVault.Tests/Unit exists with DCoding.Data.DVault.Tests.Unit.csproj and DVault.slnx includes the Unit, Integration, and Shared test projects, matching the requested test placement.
- git show --name-status b0d665b7 shows the PO handoff commit updated the ticket description and added PO refinement/handover comments; git show --name-status 7b337ca2 shows the current po-critic lease claim only changed .gicket ticket/comment/event files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Guid normalization is in Scope In but not called out in the acceptance criteria's explicit published-vector list; developer should still cover it because Scope In and the stable contract include lowercase d-format GUIDs.
- The contract requires invalid supported values to fail before hashing, but the current scoped scalar set has few obvious invalid states; non-finite floating point is mentioned in docs/plans/stable-hashing-contract.md even though float/double are not listed as stable scalar encodings.
- Timestamp tests should make the UTC expectation explicit enough to prevent local-time or current-culture formatting drift.

Risky assumptions
- The implementation may choose exact production type names other than IStableHashService and StableHashDigest; the ticket permits accepted equivalents, so reviewers must verify behavioral/member equivalence directly.
- The title still reads like a test task, but the persisted Delivery Contract is authoritative and explicitly makes production implementation in scope.
- Parent story 06EXB765S2X2MR2K18ZBV8RC38 remains broad and unrefined, so developers must not pull parent-story hash key/hash diff behavior into this child.

AC / test suggestions
- Assert every published vector from docs/plans/stable-hashing-contract.md with AlgorithmId sha256-v1 and lowercase 64-character digest shape.
- Add concrete canonical-text assertions before digest assertions for NFC normalization, LF line endings, null field inclusion, ordinal field sorting, invariant decimals/timestamps, and GUID formatting.
- Use at least two differently ordered structured inputs to prove source order and dictionary iteration order cannot affect canonical text or digest.
- Culture tests should restore CurrentCulture and CurrentUICulture in cleanup logic.

Implementation watchouts
- Register the default service through AddDVault or the established DI/options path while preserving caller overrides.
- Do not use serializer output as the canonical structured representation; deliberately map supported fields, sort by ordinal field path, join with LF, and omit trailing LF.
- Keep null service input, empty service input, null stable scalar n:, and empty string stable scalar s:0: distinct.
- Do not add byte-array, stream, base64, persistence schema, or full entity-specific hub/link/satellite hash behavior in this ticket.
- New public APIs need XML documentation because CS1591 is configured as an error in src/DCoding.Data.DVault/DCoding.Data.DVault.csproj.

Non-blocking notes
- The persisted Open Questions section contains '- none', so the open-question gate does not block approval.
- Existing source already reserves StableHashAlgorithmId = sha256-v1 in DataVaultConventions, but no production hash service API exists yet; that absence is now accounted for by the refined scope.
- Existing relations observed in .gicket events include parentOf from 06EXB765S2X2MR2K18ZBV8RC38 and a blocks path to 06EXB80FPE3REH11RQ1YR6BW1G; the PO contract explicitly keeps this child independently bounded.

Split recommendations
- No split is required before dev handoff for this bounded stable hash service plus canonical normalizer slice.
- Keep full Data Vault hash key/hash diff entity services, persistence integration, participating-field selection, and first-class binary scalar normalization as follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment