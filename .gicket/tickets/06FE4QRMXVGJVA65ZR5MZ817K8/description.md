<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified that the v0.42 provider-evidence documentation baseline is already finite, backed by done upstream evidence tickets, and ready for PO-critic review with no remaining PO blockers or split needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The live ticket is `todo`, carries `needs-po` and `automation/bot-ready`, and `is-blocked=false`; all verified incoming `blocks` links come from done upstream evidence tickets (`06FE4QQTS5NFAYN39KP4QW2424`, `06FE4QRC7D55RS8ZZ37ZAEJ98M`, `06FE4QQ0YTHD7624MGVPKKK1C0`, `06FE4QPR8TF8R6PXNM3RMXN8JG`, `06FE4QR3DD7EFZ4F35SBTFGWSR`, `06FE4QQ9VF7B74E60CXEHSS5XW`).
- The checked-in repository already exposes the finite v0.42 documentation baseline in `docs/performance-profiles.md`, `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `docs/local-validation.md`, `docs/releases/v0.42.0.md`, and `CHANGELOG.md`; this refinement ratifies that authoritative doc set instead of reopening tuning design.
- Measured provider timing remains bounded to preserved artifact bundles: root SQLite rows, v0.32 smoke-read PIT/bridge rows, ticket `06FE4QQ9VF7B74E60CXEHSS5XW` for MySQL latest-satellite, ticket `06FE4QRC7D55RS8ZZ37ZAEJ98M` for SQL Server bulk thresholds, and ticket `06FE4QR3DD7EFZ4F35SBTFGWSR` for DB2 hotspot save/read evidence.
- The current v0.42 release baseline dated 2026-06-20 keeps `v0.42.0` as the release label and `8.42.0` / `10.42.0` as the consumer package versions; no consumer-facing `0.42.0` package version is in scope.

### Scope In
- Ratify the v0.42 documentation baseline for `docs/performance-profiles.md`, `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `docs/local-validation.md`, `docs/releases/v0.42.0.md`, and `CHANGELOG.md`.
- Document which provider rows are completed timing versus skipped-placeholder, diagnostics-only, smoke-only, storage-footprint, or follow-up recommendation posture.
- Keep provider-specific tuning gates and stop/fallback boundaries aligned across release notes, performance guidance, local validation, and the matrices.
- Preserve the package-version mapping `v0.42.0` to `8.42.0` / `10.42.0` in release-facing documentation.

### Scope Out
- Rerunning benchmarks, provisioning external databases, or generating new provider-configured artifact triplets.
- Changing provider strategy thresholds or widening supported save/read shapes beyond the already documented boundaries.
- Promoting PostgreSQL latest-satellite, SQL Server latest-satellite, Oracle latest-satellite, PostgreSQL bulk, MySQL bulk, or Oracle bulk rows to measured timing without new accepted evidence.
- Expanding DB2 beyond clean-context optimized save and supported latest-satellite/PIT/bridge rows.
- A broad README/package-publication/adopter-documentation sweep outside the listed current-ticket surfaces unless another ticket explicitly reopens it.

## Acceptance Criteria
- `docs/performance-profiles.md`, `docs/releases/v0.42.0.md`, and `CHANGELOG.md` consistently distinguish measured evidence from planned guidance, skipped placeholders, diagnostics-only/smoke-only posture, and follow-up gaps.
- `docs/plans/provider-optimization-evidence-matrix.md` and `docs/plans/provider-optimization-gap-matrix.md` remain the canonical row-citation surfaces and reflect the accepted v0.42 posture: MySQL latest-satellite, SQL Server bulk thresholds, and DB2 hotspot rows are promoted only within their accepted scenario boundaries, while unsupported or unmeasured rows stay deferred.
- `docs/local-validation.md` explains the default local lane, opt-in external-provider test gates, and the benchmark contract for optional-provider skipped rows versus provider-configured evidence bundles, including the narrower DB2 benchmark boundary.
- The v0.42 docs keep the verified provider tuning gates aligned: PostgreSQL direct or UNNEST versus staged COPY at 60-plus operations, SQL Server native bulk at 50-plus total operations with at most 500 satellite operations, MySQL retained or staged candidate gating, Oracle direct optimized batching at 50-plus total operations with at most 10000 satellite operations, and DB2 clean-context set-based save only.
- Release-facing docs preserve the package-version contract that `v0.42.0` is the release label and `8.42.0` / `10.42.0` are the consumer package lines.
- No surface in this ticket claims automatic PIT or bridge maintenance, staged DB2 bulk, provider-native chunk execution, DB2 live-schema reading, or a consumer-facing `0.42.0` package version.

