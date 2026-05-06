[gicket-bot] PO-critic review contract

Summary
- The bridge mapping ticket is directionally scoped, but it is not ready for developer handoff because its upstream bridge-metadata contract is still unresolved and the current repo exposes no bridge source surface to implement against.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/description.md explicitly says ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 is the source of truth for which bridge metadata shapes are valid.
- A repository search for bridge terms across src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests returned no matches, so the current source/test baseline has no persisted bridge implementation surface yet.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs only models Hubs, Links, and Satellites, and src/DCoding.Data.DVault/Modeling/DataVaultModel.cs defines DataVaultTableKind as Hub, Link, and Satellite only.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs CreateEntities(...) iterates only metadataModel.Hubs, metadataModel.Links, and metadataModel.Satellites.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs and tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs only assert hub/link/satellite outputs, and the unit test helper AssertNoRelationships() still requires no foreign keys, navigations, or skip navigations.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs defines DataVaultLogicalPropertyKind only as HashKey, HashDiff, LoadTimestamp, RecordSource, ParticipantReference, BusinessKey, and PayloadText, and src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs is closed to HashKey, HashDiff, LoadTimestamp, and RecordSource.
- src/DCoding.Data.DVault/Modeling/IDataVaultNamingPolicy.cs and src/DCoding.Data.DVault/Modeling/DefaultDataVaultNamingPolicy.cs expose only hub/link/satellite naming contexts plus BusinessKey, Relationship, and SatelliteParent index kinds; there is no bridge naming context.
- The .gicket/relations entries for 06EZ0NV7KG94MTMNXMGVRYVW9C and 06EZ0NV0Y81AE1Z1Q3223TX2S4 show only parentOf links from story 06EZ0NTV4SVAKV98C418T8A3CC; there is no persisted blocks or dependency relation between the metadata child and the mapping child.
- git show --stat --oneline 5c8d2bad42d0 -- on branch ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m changed only .gicket ticket artifacts for the PO to PO-critic handoff, with no src or tests changes.

Blocking findings
- This ticket delegates bridge-shape validity to sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4, but that sibling is still needs-po and unrefined. Because the repo has no bridge metadata or public API surface today, developers do not yet have a stable input contract to implement this mapping against.
- The sequencing dependency between metadata definition and mapping implementation is not persisted. With only parentOf relations in .gicket/relations, the current ticket set can be misread as independently actionable even though this ticket says the metadata sibling is the source of truth.
- The handoff still lacks concrete worked examples for the supported many-to-many and hierarchy bridge shapes, especially the exact projected table, column, primary-key, index, and translator-failure expectations needed for deterministic implementation and tests.

Required PO actions
- Refine ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 to a durable ready-for-dev contract first, including the authoritative bridge metadata shapes, validation ownership, and any required public API additions.
- Persist sequencing between the sibling tickets, for example by making 06EZ0NV0Y81AE1Z1Q3223TX2S4 block 06EZ0NV7KG94MTMNXMGVRYVW9C or by stating the same dependency unambiguously in the ticket state/comment trail.
- Add or reference concrete examples for the supported baseline many-to-many and hierarchy shapes, including expected generated columns, primary key and index layout, annotations, and which unsupported cases belong to translator-time failure versus metadata validation.

Open issues ledger
- critic-item-1 [required-po-action] Refine ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 to a durable ready-for-dev contract first, including the authoritative bridge metadata shapes, validation ownership, and any required public API additions.
- critic-item-2 [required-po-action] Persist sequencing between the sibling tickets, for example by making 06EZ0NV0Y81AE1Z1Q3223TX2S4 block 06EZ0NV7KG94MTMNXMGVRYVW9C or by stating the same dependency unambiguously in the ticket state/comment trail.
- critic-item-3 [required-po-action] Add or reference concrete examples for the supported baseline many-to-many and hierarchy shapes, including expected generated columns, primary key and index layout, annotations, and which unsupported cases belong to translator-time failure versus metadata validation.
- critic-item-4 [blocking-finding] This ticket delegates bridge-shape validity to sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4, but that sibling is still needs-po and unrefined. Because the repo has no bridge metadata or public API surface today, developers do not yet have a stable input contract to implement this mapping against.
- critic-item-5 [blocking-finding] The sequencing dependency between metadata definition and mapping implementation is not persisted. With only parentOf relations in .gicket/relations, the current ticket set can be misread as independently actionable even though this ticket says the metadata sibling is the source of truth.
- critic-item-6 [blocking-finding] The handoff still lacks concrete worked examples for the supported many-to-many and hierarchy bridge shapes, especially the exact projected table, column, primary-key, index, and translator-failure expectations needed for deterministic implementation and tests.

Missing examples / edge cases
- A concrete many-to-many bridge metadata example and its exact ApplyDataVaultMetadata output.
- A concrete hierarchy traversal bridge example, including depth or closure assumptions and the expected load-timestamp or effectivity treatment.
- At least one example of metadata that is valid overall but still translator-unsupported, so the boundary between sibling metadata validation and this translator ticket is testable.
- Long produced names that trigger MySQL identifier-length hashing or Oracle primary-key-covered index suppression behavior.

Risky assumptions
- Bridge support can be added with only minimal public API expansion even though the current public API snapshot and modeling types contain no bridge surface.
- The existing shared-type, no-navigation translator posture is sufficient for both supported bridge shapes without leaking new EF relationship semantics.
- The current load-timestamp contract is enough for the baseline hierarchy case and does not hide a new effectivity or window concept.
- ParentOf-only ticket structure is enough for execution sequencing.

AC / test suggestions
- Persist one canonical expected schema example per supported bridge shape in unit and SQLite tests: table name, column order, produced-name annotations, primary key, indexes, and provider annotations.
- Add deterministic failure tests that separate metadata-validation failures from translator not-supported failures.
- Add regression assertions that existing hub, link, and satellite projections remain unchanged.
- Exercise long-name and provider-profile edge cases because bridge-produced names are likely to be longer than current hub/link/satellite baselines.

Implementation watchouts
- Any bridge addition likely touches public API snapshot discipline through DataVaultMetadataModel, DataVaultTableKind, naming contexts, or other public modeling types.
- Current translator tests explicitly require no foreign keys, navigations, or skip navigations for generated vault entities.
- Current provider-capability and technical-column enums are closed; avoid inventing bridge-effectivity-specific roles in this ticket unless the upstream metadata contract explicitly calls for them.
- Default naming and index families currently stop at BusinessKey, Relationship, and SatelliteParent, so bridge naming rules must be specified before implementation.

Non-blocking notes
- This ticket's own persisted contract has Open Questions set to none, so the blocker is upstream dependency clarity rather than an unresolved question left in this description.
- The existing story split into metadata, mapping, and documentation tickets still looks reasonable; the issue is sequencing and missing upstream contract, not immediate oversplitting.
- The latest PO handoff artifacts are consistent with a planning-only pass; commit 5c8d2bad42d0 changed only .gicket ticket files.

Split recommendations
- Do not create another child ticket yet; keep the current split.
- Add an explicit dependency or sequencing relation from metadata ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 to mapping ticket 06EZ0NV7KG94MTMNXMGVRYVW9C.
- Leave documentation work in 06EZ0NVE88WW9PMM04NVAZHRG0 once metadata and mapping contracts are stable.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment