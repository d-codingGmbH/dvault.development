[gicket-bot] PO-critic review contract

Summary
- Source-backed contract and repository evidence are aligned, the previous PO-critic blocker was resolved, and the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/description.md contains PO Handoff decision ready_for_po_critic and Open Questions: none.
- .gicket/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/comments/06F41054VT980VBZT50Y9T4XWW.md returned the ticket to PO for missing source-backed API evidence, and .gicket/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/comments/06F412Q8FZBQ1SWXA75D72SVMM.md answers those critic items with current-branch source references.
- src/DCoding.Data.DVault/DataVaultPitAsOfReadRequest.cs, src/DCoding.Data.DVault/IDataVaultReadService.cs, and src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs show the existing public PIT request and read helpers that the contract preserves.
- src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs, src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs, and src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs show the existing public bridge request/helper surface and registry-backed bridge adapters.
- src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs exposes only latest/as-of satellite hooks, src/DCoding.Data.DVault/DefaultDataVaultReadService.cs still routes ReadPitRowsAsync(...) to DataVaultPitReadPipeline, and src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs still routes bridge reads directly to DataVaultBridgeReadPipeline; this matches the stated implementation gap the story asks developers to close.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs already provide SQLite semantic baselines for PIT row selection/snapshot binding and bridge endpoint filtering plus bounded hierarchy depth.
- README.md, docs/releases/v0.7.0.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md all describe PIT and bridge reads as provider-neutral today; the benchmark README says PIT and bridge rows remain provider-neutral baselines and provider-specific read evidence is limited to latest-satellite reads.
- git rev-parse --verify ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt returned 6f4a6b71f8e29973702927e5e563a8ca65e01cca, matching the provided scratch-source-ref, and git diff --name-only develop...ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt -- . ':(exclude).gicket' returned no non-.gicket paths, so the branch is still in pre-development metadata-only state.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Acceptance criteria describe unsupported request shapes generically but do not name a concrete declined-optimization example for PIT and bridge; keep at least one such example visible in dev/test evidence.
- The contract does not explicitly call out empty-key-list parity for optimized-vs-fallback selection; treat that as an edge case to preserve in verification evidence.

Risky assumptions
- Assumes additive internal dispatch plumbing can be introduced without caller-visible API churn even though IDataVaultProviderReadStrategy currently only models latest/as-of satellite reads.
- Assumes bridge optimization can be added behind existing helpers even though bridge reads currently bypass DefaultDataVaultReadService.
- Assumes SQLite-only proof remains sufficient for release posture; any non-SQLite optimized-read claim would need separate in-repo evidence.

AC / test suggestions
- Make optimized-path selection observable in diagnostics for both PIT and bridge reads, not only through result parity.
- Include explicit fallback coverage for unsupported provider and unsupported request-shape cases on both PIT and bridge reads.
- Keep benchmark and release-note evidence clearly split between provider-neutral baselines and any new SQLite-optimized path.

Implementation watchouts
- Do not change the caller-visible DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, ReadPitRowsAsync(...), ReadPitAsync(...), ReadBridgeRowsAsync(...), or ReadBridgeAsync(...) surface named in the contract.
- Preserve PIT behavior against docs/plans/pit-maintenance-service-v1-contract.md; no implicit PIT refresh or maintenance side effects may leak into the read path.
- Bridge dispatch likely needs additive plumbing because src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs currently calls DataVaultBridgeReadPipeline directly.
- Documentation and benchmarks currently frame PIT and bridge as provider-neutral baselines; update those artifacts with any SQLite optimization evidence before integration.

Non-blocking notes
- No product-code changes are on the ticket branch yet; for this pre-development gate that is expected and not a blocker.

Split recommendations
- No split is required for handoff if the work stays bounded to SQLite proof plus provider-neutral fallback safety.
- If dispatch plumbing grows materially, split shared provider-aware read dispatch from PIT and bridge execution slices before expanding provider scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment