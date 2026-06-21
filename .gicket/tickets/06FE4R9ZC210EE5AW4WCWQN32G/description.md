<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket into an additive privacy-metadata contract lane: keep existing satellite `payload` declarations unchanged, add opt-in provider-neutral personal-data field metadata keyed to existing payload names, and define one stable logical encrypted-payload alias per marked field without reopening DVault history semantics or provider-specific EF mapping. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence already fixes the v1 baseline: `docs/plans/dvault-model-v1-schema-contract.md` defines satellites through ordered `payload` name arrays, and current core modeling only carries provider-neutral payload column names, so this ticket should add privacy metadata beside that baseline instead of replacing it.
- The approved privacy boundary in `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` already allows opt-in metadata annotations or sidecar metadata visible at model-configuration time; this ticket is the bounded contract for that metadata, not an encryption implementation story.
- The safe v1 default is additive and opt-in: unannotated satellite payload fields keep current behavior, current payload/logical-property mapping, and current history semantics.
- Personal-data metadata applies only to satellite payload fields, not to hub business keys, link participant references, driving keys, hash keys, load timestamp, record source, PIT rows, or bridge rows.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- Define the authoritative v1 contract for marking existing satellite payload fields as personal-data fields.
- Define an additive provider-neutral metadata shape that references already-declared satellite `payload` names rather than replacing the existing payload declaration model.
- Define one stable logical encrypted-payload alias per marked field so downstream privacy packages can resolve caller-owned encryption behavior without hard-coding provider-specific column or DDL choices in the shared contract.
- Define validation rules for field existence, uniqueness, opt-in defaults, and conflicts with non-payload names.
- Define compatibility rules that keep satellite parent semantics, multi-active driving-key semantics, hash-diff presence, load timestamp, record source, and existing provider-neutral EF mapping assumptions intact.

### Scope Out
- Implementing encryption, decryption, pseudonymization, redaction, export filtering, or retention behavior in product code.
- KMS or HSM integration, key lifecycle, secret storage, key rotation, or compliance guarantees.
- Provider-specific DDL, ciphertext store types, generated SQL, migrations, or physical storage layouts.
- Reworking the base `dvault.model.v1` satellite `payload` array into a new non-compatible declaration shape.
- Extending the same metadata contract to hubs, links, PITs, bridges, diagnostics payloads, or workflow orchestration in this ticket.

## Acceptance Criteria
- A reviewed contract document defines personal-data satellite metadata as an explicit opt-in additive layer over the existing satellite `payload` contract in `docs/plans/dvault-model-v1-schema-contract.md`.
- The contract states that each personal-data declaration must reference an existing payload field on the same satellite by exact logical name and that undeclared payloads remain ordinary non-privacy payloads by default.
- The contract defines one stable provider-neutral encrypted-payload alias per marked field and explicitly keeps provider-specific ciphertext storage details out of the shared contract.
- The contract defines finite validation failures for unknown payload references, duplicate field declarations, duplicate encrypted-payload aliases within one satellite, and attempts to tag driving keys or technical columns through this surface.
- The contract states that privacy metadata does not change satellite parent identity, row history semantics, multi-active semantics, or the requirement that provider-neutral EF mapping remain compatible with the existing payload/logical-property baseline.
- The contract identifies downstream implementation work as separate tickets for parser or API changes, privacy package behavior, and any provider-specific execution lanes.

## Definition of Done
- Downstream developers can implement model-first parsing, code-first or registry metadata registration, and EF translation without reopening whether privacy markers replace or augment current satellite payload declarations.
- The contract keeps the core DVault metadata surface provider-neutral and avoids promising any specific cipher, provider type mapping, DDL shape, or runtime automation.
- The contract is explicit enough that validators can reject bad field references and unsupported metadata collisions before model application.
- The contract preserves existing satellite history and technical metadata semantics unless a later implementation ticket proves a separate behavior within the approved privacy boundary.

