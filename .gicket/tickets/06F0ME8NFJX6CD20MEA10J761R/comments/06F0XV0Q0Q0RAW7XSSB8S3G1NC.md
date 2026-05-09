[gicket-bot] PO refinement contract

Summary
- Repository, ticket, comment, relation, and referenced planning evidence all support the existing bounded fluent code-first story split; no new child tickets, relation changes, attachments, or planning documents were needed, and the story is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md remains the authoritative parent planning contract for this story.
- Live parentOf relations already link this story to done design task 06F0ME976PM5455JK04S6GPNNW and done implementation children 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G; no relation cleanup or new split was required.
- Repository evidence already ratifies an additive root-namespace DataVaultCodeFirst builder family and the ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) entry point instead of mutating the older DCoding.Data.DVault.Modeling builders.
- The bounded v1 fluent satellite baseline is hub-parent satellites only; DrivingKey(...) is the only fluent multi-active opt-in and link-parent satellites stay on the metadata-first path for now.
- No new attachment or planning-document write was materialized during this refinement because the referenced contract document and existing done child tickets already supplied the required evidence.

Scope In
- Additive EF Core code-first entry point in DCoding.Data.DVault via ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>).
- Hub declarations by CLR entity type with repeated direct scalar BusinessKey(...) selectors in canonical order.
- Hub-parent satellite declarations with explicit satellite names, repeated Payload(...) selectors, and ordered DrivingKey(...) selectors for the covered multi-active shape.
- Link declarations over previously configured hubs with explicit or derived relationship names and declaration-ordered participants.
- Projection of fluent declarations into DataVaultMetadataModel and then through the existing provider-aware metadata-first translator.
- Parity coverage for generated names, columns, keys, indexes, and the visible built-in provider profiles Sqlite, Oracle, Postgres, SqlServer, and MySql.

Scope Out
- Link-parent satellite fluent declarations; v1 stays bounded to hub-parent satellites.
- Model-first import or export workflows and registry file workflows.
- Automatic SaveChanges writes, save-service convenience generation, or other hidden write interception.
- Breaking changes to existing metadata-first ApplyDataVaultMetadata APIs or the legacy DCoding.Data.DVault.Modeling builders.
- Hub-name overrides, PIT or bridge work, and same-hub or recursive link participant alias support.

Open questions
- none

Follow-up questions
- If a later release needs fluent link-parent satellites, should that land as a dedicated follow-up ticket instead of widening the bounded hub-parent v1 surface?
- After registry or model-first work lands, does the team want an explicit code-first hub-name override, or should metadata-first remain the escape hatch for non-CLR logical hub names?
- If consumers need recursive or same-hub self-links, should a future fluent expansion add participant-role or alias support instead of relying only on repeated Participant<TEntity>() calls?

Risks
- If future implementation broadens selector parsing beyond direct readable scalar members without updating the contract and parity coverage, deterministic ordering and validation behavior can drift.
- If fluent projection bypasses DataVaultMetadataModel or redefines naming and key rules locally, schema parity with the metadata-first translator can regress.
- If later work quietly widens the surface to link-parent satellites or same-hub recursive links without a dedicated contract update, participant naming and column-shape assumptions can become ambiguous.
- If a consumer immediately needs non-CLR hub names, the v1 default-to-CLR-type-name stance forces a temporary fallback to metadata-first declarations.

Split recommendations
- No new split is required; keep the current live parentOf structure with the completed design contract ticket 06F0ME976PM5455JK04S6GPNNW and implementation children 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G.
- If release scope later expands to hub-name overrides, link-parent satellites, or same-hub or recursive link roles, materialize those as dedicated follow-up tickets rather than reopening this bounded story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment