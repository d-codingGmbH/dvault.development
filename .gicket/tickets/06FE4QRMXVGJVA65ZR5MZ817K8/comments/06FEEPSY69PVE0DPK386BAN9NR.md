[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QRMXVGJVA65ZR5MZ817K8/description.md:52-53 says Open Questions -> none; lines 30-36 define the acceptance boundary and lines 44-50 limit the work to the six documentation surfaces plus accepted evidence bundles.
- docs/performance-profiles.md:21-39 and 52-54 make the evidence matrix and gap matrix the canonical row surfaces, keep MySQL latest-satellite scoped to ticket 06FE4QQ9VF7B74E60CXEHSS5XW, keep SQL Server save claims on the 2026-06-20 bulk-threshold bundle, and limit DB2 completed timing to the hotspot bundle.
- docs/plans/provider-optimization-evidence-matrix.md:24-40 promotes only completed-timing rows, preserves the PostgreSQL 60-op, SQL Server 50/500, MySQL staged-vs-retained, Oracle <redacted>, and DB2 clean-context-only gates, and explicitly excludes staged DB2 bulk, provider-native chunk execution, and DB2 live-schema reading.
- docs/plans/provider-optimization-gap-matrix.md:12-18, 49-59, and 89-109 keep SQL Server and Oracle latest-satellite plus PostgreSQL/MySQL/Oracle bulk as deferred evidence gaps while marking MySQL latest-satellite, SQL Server bulk thresholds, and DB2 hotspot rows as closed only within their accepted boundaries.
- docs/local-validation.md:25-39 and 81-91 define default local vs external opt-in validation and the skipped-row contract; benchmark-summary.md:7-14 directly shows PostgreSQL, SQL Server, MySQL, Oracle, and DB2 all skipped in the root quick baseline when connection strings are unset.
- docs/releases/v0.42.0.md:6-19, 45-65, and 99-103 plus CHANGELOG.md:7-13 preserve the release/package mapping v0.42.0 -> 8.42.0 / 10.42.0, reject consumer-facing 0.42.0, and keep automatic PIT/bridge maintenance plus staged DB2 bulk out of scope.
- The referenced artifact files are present at repo root benchmark-summary.*, artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md, artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-<redacted>/benchmark-summary.md, artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-<redacted>/benchmark-summary.md, and artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-<redacted>/benchmark-summary.md; the DB2 and MySQL summaries show completed optimized rows and the SQL Server bundle contains the incidental latest-satellite row that the docs intentionally keep out of scope for promotion.
- git rev-parse HEAD returned d93dc7652eb3085b56a02af63e5a8b85391791f8, matching the supplied scratch-source-ref; git show --stat d93dc7652eb3085b56a02af63e5a8b85391791f8 touched only .gicket/tickets/06FE4QRMXVGJVA65ZR5MZ817K8 lease/comment metadata, so the review is against the bounded snapshot rather than pending unreviewed doc edits.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers will continue treating docs/plans/provider-optimization-evidence-matrix.md as the only promotable evidence surface and will not cite gap-matrix rows or root skipped rows as completed timing.
- Any future SQL Server latest-satellite promotion will go through a dedicated evidence ticket rather than reusing the incidental row in the 2026-06-20 bulk-threshold bundle.
- The scope remains doc-only as described in description.md:44-50; if the work expands into benchmark reruns or provider capability changes, the ticket needs re-refinement.

AC / test suggestions
- Keep the handoff acceptance check at doc-consistency level: the six scoped docs should agree on which rows are completed-timing versus skipped-placeholder, diagnostics-only, smoke-only, storage-footprint, or deferred-gap posture.
- Require every promoted provider claim to cite one of the preserved artifact bundles (v0.32.0 smoke-read, 06FE4QQ9VF7B74E60CXEHSS5XW, 06FE4QRC7D55RS8ZZ37ZAEJ98M, or 06FE4QR3DD7EFZ4F35SBTFGWSR) rather than the root skipped baseline.
- Keep an explicit review check that release-facing wording still maps v0.42.0 to 8.42.0 / 10.42.0 and never introduces a consumer-facing 0.42.0 package version.

Implementation watchouts
- Do not promote the incidental SQL Server latest-satellite-read row from 06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-<redacted>; the current contract and gap matrix keep it out of scope.
- Keep DB2 limited to clean-context optimized save plus supported latest-satellite/PIT/bridge reads from 06FE4QR3DD7EFZ4F35SBTFGWSR; no staged DB2 bulk, provider-native chunk execution, dirty-context saves, stale PIT/bridge maintenance, unsupported shapes, or live-schema reading.
- Treat the root benchmark-summary.* triplet as SQLite/skipped-placeholder baseline only; completed external-provider claims must come from the preserved provider-configured bundles.
- Preserve the documented save gates: PostgreSQL 60-plus staged COPY, SQL Server 50-plus total and at most 500 satellite ops, MySQL retained-vs-staged candidate gating, Oracle 50-plus and at most 10000 satellite ops.

Non-blocking notes
- The outgoing blocks relation .gicket/relations/K8/CG/06FE4QRMXVGJVA65ZR5MZ817K8--06FE4R089MT3BYRCVH7Q4EX6CG--blocks.json is still live, but description.md:50 explicitly says this ticket should not absorb that downstream binary/hash-storage story.

Split recommendations
- No split recommended; the remaining PostgreSQL/SQL Server/Oracle latest-satellite and PostgreSQL/MySQL/Oracle bulk follow-up work is already tracked as explicit gap-matrix rows and follow-up questions.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment