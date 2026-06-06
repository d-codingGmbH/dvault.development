<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement confirms this is a bounded documentation task: build on the existing README observability baseline, add quickstart-facing examples without new runtime dependencies, and leave live ticket relations unchanged.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No human comments or ticket attachments add new constraints; the only current comments are system claim/lease records.
- Repository evidence already defines the observability baseline in `README.md`: `AddDVaultTelemetry()` is the opt-in built-in `System.Diagnostics.Metrics` observer path, the built-in meter name is `DCoding.Data.DVault`, and Activity tracing is listener-driven through the `DCoding.Data.DVault` `ActivitySource`.
- The tracing contract remains authoritative in `docs/architecture/dvault-v1-activity-tracing-contract.md`; the new examples should point to that document instead of restating span, tag, sampling, or redaction rules in full.
- Live relation evidence shows this ticket is a child of epic `06F8KZQNH8CCMTJW9P95W1N388` and currently blocks `06F8KZSYCVZ21MS983501BZG18`; no child-ticket, relation, description, attachment, or planning-document writes were materialized during this refinement.
- No active incoming `blocks` relation was found for this ticket; the done sibling `06F8KZRSTHAGSP6GPGFBFQGY08` appears only as historical event/comment context, not as a live blocker.

### Scope In
- A bounded documentation/example update in `README.md` and/or `examples/README.md` that shows how an application opts into DVault metrics and Activity tracing without changing DVault runtime behavior.
- Separate examples for metrics via `services.AddDVault(); services.AddDVaultTelemetry();` and for tracing via `ActivityListener` or clearly application-owned OpenTelemetry-style pseudo-code targeting `DCoding.Data.DVault`.
- Quickstart-facing wording that keeps `AddDVault()` telemetry-free by default and treats metrics, `IDataVaultTelemetryObserver`, and Activity tracing as sibling opt-in surfaces.
- Cross-links to the tracing contract and explicit reminders about redaction/omission boundaries for example output.

### Scope Out
- New observability runtime APIs, meter/activity-name changes, or any change under `src/` that alters tracing or metrics behavior.
- NuGet package installation instructions for OpenTelemetry exporters, AppInsights, Jaeger, collectors, dashboards, alerts, hosting templates, or deployment setup.
- A new runnable observability sample project, backend-specific pipeline setup, or environment-specific operations guidance.
- Updating the dependent release-documentation ticket `06F8KZSYCVZ21MS983501BZG18` as part of this ticket.

## Acceptance Criteria
- At least one repository document outside `.gicket` (`README.md` and/or `examples/README.md`) is updated with compact adopter-facing observability examples.
- The updated docs show the built-in metrics path separately from tracing: metrics use `AddDVaultTelemetry()`, while tracing is listener-driven for the `DCoding.Data.DVault` `ActivitySource` and does not require `AddDVaultTelemetry()`.
- The updated docs explicitly state that `AddDVault()` remains telemetry-free by default and that any OpenTelemetry-style tracing/metrics wiring is application-owned.
- The updated docs link to `docs/architecture/dvault-v1-activity-tracing-contract.md` for authoritative ActivitySource, span/event/tag, sampling, omission, and redaction rules instead of duplicating that contract.
- All examples stay bounded and sanitized: no raw keys, payload values, SQL text, connection strings, provider messages, exception text, stack traces, support-bundle content, exporter endpoints, or deployment instructions.
- If an OpenTelemetry-style snippet is included, it is clearly pseudo-code or package-agnostic and does not introduce DVault-owned package or runtime dependency claims.

## Definition of Done
- The documentation change lands outside `.gicket` and fits the existing quickstart/adopter documentation style.
- Any touched example uses the current repository names `DCoding.Data.DVault`, `AddDVaultTelemetry()`, `IDataVaultTelemetryObserver`, and `ActivityListener` consistently with the README and tracing contract.
- The final wording keeps contract details link-first and avoids duplicating large tracing tables or redefining redaction rules.
- No new DVault package references, exporters, or runtime dependency claims are introduced by the documentation update.
- The resulting doc section is sufficient for the blocked follow-on ticket `06F8KZSYCVZ21MS983501BZG18` to reference instead of needing another observability-example design pass.

## Implementation Notes
- The repository already has general observability guidance in the root `README.md`, including `AddDVaultTelemetry()` and an `ActivityListener` snippet; the smallest coherent delivery is to add a quickstart-facing observability subsection to `examples/README.md` and optionally add a short cross-link back to the root README section instead of duplicating long contract prose.
- Keep the new example aligned with the existing metadata-first quickstart startup shape that already uses `AddDVault(...)`, provider registration, and `UseDataVaultMetadata()`; the observability snippet should be an additive application-wiring example, not a new quickstart architecture.
- If a tracing-provider example is shown, prefer a minimal package-agnostic shape such as registering `DCoding.Data.DVault` as the source and meter in application-owned tracing or metrics configuration, while leaving exporter and backend specifics entirely out of scope.
- The root README already states that the built-in meter name is `DCoding.Data.DVault`; reuse that exact name when a metrics snippet needs to mention meter registration.
- Because the live relation set shows this ticket blocks `06F8KZSYCVZ21MS983501BZG18`, keep the work tightly scoped to the bounded doc/example delta that the dependent release-guidance update needs.

## Open Questions
- none

## Follow-Up Questions
- After this ticket lands, should `06F8KZSYCVZ21MS983501BZG18` link directly to the new observability example section rather than repeat its own tracing/metrics snippet?
- Do we want a later separate ticket for a runnable observability sample or provider-specific OpenTelemetry package guidance if adopters ask for more than the bounded documentation baseline?

## Risks
- The root `README.md` already contains detailed tracing and telemetry prose; if `examples/README.md` repeats too much contract detail, the docs can drift unless the new section stays compact and link-first.
- `examples/README.md` currently shows `0.16.0` package-version examples while the root README installation baseline is `0.30.0`; touching that file without care could preserve stale version guidance even though version alignment is not the main scope of this ticket.
- An overly concrete OpenTelemetry snippet could accidentally imply DVault-owned package, exporter, or backend responsibilities; the wording must keep all such integration choices explicitly application-owned.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add bounded observability examples for DVault Activity tracing and Metrics without adding application-platform responsibilities.

Required repository output
- Update `README.md` and/or `examples/README.md` with compact examples that show how an application opts into DVault Activity tracing and built-in Metrics.
- Link to `docs/architecture/dvault-v1-activity-tracing-contract.md` for the authoritative ActivitySource, span/event/tag, sampling, and redaction rules.
- This ticket must produce documentation or example changes outside `.gicket`.

Scope in
- Show listener-driven ActivitySource usage for `DCoding.Data.DVault` and make clear that `AddDVault()` remains telemetry-free by default.
- Show `AddDVaultTelemetry()` as the built-in `System.Diagnostics.Metrics` observer path, separate from Activity tracing.
- Include pseudo-code or minimal illustrative wiring for OpenTelemetry-style tracing/metrics integration only if it stays application-owned and does not add package references or runtime dependencies to DVault.
- Point adopters at redaction and omission rules; examples must not include raw keys, payload values, SQL text, connection strings, provider messages, exception text, stack traces, or support-bundle content.

Scope out
- Adding OpenTelemetry exporter packages, AppInsights or Jaeger dependencies, collectors, dashboards, alerts, hosting templates, sampling policy defaults, custom correlation storage, or deployment instructions.
- Changing DVault tracing or metrics runtime behavior.