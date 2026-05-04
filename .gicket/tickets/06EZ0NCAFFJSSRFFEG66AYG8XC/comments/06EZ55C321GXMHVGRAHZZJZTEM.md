[gicket-bot] PO refinement contract

Summary
- Refined the story to bound v1 provider comparison to SQLite as the required baseline plus opt-in PostgreSQL via DVAULT_TEST_POSTGRES_CONNECTION_STRING, kept SQL Server/Oracle/MySQL out of scope until they own optimized strategies, and retained existing child ticket 06EZ0NCGYCADKEYGR16J5PJFS0 as the SQLite artifact baseline; no new child tickets or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - V1 external-provider scope is SQLite as the required baseline and PostgreSQL as the only optional opt-in external provider for fallback-versus-optimized comparison; SQL Server, Oracle, and MySQL are out of scope for this story and must not be treated as configured comparison targets.
- critic-item-2: `answered` - The benchmark runner should treat PostgreSQL as configured only when DVAULT_TEST_POSTGRES_CONNECTION_STRING is present and non-empty in the process environment. V1 does not add new provider CLI or options input beyond the existing --iterations, --warmup, and --output flags.
- critic-item-3: `answered` - When a provider package exists but does not own an optimized save strategy in the visible repository baseline, that provider is out of scope for this story. V1 artifacts do not emit fallback-only or skipped optimized rows for MySQL, Oracle, or SQL Server; skipped-provider behavior in this story applies only to the in-scope optional PostgreSQL path.
- critic-item-4: `answered` - The contract is now bounded so any configured external providers means only the optional PostgreSQL benchmark path. Compatibility-only packages are excluded from the fallback-versus-optimized comparison set, which removes the earlier ambiguity.
- critic-item-5: `answered` - The benchmark discovery contract is env-var based for v1: PostgreSQL is configured when DVAULT_TEST_POSTGRES_CONNECTION_STRING is set, reachable when the benchmark can open and use that provider successfully, and otherwise represented as a skipped PostgreSQL entry with a normalized human-readable reason such as not configured, provider dependency unavailable, or connection unreachable.

Clarifications
- The comparison baseline remains the provider-neutral fallback save path versus provider-specific strategies selected through the current strategy-dispatch architecture.
- SQLite is the required v1 benchmark baseline for every run.
- PostgreSQL is the only optional external provider in scope for v1 comparison reporting, and it is discovered by the DVAULT_TEST_POSTGRES_CONNECTION_STRING environment variable.
- SQL Server, Oracle, and MySQL are compatibility-only provider packages for this story and are out of scope until their own optimized strategy tickets exist.
- If PostgreSQL is not configured, its provider dependency is unavailable, or the connection cannot be opened, the artifact must still emit skipped PostgreSQL entries with normalized human-readable reasons.
- Existing child ticket 06EZ0NCGYCADKEYGR16J5PJFS0 remains the SQLite artifact baseline slice under this story; this refinement adds the external-provider boundary and discovery contract rather than replacing that work.

Scope In
- Consolidate benchmark output so each scenario row or record shows provider, strategy, dataset size, change ratio, execution status, and measured results in one stable artifact.
- Support direct fallback-versus-optimized comparison for SQLite and for PostgreSQL when the PostgreSQL benchmark path is configured.
- Keep SQLite-only local benchmark execution useful when no external provider is configured.
- Emit explicit skipped PostgreSQL entries when the PostgreSQL benchmark path is not configured, not reachable, or otherwise unavailable.
- Document the exact provider-discovery contract, run prerequisites, and skipped-row interpretation guidance.

Scope Out
- Benchmark comparison, skipped-row reporting, or configuration discovery for MySQL, Oracle, or SQL Server in this story.
- Implementing new provider-specific save optimizations or changing the public IDataVaultSaveService, IDataVaultProviderSaveStrategy, or provider capability profile contracts.
- Adding benchmark-specific provider CLI flags or alternate connection-input surfaces beyond the existing benchmark options and the reused DVAULT_TEST_POSTGRES_CONNECTION_STRING env var.
- Requiring every external provider package in normal local developer environments.
- CI orchestration, environment provisioning, dashboards, or long-term benchmark history beyond the single-run consolidated artifact.

Open questions
- none

Follow-up questions
- Should later provider tickets add SQL Server, Oracle, or MySQL rows only after each provider owns an optimized strategy path?
- Should a later benchmark-infrastructure ticket introduce benchmark-specific env-var names or CLI overrides instead of reusing DVAULT_TEST_POSTGRES_CONNECTION_STRING?
- Should CI eventually provision PostgreSQL so release evidence includes more than the SQLite baseline by default?

Risks
- Reusing a test-named environment variable for benchmarks may confuse users unless the README and skipped-row reasons are explicit.
- If skipped-provider reason normalization drifts, archived artifacts may still be hard to compare across machines.
- Absolute PostgreSQL timings can vary substantially across local environments, so the report must keep scenario metadata and skip semantics prominent.

Split recommendations
- Keep this story focused on the consolidated artifact plus the SQLite-required and PostgreSQL-optional contract; move SQL Server, Oracle, and MySQL expansion into separate provider tickets.
- If benchmark-specific configuration surfaces or CI provisioning grow beyond straightforward env-var discovery, split that infrastructure work into separate follow-up tickets rather than widening this story.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment