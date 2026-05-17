[gicket-bot] PO refinement contract

Summary
- Restated the story against source-backed branch evidence and kept it as one bounded story; no child tickets, relation edits, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Resolved by restating the contract so missing Code-First link-satellite API and declaration storage are created explicitly on this branch: the current public link builder exposes only participant declaration, and the current model builder has no link-satellite declaration path.
- critic-item-2: `answered` - The contract no longer assumes an existing public link-satellite API or type. It treats `Satellite<TSatellite>(...)` on the existing link builder, plus the supporting link-satellite declaration storage and projection, as new additive work while preserving current `Link(...)` and `Participant<TEntity>()` behavior.
- critic-item-3: `answered` - Source-backed branch docs and code show that the current Code-First surface stops at hubs, hub-parent satellites, multi-active driving keys, and ordered hub links. This ticket is therefore defined as the additive Code-First expansion for link-parent satellites, not as activation of a hidden existing Code-First API.
- critic-item-4: `answered` - The visible types on this branch are enough to state the additive API precisely without inferring an existing method: `DataVaultCodeFirstLinkBuilder` already exists as the public non-generic link builder, and `DataVaultCodeFirstSatelliteBuilder<T>` already exists as the public generic payload/driving-key builder used by hub satellites. The new work is to add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` to the existing link builder and wire it through link declaration storage and projection.
- critic-item-5: `answered` - The contract explicitly removes any dependency on a product CLR type named `State`. Any regression baseline may use a local caller-owned sample satellite CLR type; the story only requires that the new API accept a caller-supplied generic CLR type for the link-parent satellite declaration.

Clarifications
- Current branch evidence is explicit: public `DataVaultCodeFirstLinkBuilder` currently has only `Participant<TEntity>()`; no link-parent satellite declaration method exists today.
- The existing public reuse point for satellite member selection is `DataVaultCodeFirstSatelliteBuilder<T>`, already used by `DataVaultCodeFirstHubBuilder<TEntity>.Satellite(...)` for `Payload(...)` and optional `DrivingKey(...)` behavior.
- Current `DataVaultCodeFirstModelBuilder.LinkDeclaration` stores only participant CLR types, and current `BuildMetadataModel()` materializes satellites only from hub declarations; this story explicitly owns adding link-satellite declaration storage and projection.
- Repository planning and public docs currently bound the implemented Code-First surface to hubs, hub-parent satellites, multi-active driving keys, and ordered hub links; link-parent satellites are the bounded next additive expansion.
- Any sample name like `State` is test-only and caller-owned; the ticket does not require a pre-existing product CLR type or repository fixture with that CLR type name.
- No child tickets, relation updates, or planning documents were materialized in this run; current evidence keeps the work in one story, and documentation follow-through remains separated on ticket `06F2PGM9038RXVJH0RJFYEJEV0`.

Scope In
- Add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` on the existing public `DataVaultCodeFirstLinkBuilder` while leaving current `Link(...)` overloads and `Participant<TEntity>()` semantics additive and unchanged.
- Extend Code-First link declaration storage so a link can retain ordered participant declarations plus one or more satellite declarations.
- Update Code-First metadata projection so a link-parent satellite is emitted with link-parent semantics bound to the resolved link name and aligned with the repository's documented `parent.kind = link` model contract.
- Reuse existing `DataVaultCodeFirstSatelliteBuilder<T>` behavior for `Payload(...)`, duplicate-member rejection, declaration ordering, and optional `DrivingKey(...)` on link-parent satellites.

Scope Out
- Any assumption that a product CLR type named `State` already exists; tests may introduce a local sample type if useful.
- Participant role or alias expansion, repeated same-hub participant support, recursive same-hub link modeling, same-as links, effectivity satellites, PIT changes, or bridge changes.
- Typed save-helper expansion, source-generator or compile-time mapping additions, save interception work, or broader model-first artifact redesign.
- README and release-note authoring, which remains on ticket `06F2PGM9038RXVJH0RJFYEJEV0`.

Open questions
- none

Follow-up questions
- After this story lands, should source-generator or compile-time mapping parity for link-parent satellites be tracked as a separate ticket?
- After ticket `06F2PGM9038RXVJH0RJFYEJEV0` lands, should a runnable example or quickstart cover end-to-end declaration, save, and read usage for a Code-First link-parent satellite?

Risks
- Scope creep into participant roles, aliases, or same-hub recursive link shapes is the main delivery risk because current Code-First link validation already marks those shapes as unsupported.
- Partial delivery is possible if the new API is added but link-satellite projection or downstream regression coverage stops before the documented `parent.kind = link` contract is exercised.
- Public docs remain hub-parent-focused until ticket `06F2PGM9038RXVJH0RJFYEJEV0` lands, so short-term documentation drift remains possible.

Split recommendations
- No further split is recommended from current evidence; keep this ticket focused on the additive Code-First link-parent satellite API and projection gap.
- Leave documentation follow-through on ticket `06F2PGM9038RXVJH0RJFYEJEV0`, and handle any future mapping or example work as separate follow-up tickets if needed.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment