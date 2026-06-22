<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repaired the ticket against the current privacy boundary and provider matrix; upstream encrypted-payload conversion proof 06FE4RASEQZN7XEYH1XR4H06PR is done, the blocking relation into this ticket is materialized, existing done blockers remain historical, and this ticket still blocks 06FE4RBK2MJBS5K3C15JTB8Z9W.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket validates the provider-neutral encrypted attribute conversion proof from done ticket 06FE4RASEQZN7XEYH1XR4H06PR; it does not invent a provider-native encryption lane.
- The concrete upstream privacy seam is `DataVaultEncryptedPayloadValueConverter` plus `DataVaultEncryptedPayloadConversionRequest`, `DataVaultEncryptedPayloadConversionResult`, and `IDataVaultEncryptedPayloadKeyProvider` in `DCoding.Data.DVault.Privacy`; provider metadata still maps the resulting stored value through the normal payload mapping surface.
- The supported provider baseline for this test matrix is the current visible DVault profile set: sqlite-v1, postgres-v1, sqlserver-v1, oracle-v1, db2-v1, and mysql-pomelo-v1.
- At the shared-core mapping layer, encrypted payload storage stays on the existing PayloadText baseline rather than introducing provider-native encrypted column kinds or SQL crypto features.
- The visible provider store types for that baseline are SQLite TEXT, PostgreSQL text, SQL Server nvarchar(max), Oracle CLOB, DB2 CLOB, and MySQL longtext.
- MySQL provider-name aliases remain one capability profile decision surface; provider mapping assertions should cover the shared MySQL profile once unless a test is explicitly about provider-name selection.
- The sequencing relation 06FE4RASEQZN7XEYH1XR4H06PR --blocks--> 06FE4RB219AXVF2535MFF36PN4 is materialized; incoming blocks from done tickets 06FE4RAGWXQCQFCTX7QW1T9NAC, 06FE4SENE1ZV45P8DKRQTMG0A0, and 06FE4RASEQZN7XEYH1XR4H06PR are satisfied, and this ticket still blocks 06FE4RBK2MJBS5K3C15JTB8Z9W.

### Scope In
- Add automated tests for the provider-neutral encrypted payload mapping path introduced by the privacy conversion-proof work, not just generic payload profile coverage in isolation.
- Cover the finite supported-provider matrix across SQLite, PostgreSQL, SQL Server, Oracle, DB2, and MySQL.
- Assert the provider column type, provider profile, logical property kind, and value-format facts exposed by EF metadata translation or the equivalent encrypted payload mapping surface.
- Verify deterministic unsupported-case diagnostics when the encrypted payload mapping path is asked to use a profile that does not declare the required payload capability.
- Make provider caveats explicit in test names and assertions, including the single shared MySQL profile and any existing gated integration-only limits.

### Scope Out
- Provider-native encryption features such as SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL function-based encryption, SQLite encrypted-file variants, or DB2 native encryption.
- Designing the caller-owned key-provider or crypto-shredding lifecycle; that belongs to 06FE4RA88AV7ZRRPMDS8YADEX4.
- Implementing the provider-neutral encrypted attribute conversion proof itself; that belongs to 06FE4RASEQZN7XEYH1XR4H06PR.
- Documentation examples and adopter guidance; that remains downstream in 06FE4RBK2MJBS5K3C15JTB8Z9W.
- Adding new provider profiles, a separate MariaDB baseline, or any new provider-specific DDL contract.

## Acceptance Criteria
- Tests cover the provider-neutral encrypted payload storage mapping for each supported built-in provider profile: sqlite-v1, postgres-v1, sqlserver-v1, oracle-v1, db2-v1, and mysql-pomelo-v1.
- The encrypted payload mapping path resolves to the current ordinary payload store-type baseline per provider: SQLite TEXT, PostgreSQL text, SQL Server nvarchar(max), Oracle CLOB, DB2 CLOB, and MySQL longtext.
- Tests assert the same metadata facts the translator exposes for mapped properties, including provider profile name, provider logical property kind, provider storage type, and provider value format, so the encrypted payload lane cannot silently drift from the agreed provider matrix.
- Unsupported or incomplete capability profiles fail with a deterministic useful diagnostic that names the profile and the missing capability instead of silently downgrading, silently falling back to plaintext behavior, or selecting provider-native encryption features.
- Provider caveats are explicit in the test contract: MySQL provider-name aliases are one shared capability profile, and any live-provider coverage is limited to existing repository gates rather than new infrastructure requirements.

## Definition of Done
- Relevant unit tests for provider capability or metadata translation coverage pass for the encrypted payload mapping matrix.
- Any added integration or live-schema assertions run only through the repository's existing provider gates and do not introduce new mandatory environment prerequisites.
- The test suite proves the privacy conversion lane keeps provider-neutral storage behavior and does not claim provider-native encryption support.
- The resulting matrix and caveats are stable enough that 06FE4RBK2MJBS5K3C15JTB8Z9W can document them without reopening storage-policy scope.

## Implementation Notes
- Use docs/architecture/dvault-v1-optional-privacy-extension-boundary.md and the done decision ticket 06FE4SENE1ZV45P8DKRQTMG0A0 as the authority: shared privacy work is explicit, opt-in, provider-neutral, and not provider-native encryption.
- Anchor the tests in the existing provider-mapping seams already used by DataVaultProviderCapabilityProfileTests, DataVaultEfMetadataTranslationTests, and the live-schema contract fixtures instead of creating a parallel ad hoc matrix harness.
- Target the privacy-specific encrypted payload path by exercising `DataVaultEncryptedPayloadValueConverter` output as a Data Vault payload property and then asserting the EF/provider metadata translation still resolves that property through `DataVaultPropertyRole.Payload` and `DataVaultLogicalPropertyKind.PayloadText`, not a provider-native encrypted column kind.
- Reuse the current deterministic unsupported-capability pattern from DataVaultProviderCapabilityProfile.GetRequiredTypeMapping for negative coverage when a profile lacks the required payload mapping.
- If implementation pressure suggests binary ciphertext columns or provider-native encrypted column types, stop widening this ticket and split that behavior into a separate provider-specific or storage-policy ticket.

## Open Questions
- none

## Follow-Up Questions
- After this mapping matrix lands, should 06FE4RBK2MJBS5K3C15JTB8Z9W publish the exact provider store-type table or summarize the behavior at the provider-neutral contract level only?
- If future privacy work needs a binary ciphertext storage profile instead of ordinary payload text storage, should that be introduced as a separate storage-policy ticket before any provider mapping changes are attempted?

## Risks
- The upstream conversion proof is now landed; the remaining risk is writing tests that exercise only standalone converter behavior or generic `PayloadText` coverage instead of the combined privacy-conversion-plus-provider-mapping path required here.
- Because generic payload store-type coverage already exists in provider profile tests, this ticket can appear done without actually proving the privacy-specific encrypted payload lane unless the tests bind to that explicit path.
- This ticket currently blocks 06FE4RBK2MJBS5K3C15JTB8Z9W, so vague provider caveats or unsupported-case wording here will cascade into documentation churn.

## Split Recommendations
- No split is needed for the current finite provider-matrix test scope.
- If live provider coverage expands beyond the existing gated fixtures, keep the unit or metadata matrix in this ticket and move heavier environment-specific smoke coverage into a separate follow-up.
- If future work wants provider-native encryption behavior or non-text ciphertext storage, split it per provider or per storage policy instead of widening this test ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: add provider mapping tests for encrypted payload column types across supported providers where feasible. Acceptance: provider caveats are explicit and unsupported cases fail with useful diagnostics.