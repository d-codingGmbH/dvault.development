<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the existing SQLite privacy quickstart and repository-backed privacy boundary as the v1 baseline for this ticket; no ticket, relation, attachment, or planning-document writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already fixes the v1 example shape: a checked-in SQLite quickstart wires `AddDVaultPrivacy(...)`, `RegisterEncryptedPayloadAlias(...)`, a caller-owned provider via `UseCallerOwnedKeyProvider(...)`, and `DataVaultEncryptedPayloadValueConverter` on an ordinary EF Core mapped payload property.
- The stable alias baseline is the existing model-first `personalData[].encryptedPayloadAlias` value; runtime registration must use that same logical alias rather than inferring aliases from column names or provider details.
- `UseCallerOwnedKeyProvider(...)` accepts `IDataVaultPrivacyKeyProvider`, but encrypted payload conversion requires the configured provider to also implement `IDataVaultEncryptedPayloadKeyProvider`; this type boundary is already documented and test-backed.
- Key rotation, destruction, escrow, recovery, and crypto-shredding remain caller-owned. DVault provides explicit seams and fail-closed conversion behavior, not compliance automation, retention completion, deletion workflows, or provider-native encryption management.
- The provider-native caveat baseline is already covered by the done upstream boundary ticket `06FGX5NTKQX87FWCZ2GDDVCXEW`; this ticket should consume that authoritative boundary instead of reopening provider-capability research or duplicating inconsistent claims.

### Scope In
- Refine the ticket around the existing compact privacy proof that demonstrates caller-owned key-provider registration, alias registration, encrypted payload conversion on an ordinary mapped property, and round-trip decryption behavior.
- Require fail-closed behavior for missing alias registration, missing key-provider wiring, or unusable encrypted-payload provider posture before plaintext can be stored or silently returned.
- Document that the quickstart is provider-neutral at the DVault seam and SQLite-backed only as the local runnable proof, while provider-native encryption claims stay routed to the existing boundary contract.
- Keep the text explicit that DVault save and read services remain caller-driven and provider-neutral; the privacy proof only affects the opted-in mapped EF Core property.

### Scope Out
- Implementing provider-native encryption, encrypted DDL, provider SQL crypto calls, provider capability probing, or runtime dispatch based on native encryption availability.
- Owning application key lifecycle, compliance posture, deletion, retention, PIT cleanup, bridge cleanup, backup purge, or legal-erasure completion.
- Creating a new provider matrix in this ticket or broadening scope beyond the repository-backed baseline of SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Adding separate provider-specific quickstarts or changing DVault core save/read runtime behavior outside the opt-in privacy seam.

## Acceptance Criteria
- The refined contract uses the checked-in SQLite privacy quickstart shape as the v1 default: `AddDVault(...)` plus `AddDVaultPrivacy(...)`, one registered encrypted-payload alias, one caller-owned key provider, and `DataVaultEncryptedPayloadValueConverter` on an ordinary EF Core mapped payload property.
- The example demonstrates alias registration, provider wiring, encrypted provider-value storage, and decrypted round-trip behavior without exposing raw payload values, ciphertext, key material, connection strings, or provider messages.
- The refined contract requires fail-closed behavior when the alias is unregistered, the key provider is missing, the configured provider is marker-only instead of encrypted-payload-capable, or the caller declines or fails to return a conversion result.
- The text states that key lifecycle, including rotation and destruction, is caller-owned and that DVault provides seams rather than GDPR/DSGVO compliance automation or provider-native encryption behavior.
- Provider-specific caveats stay anchored to the existing privacy boundary documentation and finite provider baseline instead of introducing a second conflicting capability matrix in the quickstart text.

## Definition of Done
- The ticket is refined around the existing repository baseline with no blocking PO questions remaining.
- The runnable example and existing validation lane together cover the privacy proof: the SQLite quickstart remains the local runnable proof, and the current unit coverage verifies encrypted provider-value persistence, decrypted round trip, and fail-closed converter behavior.
- Consumer-facing guidance stays aligned across the current privacy quickstart documentation surfaces and the upstream boundary source, with no claim that DVault automates compliance, key management, or provider-native encryption.
- Live relation context remains coherent after refinement: this ticket stays under story `06FGX5KZHC9ZAKAT71C89MEYV8`, consumes the done upstream boundary ticket `06FGX5NTKQX87FWCZ2GDDVCXEW`, and continues to block downstream docs-alignment ticket `06FGX5S4FTGBE7YQ897BMY1974` without requiring relation cleanup.

## Implementation Notes
- Use the existing checked-in SQLite quickstart as the bounded implementation target rather than inventing a second example surface; that keeps the sample compact and tied to the current validation path.
- Keep alias naming aligned with the model-first `personalData[].encryptedPayloadAlias` entry for the logical field. The repository baseline already uses `CustomerProfileEmailEncrypted` for this purpose.
- Preserve the existing type-boundary wording: startup may register any `IDataVaultPrivacyKeyProvider`, but field-level encrypted payload conversion only works when the configured provider also implements `IDataVaultEncryptedPayloadKeyProvider`.
- Preserve the explicit boundary that the privacy proof uses ordinary EF Core value conversion on a mapped property while DVault history writes and reads still flow through explicit caller-invoked save/read services.
- The authoritative provider-native caveat source is the existing privacy boundary documentation, not ad hoc wording inside the example. No description update, attachment, child-ticket split, or planning-document materialization was necessary during this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- Should downstream documentation work add stronger direct cross-links from the quickstart-facing docs back to the authoritative privacy boundary document to reduce future wording drift?
- If future work wants actual native encryption support, which single provider and exact capability should get the first separate bounded ticket?

## Risks
- Because the privacy caveat appears in several documentation surfaces, partial edits could reintroduce contradictory claims about provider-native encryption or compliance automation if downstream docs work does not keep them aligned.
- Readers may over-interpret the demo key provider as a supported key-management lifecycle unless the caller-owned boundary remains explicit wherever the quickstart is referenced.

## Split Recommendations
- No split is needed for this ticket at refinement time. Any future native-encryption feature should be created as a separate provider-specific ticket for one exact capability at a time.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add a compact, test-backed privacy example that shows how consumers wire a caller-owned key provider and encrypted payload value converter into ordinary EF Core mapped properties.

Acceptance:
- The example demonstrates registration, alias mapping, encryption/decryption flow, and failure behavior when the alias or key provider is missing.
- The text explains that key rotation/destruction is caller-owned and that DVault provides seams rather than compliance automation.
- The sample is runnable or covered by tests in the existing validation lane.
- Provider-specific caveats point back to the boundary matrix instead of duplicating inconsistent claims.