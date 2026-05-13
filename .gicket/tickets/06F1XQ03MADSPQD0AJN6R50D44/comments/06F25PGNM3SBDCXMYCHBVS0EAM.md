[gicket-bot] PO refinement contract

Summary
- Refined the story against the current DVault baseline: the repository already exposes a provider-neutral save-strategy SPI, ordered bulk dispatch, request-bound diagnostics, provider package implementations, and bulk benchmark evidence. Verified done child task `06F1XQ0DB1PRZXNXY7NKEZCS68`, verified live follow-on relations to `06F1XQ1VWEX0WPAXE78FHSWJ8G` and `06F1XQ25KK4VY4MYJSDG9V4BZM`, and performed no planning writes or relation changes in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ratify the existing core-owned `IDataVaultProviderSaveStrategy` plus `DataVaultProviderSaveStrategyContext` surface as the v1 optional provider bulk-insert extension point instead of introducing a second parallel SPI.
- `DefaultDataVaultSaveService` already routes both single `DataVaultSaveRequest` and ordered `DataVaultBulkSaveRequest` batches through the same provider-strategy dispatcher before falling back to the provider-neutral EF writer.
- Strategy selection is already deterministic in the visible source: higher `Priority` wins and equal priorities preserve dependency-injection registration order.
- The deterministic observability contract should be the existing request-bound diagnostics surface (`ProviderStrategySelected`, `ProviderNeutralFallback`, candidate ordering, and fallback causes), not a logging-only requirement.
- The repository already provides real proof-provider baselines through SQLite, PostgreSQL, SQL Server, MySQL, and Oracle save-strategy registrations plus documented benchmark rows; this story does not need a new core-owned no-op proof implementation.
- Child task `06F1XQ0DB1PRZXNXY7NKEZCS68` is done and already owns the core contract and fallback-test slice. Live `blocks` relations to `06F1XQ1VWEX0WPAXE78FHSWJ8G` and `06F1XQ25KK4VY4MYJSDG9V4BZM` were verified and left unchanged.
- Incoming parent epic `06F1XPX99KQRB09GRQG50Z75FM` was verified. Incoming `blocks` from done epic `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` is treated as historical/non-blocking because the source ticket is already done.

Scope In
- Use the existing core save-strategy SPI and ordered bulk-save dispatcher as the authoritative v1 bulk append extension point.
- Preserve provider-neutral fallback semantics and save-result ordering for both single-save and ordered bulk-save paths when no compatible provider strategy is selected.
- Keep strategy selection and fallback reasons observable through the existing request-bound diagnostics surfaces for single and bulk saves.
- Use the current benchmark README and existing provider-specific strategy families as the evidence boundary for bulk-path performance claims and comparison posture.
- Treat this story as the parent planning ticket over the completed core contract/test child task `06F1XQ0DB1PRZXNXY7NKEZCS68`.

Scope Out
- No second `IDataVaultProviderBulkInsertStrategy`-style API parallel to the existing save-strategy contract.
- No mandatory third-party bulk library in `DCoding.Data.DVault`.
- No provider-name branching in the core save service and no requirement that every provider expose optimized bulk behavior in the first slice.
- No destructive update or delete path and no SaveChanges-interceptor persistence model.
- No expansion of this story into the separate Testcontainers/helper tickets `06F1XQ1VWEX0WPAXE78FHSWJ8G` and `06F1XQ25KK4VY4MYJSDG9V4BZM`.

Open questions
- none

Follow-up questions
- If a future provider needs finer-grained capability advertisement than `CanSave` plus `Priority`, should that extension live on the existing save-strategy surface or on a later additive capability contract?
- After the current core/save-strategy baseline, should the existing Testcontainers/helper tickets `06F1XQ1VWEX0WPAXE78FHSWJ8G` and `06F1XQ25KK4VY4MYJSDG9V4BZM` become the preferred local proof path for optional-provider benchmark and integration validation?
- If future append-only optimization work needs a narrower contract than the general provider save-strategy SPI, what concrete provider constraint would justify that split instead of reusing the existing surface?

Risks
- Reopening the story around a brand-new parallel SPI would duplicate the already-visible core save-strategy surface and create avoidable provider-package guidance drift.
- Performance claims can drift if story text forgets that optional provider benchmark rows are configuration-dependent and that skipped rows are part of the documented evidence boundary.
- Live `blocks` relations to the Testcontainers/example tickets remain in place; if workflow intent changes, they need explicit relation cleanup rather than an implicit assumption.

Split recommendations
- No new split is required. Existing child task `06F1XQ0DB1PRZXNXY7NKEZCS68` already owns the core contract and fallback-test slice, and existing tickets `06F1XQ1VWEX0WPAXE78FHSWJ8G` and `06F1XQ25KK4VY4MYJSDG9V4BZM` already cover the separate container/example follow-up work.

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