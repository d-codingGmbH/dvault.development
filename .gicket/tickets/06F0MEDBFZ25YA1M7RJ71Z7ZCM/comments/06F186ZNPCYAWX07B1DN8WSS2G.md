[gicket-bot] PO refinement contract

Summary
- Refined the ticket to remove the unsupported code-first-to-registry expectation, name the authoritative PostgreSQL setup path, and lock the missing-configuration behavior to an explicit DVAULT_TEST_POSTGRES_CONNECTION_STRING skip contract; no child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Changed the contract from code-first plus registry-backed to one authoritative public-surface pattern the repository already exposes: define one shared DataVaultMetadataModel, register it once through the public AddDVault metadata options surface, and let each example DbContext consume it through UseDataVaultMetadata(). The quickstarts no longer require a public code-first-builder-to-registry bridge or duplicated declarations.
- critic-item-2: `answered` - The contract now says the PostgreSQL quickstart must wire provider services through AddDVaultPostgres() and use the same registry-backed UseDataVaultMetadata() context opt-in as SQLite; example-local docs must name that exact path instead of paraphrasing stale README or architecture prose.
- critic-item-3: `answered` - The example uses DVAULT_TEST_POSTGRES_CONNECTION_STRING. If the variable is absent, the PostgreSQL example prints 'Skipping PostgreSQL quickstart. Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to a developer-managed PostgreSQL connection string and rerun this example.' and exits successfully without attempting a connection.
- critic-item-4: `answered` - The blocking mismatch is removed by redefining the example around the public registry-backed metadata-model path that the repository already documents. Developers are no longer asked to synthesize a registry from code-first declarations or rely on internal APIs.
- critic-item-5: `answered` - The contract now treats current source as the example authority for PostgreSQL wiring: provider registration is AddDVaultPostgres(), metadata opt-in stays registry-backed through UseDataVaultMetadata(), and stale broader docs remain downstream documentation cleanup rather than an ambiguity left to implementers.

Clarifications
- The authoritative metadata pattern for both examples is one shared DataVaultMetadataModel registered once through the public AddDVault metadata options surface and consumed in each example DbContext through UseDataVaultMetadata().
- This ticket no longer requires a public code-first-builder-to-registry conversion path and does not authorize internal API usage or duplicated metadata declarations just to simulate that bridge.
- The authoritative PostgreSQL provider setup for the example is AddDVaultPostgres() together with the same registry-backed UseDataVaultMetadata() path used by the SQLite example.
- The PostgreSQL example uses only DVAULT_TEST_POSTGRES_CONNECTION_STRING for connection input. If the variable is absent, the example must print 'Skipping PostgreSQL quickstart. Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to a developer-managed PostgreSQL connection string and rerun this example.' and exit successfully before attempting a connection.
- Example-local run docs are in scope here. The broader README and release-document alignment remains downstream on blocked ticket 06F0MEDJC732GDD77H60R259P0, and the existing blocks relation remains unchanged.
- No child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

Scope In
- One runnable SQLite quickstart example that uses the current public registry-backed metadata path, explicit IDataVaultSaveService writes, and typed latest/as-of reads.
- One runnable PostgreSQL quickstart example that uses the same shared domain story and the explicit AddDVaultPostgres() provider setup with registry-backed UseDataVaultMetadata().
- Example-local docs that give exact build and run commands, the DVAULT_TEST_POSTGRES_CONNECTION_STRING prerequisite, and the missing-configuration skip behavior.
- A minimal history flow that creates schema, writes at least two timestamped versions, and shows a visibly distinct latest read and as-of read.

Scope Out
- Any public API expansion to expose a code-first-builder-to-registry bridge.
- Broader root README, release-note, or architecture-document cleanup outside the example-local instructions already downstream on ticket 06F0MEDJC732GDD77H60R259P0.
- Provisioning PostgreSQL or any other external database infrastructure for developers.
- Additional provider quickstarts, performance tuning, migration guidance, or advanced provider behavior.

Open questions
- none

Follow-up questions
- After these examples land, should ticket 06F0MEDJC732GDD77H60R259P0 replace the root README quickstart with direct links to the runnable examples and align the stale PostgreSQL provider-registration prose?
- Should a later ticket add a first-class public bridge from code-first declarations to DataVaultMetadataModel or DataVaultMetadataRegistry so future examples can avoid choosing the metadata-model path explicitly?
- Once PostgreSQL example guidance is stable, should CI or an optional documented local harness be added later to exercise the provider-backed quickstart automatically?

Risks
- Until ticket 06F0MEDJC732GDD77H60R259P0 lands, broader repository prose may still disagree with the example-local PostgreSQL setup path even though this ticket now names the example authority explicitly.
- If implementation drifts back to code-first builder callbacks or internal APIs instead of the documented public metadata-model path, the examples will reintroduce the PO-critic blocker.
- If the examples save only one version of the satellite data or do not print distinct latest and as-of results, the quickstarts will fail to prove the intended historical read semantics.

Split recommendations
- No split recommended. The remaining PO work is fully bounded by clarifying one authoritative public metadata path and one explicit PostgreSQL configuration contract, so no child tickets or planning documents were created.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment