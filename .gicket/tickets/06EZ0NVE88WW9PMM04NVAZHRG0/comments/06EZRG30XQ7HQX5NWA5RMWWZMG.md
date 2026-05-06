[gicket-bot] PO refinement contract

Summary
- Refined the bridge-doc child so it stays blocked on parent story 06EZ0NTV4SVAKV98C418T8A3CC, points to that parent ticket contract as the authoritative bridge artifact, and requires a many-to-many traversal example only; no new relation, attachment, child ticket, or planning document was materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent story still carries needs-po, so this child contract now states explicitly that documentation work is blocked until parent story 06EZ0NTV4SVAKV98C418T8A3CC leaves needs-po and establishes the authoritative bridge surface in its persisted ticket contract.
- critic-item-2: `answered` - The exact authoritative bridge artifact for this child is the persisted parent ticket contract for story 06EZ0NTV4SVAKV98C418T8A3CC. The docs task must follow that parent contract for bridge metadata and table shape once it is refined, rather than inventing bridge APIs, types, annotations, or table names from architecture-only planning docs.
- critic-item-3: `answered` - The live sequencing signal already exists on the child ticket through blocked/dev and blocked/test labels, and this contract now ties those labels directly to the parent dependency. Together with the existing parentOf relation, that is the explicit signal that this child must not route to dev before the bridge surface exists.
- critic-item-4: `answered` - The single required minimal example is the many-to-many traversal case over existing hubs and links. Hierarchy-style traversal may be mentioned only as deferred or unsupported context on this ticket, not as the worked example, because hierarchy depth and business-rule semantics remain deferred assumptions.
- critic-item-5: `answered` - This blocking finding is now handled explicitly in the child contract: because parent story 06EZ0NTV4SVAKV98C418T8A3CC still has needs-po, this ticket is refined as blocked documentation scope and is not dev-ready until that parent contract becomes authoritative.
- critic-item-6: `answered` - The contract no longer asks a developer to guess bridge shapes. It points to the parent bridge story contract as the authoritative future artifact and keeps this docs task blocked until that artifact exists. Current source evidence still exposes only hub, link, and satellite projection surfaces, so no concrete bridge table, API, annotation, or type contract should be inferred yet.

Clarifications
- The authoritative bridge artifact for this child is that parent ticket contract, not the architecture-level deferred-capabilities decision record and not an inferred source API.
- The single required minimal example for this ticket is the many-to-many traversal case; hierarchy-style traversal is not the worked example on this child.

Scope In
- Document bridge tables as an opt-in v0.5 deferred capability layered on the existing hub, link, and satellite baseline.
- Provide one minimal many-to-many traversal example aligned with the authoritative parent bridge contract once that contract is refined.
- Reuse existing repository vocabulary around AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), IDataVaultSaveService, and deterministic naming conventions.
- State unsupported or deferred bridge behavior without implying hierarchy depth, recursive semantics, or provider-specific guarantees.

Scope Out
- Implementing bridge modeling, EF metadata projection, save behavior, validation, or tests.
- A hierarchy-style worked example or concrete hierarchy-depth semantics before the parent bridge contract defines them.
- Provider-specific DDL, migrations, optimization, or maintenance behavior.
- Runnable sample applications or broad README expansion beyond the bounded bridge documentation task.

Open questions
- none

Follow-up questions
- After the parent bridge contract is refined, should the root README add a short cross-link to the bridge documentation page?
- If hierarchy-style traversal later needs its own worked example, should that be a separate follow-up docs ticket instead of expanding this many-to-many-focused child?

Risks
- When parent story 06EZ0NTV4SVAKV98C418T8A3CC is refined, its final bridge naming or metadata shape may require one last sync pass on this child before dev handoff.
- If this child reaches dev before the parent contract becomes authoritative, the docs example would again rely on guesses rather than a ratified bridge surface.

Split recommendations
- No split recommended for the current child while it remains a bounded docs task blocked on the parent bridge contract.
- If future scope needs a concrete hierarchy-style walkthrough in addition to the many-to-many example, create a separate follow-up docs ticket after the parent bridge surface is defined.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment