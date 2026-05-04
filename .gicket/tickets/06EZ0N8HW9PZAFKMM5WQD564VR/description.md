Goal: define the common provider optimization boundary before individual database providers add optimized implementations.

Scope:
- Introduce or refine contracts for provider capability discovery, optimized save strategy dispatch, and fallback selection.
- Keep the core package free of provider-specific SQL and provider-name branching outside the strategy boundary.
- Document the capability matrix for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Add tests proving that unsupported or unregistered capabilities fall back to the provider-neutral implementation.

Acceptance Criteria:
- Provider capability selection is explicit, deterministic, and test-covered.
- The provider-neutral fallback remains the default when no provider-specific implementation is available.
- Individual provider stories can implement optimized strategies without changing public core API shape unless the contract story documents and tests that change.
- Documentation identifies which provider projects own which optimization hooks.