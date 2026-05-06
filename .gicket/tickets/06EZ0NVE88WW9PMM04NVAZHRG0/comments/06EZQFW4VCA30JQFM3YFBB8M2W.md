[gicket-bot] PO refinement contract

Summary
- Refined the bridge documentation task against the parent bridge story and repository documentation baseline; no additional split or planning write is needed, and the ticket is ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the architectural frame: bridge tables are a v0.5 deferred, opt-in capability layered on the ordinary hub/link/satellite baseline rather than part of the default DVault path.
- This ticket remains the documentation child of bridge story `06EZ0NTV4SVAKV98C418T8A3CC`; the existing `parentOf` relation is correct, and no new child tickets, relation changes, attachments, or planning documents were materialized in this refinement.
- The required example scenario should be a minimal documentation example aligned to generated bridge metadata and existing deterministic naming patterns, not a new runnable sample application.

Scope In
- Document bridge-table use cases for bounded many-to-many traversal and hierarchy-style traversal over existing hub/link structures.
- Explain the v0.5 bridge baseline as opt-in extension work that preserves the current convention-first hub/link/satellite default path and SQLite-oriented local baseline.
- Provide one minimal example scenario that uses repository-established naming and generated-model terminology.
- Explicitly call out unsupported advanced traversal patterns and deferred bridge decisions.

Scope Out
- Implementing bridge modeling, EF metadata projection, validation rules, or bridge tests themselves.
- Provider-specific bridge optimization, DDL, migrations, or maintenance behavior.
- PIT tables, multi-active satellites, or advanced configuration hook design.
- Creating a runnable example project or broad README expansion beyond the bounded bridge documentation task.

Open questions
- none

Follow-up questions
- After bridge implementation lands, should the root README add a short cross-link to the bridge documentation page?
- If bridge support later grows provider-specific behavior or richer hierarchy semantics, should that become a separate follow-up documentation ticket instead of expanding this task?

Risks
- If the parent bridge implementation changes its generated shape late in development, the documentation example and unsupported-pattern notes will need a final consistency pass.
- Because bridge behavior is explicitly deferred and opt-in at v0.5, the documentation must avoid overstating hierarchy depth, maintenance semantics, or provider-specific guarantees that the implementation does not prove.

Split recommendations
- No split recommended; the current task is already a bounded documentation/example child under bridge story `06EZ0NTV4SVAKV98C418T8A3CC`.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment