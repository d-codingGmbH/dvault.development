[gicket-bot] PO refinement contract

Summary
- Refined the epic against the current repository baseline: the read-plan/ReadShape contract, support-bundle-driven PIT and bridge helper contract, PIT and bridge implementation, and the v0.25.0 documentation rollout are already decomposed into child tickets with done repository evidence, so this tracking epic is ready for PO-critic with no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already breaks this epic into done child tickets 06F7Y0FZXX5J0G7G15681HVEBR, 06F7Y0GT7A5QT77TADMRZBVYN8, 06F7Y0H83H29E1D9K5RK3K7Y9W, 06F7Y0HJ1ZPY7ND9N8RVS92H4C, and 06F7Y0HZKHBHMYX9EYDYFRYXZ0.
- Archived child 06F7Y0GFY7TP3V4B76JB759KB0 was closed as duplicate/already implemented under 06F7Y0FZXX5J0G7G15681HVEBR and does not leave residual epic scope.
- This ticket should stay a tracking parent only; child-level API shape and implementation details are already ratified in the architecture docs, generator tests, and child ticket contracts.
- The authoritative read-plan surface is IDataVaultReadDiagnosticsService.Analyze(...) returning DataVaultDiagnosticsResult with ReadStrategy plus additive ReadShape; the same bounded facts serialize under readShape in dvault.support-bundle.v1 when representative diagnostics are supplied.
- The authoritative generator boundary is support-bundle-driven: exactly one authoritative dvault.support-bundle.v1 input, optional DVaultTypedReadModelMetadataSourceFingerprint gating, and no raw dvault.model.v1 or source-visible declaration parsing inside the generator.
- The implemented helper and diagnostics vocabulary is bounded to satellite latest/current/as-of diagnostics, PIT Read...AsOfAsync helpers, and bridge Read...FromAsync/Read...ToAsync plus hierarchy Read...AncestorAsync/Read...DescendantAsync helpers with required maximumDepth.
- docs/releases/v0.25.0.md is the current coordinated public documentation baseline for this epic's contract.

Scope In
- Bounded redacted ReadShape diagnostics for latest satellite, PIT as-of, and bridge reads, including provider strategy and fallback facts plus translated table and column facts.
- Support-bundle serialization of the same redacted readShape evidence for reviewed representative requests.
- Support-bundle-driven typed PIT helper generation for supported hub-parent, shared-driving-key multi-active hub-parent, and bounded link-parent PIT shapes.
- Support-bundle-driven typed bridge helper generation for supported many-to-many From/To and hierarchy Ancestor/Descendant traversal with required maximumDepth.
- Coordinated documentation and release-note rollout that describes the implemented read-plan and typed-helper surface consistently.

Scope Out
- Any custom LINQ provider, alternate query planner, dashboard, or query orchestration platform.
- Raw SQL capture, provider query-plan export, physical-plan promises, automatic index advice, or secret-bearing diagnostics output.
- Raw dvault.model.v1 parsing, source-visible Code-First inspection, or literal metadata-first inference inside the typed-helper generator.
- Automatic PIT or bridge maintenance, read-time refresh, scheduling, SaveChanges orchestration, or widened runtime read semantics.
- Unbounded bridge traversal, dynamic runtime query compilation, or new runtime read primitives beyond the existing IDataVaultReadService boundary.
- Support-bundle transport automation, package-publication claims, or end-to-end sample-app expansion.

Open questions
- none

Follow-up questions
- Should a separate follow-up ticket add an end-to-end consumer sample that exports representative readShape diagnostics and compiles generated PIT and bridge helpers?
- Should superseded satellite-only planning documents receive a stronger banner or note so readers do not mistake them for the current v0.25.0 baseline?

Risks
- Live relation state still shows historical blocks edges from done tickets 06F7Y0HZKHBHMYX9EYDYFRYXZ0 and 06F7Y0F650KM61BQXMEQPZ86DR to this epic; humans or automation could misread those as active blockers until relation cleanup happens.
- Superseded satellite-only planning context can still confuse readers if current-baseline links drift away from docs/releases/v0.25.0.md and the architecture contracts.
- ReadShape explanatory strings and expected index baselines could be over-interpreted as physical-plan guarantees if later docs or consumers blur the diagnostics-only boundary.

Split recommendations
- No new split is required; this epic is already decomposed into diagnostics contract, helper contract, PIT implementation, bridge implementation, and documentation rollout tickets.
- Keep any future raw-SQL or plan capture, automatic maintenance or orchestration, support-bundle transport automation, or sample-app work in separate additive tickets rather than reopening this epic.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment