Goal: implement the first release-quality slice of the deferred Data Vault capabilities that were intentionally excluded from the MVP.

Scope:
- Define the extension architecture and compatibility guardrails for advanced Data Vault patterns.
- Add baseline PIT table modeling and generation.
- Add baseline bridge table modeling and generation.
- Add baseline multi-active satellite modeling and persistence semantics.
- Add advanced hooks needed by those capabilities without weakening deterministic defaults.

Out of scope:
- Replacing existing hub/link/satellite APIs for the MVP scenarios.
- Provider-specific performance work; that is tracked in the provider optimization epic.
- Unbounded automation for every Data Vault pattern variation. Each capability must document supported cases and explicit limitations.

Acceptance Criteria:
- Each capability has tests, documentation, and a clear provider-neutral baseline.
- Advanced capabilities do not silently change existing v0.4 behavior.
- API additions are reviewed through compatibility snapshots or an explicit documented decision.
- The epic cannot close merely because child tickets exist; the combined child outcome must be reviewed as satisfying the deferred capability goal.