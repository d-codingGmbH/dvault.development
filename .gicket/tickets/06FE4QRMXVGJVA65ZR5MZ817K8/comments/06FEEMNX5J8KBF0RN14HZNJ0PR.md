[gicket-bot] PO refinement contract

Summary
- Verified that the v0.42 provider-evidence documentation baseline is already finite, backed by done upstream evidence tickets, and ready for PO-critic review with no remaining PO blockers or split needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The live ticket is `todo`, carries `needs-po` and `automation/bot-ready`, and `is-blocked=false`; all verified incoming `blocks` links come from done upstream evidence tickets (`06FE4QQTS5NFAYN39KP4QW2424`, `06FE4QRC7D55RS8ZZ37ZAEJ98M`, `06FE4QQ0YTHD7624MGVPKKK1C0`, `06FE4QPR8TF8R6PXNM3RMXN8JG`, `06FE4QR3DD7EFZ4F35SBTFGWSR`, `06FE4QQ9VF7B74E60CXEHSS5XW`).
- The checked-in repository already exposes the finite v0.42 documentation baseline in `docs/performance-profiles.md`, `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `docs/local-validation.md`, `docs/releases/v0.42.0.md`, and `CHANGELOG.md`; this refinement ratifies that authoritative doc set instead of reopening tuning design.
- Measured provider timing remains bounded to preserved artifact bundles: root SQLite rows, v0.32 smoke-read PIT/bridge rows, ticket `06FE4QQ9VF7B74E60CXEHSS5XW` for MySQL latest-satellite, ticket `06FE4QRC7D55RS8ZZ37ZAEJ98M` for SQL Server bulk thresholds, and ticket `06FE4QR3DD7EFZ4F35SBTFGWSR` for DB2 hotspot save/read evidence.
- The current v0.42 release baseline dated 2026-06-20 keeps `v0.42.0` as the release label and `8.42.0` / `10.42.0` as the consumer package versions; no consumer-facing `0.42.0` package version is in scope.

Scope In
- Ratify the v0.42 documentation baseline for `docs/performance-profiles.md`, `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `docs/local-validation.md`, `docs/releases/v0.42.0.md`, and `CHANGELOG.md`.
- Document which provider rows are completed timing versus skipped-placeholder, diagnostics-only, smoke-only, storage-footprint, or follow-up recommendation posture.
- Keep provider-specific tuning gates and stop/fallback boundaries aligned across release notes, performance guidance, local validation, and the matrices.
- Preserve the package-version mapping `v0.42.0` to `8.42.0` / `10.42.0` in release-facing documentation.

Scope Out
- Rerunning benchmarks, provisioning external databases, or generating new provider-configured artifact triplets.
- Changing provider strategy thresholds or widening supported save/read shapes beyond the already documented boundaries.
- Promoting PostgreSQL latest-satellite, SQL Server latest-satellite, Oracle latest-satellite, PostgreSQL bulk, MySQL bulk, or Oracle bulk rows to measured timing without new accepted evidence.
- Expanding DB2 beyond clean-context optimized save and supported latest-satellite/PIT/bridge rows.
- A broad README/package-publication/adopter-documentation sweep outside the listed current-ticket surfaces unless another ticket explicitly reopens it.

Open questions
- none

Follow-up questions
- Does the team want separate future tickets for the remaining evidence gaps that the gap matrix still tracks, or is the matrix itself the sufficient backlog surface for PostgreSQL latest-satellite, SQL Server latest-satellite, Oracle latest-satellite, PostgreSQL bulk, MySQL bulk, and Oracle bulk?
- If SQL Server latest-satellite timing should ever be promoted, should that happen only through a dedicated evidence ticket that accepts the read lane explicitly rather than reusing the incidental row from the bulk-threshold bundle?

Risks
- Future provider work can drift the docs if thresholds, accepted bundles, or fallback boundaries change without updating the release note, performance guide, local validation, and matrix surfaces together.
- Downstream tickets could overstate performance if they cite skipped-placeholder, diagnostics-only, smoke-only, or gap-matrix rows as completed timing instead of using the accepted artifact bundles.

Split recommendations
- No split recommended; the ticket is already bounded to a finite documentation baseline and the remaining unmeasured provider lanes are explicitly tracked in the gap matrix instead of needing new child tickets from this PO refinement.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment