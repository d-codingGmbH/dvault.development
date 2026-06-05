Define the authoritative v0.31.0 performance decision-tree contract as a concrete repository documentation change.

Required repository output
- Update `docs/performance-profiles.md` with an explicit decision-tree contract section for v0.31.0.
- The contract must cover write selection, read selection, typed helper generation, diagnostics evidence, and stop/fallback conditions.
- This ticket must produce a repository diff outside `.gicket`; it is not a tracking-only or ratification-only ticket.

Scope in
- State the ordered questions an adopter should answer before choosing `DataVaultBulkSaveRequest`, `DataVaultChunkedSaveRequest`, async chunk-source saves, staged provider ingestion, latest reads, PIT reads, bridge reads, or typed helper generation.
- Name the evidence surfaces used for decisions: `IDataVaultDiagnosticsService`, `IDataVaultReadDiagnosticsService`, read-shape diagnostics, telemetry summaries, benchmark artifacts, and explicit PIT/bridge maintenance freshness.
- Keep typed helper generation as an opt-in design-time branch over authoritative support bundles, not as a fifth runtime performance profile.
- Link to the existing PIT/bridge, typed helper, activity tracing, and benchmark evidence documents where they are the authoritative detail.

Scope out
- New runtime APIs, provider dispatch changes, automatic strategy routing, benchmark reruns, dashboards, exporters, background maintenance, or provider-specific SQL artifact contracts.
- Rewriting the practical examples or release notes; those are handled by downstream v0.31.0 tasks.

Relation intent
- This story blocks the practical decision-tree documentation task because the downstream task should elaborate the contract, not invent a competing decision model.