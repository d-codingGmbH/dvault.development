<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to remove the unsupported code-first-to-registry expectation, name the authoritative PostgreSQL setup path, and lock the missing-configuration behavior to an explicit DVAULT_TEST_POSTGRES_CONNECTION_STRING skip contract; no child tickets, relation edits, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative metadata pattern for both examples is one shared DataVaultMetadataModel registered once through the public AddDVault metadata options surface and consumed in each example DbContext through UseDataVaultMetadata().
- This ticket no longer requires a public code-first-builder-to-registry conversion path and does not authorize internal API usage or duplicated metadata declarations just to simulate that bridge.
- The authoritative PostgreSQL provider setup for the example is AddDVaultPostgres() together with the same registry-backed UseDataVaultMetadata() path used by the SQLite example.
- The PostgreSQL example uses only DVAULT_TEST_POSTGRES_CONNECTION_STRING for connection input. If the variable is absent, the example must print 'Skipping PostgreSQL quickstart. Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to a developer-managed PostgreSQL connection string and rerun this example.' and exit successfully before attempting a connection.
- Example-local run docs are in scope here. The broader README and release-document alignment remains downstream on blocked ticket 06F0MEDJC732GDD77H60R259P0, and the existing blocks relation remains unchanged.
- No child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- One runnable SQLite quickstart example that uses the current public registry-backed metadata path, explicit IDataVaultSaveService writes, and typed latest/as-of reads.
- One runnable PostgreSQL quickstart example that uses the same shared domain story and the explicit AddDVaultPostgres() provider setup with registry-backed UseDataVaultMetadata().
- Example-local docs that give exact build and run commands, the DVAULT_TEST_POSTGRES_CONNECTION_STRING prerequisite, and the missing-configuration skip behavior.
- A minimal history flow that creates schema, writes at least two timestamped versions, and shows a visibly distinct latest read and as-of read.

### Scope Out
- Any public API expansion to expose a code-first-builder-to-registry bridge.
- Broader root README, release-note, or architecture-document cleanup outside the example-local instructions already downstream on ticket 06F0MEDJC732GDD77H60R259P0.
- Provisioning PostgreSQL or any other external database infrastructure for developers.
- Additional provider quickstarts, performance tuning, migration guidance, or advanced provider behavior.

## Acceptance Criteria
- The repository contains a SQLite quickstart example that builds from DVault.slnx or documented dotnet run --project commands and completes end to end with no external infrastructure.
- Both examples use one authoritative public metadata source: a shared DataVaultMetadataModel registered through the public AddDVault metadata options surface and consumed by each example DbContext through UseDataVaultMetadata(); the examples do not rely on internal APIs or invent a code-first-to-registry bridge.
- The PostgreSQL example and its docs explicitly name the intended provider path as AddDVaultPostgres() plus the same registry-backed UseDataVaultMetadata() flow used by the SQLite example.
- The PostgreSQL example reads connection input only from DVAULT_TEST_POSTGRES_CONNECTION_STRING; when the variable is absent it prints the exact configured skip message and exits successfully without attempting a database connection.
- Both examples share one minimal bounded domain story, create the schema, write enough history to distinguish latest from as-of behavior, and display typed read results clearly enough for a developer to verify the time-sliced semantics.
- No committed example file contains credentials, absolute machine paths, or repository-external assumptions.

## Definition of Done
- Example source, project wiring, and example-local usage docs are committed and discoverable from the repository.
- Both examples compile against the current public branch surface without internal APIs, speculative APIs, or duplicated metadata declarations to fake a missing public bridge.
- The SQLite path is the default local proof and runs end to end without external services.
- The PostgreSQL path uses AddDVaultPostgres(), the shared registry-backed metadata path, and the explicit DVAULT_TEST_POSTGRES_CONNECTION_STRING skip contract.
- The examples exercise typed latest and as-of reads on persisted data that proves time-sliced behavior, and any broader README or release narrative changes remain on ticket 06F0MEDJC732GDD77H60R259P0.

## Implementation Notes
- Use one very small shared model, with a single parent entity and one descriptive satellite with two historical versions, so the metadata, save, and read flow stays identical across SQLite and PostgreSQL.
- Keep provider-specific code thin. SQLite and PostgreSQL should share the same metadata, save workflow, and read workflow, with only the EF provider wiring and connection bootstrap differing.
- For the authoritative metadata surface, register the shared DataVaultMetadataModel once through the public AddDVault metadata options surface already shown in README.md, then opt each DbContext into that shared registry through UseDataVaultMetadata().
- For PostgreSQL provider services, use AddDVaultPostgres() and treat src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs as the example authority when repo prose disagrees.
- Guard the PostgreSQL entry point before connection use: if DVAULT_TEST_POSTGRES_CONNECTION_STRING is missing or empty, print the exact skip message and return success without opening a provider connection.
- Keep docs narrowly focused on running these examples. Do not expand this ticket into the broader README or release-document rewrite, and do not materialize child tickets, attachments, or planning documents from this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- After these examples land, should ticket 06F0MEDJC732GDD77H60R259P0 replace the root README quickstart with direct links to the runnable examples and align the stale PostgreSQL provider-registration prose?
- Should a later ticket add a first-class public bridge from code-first declarations to DataVaultMetadataModel or DataVaultMetadataRegistry so future examples can avoid choosing the metadata-model path explicitly?
- Once PostgreSQL example guidance is stable, should CI or an optional documented local harness be added later to exercise the provider-backed quickstart automatically?

## Risks
- Until ticket 06F0MEDJC732GDD77H60R259P0 lands, broader repository prose may still disagree with the example-local PostgreSQL setup path even though this ticket now names the example authority explicitly.
- If implementation drifts back to code-first builder callbacks or internal APIs instead of the documented public metadata-model path, the examples will reintroduce the PO-critic blocker.
- If the examples save only one version of the satellite data or do not print distinct latest and as-of results, the quickstarts will fail to prove the intended historical read semantics.

## Split Recommendations
- No split recommended. The remaining PO work is fully bounded by clarifying one authoritative public metadata path and one explicit PostgreSQL configuration contract, so no child tickets or planning documents were created.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Create small runnable examples that demonstrate the v0.6 Code-First, registry, typed save, and typed read flow.

## Scope In

- SQLite example that runs without external infrastructure.
- PostgreSQL example with environment-variable configuration and clear skip/setup guidance.
- Minimal domain model, schema creation, save, latest read, and as-of read.

## Scope Out

- Provisioning every provider in examples.
- Embedding credentials or machine-specific paths.

## Acceptance Criteria

- Examples are buildable from the solution or documented command lines.
- No credentials or machine-specific paths are committed.
- Examples avoid future APIs and reflect the implemented v0.6 surface.