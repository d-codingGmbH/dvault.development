# DB2 Hotspot Evidence Refinement

Status: ticket-bound refinement note
Ticket: `06FE4QR3DD7EFZ4F35SBTFGWSR`
Title: `Task: Tune DB2 optimized save and read evidence path`

## Purpose

Persist the verified PO refinement baseline for the DB2 hotspot evidence task after the upstream DB2 promotion-guardrail work landed and before the downstream v0.42 documentation-update task consumes this ticket's outcome.

## Verified Ticket And Relation Baseline

- `.gicket/tickets/06FE4QR3DD7EFZ4F35SBTFGWSR/ticket.json` keeps this ticket in `todo` with `needs-po`, `automation/bot-ready`, and the expected DB2 performance labels.
- `.gicket/relations/TG/SR/06FE4QPEZW97YR6YT7MQD1MXTG--06FE4QR3DD7EFZ4F35SBTFGWSR--blocks.json` is the live incoming dependency from done task `06FE4QPEZW97YR6YT7MQD1MXTG` (`Task: Add DB2 benchmark promotion guardrails`). Because that source ticket is `done` and this ticket's `is-blocked` flag is `false`, treat it as completed upstream guardrail context, not as an active blocker.
- `.gicket/relations/SR/K8/06FE4QR3DD7EFZ4F35SBTFGWSR--06FE4QRMXVGJVA65ZR5MZ817K8--blocks.json` is the live downstream dependency to `06FE4QRMXVGJVA65ZR5MZ817K8` (`Task: Update provider performance matrices and v0.42 release docs`). This ticket remains the upstream owner for DB2 provider-configured tuning and evidence before documentation promotion.
- `.gicket/relations/DG/SR/06FE4QNWP9606HTB92MTVQMYDG--06FE4QR3DD7EFZ4F35SBTFGWSR--relates.json` preserves the done parent story `06FE4QNWP9606HTB92MTVQMYDG` as historical routing context.
- No additional PO split is justified. The current ticket already owns the downstream DB2 hotspot evidence slice that the done guardrail ticket left open.

## Verified Repository Baseline

- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` registers `Db2DataVaultSaveStrategy`, `Db2DataVaultReadStrategy`, and the PIT/bridge read interfaces after `AddDVaultDb2()`.
- `src/DCoding.Data.DVault.Db2/Db2DataVaultSaveStrategy.cs` and `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs` keep DB2 save scope bounded to clean-context provider-specific execution. The repository does not expose a staged DB2 bulk or provider-native chunk-execution claim.
- `src/DCoding.Data.DVault.Db2/Db2DataVaultReadStrategy.cs` and `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs` keep DB2 latest-satellite reads limited to supported hub-parent, non-multi-active shapes and keep PIT/bridge reads behind supported-shape, complete-read-shape, and fresh-maintenance gates.
- `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` proves representative configured DB2 save behavior plus latest-satellite, PIT, and bridge read execution with diagnostics selecting `Db2DataVaultSaveStrategy` and `Db2DataVaultReadStrategy`.
- `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` still keep DB2 provider-native save, latest-satellite, PIT, and bridge rows as `executionStatus=skipped` with `iterations=0` and `persistedOutcome=not executed` when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset.
- `artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.md`, `artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.csv`, and `artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.json` are the provider-configured DB2 hotspot artifact triplet for this ticket. They record `Provider filter: db2`, `Iterations: 1`, DB2 provider status `completed`, a provider-neutral save comparison row, a completed clean-context optimized save row selected by `Db2DataVaultSaveStrategy`, and completed latest-satellite/PIT/bridge rows selected by `Db2DataVaultReadStrategy`.
- `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `docs/performance-profiles.md`, and `docs/releases/v0.42.0.md` ratify the finite DB2 baseline: completed timing is limited to the DB2 hotspot artifact triplet for clean-context save and supported latest-satellite/PIT/bridge reads, while staged DB2 bulk, provider-native chunk execution, dirty-context saves, unsupported latest-satellite shapes, stale PIT/bridge maintenance, incomplete read-shape evidence, and DB2 live-schema reading remain outside the current baseline.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` explicitly maps DB2 to `UnsupportedDataVaultLiveSchemaReader`, so DB2 live-schema reading remains out of scope for this ticket.

## Scope In

- Collect provider-configured DB2 evidence only for the already supported provider-specific paths:
  - clean-context optimized save
  - latest-satellite read on supported hub-parent, non-multi-active shapes
  - PIT as-of read on supported maintained PIT shapes
  - bridge traversal read on supported maintained bridge shapes
- Keep diagnostics and benchmark evidence aligned so DB2 strategy selection, fallback causes, and supported-shape limits remain explicit.
- Preserve the provider-configured benchmark artifact triplet needed for any completed DB2 timing claim that downstream documentation promotes.
- Enumerate exactly which DB2 rows are `completed-timing` from the configured benchmark evidence and which rows remain diagnostics-only, smoke-only, skipped-placeholder, or historical guidance.

## Scope Out

- New DB2 provider features beyond the currently supported strategy surface.
- Staged DB2 bulk, provider-native chunk execution, DB2 live-schema reading, automatic PIT or bridge maintenance, or broader latest-satellite shape support.
- Documentation/release-note alignment that is already owned downstream by `06FE4QRMXVGJVA65ZR5MZ817K8`, except where this ticket must supply the evidence that those docs cite.

## Acceptance Boundary

- Any DB2 timing claim is backed by a provider-configured benchmark artifact triplet with preserved run context and a completed row for the cited matrix identity.
- Save evidence remains limited to the current clean-context DB2 optimized path selected by diagnostics; dirty contexts or unsupported save shapes continue to fall back to the provider-neutral writer.
- Latest-satellite evidence remains limited to the current DB2 provider strategy for supported hub-parent, non-multi-active shapes; provider mismatch, unsupported parents, or multi-active shapes continue to fall back to the provider-neutral read path.
- PIT and bridge evidence remains limited to explicitly maintained, supported shapes with complete read-shape evidence and fresh maintenance signals; stale or incomplete shapes continue to fall back to provider-neutral reads.
- The repository does not promote skipped-placeholder, diagnostics-only, or smoke-only DB2 rows into completed timing evidence without the configured artifact triplet.
- DB2 live-schema reading remains explicitly unsupported after this ticket unless a separate ticket adds and proves a DB2 catalog reader.

## Definition Of Done Boundary

- Downstream docs can cite the finite, repository-backed DB2 completed-timing rows from the DB2 hotspot artifact triplet without reopening save/read scope decisions.
- The benchmark artifact triplet, diagnostics wording, and evidence-matrix posture agree on supported DB2 optimized paths, fallback behavior, and remaining non-goals.
- No residual PO split or relation rewrite is needed for DB2 hotspot evidence; this ticket remains the bounded owner and the existing downstream docs-update dependency stays intact.

## Follow-Up And Risks

- Follow-up note: the completed DB2 timing rows are limited to `provider-native-bulk-ingestion` for the provider-neutral save comparison and clean-context optimized save, plus `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` for supported DB2 read shapes. Diagnostics-only, smoke-only, skipped-placeholder, dirty-context, unsupported-shape, staged-bulk, provider-native chunk, and live-schema guidance remains non-promoted.
- Risk: without strict evidence posture wording, downstream work could overread `selectedStrategy`, `plannedReadStrategy`, or smoke-test success as measured timing.
- Risk: DB2 support remains sensitive to configuration, clean-context prerequisites, supported read shapes, and explicit PIT/bridge maintenance freshness.
- Risk: this ticket must stay within the already implemented DB2 provider boundary; widening runtime capability and collecting timing evidence are separate concerns.

## Materialization

- Persisted this PO refinement note at `docs/plans/db2-hotspot-evidence-refinement-06FE4QR3DD7EFZ4F35SBTFGWSR.md`.
- Preserved the DB2 hotspot benchmark artifact triplet at `artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.*` and aligned the provider evidence matrix, gap matrix, performance profile, and v0.42 release note around its completed-timing rows.
- No ticket-description write, relation mutation, child-ticket creation, or attachment write was applied in this refinement pass.
