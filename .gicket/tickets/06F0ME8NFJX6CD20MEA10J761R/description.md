<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository, ticket, comment, relation, and referenced planning evidence all support the existing bounded fluent code-first story split; no new child tickets, relation changes, attachments, or planning documents were needed, and the story is ready for PO-critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md remains the authoritative parent planning contract for this story.
- Live parentOf relations already link this story to done design task 06F0ME976PM5455JK04S6GPNNW and done implementation children 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G; no relation cleanup or new split was required.
- Repository evidence already ratifies an additive root-namespace DataVaultCodeFirst builder family and the ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) entry point instead of mutating the older DCoding.Data.DVault.Modeling builders.
- The bounded v1 fluent satellite baseline is hub-parent satellites only; DrivingKey(...) is the only fluent multi-active opt-in and link-parent satellites stay on the metadata-first path for now.
- No new attachment or planning-document write was materialized during this refinement because the referenced contract document and existing done child tickets already supplied the required evidence.

### Scope In
- Additive EF Core code-first entry point in DCoding.Data.DVault via ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>).
- Hub declarations by CLR entity type with repeated direct scalar BusinessKey(...) selectors in canonical order.
- Hub-parent satellite declarations with explicit satellite names, repeated Payload(...) selectors, and ordered DrivingKey(...) selectors for the covered multi-active shape.
- Link declarations over previously configured hubs with explicit or derived relationship names and declaration-ordered participants.
- Projection of fluent declarations into DataVaultMetadataModel and then through the existing provider-aware metadata-first translator.
- Parity coverage for generated names, columns, keys, indexes, and the visible built-in provider profiles Sqlite, Oracle, Postgres, SqlServer, and MySql.

### Scope Out
- Link-parent satellite fluent declarations; v1 stays bounded to hub-parent satellites.
- Model-first import or export workflows and registry file workflows.
- Automatic SaveChanges writes, save-service convenience generation, or other hidden write interception.
- Breaking changes to existing metadata-first ApplyDataVaultMetadata APIs or the legacy DCoding.Data.DVault.Modeling builders.
- Hub-name overrides, PIT or bridge work, and same-hub or recursive link participant alias support.

## Acceptance Criteria
- Callers can configure hubs, hub-parent satellites, and links through the additive code-first overload without regressing current metadata-first overloads.
- Repeated direct scalar BusinessKey(...), Payload(...), and DrivingKey(...) calls preserve declaration order and reject duplicate logical members.
- Links require at least two previously configured hub participants, preserve participant order end-to-end, and support both explicit relationship names and the derived default name.
- Unsupported selector or participant-resolution shapes fail fast with actionable ArgumentException messages that name the fluent API being used.
- For covered hub, hub-parent satellite, multi-active driving-key, and link scenarios, code-first projection remains schema-equivalent to the metadata-first path in table, column, primary-key, and secondary-index shape across the built-in provider-profile matrix.

## Definition of Done
- The authoritative parent contract remains docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md, and the story's existing parentOf relations to tickets 06F0ME976PM5455JK04S6GPNNW, 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G stay consistent with the intended split.
- Public API and snapshot coverage expose the additive DataVaultCodeFirst builder family in DCoding.Data.DVault while keeping current metadata-first APIs intact.
- Automated tests cover hub, ordinary hub-parent satellite, covered DrivingKey(...) multi-active hub-parent satellite, and link parity through the existing translator path, including SQLite schema parity and built-in provider-profile inspection.
- No link-parent satellite, model-first, save-interception, provider-specific SQL, PIT, or bridge behavior is introduced under this story.
- No blocking PO clarification remains on entry-point placement, selector rules, participant ordering, bounded multi-active shape, child ownership, or metadata-first compatibility.

## Implementation Notes
- Repository source already shows DataVaultCodeFirstModelBuilder building DataVaultMetadataModel, DataVaultCodeFirstHubBuilder capturing repeated BusinessKey(...) calls, DataVaultCodeFirstLinkBuilder capturing ordered Participant<TEntity>() calls, and DataVaultCodeFirstMemberSelector enforcing direct readable scalar selector rules.
- The visible annotation and model baseline already includes DataVaultPropertyRole.DrivingKey, which ratifies the covered multi-active projection path instead of reopening that naming or column-role decision.
- Repository tests already expose the bounded verification layers DataVaultCodeFirstMetadataTranslationTests, DataVaultCodeFirstLinkTests, and DataVaultCodeFirstSchemaParityTests; use that finite baseline rather than inventing a second translation or naming harness.
- The completed contract-design ticket 06F0ME976PM5455JK04S6GPNNW and completed implementation children 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G are historical completion context, not open blockers for this story.
- No bounded planning writes were needed during this refinement because the existing contract document, live relations, and repository evidence were already sufficient.

## Open Questions
- none

## Follow-Up Questions
- If a later release needs fluent link-parent satellites, should that land as a dedicated follow-up ticket instead of widening the bounded hub-parent v1 surface?
- After registry or model-first work lands, does the team want an explicit code-first hub-name override, or should metadata-first remain the escape hatch for non-CLR logical hub names?
- If consumers need recursive or same-hub self-links, should a future fluent expansion add participant-role or alias support instead of relying only on repeated Participant<TEntity>() calls?

## Risks
- If future implementation broadens selector parsing beyond direct readable scalar members without updating the contract and parity coverage, deterministic ordering and validation behavior can drift.
- If fluent projection bypasses DataVaultMetadataModel or redefines naming and key rules locally, schema parity with the metadata-first translator can regress.
- If later work quietly widens the surface to link-parent satellites or same-hub recursive links without a dedicated contract update, participant naming and column-shape assumptions can become ambiguous.
- If a consumer immediately needs non-CLR hub names, the v1 default-to-CLR-type-name stance forces a temporary fallback to metadata-first declarations.

## Split Recommendations
- No new split is required; keep the current live parentOf structure with the completed design contract ticket 06F0ME976PM5455JK04S6GPNNW and implementation children 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G.
- If release scope later expands to hub-name overrides, link-parent satellites, or same-hub or recursive link roles, materialize those as dedicated follow-up tickets rather than reopening this bounded story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Expose a fluent EF Core modeling API so users can declare hubs, satellites, and links from domain entity configuration instead of hand-building the full metadata graph.

## Scope In

- Public API design for hub, satellite, and link declarations.
- Projection into existing DataVaultMetadataModel and EF shared-type table metadata.
- Tests for generated names, columns, keys, indexes, and provider profiles.

## Scope Out

- Model-first file import/export.
- Automatic writes from SaveChanges.
- Breaking changes to existing ApplyDataVaultMetadata APIs.

## Acceptance Criteria

- Domain entities can be configured as hubs with business-key selectors.
- Ordinary satellites can be declared fluently with payload selectors.
- Links can be declared fluently with deterministic participant ordering.
- Generated schema remains equivalent to the metadata-first path for covered scenarios.