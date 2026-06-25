<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Reconciled the ticket with current repository reality by making this task explicitly own the missing `personalData` transport into runtime diagnostics instead of assuming that transport already exists.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket now owns the smallest complete path needed to make the diagnostic contract real: model-first import of `personalData` markers, metadata-first runtime carriage of the same marked-field evidence, and diagnostics over that shared runtime representation.
- The runtime representation is additive over existing satellite payload metadata and stays keyed by exact logical payload field name plus one stable `encryptedPayloadAlias`, matching the documented schema contract rather than introducing store-specific or provider-specific identifiers.
- The existing opt-in privacy boundary is unchanged: this work adds coverage transport and diagnostics only, not automatic encryption, implicit privacy activation, or key lifecycle behavior.

### Scope In
- Add the minimal model-first parser or import projection needed to carry valid `satellite.personalData[]` field-plus-alias declarations onto the runtime diagnostic path.
- Add a metadata-first runtime carrier for marked satellite payload fields so `DataVaultMetadataModel`-based input can express the same field-plus-alias evidence without inventing a separate privacy execution feature.
- Run missing-alias or unusable-coverage diagnostics over that shared runtime carrier and preserve the advisory versus fail-closed split based on the existing privacy opt-in mode and converter coverage.

### Scope Out
- Automatic encryption, automatic redaction, implicit `SaveChanges` behavior, or any default runtime privacy activation in the core DVault package.
- Key storage, key rotation, deletion workflow, retention workflow, compliance claims, provider-native encryption behavior, or other privacy-governance ownership outside the documented optional extension boundary.
- A new code-first `personalData` authoring surface; this ticket is limited to model-first import, metadata-first carriage, and diagnostics.

## Acceptance Criteria
- Valid model-first `dvault.model.v1` satellite `personalData[]` declarations are projected onto the runtime diagnostic path as marked payload-field plus `encryptedPayloadAlias` evidence instead of being silently unavailable to diagnostics.
- Metadata-first runtime metadata can express the same marked-field evidence per satellite payload using exact logical payload names plus one stable `encryptedPayloadAlias`, without changing the baseline behavior of unmarked payloads.
- Diagnostics evaluate that shared runtime carrier and detect marked fields whose alias or converter coverage is missing or unusable for the active privacy configuration.
- If no privacy extension proof is configured for the affected model boundary, the result is advisory guidance that the field is marked but not covered and that no automatic encryption is implied.
- If the application has opted into the privacy proof but a marked field still lacks usable alias or converter coverage, the result is fail-closed instead of silently permitting plaintext handling or pretending the field is protected.
- Diagnostic output stays provider-neutral and reports logical payload-field and alias coverage rather than store columns, SQL, algorithm choices, or key identifiers.
- Models and metadata declarations without marked personal-data fields keep existing behavior.

## Definition of Done
- One shared runtime marked-field carrier exists for the diagnostic path, and both model-first import and metadata-first declarations can populate it with exact payload-field plus alias evidence.
- The implementation no longer relies on an implicit prerequisite for `personalData` transport; the carrier work required by the diagnostics is delivered as part of this ticket.
- The advisory-versus-fail-closed split matches the documented optional privacy-extension boundary and the existing fail-closed encrypted-payload converter proof.
- The resulting behavior is bounded to coverage transport and diagnostics and does not expand into code-first authoring, automatic crypto behavior, or wider privacy workflow ownership.

## Implementation Notes
- Current source evidence shows that `DataVaultMetadataModel` and `DataVaultSatelliteMetadata` do not yet carry `personalData` or `encryptedPayloadAlias` evidence, so this ticket must add that additive runtime metadata carriage rather than assuming it already exists.
- Use one runtime representation for marked payload fields across declaration paths: model-first import should project `satellite.personalData[]` into it, and metadata-first callers should be able to declare the same field-plus-alias facts directly.
- Keep the carrier aligned to existing satellite payload naming semantics by matching exact logical payload names on the same satellite; do not key diagnostics off generated column names or provider-specific storage details.
- Reuse the existing privacy opt-in baseline for outcome semantics: the documented optional extension boundary governs advisory behavior when privacy is not enabled, and the existing `DataVaultEncryptedPayloadValueConverter` fail-closed proof governs opted-in unusable coverage behavior.
- No external prerequisite ticket or new relation is required after this refinement because the missing transport work is now explicitly in scope here.

## Open Questions
- none

## Follow-Up Questions
- After this core transport-plus-diagnostics work lands, decide whether the follow-on documentation should show paired advisory and fail-closed examples for the same marked-field scenario.
- Consider a later additive ticket for a code-first `personalData` authoring surface if teams need parity with model-first and metadata-first declarations.

## Risks
- Because the ticket now includes minimal transport work in addition to diagnostics, implementation scope must stay tightly bounded to field-plus-alias carriage and must not sprawl into unrelated model-first or metadata-first feature expansion.
- If the metadata-first carrier is not aligned exactly to existing payload-field naming, diagnostics may drift between imported model-first metadata and directly constructed metadata-first models.
- Advisory-mode wording must stay precise so the optional privacy boundary remains opt-in and the ticket does not accidentally imply automatic encryption or compliance guarantees.

## Split Recommendations
- No additional split is recommended at refinement time. The missing transport and the diagnostics that consume it are a single bounded implementation slice, and separating them now would introduce an avoidable implicit prerequisite.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add diagnostics when model-first or metadata-first personalData markers are present but no matching privacy alias/converter configuration exists. Acceptance: diagnostics are advisory/fail-closed depending on existing privacy registration mode and do not imply automatic encryption.