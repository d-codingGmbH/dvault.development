<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this as a repository-backed acceptance contract for future provider bulk expansion on the existing save path: finite supported shapes, caller-owned EF Core transaction semantics, provider-neutral fallback, diagnostics and benchmark gates, and explicit non-goals. No child tickets, relation updates, description updates, attachments, or planning documents were materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current repository baseline already documents provider-specific save-strategy lanes for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2; this ticket only defines the acceptance gate for later bulk expansion or threshold changes, not a new provider-support decision.
- Provider bulk expansion stays inside the existing IDataVaultSaveService and DI-selected IDataVaultProviderSaveStrategy boundary; it does not add a new public runtime API, SaveChanges interception path, or read-model surface.
- A provider implementation ticket may close as no-work when the exact provider and workload cannot preserve current save semantics or cannot produce repository-backed threshold evidence that beats the provider-neutral fallback.

### Scope In
- Define the accepted save-path scope for provider-specific bulk work behind the existing explicit IDataVaultSaveService boundary.
- Define the finite supported shape baseline for optimized bulk candidates: clean EF Core contexts, ordered explicit bulk batches or chunk-internal ordered batches, provider-name match, and ordinary hub/link/satellite operations.
- Define required caller-owned transaction behavior, provider-neutral fallback behavior, diagnostics/telemetry/tracing evidence, and benchmark-threshold evidence for future provider bulk tickets.
- Define that future implementation tickets may close as no-work when the documented gate is not met.

### Scope Out
- Read-strategy expansion, PIT or bridge optimization, latest-satellite work, and live-schema reading.
- SaveChanges interception, background ingestion, CDC, scheduler orchestration, file-ingestion lanes, and automatic strategy routing outside the save-service dispatcher.
- Deployable SQL payloads, stored-procedure deployment, runtime artifact dispatch, migration synchronization, database provisioning, credential handling, environment routing, dashboards, or package publication responsibilities.
- Re-deciding the visible supported-provider save baseline already documented in repository architecture and performance guidance.

## Acceptance Criteria
- The refinement states that provider bulk expansion must remain behind the existing IDataVaultSaveService plus IDataVaultProviderSaveStrategy dispatch and must preserve current caller-visible save semantics.
- The refinement names the finite supported-shape boundary: clean provider context, ordered explicit bulk batch or per-chunk ordered batch, provider-name match, no pending tracked changes, and no multi-active satellite batch support unless a later ticket adds separate repository-backed evidence.
- The refinement makes EF Core transaction ownership explicit: provider-specific bulk execution participates in the caller's current transaction and does not auto-open, commit, roll back, suppress, or background/retry transactions on the caller's behalf.
- The refinement makes fallback explicit: unsupported providers, unregistered providers, declined gates, unsupported shapes, or missing evidence continue through the provider-neutral writer with finite diagnostics and fallback reporting instead of widening scope.
- The refinement requires repository-backed diagnostics evidence for any future provider bulk claim: request-bound save diagnostics must show selected strategy or fallback, provider identity, gate facts, and redacted observability surfaces without raw business data or SQL.
- The refinement requires benchmark-threshold evidence for the exact provider and workload before claiming provider-specific bulk as accepted work; if the measured evidence does not justify a thresholded provider path, the implementation ticket may close as no-work.
- The refinement explicitly excludes deployment and runtime-platform responsibilities from provider bulk acceptance, including artifact deployment, runtime artifact dispatch, migration automation, and operational ownership.

## Definition of Done
- The ticket contract cites the existing architecture and performance documents as the authoritative baseline for explicit save boundaries, diagnostics, fallback, and evidence vocabulary.
- The contract is specific enough that a future developer ticket does not need more PO clarification about supported shapes, transaction ownership, fallback semantics, diagnostics expectations, or benchmark-threshold proof.
- The contract explicitly states the no-work close path when a provider candidate cannot satisfy semantic-parity and evidence gates.
- No acceptance item widens the story into deployment, runtime-platform, read-model, or operational responsibilities.

## Implementation Notes
- Use docs/architecture/dvault-v1-explicit-save-service.md as the primary save-boundary source: provider-specific strategies are DI-selected, the core save service does not branch on provider names, and chunked execution stays inside caller-owned transaction semantics.
- Use docs/performance-profiles.md as the primary evidence-gate source: provider promotion requires request-bound diagnostics, redacted telemetry and activity-tracing surfaces, and explicit stop or fallback handling when evidence is missing, skipped, stale, or unsupported.
- Use benchmark-summary.md only as the quick baseline for row identity and current SQLite completed timing; the checked-in root run keeps PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows as skipped placeholders when connection strings are unset, so it cannot by itself satisfy a new provider-bulk acceptance gate.
- Use docs/releases/v0.32.0.md as the current threshold-evidence pattern: a provider-specific threshold can be accepted only when the exact provider and workload has preserved benchmark artifacts or when the story reuses an already documented provider gate backed by those artifacts.
- Keep provider-specific SQL artifact work separate from this acceptance contract. The visible repository baseline keeps that lane design-time, review-only, and narrower than general provider bulk runtime support.
- This run relied on the provided ticket snapshot, empty comment snapshot, and repository documents already in prompt context; no child tickets, relation updates, description updates, attachments, or planning documents were persisted.

## Open Questions
- none

## Follow-Up Questions
- When a concrete provider expansion ticket is opened, which exact provider/workload comparator and artifact bundle will be the preserved threshold-evidence record for that ticket?
- If a later provider proposal needs both runtime save-strategy work and design-time SQL artifact review work, should those be split into separate tickets to preserve the current runtime-versus-artifact boundary?

## Risks
- Without an explicit evidence gate, later tickets could overclaim provider performance from skipped-placeholder rows or diagnostics-only evidence.
- Without the finite supported-shape boundary, future stories may accidentally absorb dirty-context handling, multi-active satellite support, or read-model work that the current repository baseline treats as fallback or separate scope.
- Without the explicit non-goal statement, provider bulk work can drift into deployment, migration, or operational ownership that the repository documents currently keep consumer-owned or out of scope.

## Split Recommendations
- No additional split is justified for this PO refinement ticket.
- Materialize future implementation work per provider and, when needed, separate runtime save-strategy changes from artifact or deployment review work instead of broadening one provider-bulk ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the narrow acceptance criteria for new provider bulk work: supported shapes, EF Core transaction behavior, provider-neutral fallback, diagnostics, benchmark threshold, and no deployment/runtime-platform responsibilities. Acceptance: provider implementation tickets can be closed as no-work if evidence does not meet the criteria.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- decision: `already_satisfied_on_branch`
- repository change: none
- repository paths validated:
  - `docs/architecture/dvault-v1-explicit-save-service.md`
  - `docs/performance-profiles.md`
  - `docs/releases/v0.32.0.md`
  - `benchmark-summary.md`

### Validation Summary
- The explicit save service document already keeps provider bulk work behind `IDataVaultSaveService` plus `IDataVaultProviderSaveStrategy`, preserves caller-owned transaction behavior, documents provider-neutral fallback, names clean-context/provider-name/multi-active gate conditions, and excludes runtime dispatch, deployment automation, and default `SaveChanges` interception from the acceptance surface.
- The performance profile already defines staged provider ingestion as diagnostics-gated work for clean eligible ordered batches, with stop or fallback handling for skipped or missing evidence, dirty contexts, provider-name mismatch, unsupported multi-active satellite batches, declined strategy gates, threshold failures, and missing benchmark evidence.
- The v0.32.0 release notes already preserve the evidence pattern for exact provider/workload claims: request-bound diagnostics, benchmark artifact triplet, semantic parity review, skipped optional-provider rows, and consumer-owned deployment, migration, and operational responsibilities.
- The root benchmark summary already keeps provider-native bulk-ingestion row identity visible, including skipped optional-provider placeholders for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 when connection strings are unset.

### Verification
- `git ls-files -- docs/architecture/dvault-v1-explicit-save-service.md docs/performance-profiles.md docs/releases/v0.32.0.md benchmark-summary.md`
- `rg -n "IDataVaultSaveService|IDataVaultProviderSaveStrategy|provider-neutral writer|caller-owned transaction|pending tracked changes|multi-active satellite|benchmark artifact triplet|request-bound diagnostics|runtimeDispatch=not-generated|provider-native-bulk-ingestion" docs/architecture/dvault-v1-explicit-save-service.md docs/performance-profiles.md docs/releases/v0.32.0.md benchmark-summary.md`
- Policy validation, if required by the next role: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh`.

### Open Questions
- none
<!-- gicket-bot:developer-delivery:v1:end -->