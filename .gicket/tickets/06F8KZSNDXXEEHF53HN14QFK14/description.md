<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the existing shared SQLite/PostgreSQL quickstart boundary: reuse the checked-in customer-profile flow, add one bounded diagnostics-aware realistic example pass plus README guidance, and keep SQLite as the default runnable lane. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Recent comments and closure evidence are empty in the ticket snapshot, so no newer human direction overrides the repository baseline.
- Repository evidence already fixes the v1 examples boundary to the two runnable quickstarts under `examples/` that share `examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs`; this ticket should extend that surface rather than introduce a broader sample family.
- The default scenario should stay on the existing customer-profile history domain: one `Customer` hub, one `CustomerProfile` satellite, two ordered profile-state changes, explicit `IDataVaultSaveService` writes, and typed latest/as-of reads through `IDataVaultReadService`.
- SQLite is the authoritative no-external-infrastructure runnable baseline; PostgreSQL remains the same scenario behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING` and must stay optional.
- The example's observability and diagnostics layer is bounded to request-level DVault diagnostics plus opt-in telemetry/tracing guidance already documented in the repository; it must not imply dashboards, exporters, collectors, hosting, schedulers, automatic PIT/bridge maintenance, or runtime routing changes.
- Existing ticket reads in this session already established the parent epic `06F8KZQNH8CCMTJW9P95W1N388` and downstream release-documentation ticket `06F8KZSYCVZ21MS983501BZG18` context; no relation changes were applied.

### Scope In
- Update the existing quickstart surface under `examples/` so one compact realistic EF Core plus DVault scenario is visible in checked-in code without introducing multiple unrelated domains.
- Keep the existing metadata-first customer-profile history flow and make explicit load timestamps, record source, hub/satellite intent, and typed latest/as-of read behavior visible.
- Show at least one bounded diagnostics check for the scenario, using the current redacted DVault diagnostics surfaces rather than ad hoc provider output.
- Update `examples/README.md` so adopters can find the scenario, understand the SQLite-first and PostgreSQL-optional provider story, and see the v0.31 observability/non-goal boundaries.

### Scope Out
- Adding a new web app, hosted worker, scheduler, dashboard, exporter, collector, or platform sample.
- Expanding the quickstart into PIT/bridge maintenance, typed read-model generator, stored-procedure artifact, or broad production tutorial coverage beyond brief pointers to existing docs.
- Changing DVault public APIs, runtime behavior, provider strategy gates, or repository-wide documentation contracts to fit the example.
- Introducing extra provider-specific runnable projects beyond the current SQLite and PostgreSQL quickstarts unless a separate ticket explicitly expands the examples boundary.

## Acceptance Criteria
- The checked-in example reuses the existing quickstart surface and demonstrates one realistic customer-profile history flow end to end with explicit saves and typed latest/as-of reads.
- The runnable SQLite path remains the default proof, and the docs make clear that PostgreSQL reuses the same flow behind the existing environment-variable gate.
- The example code or accompanying README shows how to inspect at least one bounded DVault diagnostics surface for the scenario without exposing raw SQL, request keys, connection strings, business keys, hash keys, payload values, or provider message text.
- `examples/README.md` explains what the scenario demonstrates, how to run it, and the v0.31 guardrails/non-goals: no hosted observability stack, no automatic PIT/bridge maintenance or orchestration, and no new runtime routing promises.
- All changes stay within the existing examples/documentation boundary and remain compatible with the repository's normal build/test expectations without committing generated runtime artifacts.

## Definition of Done
- `dotnet build DVault.slnx --nologo` passes after the example changes.
- The SQLite quickstart run path remains executable, and the PostgreSQL quickstart still skips cleanly when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset.
- README/example wording stays aligned with the current repository terminology for `AddDVault()`, `AddDVaultTelemetry()`, `IDataVaultSaveService`, `IDataVaultReadService`, and the listener-driven `DCoding.Data.DVault` ActivitySource.
- No temporary databases, benchmark artifacts, support bundles, or other generated outputs from running the example are committed.

## Implementation Notes
- Prefer extending `examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs` and the current quickstart entry points so SQLite and PostgreSQL keep one shared scenario.
- Reuse the existing customer-profile baseline already reflected in `examples/README.md`, the root README quickstart, and `docs/plans/customer-profile-comparison-contract.md` instead of inventing a new domain.
- If runnable diagnostics output is added, keep it to stable redacted facts such as selected strategy/status, fallback presence, or read-shape category; do not print raw SQL, credentials, full diagnostic text, or provider messages.
- Keep observability guidance README-level and link to `docs/performance-profiles.md` and `docs/architecture/dvault-v1-activity-tracing-contract.md` rather than duplicating new contracts in the example.
- Inspected branch evidence shows no current diff in `examples/` or the related README/performance-guidance surfaces relative to the supplied scratch source, so this ticket should be implemented as new example/documentation work rather than reconciled with partial in-branch changes.

## Open Questions
- none

## Follow-Up Questions
- When ticket `06F8KZSYCVZ21MS983501BZG18` updates `docs/releases/v0.31.0.md`, should it only link to the final example section or also summarize the final diagnostics snippet/output?
- If adopters later want a richer provider-specific or observability-heavy sample than the shared customer-profile quickstart, should that be captured as a separate post-v0.31 ticket instead of broadening this task?

## Risks
- The implementation can drift into a broad tutorial rewrite or new sample family unless it stays anchored to the existing quickstart pair and shared flow.
- Diagnostics or observability snippets can accidentally over-promise raw SQL visibility, hosted tooling, or automatic maintenance behavior that the current contracts explicitly exclude.
- Requiring live PostgreSQL validation for the default success path would break the repository's SQLite-first no-infrastructure example posture.

## Split Recommendations
- No immediate split is recommended; the repository already has a bounded shared quickstart surface and the code-plus-README work fits one task.
- If scope expands to separate provider-specific scenarios or dedicated observability walkthroughs, create follow-up tickets rather than enlarging this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add one realistic but compact EF Core example scenario within the existing examples boundary.

Required repository output
- Update an existing quickstart/example under `examples/` or add one small example project if reuse would make the scenario unclear.
- Update `examples/README.md` so adopters can find the scenario and understand what it demonstrates.
- This ticket must produce example or documentation changes outside `.gicket`.

Scope in
- Use a fixed, compact domain such as customer/order activity or account activity; do not introduce multiple unrelated domains.
- Demonstrate ordinary EF Core usage with DVault metadata, explicit saves, typed or provider-neutral reads where already supported, diagnostics checks, and the guardrail/non-goal boundaries from v0.31.0 guidance.
- Prefer SQLite or an existing quickstart-friendly provider path unless the current examples already provide the provider setup needed without external infrastructure.
- Keep the example runnable with the repository's normal build/test expectations and avoid generated build artifacts in source control.

Scope out
- A sample platform, web app, hosted worker, dashboard, ingestion scheduler, external service dependency, container orchestration, or broad tutorial rewrite.
- Inventing new DVault APIs or changing library behavior to fit the example.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
Implemented the existing shared SQLite/PostgreSQL quickstart boundary for one realistic customer-profile EF Core plus DVault scenario. The shared flow now uses fixed CRM import/change events, explicit load timestamps and record sources, explicit `IDataVaultSaveService` writes, typed latest/as-of `IDataVaultReadService` reads, and bounded sanitized diagnostics output.

### Repository Artifacts
- `examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs`
- `examples/README.md`

### Verification
- `dotnet build DVault.slnx --nologo` passed with existing warning noise, including NU1900 warnings from the read-only NuGet vulnerability-cache path and existing analyzer/test warnings.
- `dotnet run --project examples/DCoding.Data.DVault.SqliteQuickstart/DCoding.Data.DVault.SqliteQuickstart.csproj` passed and printed the customer-profile scenario, fixed load timestamps, record sources, typed read summaries, and sanitized save/read diagnostics.
- `env -u DVAULT_TEST_POSTGRES_CONNECTION_STRING dotnet examples/DCoding.Data.DVault.PostgresQuickstart/bin/Debug/net10.0/DCoding.Data.DVault.PostgresQuickstart.dll` passed and printed the expected PostgreSQL skip message.
- `bash tools/check-format.sh` passed.
- `timeout 600s dotnet test DVault.slnx --nologo` reached the 10-minute timeout during MSBuild after restore/build progress; after the successful full build, `timeout 600s dotnet test DVault.slnx --nologo --no-build` passed with optional external-provider tests skipped because their connection strings were unset.

### Notes
No `.gicket` paths, release-note files, solution/project files, public DVault APIs, provider runtime behavior, hosted observability stack, PIT/bridge maintenance or orchestration flow, or generated runtime artifacts were changed.
<!-- gicket-bot:developer-delivery:v1:end -->