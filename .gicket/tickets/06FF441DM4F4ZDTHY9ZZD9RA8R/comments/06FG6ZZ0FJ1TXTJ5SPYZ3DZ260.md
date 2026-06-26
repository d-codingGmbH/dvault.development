[gicket-bot] PO refinement contract

Summary
- Upstream contract ticket 06FF440F02AFQNQ0A3XNA2ZS3W is done and deferred dependent child key modeling, so this ticket refines to a no-work closure against the current finite DVault baseline rather than a prototype implementation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository baseline already keeps dependent child key modeling outside the public DVault claim set in docs/model-first-governance.md, docs/production-adoption-checklist.md, and docs/releases/v0.13.0.md.
- The completed contract ticket 06FF440F02AFQNQ0A3XNA2ZS3W is authoritative for the defer-now decision and does not reopen this feature for implementation.
- The visible metadata surface remains bounded to existing hub, link, satellite, PIT or point-in-time, and bridge families; src/DCoding.Data.DVault/Modeling/DataVaultTableKind.cs and DataVaultMetadataModel.cs expose no dependent-child concept.
- git grep over src and tests returned no dependent-child matches, so the current code and test surface does not claim dependent child support.

Scope In
- Refine this ticket as the no-work follow-on to the upstream defer-now contract.
- Record that the accepted baseline remains existing hub, link, satellite, PIT or point-in-time, and bridge concepts plus already documented repeated same-hub roles, link-parent satellites, and multi-active driving keys where those shapes are already supported.
- State that unsupported dependent-child requests stay on the existing validation or unsupported-capability path, including DMV1501 for out-of-baseline model-first capability requests.

Scope Out
- Adding a dependent-child metadata concept, parent-reference kind, fluent builder verb, dvault.model.v1 token or section, runtime mapper contract, support-bundle shape, or save/read API.
- Schema projection, diagnostics, tests, docs, or migrations that would prototype dependent child key support.
- Reopening repeated same-hub, link-parent satellite, multi-active driving-key, PIT, or bridge contracts beyond using them as the bounded current baseline.

Open questions
- none

Follow-up questions
- If dependent child support is reconsidered later, should it become a first-class metadata concept or a narrowly bounded extension over existing link and satellite semantics?
- Should a later cleanup pass explicitly align or remove stale blocking relations that still reflect the superseded if-accepted routing for this ticket?
- Does the release-documentation stream still need an explicit note for this deferred feature beyond the existing limitations text already present in docs/releases/v0.13.0.md and docs/production-adoption-checklist.md?

Risks
- The current ticket title and description still read like an implementation ticket, so without the refined contract downstream roles could misread the work as approved prototype scope.
- Stale blocks relations still point into and out of this ticket in repository state, which can preserve an outdated impression of pending implementation even though the upstream contract deferred the feature.
- A future developer could overread repeated-role, link-parent-satellite, or multi-active support as precedent for dependent-child parity unless the no-work boundary stays explicit.

Split recommendations
- If product later reopens dependent child key modeling, split it into separate follow-on tickets for contract and naming, metadata and model-first schema changes, Code-First API changes, runtime translation and migration behavior, and diagnostics or tooling parity rather than reopening this ticket as one large implementation bucket.

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