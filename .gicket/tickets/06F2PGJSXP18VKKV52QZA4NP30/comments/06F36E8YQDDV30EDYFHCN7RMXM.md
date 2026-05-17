[gicket-bot] PO refinement contract

Summary
- Refined 06F2PGJSXP18VKKV52QZA4NP30 into a bounded v1 generator implementation ticket: extend DCoding.Data.DVault.Analyzers with the first source generator, add the consumer-facing compile-time mapping declaration surface in DCoding.Data.DVault, generate deterministic metadata helpers plus registry-backed row-helper code for hubs, unique-participant links, ordinary hub-parent satellites, and hub-parent multi-active satellites, and verify through generator, runtime, package, and SQLite integration coverage; no child tickets, attachments, planning documents, or relation writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Upstream contract ticket 06F2PGJN1XCV8F7NWH567SQSKM is already done and integrated, so its generator boundary is the authoritative baseline for this ticket rather than an open design dependency.
- Repository evidence shows src/DCoding.Data.DVault.Analyzers is already the optional packable analyzer and code-fix package, but it currently contains no source-generator implementation, so v1 generator work should extend that package instead of adding a new package family.
- The analyzer package ships analyzer assets only, suppresses dependencies, and does not expose compile-reference assets to consumers; consumer-authored mapping declaration types therefore belong in src/DCoding.Data.DVault, while the generator remains self-contained in the analyzer package.
- Existing runtime integration stays anchored on IDataVaultHubMapper<TSource>, IDataVaultLinkMapper<TSource>, IDataVaultSatelliteMapper<TSource>, DataVaultRegistryHubSaveOperation, DataVaultRegistryLinkSaveOperation, DataVaultRegistrySatelliteSaveOperation, and caller-supplied loadTimestamp plus recordSource through the current save-service request flow.
- Current typed-mapper boundaries already constrain v1 generator output: links require unique participant hub names by StringComparer.Ordinal, and link-parent satellites plus same-hub or repeated-participant link mappings are outside this ticket.
- No child tickets, attachments, planning documents, or relation writes were created in this refinement pass; live relation state still shows historical incoming blocks edges from done contract ticket 06F2PGJN1XCV8F7NWH567SQSKM, done story 06F2PGJBRXFCP038CN6XVAYSZM, and done epic 06F2PGFT8Z406HFBJGQSY7YRJ0.

Scope In
- Add the first public compile-time mapping declaration surface in src/DCoding.Data.DVault so consuming code can declare one source type to one logical hub, link, or hub-parent satellite target by exact DVault names and ordered member bindings.
- Implement the first source generator in src/DCoding.Data.DVault.Analyzers that reads those declarations and emits deterministic metadata helper output plus row-helper code that constructs existing registry-backed save operations.
- Support hub mappings, link mappings whose participant hub names are unique by StringComparer.Ordinal, ordinary hub-parent satellite mappings, and hub-parent multi-active satellite mappings.
- Require supported declarations to bind all runtime-required values explicitly: hub business keys, link participant hash keys, satellite parent hash key, satellite payload values, satellite hash diff, and multi-active driving keys where applicable.
- Add the minimal compile-time diagnostics, generator tests, runtime API tests, package verification, and end-to-end SQLite proof needed to make the supported generator slice safe and usable.

Scope Out
- No new package family, no fourth metadata authority, and no generator-time execution of EF models, ApplyDataVaultMetadata(...), JSON artifacts, or design-time commands.
- No hidden SaveAsync orchestration, no automatic loadTimestamp or recordSource, and no automatic hash-key or hash-diff derivation beyond caller-provided mapped values.
- No link-parent satellite generation, no same-hub or repeated-participant or self-link typed link generation, and no widening into other unsupported runtime shapes.
- No broader analyzer or code-fix, migration, design-time CLI, or provider behavior work outside the bounded generator slice.
- No coordinated v0.12 README, examples, release-note closure, or docs/releases/v0.12.0.md work; that remains with 06F2PGJYY6S97B4Z8044D34K5C, aside from source-local XML docs or test updates required by touched code.

Open questions
- none

Follow-up questions
- After the bounded v1 slice lands, should a follow-on ticket add link-parent satellite generation on the same runtime boundary?
- Should a separate follow-on ticket add explicit participant-alias support for repeated-participant or self-link mappings instead of widening the first generator slice?
- When 06F2PGJYY6S97B4Z8044D34K5C runs, should the v0.12 documentation explicitly compare manual typed mappers against generated helpers on the same DataVaultRegistry*SaveOperation boundary?

Risks
- If the declaration surface or generated helpers start behaving like a new metadata authority or hidden persistence layer, the ticket will sprawl beyond the ratified v1 boundary.
- If the analyzer package gains a runtime dependency or consumer-only declaration types live in analyzer-only assets, the current package shape can break consumer compilation or analyzer loading.
- Generated support can accidentally overreach into repeated-participant or self-link or link-parent satellite shapes that the current runtime and typed-mapper contracts do not safely support.
- New public API in DCoding.Data.DVault and new analyzer behavior both require disciplined snapshot and package verification to avoid silent package-shape regressions.
- Because no relation cleanup was materialized in this pass, live planning views may still show historical blockers from done tickets even though the design baseline for this implementation is already settled.

Split recommendations
- No additional split is required before PO-critic review; the current separation between contract ticket 06F2PGJN1XCV8F7NWH567SQSKM, implementation ticket 06F2PGJSXP18VKKV52QZA4NP30, and documentation ticket 06F2PGJYY6S97B4Z8044D34K5C is sufficient for the available evidence.
- If development proves the bounded v1 implementation is still too large, split follow-on work by excluded shape families such as link-parent satellites or repeated-participant or self-link support instead of widening this ticket.
- Keep any later ergonomic wrappers around SaveHubAsync(...), SaveLinkAsync(...), bulk orchestration, or relation-graph cleanup in separate downstream tickets rather than mixing them into the first generator-output implementation.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment