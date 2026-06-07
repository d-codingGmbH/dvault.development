<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the live ticket and repository state, narrowed the story to a single SQL Server `provider-native-bulk-ingestion` dry-run manifest slice, and did not materialize any ticket or planning writes in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The first prototype is narrowed to the existing SQL Server external-provider `provider-native-bulk-ingestion` scenario: 20 order-product pairs, 20 order-product links, and 3 ordered fulfillment satellite operations including one unchanged replay in one provider-eligible bulk request.
- The manifest binds one exact provider/workload pair only: SQL Server external provider using the existing `SqlServerDataVaultSaveStrategy` boundary, not a multi-provider or multi-workload matrix.
- The dry-run manifest must record exact provider/profile identity, including the SQL Server provider name and the existing `sqlserver-v1` capability profile, plus metadata-source kind/fingerprint and explicit dry-run status.
- A valid first prototype may contain zero sidecar SQL payload files; if payload files are present later, they must be manifest-relative and content-hashed.
- Output path selection remains consumer-owned and caller-supplied through the existing design-time command boundary; this ticket does not standardize a repository storage convention.
- Verified live split context: epic `06F8KZTCEMNNFBFTVMFXEN268M` parents this ticket, the parent architecture contract ticket `06F8KZTNG44XDPMVTVCV4WJSHG` is done, and no new child tickets, relation changes, attachments, or planning documents were required in this pass.

### Scope In
- Prototype one deterministic dry-run `dvault.sql-artifact.v1` manifest for the SQL Server `provider-native-bulk-ingestion` workload only.
- Reuse the existing consumer-owned design-time command/host boundary to emit the manifest with a caller-supplied output path.
- Capture manifest metadata for exact provider/profile binding, workload identity, metadata-source traceability, benchmark-evidence references, semantic-parity references, and dry-run review status.
- Keep the prototype bounded to design-time review output and optional manifest-relative payload metadata only.

### Scope Out
- Runtime dispatch, automatic invocation, registration, background execution, or a second default DVault runtime path.
- Automatic deployment, automatic cleanup, EF migration mutation or synchronization, live-schema mutation, or support-bundle refresh automation.
- Additional providers, additional workload shapes, or a full provider matrix in this ticket.
- Collecting or approving the benchmark artifact triplet and semantic-parity evidence itself; that remains downstream work in `06F8KZVCVRPS3NAGQA7J55EAA4`.

## Acceptance Criteria
- The refined prototype targets exactly one provider/workload pair: SQL Server external provider plus the existing `provider-native-bulk-ingestion` scenario.
- The manifest output is deterministic JSON with schema version `dvault.sql-artifact.v1` and no wall-clock timestamps, random ids, machine-specific paths, credentials, raw business data, or raw diagnostics text.
- The manifest records the exact provider identity, the existing `sqlserver-v1` capability profile, workload label `provider-native-bulk-ingestion`, metadata-source kind, metadata-source fingerprint, and an explicit dry-run indicator.
- The manifest records evidence references for the SQL Server provider-neutral fallback row and optimized row for the same scenario, using the existing benchmark artifact triplet rather than inventing ticket-specific benchmark filenames.
- The workload facts in the manifest match the checked-in benchmark baseline: 20 order-product pairs, 20 order-product links, 3 ordered fulfillment satellite operations, one unchanged replay, `selectedStrategy=SqlServerDataVaultSaveStrategy`, `transfer=SqlBulkCopy`, `nativeBulkBoundary=50-plus-operations`, and `cleanupBoundary=temporary-staging-table`.
- The manifest includes semantic-parity reference fields for ordering, load timestamp, record source, hash key, hash diff, latest-state behavior, cancellation, cleanup, and caller-owned transaction behavior for the selected workload.
- The first prototype is valid without sidecar SQL payload files; when payload files are present, the manifest stores only manifest-relative paths and deterministic content hashes.
- Generation stays inside the existing consumer-owned design-time command boundary with a caller-supplied output path and does not add standalone CLI behavior, runtime dispatch, automatic deployment, or EF migration mutation.

