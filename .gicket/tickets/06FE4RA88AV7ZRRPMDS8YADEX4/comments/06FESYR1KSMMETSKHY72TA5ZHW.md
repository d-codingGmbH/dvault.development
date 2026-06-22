[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4RA88AV7ZRRPMDS8YADEX4/description.md contains a full Delivery Contract with 6 acceptance criteria, 4 definition-of-done items, and ## Open Questions set to - none.
- docs/plans/dvault-model-v1-schema-contract.md defines personalData[].encryptedPayloadAlias as required metadata and says it is a logical lookup key, not a provider column, SQL expression, key id, or DDL promise.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md states the privacy add-on is explicitly opt-in, keeps AddDVault()/IDataVaultSaveService behavior unchanged by default, forbids automatic SaveChanges privacy work, and includes the illustrative AddDVaultPrivacy(...).UseCallerOwnedKeyProvider(keyProvider) shape.
- docs/architecture/dvault-v1-explicit-save-service.md states IDataVaultSaveService is the default write boundary and that SaveChanges interceptors do not make DVault persistence implicit by default.
- src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs plus src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs show an existing provider-neutral logical-to-physical conversion precedent via canonical hex <-> bytes conversion and LowercaseHexStringToBytesConverter.
- git diff --name-only 4bb76c892..HEAD lists only .gicket/tickets/06FE4RA88AV7ZRRPMDS8YADEX4/*, so this branch is a ticket-refinement branch rather than a partially implemented code branch.
- git log --oneline --decorate -n 12 -- .gicket/tickets/06FE4RA88AV7ZRRPMDS8YADEX4 docs/architecture/dvault-v1-optional-privacy-extension-boundary.md docs/plans/dvault-model-v1-schema-contract.md src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs shows the PO handoff commit fc43a4a4a followed by the current PO-critic claim 06a336613 on branch ticket/06FE4RA88AV7ZRRPMDS8YADEX4-task-design-caller-owned-key-provider-and-crypto.
- Relation files .gicket/relations/60/X4/06FE4R9PP99G6Q1PTPK4TKD460--06FE4RA88AV7ZRRPMDS8YADEX4--blocks.json, .gicket/relations/A0/X4/06FE4SENE1ZV45P8DKRQTMG0A0--06FE4RA88AV7ZRRPMDS8YADEX4--blocks.json, and .gicket/relations/X4/AC/06FE4RA88AV7ZRRPMDS8YADEX4--06FE4RAGWXQCQFCTX7QW1T9NAC--blocks.json confirm coherent upstream/downstream graph context.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking product gap remains, but downstream proof/tests should cover the fail-closed path for missing alias mappings, unsupported shapes, and provider-decline diagnostics.
- No blocking product gap remains, but downstream proof/tests should show whether reusing one encryptedPayloadAlias across multiple fields or satellites is an intentional shared-lookup scenario.

Risky assumptions
- The contract assumes alias-based lookup is sufficient for v1 without needing a second provider-selection surface; if adopters later need helper-level abstraction or alias-to-key-version rollover guidance, that belongs in follow-up tickets, not this handoff.
- The ticket correctly limits crypto-shredding to loss of decryptability; downstream work must not let callers reinterpret that as row deletion, historical rewrite, or compliance completion.

AC / test suggestions
- Add downstream acceptance/tests that prove plaintext is never silently persisted when alias mapping is missing or a provider declines the shape.
- Add downstream tests that prove diagnostics remain redaction-safe and exclude plaintext, ciphertext, raw keys, secrets, and policy internals.
- Add downstream examples/tests for explicit opt-in activation through helper/value-conversion flows rather than default SaveChanges or hidden background processing.

Implementation watchouts
- Keep the seam provider-neutral and caller-owned: no DVault-owned key generation, storage, rotation, escrow, KMS/HSM integration, or provider-native encryption dispatch.
- Use personalData[].encryptedPayloadAlias as the only v1 lookup handle; do not reopen provider-column, store-type, SQL, or key-id naming surfaces.
- Treat the existing hash-key conversion pipeline as architectural precedent only; do not let the proof ticket drift into claiming stable hashing is itself a privacy control.

Non-blocking notes
- The latest PO refinement comment .gicket/tickets/06FE4RA88AV7ZRRPMDS8YADEX4/comments/06FESWRSYCNF9XZYXADGW7X938.md already marks the ticket ready_for_po_critic and records Open questions - none.
- The prompt seed claimed personalData[].encryptedPayloadAlias was missing, but direct repository inspection shows the field is already documented in docs/plans/dvault-model-v1-schema-contract.md; the live repo evidence is sufficient.

Split recommendations
- Keep the current split. Separate future tickets only if product wants a higher-level helper API, alias-to-key-version rollover guidance, provider-native encryption, KMS/HSM integration, re-encryption tooling, or retention/purge/compliance workflow guidance.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment