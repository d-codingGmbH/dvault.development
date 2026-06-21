[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing hash-key benchmark harness: land provider-configured binary-vs-hex evidence with the bounded four-variant matrix, promote only benchmark-backed timing claims, and update performance/evidence docs to call out wins, neutral cases, and caveats without changing storage-profile behavior.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the existing bounded hash-key matrix variants already defined in the repo: `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, and `sha256-128-v1-binary`.
- Reuse the existing benchmark artifact contract: `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, plus same-label `hash-key-footprint.md`, `hash-key-footprint.csv`, and `hash-key-footprint.json` sidecars.
- Treat the matrix as variant-driven evidence on existing benchmark scenarios, not as a new scenario family or a new artifact schema.
- Treat provider-configured completed rows as measured timing evidence and skipped rows as placeholders only; keep footprint-only facts separate from timing claims.
- For this ticket, the provider comparison boundary follows the current runner and artifact contract: required SQLite local baseline plus optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 lanes.
- Binary-vs-hex conclusions must compare like-for-like algorithm pairs first; shortened-digest variants are a separate dimension and cannot be described as pure binary-storage wins.

Scope In
- Checked-in provider-configured hash-key matrix evidence using the existing bounded four-variant harness and artifact contract.
- SQLite required-baseline rows plus optional-provider comparison rows for the benchmark families the current runner already emits: `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read`, including retained PostgreSQL direct/UNNEST and MySQL multi-row rows where applicable.
- Canonical evidence-surface and adopter-facing documentation updates that summarize measured wins, neutral or regressive cases, and provider-specific caveats by `scenario`, `provider`, `baseline`, and `posture`.
- Footprint sidecars and supporting documentation that explain provider store types and payload-byte differences while preserving the lowercase-hex logical boundary.

Scope Out
- New stable-hash algorithms, extra variant combinations, or a second benchmark output schema.
- Changes to runtime save/read strategies, public hash-key types, or the logical lowercase-hex DVault boundary.
- Automatic migration, backfill, dual-write, or database-provisioning work for hex-to-binary adoption.
- New provider support or widened read/save shape claims beyond the existing benchmark and strategy boundaries.
- Release/version/package-publication changes unless another ticket explicitly asks for release-alignment docs.

Open questions
- none

Follow-up questions
- Should a later ticket add higher-iteration reruns for providers where binary-vs-hex results are directionally flat or noisy before any stronger recommendation is published?
- After provider evidence lands, should adopter guidance ever recommend a provider-specific binary default for new projects, or should `HexString` remain the compatibility default regardless of measured wins?
- Does any provider need a later scale-matrix or latest-index follow-up if the standard binary-vs-hex matrix shows materially different behavior under larger data sets or replay-heavy workloads?

Risks
- Provider timing is hardware- and environment-sensitive; conclusions are only valid with the preserved artifact triplet and run context and should not be generalized beyond those bundles.
- Some providers may show clear storage-footprint reductions without a matching timing win, or may trade time versus allocation differently across save and read scenarios.
- The bounded matrix mixes storage-profile and digest-width variants, so summary language can misattribute shortened-digest gains to binary storage if comparisons are not written carefully.
- Collecting comparable evidence across all optional providers depends on reachable provider environments; missing lanes must be treated as incomplete coverage, not silently satisfied by skipped placeholders.
- Current docs still contain SQLite-only hash-key evidence language, so documentation alignment is part of avoiding contradictory adoption guidance.

Split recommendations
- If capturing comparable configured evidence across PostgreSQL, SQL Server, MySQL, Oracle, and DB2 in one pass proves operationally unstable, split evidence collection by provider family but keep one aggregation step that updates the canonical evidence surfaces only after all required bundles exist.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment