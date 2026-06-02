<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Persisted delivery contract already matches bounded repository and ticket evidence, so no additional PO materialization was needed; the current split and live relations remain intact and the story is ready for PO-critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The persisted delivery contract already serves as the authoritative handoff surface for this story, so no ticket-description update was needed in this run.
- Repository evidence already fixes the bounded recommendation baseline to the four checked-in performance-profile categories: Small app-local vault, Medium chunked ingestion, Staged provider ingestion, and Read-model heavy.
- Read diagnostics stay anchored to the existing request-bound explainability contract and vocabulary: supported read kinds `LatestSatellite`, `PitAsOf`, and `Bridge`, existing read-strategy statuses and fallback causes, and activity-tracing read modes `Current`, `AsOf`, and `Traversal`.
- The live relation state already preserves the approved split: this story blocks verifier story `06F7Y0K95VW0PX21F6R2YGP8DM` and documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4`, and remains under parent `06F7Y0J8PRFRSSWZ3GGT91S0TW`; no relation cleanup was required.
- Historical contract story `06F7Y0JQ2FZQZVTNFX2T25DAS4` remains background contract evidence rather than new implementation scope.
- No child tickets, attachments, or planning documents were created because the referenced repository documents and current persisted contract already cover the necessary PO refinement evidence.

### Scope In
- Add additive request-bound provider tuning diagnostics on `DataVaultDiagnosticsResult` and deterministic support-bundle serialization for save and read analysis.
- Surface save-path selected-strategy facts, candidate eligibility, supported provider names, gate requirements, finite fallback causes, staged-provider bulk caveats, and evidenced threshold guidance where the repository already proves it.
- Surface read-path selected-strategy and fallback facts plus `ReadShape.Provider` recommendation context for `LatestSatellite`, `PitAsOf`, and `Bridge`.
- Add or update tests for selected, declined, fallback, unsupported-provider, serialization, redaction, and omission behavior for the new diagnostics fields.

### Scope Out
- Automatic batch-size tuning, strategy switching, deployment, migration, or DBA workflow automation.
- New save or read strategy implementations, benchmark reruns, or benchmark artifact schema changes.
- Raw SQL, query-plan capture, credentials, connection strings, provider exception text, stack traces, or workload-value serialization.
- Numeric read-threshold guidance or optimized read claims beyond the current repository evidence, including any non-SQLite optimized read posture.

## Acceptance Criteria
- `DataVaultDiagnosticsResult` exposes additive request-bound provider tuning diagnostics for save and read paths without changing strategy-selection behavior.
- Save diagnostics surface selected strategy name and priority when applicable, candidate eligibility, supported provider names, finite gate requirements, finite fallback causes, staged-provider bulk caveats, and the evidenced threshold facts already visible in source: SQL Server minimum `50` total operations and maximum `500` satellite operations, MySQL minimum-operation gate, and Oracle minimum `50` total operations and maximum `10000` satellite operations.
- Read diagnostics surface selected-strategy facts, candidate eligibility, finite fallback causes, and `ReadShape.Provider` facts for `LatestSatellite`, `PitAsOf`, and `Bridge`, with SQLite remaining the only repository-proven optimized read provider path.
- Recommendation output uses a closed machine-readable category set with bounded human messages and only the four checked-in performance-profile categories; non-applicable profile or recommendation fields are omitted.
- Deterministic serialized diagnostics and support-bundle output keep camelCase output, preserve redaction and omission rules, and omit non-applicable optional fields such as selected strategy, threshold facts, profile, or recommendation when they do not apply.
- Tests cover selected, declined, fallback, and unsupported provider cases for save and read flows, plus serialization or redaction coverage for the new provider-tuning fields.

## Definition of Done
- The current ticket description remains the authoritative implementation contract for provider eligibility, threshold, and recommendation diagnostics.
- The implementation reuses the existing diagnostics and tracing vocabulary instead of inventing parallel provider-tuning names for status, fallback, or read kinds.
- Public API or contract snapshot coverage and deterministic serialization coverage are updated for any newly exposed diagnostics surface.
- The implementation leaves no blocking ambiguity for verifier story `06F7Y0K95VW0PX21F6R2YGP8DM` or documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4` about supported thresholds, profile categories, redaction, or omission rules.
- The implementation does not overstate unsupported provider read or write behavior and keeps non-applicable optional fields absent.

## Implementation Notes
- Prefer additive structured sections on `DataVaultDiagnosticsResult` rather than a separate free-form blob or parallel result type.
- Reuse `DataVaultSaveStrategyDiagnostics`, `DataVaultReadStrategyDiagnostics`, `ReadShape.Provider`, `DataVaultStagedProviderBulkDiagnostics`, and `DataVaultSupportBundleExporter`; recommendation logic should derive from existing structured diagnostics and checked-in profile evidence rather than parsing display strings.
- Anchor profile mapping to `docs/performance-profiles.md` and the benchmark artifact contract, but keep raw benchmark timings and workload values out of diagnostics payloads.
- Save-path guidance should remain bounded to visible gate facts: dirty-context, provider-name mismatch, multi-active and staged-provider caveats, SQL Server minimum `50` total operations and maximum `500` satellite operations, MySQL minimum-operation gate, and Oracle minimum `50` total operations and maximum `10000` satellite operations.
- Read-path guidance should remain profile-based over `LatestSatellite`, `PitAsOf`, and `Bridge`; non-SQLite or unsupported shapes should resolve to provider-neutral guidance or omitted recommendation fields.
- Preserve the redaction and omission rules already evidenced by the read-plan explain contract: no raw keys, timestamps, as-of values, SQL text, query plans, credentials, connection strings, provider exception text, stack traces, or workload metrics.
- Keep the current split unchanged: this ticket implements the diagnostics surface, `06F7Y0K95VW0PX21F6R2YGP8DM` owns benchmark-artifact verification, and `06F7Y0NBHXQ6CK8R3AH4DEP9V4` owns documentation alignment.

## Open Questions
- none

## Follow-Up Questions
- After implementation lands, should a follow-up ticket add explicit cross-links from each recommendation category to the matching `docs/performance-profiles.md` section and verified benchmark artifacts?
- When dedicated read-path benchmark evidence exists, should a follow-up ticket add finite numeric read-threshold guidance for provider-specific read recommendations instead of the current profile-only mapping?

## Risks
- Recommendation mappings can drift from the checked-in profile and benchmark baseline unless the closed category set stays anchored to those documents.
- Read guidance can overpromise provider-specific behavior if implementation turns profile hints into non-SQLite optimized read claims that the repository does not prove.
- Redaction can regress if provider exception text or workload values leak into serialized diagnostics instead of staying behind finite fallback messages and omitted optional fields.

## Split Recommendations
- Keep the current split unchanged: historical contract story `06F7Y0JQ2FZQZVTNFX2T25DAS4` remains background evidence, this story owns the diagnostics implementation, `06F7Y0K95VW0PX21F6R2YGP8DM` owns verification, and `06F7Y0NBHXQ6CK8R3AH4DEP9V4` owns documentation.
- If the team later wants new benchmark profiles, provider-specific read thresholds, attachment-backed support material, transport or reporting surfaces, or automatic tuning behavior, create separate follow-up work instead of widening this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Expose bounded diagnostics that explain provider strategy eligibility and tuning recommendations for supported save/read paths.

# Scope In
- Add provider-specific eligibility reasons and threshold guidance where benchmark evidence exists.
- Explain why optimized strategies are selected, declined, or falling back.

# Scope Out
No automatic batch-size tuning, stored-procedure switch, query-plan parsing, or provider deployment action.

# Acceptance Criteria
- Tests cover selected, declined, fallback, and unsupported provider cases.
- Output links naturally to performance profiles and remains redacted.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Implemented additive request-bound provider tuning diagnostics on `DataVaultDiagnosticsResult`.
- Added bounded read-shape provider recommendation context and deterministic support-bundle serialization coverage.
- Added closed profile-category output for `SmallAppLocalVault`, `MediumChunkedIngestion`, `StagedProviderIngestion`, and `ReadModelHeavy`, plus save threshold facts for known provider strategy gates.

Verification
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed: integration `189` succeeded and `21` external-provider tests skipped; unit `414` succeeded.
- `bash tools/check-format.sh` passed.

Notes
- Verification emitted NU1900 warnings because the NuGet vulnerability metadata cache is read-only in this sandbox; the warnings did not fail build or test execution.
<!-- gicket-bot:developer-delivery:v1:end -->