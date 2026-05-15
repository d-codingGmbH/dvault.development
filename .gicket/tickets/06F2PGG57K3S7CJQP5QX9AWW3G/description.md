<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the existing live-schema contract already present on the branch and scopes this ticket to shared provider-reader fixtures and contract coverage; verified live state remains child of Story 06F2PGFZWC5PXSDH46RCZPN1CG and blocker of Task 06F2PGG8ZKSYGC8863118H56G8. No child tickets, relation changes, or planning documents were materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Recent ticket comments are automation claim and lease comments only; there is no human clarification to reconcile.
- Repository evidence already fixes the shared v1 contract baseline through IDataVaultLiveSchemaReader, DataVaultLiveSchemaReadResult, DataVaultLiveSchemaReadStatus with Succeeded, UnsupportedProvider, and Unavailable, and the snapshot/table/column/primary-key/index object graph.
- Current docs and code already ratify the bounded live-schema surface as DVault-owned tables, ordered columns with provider storage types, named primary keys, and secondary indexes only.
- Current branch state remains SQLite-first: SQLite has the built-in reader today, while other providers are still external opt-in or UnsupportedProvider until downstream reader work lands.
- Live relations verified from the local ticket store: Story 06F2PGFZWC5PXSDH46RCZPN1CG parentOf this ticket, and this ticket blocks Task 06F2PGG8ZKSYGC8863118H56G8.

### Scope In
- Ratify the existing live-schema reader public contract and keep downstream provider work compatible with DataVaultLiveSchemaDriftReporter.
- Add shared test fixtures for one reusable logical metadata scenario and expected live-schema assertions across tables, columns, provider storage types, primary keys, and secondary indexes.
- Add reusable external opt-in integration fixture helpers for Postgres, SQL Server, Oracle, and MySQL that build on the existing environment-variable configuration and conditional provider restore pattern.
- Add contract coverage for deterministic ordering and classified success, unavailable, and unsupported outcomes that downstream provider readers must satisfy.

### Scope Out
- Implementing the actual PostgreSQL, SQL Server, Oracle, and MySQL catalog-query readers and dispatch wiring in DataVaultLiveSchemaReader; that belongs to Task 06F2PGG8ZKSYGC8863118H56G8.
- Expanding the live-schema surface to foreign keys, views, arbitrary non-DVault objects, destructive repair logic, or automatic migration behavior.
- Reworking SQLite-first documentation, CI examples, or release notes beyond minimal contract-coherence changes; broader rollout already sits under later documentation and CI tickets.
- Changing the public status taxonomy or drift-report semantics without a separate public API justification.

## Acceptance Criteria
- The ticket makes the shared v1 reader contract explicit without reopening API shape: provider readers must return DataVaultLiveSchemaReadResult with Succeeded, UnsupportedProvider, or Unavailable status and, on success, a snapshot composed of DataVaultLiveSchemaTable, DataVaultLiveSchemaColumn, DataVaultLiveSchemaPrimaryKey, and DataVaultLiveSchemaIndex.
- Shared fixtures define a canonical DVault metadata scenario plus expected live-schema assertions for DVault-owned tables, ordered columns, provider storage types, named primary-key constraints, and secondary indexes so provider-reader tests do not invent provider-specific contract variants.
- Reusable external-provider fixture helpers exist for Postgres, SQL Server, Oracle, and MySQL, reuse the existing DVAULT_TEST_*_CONNECTION_STRING opt-in boundary and provider reflection or configuration helpers, and create or drop isolated database objects without leaving durable residue.
- Contract coverage proves deterministic ordering and classified failure handling, and continues to keep DataVaultLiveSchemaDriftReporter comparison behavior stable for downstream provider readers.

## Definition of Done
- Shared contract or fixture code is covered by unit or integration tests, and existing SQLite live-schema drift tests still pass against the ratified contract.
- Any new provider fixture or configuration test classes keep the established Category and Provider traits and update default-smoke discovery coverage when needed.
- The resulting contract is precise enough that Task 06F2PGG8ZKSYGC8863118H56G8 can implement provider readers without redefining statuses, snapshot shape, or test-harness boundaries.
- README or release-note updates are only required if this ticket changes the public contract text; otherwise the separate v0.11.0 documentation ticket carries public rollout.

## Implementation Notes
- Start from the existing SqliteLiveSchemaDriftTests contract and the shared customer/order/contact/state metadata scenario rather than inventing a second logical schema contract.
- Reuse the existing PostgresIntegrationTestConfiguration, SqlServerIntegrationTestConfiguration, OracleIntegrationTestConfiguration, MySqlIntegrationTestConfiguration, and provider reflection helpers instead of introducing new environment variables or restore switches.
- Use the explicit IDataVaultLiveSchemaReader overloads already present on DataVaultLiveSchemaDriftReporter when a test should exercise contract behavior without relying on the built-in static dispatch path.
- Keep shared fixture helpers test-only and bounded to the current live-schema surface; do not broaden package boundaries or move provider reader implementation work into this ticket.
- Allow provider-specific physical-name adjustments only when required by provider constraints such as Oracle identifier limits, while keeping the logical contract and asserted drift surface consistent.

## Open Questions
- none

## Follow-Up Questions
- After the shared contract lands, should Task 06F2PGG8ZKSYGC8863118H56G8 remain a single implementation ticket or split by provider if catalog quirks make review or test setup too large?
- Once first-class readers exist, should the documentation ticket add SQL Server, Oracle, and MySQL local fixture walkthroughs comparable to the existing Postgres guidance?
- Do we later want reusable local provisioning scripts or containers for non-Postgres live-schema validation, or keep all external database provisioning caller-managed in v0.11.0?

## Risks
- Provider catalogs vary in schema scoping, quoting, case folding, index metadata shape, and storage-type text; a fixture layer that is too generic may miss real reader bugs or become brittle.
- Oracle identifier-length limits and provider-specific physical naming rules can force fixture-specific naming overrides even when the logical metadata model is shared.
- External opt-in coverage depends on configured provider packages and create or drop permissions in developer-managed databases, so some reader defects may remain latent until those lanes are exercised.

## Split Recommendations
- No child split is needed for this contract-and-fixture ticket. If downstream implementation grows, split Task 06F2PGG8ZKSYGC8863118H56G8 by provider after this shared contract lands.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Prepare the contract and shared fixtures needed before implementing provider-specific schema readers.

## Scope
- Refine and complete the work for "Define live schema reader contract and fixtures" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.