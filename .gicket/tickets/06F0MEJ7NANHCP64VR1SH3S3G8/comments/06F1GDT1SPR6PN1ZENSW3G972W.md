[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded provider-neutral read-strategy dispatch task that mirrors the existing save-strategy pattern while preserving current read APIs and fallback behavior.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 baseline should mirror the existing save-strategy dispatch model: core package owns provider-neutral strategy contracts and dispatcher behavior, provider packages may register concrete strategies later, and fallback remains in the core read service pipeline.
- The initial strategy surface applies to the currently implemented public read service shapes, especially latest/as-of satellite reads, and should not require user code to select a provider path.
- Diagnostics/explain output should report read-strategy evaluation when a read request is present, including selected strategy, declined strategies, and provider-neutral fallback usage.

Scope In
- Add provider-neutral read strategy contracts in the core DVault package for provider read optimization selection.
- Define deterministic strategy ordering using descending priority and dependency-injection registration order as the tie breaker, consistent with the documented save-strategy baseline.
- Route existing public read-service calls through provider-specific read strategy evaluation before using the provider-neutral fallback implementation.
- Preserve correct provider-neutral fallback reads when no strategy is registered or every registered strategy declines the current DbContext/request shape.
- Expose read-strategy selection and decline/fallback information through existing diagnostics/explain output surfaces.
- Add tests covering registration order, priority ordering, decline behavior, fallback behavior, and diagnostics/explain visibility.

Scope Out
- Implementing provider-specific optimized read SQL or provider package strategy implementations.
- Changing public save strategy APIs or save dispatch behavior.
- Changing the caller-facing read-service contract so application code has to select providers explicitly.
- Adding PIT-backed read APIs, bridge traversal helpers, or new read-model feature scope beyond already implemented public read methods.
- Changing persistence semantics, schema generation, or provider capability profile selection rules except where read diagnostics need to observe the current provider/profile context.

Open questions
- none

Follow-up questions
- Which provider packages should receive the first optimized read strategies after this hook exists?
- Should future provider-specific read optimizations cover PIT-backed reads, bridge reads, and multi-active satellite reads once those public surfaces are finalized?
- Should release documentation call out the hook as an extension point before any provider package ships an optimized read implementation?

Risks
- The dispatch layer touches shared read-service behavior, so regression coverage needs to prove fallback output remains stable for existing latest/as-of read scenarios.
- Diagnostics wording may drift from the save-strategy explain vocabulary unless implementation reuses the established conventions.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment