[gicket-bot] PO refinement contract

Summary
- Refinement ratifies the existing SQLite privacy quickstart and repository-backed privacy boundary as the v1 baseline for this ticket; no ticket, relation, attachment, or planning-document writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the v1 example shape: a checked-in SQLite quickstart wires `AddDVaultPrivacy(...)`, `RegisterEncryptedPayloadAlias(...)`, a caller-owned provider via `UseCallerOwnedKeyProvider(...)`, and `DataVaultEncryptedPayloadValueConverter` on an ordinary EF Core mapped payload property.
- The stable alias baseline is the existing model-first `personalData[].encryptedPayloadAlias` value; runtime registration must use that same logical alias rather than inferring aliases from column names or provider details.
- `UseCallerOwnedKeyProvider(...)` accepts `IDataVaultPrivacyKeyProvider`, but encrypted payload conversion requires the configured provider to also implement `IDataVaultEncryptedPayloadKeyProvider`; this type boundary is already documented and test-backed.
- Key rotation, destruction, escrow, recovery, and crypto-shredding remain caller-owned. DVault provides explicit seams and fail-closed conversion behavior, not compliance automation, retention completion, deletion workflows, or provider-native encryption management.
- The provider-native caveat baseline is already covered by the done upstream boundary ticket `06FGX5NTKQX87FWCZ2GDDVCXEW`; this ticket should consume that authoritative boundary instead of reopening provider-capability research or duplicating inconsistent claims.

Scope In
- Refine the ticket around the existing compact privacy proof that demonstrates caller-owned key-provider registration, alias registration, encrypted payload conversion on an ordinary mapped property, and round-trip decryption behavior.
- Require fail-closed behavior for missing alias registration, missing key-provider wiring, or unusable encrypted-payload provider posture before plaintext can be stored or silently returned.
- Document that the quickstart is provider-neutral at the DVault seam and SQLite-backed only as the local runnable proof, while provider-native encryption claims stay routed to the existing boundary contract.
- Keep the text explicit that DVault save and read services remain caller-driven and provider-neutral; the privacy proof only affects the opted-in mapped EF Core property.

Scope Out
- Implementing provider-native encryption, encrypted DDL, provider SQL crypto calls, provider capability probing, or runtime dispatch based on native encryption availability.
- Owning application key lifecycle, compliance posture, deletion, retention, PIT cleanup, bridge cleanup, backup purge, or legal-erasure completion.
- Creating a new provider matrix in this ticket or broadening scope beyond the repository-backed baseline of SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Adding separate provider-specific quickstarts or changing DVault core save/read runtime behavior outside the opt-in privacy seam.

Open questions
- none

Follow-up questions
- Should downstream documentation work add stronger direct cross-links from the quickstart-facing docs back to the authoritative privacy boundary document to reduce future wording drift?
- If future work wants actual native encryption support, which single provider and exact capability should get the first separate bounded ticket?

Risks
- Because the privacy caveat appears in several documentation surfaces, partial edits could reintroduce contradictory claims about provider-native encryption or compliance automation if downstream docs work does not keep them aligned.
- Readers may over-interpret the demo key provider as a supported key-management lifecycle unless the caller-owned boundary remains explicit wherever the quickstart is referenced.

Split recommendations
- No split is needed for this ticket at refinement time. Any future native-encryption feature should be created as a separate provider-specific ticket for one exact capability at a time.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment