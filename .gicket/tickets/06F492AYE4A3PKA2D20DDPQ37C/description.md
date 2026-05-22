<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded opt-in SaveChanges guard interceptor slice for DVault hub/link/satellite direct-write misuse, with no planning writes or relation changes materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes IDataVaultSaveService as the default write boundary, UseDataVaultSaveChangesMetadataInterceptor(...) as an optional metadata-only EF lane, and ordinary EF SaveChanges as something consumers should keep separate from DVault persistence unless they deliberately own generated DVault rows.
- This story should use a new opt-in EF SaveChanges interceptor mode as the implementation lane, not a separate design-time command or broad validation-hook platform.
- The first bounded guard scope is generated DVault hub, link, and satellite entities only; PIT and bridge maintenance stay on their explicit services and are not part of this slice.
- When guard and metadata-fill interceptors are both enabled, guard evaluation must consider the post-fill state so missing LoadTimestamp or RecordSource values that are intentionally auto-populated are not treated as violations.
- Current persisted relations remain one incoming parentOf from epic 06F492A3MPSGP3KXDNZECN01QM and one outgoing blocks to docs task 06F492BNDPWS9P4EDSV0W7G6VM; no relation changes, child tickets, description updates, attachments, or planning documents were materialized in this pass.

### Scope In
- Add explicit opt-in DbContextOptionsBuilder registration for a runtime guard interceptor mode that is off by default and separate from AddDVault().
- Detect high-confidence unsafe direct SaveChanges patterns on generated DVault hub, link, and satellite rows, especially direct Modified or Deleted states and Added rows missing required DVault-owned structural values after any configured metadata fill.
- Support at least blocking and warning modes with deterministic explanations consumers can inspect or surface.
- Preserve the documented caller-owned generated-row lane where the row already has required structural data and only relies on optional metadata fill for LoadTimestamp and or RecordSource.
- Add unit and integration coverage for default non-registration, guard findings, warning and block behavior, metadata-fill coexistence, and explicit save-service compatibility.

### Scope Out
- No default guard registration on AddDVault() or provider package startup.
- No hash computation, hash-diff computation, row creation, ordering, or replacement of IDataVaultSaveService.
- No PIT or bridge runtime guard coverage in this first slice.
- No analyzer implementation, design-time preflight command work, migration or drift guardrails, or logging platform integration.
- No broad business or payload validation beyond high-confidence DVault structural guard checks.

## Acceptance Criteria
- A consumer can opt a DbContext into runtime guard behavior through new explicit DbContextOptionsBuilder API(s), and the existing default AddDVault() path still registers no runtime guard interceptor.
- In block mode, direct SaveChanges on generated DVault hub, link, or satellite entries that are in Modified or Deleted state, or Added entries that still lack required non-fillable DVault structural values, fails with a deterministic explanation of the offending entries and reasons.
- In warning mode, the same findings are emitted through a deterministic caller-facing explanation surface without silently mutating the tracked rows or requiring a logging dependency.
- When UseDataVaultSaveChangesMetadataInterceptor(...) is also configured, rows that are otherwise valid and only rely on interceptor-populated LoadTimestamp or RecordSource are not reported as unsafe.
- IDataVaultSaveService continues to work unchanged as the default write boundary under the guard configuration, and documented direct caller-owned generated-row scenarios that already supply required structural data continue to save successfully.
- Detection relies on DVault EF annotations and roles rather than hard-coded table or property names, so effective-name overrides and generated shared-type tables remain supported.

## Definition of Done
- Public API snapshot coverage reflects the new runtime guard options, mode or report surface, and DbContextOptionsBuilder opt-in extension methods.
- Unit tests prove default non-registration, blocking and warning decisions, deterministic explanation content, and annotation-driven detection.
- SQLite integration tests prove coexistence with the metadata-fill interceptor, safe caller-owned generated-row saves, and guard failures for unsafe tracked DVault hub, link, and satellite mutations.
- Tests prove the explicit IDataVaultSaveService path still succeeds under the opt-in guard and remains the documented default write boundary.
- The final docs-facing contract remains truthful that this is an optional runtime guardrail, not an implicit persistence model or replacement for analyzers or preflight.

