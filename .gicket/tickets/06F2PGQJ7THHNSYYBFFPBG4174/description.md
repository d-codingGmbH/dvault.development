<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the live ticket files, comments, and relations in .gicket plus the repository diagnostics/design-time surfaces; the story is ready to move to PO critic as a bounded redacted support-bundle export over the existing DVault diagnostics and command infrastructure, with no child tickets, relation edits, attachments, or planning documents created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The live relation set is consistent and already bounded: 06F2PGQ27NWVZ1B1R651S7SM4M is the parent epic, the done story 06F2PGQ6T5TGNWCBQBX3700D84 blocks this ticket, and this ticket blocks the separate v0.16 documentation task 06F2PGQQJB5FJGDB16M2G7CPCM.
- The inbound blocks relation from done epic 06F2PGP7HM8F39K3J0H5JHB3B4 is historical routing context only and is not an open blocker.
- The current ticket has no human refinement comments or persisted attachments; only bot claim comments are present, and no planning document was previously attached.
- Repository evidence shows no existing support-bundle export surface yet, but it already provides consumer-owned design-time commands (validate, export, drift, guardrail), structured serializable diagnostics via DataVaultDiagnosticsResult, and public drift/live-schema result types that can be reused instead of inventing parallel troubleshooting contracts.
- The completed strategy-explanation story already established the reusable request-bound save/read decision shapes; this story should serialize those existing diagnostics sections when available instead of defining a second explain format.
- No child tickets, relation edits, attachments, or planning documents were materialized during this refinement pass.

### Scope In
- A deterministic redacted support-bundle export for DVault configuration and provider-behavior troubleshooting, built on existing diagnostics and design-time infrastructure.
- An explicit consumer-owned export path that extends the current DataVaultDesignTimeCommand/DataVaultDesignTimeCommandHost pattern rather than adding a standalone DVault CLI or EF command interception surface.
- Default bundle content derived from existing DataVaultDiagnosticsResult data, including validation status, metadata source kind/fingerprint, provider name, capability profile, provider-behavior profile, load-timestamp storage details, and translated entity/table explain data.
- Preservation of existing request-bound save/read strategy diagnostics when the caller supplies a diagnostics result produced from save or read analysis, including selected strategy, candidate order, priority, and fallback causes.
- Optional opt-in live-schema or drift sections using the existing DataVaultLiveSchemaReadResult and DataVaultModelDriftReport surfaces, without making external-provider connectivity part of the default local bundle path.
- Tests, public API snapshot updates, and minimal source-local docs needed to ship the bundle contract safely.

### Scope Out
- A standalone DVault CLI, dotnet ef shim/interception, automatic database provisioning, or automatic schema repair.
- Changes to actual save/read dispatch behavior, provider thresholds, provider capability selection, or provider-behavior selection semantics.
- Telemetry hooks, counters, or metrics emission; that remains ticket 06F2PGQBGNZPEEJE4KBET4JG24.
- The coordinated v0.16 documentation and release-note wrap-up beyond minimal source-local docs needed for this story; that remains ticket 06F2PGQQJB5FJGDB16M2G7CPCM.
- A v1 multi-file archive, upload workflow, or distribution transport; the safe bounded default is a single deterministic exported artifact.
- App-specific synthetic save/read request generation inside the generic design-time host when the consumer application does not already have a representative request to analyze.

