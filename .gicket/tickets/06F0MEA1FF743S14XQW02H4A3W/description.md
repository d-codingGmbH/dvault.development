<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the fluent link child ticket to the parent contract: it now covers only link-specific code-first API, ordered participant resolution, projection through the existing metadata translator, and focused link tests, with no blocking PO questions left.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The parent contract and the fluent-link child boundary are authoritative; this child owns only fluent link and relationship projection.
- The fluent surface is additive in DCoding.Data.DVault and must project into DataVaultMetadataModel before reusing the existing ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...) translator path.
- Links are declared from hub CLR types that have already been configured in the same code-first model; participant declaration order is the canonical order for default naming, metadata projection, and generated relationship index columns.
- This child is responsible for explicit relationship names and the derived default when no name is supplied, but not for hub-name overrides, link-parent satellites, save helpers, or provider-specific SQL behavior.
- Covered v1 verification for this child is link-focused: one explicit-name two-participant case, one derived-name multi-participant case, and clear failure paths for missing, ambiguous, or unsupported participant resolution.

### Scope In
- Add the minimum fluent code-first link API surface needed to declare links from previously configured hub CLR types, with both explicit-name and derived-name entry points.
- Capture ordered Participant<T>() declarations and resolve them to configured hubs in the same code-first model.
- Project fluent link declarations into DataVaultLinkMetadata and then through the existing provider-aware EF shared-type translation pipeline.
- Add link-specific unit and schema/parity-style tests for covered two-participant and multi-participant shapes plus failure cases.

### Scope Out
- Hub business-key capture and hub-parent satellite payload or DrivingKey capture, which remain on ticket 06F0ME9PM8KXH3VP59TQR0ETA8.
- Parity breadth beyond the link-specific scenarios needed here, which remains on ticket 06F0MEAD1BAA5QEVM3F9QJA38G.
- Link-parent satellites, typed save or read helpers, save-service behavior, or SaveChanges interception.
- Provider-specific SQL, migrations, foreign keys, navigations, or new translator branches outside the existing metadata-first path.

## Acceptance Criteria
- An additive ModelBuilder code-first overload and the minimum public DataVaultCodeFirst* link builder types in DCoding.Data.DVault allow callers to declare a link with either an explicit relationship name or a derived default name from ordered participants.
- Covered link declarations require at least two participants, preserve declaration order end-to-end, and project that same order into DataVaultLinkMetadata participants, participant hash-key columns, and the relationship index column order.
- Link configuration throws actionable ArgumentException failures when a participant hub is missing, when more than one configured hub resolves to the requested participant CLR type, or when the participant shape is outside the bounded v1 support for this child.
- For one explicit-name two-participant example and one derived-name multi-participant example, the fluent-produced link metadata and generated EF schema match the metadata-first equivalent in table, column, primary-key, and relationship-index shape.
- Provider-aware identifier truncation, included-index handling, and other provider differences continue to come only from the existing ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...) path, with no new provider-specific translation logic introduced by this child.

## Definition of Done
- The new public link-focused code-first API is exposed from DCoding.Data.DVault using the DataVaultCodeFirst* naming family, while existing metadata-first APIs remain available unchanged.
- Automated tests cover successful explicit-name two-participant and derived-name multi-participant link declarations plus clear missing-hub, ambiguous-hub, and unsupported-shape failures.
- Schema or translation assertions prove the produced link names and column order remain aligned with the current metadata-first baseline, including ordered participant columns for the multi-participant example.
- No unrelated hub, satellite, parity, save-service, or provider-specific behavior is added under this ticket.

## Implementation Notes
- Keep the new fluent types in the root DCoding.Data.DVault namespace and use the DataVaultCodeFirst*Builder naming family defined by the parent contract to avoid colliding with the existing Modeling builders.
- Reuse the current naming and translation baseline instead of re-implementing it: build ordered DataVaultLinkMetadata from the fluent declarations, then call the existing metadata overload so keys, indexes, and provider-aware behavior stay centralized.
- Use exact one-match CLR hub resolution semantics for participants, aligned with the repository's existing metadata registry behavior: missing mappings fail clearly, and duplicate configured hub mappings for one CLR type are treated as ambiguous.
- Preserve current produced-name conventions from DefaultNamingPolicy and DefaultDataVaultNamingPolicy; for example, the existing multi-participant baseline projects LinkCustomerOrderRegion with ordered participant columns CustomerHashKey, OrderHashKey, and SaleRegionHashKey.
- If repeated same-hub participants are not representable within the bounded v1 link projection without colliding participant hash-key names, reject them before translation with an actionable error rather than silently producing invalid metadata.

## Open Questions
- none

## Follow-Up Questions
- If a later code-first consumer needs recursive or same-hub self-links, should a future fluent expansion add explicit participant-role or alias support instead of relying only on repeated Participant<T>() calls?

## Risks
- This child and the hub or satellite sibling both touch the shared code-first entry surface, so parallel delivery can create API or merge drift unless shared scaffolding stays minimal.
- Any loss of participant declaration order or drift from current naming normalization will change produced link table, key, and index names and break metadata-first equivalence.
- Repeated same-hub participants can produce duplicate participant hash-key names under the current link naming path if the code-first layer does not reject unsupported shapes early.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Add fluent link declarations for relationships between configured hubs while preserving deterministic participant ordering and existing link naming conventions.

## Scope In

- Relationship-name support.
- Participant resolution from configured hubs.
- Projection to DataVaultLinkMetadata and EF shared-type metadata.
- Tests for two-participant and multi-participant links.

## Scope Out

- Hub/satellite fluent projection.
- Typed save helpers.
- Provider-specific SQL changes.

## Acceptance Criteria

- Link configuration fails clearly when a participant hub is missing or ambiguous.
- Generated link schema matches the metadata-first equivalent.
- Relationship indexes and primary keys remain provider-aware through the existing translator path.