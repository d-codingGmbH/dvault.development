Goal: define how deferred Data Vault patterns plug into DVault without destabilizing existing model generation and persistence behavior.

Scope:
- Document capability contracts for PIT tables, bridge tables, multi-active satellites, and advanced hooks.
- Decide which APIs are public, which are internal, and which remain experimental for this release.
- Add guardrails for API snapshots, documentation, and examples.
- Preserve deterministic defaults for timestamp, record source, hash keys, and hash diffs unless a hook explicitly overrides them.

Acceptance Criteria:
- A decision record explains the capability architecture and extension boundaries.
- New public contracts are covered by API snapshot review or an explicit compatibility note.
- The story identifies which provider-specific behavior belongs in provider packages instead of the core package.
- PIT, bridge, multi-active, and hook stories have enough architectural guidance to proceed without conflicting designs.