## Implementation Notes
- Use `docs/plans/dvault-model-v1-schema-contract.md` as the artifact-shape anchor: satellites currently own ordered `payload` names and optional `drivingKeys`, so privacy metadata should reference those names rather than redefining satellite payload ordering.
- Use `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` as the boundary anchor: the approved lane is opt-in metadata annotations or sidecar metadata that are caller-owned at activation time.
- Current core modeling evidence supports a safe v1 default where privacy metadata is descriptive and provider-neutral first, and later implementation tickets decide how an optional privacy package turns that metadata into explicit save or read behavior.
- A reasonable deliverable is one authoritative planning or architecture contract document plus worked examples, not product-code encryption behavior.
- If the contract needs an encrypted-payload alias, keep it a stable logical metadata name; do not turn this ticket into a provider-column naming or migration-design story.
- Existing separate work such as ticket `06FE4RAGWXQCQFCTX7QW1T9NAC` can consume this contract later for package structure and dependency boundaries instead of pulling package implementation scope into this metadata-design ticket.

## Open Questions
- none

## Follow-Up Questions
- After this contract is approved, should the first implementation lane be model-first or parser support, code-first or registry API support, or the privacy package skeleton ticket `06FE4RAGWXQCQFCTX7QW1T9NAC`?
- Does a later privacy-read ticket need a sibling contract for redaction or export behavior over the same marked satellite fields, or is field identification plus encrypted-payload aliasing enough for the first v1 implementation wave?
- If multiple marked fields eventually need to share one encrypted container, should that be a later additive capability rather than part of the initial per-field v1 baseline?

## Risks
- If the contract tries to replace the existing `payload` declaration shape instead of augmenting it, it will reopen the already-fixed `dvault.model.v1` artifact contract and create parser and exporter churn.
- If provider-specific ciphertext storage or crypto choices leak into the shared contract, the provider-neutral EF boundary approved by the privacy story will erode quickly.
- If the metadata contract is vague about history compatibility, later implementation work may accidentally couple privacy mapping to changed hash-diff or multi-active behavior.

## Split Recommendations
- No split is needed if this ticket stays as the authoritative contract-definition lane for personal-data satellite field metadata.
- Keep parser or API changes, privacy package behavior, and provider-specific execution or storage optimization as follow-on implementation tickets rather than widening this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: design metadata for identifying personal-data satellite fields and encrypted payload mapping. Acceptance: model stays compatible with DVault history semantics and provider-neutral EF Core mapping.

<!-- gicket-bot:developer-delivery-supplement:v1:start -->
## Developer Delivery Supplement

### Summary
- Added the authoritative additive satellite personal-data metadata contract to `docs/plans/dvault-model-v1-schema-contract.md`.
- Aligned `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` with the same provider-neutral boundary and follow-on implementation lanes.

### Delivered Contract Points
- `personalData` is optional on satellite declarations and defaults to no marked personal-data fields.
- Each declaration references an existing same-satellite `payload` field by exact logical name.
- Each marked field declares one stable logical `encryptedPayloadAlias`.
- The alias is metadata only, not a provider column, store type, SQL expression, algorithm choice, key id, migration instruction, or DDL promise.
- Validators have finite rejection cases for unknown payload references, duplicate marked fields, duplicate encrypted aliases, non-payload targets, and provider-specific privacy/storage fields.
- Personal-data metadata preserves satellite parent identity, row history, multi-active driving keys, hash diff, load timestamp, record source, and provider-neutral EF payload/logical-property compatibility.

### Follow-On Boundaries
- Parser support, code-first or registry APIs, EF metadata/diagnostics translation, optional privacy package behavior, and provider-specific execution lanes remain separate implementation tickets.
- No root `dvault.model.v1` file was created; repository evidence treats `dvault.model.v1` as the schema-version contract name, with the authoritative repository surface in `docs/plans/dvault-model-v1-schema-contract.md`.

### Verification
- `git diff --check -- docs/plans/dvault-model-v1-schema-contract.md docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` passed.
- `bash tools/check-format.sh` passed.
- Full `dotnet build` and `dotnet test` were not run because this delivery changed documentation contracts only.
<!-- gicket-bot:developer-delivery-supplement:v1:end -->