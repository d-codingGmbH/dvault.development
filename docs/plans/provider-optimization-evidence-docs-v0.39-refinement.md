# Provider Optimization Evidence Docs v0.39 Refinement

Status: ticket-bound refinement note
Ticket: `06FBSC4QXYQ0SWB1DPMGJJ5XX0`

## Purpose

Define the bounded documentation scope for carrying the provider optimization evidence baseline into the v0.39 performance-guide and release-note surfaces without turning skipped, diagnostics-only, smoke-only, or storage-footprint rows into measured provider-timing claims.

## Verified Repository Baseline

- `docs/plans/provider-optimization-evidence-matrix.md` is the canonical lookup surface for provider row identity, evidence posture, source artifacts, claim boundaries, and the `dvault.provider-evidence.v1` manifest shape.
- `docs/plans/provider-optimization-gap-matrix.md` is the canonical follow-up backlog and recommendation surface for capability gaps and evidence gaps.
- `docs/performance-profiles.md` already points readers to the evidence matrix and preserves the bounded decision-tree guidance, but it does not yet carry a v0.39 release-alignment note or a direct handoff to the gap-matrix follow-up recommendations.
- The root benchmark artifact triplet (`benchmark-summary.md`, `.csv`, `.json`) keeps SQLite completed timing rows and optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows as skipped placeholders because the corresponding connection-string environment variables were unset in the checked-in run.
- The checked-in release history currently stops at `docs/releases/v0.38.0.md`, and `CHANGELOG.md` currently starts with the v0.38.0 entry. No v0.39.0 release note or changelog entry is present in the visible repository baseline.

## Required Documentation Surfaces

- `docs/performance-profiles.md`
- `docs/releases/v0.39.0.md`
- `CHANGELOG.md`

The evidence and gap matrices remain the source documents for row-level facts and follow-up planning. This ticket should cite them, not replace them.

## Required Content Boundary

- Separate measured facts from forward-looking work:
  - measured facts come from completed benchmark-backed evidence with preserved run context
  - skipped-placeholder, diagnostics-only, smoke-only, and storage-footprint rows are not measured external-provider timing claims
- Cite provider evidence by matrix row identity (`scenario`, `provider`, `baseline`, `posture`) instead of copying raw benchmark prose or mixing planning statements into timing claims.
- Use the gap matrix as the canonical source for follow-up recommendations:
  - non-SQLite `latest-satellite-read` remains a capability-gap recommendation set
  - external-provider save, PIT, and bridge rows remain evidence-gap recommendations until completed provider-configured benchmark artifacts exist
- Preserve the current DB2 boundary from the checked-in baseline:
  - diagnostics-gated clean-context save and PIT/bridge candidate behavior may be documented
  - completed DB2 timing, latest-satellite optimization, staged DB2 bulk, provider-native chunk execution, and live-schema reading must remain out of scope
- Preserve the established release-note pattern unless another ticket changes it explicitly: v0.39.0 should continue the coordinated release-label documentation model over the visible `8.39.0` and `10.39.0` consumer package-version lines.

## Acceptance Boundary

- `docs/performance-profiles.md` explicitly distinguishes measured evidence from follow-up recommendations and points readers to the evidence matrix for facts and the gap matrix for future work.
- `docs/releases/v0.39.0.md` records the v0.39 documentation baseline for the provider evidence matrix and caveat story without claiming new provider timings beyond the checked-in evidence.
- `CHANGELOG.md` adds the v0.39.0 summary entry and points to the new release note.
- No update in this ticket reruns benchmarks, changes benchmark schemas, adds provider implementations, widens DB2 claims, or restates skipped/diagnostics-only rows as completed timing evidence.