## Implementation Notes
- Reuse the existing DbContextOptionsBuilder interceptor opt-in pattern established by UseDataVaultSaveChangesMetadataInterceptor(...).
- Build guard decisions from DataVaultAnnotationNames.EntityKind, DataVaultAnnotationNames.PropertyRole, and DataVaultAnnotationNames.TechnicalColumnRole so generated-name overrides and shared-type tables keep working.
- Keep the first slice narrowly focused on hub, link, and satellite SaveChanges misuse; treat PIT and bridge maintenance as later work if needed.
- Use a deterministic structured explanation surface for warn and block behavior instead of introducing a mandatory ILogger or telemetry dependency, since current repository telemetry is scoped to explicit save and read services.
- Guard findings should be based on high-confidence DVault structural invariants: modified or deleted tracked DVault write rows are unsafe, and added rows are unsafe only when required DVault-owned structural values remain missing after any metadata-fill pass.
- No planning writes were materialized in this pass.

## Open Questions
- none

## Follow-Up Questions
- After this slice lands, should PIT and bridge explicit maintenance paths get their own opt-in runtime guard story rather than broadening this story now?
- Should later docs and analyzer work align warning and error wording between this runtime guard story and analyzer story 06F492ARW2N6SNYJH15RHMZEN8?
- If consumers want built-in console or logging integration for warning mode later, should that be a separate observability ticket rather than expanding the core interceptor contract?

## Risks
- If guard evaluation runs before metadata-fill behavior or ignores companion interceptor state, it will produce false positives for the documented caller-owned generated-row lane.
- If the guard tries to validate too much business or payload shape instead of high-confidence DVault structural invariants, it will become noisy and undermine the opt-in safety goal.
- If the explanation surface is only exception text with no deterministic structure, docs and tests will struggle to keep warning and blocking modes aligned.

## Split Recommendations
- No split recommended for this story; current repository evidence supports one bounded runtime guard slice around opt-in hub, link, and satellite SaveChanges misuse detection plus deterministic warning and blocking explanations.
- If future work expands into PIT or bridge guard coverage, richer observability sinks, or analyzer and runtime diagnostic unification, split that into follow-up tickets instead of widening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Provide an opt-in runtime guard mode using EF Core interception or validation hooks to catch unsafe direct SaveChanges patterns around DVault-managed entities. It must be off by default and explainable when it blocks or warns.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Added an explicit opt-in `UseDataVaultSaveChangesGuardInterceptor(...)` DbContextOptionsBuilder API for DVault generated-row SaveChanges guard behavior.
- Added blocking and warning modes with structured `DataVaultSaveChangesGuardReport` / finding output and a blocking exception carrying the report.
- Kept `AddDVault()` unchanged and interceptor-free by default.
- Guard detection is annotation-driven for generated hub, link, and satellite rows and excludes PIT/bridge scope.

Verification
- `dotnet build DVault.slnx --nologo --no-restore` passed with warnings only.
- `dotnet test DVault.slnx --nologo --no-build --no-restore` passed: integration reported 165 total, 149 succeeded, 16 skipped for unconfigured external providers; unit reported 346 succeeded.
- `bash tools/check-format.sh` passed.

Notes
- Warning mode reports through the configured callback and does not mutate tracked rows.
- Blocking mode throws before persistence when findings exist.
- Metadata-fill coexistence is covered by SQLite tests; missing LoadTimestamp and RecordSource values are not treated as non-fillable structural guard failures.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Added direct unit coverage for `DataVaultSaveChangesGuardInterceptor` decision behavior, including blocking findings, warning callback emission without row mutation, annotation-driven detection, and PIT/bridge exclusion.
- Exposed the interceptor evaluation method internally so unit tests can exercise the decision path without depending on SQLite persistence as the only proof.
- Preserved the existing opt-in guard API, warning/blocking behavior, metadata-fill coexistence, and explicit save-service compatibility from the earlier implementation.

Verification
- `dotnet build DVault.slnx --nologo` passed with warnings only.
- `dotnet test DVault.slnx --nologo` passed: integration reported 165 total, 149 succeeded, 16 skipped for unconfigured external providers; unit reported 349 total, 349 succeeded.
- `bash tools/check-format.sh` passed.

Notes
- The tester-returned unit DoD gap is addressed in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultSaveChangesGuardInterceptorDecisionTests.cs`.
- External provider integration tests remain opt-in through provider connection-string environment variables.
<!-- gicket-bot:developer-delivery:v1:end -->