[gicket-bot] PO refinement contract

Summary
- Refined the story around two bounded outcomes: prove the built-in DVault model-cache isolation for registry-backed metadata sources, and document the supported consumer-owned cache-key customization path when model shape varies by tenant/schema/profile state outside that built-in path.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Registry-backed `UseDataVaultMetadata()` is the default supported isolation path for multiple DVault metadata sources because the current implementation already keys the EF model cache by DVault source kind plus metadata fingerprint.
- `UseDataVaultMetadata(DataVaultModelImportResult)` is in scope under the same registry-backed isolation boundary because it resolves to a metadata registry before projection.
- Caller-owned model-shaping inputs that are not encoded by the DVault options extension, such as tenant/schema selection, naming overrides, or load-timestamp/profile variants applied in `OnModelCreating`, are supported only through a caller-supplied `IModelCacheKeyFactory` that includes those discriminators.
- The ticket should document the guarantee boundary explicitly: DVault prevents incompatible cache reuse for its own registry-backed metadata-selection path, but it does not auto-discover arbitrary consumer-specific model-shaping state.

Scope In
- Add regression coverage proving that the same `DbContext` CLR type can use different DVault metadata registries or imported artifacts without reusing an incompatible EF model cache entry.
- Add regression coverage for at least one documented consumer-owned customization example where model shape varies by caller state outside the DVault options extension and a custom `IModelCacheKeyFactory` isolates the models correctly.
- Document the supported usage guidance for model-cache isolation, including when `UseDataVaultMetadata(...)` is sufficient and when consumers must replace `IModelCacheKeyFactory`.
- Keep the proof aligned with the current DVault metadata source annotations, fingerprinting, and one-authoritative-source rules rather than introducing a new metadata selection mechanism.

Scope Out
- No new multi-tenant runtime abstraction, tenant resolver, naming-policy platform, or automatic per-tenant orchestration feature.
- No attempt to make DVault infer arbitrary constructor fields, ambient state, or custom `OnModelCreating` branches automatically for cache-key purposes.
- No compiled-model generator, EF CLI shim, or preflight aggregator work beyond the cache-isolation proof and documentation this story owns.
- No redesign of the existing metadata-source conflict diagnostics or provider capability architecture unless a narrow change is required to support the proof.

Open questions
- none

Follow-up questions
- Should a later diagnostics or preflight ticket add an advisory check that points consumers toward a custom `IModelCacheKeyFactory` when their DVault model shape appears to vary by caller-owned state?
- Should the later v0.17 documentation pass add provider-specific tenant examples, such as schema-per-tenant or prefix-per-tenant, once this story establishes the core cache-isolation guidance?

Risks
- If the docs blur the line between built-in registry-backed isolation and consumer-owned dynamic model variation, adopters may assume unsafe tenant/profile permutations are automatically protected when they are not.
- A proof that relies only on external-provider schema tests could make the regression story harder to run locally; the implementation should keep at least one stable non-external example for the supported custom-cache-key pattern.
- Changes around model-cache keys must preserve the current compiled/runtime metadata behavior and should avoid accidental service-provider churn or over-broad cache fragmentation.

Split recommendations
- none

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