## Acceptance Criteria
- DVault exposes a deterministic redacted support-bundle export that emits one machine-readable JSON artifact from existing diagnostics/design-time data and can write to stdout or an explicit output path, mirroring the current design-time export ergonomics.
- The default support-bundle lane can be produced from the configured design-time DbContext without requiring a live database connection and includes enough existing diagnostics data to identify metadata source kind/fingerprint, provider name, capability profile, provider-behavior profile, load-timestamp storage format, and translated Data Vault entities/tables.
- When the input diagnostics result was produced from the existing save or read diagnostics APIs, the bundle preserves the structured SaveStrategy and ReadStrategy sections, including status, selected strategy name/priority, candidate ordering, and fallback causes, rather than inventing a second explanation schema.
- Any live-schema or drift data included in the bundle is opt-in, reuses the existing DataVaultLiveSchemaReadResult and DataVaultModelDriftReport semantics, and does not make non-SQLite external-provider connectivity part of the default local support-bundle path.
- Redaction rules remove or mask secret-bearing provider failure text such as connection-string or credential details while preserving provider names, diagnostic codes, profile names, and other troubleshooting-relevant contract data.
- Automated coverage locks JSON contract shape, deterministic output, redaction behavior, and any required command/API snapshot/source-local documentation updates for the support-bundle surface.

## Definition of Done
- Unit and any applicable integration tests prove the bundle contract, redaction behavior, and command/export path for the touched surfaces.
- Any changed public API, snapshot, XML/source-local docs, or README content is updated consistently enough that the downstream v0.16 documentation ticket can consume the contract without reopening scope.
- The final implementation reuses existing diagnostics/drift result shapes as the bundle payload source instead of maintaining a separate divergent troubleshooting model.
- Deferred work remains explicitly in the already-related telemetry and documentation tickets, and no secret-bearing sample output is left in tests or docs.

## Implementation Notes
- Reuse the existing public result shapes as the bundle payload source: DataVaultDiagnosticsResult, DataVaultExplainDiagnostics, DataVaultSaveStrategyDiagnostics, DataVaultReadStrategyDiagnostics, DataVaultLiveSchemaReadResult, and DataVaultModelDriftReport.
- Keep the export inside the current consumer-owned design-time architecture; prefer a dedicated support-bundle verb or equivalent explicit entrypoint on DataVaultDesignTimeCommand/DataVaultDesignTimeCommandHost instead of a new standalone tool.
- Mirror the current export behavior for output handling: a single deterministic JSON document, stdout by default, and an explicit output-path option when the caller wants a file.
- Do not make a live database mandatory for the default bundle. The baseline should work from design-time model analysis alone, and live-schema or drift capture should stay opt-in, with SQLite as the first-class local proof and external providers operationally managed by the consumer.
- Treat request-bound strategy data as caller-supplied context: when an application already has a representative save or read request, the bundle should serialize the resulting diagnostics sections; the generic design-time host should not invent app-specific requests just to populate those fields.
- Redaction should focus on provider exception text and other free-text surfaces that may carry credentials or connection strings; do not strip provider names, profile names, metadata names, or diagnostic identifiers that are necessary for troubleshooting.
- Minimal source-local docs for the new public behavior belong here, but the missing docs/releases/v0.16.0.md creation and coordinated release-note narrative remain downstream documentation work.

## Open Questions
- none

## Follow-Up Questions
- Should the telemetry story later emit counters or events when a support bundle is generated, or should bundle generation remain operationally silent?
- Should the downstream v0.16 documentation story add troubleshooting playbooks that map common strategy fallback causes to specific support-bundle sections?
- If later adoption requires multi-file packaging, attachment upload, or transport workflows, should that be handled by a separate post-v0.16 ticket rather than extending the initial support-bundle contract?

## Risks
- Secret leakage is the primary risk if raw provider availability/error text or consumer-supplied request values are serialized without redaction.
- The support-bundle contract can drift from real diagnostics behavior if it copies or rephrases strategy/drift data instead of serializing the existing result surfaces directly.
- Scope can sprawl into telemetry, documentation wrap-up, standalone tooling, or archive/distribution workflow unless the story stays bounded to redacted export over the existing diagnostics/design-time surfaces.

## Split Recommendations
- No additional split is recommended. The live relation set already separates completed strategy explanation work, sibling telemetry work, and downstream v0.16 documentation work; keep this story bounded to the redacted support-bundle export contract, command integration, and tests/docs.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Provide a redacted support bundle for troubleshooting DVault configuration and provider behavior.

## Scope
- Refine and complete the work for "Add diagnostics support bundle export" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.