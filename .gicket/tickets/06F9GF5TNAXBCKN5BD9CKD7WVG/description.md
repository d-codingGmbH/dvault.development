<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository and ticket evidence already bound this story to the six built-in provider capability profiles; the v1 binary mapping baseline is SQLite BLOB, Oracle RAW(n), PostgreSQL bytea, SQL Server varbinary(n), DB2 VARBINARY(n), and MySQL varbinary(n), with no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence ratifies the v1 provider-mapping baseline instead of reopening the original draft wording: SQLite uses BLOB, Oracle uses RAW(n), PostgreSQL uses bytea, SQL Server uses varbinary(n), DB2 uses VARBINARY(n), and MySQL uses varbinary(n), sized by the active stable-hash digest byte length except PostgreSQL bytea.
- The visible built-in provider baseline remains finite and exact: sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1 selected from exact provider-name registration.
- The done predecessor story 06F9GF5N4N3Q685XQPKTM5EC00 already owns provider-neutral lowercase-hex string to byte conversion; this story is limited to provider-specific store-type projection and the downstream task 06F9GF60BKEW0CC9FCZRPVX0SR remains the owner of schema/save/read integration coverage.
- No child-ticket, relation, attachment, description, or planning-document writes were materialized in this refinement run.

### Scope In
- Provider-specific Binary hash-key and participant-reference store-type projection in DataVault provider capability profiles for the six built-in provider profiles.
- Sizing each Binary mapping by the active stable-hash digest byte length while keeping the model CLR and public hash-key boundary as canonical lowercase hexadecimal string values.
- Flowing provider profile, store type, value format, storage profile, algorithmId, digestByteLength, digestEncoding, and conversionBehavior facts into EF metadata, explain/support-bundle diagnostics, and migration/preflight guardrail surfaces.
- Deterministic diagnostics for unresolved provider capability selection through the existing capability-profile-defaulted and provider-behavior-defaulted warning surfaces rather than silent new provider-specific claims.

### Scope Out
- Changing public or EF CLR hash-key values from string to byte[] or revisiting the provider-neutral converter implemented in 06F9GF5N4N3Q685XQPKTM5EC00.
- HashDiff storage changes, provider-side SQL hashing, or caller migration/backfill/dual-write tooling.
- End-to-end schema, save, and read integration coverage across providers, owned by 06F9GF60BKEW0CC9FCZRPVX0SR.
- DB2 live-schema reader parity; current contract keeps DB2 live-schema reads on the existing unsupported-provider path.

## Acceptance Criteria
- When HashKeyStorageProfile.Binary is selected, built-in provider profiles project HashKey and ParticipantReference columns to BLOB (sqlite-v1), RAW(n) (oracle-v1), bytea (postgres-v1), varbinary(n) (sqlserver-v1), VARBINARY(n) (db2-v1), and varbinary(n) (mysql-pomelo-v1), with n equal to the active stable-hash digest byte length where the provider uses sized binary storage.
- HexString remains the default storage profile for every built-in provider profile and Binary remains explicit opt-in; neither choice changes the public or EF CLR hash-key boundary away from canonical lowercase hexadecimal string values.
- Translated EF metadata for DVault-owned HashKey and ParticipantReference properties carries ProviderProfile, ProviderLogicalPropertyKind, ProviderStorageType, ProviderValueFormat, HashKeyStorageProfile, StableHashAlgorithmId, StableHashDigestByteLength, StableHashDigestEncoding, and HashKeyConversionBehavior facts consistent with the selected provider profile.
- Diagnostics and support-bundle explain surfaces expose the same provider mapping and hash-key compatibility facts without raw hash values, and unresolved provider-specific selection continues to surface bounded defaulted warnings instead of silent provider-specific guarantees.
- Migration and preflight guardrail inputs continue to compare provider store type, value format, storage profile, algorithmId, digestByteLength, digestEncoding, and conversion behavior so provider-specific Binary mappings remain fail-closed for compatibility drift.

## Definition of Done
- Provider capability profile tests cover the six built-in provider profiles for Binary mapping selection and digest-length sizing.
- EF translation tests prove Binary mappings keep string model CLR projection, annotate the authoritative hash-key facts, and apply the byte[] provider conversion boundary only at the provider layer.
- Diagnostics or support-bundle coverage proves provider store type and hash-key compatibility facts are exported for DVault-owned hash-key properties.
- No implementation work in this story expands into provider-neutral converter rework, HashDiff storage, live-schema DB2 parity, or the separate schema/save/read integration task.

## Implementation Notes
- Repository evidence already fixes the implementation center on DataVaultProviderCapabilityProfile.WithHashKeyStorageProfile(...) and DataVaultProviderCapabilityProfiles rather than a new provider-mapping abstraction.
- The current branch already exposes Binary as LowercaseHexBinary with lowercase-hex-string-to-bytes conversion in DataVaultEfMetadataTranslator, so this story should only change provider-specific store-type projection and preserve the provider-neutral string model boundary.
- Use the existing exact provider-name selection table in DataVaultProviderCapabilityProfileSelection as the finite v1 provider baseline; do not add heuristic or ambiguous provider matching.
- Preserve DefaultDataVaultDiagnosticsService warning behavior when provider resolution defaults, because that is the bounded diagnostics surface for unsupported or unregistered provider-specific mappings.
- DB2 capability verification is already bounded by visible repository evidence: db2-v1 is registered, unit tests expect VARBINARY(n), DB2 save/read provider packages exist, and DB2 live-schema reading remains intentionally unsupported.
- No bounded planning or ticket-surface write was applied during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- Should a later optimization ticket justify provider-specific fixed-length binary(n) or DB2 FOR BIT DATA variants if benchmarks or vendor constraints show material benefit over the current varbinary baseline?
- After this story lands, 06F9GF60BKEW0CC9FCZRPVX0SR should confirm end-to-end schema generation, save paths, and latest/as-of/PIT/bridge read behavior across the supported-provider baseline.
- Should later work add DB2 live-schema reader parity so runtime drift checks can validate the persisted Binary store type against catalog metadata instead of the current unsupported-provider outcome?

## Risks
- If callers rely on implicit provider fallback instead of a resolved built-in or registered provider profile, SQLite-default capability selection could be mistaken for a provider-specific guarantee unless the existing defaulted diagnostics warnings remain visible.
- DB2 parity is only partially proven at the repository level for this story because the live-schema reader intentionally remains unsupported, so full drift verification for DB2 stays outside the current scope.

## Split Recommendations
- No further split recommended; the current story is already bounded between done provider-neutral conversion work in 06F9GF5N4N3Q685XQPKTM5EC00 and downstream integration coverage in 06F9GF60BKEW0CC9FCZRPVX0SR.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add provider-aware binary hash-key column mappings for supported providers: SQL Server binary(n), PostgreSQL bytea, SQLite BLOB, MySQL binary(n) or varbinary(n), Oracle RAW(n), and DB2 binary/bit-data type after DB2 capability verification. Preserve provider-neutral fallback and add capability diagnostics when binary storage is unsupported or ambiguous.