## Definition of Done
- The refinement contract names the authoritative doc surfaces and accepted evidence bundles that bound this ticket.
- Upstream done evidence tickets are treated as completed input, not as open blockers, and the current ticket keeps `open_questions` empty.
- Remaining implementation, if any, stays doc-only inside the listed surfaces and does not require new benchmark generation, provider feature expansion, or package-publication work.
- PO critic can review the ticket against the already finite v0.42 baseline without reopening matrix ownership, evidence promotion rules, or provider caveat boundaries.

## Implementation Notes
- Treat the current repository baseline as authoritative: `docs/performance-profiles.md`, the evidence matrix, the gap matrix, `docs/local-validation.md`, `docs/releases/v0.42.0.md`, and `CHANGELOG.md` already carry the v0.42 evidence and tuning framing and should be kept mutually consistent.
- MySQL latest-satellite timing claims must cite ticket `06FE4QQ9VF7B74E60CXEHSS5XW` and stay limited to the current optimized MySQL lane; do not restate that bundle as a provider-neutral fallback improvement comparator.
- SQL Server save-tuning claims must cite ticket `06FE4QRC7D55RS8ZZ37ZAEJ98M` for the accepted 2026-06-20 bulk-threshold bundle; the incidental latest-satellite row in that bundle remains out of scope until a dedicated latest-satellite evidence ticket accepts it.
- DB2 timing claims must cite ticket `06FE4QR3DD7EFZ4F35SBTFGWSR` and stay limited to clean-context optimized save plus supported latest-satellite, PIT, and bridge reads; staged DB2 bulk, provider-native chunk execution, dirty-context saves, stale PIT or bridge maintenance, incomplete read-shape evidence, unsupported latest-satellite shapes, and live-schema reading remain out of scope.
- PostgreSQL latest-satellite remains a strategy-registration or capability closure rather than a completed timing row, and Oracle latest-satellite plus PostgreSQL, MySQL, and Oracle bulk rows remain gap-matrix follow-up work until provider-configured bundles exist.
- The live outgoing `blocks` link from this ticket to `06FE4R089MT3BYRCVH7Q4EX6CG` can remain; this ticket should not absorb the downstream binary or hash-storage story.

## Open Questions
- none

## Follow-Up Questions
- Does the team want separate future tickets for the remaining evidence gaps that the gap matrix still tracks, or is the matrix itself the sufficient backlog surface for PostgreSQL latest-satellite, SQL Server latest-satellite, Oracle latest-satellite, PostgreSQL bulk, MySQL bulk, and Oracle bulk?
- If SQL Server latest-satellite timing should ever be promoted, should that happen only through a dedicated evidence ticket that accepts the read lane explicitly rather than reusing the incidental row from the bulk-threshold bundle?

## Risks
- Future provider work can drift the docs if thresholds, accepted bundles, or fallback boundaries change without updating the release note, performance guide, local validation, and matrix surfaces together.
- Downstream tickets could overstate performance if they cite skipped-placeholder, diagnostics-only, smoke-only, or gap-matrix rows as completed timing instead of using the accepted artifact bundles.

## Split Recommendations
- No split recommended; the ticket is already bounded to a finite documentation baseline and the remaining unmeasured provider lanes are explicitly tracked in the gap matrix instead of needing new child tickets from this PO refinement.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: update performance profiles, gap/evidence matrices, local validation notes, and v0.42 release docs after tuning. Acceptance: docs distinguish measured improvements, deferred gaps, and provider-specific caveats.