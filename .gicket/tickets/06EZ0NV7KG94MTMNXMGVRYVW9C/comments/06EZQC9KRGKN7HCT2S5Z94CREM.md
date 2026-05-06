[gicket-bot] PO refinement contract

Summary
- Repository and ticket evidence support a bounded bridge-mapping contract: the existing bridge split is sufficient, the current translator still projects only hub, link, and satellite shared-type EF metadata, and no new child tickets, relation writes, attachments, or planning documents were needed in this refinement pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This child ticket stays under parent bridge story 06EZ0NTV4SVAKV98C418T8A3CC and remains the EF-generation slice beside metadata ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 and documentation ticket 06EZ0NVE88WW9PMM04NVAZHRG0; the current split already matches the visible work breakdown.
- Current repository evidence shows DataVaultEfMetadataTranslator and its unit and SQLite schema tests project only hub, link, and satellite shapes into EF shared-type entities, so bridge work here is new provider-neutral translation work rather than a save-service, provider-specific, or runtime-loading change.
- The current core technical metadata baseline is closed to HashKey, HashDiff, LoadTimestamp, and RecordSource, and current provider logical property kinds are closed to hash, timestamp, record source, participant-reference, business-key, and payload text mappings. This ticket should not invent separate effectivity or window timestamp families as part of the baseline bridge mapping.
- Current translation tests assert no EF foreign keys, navigations, or skip navigations for generated vault entities. Baseline bridge mapping should preserve that shared-type column-and-annotation posture unless a later ticket explicitly broadens the EF relationship model.
- No persistent planning artifacts were materialized during this pass because the live parent relation and existing child split already cover the needed planning boundary.

Scope In
- Translate validated baseline bridge metadata into provider-neutral EF shared-type entity metadata for the supported v0.5 bridge scenarios: many-to-many traversal over an existing link and the baseline hierarchy traversal shape already bounded by the bridge parent and metadata sibling tickets.
- Project deterministic bridge table names, column names, primary keys, indexes, annotations, and provider-profile storage metadata through the same ApplyDataVaultMetadata translation path used today for hubs, links, and satellites.
- Add unit coverage and local SQLite schema coverage that verify produced bridge names, column order, key and index layout, provider annotations, and deterministic repeatability for each supported baseline bridge shape.
- Fail unsupported bridge shapes that reach the translation layer with clear deterministic diagnostics when they are outside the bounded v0.5 bridge translator scope.

Scope Out
- Defining the bridge metadata API surface, missing-reference validation, cycle rules, or ambiguous-relationship rules already owned by ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4.
- Writing bridge documentation, examples, or usage guidance already owned by ticket 06EZ0NVE88WW9PMM04NVAZHRG0.
- Changing IDataVaultSaveService, DataVaultSaveRequest, runtime loading behavior, migrations, schema-refresh automation, or provider-specific SQL and DDL behavior.
- Broad bridge expansions such as effectivity windows, recursive closure maintenance, auto-populated bridge refresh strategies, consumer-specific query models, or provider-specific optimization depth beyond the provider-neutral EF mapping baseline.

Open questions
- none

Follow-up questions
- After the baseline bridge translator lands, should a later ticket define richer effectivity-window or closure-maintenance bridge variants as separate capability work rather than expanding this baseline ticket?
- If downstream consumers later need actual EF navigation or foreign-key behavior for bridges, should that be a new capability ticket instead of extending the current shared-type provider-neutral bridge projection?
- Once metadata and mapping tickets land, should the documentation child add explicit dependency relations for workflow sequencing, or is the existing shared parent split sufficient?

Risks
- If this ticket starts redefining bridge metadata semantics or validation rules, it will overlap with 06EZ0NV0Y81AE1Z1Q3223TX2S4 and create avoidable API churn between sibling tickets.
- If bridge mapping introduces new technical timestamp families or provider logical kinds prematurely, provider capability profiles and public API snapshots can expand beyond what the current repository evidence justifies.
- If the implementation adds EF relationships or navigations instead of preserving the current column-and-annotation shared-type posture, existing translator assumptions and tests may regress even if bridge schema generation appears to work.

Split recommendations
- No additional immediate split is recommended. The existing parent bridge story plus metadata child 06EZ0NV0Y81AE1Z1Q3223TX2S4, mapping child 06EZ0NV7KG94MTMNXMGVRYVW9C, and documentation child 06EZ0NVE88WW9PMM04NVAZHRG0 already provide the right bounded decomposition.
- If future bridge work needs effectivity windows, closure-maintenance semantics, or consumer-specific query optimization, create separate follow-up tickets rather than broadening this provider-neutral EF mapping baseline ticket.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment