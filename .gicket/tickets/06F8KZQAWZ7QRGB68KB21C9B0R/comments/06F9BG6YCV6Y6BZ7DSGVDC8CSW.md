[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the baseline: typed helpers are opt-in, consume exactly one authoritative `dvault.support-bundle.v1`, use `DVaultTypedReadModelMetadataSourceFingerprint` for optional drift enforcement, and require request-bound `ReadShape` facts only for PIT/bridge helpers.
- Sibling tickets `06F8KZP9XJ868GY6GT934QVFH4`, `06F8KZPN02NWFGMRC2Q1PKYKDR`, and `06F8KZPZZE8VZEBANP5MPN8HH8` are `done`, so this ticket is documentation follow-through rather than contract or implementation discovery.
- The repository has no `docs/releases/v0.30.0.md` file in the current branch evidence, so the v0.30.0 release-note update should be a new current-baseline release document instead of rewriting historical `v0.29.0` notes.

Scope In
- Update `README.md` typed-helper and support-bundle guidance to explain refresh workflow when the authoritative bundle or pinned fingerprint changes, including how stale or incompatible inputs surface through existing diagnostics.
- Update `src/DCoding.Data.DVault.Analyzers/README.md` so the typed read-model generator section and diagnostic table describe stale fingerprint, incompatible bundle, and partial PIT/bridge skip behavior using the shipped `DMV1960`/`DMV1961`/`DMV1963`/`DMV1964`/`DMV1967` mapping.
- Update the existing support-bundle troubleshooting surface in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` with consumer-owned refresh guidance and typed-helper failure examples.
- Add or update the coordinated `docs/releases/v0.30.0.md` release note to record the documentation boundary for support-bundle freshness and typed-helper troubleshooting examples.
- Keep all documentation aligned with the current v1 typed PIT/bridge helper contract and the consumer-owned `CreateSupportBundleDiagnostics` request-bound evidence path.

Scope Out
- New runtime diagnostics, generator behavior changes, or test-harness work already covered by the done sibling tickets.
- Direct generator parsing of raw `dvault.model.v1`, source callbacks, or literal metadata objects.
- Automatic support-bundle publication, attachment, routing, background refresh, or runtime request invention.
- Rewriting historical release notes as if v0.25.0 or v0.29.0 behavior changed.
- Provider-specific SQL/read-strategy changes or PIT/bridge maintenance behavior changes.

Open questions
- none

Follow-up questions
- If v0.30.0 adopters still need a shorter checklist surface after this doc pass, decide later whether the same freshness troubleshooting examples should also be mirrored into `docs/production-adoption-checklist.md`.

Risks
- Because the authoritative boundary is spread across README, analyzer README, design-time workflow guidance, and architecture docs, partial documentation updates can reintroduce ambiguity about freshness versus PIT/bridge ReadShape compatibility.
- No `docs/releases/v0.30.0.md` file is present in the current branch evidence, so the current coordinated release narrative will stay incomplete unless this ticket explicitly adds that release-note record.

Split recommendations
- No split recommended; the epic already separated contract definition, diagnostics implementation, transition-test coverage, and this documentation follow-through into bounded sibling tickets.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment