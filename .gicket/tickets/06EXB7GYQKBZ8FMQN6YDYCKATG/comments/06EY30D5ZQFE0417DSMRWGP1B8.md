[gicket-bot] PO-critic review contract

Summary
- Persisted contract is source-backed and has no open questions; the referenced APIs, docs, tests, child tickets, and dependency ticket all exist, so the story is ready for developer handoff with only non-blocking coverage gaps.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7GYQKBZ8FMQN6YDYCKATG/description.md contains PO handoff decision ready_for_po_critic and ## Open Questions with '- none', so approve_for_dev is permitted by the stated rule.
- Ticket comment history exists on disk despite the prompt's stale 'Recent comments: <none>' snapshot; the latest refinement/handoff comments include 06EY2Z28P9WSDGHHK6FZS3HGA8.md (PO refinement contract), 06EY2Z3B7X51NB8RS4WGTWJENW.md (po-refinement-ready run report), and 06EY2Z44C56W40SSH90SPWHZXW.md (relation automation follow-up).
- Direct source evidence exists for the required public API/type assumptions: src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21 defines IDataVaultSaveService, :35-67 normalizes DataVaultSaveRequest.LoadTimestamp to UTC, :318-355 processes hub then link then satellite operations, and :529-575 implements latest-row satellite change detection plus stable-hash-based hub/link key computation.
- Direct source evidence exists for the registration and translation boundaries named in the contract: src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-24 registers IDataVaultSaveService, IStableHashService, and IStableHashNormalizer via AddDVault(); src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29-37 exposes ApplyDataVaultMetadata().
- The SQLite-first provider assumption is directly backed by source: src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:9-12 binds translation to DataVaultProviderCapabilityProfiles.Sqlite, and src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:233-249 defines that profile with DataVaultProviderConcurrencySupport.NoneInV1Unsupported.
- Repository tests named by the contract are present and aligned: tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:11-58 covers AddDVault registration, override preservation, UTC request normalization, and validation; tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11-349 covers hub/link persistence, cross-context replay idempotency, and satellite latest-row history behavior; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:103-130 covers link-parent satellite metadata translation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No save-service integration test currently persists a link-parent satellite row end to end; repository evidence covers link-parent satellite metadata translation (SatCustomerOrderState) but the explicit save-service persistence examples only exercise a hub-parent satellite.
- No single repository example builds one mixed DataVaultSaveRequest containing hub, link, and satellite operations together and asserts SavedRecords ordering across all three kinds; the behavior is source-backed by DataVaultSaveService.cs:328-350 rather than shown in one integrated example.
- The repository evidence proves UTC normalization at request construction time and persistence with UTC inputs, but there is no explicit persisted non-UTC example showing the stored UTC value for hub/link/satellite rows.

Risky assumptions
- Approval assumes the SQLite-first representative coverage is sufficient for this parent story even though broader link-attached satellite save coverage is deferred in the ticket's follow-up questions.
- Approval assumes the parent story now functions as an umbrella handoff over already-materialized child work; the repository already contains the APIs/tests named by the contract rather than representing a greenfield implementation gap.
- Approval assumes provider-specific concurrency/upsert behavior remains out of scope, consistent with DataVaultProviderCapabilityProfiles.Sqlite and docs/plans/deferred-data-vault-capabilities.md.

AC / test suggestions
- If v1 developers are expected to validate link-attached satellite persistence explicitly, add one acceptance example or test note for that scenario instead of leaving it only as a later follow-up question.
- Add one end-to-end example that saves hubs, links, and satellites in a single DataVaultSaveRequest and asserts SavedRecords order plus RowsWritten semantics across all three operation kinds.
- If PO wants the UTC guarantee to be more externally testable, add one acceptance example with a non-UTC input timestamp and the expected persisted UTC value.

Implementation watchouts
- Do not expand this story into SaveChanges interception; both the contract and src/DCoding.Data.DVault/DataVaultSaveService.cs define the v1 boundary as explicit IDataVaultSaveService usage.
- Do not imply multi-writer safety, retry semantics, or provider-neutral upsert behavior; the active SQLite profile explicitly declares NoneInV1Unsupported concurrency support.
- Do not fold automatic satellite hash-diff computation into this ticket; the contract and source treat satellite hash diffs as caller-supplied while hub/link keys use IStableHashNormalizer and IStableHashService.

Non-blocking notes
- The prompt snapshot's 'Recent comments: <none>' is stale; persisted review evidence was taken from the on-disk comment history under .gicket/tickets/06EXB7GYQKBZ8FMQN6YDYCKATG/comments/.
- No build or test commands were executed in this review because the run boundary is read-only; the assessment relies on persisted repository source, tests, ticket files, comments, and git history.

Split recommendations
- No new split is recommended; the persisted contract already points to child tickets 06EXB7H6KV753KM125XN3VDRTM, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4.
- Keep provider-specific concurrency or upsert work in a separate follow-up ticket rather than expanding this SQLite-first parent story.
- Keep broader link-attached satellite coverage and any caller convenience API for computing satellite hash diffs as separate follow-up tickets if they are later prioritized.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment