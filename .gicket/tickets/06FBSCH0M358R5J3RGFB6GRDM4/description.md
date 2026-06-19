<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket as a bounded Oracle PIT/bridge external-provider evidence task; repository evidence confirms the Oracle strategy candidates already exist, the current benchmark rows are still skipped placeholders, and no child-ticket, relation, attachment, or planning-document write was materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already fixes the Oracle v1 baseline: AddDVaultOracle() registers OracleDataVaultReadStrategy for PIT and bridge reads, not for latest-satellite reads.
- The current benchmark contract already carries Oracle guidance rows for pit-as-of-read and bridge-traversal-read under the dvault-adddvaultoracle-optimized / oracle-optimized-dvault baseline, but the checked-in root benchmark triplet still records Oracle as skipped when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset.
- Canonical closure evidence must follow docs/plans/provider-optimization-evidence-matrix.md; skipped-placeholder, diagnostics-only, or smoke-only Oracle rows do not satisfy completed timing evidence for this ticket.
- Oracle PIT/bridge fallback boundaries are already repository-backed and stay in scope: provider mismatch, unsupported shape, incomplete read-shape evidence, and stale read-model maintenance must continue to fall back to provider-neutral reads.
- No planning document, attachment, description update, child ticket, or relation write was applied during refinement; the live blocks relations remain unchanged.

### Scope In
- Checked-in Oracle external-provider evidence for the pit-as-of-read and bridge-traversal-read scenarios using the existing dvault-adddvaultoracle-optimized baseline and OracleDataVaultReadStrategy candidate.
- Benchmark and verifier updates that promote the Oracle PIT and bridge rows from skipped-placeholder evidence to completed timing evidence when configured artifacts are present.
- Documentation and evidence-matrix updates needed to cite the Oracle artifact source and claim boundary accurately.
- Regression coverage that preserves explicit maintenance requirements and incomplete or stale evidence fallback behavior for Oracle PIT and bridge reads.

### Scope Out
- Adding an Oracle latest-satellite provider strategy or changing the current provider-neutral latest-satellite posture.
- New public API surface, new read-shape design, or alternative Oracle PIT/bridge strategy invention.
- Automatic PIT or bridge maintenance, scheduler work, SaveChanges-triggered refresh, or other maintenance orchestration.
- Cross-provider evidence closure for PostgreSQL, SQL Server, MySQL, or DB2 beyond the Oracle PIT and bridge rows.

## Acceptance Criteria
- A checked-in Oracle provider-configured benchmark artifact triplet or approved equivalent repository evidence bundle records completed pit-as-of-read and bridge-traversal-read rows for provider Oracle external provider and baseline dvault-adddvaultoracle-optimized.
- The canonical provider evidence docs update the Oracle PIT and bridge rows so their posture is completed-timing and their cited artifact source no longer relies on the current skipped root-row placeholders.
- Verifier coverage continues to require the Oracle PIT and bridge guidance rows and fails if those rows regress to missing or malformed provider strategy metadata.
- Oracle PIT and bridge diagnostics and gate tests still show provider-specific selection only for supported maintained shapes, with provider-neutral fallback for provider mismatch, unsupported shape, incomplete read-shape evidence, and stale read-model maintenance.
- The delivered change closes only the Oracle PIT and bridge evidence gap and does not add latest-satellite optimization or automatic read-model maintenance behavior.

## Definition of Done
- Repository benchmark artifacts and canonical evidence docs contain a checked-in Oracle source of truth for completed PIT and bridge timing claims.
- Oracle PIT and bridge verifier and fallback coverage are updated or confirmed so the repository keeps enforcing the existing claim boundary after the evidence lands.
- Any supporting documentation that cites the provider optimization matrix or gap posture is aligned so Oracle PIT and bridge rows are no longer described as unresolved evidence gaps.
- The resulting repository state still reflects the existing Oracle registration boundary rather than a new provider feature surface.

## Implementation Notes
- Use tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs as the canonical row-verifier baseline; it already defines Oracle guidance rows, so this ticket should upgrade only the PIT and bridge evidence posture rather than invent new scenario identities.
- Use docs/plans/provider-optimization-evidence-matrix.md as the authoritative claim surface, and update docs/plans/provider-optimization-gap-matrix.md plus any dependent performance guidance so Oracle PIT and bridge rows no longer read as evidence-gap placeholders once artifacts exist.
- Keep src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs and src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs as the registration baseline; this ticket is about proving the existing Oracle read candidates, not adding a new strategy family.
- Preserve the Oracle gate behavior already covered in tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs: supported PIT and bridge shapes may select OracleDataVaultReadStrategy, while incomplete read-shape evidence and stale maintenance signals must still fall back.
- The current root benchmark-summary files prove Oracle row identity but still show executionStatus=skipped when Oracle is not configured; do not treat those skipped rows alone as closure evidence.

## Open Questions
- none

## Follow-Up Questions
- After this Oracle PIT and bridge evidence gap closes, should Oracle latest-satellite optimization remain an explicit separate backlog capability-gap item alongside the other non-SQLite providers?
- Once delivery lands, should the live blocks chain 06FBSCGBG8CJ0QNRX4JZJA638G -> 06FBSCH0M358R5J3RGFB6GRDM4 -> 06FBSCHBJEYYERDPA7JN34Y8PG be revalidated for housekeeping?

## Risks
- Completed Oracle timing evidence still depends on a reachable configured Oracle test environment; until that run is checked in, the repository only preserves skipped placeholder rows for Oracle.
- If matrix or guidance docs are updated without matching artifact-backed verifier coverage, the repository could overstate Oracle timing claims relative to the evidence contract.
- Delivery sequencing may still depend on the existing live blocks relations even though PO clarification is complete.

## Split Recommendations
- No additional split is recommended; the repository already bounds this ticket to two Oracle read scenarios plus the required evidence, docs, and verifier updates.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Produce provider-configured PIT/bridge timing evidence for the existing Oracle strategy candidates already registered by `AddDVaultOracle()` and named in benchmark guidance as `OracleDataVaultReadStrategy`. Acceptance: checked-in evidence covers the Oracle `pit-as-of-read` and `bridge-traversal-read` rows with configured benchmark artifacts or other approved repository evidence; diagnostics, tests, and fallback behavior continue to enforce explicit maintenance plus incomplete/stale evidence fallback boundaries; the ticket does not widen scope into new public API, new read-shape design, or alternative strategy invention.