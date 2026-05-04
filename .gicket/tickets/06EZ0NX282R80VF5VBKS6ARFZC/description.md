Goal: expose a provider behavior hook surface that deferred capabilities can use without hard-coding provider details in core.

Acceptance Criteria:
- Provider behavior hooks can inherit defaults from the provider-neutral baseline.
- Provider packages can override behavior through explicit registration.
- Tests prove that missing provider overrides do not change existing behavior.