<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Resolved the PO-critic version-line blocker by re-scoping this as a docs-only `v0.39.0` documentation update: the new release note and changelog entry may document the provider-evidence baseline, but they must not assert new consumer package-version lines because repository evidence still exposes only `8.38.0` and `10.38.0`. No child tickets, relation changes, attachments, description writes, or additional planning documents were materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is explicitly allowed to ship a docs-only `v0.39.0` release note and matching changelog entry even though the repository still exposes `8.38.0` / `10.38.0` consumer package lines.
- The `v0.39.0` surfaces in this ticket must not claim, imply, or document `8.39.0`, `10.39.0`, or a consumer-facing `0.39.0` package version.
- The earlier planning-note sentence that preserved an assumed `8.39.0` / `10.39.0` release-note pattern is superseded by this refinement decision because no repo-backed version-alignment evidence is visible.
- No bounded child-ticket creation, relation changes, attachment writes, description writes, or additional planning-document writes were materialized in this pass because repository evidence already resolved the blocker.

### Scope In
- Update `docs/performance-profiles.md` so measured provider evidence is clearly separated from follow-up recommendations and readers are directed to the evidence and gap matrices for canonical details.
- Create `docs/releases/v0.39.0.md` as a docs-only release record for the provider-evidence baseline, caveats, and follow-up posture, without consumer package-version claims.
- Add the matching `CHANGELOG.md` entry that points to the new `v0.39.0` release note and stays consistent with the docs-only scope.

### Scope Out
- Changing `README.md`, `docs/package-compatibility.md`, `docs/production-adoption-checklist.md`, `docs/manual-nuget-publication.md`, `tools/pack-release-packages.sh`, package verification code, or any other package-version surface to `8.39.0` or `10.39.0`.
- Documenting or publishing new consumer package versions, package approval, package hashes, or release automation outcomes.
- Rerunning benchmarks, generating new benchmark artifact triplets, changing benchmark schemas, or widening provider claims beyond the checked-in evidence baseline.
- Provider implementation work, diagnostics behavior changes, or DB2 claim expansion beyond the current evidence posture.

## Acceptance Criteria
- `docs/performance-profiles.md` explicitly distinguishes completed timing evidence from planning-only recommendations and links readers to the evidence matrix for facts and the gap matrix for future work.
- `docs/releases/v0.39.0.md` documents the provider-evidence baseline, caveats, and follow-up recommendations without asserting new provider timings or any consumer package-version line not already backed by visible repository surfaces.
- `CHANGELOG.md` adds a `v0.39.0` summary entry that points to the release note and remains consistent with the docs-only scope.
- The updated docs cite matrix row identity and posture semantics instead of copying raw benchmark tables, mixing planning statements into measured claims, or inventing `8.39.0` / `10.39.0` version facts.

## Definition of Done
- `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, and `CHANGELOG.md` tell one consistent docs-only `v0.39.0` story about the provider-evidence baseline and future work.
- No documentation in this ticket claims `8.39.0`, `10.39.0`, or a consumer-facing `0.39.0` package version without separate repo-backed release-planning/version-alignment evidence.
- All external-provider save, PIT, bridge, and DB2 statements remain bounded by the current evidence posture: SQLite completed timing where present, skipped placeholders where connection strings were unset, and narrower DB2 diagnostics or smoke caveats where applicable.
- No documentation in this ticket introduces new benchmark numbers, package-version facts, provider capability claims, or release promises that the repository evidence does not already prove.

## Implementation Notes
- Repo-backed version surfaces currently stop at `v0.38.0` and `8.38.0` / `10.38.0`: `CHANGELOG.md`, `docs/releases/v0.38.0.md`, `README.md`, `docs/package-compatibility.md`, `docs/production-adoption-checklist.md`, and `tools/pack-release-packages.sh` all confirm that baseline.
- `docs/releases/v0.39.0.md` is missing in the branch snapshot, so this ticket should create that file as a documentation surface only; it should not be treated as proof of new published package lines.
- `docs/plans/provider-optimization-evidence-matrix.md` remains the canonical row-identity and posture source, and `docs/plans/provider-optimization-gap-matrix.md` remains the canonical follow-up recommendation source.
- The root benchmark artifact triplet already proves the bounded evidence story the docs must preserve: SQLite completed timing rows exist, while PostgreSQL, SQL Server, MySQL, Oracle, and DB2 optional-provider rows are skipped placeholders because the checked-in run had no connection strings configured.
- Treat this refinement contract as authoritative over the stale sentence in `docs/plans/provider-optimization-evidence-docs-v0.39-refinement.md` that mentions `8.39.0` / `10.39.0` consumer package-version lines.
- If a later ticket actually moves package/version surfaces to `8.39.0` / `10.39.0`, that separate work must update the pack script, package verifier, release notes, README guidance, and compatibility documentation together; it is not part of this ticket.

## Open Questions
- none

## Follow-Up Questions
- Should a later release-planning/version-alignment ticket move the repository package-version surfaces to a future `8.39.0` / `10.39.0` baseline before any consumer-facing installation guidance changes?
- After the docs-only `v0.39.0` note lands, should other adopter-facing docs such as `docs/production-adoption-checklist.md` receive the same evidence-matrix and gap-matrix cross-links if drift appears there?
- When provider-configured benchmark bundles are added later, which gap-matrix rows should be promoted first from follow-up recommendations into release-note-ready completed timing claims?

## Risks
- If another ticket later introduces repo-backed `8.39.0` / `10.39.0` version alignment, the docs-only `v0.39.0` wording may need a follow-up update to stay aligned with the new release baseline.
- Live ticket comment and relation reads were trust-policy blocked earlier in the session, so duplicate and relation conclusions still rely on the provided ticket snapshot; no blocking duplicate evidence is present in that snapshot.

## Split Recommendations
- No split recommended. The remaining work is one bounded docs-only task across `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, and `CHANGELOG.md`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update performance documentation and release notes with the v0.39 evidence matrix, caveats, and follow-up recommendations. Acceptance: docs separate measured facts from future optimization ideas.