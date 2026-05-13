<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the existing provider-neutral save-strategy dispatch baseline and narrowed it to defining the bulk-capable contract plus fallback/selection tests in the core package.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 default is the documented core-owned provider-neutral dispatcher: provider packages may register strategies, while the core service remains provider-name agnostic and falls back to the built-in provider-neutral writer.
- Bulk strategy dispatch should use the same ordering rule already documented for save strategies: highest Priority wins, and dependency-injection registration order is preserved when priorities tie.
- The request batch passed to a strategy is ordered and must preserve caller-visible save ordering semantics, including existing hub/link saved-record ordering and RowsWritten behavior.
- Diagnostics only need to make the selected path observable for tests and operators; they do not need a new provider-specific implementation or benchmark expansion for this ticket.

### Scope In
- Define provider-neutral bulk/save strategy contracts and options in DCoding.Data.DVault without adding provider package dependencies.
- Route explicit single-save and ordered bulk-save requests through the strategy dispatcher before falling back to the provider-neutral writer.
- Add deterministic tests for no registered strategy, registered-but-unsupported strategy, and selected compatible strategy behavior.
- Verify strategy selection respects descending Priority and stable DI registration order for equal priorities where the contract exposes priority.
- Add or update concise public XML docs and architecture-facing documentation so provider packages know how to implement and register a strategy.
- Expose diagnostics or logging sufficient to distinguish provider-neutral fallback, unsupported strategy rejection, and selected strategy execution.

### Scope Out
- Implementing real provider-specific bulk SQL for SQLite, PostgreSQL, SQL Server, MySQL, or Oracle.
- Changing the public explicit save boundary or introducing SaveChanges interceptor behavior.
- Changing provider capability profile naming, auto-registration posture, or release-positioning decisions outside this contract.
- Adding broad benchmark coverage unless a small deterministic smoke check falls out naturally from the implementation.
- Creating subtickets or expanding this task into provider-specific optimization work.

## Acceptance Criteria
- Core exposes a provider-neutral strategy contract and context that can represent an ordered save/bulk request batch, the current DbContext, the stable hash service, and the stable hash normalizer.
- When no strategy is registered, explicit save and ordered bulk-save behavior falls back to the existing provider-neutral writer and preserves existing save results.
- When registered strategies decline the current context or batch, the dispatcher falls back to the provider-neutral writer without provider-name branching.
- When a compatible strategy is registered, the dispatcher selects it according to documented priority and registration-order rules and returns its result.
- Tests cover no-strategy fallback, unsupported-strategy fallback, selected-strategy execution, and the non-regression path for existing save behavior.
- Diagnostics or logging make the selected path visible in a deterministic way that tests can assert without depending on a specific provider package.

## Definition of Done
- The new contract, context, and dispatcher behavior are implemented in the core package with nullable-safe public APIs and no new provider-specific dependencies.
- Provider-neutral fallback tests and selected-strategy tests are committed in the existing test layout and pass locally with the relevant DVault test project.
- Existing explicit save service tests continue to pass, demonstrating that baseline save semantics and result ordering did not regress.
- Public XML docs or existing architecture documentation describe how provider packages should implement, prioritize, and register strategies.
- Diagnostics coverage proves operators can tell whether fallback or a selected strategy handled a request.

## Implementation Notes
- Use the existing explicit IDataVaultSaveService boundary and provider-neutral fallback writer as the baseline rather than adding provider-name conditionals.
- Keep DCoding.Data.DVault as the owner of contracts, dispatch, and tests using fake strategies; provider packages should remain consumers of the contract.
- Prefer small fake/test strategies to prove selected, unsupported, and priority behavior instead of adding provider-specific SQL.
- Preserve deterministic ordering of requests and saved records; fallback should continue to report RowsWritten according to inserted rows only.
- Keep diagnostics request-bound, matching the v0.6 note that validation-only analysis need not evaluate save-strategy status.

## Open Questions
- none

## Follow-Up Questions
- Should future provider packages expose richer bulk capability declarations beyond CanSave/Priority once real provider-specific SQL implementations need finer-grained shape matching?
- Should benchmark coverage be expanded later for provider-specific bulk strategies after at least one real optimized provider implementation exists?

## Risks
- A contract that is too narrow could force provider packages to add parallel strategy APIs later; the context should carry enough ordered request and hashing information for known provider optimization paths.
- Diagnostics that are only log-text based may be brittle in tests; prefer a stable observable diagnostic surface already used by the project if one exists.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Add the provider bulk strategy contract and verify fallback behavior.

## Scope In

- Introduce provider-neutral strategy interfaces/options.
- Add tests for no strategy, unsupported strategy, and selected strategy.
- Ensure diagnostics/logging make the selected path visible.

## Scope Out

- No provider-specific bulk implementation unless needed as a fake/test strategy.
- No benchmark expansion unless cheap and deterministic.

## Acceptance Criteria

- Fallback tests pass.
- Contract is documented enough for provider packages.
- Existing save strategy behavior does not regress.

## Implementation Notes

- Keep core package dependency-free.

## Open Questions

- none