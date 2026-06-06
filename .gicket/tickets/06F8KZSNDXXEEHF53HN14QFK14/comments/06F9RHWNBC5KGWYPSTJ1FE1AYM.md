[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing shared SQLite/PostgreSQL quickstart boundary: reuse the checked-in customer-profile flow, add one bounded diagnostics-aware realistic example pass plus README guidance, and keep SQLite as the default runnable lane. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Recent comments and closure evidence are empty in the ticket snapshot, so no newer human direction overrides the repository baseline.
- Repository evidence already fixes the v1 examples boundary to the two runnable quickstarts under `examples/` that share `examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs`; this ticket should extend that surface rather than introduce a broader sample family.
- The default scenario should stay on the existing customer-profile history domain: one `Customer` hub, one `CustomerProfile` satellite, two ordered profile-state changes, explicit `IDataVaultSaveService` writes, and typed latest/as-of reads through `IDataVaultReadService`.
- SQLite is the authoritative no-external-infrastructure runnable baseline; PostgreSQL remains the same scenario behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING` and must stay optional.
- The example's observability and diagnostics layer is bounded to request-level DVault diagnostics plus opt-in telemetry/tracing guidance already documented in the repository; it must not imply dashboards, exporters, collectors, hosting, schedulers, automatic PIT/bridge maintenance, or runtime routing changes.
- Existing ticket reads in this session already established the parent epic `06F8KZQNH8CCMTJW9P95W1N388` and downstream release-documentation ticket `06F8KZSYCVZ21MS983501BZG18` context; no relation changes were applied.

Scope In
- Update the existing quickstart surface under `examples/` so one compact realistic EF Core plus DVault scenario is visible in checked-in code without introducing multiple unrelated domains.
- Keep the existing metadata-first customer-profile history flow and make explicit load timestamps, record source, hub/satellite intent, and typed latest/as-of read behavior visible.
- Show at least one bounded diagnostics check for the scenario, using the current redacted DVault diagnostics surfaces rather than ad hoc provider output.
- Update `examples/README.md` so adopters can find the scenario, understand the SQLite-first and PostgreSQL-optional provider story, and see the v0.31 observability/non-goal boundaries.

Scope Out
- Adding a new web app, hosted worker, scheduler, dashboard, exporter, collector, or platform sample.
- Expanding the quickstart into PIT/bridge maintenance, typed read-model generator, stored-procedure artifact, or broad production tutorial coverage beyond brief pointers to existing docs.
- Changing DVault public APIs, runtime behavior, provider strategy gates, or repository-wide documentation contracts to fit the example.
- Introducing extra provider-specific runnable projects beyond the current SQLite and PostgreSQL quickstarts unless a separate ticket explicitly expands the examples boundary.

Open questions
- none

Follow-up questions
- When ticket `06F8KZSYCVZ21MS983501BZG18` updates `docs/releases/v0.31.0.md`, should it only link to the final example section or also summarize the final diagnostics snippet/output?
- If adopters later want a richer provider-specific or observability-heavy sample than the shared customer-profile quickstart, should that be captured as a separate post-v0.31 ticket instead of broadening this task?

Risks
- The implementation can drift into a broad tutorial rewrite or new sample family unless it stays anchored to the existing quickstart pair and shared flow.
- Diagnostics or observability snippets can accidentally over-promise raw SQL visibility, hosted tooling, or automatic maintenance behavior that the current contracts explicitly exclude.
- Requiring live PostgreSQL validation for the default success path would break the repository's SQLite-first no-infrastructure example posture.

Split recommendations
- No immediate split is recommended; the repository already has a bounded shared quickstart surface and the code-plus-README work fits one task.
- If scope expands to separate provider-specific scenarios or dedicated observability walkthroughs, create follow-up tickets rather than enlarging this ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment