[gicket-bot] PO refinement contract

Summary
- Refined the ticket into an additive privacy-metadata contract lane: keep existing satellite `payload` declarations unchanged, add opt-in provider-neutral personal-data field metadata keyed to existing payload names, and define one stable logical encrypted-payload alias per marked field without reopening DVault history semantics or provider-specific EF mapping. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already fixes the v1 baseline: `docs/plans/dvault-model-v1-schema-contract.md` defines satellites through ordered `payload` name arrays, and current core modeling only carries provider-neutral payload column names, so this ticket should add privacy metadata beside that baseline instead of replacing it.
- The approved privacy boundary in `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` already allows opt-in metadata annotations or sidecar metadata visible at model-configuration time; this ticket is the bounded contract for that metadata, not an encryption implementation story.
- The safe v1 default is additive and opt-in: unannotated satellite payload fields keep current behavior, current payload/logical-property mapping, and current history semantics.
- Personal-data metadata applies only to satellite payload fields, not to hub business keys, link participant references, driving keys, hash keys, load timestamp, record source, PIT rows, or bridge rows.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Define the authoritative v1 contract for marking existing satellite payload fields as personal-data fields.
- Define an additive provider-neutral metadata shape that references already-declared satellite `payload` names rather than replacing the existing payload declaration model.
- Define one stable logical encrypted-payload alias per marked field so downstream privacy packages can resolve caller-owned encryption behavior without hard-coding provider-specific column or DDL choices in the shared contract.
- Define validation rules for field existence, uniqueness, opt-in defaults, and conflicts with non-payload names.
- Define compatibility rules that keep satellite parent semantics, multi-active driving-key semantics, hash-diff presence, load timestamp, record source, and existing provider-neutral EF mapping assumptions intact.

Scope Out
- Implementing encryption, decryption, pseudonymization, redaction, export filtering, or retention behavior in product code.
- KMS or HSM integration, key lifecycle, secret storage, key rotation, or compliance guarantees.
- Provider-specific DDL, ciphertext store types, generated SQL, migrations, or physical storage layouts.
- Reworking the base `dvault.model.v1` satellite `payload` array into a new non-compatible declaration shape.
- Extending the same metadata contract to hubs, links, PITs, bridges, diagnostics payloads, or workflow orchestration in this ticket.

Open questions
- none

Follow-up questions
- After this contract is approved, should the first implementation lane be model-first or parser support, code-first or registry API support, or the privacy package skeleton ticket `06FE4RAGWXQCQFCTX7QW1T9NAC`?
- Does a later privacy-read ticket need a sibling contract for redaction or export behavior over the same marked satellite fields, or is field identification plus encrypted-payload aliasing enough for the first v1 implementation wave?
- If multiple marked fields eventually need to share one encrypted container, should that be a later additive capability rather than part of the initial per-field v1 baseline?

Risks
- If the contract tries to replace the existing `payload` declaration shape instead of augmenting it, it will reopen the already-fixed `dvault.model.v1` artifact contract and create parser and exporter churn.
- If provider-specific ciphertext storage or crypto choices leak into the shared contract, the provider-neutral EF boundary approved by the privacy story will erode quickly.
- If the metadata contract is vague about history compatibility, later implementation work may accidentally couple privacy mapping to changed hash-diff or multi-active behavior.

Split recommendations
- No split is needed if this ticket stays as the authoritative contract-definition lane for personal-data satellite field metadata.
- Keep parser or API changes, privacy package behavior, and provider-specific execution or storage optimization as follow-on implementation tickets rather than widening this ticket.

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