## Definition of Done
- Implementation proof shows deterministic manifest output for identical inputs, including the no-sidecar dry-run case.
- Tests or review proof show the prototype emits the exact SQL Server/workload metadata and rejects or skips expansion to other providers or workload shapes outside this ticket's slice.
- The implementation reuses the existing consumer-owned design-time command/host pattern instead of inventing a separate runtime service or standalone DVault CLI.
- No product change from this ticket widens DVault into runtime artifact discovery, automatic invocation, automatic deployment, or automatic migration synchronization.

## Implementation Notes
- Use the existing `DataVaultDesignTimeCommand` / `DataVaultDesignTimeCommandHost` shape as the architecture baseline: caller-supplied output path, consumer-owned `DbContext` factory, and no standalone DVault executable.
- Populate request-bound provider/workload facts through the same design-time diagnostics boundary already used by support-bundle generation, preferring `CreateSupportBundleDiagnostics` when the consumer host supplies it.
- SQL Server is the bounded v1 default because the repository already exposes one single optimized provider-native bulk row for this workload (`dvault-adddvaultsqlserver-optimized`) without the extra below-threshold split that PostgreSQL and MySQL retain.
- Carry benchmark references, semantic-parity references, and dry-run status in the manifest; do not make this ticket responsible for collecting configured external-provider runs or approving production readiness.
- The checked-in SQL Server benchmark row is currently visible but skipped because `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset; that is consistent with keeping this ticket prototype-only.
- No persistent ticket-description update, relation cleanup, attachment binding, child-ticket creation, or planning-document write was materialized in this pass.

## Open Questions
- none

## Follow-Up Questions
- After the SQL Server prototype is stable, should the next provider example be PostgreSQL staged COPY or MySQL staged bulk to cover a second provider boundary?
- Should adopter-facing documentation later recommend one example repository layout for reviewed manifests and future sidecar SQL files, even though output-path selection stays consumer-owned?
- After the dry-run prototype and evidence ticket land, should a later lane allow deployable sidecar SQL payload emission, or should the artifact lane remain review-only longer?

## Risks
- The SQL Server benchmark rows exist in the checked-in artifact triplet but are currently skipped because the external provider is not configured locally, so this ticket must not be treated as evidence completion or production-ready artifact approval.
- If implementation starts inferring provider/workload evidence when request-bound diagnostics are absent, the manifest could create unreviewed provider-specific claims.
- If developers widen this slice into automatic invocation, deployment, or migration synchronization, they will violate the parent contract already marked done in `06F8KZTNG44XDPMVTVCV4WJSHG`.
- A metadata-only dry-run manifest can be misread as a deployable artifact unless the explicit dry-run indicator and consumer-owned operational boundary stay visible.

## Split Recommendations
- No new split is justified now; the existing epic/parent/evidence/prototype/documentation separation is sufficient for this refinement pass.
- If later work wants provider-matrix coverage, deployable sidecar SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this first prototype.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Prototype a dry-run artifact manifest for one provider and one representative workload. It must not deploy, invoke, register runtime dispatch, or alter EF migrations automatically.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery
- Implemented a design-time `sql-artifact` dry-run manifest lane for the single SQL Server `provider-native-bulk-ingestion` slice.
- The emitted manifest uses schema version `dvault.sql-artifact.v1`, records SQL Server provider identity, `sqlserver-v1`, metadata-source kind/fingerprint, explicit dry-run review status, benchmark triplet references, semantic-parity references, exact workload facts, and empty sidecar payload arrays.
- Generation stays behind `DataVaultDesignTimeCommand` with caller-supplied `--output`; it prefers `DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics` and fails closed unless request-bound diagnostics select `SqlServerDataVaultSaveStrategy` for `Microsoft.EntityFrameworkCore.SqlServer`.

## Verification
- `bash tools/check-format.sh` passed.
- `dotnet build DVault.slnx --nologo --no-restore` passed after a local-only package-cache restore; build warnings were pre-existing integration/test analyzer warnings outside the touched files.
- `dotnet test DVault.slnx --nologo --no-restore --no-build` passed; SQL Server and other external-provider tests remained skipped because their connection-string environment variables are unset.

## Boundary Notes
- No sidecar SQL payload files are emitted by this prototype.
- No runtime dispatch, deployment, automatic cleanup, EF migration mutation, or repository storage convention was added.
<!-- gicket-bot:developer-delivery:v1:end -->