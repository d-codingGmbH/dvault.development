[gicket-bot] PO refinement contract

Summary
- Refinement confirms this is a bounded documentation task: build on the existing README observability baseline, add quickstart-facing examples without new runtime dependencies, and leave live ticket relations unchanged.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- No human comments or ticket attachments add new constraints; the only current comments are system claim/lease records.
- Repository evidence already defines the observability baseline in `README.md`: `AddDVaultTelemetry()` is the opt-in built-in `System.Diagnostics.Metrics` observer path, the built-in meter name is `DCoding.Data.DVault`, and Activity tracing is listener-driven through the `DCoding.Data.DVault` `ActivitySource`.
- The tracing contract remains authoritative in `docs/architecture/dvault-v1-activity-tracing-contract.md`; the new examples should point to that document instead of restating span, tag, sampling, or redaction rules in full.
- Live relation evidence shows this ticket is a child of epic `06F8KZQNH8CCMTJW9P95W1N388` and currently blocks `06F8KZSYCVZ21MS983501BZG18`; no child-ticket, relation, description, attachment, or planning-document writes were materialized during this refinement.
- No active incoming `blocks` relation was found for this ticket; the done sibling `06F8KZRSTHAGSP6GPGFBFQGY08` appears only as historical event/comment context, not as a live blocker.

Scope In
- A bounded documentation/example update in `README.md` and/or `examples/README.md` that shows how an application opts into DVault metrics and Activity tracing without changing DVault runtime behavior.
- Separate examples for metrics via `services.AddDVault(); services.AddDVaultTelemetry();` and for tracing via `ActivityListener` or clearly application-owned OpenTelemetry-style pseudo-code targeting `DCoding.Data.DVault`.
- Quickstart-facing wording that keeps `AddDVault()` telemetry-free by default and treats metrics, `IDataVaultTelemetryObserver`, and Activity tracing as sibling opt-in surfaces.
- Cross-links to the tracing contract and explicit reminders about redaction/omission boundaries for example output.

Scope Out
- New observability runtime APIs, meter/activity-name changes, or any change under `src/` that alters tracing or metrics behavior.
- NuGet package installation instructions for OpenTelemetry exporters, AppInsights, Jaeger, collectors, dashboards, alerts, hosting templates, or deployment setup.
- A new runnable observability sample project, backend-specific pipeline setup, or environment-specific operations guidance.
- Updating the dependent release-documentation ticket `06F8KZSYCVZ21MS983501BZG18` as part of this ticket.

Open questions
- none

Follow-up questions
- After this ticket lands, should `06F8KZSYCVZ21MS983501BZG18` link directly to the new observability example section rather than repeat its own tracing/metrics snippet?
- Do we want a later separate ticket for a runnable observability sample or provider-specific OpenTelemetry package guidance if adopters ask for more than the bounded documentation baseline?

Risks
- The root `README.md` already contains detailed tracing and telemetry prose; if `examples/README.md` repeats too much contract detail, the docs can drift unless the new section stays compact and link-first.
- `examples/README.md` currently shows `0.16.0` package-version examples while the root README installation baseline is `0.30.0`; touching that file without care could preserve stale version guidance even though version alignment is not the main scope of this ticket.
- An overly concrete OpenTelemetry snippet could accidentally imply DVault-owned package, exporter, or backend responsibilities; the wording must keep all such integration choices explicitly application-owned.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment