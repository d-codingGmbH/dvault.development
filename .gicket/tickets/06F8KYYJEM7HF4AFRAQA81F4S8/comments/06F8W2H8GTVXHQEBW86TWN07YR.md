[gicket-bot] PO refinement contract

Summary
- Refined this epic to the already-bounded v0.27.0 release contract: `DMV1912` through `DMV1914` are the new EF lifecycle analyzer guardrails, the safe non-diagnostic lanes are explicit, and no blocking PO clarification remains.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This epic advances the v0.27.0 analyzer/documentation baseline by adding lifecycle diagnostics `DMV1912`, `DMV1913`, and `DMV1914`; `DMV1910` and `DMV1911` remain carried-forward EF misuse context, not fresh scope.
- The lifecycle slice is analyzer-only and source-visible. It must not add runtime guards, runtime behavior changes, compiled-model generation, or whole-application/cross-assembly inference.
- Safe non-diagnostic baselines are already fixed in repo docs: registry-backed `UseDataVaultMetadata(...)`, fixed-shape `ApplyDataVaultMetadata(...)` with visible cache-key coverage, fixed-shape `UseModel(runtimeModel)`, stable direct compiled queries, and options-only fixed-shape `AddDbContextPool<TContext>(...)`.
- Ambiguous, helper-expanded, pooled-factory, generated-artifact-inspection, provider-SQL-specific, and cross-assembly cases are explicit skip/non-goal territory for this ticket.

Scope In
- Define and deliver `DMV1912` for source-visible caller-owned DVault model-shape variation whose visible `IModelCacheKeyFactory` path omits the same discriminator.
- Define and deliver `DMV1913` for direct `UseModel(...)` compiled-model selection on a visibly variable-shape DVault context.
- Define and deliver `DMV1914` for direct `AddDbContextPool<TContext>(...)` registration of a visibly variable-shape DVault context.
- Document one consistent v0.27.0 lifecycle guardrail story across release notes, README/adoption guidance, analyzer package guidance, and the compiled-compatibility architecture note.
- Back the boundary with analyzer tests and SQLite compiled-compatibility evidence for fixed-shape runtime-model and compiled-query support.

Scope Out
- Any runtime guard, runtime behavior change, or save/read pipeline change.
- Generated compiled-model artifacts, custom `dotnet ef` tooling, or design-time service generation owned by DVault.
- Whole-application DI inference, cross-assembly inference, arbitrary helper expansion, or generated compiled-model artifact inspection.
- Provider-specific lifecycle guarantees, provider-specific SQL validation, or pooled-factory diagnostics.
- Benchmark reruns, package publication, release approval, or package-publication automation.

Open questions
- none

Follow-up questions
- Should a later ticket extend lifecycle diagnostics beyond direct `AddDbContextPool<TContext>(...)` to pooled factories or other pooling entrypoints?
- Should a later ticket add separate guardrails for helper-expanded or cross-assembly lifecycle patterns that this analyzer slice intentionally skips?
- Does the product want future runtime enforcement or provider-specific lifecycle guarantees, or should those remain permanently outside the analyzer-only posture?

Risks
- Because the analyzer intentionally skips ambiguous and indirect code paths, some real lifecycle misuse can remain undiagnosed until a future ticket broadens the contract.
- The safe pooled and compiled-model baselines still depend on consumer-owned model-cache-key discipline when model shape varies.
- This release note is a documentation baseline only; final package publication still depends on a separate approval record outside this ticket.

Split recommendations
- If delivery breadth re-expands, keep this epic limited to `DMV1912` through `DMV1914` plus bounded docs/tests, and move pooled-factory, helper-expansion, cross-assembly, or runtime-guard ideas into separate follow-on tickets.

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