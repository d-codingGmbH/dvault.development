[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the delivery contract is concrete, grounded in existing benchmark and documentation surfaces, and has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4R1N2ADN77NDFDP4GR7020/description.md keeps PO handoff `ready_for_po_critic` and `## Open Questions` = `none`.
- `git --no-pager diff --name-only develop...HEAD` lists only `.gicket/tickets/06FE4R1N2ADN77NDFDP4GR7020/**`, and `git log --oneline -n 5` on this branch shows PO handoff/lease commits (`5ed16eadb`, `8352db191`, `7c4134540`), so this is still a pre-development ticket-quality gate rather than an implementation review.
- docs/plans/performance-evidence-benchmark-artifact-contract.md already requires the existing `benchmark-summary.md/csv/json` triplet, preserved run context including hash-key variants and optional-provider status, and supplemental footprint sidecars for hash-key width/storage comparisons.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkHashKeyVariant.cs defines `BenchmarkHashKeyVariant.BoundedStorageMatrix` as `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, and `sha256-128-v1-binary`, and `CreateExecutionDetail()` emits `hashKeyVariant`, `stableHashAlgorithm`, `digestBytes`, `hashKeyStorage`, and `hashKeyPayloadBytes` tokens.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkHashKeyFootprintArtifacts.cs writes `hash-key-footprint.md`, `hash-key-footprint.csv`, and `hash-key-footprint.json` beside the benchmark triplet when multiple variants run.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs already reuses the existing runner for `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` across SQLite plus optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 providers.
- benchmark-summary.md currently reports `Hash key variants: sha256-v1-hex` and all optional providers skipped because `DVAULT_TEST_*_CONNECTION_STRING` values are unset, confirming that provider-configured binary-vs-hex evidence is a real current gap.
- hash-key-footprint.md and docs/plans/provider-optimization-evidence-matrix.md currently scope hash-key storage evidence to SQLite-local `storage-footprint` rows and explicitly keep claims SQLite-only until a future provider-specific bundle is checked in.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not show a worked documentation example for a provider/scenario where binary improves footprint but is timing-neutral or regressive while shortened-digest variants also move the result.
- The contract does not give a concrete rollup example for split provider-family evidence bundles when some optional lanes remain skipped or failed.

Risky assumptions
- Assumes the `same execution` requirement applies per checked-in evidence bundle; otherwise the split recommendation would conflict with a single all-provider run interpretation.
- Assumes closure can still proceed when some optional-provider lanes remain skipped or failed, provided those rows stay visible as placeholders/caveats and are not promoted as timing evidence.

AC / test suggestions
- Add acceptance/verifier coverage that `--hash-key-storage-matrix` preserves all four variant labels plus `optionalProviders` execution status in `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`.
- Add validation that canonical docs cite provider rows by `scenario`, `provider`, `baseline`, and `posture`, and never convert `storage-footprint` or skipped-placeholder rows into timing claims.

Implementation watchouts
- Keep binary-vs-hex conclusions like-for-like within the same stable-hash algorithm; do not attribute `sha256-128-v1` gains to binary storage alone.
- Preserve the existing provider row identities already emitted by `BenchmarkRunner`, including PostgreSQL direct/UNNEST and MySQL retained multi-row save rows where applicable.
- Keep `HexString` and lowercase-hex public-boundary and migration caveats intact even if provider-configured binary rows show footprint or timing wins.

Non-blocking notes
- The repository baseline already contains the reusable runner, artifact contract, and four-variant matrix plumbing, so this ticket is concrete enough for direct developer handoff.
- Current branch history is ticket-metadata-only relative to `develop`, which matches a normal pre-development PO gate and is not a blocker.

Split recommendations
- If one all-provider collection pass is operationally unstable, split collection by provider family, but delay canonical evidence-surface promotion until the required bundles for the agreed scope are assembled.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment