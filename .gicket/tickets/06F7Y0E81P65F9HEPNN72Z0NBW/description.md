Closure decision: v0.24.0 keeps EF model-cache, compiled-model, and DbContext pooling safety as documentation guidance rather than adding new DMV diagnostics.

The checked-in analyzer surface remains DMV1910 and DMV1911 for generated shared-type table misuse. The root README, v0.24.0 release notes, DVault EF compiled compatibility note, and analyzer README document the safe registry-backed model-cache baseline and the caller-owned responsibilities for custom IModelCacheKeyFactory, UseModel(...), and AddDbContextPool<TContext>(...).

No product-code or analyzer change is required for this release. A future analyzer expansion would need a new focused ticket with high-confidence heuristics and explicit false-positive tradeoffs.