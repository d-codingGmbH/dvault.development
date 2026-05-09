[gicket-bot] PO refinement contract

Summary
- Refined the story around the existing DataVaultMetadataRegistry baseline, ratified the DI and EF integration already evidenced in the repo, and kept the live three-child split with no new planning writes or relation changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already establishes DataVaultMetadataRegistry as the v1 registry type, so this story should ratify that concrete baseline instead of reopening a second separate model-registry abstraction.
- Satellite lookup is parent-scoped in v1: hubs, links, bridges, and current point-in-time metadata can be looked up by exact logical name, while satellites resolve by exact parent reference plus logical name or CLR type where a CLR mapping exists.
- CLR-type lookup is opt-in rather than universal; it is available only for metadata entries that have one unambiguous CLR mapping, and ambiguous or missing mappings must fail with actionable diagnostics.
- The default service-registration path remains optionless AddDVault(), while callers that want one authoritative registry can register a DataVaultMetadataModel or prebuilt DataVaultMetadataRegistry during service setup and consume it through UseDataVaultMetadata() or downstream services.
- Future model-first import work stays additive to the same immutable registry surface; this story only needs the registry contract to be able to host imported metadata later, not to implement file formats or import-export behavior now.
- The parent already has three materialized child tickets through live parentOf relations to 06F0MEAXT99V0P115P0WEJD4P0, 06F0MEB634X6CTBZ00W108G3FG, and 06F0MEBFTW8FY5T7PY5HJ5JXJ4; this refinement created no new child tickets, attachments, or relation changes.

Scope In
- Immutable registry creation and deterministic lookup over the current DVault metadata families used by projection, save-read services, diagnostics, and examples.
- DI and EF integration that lets AddDVault(...) provide one authoritative registry or metadata model and lets UseDataVaultMetadata(...) consume that app-level registry with explicit context overrides when needed.
- A single unified registry path for both metadata-first declarations and code-first declarations that first normalize to DataVaultMetadataModel.
- Actionable validation and diagnostics for duplicate logical names, missing metadata dependencies, conflicting metadata sources, and ambiguous or absent CLR-based lookups.

Scope Out
- Model-first file import-export, external serialization formats, or repository-to-registry tooling.
- Runtime mutation of registry contents after service-provider build.
- New provider-specific SQL, save-service semantics, read-service behavior, PIT refresh, or bridge maintenance behavior beyond consuming the registry as authoritative metadata.
- Additional fluent API expansion or broad schema parity work already covered by completed code-first children and follow-up parity ticket 06F0MEAD1BAA5QEVM3F9QJA38G.

Open questions
- none

Follow-up questions
- After 06F0MEAD1BAA5QEVM3F9QJA38G lands, does the team want additional provider-profile regression matrices specifically for app-level registry defaults versus context-level overrides?
- When future model-first import work is scheduled, should imported artifacts be allowed to supply CLR mappings directly, or should v1 import start as logical-name-only metadata until callers opt into explicit type bindings?

Risks
- If app-level registry defaults and explicit context overrides are not conflict-checked consistently, different workflows can project different metadata from the same DbContext model.
- If CLR lookup ever falls back to first-match or registration-order behavior, the registry loses the deterministic semantics this story is supposed to centralize.
- Because PIT and bridge metadata are already representable, downstream consumers may over-assume runtime support unless docs and diagnostics keep the deferred-capability boundary explicit.

Split recommendations
- Keep the already-materialized three-child split under 06F0MEANEV00QSYHMSGWX1X0R4; current evidence does not justify creating more child tickets or changing live relations.
- Keep broader schema-parity expansion on 06F0MEAD1BAA5QEVM3F9QJA38G instead of folding that follow-up breadth back into this registry story.

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