[gicket-bot] PO refinement contract

Summary
- Ratified the ticket as a metadata-only privacy preflight and coverage contract backed by existing repository docs, diagnostics code, and tests; no child-ticket, relation, description, attachment, or planning-document writes were needed in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 contract is opt-in and provider-neutral: it evaluates configured `personalData[].encryptedPayloadAlias` markers, registered privacy aliases, caller-owned key-provider posture, and observed EF model converter wiring only.
- Coverage is preflight diagnostics only. It does not own encryption policy, key lifecycle, deletion workflow, retention workflow, crypto-shredding execution, provider-native encryption behavior, or compliance guarantees.
- Repository evidence already fixes the severity baseline: missing privacy proof is a warning (`personal-data-privacy-proof-missing`), while configured-but-unusable coverage is an error (`personal-data-privacy-coverage-unusable`).
- No ticket description update, attachment, planning document, child-ticket creation, or relation change was applied; the existing six `parentOf` links and one `relates` link remain unchanged.

Scope In
- Define the v1 preflight contract for satellite personal-data metadata that marks existing payload fields with stable `encryptedPayloadAlias` values.
- Define how diagnostics evaluate alias coverage through opt-in privacy proof(s), caller-owned key-provider capability, and EF model inspection of `DataVaultEncryptedPayloadValueConverter` wiring on the marked payload field.
- Define deterministic redaction-safe coverage reporting over registered aliases, covered aliases, registered-but-unmapped aliases, and key-provider posture without database queries.
- Define the bounded warning and error taxonomy and message posture for missing proof, missing alias registration, unusable key provider, proof failure or no evaluation, missing EF satellite or payload mapping, and missing or wrong converter alias wiring.

Scope Out
- Key creation, storage, selection, rotation, destruction, escrow, KMS or HSM integration, and other key-lifecycle responsibilities.
- Deletion, retention, purge, archival, re-encryption, historical rewrite, crypto-shredding workflow ownership, or legal and compliance attestation.
- Provider-native encryption capability detection, provider-specific encryption DDL or SQL, migration behavior, or automatic runtime routing based on provider features.
- Default `AddDVault()` or `SaveChanges` behavior changes, hidden background processing, or automatic privacy execution for callers that did not opt in.

Open questions
- none

Follow-up questions
- Downstream delivery should confirm which of the existing child tickets own parser, registry, EF translation, or provider follow-through against this parent contract; no additional split was required during PO refinement.
- If an application-facing preflight artifact or CLI output is later desired, decide in a separate ticket whether the current coverage report display string is sufficient or whether a machine-readable export contract is needed.
- Any later provider-native or provider-optimized privacy capability should be scoped as a separate provider-specific ticket per provider and capability pair.

Risks
- Consumers may misread personal-data metadata or `AddDVaultPrivacy(...)` registration as a compliance or automatic-encryption guarantee unless the warning text and docs stay explicit.
- Metadata-only analysis is intentionally strict: callers that do not analyze a DbContext with observed converter wiring may see unusable-coverage diagnostics even when they have partial application code in progress.
- Follow-on implementation tickets may try to absorb retention, deletion, or key-lifecycle behavior into this shared contract unless scope boundaries are reasserted during review.

Split recommendations
- No new split is recommended in this PO pass; the ticket already has six `parentOf` child tickets and one `relates` link, and this refinement ratifies the parent contract those follow-on lanes should consume.
- If new work appears around provider-native encryption or operational lifecycle workflows, create separate child or related tickets instead of widening this contract.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment