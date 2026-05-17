[gicket-bot] PO refinement contract

Summary
- Restated the story against prompt-visible branch evidence, removed unsupported inferred builder/type claims, kept the work as one bounded Code-First story, and made no child-ticket, relation, or planning-document changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is rewritten so the only asserted current-branch API fact is source-backed: the visible public DataVaultCodeFirstLinkBuilder currently exposes Participant<TEntity>(). All earlier claims about existing satellite builder types or hub satellite reuse were removed or converted into explicit implementation scope if the code does not already provide them.
- critic-item-2: `answered` - The delivery contract no longer relies on a visible-existing public DataVaultCodeFirstSatelliteBuilder<T> or DataVaultCodeFirstHubBuilder<TEntity>.Satellite(...). Instead it requires an additive link-level Satellite<TSatellite>(...) capability whose concrete configuration type may be reused if verified in source or introduced explicitly within this story.
- critic-item-3: `answered` - The prior statement that the branch already reuses DataVaultCodeFirstSatelliteBuilder<T> from DataVaultCodeFirstHubBuilder<TEntity>.Satellite(...) is removed. The prompt-visible evidence does not show those types, so the contract now anchors only to the documented Satellite(...), Payload(...), and DrivingKey(...) verb pattern and leaves concrete type reuse to implementation after source verification during delivery.
- critic-item-4: `answered` - The contract keeps the additive requirement for a generic link-level Satellite<TSatellite>(string satelliteName, configure) entry point on DataVaultCodeFirstLinkBuilder, but it no longer hardcodes an unsupported configure-callback type name. The callback must support payload and optional driving-key declaration semantics while existing Link(...) overloads and Participant<TEntity>() behavior remain unchanged.
- critic-item-5: `answered` - The contract now treats any sample CLR type as caller-owned test code and does not name or require a product CLR type State. A test may introduce whatever local sample type is useful.
- critic-item-6: `answered` - The contract no longer requires source-unverified reuse of DataVaultCodeFirstSatelliteBuilder<T>. It states the behavioral goal instead: the link-level configuration surface must expose the same payload and optional driving-key semantics as the established Code-First satellite contract, whether that is achieved by verified reuse of existing infrastructure or by explicit new infrastructure added in this story.

Clarifications
- Current prompt-visible branch evidence is explicit: public DataVaultCodeFirstLinkBuilder currently exposes only Participant<TEntity>(), so link-parent satellite declaration is net-new additive API work.
- The established fluent satellite pattern in docs/plans/fluent-code-first-api-contract.md is a Satellite(...) entry point with nested Payload(...) and optional DrivingKey(...); this story extends that behavior to link-parent satellites without reopening unrelated link-shape work.
- The contract intentionally avoids assuming any already-existing public satellite builder type or hub helper beyond what is visible in the prompt-backed branch snapshot.
- No child tickets, relation edits, or planning documents were materialized in this run; previous tool result tc2 already shows documentation follow-through remains tracked separately by the existing blocks relation to ticket 06F2PGM9038RXVJH0RJFYEJEV0.

Scope In
- Add an additive generic link-level Satellite<TSatellite>(string satelliteName, configure) fluent entry point on DataVaultCodeFirstLinkBuilder so callers can declare payload members and optional driving keys for a link-parent satellite.
- Preserve current Link(...) overload behavior and Participant<TEntity>() declaration semantics for existing callers.
- Extend internal Code-First link declaration and metadata projection so ordered link participants and link-parent satellite declarations survive into the projected metadata model with the resolved link as the satellite parent.
- Add regression tests covering fluent link-satellite declaration, metadata projection, and at least one downstream artifact, schema, or projection surface.

Scope Out
- Assuming an existing product CLR type named State or any other repository-owned sample entity.
- Participant aliases or roles, repeated same-hub participant support, recursive same-hub link modeling, same-as links, effectivity satellites, PIT changes, or bridge changes.
- Save pipeline refactors, source-generator or compile-time mapping expansion, or README and release-note authoring already tracked on ticket 06F2PGM9038RXVJH0RJFYEJEV0.

Open questions
- none

Follow-up questions
- After this story lands, should source-generator or compile-time mapping parity for link-parent satellites be tracked as a separate ticket?
- After ticket 06F2PGM9038RXVJH0RJFYEJEV0 lands, should a runnable example or quickstart cover end-to-end declaration, save, and read usage for a Code-First link-parent satellite?

Risks
- Scope creep into participant aliases, roles, or same-hub recursive link shapes remains the main delivery risk because the current ticket stays bounded to link-parent satellite parity.
- The prompt-backed branch snapshot does not prove an existing reusable satellite builder or storage path, so implementation may need both new public API and new internal declaration plumbing inside the same story.
- Public documentation remains hub-parent-focused until ticket 06F2PGM9038RXVJH0RJFYEJEV0 lands, so short-term documentation drift is still possible.

Split recommendations
- No further split is recommended from current prompt-backed evidence; keep this story focused on the additive link-parent satellite declaration and projection gap.
- Keep documentation and release-note work on ticket 06F2PGM9038RXVJH0RJFYEJEV0, and raise any later generator, mapping, or example expansion as separate follow-up work if needed.

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