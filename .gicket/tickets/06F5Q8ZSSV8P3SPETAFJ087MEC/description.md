<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined against the current repo baseline: MySQL already has a dual-provider set-based optimized path, v0.20.0 staged bulk work should stay additive behind that boundary, the shared staging-contract prerequisite is done, and no child-ticket, relation, description, attachment, or planning-document writes were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified persisted context: this story is a child of epic `06F5Q8YBVRS2EZVMJK5EATV9AR`, it preserves a historical incoming `blocks` relation from done story `06F5Q8YKR31DXGRXVPJ9031BQW`, it is not currently blocked, and it blocks benchmark story `06F5Q900FC0P3HBZP81CVK7264`.
- Verified comment context: there were no human comments or attachments to incorporate, only bot claim/lease comments.
- Repository evidence already fixes the current MySQL baseline: `AddDVaultMySql()` and `MySqlDataVaultSaveStrategy` support both `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`, require a clean context with no multi-active satellites, enforce the current 50-operation native gate, and fall back through the provider-neutral writer when they decline.
- The root benchmark triplet already emits MySQL `provider-native-bulk-ingestion` rows and keeps them visible as skipped when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is not configured; this ticket should reuse that evidence contract instead of inventing a new artifact format.
- No child tickets, relation cleanup, description updates, attachments, or planning documents were applied in this refinement pass.

### Scope In
- Evaluate staged MySQL execution only behind the existing `IDataVaultSaveService` and provider-strategy boundary.
- Define and implement the decision boundary between the current MySQL multi-row optimized path, staged execution, and provider-neutral fallback for eligible ordered hub, link, and ordinary satellite batches.
- Preserve current caller-visible semantics for transactions, cancellation, ordering, hash-key/hash-diff rules, idempotent unique-row behavior, and latest-state satellite checks.
- Add MySQL-gated external integration coverage and performance evidence that make the selected MySQL path observable when the provider is configured.
- Keep deterministic diagnostics for unsupported or unproven provider/provider-version combinations and unsupported request shapes.

### Scope Out
- Any new public save API or widening beyond the existing `IDataVaultSaveService` contract.
- Cross-provider staged bulk implementation work for PostgreSQL, SQL Server, or Oracle.
- Benchmark artifact schema redesign or cross-provider matrix policy work already owned by `06F5Q900FC0P3HBZP81CVK7264`.
- Stored-procedure positioning and broader staged-bulk documentation rollout already owned by `06F5Q90718D21DN1N1Q2AP7YEM`.
- Unrelated MySQL live-schema, capability-profile, or read-path work.

## Acceptance Criteria
- The MySQL provider-specific save path records and enforces a deterministic decision boundary among the existing multi-row optimized inserts, staged bulk execution, and provider-neutral fallback.
- Staged execution runs only for supported MySQL provider contexts and request shapes; unsupported or unproven provider/provider-version combinations and unsupported shapes decline with deterministic diagnostics instead of silently attempting staging.
- When selected, the staged path preserves current `IDataVaultSaveService` semantics for caller-owned transaction/cancellation behavior, deterministic saved-record ordering, hash-key/hash-diff computation, unique-row idempotency, latest-state satellite filtering, and staging cleanup on success, failure, and cancellation.
- MySQL external integration tests prove the supported staged path and at least one deterministic decline or fallback path when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is configured, and retain deterministic skips when it is not.
- Performance evidence follows `docs/plans/performance-evidence-benchmark-artifact-contract.md`: before/after benchmark-summary triplets use comparable inputs, MySQL rows stay visible when skipped, and `executionDetail` makes the selected MySQL path visible enough to distinguish staged execution from the current multi-row path.

## Definition of Done
- Touched code, diagnostics, and tests make it observable whether a MySQL batch used staged execution, stayed on the current multi-row optimized path, or fell back.
- The supported MySQL staged path and deterministic decline behavior are covered by automated tests within the existing MySQL opt-in integration lane and any necessary local smoke or unit coverage.
- Benchmark evidence or checked-in artifacts for the ticket follow the shared benchmark triplet contract and preserve the MySQL selected-path detail needed for downstream release-note and benchmark-matrix work.
- No public API widening or regression of caller-owned transaction, cancellation, ordering, or fallback semantics is introduced.

