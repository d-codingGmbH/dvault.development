[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded extension of the existing EF safety analyzer: add high-confidence DMV1912+ warnings for visible caller-owned DVault model-shape hazards around EF model caching, compiled models, and pooled DbContexts, while preserving the already-safe registry-backed UseDataVaultMetadata baseline.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Completed story 06F492AKGMKPCRJYF4Z1EC9WY4 already fixed the baseline: registry-backed UseDataVaultMetadata participates in the EF model cache key, while tenant/schema/naming/provider/profile state outside that DVault-owned path remains consumer-owned via IModelCacheKeyFactory.
- Completed story 06F1XPYA9MD0T9C4651ND8KX0W already fixed the compiled-model boundary: UseModel is supported only for one fixed realized model shape and is not a DVault-owned compiled-model toolchain.
- This ticket should extend the existing EfCore analyzer family after DMV1911 instead of creating a new analyzer package or parallel diagnostic taxonomy.
- The v1 analyzer boundary is source-visible and high-confidence only: warn on visible caller-owned model-shape discriminators and risky EF registration patterns, but do not attempt whole-application DI inference or proof that a custom IModelCacheKeyFactory carries every discriminator.
- Safe app-default, explicit-registry, and import-result UseDataVaultMetadata paths are non-diagnostic by default; the warning boundary begins only when extra caller-owned state can change the realized model shape.

Scope In
- Add new EfCore analyzer warnings for source-visible contexts whose DVault model shape varies by caller-owned tenant, schema, naming, provider, or similar profile state and is then used through model-cache-sensitive EF paths.
- Cover risky AddDbContextPool<TContext>(...) registrations when the pooled context is not visibly fixed-model because model-shaping state lives outside options-only configuration.
- Cover risky compiled-model UseModel(...) configurations when the same context type visibly depends on caller-owned model-shape discriminators.
- Cover the missing-escape-hatch case where configuration using a context with visible model-shape discriminators does not replace IModelCacheKeyFactory with an application-owned implementation.
- Treat UseDataVaultMetadata(), explicit DataVaultMetadataModel/DataVaultMetadataRegistry, and UseDataVaultMetadata(DataVaultModelImportResult) as the safe built-in baseline unless extra caller-owned model-shaping state is also present.
- Add analyzer tests for safe and unsafe patterns using current repo vocabulary and examples around tenant/schema/naming/provider/profile-style discriminators.
- Add bounded analyzer-owned guidance so the new diagnostics provide actionable remediation and point to the existing model-cache isolation and EF compiled-compatibility docs.

Scope Out
- No runtime guard, preflight lane, cache-stress harness, or live detection of model-cache reuse beyond static analyzer advisories.
- No attempt to validate that a custom IModelCacheKeyFactory includes every discriminator; v1 only requires a visible application-owned replacement as the escape hatch.
- No new tenant abstraction, dynamic metadata selector, provider-specific pooling strategy, or compiled-model generator/design-time tooling.
- No warning on ordinary fixed-shape registry-backed UseDataVaultMetadata usage, existing read-only compiled-query examples, or the current DMV1910/DMV1911 write-boundary behavior except as unchanged guardrails.
- No broad release-note or adoption-guide rollout beyond the analyzer-owned guidance needed to explain the new warnings.

Open questions
- none

Follow-up questions
- Should a later story add runtime or preflight advisories for non-source-visible model-shape variation that the static analyzer cannot see?
- Once the diagnostic IDs stabilize, should ticket 06F7Y0F650KM61BQXMEQPZ86DR publish provider-specific tenant/schema examples in README and release notes, or keep the broader docs provider-neutral?
- If users want stronger assurance than visible custom IModelCacheKeyFactory presence, should a future advisory pass inspect common factory implementations for obviously missing discriminators?

Risks
- A high-confidence static analyzer will intentionally miss indirection, ambient state, or factory-based model-shaping that is not source-visible in the analyzed lane.
- If the heuristics are too broad, safe fixed-model compiled or pooled patterns will look broken and the warnings will lose credibility.
- If the messages blur the distinction between built-in registry-backed isolation and caller-owned discriminator handling, consumers may assume DVault validates custom cache-key completeness when it does not.
- Overlapping too much with the blocked documentation task could create duplicate guidance or conflicting wording for the same diagnostic IDs.

Split recommendations
- Keep the static Roslyn analyzer slice on this ticket; if the team later wants runtime or preflight detection of cache-key mismatches, raise that as a separate follow-up instead of widening this story.
- Keep broad README, production-checklist, and release-note rollout on ticket 06F7Y0F650KM61BQXMEQPZ86DR; this story should own only the analyzer contract and minimal package guidance.
- If support for indirect DI-registration patterns or deeper custom-cache-key validation becomes necessary, split that into a later advisory expansion rather than weakening the v1 high-confidence boundary.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment