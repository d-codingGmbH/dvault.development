# Goal
Warn consumers when EF Core model caching, compiled models, or DbContext pooling can hide caller-owned DVault model-shape differences.

# Scope In
- Add high-confidence analyzer diagnostics for AddDbContextPool, compiled models, IModelCacheKeyFactory, tenant/schema/naming/provider discriminators, and UseDataVaultMetadata inputs.
- Provide actionable remediation and docs links.

# Scope Out
No automatic service replacement, runtime tenant discovery, or inference of arbitrary OnModelCreating state.

# Acceptance Criteria
- Analyzer tests cover safe and unsafe patterns.
- Diagnostics align with README guidance that caller-owned model-shape discriminators belong in EF model cache keys.