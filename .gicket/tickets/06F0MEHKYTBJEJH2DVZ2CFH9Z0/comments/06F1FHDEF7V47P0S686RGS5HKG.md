[gicket-bot] PO-critic review contract

Summary
- PO-critic review approves ticket 06F0MEHKYTBJEJH2DVZ2CFH9Z0 for developer handoff. The persisted contract has no open questions, the implementation scope is bounded to provider-neutral bridge reads, and repository evidence confirms the referenced bridge metadata/schema and read-service baseline exist.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEHKYTBJEJH2DVZ2CFH9Z0/description.md lines 17-23 scope provider-neutral bridge read helpers, many-to-many rows, bounded hierarchy rows, empty-result behavior, deterministic diagnostics, and focused unit/integration coverage.
- .gicket/tickets/06F0MEHKYTBJEJH2DVZ2CFH9Z0/description.md lines 25-30 explicitly exclude provider-specific SQL/tuning, PIT composition, row population, save-service changes, EF FKs/navigations, unbounded traversal, graph engine behavior, and Code-First expansion.
- .gicket/tickets/06F0MEHKYTBJEJH2DVZ2CFH9Z0/description.md lines 32-39 define acceptance criteria for deterministic many-to-many ordering, hierarchy TraversalDepth, empty/no-match results, diagnostics, provider-neutral EF querying, schema compatibility, new tests, and public API snapshot updates if needed.
- .gicket/tickets/06F0MEHKYTBJEJH2DVZ2CFH9Z0/description.md lines 56-57 show Open Questions: none, so approve_for_dev is not blocked by unresolved contract questions.
- .gicket/relations/JW/Z0/06F0MEGPPETJD4ZDEN5ESGR7JW--06F0MEHKYTBJEJH2DVZ2CFH9Z0--parentOf.json confirms parent story 06F0MEGPPETJD4ZDEN5ESGR7JW; .gicket/relations/Z0/VR and Z0/7W confirm this ticket blocks 06F0MEJ0NE80R7CNS982S3PKVR and 06F0MEJPGG7JBFEXD693BHY07W.
- .gicket/tickets/06F0MEHDFYCVK42FFY77FXHXBR/ticket.json shows the related bridge traversal query helper contract ticket is done; its description lines 18-23 and 46-52 define public request/response contract expectations while separating implementation.
- git rev-parse HEAD returned 6f3038e687f05020309c5441ef7518293de66591 on branch ticket/06F0MEHKYTBJEJH2DVZ2CFH9Z0-task-implement-provider-neutral-bridge-traversal; git log shows the latest commit is the po-critic lease claim and develop is an ancestor of HEAD.
- git diff --name-status develop...HEAD shows only .gicket ticket/comment/event metadata changed for this PO branch, with no source implementation changes mixed into the handoff.
- src/DCoding.Data.DVault/IDataVaultReadService.cs lines 8-19 defines the existing public IDataVaultReadService latest-satellite read API; src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs lines 48-53 shows the caller-owned projection style referenced by the contract.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs lines 47-65 and 121-155 directly expose DataVaultMetadataModel.Bridges; src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs lines 377-428 exposes ManyToMany and Hierarchy bridge metadata factory methods.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs lines 264-399 directly maps Bridge metadata to provider-neutral Bridge tables, endpoint hash-key columns, hierarchy TraversalDepth, DataVaultTableKind.Bridge, and bridge traversal indexes while rejecting unsupported projection features/kinds.
- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs lines 26-64 verify BridgeCustomerOrder and BridgeSalesRegionHierarchy table names, columns, primary keys, traversal indexes, and zero foreign keys; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs lines 138-166 verify the same relational metadata projection.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The implementation ticket delegates exact public type and method naming to the developer following existing read-service conventions; this is acceptable because the done contract ticket separates API contract design and the current ticket requires public API snapshots/XML docs if the surface changes.
- Hierarchy depth semantics remain a high-risk implementation detail: the contract requires unsupported or unbounded depth requests to fail rather than approximating partial graph answers.

AC / test suggestions
- Keep tests explicit for empty bridge tables, valid endpoint hashes with no matching rows, missing bridge metadata, missing generated table/property, unsupported bridge kind/features, malformed endpoint binding, and unsupported depth requests.
- Add public API snapshot assertions for any new bridge request/response/helper types or IDataVaultReadService extensions.

Implementation watchouts
- Use DataVaultMetadataModel.Bridges or registry-backed bridge lookup as authoritative; do not infer bridge semantics from EF foreign keys or navigations.
- Preserve provider-neutral EF shared-type dictionary querying with AsNoTracking and deterministic ordering; do not introduce provider-specific SQL or provider package behavior.
- Diagnose metadata/model mismatches as failures with bridge/table/property/endpoint detail, while treating valid no-row reads as empty results.
- Match existing BridgeCustomerOrder and BridgeSalesRegionHierarchy column order and TraversalDepth/BridgeDepth semantics exactly.

Non-blocking notes
- none

Split recommendations
- No split recommended; the contract design ticket is done, provider-specific optimization and documentation/benchmark work are already represented by separate downstream tickets, and this ticket is bounded to the provider-neutral implementation baseline.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment