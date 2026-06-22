[gicket-bot] PO refinement contract

Summary
- Refined the ticket to an alias-driven caller-owned key-provider and crypto-shredding boundary; no new child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The done parent story `06FE4R9PP99G6Q1PTPK4TKD460` and the done provider-native decision ticket `06FE4SENE1ZV45P8DKRQTMG0A0` already fix the outer boundary: privacy behavior stays explicit, opt-in, provider-neutral in the shared surface, and not a provider-native encryption platform, KMS surface, or automatic workflow engine.
- The done metadata contract ticket `06FE4R9ZC210EE5AW4WCWQN32G` already ratifies `personalData[].encryptedPayloadAlias` as the stable logical encrypted-payload identifier; this ticket should use that alias as the v1 lookup key for caller-owned key-provider selection instead of reopening naming or provider-column questions.
- Repository evidence already shows the safe architectural precedent for provider-neutral model/provider conversion without changing caller-facing value types: `DataVaultHashKeyProviderValueConverter` and the EF `LowercaseHexStringToBytesConverter` lane in `DataVaultEfMetadataTranslator`.
- No child tickets, relation changes, attachments, or planning documents were materialized in this refinement pass; the live graph already contains the done boundary anchors above, the current outgoing `blocks` edge to `06FE4RAGWXQCQFCTX7QW1T9NAC`, and the sibling implementation split from `06FE4SENE1ZV45P8DKRQTMG0A0`.

Scope In
- Define the provider-neutral caller-owned key-provider boundary for the privacy extension, including how explicit DVault privacy flows resolve cryptographic behavior without taking ownership of key material or provider-native encryption features.
- Ratify `encryptedPayloadAlias` as the stable logical lookup key that binds marked satellite payload fields to caller-owned encryption and decryption behavior.
- Define the explicit activation posture: any future encryption lane is opt-in and reached only through explicit save, read, helper, or value-conversion flows, not through default `SaveChanges`, hidden background jobs, or implicit provider negotiation.
- Define the ownership split for key creation, storage, lookup policy, rotation, destruction, access control, and audit versus the limited DVault-owned seam, metadata interpretation, and diagnostics responsibilities.
- Define the v1 meaning of crypto-shredding for this lane: previously stored encrypted payloads become intentionally undecryptable when the caller withdraws or destroys the relevant key material; DVault does not own the operational workflow.

Scope Out
- Implementing the provider-neutral conversion proof; that remains the separate downstream ticket `06FE4RASEQZN7XEYH1XR4H06PR`.
- Creating the privacy package skeleton or package layout details; that remains the separate downstream ticket `06FE4RAGWXQCQFCTX7QW1T9NAC`.
- Provider mapping tests and documentation examples; those remain the separate downstream tickets `06FE4RB219AXVF2535MFF36PN4` and `06FE4RBK2MJBS5K3C15JTB8Z9W`.
- DVault-owned key generation, secret storage, KMS or HSM integration, key rotation orchestration, key escrow, access-policy enforcement, or audit workflow routing.
- DVault-owned purge, retention scheduling, row deletion, PIT or bridge cleanup, re-encryption, backfill, migration, or compliance-completion workflow execution.
- Provider-native encryption DDL, SQL function generation, driver key-store negotiation, or treating stable hashing as a privacy or cryptographic control.

Open questions
- none

Follow-up questions
- After the provider-neutral proof lands, does the product want a higher-level helper API over raw alias-based encryption/decryption lookup, or is the lower-level seam enough for the first privacy package wave?
- Should a later ticket define a recommended alias-to-key-version rollover or re-encryption documentation pattern for adopters, while keeping the operational work caller-owned?
- If adopters need physical deletion, retention, or compliance workflow guidance in addition to crypto-shredding posture, should that be a separate explicit documentation or architecture ticket rather than an expansion of this boundary?

Risks
- If fail-closed behavior is not stated explicitly, downstream implementations could silently fall back to plaintext or mismatched alias behavior when a caller-owned mapping is missing.
- The term crypto-shredding can be overread as DVault-owned deletion or compliance orchestration unless the contract keeps it limited to caller-owned key unavailability and separates it from purge or retention workflows.
- Diagnostics, support bundles, or exception paths could leak sensitive material if the contract does not preserve the repository's existing redaction posture for secrets and raw business data.
- A too-generic abstraction could drift into provider-native encryption promises or KMS ownership; keeping the alias-based seam narrow is the main guardrail against scope creep.

Split recommendations
- No additional split is needed now; the existing downstream tickets already cover package skeleton, provider-neutral conversion proof, mapping tests, and documentation after this boundary ticket.
- If future work needs KMS integration, provider-native encryption, re-encryption tooling, or retention or purge orchestration, split that work by exact capability and by provider or operational lane instead of broadening this ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment