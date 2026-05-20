[gicket-bot] PO refinement contract

Summary
- Verified the live ticket files, comments, and relations in .gicket plus the repository diagnostics/design-time surfaces; the story is ready to move to PO critic as a bounded redacted support-bundle export over the existing DVault diagnostics and command infrastructure, with no child tickets, relation edits, attachments, or planning documents created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The live relation set is consistent and already bounded: 06F2PGQ27NWVZ1B1R651S7SM4M is the parent epic, the done story 06F2PGQ6T5TGNWCBQBX3700D84 blocks this ticket, and this ticket blocks the separate v0.16 documentation task 06F2PGQQJB5FJGDB16M2G7CPCM.
- The inbound blocks relation from done epic 06F2PGP7HM8F39K3J0H5JHB3B4 is historical routing context only and is not an open blocker.
- The current ticket has no human refinement comments or persisted attachments; only bot claim comments are present, and no planning document was previously attached.
- Repository evidence shows no existing support-bundle export surface yet, but it already provides consumer-owned design-time commands (validate, export, drift, guardrail), structured serializable diagnostics via DataVaultDiagnosticsResult, and public drift/live-schema result types that can be reused instead of inventing parallel troubleshooting contracts.
- The completed strategy-explanation story already established the reusable request-bound save/read decision shapes; this story should serialize those existing diagnostics sections when available instead of defining a second explain format.
- No child tickets, relation edits, attachments, or planning documents were materialized during this refinement pass.

Scope In
- A deterministic redacted support-bundle export for DVault configuration and provider-behavior troubleshooting, built on existing diagnostics and design-time infrastructure.
- An explicit consumer-owned export path that extends the current DataVaultDesignTimeCommand/DataVaultDesignTimeCommandHost pattern rather than adding a standalone DVault CLI or EF command interception surface.
- Default bundle content derived from existing DataVaultDiagnosticsResult data, including validation status, metadata source kind/fingerprint, provider name, capability profile, provider-behavior profile, load-timestamp storage details, and translated entity/table explain data.
- Preservation of existing request-bound save/read strategy diagnostics when the caller supplies a diagnostics result produced from save or read analysis, including selected strategy, candidate order, priority, and fallback causes.
- Optional opt-in live-schema or drift sections using the existing DataVaultLiveSchemaReadResult and DataVaultModelDriftReport surfaces, without making external-provider connectivity part of the default local bundle path.
- Tests, public API snapshot updates, and minimal source-local docs needed to ship the bundle contract safely.

Scope Out
- A standalone DVault CLI, dotnet ef shim/interception, automatic database provisioning, or automatic schema repair.
- Changes to actual save/read dispatch behavior, provider thresholds, provider capability selection, or provider-behavior selection semantics.
- Telemetry hooks, counters, or metrics emission; that remains ticket 06F2PGQBGNZPEEJE4KBET4JG24.
- The coordinated v0.16 documentation and release-note wrap-up beyond minimal source-local docs needed for this story; that remains ticket 06F2PGQQJB5FJGDB16M2G7CPCM.
- A v1 multi-file archive, upload workflow, or distribution transport; the safe bounded default is a single deterministic exported artifact.
- App-specific synthetic save/read request generation inside the generic design-time host when the consumer application does not already have a representative request to analyze.

Open questions
- none

Follow-up questions
- Should the telemetry story later emit counters or events when a support bundle is generated, or should bundle generation remain operationally silent?
- Should the downstream v0.16 documentation story add troubleshooting playbooks that map common strategy fallback causes to specific support-bundle sections?
- If later adoption requires multi-file packaging, attachment upload, or transport workflows, should that be handled by a separate post-v0.16 ticket rather than extending the initial support-bundle contract?

Risks
- Secret leakage is the primary risk if raw provider availability/error text or consumer-supplied request values are serialized without redaction.
- The support-bundle contract can drift from real diagnostics behavior if it copies or rephrases strategy/drift data instead of serializing the existing result surfaces directly.
- Scope can sprawl into telemetry, documentation wrap-up, standalone tooling, or archive/distribution workflow unless the story stays bounded to redacted export over the existing diagnostics/design-time surfaces.

Split recommendations
- No additional split is recommended. The live relation set already separates completed strategy explanation work, sibling telemetry work, and downstream v0.16 documentation work; keep this story bounded to the redacted support-bundle export contract, command integration, and tests/docs.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment