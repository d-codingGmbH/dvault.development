[gicket-bot] PO refinement contract

Summary
- Reframed the story as an explicit additive Code-First API expansion: metadata-first link-parent satellites already exist in the current branch, while Code-First currently lacks link satellite declaration and projection.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced the inferred existing-API claim with an explicit creation contract. Current branch evidence shows the public DataVaultCodeFirstLinkBuilder exists but currently exposes only Participant<TEntity>(); the refined story now explicitly requires adding Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null) and does not assume an existing link-satellite API or a pre-existing CLR type named State.
- critic-item-2: `answered` - The persisted contract no longer infers a public API that already exists. It now treats link-parent satellite declaration as the missing public surface to be added on the existing untyped link builder, while preserving the current Link(...) overloads and Participant<TEntity>() behavior unchanged.
- critic-item-3: `answered` - Separated existing branch evidence from the new work. Metadata-first already supports link-parent satellites end-to-end: DataVaultSatelliteMetadata accepts a hub or link parent, registry validation accepts link parents, EF translation produces SatCustomerOrderState for CustomerOrder/State, SQLite schema fixtures include that table, and importer/exporter already serialize satellite parent.kind = link. Code-First still excludes the feature today because DataVaultCodeFirstModelBuilder only materializes satellites from hub declarations and current Code-First schema parity omits SatCustomerOrderState. The refined story therefore scopes the work to extend LinkDeclaration, DataVaultCodeFirstLinkBuilder, and BuildMetadataModel() so Code-First can generate the existing metadata-first CustomerOrder/State baseline without assuming an existing CLR type named State.

Clarifications
- Current branch evidence is explicit: DataVaultCodeFirstLinkBuilder is public but currently exposes only Participant<TEntity>(); this story adds the missing link-satellite declaration API rather than relying on an existing one.
- Metadata-first link-parent satellites already exist in the branch through DataVaultSatelliteMetadata with DataVaultMetadataReference.Link(...), registry validation, EF translation, design-time import/export, and schema fixtures that produce SatCustomerOrderState.
- The CustomerOrder/State baseline names metadata and produced table shape only; the story must not assume a pre-existing CLR type named State. The new Code-First API uses a caller-supplied generic CLR type at Satellite<TSatellite>(...).
- The additive v1 shape stays on the existing untyped link builder: preserve Link(...) overloads and Participant<TEntity>() semantics, and add the CLR type only at the satellite declaration operation.
- Repository planning and public docs currently describe the implemented Code-First surface as hubs, hub-parent satellites, multi-active driving keys, and ordered hub links only; this ticket is the planned later expansion for link-parent satellites, and documentation authoring remains isolated in blocked task 06F2PGM9038RXVJH0RJFYEJEV0.

Scope In
- Add Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null) to the existing public DataVaultCodeFirstLinkBuilder while keeping current Link(...) overloads and Participant<TEntity>() semantics unchanged.
- Extend Code-First declaration storage so a link can own satellite declarations, and update DataVaultCodeFirstModelBuilder.BuildMetadataModel() to emit DataVaultSatelliteMetadata entries whose parent is DataVaultMetadataReference.Link(resolvedLinkName).
- Reuse existing DataVaultCodeFirstSatelliteBuilder<T> selector validation, duplicate logical-name rejection, payload ordering, and optional DrivingKey(...) behavior for link-parent satellites.
- Add representative regression coverage across public API snapshot, Code-First metadata translation, Code-First schema parity, and canonical JSON export so Code-First can reach the existing metadata-first CustomerOrder/State baseline including SatCustomerOrderState and parent.kind = link.

Scope Out
- Any assumption that a CLR type named State already exists in the product surface; test coverage may introduce a local sample CLR type as needed, but the ticket does not depend on a pre-existing domain type.
- Participant role or alias expansion, repeated same-hub participant support, recursive same-hub Code-First link modeling, same-as links, dependent child keys, effectivity satellites, PIT changes, or bridge changes.
- Typed save-helper expansion, source-generator or compile-time mapping additions, save interception work, or broader model-first artifact redesign; importer/exporter support for link parent.kind already exists.
- README and release-note authoring, which remains isolated in blocked documentation task 06F2PGM9038RXVJH0RJFYEJEV0.

Open questions
- none

Follow-up questions
- After this story lands, should compile-time or source-generator mapping parity for link-parent satellites be tracked as a separate ticket, since current public mapping attributes still focus on hubs, links, and hub-parent satellites?
- After documentation lands, do we want a separate quickstart or example ticket that demonstrates end-to-end save and read usage for a Code-First link-parent satellite?

Risks
- The main scope-creep risk is accidentally folding participant-role/alias support, recursive same-hub links, effectivity, same-as, or other advanced link shapes into this story because those capabilities are adjacent but not required for the bounded Code-First parity gap.
- If implementation adds the new API but misses Code-First metadata/schema/export regression coverage, the branch could still ship a partial feature that diverges from the existing metadata-first CustomerOrder/State baseline.
- Public documentation currently still describes the implemented Code-First surface as hub-parent-satellite-only; if task 06F2PGM9038RXVJH0RJFYEJEV0 does not land promptly after implementation, supported behavior and docs will diverge.

Split recommendations
- No additional split recommended. The product split already exists: this ticket covers the additive Code-First API and projection gap, task 06F2PGM9038RXVJH0RJFYEJEV0 covers documentation and release-note follow-through, and any future mapping/example work can remain separate follow-up tickets.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment