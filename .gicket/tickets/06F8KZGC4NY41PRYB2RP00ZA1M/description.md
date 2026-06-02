<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the live ticket snapshot, bot-only comments, and persisted relations. Repository evidence shows model-cache, compiled-model, and DbContext pooling safety are still documentation-only, existing EF analyzer ids stop at DMV1911, and the current parent and blocking relations already match the intended workflow.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ticket 06F8KZGC4NY41PRYB2RP00ZA1M is already a child of epic 06F8KYYJEM7HF4AFRAQA81F4S8, and its existing blocks relation to implementation story 06F8KZGNRG5FY4WWCY3FAX2NS4 matches the intended execution order.
- Current repository guidance already fixes the safe baseline: UseDataVaultMetadata(...) supplies DVault-owned metadata-source isolation, while UseModel(...) and AddDbContextPool<TContext>(...) are only documented as safe for one fixed realized model shape.
- The current analyzer catalog already uses the EfCore slice for DMV1910 and DMV1911, so this contract should reserve contiguous ids DMV1912 through DMV1914 for the new lifecycle rules.
- No bounded ticket write, attachment, or planning-document materialization was applied in this run; the refined contract is returned inline.

### Scope In
- Define the v0.27 high-confidence analyzer contract for source-visible DVault EF lifecycle misuse around missing caller-owned model-cache discriminators, UseModel(...) compiled-model lanes, and AddDbContextPool<TContext>(...) fixed-model lanes.
- Name the diagnostic ids, warning intent, supported source patterns, explicit non-goals, and false-positive avoidance rules for that lifecycle slice.
- Anchor the contract to the existing safe baselines already proven in DataVaultMetadataRegistrationIntegrationTests, DataVaultCompiledCompatibilitySqliteTests, the README model-cache guidance, and docs/architecture/dvault-ef-compiled-compatibility.md.

### Scope Out
- Implementing the analyzer, fixture coverage, or documentation updates; those remain in sibling tickets 06F8KZGNRG5FY4WWCY3FAX2NS4, 06F8KZGZND5ZCH147PVBRWXYN4, and 06F8KZHAB717MJJNAWWK7S0A5W.
- Whole-application DI inference, provider-specific SQL validation, pooled registration discovery outside direct source visibility, or proof that an opaque custom IModelCacheKeyFactory captures every possible discriminator.
- Any runtime guard, runtime behavior change, compiled-model generator, or change to the existing DMV1910 and DMV1911 shared-type-table misuse rules.

## Acceptance Criteria
- The contract reserves DMV1912 for a missing caller-owned EF model-cache discriminator when a DbContext visibly varies DVault model shape from instance state or source-selected metadata and the visible cache-key path does not include that varying state.
- The contract reserves DMV1913 for unsafe compiled-model usage when source-visible UseModel(...) is applied to a DVault context whose realized model shape is visibly variable and the same source scope does not prove one fixed model shape or a matching design-model-to-runtime-model lane.
- The contract reserves DMV1914 for unsafe DbContext pooling when source-visible AddDbContextPool<TContext>(...) targets a DVault context whose model shape visibly varies beyond one fixed options-only shape.
- The contract states that UseDataVaultMetadata(), UseDataVaultMetadata(registry), and UseDataVaultMetadata(importResult) are the non-diagnostic built-in baseline for DVault-owned metadata-source isolation, and that direct ApplyDataVaultMetadata(...) is only non-diagnostic when the model shape is fixed or caller-owned discriminators are visibly accounted for.
- The contract states that the analyzer is high-confidence only: it reports only direct source-visible model-shape variation and direct source-visible lifecycle registrations, and skips cases that require helper expansion, cross-assembly inference, generated compiled-model artifact inspection, or ambiguous dataflow.
- The contract explicitly preserves existing non-diagnostic lanes for read-only compiled queries, AsNoTracking() generated-table reads, safe registry-backed metadata registration, safe custom-cache-key examples, and the documented SQLite compiled-compatibility proof.

## Definition of Done
- The authoritative contract names DMV1912 through DMV1914, their intent, and their bounded supported-pattern rules.
- The contract enumerates supported patterns, false-positive avoidance rules, and unsupported inference boundaries clearly enough that the implementation and fixture sibling tickets can proceed without reopening naming or scope questions.
- The contract explicitly preserves the safe baselines already demonstrated by DataVaultMetadataRegistrationIntegrationTests, DataVaultCompiledCompatibilitySqliteTests, and the existing read-only generated-table query examples.
- The contract keeps the no-runtime-change posture: this lifecycle slice is analyzer and documentation work only.

## Implementation Notes
- Keep the new diagnostics in the existing EfCore category with warning severity and contiguous id allocation immediately after DMV1911.
- Use direct source evidence only: instance members read in OnModelCreating or direct DVault model-selection code, direct ReplaceService<IModelCacheKeyFactory, ...>() paths, and direct UseModel(...) or AddDbContextPool<TContext>(...) calls.
- Treat a custom IModelCacheKeyFactory as satisfying the contract only when the relevant varying members are directly visible in the returned key shape; if the key computation is opaque or indirect, skip instead of guessing.
- Do not diagnose compiled-model usage just because UseModel(...) appears; the existing fixed-model compiled-compatibility lane must remain non-diagnostic.
- Sibling tickets already exist for implementation, fixtures, and docs, so this ticket should remain the contract and id-allocation authority for the lifecycle slice.
- No bounded writes were applied during this run; relation state was verified from persisted ticket storage and already matches the intended epic to contract to implementation flow.

## Open Questions
- none

## Follow-Up Questions
- Whether to extend the same lifecycle rules later to AddPooledDbContextFactory<TContext> or other pooling entrypoints can be decided separately; the current contract is intentionally bounded to the documented AddDbContextPool<TContext> baseline.
- After implementation lands, decide whether to copy the accepted DMV1912 through DMV1914 wording into the ticket description or let the sibling documentation surfaces remain the long-term public wording.

## Risks
- If the implementation tries to infer more than direct source-visible evidence, it is likely to regress existing safe compiled-model and registry-backed fixtures with false positives.
- If the implementation stays intentionally high-confidence, some caller-owned variable-shape cases will remain guidance-only; the documentation task must say that skipped ambiguous cases are intentional rather than a runtime guarantee.

## Split Recommendations
- No further split is recommended. The epic already separates contract (06F8KZGC4NY41PRYB2RP00ZA1M), implementation (06F8KZGNRG5FY4WWCY3FAX2NS4), fixtures (06F8KZGZND5ZCH147PVBRWXYN4), and docs (06F8KZHAB717MJJNAWWK7S0A5W).

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the analyzer contract for visible DVault misuse around EF model-cache keys, compiled models, UseModel(...), and DbContext pooling. Acceptance requires explicit supported patterns, unsupported inference boundaries, diagnostic ids, false-positive avoidance rules, and fixture expectations.