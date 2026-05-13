[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing provider-neutral save-strategy dispatch baseline and narrowed it to defining the bulk-capable contract plus fallback/selection tests in the core package.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 default is the documented core-owned provider-neutral dispatcher: provider packages may register strategies, while the core service remains provider-name agnostic and falls back to the built-in provider-neutral writer.
- Bulk strategy dispatch should use the same ordering rule already documented for save strategies: highest Priority wins, and dependency-injection registration order is preserved when priorities tie.
- The request batch passed to a strategy is ordered and must preserve caller-visible save ordering semantics, including existing hub/link saved-record ordering and RowsWritten behavior.
- Diagnostics only need to make the selected path observable for tests and operators; they do not need a new provider-specific implementation or benchmark expansion for this ticket.

Scope In
- Define provider-neutral bulk/save strategy contracts and options in DCoding.Data.DVault without adding provider package dependencies.
- Route explicit single-save and ordered bulk-save requests through the strategy dispatcher before falling back to the provider-neutral writer.
- Add deterministic tests for no registered strategy, registered-but-unsupported strategy, and selected compatible strategy behavior.
- Verify strategy selection respects descending Priority and stable DI registration order for equal priorities where the contract exposes priority.
- Add or update concise public XML docs and architecture-facing documentation so provider packages know how to implement and register a strategy.
- Expose diagnostics or logging sufficient to distinguish provider-neutral fallback, unsupported strategy rejection, and selected strategy execution.

Scope Out
- Implementing real provider-specific bulk SQL for SQLite, PostgreSQL, SQL Server, MySQL, or Oracle.
- Changing the public explicit save boundary or introducing SaveChanges interceptor behavior.
- Changing provider capability profile naming, auto-registration posture, or release-positioning decisions outside this contract.
- Adding broad benchmark coverage unless a small deterministic smoke check falls out naturally from the implementation.
- Creating subtickets or expanding this task into provider-specific optimization work.

Open questions
- none

Follow-up questions
- Should future provider packages expose richer bulk capability declarations beyond CanSave/Priority once real provider-specific SQL implementations need finer-grained shape matching?
- Should benchmark coverage be expanded later for provider-specific bulk strategies after at least one real optimized provider implementation exists?

Risks
- A contract that is too narrow could force provider packages to add parallel strategy APIs later; the context should carry enough ordered request and hashing information for known provider optimization paths.
- Diagnostics that are only log-text based may be brittle in tests; prefer a stable observable diagnostic surface already used by the project if one exists.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment