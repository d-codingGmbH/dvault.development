[gicket-bot] PO refinement contract

Summary
- Refinement confirms this is a bounded additive story: add registry-backed PIT maintenance over the existing explicit hub-parent PIT maintenance pipeline, keep future link-parent and multi-active PIT work in the already split child stories, and make no planning or ticket writes in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The authoritative baseline is the existing explicit `IDataVaultPitMaintenanceService`; registry-backed support must resolve one `DataVaultPitMetadata` from `UseDataVaultMetadata()` and then delegate into `DataVaultPitRebuildRequest` or `DataVaultPitParentMaintenanceRequest` rather than introducing a second maintenance engine.
- `DataVaultMetadataRegistry` already supports exact PIT lookup by logical name and exact CLR type through `TryGetPit(string, ...)`, `TryGetPit(Type, ...)`, and `DataVaultMetadataClrMapping.Pit(...)`; this story should reuse that exact-match behavior and must not add fuzzy matching or first-match fallback.
- The incoming `blocks` relation from done task `06F5Q90718D21DN1N1Q2AP7YEM` is historical only and is not a live blocker for this ticket.
- No bounded child-ticket, relation, description, attachment, or planning-document writes were applied during this refinement run.

Scope In
- Add additive registry-backed PIT maintenance request surface(s) for callers that configure an authoritative registry through `UseDataVaultMetadata()`.
- Support exact PIT resolution by logical name and by exact CLR mapping registered through `DataVaultMetadataClrMapping.Pit(...)`.
- Delegate registry-backed rebuild and parent-maintenance calls to the existing explicit PIT maintenance pipeline so current validation, no-op handling, and row-generation semantics stay unchanged.
- Add tests, public API snapshot updates, and current-surface documentation updates required to expose the new registry-backed maintenance path.

Scope Out
- Link-parent PIT maintenance or link-parent PIT read behavior.
- Multi-active PIT maintenance semantics or driving-key PIT row rules.
- Automatic, background, scheduled, or `SaveChanges`-triggered PIT maintenance.
- Provider-specific PIT maintenance optimization or any non-registry metadata resolution heuristics.

Open questions
- none

Follow-up questions
- After this lands, should a separate registry-backed PIT as-of read request surface be planned, or should PIT reads remain explicit-metadata-only for now?

Risks
- The current README and production-adoption guidance explicitly say registry-backed PIT maintenance is out of scope; leaving those statements unchanged would create public contract drift after implementation.
- Because downstream link-parent and multi-active PIT stories are already split out, accidentally broadening validation or row-generation semantics in this story would blur ticket boundaries and risk regressions in the existing hub-parent baseline.
- This ticket currently blocks `06F5Q90SX5AQ07M4PQKDR4BZD8` and `06F5Q9102970H1VQN16QWRGQX0`, so incomplete registry error handling or missing tests would delay both follow-on PIT stories.

Split recommendations
- No additional split is recommended. The work is already bounded to additive registry resolution over the existing PIT maintenance engine, and the larger link-parent and multi-active PIT expansions are already split into `06F5Q90SX5AQ07M4PQKDR4BZD8` and `06F5Q9102970H1VQN16QWRGQX0`.

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