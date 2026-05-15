[gicket-bot] PO refinement contract

Summary
- Kept ticket `06F2PGG8ZKSYGC8863118H56G8` as the implementation slice for PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers, corrected the handoff back to PO refinement, and anchored the contract to branch `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` at `f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed`, which still differs from `develop` only in ticket metadata. No child tickets, relation writes, attachments, or planning documents were materialized in this pass.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This pass ratifies the PO-critic return to Product Owner refinement: the ticket stays at `needs_po_clarification`, not `ready_for_po_critic`, because the current branch/ref still lacks non-ticket `src/` and `tests/` implementation evidence.
- critic-item-2: `answered` - The exact current implementation evidence branch/ref is `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` at `f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed`; compared with `develop`, that ref still has no non-ticket `src/` or `tests/` changes, so it cannot be used for a PO-critic rerun. A later rerun must cite a newer ref that includes matching code and tests.
- critic-item-3: `answered` - Current repository docs remain SQLite-first today, but product intent is not to keep this work SQLite-only: the parent story explicitly scopes first-class PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers. The contract therefore keeps this ticket as the implementation slice for that expansion instead of narrowing or splitting it in this pass.
- critic-item-4: `answered` - Definition of Done items 1-2 remain unmet at `f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed`: compared with `develop`, the current branch shows no non-ticket `src/` or `tests/` changes for provider catalog readers.
- critic-item-5: `answered` - Acceptance criteria 1, 2, and 5 remain unmet at `f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed`: `DataVaultLiveSchemaReader.ReadAsync(...)` still dispatches only SQLite and returns `UnsupportedProvider` for non-SQLite providers, even though built-in provider-name recognition already exists elsewhere.
- critic-item-6: `answered` - Acceptance criterion 3 remains unmet at `f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed`: direct `DataVaultLiveSchemaReader.ReadAsync(...)` execution is evidenced only in SQLite tests, while the non-SQLite artifacts currently stop at shared fixture/config scaffolding.

Clarifications
- Current bounded review evidence is branch `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` at `f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed` against `develop` at `5c8fd578aed9f3316cc5ce5fe5b949f861b5b25b`.
- `git diff --name-status develop...f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed -- src tests` is empty; compared with `develop`, the current ref still changes ticket metadata only.
- `DataVaultLiveSchemaReader.ReadAsync(...)` remains SQLite-only at `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:13-34`; recognized non-SQLite provider names are still routed to `UnsupportedProvider` despite the existing provider-name baseline in `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:11-18`.
- Current repository docs remain SQLite-first today in `README.md:457-493` and `docs/production-adoption-checklist.md:29`; this ticket remains the implementation step intended to change that baseline under Story `06F2PGFZWC5PXSDH46RCZPN1CG`.
- Existing non-SQLite artifacts are scaffolding only: shared fixtures, fixture-contract assertions, and conditional provider package references exist, but direct non-SQLite `ReadAsync(...)` execution is not yet evidenced.
- Persisted relations were left unchanged in this pass: Story `06F2PGFZWC5PXSDH46RCZPN1CG` remains the parent, and an incoming `blocks` relation from done Task `06F2PGG57K3S7CJQP5QX9AWW3G` is still present in live relation state.

Scope In
- Implement built-in live-schema reader dispatch and catalog readers for PostgreSQL, SQL Server, Oracle, and MySQL in `DataVaultLiveSchemaReader.ReadAsync(...)`.
- Add direct external opt-in integration coverage that calls `DataVaultLiveSchemaReader.ReadAsync(...)` for PostgreSQL, SQL Server, Oracle, and MySQL using `ExternalProviderLiveSchemaFixture`, existing provider traits, and the documented `DVAULT_TEST_*_CONNECTION_STRING` boundary.
- Preserve the existing `DataVaultLiveSchemaReadResult` statuses, snapshot object model, and deterministic ordering used by drift reporting.
- Reuse the shared fixture baseline from `LiveSchemaReaderContractFixture` and `ExternalProviderLiveSchemaModelOptions.ExpectedSnapshot` so provider implementations do not invent divergent contract shapes.

Scope Out
- Treating current ref `f1a268214c7dcd8f37b0e462bfcb77798d8ed7ed` as delivered or sending it back to PO-critic without matching non-ticket `src/` and `tests/` evidence.
- Narrowing this ticket back to SQLite-only in this pass; the current repository baseline is SQLite-first today, but the parent story still scopes first-class non-SQLite readers.
- Adding new public live-schema result types, new statuses, new snapshot fields, or broader comparison coverage for foreign keys, views, sequences, triggers, or arbitrary non-DVault objects.
- Claiming fixture-only contract tests or conditional provider package references as proof that provider readers are implemented.

Open questions
- none

Follow-up questions
- Once implementation exists, which exact branch, ref, or commit and corresponding non-ticket `src/` plus `tests/` evidence will be cited for the next PO-critic rerun?
- After first-class readers land, should ticket `06F2PGHA0EXJRGDHM4GQM7NPYR` add SQL Server, Oracle, and MySQL fixture walkthroughs comparable to the existing PostgreSQL guidance?
- Do later CI or tooling tickets want opt-in smoke commands for the new provider readers, or should live-provider verification remain developer-managed and off by default?

Risks
- Provider catalogs differ in schema scoping, identifier casing, index metadata shape, and storage-type text, so normalization mistakes can create false drift or hide real mismatches.
- Oracle physical-name limits and dual MySQL provider-name handling make identifier mapping and dispatch logic more brittle than the current SQLite path.
- External opt-in coverage depends on configured provider packages and developer-managed databases, so some defects may remain latent until those lanes are exercised.
- If the ticket is resent to PO-critic without matching non-ticket `src/` and `tests/` evidence, it will fail on the same repository-gap findings.

Split recommendations
- No split was materialized in this pass; keep this as one bounded implementation ticket under Story `06F2PGFZWC5PXSDH46RCZPN1CG` while product intent remains first-class PostgreSQL, SQL Server, Oracle, and MySQL readers.
- Split by provider only if later development proves catalog quirks or external setup make review and verification unmanageably large; do not use a SQLite-only split unless product explicitly changes story scope.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment