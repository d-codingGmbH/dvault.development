<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story as an additive diagnostics and support-bundle contract expansion over the existing provider capability and strategy explainability baseline; no ticket mutations were needed because the current epic and blocker relations already match the scope.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Existing repository evidence already defines the baseline: DataVaultDiagnosticsResult exposes capability profile name, provider behavior profile name, selected strategy status and name, candidate ordering, and finite fallback-cause enums and messages; this story expands that bounded explain surface instead of creating a second classification system.
- Current live relations already place this story under epic 06F492A3MPSGP3KXDNZECN01QM and as a blocker for 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV0W7G6VM, so this ticket should own the reusable diagnostics contract those downstream stories consume.
- The current provider-behavior baseline remains provider-neutral-v1; refinement should expose current provider capability and dispatch rules without inventing new provider-behavior implementations.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this pass because the existing persisted structure already matches the refined scope.

### Scope In
- Add additive machine-readable explanation fields on diagnostics and support-bundle surfaces for selected provider capability profile details: profile name and defaulted state, provider name, load-timestamp and snapshot-reference storage and value formats, relevant type-mapping facts, identifier-length behavior, included-index fallback behavior, and declared SQL-function and concurrency posture.
- Add additive machine-readable explanation fields for provider-specific save and read strategy eligibility and fallback behavior, including candidate strategy name, priority, evaluation order, supported provider names, and bounded gate reasons and thresholds already enforced by the implementation.
- Update concise human-readable diagnostics rendering only as needed so ToDisplayString surfaces the expanded provider and strategy explanation without raw SQL or unbounded internal detail.
- Add tests and snapshots for diagnostics analysis and support-bundle serialization that cover selected-strategy and provider-neutral fallback scenarios across the request families already supported by the repository baseline.

### Scope Out
- Changing actual runtime strategy-selection behavior, save behavior, or read behavior.
- Query-shape diagnostics, preflight command aggregation, and documentation or release-note rollout, which remain in the already-linked blocked tickets.
- Raw SQL text, exception payloads, connection details, or other unredacted or high-cardinality support-bundle output.
- New provider-behavior profiles or benchmark-driven recommendation engines beyond the current declared capability and gate baseline.

## Acceptance Criteria
- DataVaultDiagnosticsResult and support-bundle JSON gain additive provider explain fields that tell consumers which capability profile was used, whether it defaulted, how DVault maps load timestamps and snapshot references, and which bounded provider limitations are declared.
- Save diagnostics explain output reports candidate strategy order and priority, the selected strategy when one is used, or finite fallback causes when provider-neutral fallback is chosen, reusing the current enum and message vocabulary instead of inventing a second taxonomy.
- Read diagnostics do the same for latest or as-of satellite, PIT, and bridge requests, including the current SQLite-only optimized read-shape limits and unsupported-shape reasons when fallback occurs.
- The explanations surface current documented tuning gates from the authoritative implementation baseline, including dirty-context, multi-active, provider mismatch, unknown-provider, SQL Server minimum 50 total operations and maximum 500 satellite operations, MySQL minimum 50 total operations, Oracle minimum 50 total operations, and current SQLite optimized read-shape constraints.
- The expanded explanation remains additive, deterministic, and redacted: no raw SQL, no hash keys, no record sources, no exception text, no connection secrets, and stable ordering suitable for support-bundle export and automated tests.
- Integration or unit coverage proves the expanded output for at least one selected-strategy case and one provider-neutral fallback case for both save and read diagnostics.

## Definition of Done
- Public diagnostics and support-bundle contract additions compile and preserve existing consumers except for additive API or JSON shape growth.
- Automated tests cover capability explain output, selected strategy output, and fallback output across the supported request families touched by this story.
- ToDisplayString and support-bundle export remain deterministic and bounded.
- Downstream tickets can consume the documented fields without reopening provider or source-code questions.

## Implementation Notes
- Reuse DataVaultProviderCapabilityProfile, DataVaultProviderBehaviorProfile, DataVaultProviderSaveStrategyGateEvaluator, DataVaultProviderReadStrategyGateEvaluator, and the existing fallback enums and messages as the authoritative source of explain data so the contract cannot drift from actual dispatch behavior.
- Keep the main contract centered on DataVaultDiagnosticsResult and DataVaultExplainDiagnostics plus additive strategy-explanation members, because support-bundle export already serializes those sections.
- Preserve the current v0.16 strategy explainability boundary: bounded classifications only, no telemetry-only taxonomy, and no raw SQL or exception-text exposure.
- Treat the currently visible repository facts as the v1 default baseline: built-in capability profiles for SQLite, Postgres, SQL Server, Oracle, and MySQL; SQL-function and concurrency support presently declared as NoneInV1Unsupported; MySQL maximum identifier length 64; Oracle disallowing secondary indexes fully covered by the primary key; and MySQL ignoring unsupported included-index columns.
- Treat the current read optimization baseline as SQLite-only, while save optimization exists for SQLite, Postgres, SQL Server, Oracle, and MySQL with the documented gates already reflected in README and diagnostics tests.

## Open Questions
- none

## Follow-Up Questions
- Should a later story add consumer-facing recommendation text layered on top of the bounded machine-readable facts, or is structured diagnostics plus documentation sufficient?
- After this lands, should the preflight and support-bundle workflow accept representative request fixtures by convention so save and read strategy explain sections are easier to populate in CI evidence?
- Do future provider-behavior profiles need to become richer than the current provider-neutral-v1 name-only baseline, or should provider behavior stay implicit until a concrete hook requires more structure?

## Risks
- Because support-bundle JSON already ships the diagnostics sections, expanding explain output is a contract-sensitive change that needs additive-only evolution and deterministic ordering.
- If provider capability or gate descriptions are duplicated instead of derived from existing profiles and evaluators, the explain output can drift from actual dispatch behavior.
- This ticket remains a prerequisite for 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV0W7G6VM; underspecified output here would force those stories to infer provider behavior from source code again.

## Split Recommendations
- No additional split is required at PO refinement time; keep this story focused on the reusable diagnostics and support-bundle contract, while downstream consumption remains in 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV0W7G6VM.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Extend capability explanations so consumers can understand provider-specific behavior, missing capabilities, fallback reasons, and relevant tuning choices without reading provider source code.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Implemented the additive diagnostics contract expansion for provider capability facts and strategy explain output.
- Kept the public constructor surface compatible while adding init-only explain properties and refreshed the approved core API snapshot.
- Added tests for support-bundle serialization and selected/fallback diagnostics across save, latest/as-of read, PIT read, and bridge read paths.

### Verification
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo --no-build` passed: integration `159` total, `143` succeeded, `16` skipped for missing external provider configuration; unit `340` succeeded.
- `bash tools/check-format.sh` passed.

### Notes
- Restore/build emitted `NU1900` warnings because the sandbox could not update the NuGet vulnerability HTTP cache under the read-only home cache path.
- External Postgres, SQL Server, MySQL, and Oracle integration tests remained skipped because their opt-in connection-string environment variables were not configured.
<!-- gicket-bot:developer-delivery:v1:end -->