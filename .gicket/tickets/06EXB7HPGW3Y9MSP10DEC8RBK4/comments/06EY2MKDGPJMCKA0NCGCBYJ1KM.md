[gicket-bot] PO-critic review contract

Summary
- Return to PO: the repository already proves satellite schema support and the hub/link explicit save-service baseline, but the ticket still leaves the caller-visible satellite request/result contract and hash-diff ownership unspecified.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/description.md:11-16 and 43-48 says the ticket extends IDataVaultSaveService/DataVaultSaveRequest, compares against the latest persisted hash diff, and explicitly allows either internally computed hash diffs or request-carried deterministic hash diffs; Open Questions is recorded as `none` at lines 50-51.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:10-68 shows the current public write request only carries LoadTimestamp, RecordSource, HubOperations, and LinkOperations.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:166-230 shows DataVaultSaveResult/DataVaultSavedRecord currently summarize generated hub/link hash-key results only; DataVaultSavedRecord is documented as one hub or link row and only carries Kind, MetadataName, TableName, and HashKey.
- Repository search rg -n 'SatelliteSaveOperation|SatelliteOperations|DataVaultSatelliteSaveOperation|SatelliteOperation' src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests returned no matches (exit 1).
- docs/architecture/dvault-v1-explicit-save-service.md:8-25 defines the current explicit write entry point as load timestamp plus record source plus hub/link row intent, with deterministic reuse lookup and no implicit SaveChanges path.
- docs/plans/stable-hashing-contract.md:47-74 and 135 says later domain-specific tickets must decide which fields participate in a hash, and docs/architecture/mvp-data-vault-concepts.md:58-63 says hash-diff algorithm and payload normalization are not prescribed and SQLite tests may use explicit text hash diffs.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:273-336, src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:164-218, tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:103-130, and tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs:54-71 show the repository already supports satellite metadata/schema for both hub and link parents, with parent hash key plus HashDiff plus LoadTimestamp plus RecordSource and PK (parentHashKey, LoadTimestamp).
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11-166 currently exercises only hub/link persistence and hub/link reuse.
- git log --oneline --grep '06EXB7HEJY18HEB5A5MVTN5KZC' --all returned `aac93dc3 [06EXB7HEJY18HEB5A5MVTN5KZC] AUTO-INTEGRATION squash into develop`, and .gicket/relations/ZC/K4/06EXB7HEJY18HEB5A5MVTN5KZC--06EXB7HPGW3Y9MSP10DEC8RBK4--blocks.json records that upstream ticket as a blocker of this one.

Blocking findings
- The ticket asks developers to extend the existing explicit save-service boundary but never fixes the caller-visible satellite request shape. Current source exposes only hub/link operations on DataVaultSaveRequest, so developers still lack a source-backed product contract for what a satellite save request must carry.
- The ticket also leaves visible result semantics unresolved. Current DataVaultSaveResult/DataVaultSavedRecord only summarize hub/link hash-key outputs, but the contract does not say whether satellite saves must appear in SavedRecords, be omitted intentionally, or require a different result surface.
- Hash-diff ownership is still ambiguous at the ticket level: description.md:48 allows either computing the hash diff inside the save path or carrying a deterministic hash diff in the request, while the repository hashing docs intentionally leave satellite field participation and normalization to a domain-specific ticket.

Required PO actions
- Specify the satellite request contract on the explicit save-service boundary: required caller inputs, how the parent hub/link is identified, and whether callers submit HashDiff explicitly or submit only payload values.
- Specify the caller-visible save result behavior for satellite operations: whether DataVaultSaveResult.SavedRecords must include satellite outcomes, whether satellite saves intentionally do not surface there, or whether another result contract is expected.
- Promote the chosen hash-diff strategy into acceptance criteria or examples so developers are not forced to make a public API decision during implementation.

Open issues ledger
- critic-item-1 [required-po-action] Specify the satellite request contract on the explicit save-service boundary: required caller inputs, how the parent hub/link is identified, and whether callers submit HashDiff explicitly or submit only payload values.
- critic-item-2 [required-po-action] Specify the caller-visible save result behavior for satellite operations: whether DataVaultSaveResult.SavedRecords must include satellite outcomes, whether satellite saves intentionally do not surface there, or whether another result contract is expected.
- critic-item-3 [required-po-action] Promote the chosen hash-diff strategy into acceptance criteria or examples so developers are not forced to make a public API decision during implementation.
- critic-item-4 [blocking-finding] The ticket asks developers to extend the existing explicit save-service boundary but never fixes the caller-visible satellite request shape. Current source exposes only hub/link operations on DataVaultSaveRequest, so developers still lack a source-backed product contract for what a satellite save request must carry.
- critic-item-5 [blocking-finding] The ticket also leaves visible result semantics unresolved. Current DataVaultSaveResult/DataVaultSavedRecord only summarize hub/link hash-key outputs, but the contract does not say whether satellite saves must appear in SavedRecords, be omitted intentionally, or require a different result surface.
- critic-item-6 [blocking-finding] Hash-diff ownership is still ambiguous at the ticket level: description.md:48 allows either computing the hash diff inside the save path or carrying a deterministic hash diff in the request, while the repository hashing docs intentionally leave satellite field participation and normalization to a domain-specific ticket.

Missing examples / edge cases
- First satellite insert for a parent with no existing satellite row.
- Hub-parent and link-parent satellite saves through the explicit save service as separate concrete examples.
- An A->B->A timeline with increasing timestamps, so the compare-only-to-current-latest rule is locked by example instead of prose only.
- A changed save that reuses the same parent and same LoadTimestamp as an existing row, which would collide with the source-backed PK (parentHashKey, LoadTimestamp) unless behavior is defined.

Risky assumptions
- Assuming developers may choose the hash-diff source themselves without changing the intended public API.
- Assuming latest persisted version always means the row with the greatest LoadTimestamp and that caller timestamps are monotonic per parent.
- Assuming DataVaultSavedRecord.HashKey can represent a satellite outcome even though the schema keys satellite history by parent hash key plus load timestamp, not by a satellite hash key.

AC / test suggestions
- Add an acceptance criterion and SQLite test for the first insert when no prior satellite row exists.
- Add separate acceptance tests for hub-parent and link-parent satellites, since repository metadata/schema already support both.
- Add an explicit acceptance test for A->B->A on the same parent with three timestamps to prove older historical payloads still insert when the current latest hash diff differs.
- Add an explicit result-contract assertion once PO fixes whether satellite saves are surfaced through SavedRecords.

Implementation watchouts
- Source-backed satellite tables use PK (parentHashKey, LoadTimestamp) and only a non-unique parent index, so change detection must find the latest row for one parent before deciding insert versus suppress.
- The current SQLite capability baseline in docs/architecture/dvault-v1-explicit-save-service.md:25 and the ticket risks section says multi-writer concurrency signals and upserts are out of scope; duplicate suppression is expected to be deterministic pre-insert lookup only.
- Any satellite boundary extension has to stay coherent with the existing explicit-service surface in src/DCoding.Data.DVault/DataVaultSaveService.cs:10-68 and 166-230.

Non-blocking notes
- The persisted contract is otherwise well bounded: Open Questions is `none`, scope-out excludes PIT, bridge, multi-active, and provider-specific work, and upstream hub/link reuse is already integrated.
- Repository source already proves satellite metadata/schema conventions, so the refinement gap is not about schema discovery; it is about locking the public save-service contract.

Split recommendations
- No split is needed if the PO tightens the satellite request/result contract and hash-diff ownership within this ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment