<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this ticket into a bounded evidence-closure/docs-consistency task: the repository already contains completed SQL Server PIT and bridge artifact evidence plus existing strategy/fallback coverage, while current planning surfaces still describe those two rows as open gaps.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `AddDVaultSqlServer()` already registers `SqlServerDataVaultReadStrategy` for provider read, PIT read, and bridge read services; this ticket is about closing evidence/documentation gaps for existing SQL Server PIT/bridge candidates, not inventing a new strategy.
- Approved repository evidence already exists in `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.{md,csv,json}`, where SQL Server `pit-as-of-read` and `bridge-traversal-read` rows are completed and select `SqlServerDataVaultReadStrategy`.
- The root quick benchmark triplet should remain a skipped-placeholder surface when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset; closing this ticket means promoting the existing checked-in artifact-bundle evidence in planning/docs, not requiring the root triplet itself to become provider-configured.
- Live relations remain consistent with the current refinement: incoming `blocks` from `06FBSCGBG8CJ0QNRX4JZJA638G` and outgoing `blocks` to `06FBSCHBJEYYERDPA7JN34Y8PG` stay unchanged.
- No child-ticket split or relation rewrite is justified from the current repository evidence.

### Scope In
- Promote the completed SQL Server `pit-as-of-read` and `bridge-traversal-read` artifact rows into the authoritative planning/documentation surfaces that still call them open gaps.
- Align evidence and gap documentation so SQL Server PIT/bridge read claims cite the preserved v0.32 smoke-read artifact bundle and its run context.
- Preserve and restate the existing read boundary: explicit PIT/bridge maintenance is required, incomplete read-shape evidence falls back, stale maintenance falls back, unsupported shapes fall back, and no new public read API is introduced.

### Scope Out
- SQL Server `latest-satellite-read` timing closure; that remains the separate `latest-satellite-read` evidence gap.
- New SQL Server PIT or bridge algorithm work, new provider strategy names, or alternative read-shape design.
- Changing skipped root quick-benchmark SQL Server rows into completed rows when provider connection strings are unset.
- PostgreSQL, MySQL, Oracle, or DB2 PIT/bridge closure work.
- New benchmark-runner features, external database provisioning, or credential/setup automation.

## Acceptance Criteria
- Repository planning/documentation cites the completed SQL Server `pit-as-of-read` and `bridge-traversal-read` rows from the v0.32 smoke-read artifact triplet as the authoritative evidence surface for this provider/shape pair.
- Surfaces that currently classify SQL Server PIT/bridge rows as `skipped-placeholder` evidence gaps are updated or explicitly bounded so they no longer claim those two rows lack completed SQL Server timing evidence.
- The refined contract continues to name `SqlServerDataVaultReadStrategy` and preserves the explicit-maintenance, incomplete-read-shape, stale-maintenance, unsupported-shape, and provider-neutral fallback boundaries already proved in repository tests and architecture guidance.

## Definition of Done
- Authoritative planning/docs are internally consistent: SQL Server PIT/bridge no longer appear simultaneously as completed artifact-backed evidence and open evidence gaps.
- Retained evidence links point to checked-in benchmark artifacts with preserved run context instead of copied timing tables or inferred claims from skipped root placeholders.
- No product-code or public-API expansion is required; repository test/diagnostic boundaries remain unchanged except for any citation or evidence-posture updates needed to reflect the existing artifact bundle.

## Implementation Notes
- Relevant existing evidence is the checked-in v0.32 smoke read artifact triplet, especially the SQL Server completed `pit-as-of-read` and `bridge-traversal-read` rows in `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md`.
- Relevant contradictory planning surfaces today are `docs/plans/provider-optimization-evidence-matrix.md` and `docs/plans/provider-optimization-gap-matrix.md`, which still treat SQL Server PIT/bridge rows as root-triplet `skipped-placeholder` evidence gaps.
- Supporting boundary evidence already exists in `docs/performance-profiles.md`, `docs/releases/v0.32.0.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs`.
- Use the shared artifact contract as written: completed benchmark rows inside a preserved artifact triplet are valid `completed-timing` evidence; skipped root placeholders remain placeholder guidance and should not be promoted into timing claims, but completed external-provider bundles also must not be ignored.
- Treat this as a planning/docs closure ticket unless a reviewer explicitly rejects the existing v0.32 artifact bundle as insufficient evidence.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket also promote SQL Server `latest-satellite-read` from root-placeholder guidance to completed external-provider evidence, or should that P0.02 gap remain separate until a dedicated benchmark lane is approved?
- After SQL Server PIT/bridge closure lands, should the broader documentation/parity ticket `06FBSCHBJEYYERDPA7JN34Y8PG` downgrade or remove any now-historical dependency on this ticket?

## Risks
- If documentation updates only add the v0.32 smoke-read link without clearing `P2.02` and `P3.02`, the repository will continue to publish contradictory evidence posture for the same SQL Server rows.
- If reviewers insist that only the root quick triplet can close a read-evidence gap, this ticket will need an explicit policy decision because the existing artifact contract and v0.32 evidence bundle already preserve completed SQL Server PIT/bridge timing evidence.

## Split Recommendations
- No split recommended. SQL Server PIT and bridge closure share one provider, one existing artifact bundle, one strategy name, and one documentation-consistency problem; keep SQL Server latest-satellite evidence as its separate existing follow-up.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Produce provider-configured PIT/bridge timing evidence for the existing SQL Server strategy candidates already registered by `AddDVaultSqlServer()` and named in benchmark guidance as `SqlServerDataVaultReadStrategy`. Acceptance: checked-in evidence covers the SQL Server `pit-as-of-read` and `bridge-traversal-read` rows with configured benchmark artifacts or other approved repository evidence; diagnostics, tests, and fallback behavior continue to enforce explicit maintenance plus incomplete/stale evidence fallback boundaries; the ticket does not widen scope into new public API, new read-shape design, or alternative strategy invention.