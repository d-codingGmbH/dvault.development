<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the implementation story around DMV1912-DMV1914 using the completed lifecycle contract and the current analyzer/test baseline; no bounded planning writes or relation changes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence shows the EfCore analyzer slice currently implements only DMV1910 and DMV1911, so this story is the implementation lane for the next contiguous lifecycle diagnostics rather than a runtime or documentation change.
- Ticket context shows no recent human comments or closure-evidence amendments that reopen or narrow the lifecycle contract.
- Related-ticket evidence already separates lifecycle contract work in done ticket 06F8KZGC4NY41PRYB2RP00ZA1M from this implementation story and from sibling fixture/documentation stories 06F8KZGZND5ZCH147PVBRWXYN4 and 06F8KZHAB717MJJNAWWK7S0A5W, so no additional child-ticket split is justified from the visible scope.
- No bounded ticket write, relation update, attachment, or planning document was applied in this refinement run.

### Scope In
- Implement Roslyn diagnostics DMV1912, DMV1913, and DMV1914 inside the existing EfCore analyzer slice for source-visible DVault EF lifecycle misuse.
- Add the analyzer metadata, messaging, and bounded detection logic for missing visible model-cache discriminators, unsafe variable-shape UseModel(...) lanes, and unsafe AddDbContextPool<TContext>(...) lanes.
- Add focused analyzer unit coverage for the new diagnostic and non-diagnostic cases needed to prove the implementation without taking on the separate broad fixture-matrix story.

### Scope Out
- Whole-application DI inference, runtime tenant-state inference, generated compiled-model artifact inspection, or proof over opaque helper and service-registration code.
- Runtime behavior changes, SaveChanges guards, provider-specific validation, or changes to existing DMV1910 and DMV1911 behavior.
- Broad fixture expansion and documentation or public release-note updates, which stay with sibling tickets 06F8KZGZND5ZCH147PVBRWXYN4 and 06F8KZHAB717MJJNAWWK7S0A5W.

## Acceptance Criteria
- DMV1912 is implemented as a warning in the existing EfCore analyzer category and reports only when source-visible DVault model-shape variation depends on instance or selected metadata state and the visible model-cache-key path does not include that varying state.
- DMV1913 is implemented as a warning and reports only when source-visible UseModel(...) applies a compiled or runtime model to a DVault context with visibly variable realized model shape and the same visible source scope does not prove one fixed shape or the documented safe design-model-to-runtime-model lane.
- DMV1914 is implemented as a warning and reports only when source-visible AddDbContextPool<TContext>(...) is used for a DVault context whose realized model shape visibly varies beyond one fixed options-only shape.
- The implementation keeps UseDataVaultMetadata(...) registration paths, safe fixed-shape ApplyDataVaultMetadata(...) paths, documented read-only generated-table query patterns, safe compiled-query use, and visibly sufficient custom cache-key examples non-diagnostic.
- The implementation skips ambiguous cases instead of guessing, including helper-expanded registrations, cross-assembly inference, opaque custom IModelCacheKeyFactory logic, and runtime-only tenant or DI state.

## Definition of Done
- EfCoreMisuseDiagnosticCatalog exposes contiguous DMV1912 through DMV1914 descriptors with warning severity and remediation text aligned to the lifecycle contract.
- DataVaultEfCoreMisuseAnalyzer emits the new diagnostics only from direct source-visible evidence and preserves existing DMV1910 and DMV1911 behavior.
- Targeted analyzer tests cover at least one positive and one non-diagnostic safe case for each new rule, while the larger regression-fixture expansion remains in the sibling fixture story.
- The implementation leaves runtime packages and runtime behavior unchanged.

## Implementation Notes
- Follow the existing analyzer structure in DataVaultEfCoreMisuseAnalyzer.cs and EfCoreMisuseDiagnosticCatalog.cs so the lifecycle rules remain in the same EfCore slice as DMV1910 and DMV1911.
- Prefer symbol and operation analysis over speculative dataflow; report only when the varying state, lifecycle registration, and DVault model-selection path are all directly visible in source.
- Treat custom IModelCacheKeyFactory handling as sufficient only when the returned key shape visibly includes the relevant varying members; if that proof is indirect, skip.
- Do not make UseModel(...) or AddDbContextPool<TContext>(...) diagnostic by themselves; preserve the documented fixed-model compiled-compatibility baseline.
- Keep broad scenario-matrix fixtures and doc wording out of this ticket to avoid re-merging scope already split into sibling stories.
- No bounded writes were applied during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- After implementation lands, decide whether later backlog work should extend the same lifecycle analysis to AddPooledDbContextFactory<TContext> or other pooling entrypoints.
- After the analyzer wording stabilizes, decide whether the ticket description should mirror the accepted DMV1912-DMV1914 contract or continue to rely on the existing contract and documentation tickets.

## Risks
- If implementation expands beyond direct source-visible evidence, it is likely to create false positives against safe fixed-model compiled-model and registry-backed metadata lanes.
- If implementation remains intentionally high-confidence, some real unsafe caller-owned variable-shape cases will remain guidance-only and must be explained by the separate documentation lane.
- The boundary between targeted implementation tests here and the broader fixture story must stay explicit or the ticket will regrow into already-split test-surface work.

## Split Recommendations
- No further split is recommended; visible evidence already separates contract, implementation, fixtures, and documentation into dedicated tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement high-confidence Roslyn diagnostics for source-visible DVault metadata selection that is unsafe with pooled contexts, compiled models, or missing model-cache discriminators. The implementation must not infer arbitrary DI graphs or runtime tenant state.