## Implementation Notes
- Treat done story `06F5Q8YKR31DXGRXVPJ9031BQW` as the settled shared staging SPI and transaction baseline; this ticket should implement the MySQL-specific path, not reopen the public contract.
- Reuse the repository-visible MySQL opt-in harness already wired through `MySqlProviderReflection`, `MySqlExplicitDataVaultSaveServiceTests`, the benchmark provider availability path, and `DVAULT_TEST_MYSQL_CONNECTION_STRING`.
- Keep selection observable through existing diagnostics surfaces such as `DataVaultProviderSaveStrategyGateEvaluator`, `DataVaultSaveTelemetryExplanation`, and benchmark `executionDetail` rather than introducing a parallel reporting path.
- The visible repo baseline already ratifies both MySQL provider names as supported for the current optimized strategy, but several test names and diagnostics still speak in Pomelo-only or ambiguously named terms; update touched tests and diagnostics to match the dual-provider baseline rather than reintroducing a Pomelo-only contract.
- Do not weaken or silently replace the current non-staged MySQL optimized path; staging is additive only when evidence shows it is preferable or otherwise required for a supported shape.
- Reuse the existing provider-native bulk-ingestion benchmark lane and shared artifact contract instead of creating a MySQL-specific benchmark schema or attachment format.

## Open Questions
- none

## Follow-Up Questions
- If staged execution depends on provider APIs available only in one MySQL EF provider, should Pomelo live-proof or parity work move to a separate follow-up ticket instead of widening this story?
- After MySQL staging lands, should `06F5Q90718D21DN1N1Q2AP7YEM` publish a provider-by-provider staged support matrix and stored-procedure boundary summary?
- Should `06F5Q900FC0P3HBZP81CVK7264` add an explicit MySQL multi-row-versus-staged comparison row once the selected MySQL path stabilizes?

## Risks
- If staging cleanup or transaction-participation behavior differs between Pomelo and official MySQL providers, a naive shared implementation could regress the current dual-provider contract.
- If diagnostics and benchmark evidence do not clearly distinguish staged selection from the existing MySQL multi-row path, supportability and performance claims will stay ambiguous.
- If staged evaluation overreaches beyond provider-supported shapes, the implementation could replace a proven optimized path with a less reliable one.

## Split Recommendations
- No additional split is recommended for refinement: the shared staging-contract work is already done in `06F5Q8YKR31DXGRXVPJ9031BQW`, benchmark-matrix follow-up is already split into `06F5Q900FC0P3HBZP81CVK7264`, and broader docs rollout is already split into `06F5Q90718D21DN1N1Q2AP7YEM`.
- If later evidence shows Pomelo and official MySQL providers need materially different staged implementations or live-proof lanes, create a provider-specific follow-up ticket then rather than widening this story now.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Evaluate and implement a MySQL staged bulk path only for provider-supported shapes with clear evidence.

Acceptance criteria:
- Records when multi-row inserts remain preferable and when staging is selected.
- Declines unsupported Pomelo/MySql provider combinations with deterministic diagnostics.
- Adds MySQL-gated integration tests and benchmark evidence when configured.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Implemented additive MySQL staged bulk dispatch through `MySqlStagedDataVaultSaveStrategy`, registered ahead of the existing MySQL multi-row strategy.
- Preserved the existing `IDataVaultSaveService` public boundary and kept provider-neutral fallback when staged and multi-row candidates decline.
- Updated diagnostics and benchmark `executionDetail` so MySQL staged selection is distinguishable from the existing multi-row path.

Evidence
- `dotnet build DVault.slnx --nologo` passed. The run reported existing NU1900 vulnerability-cache warnings because the NuGet HTTP cache is read-only in this sandbox.
- `dotnet test DVault.slnx --nologo --no-build` passed; external MySQL tests stayed deterministically skipped because `DVAULT_TEST_MYSQL_CONNECTION_STRING` is not configured.
- `bash tools/check-format.sh` passed.

Notes
- The checked-in root benchmark triplet keeps the MySQL provider-native row visible while skipped and now records the planned staged selected strategy for the MySQL optimized row.
- Live execution of the staged path remains covered by the existing opt-in MySQL integration lane when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is configured.
<!-- gicket-bot:developer-delivery:v1:end -->