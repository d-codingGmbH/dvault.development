[gicket-bot] PO refinement contract

Summary
- Rerouted the ticket away from unsupported closure-only handling and refined it as a normal documentation implementation task backed by three verified repository gaps: missing `docs/releases/v0.30.0.md`, missing refresh/recovery wording in `README.md`, and missing stale-input troubleshooting guidance in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`. No new child tickets, attachments, planning documents, or relation writes were materialized in this refinement run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Corrected. This ticket is not a closure-only audit. It is a normal documentation implementation task for still-missing repository work in the README, the design-time workflow guide, and a new `docs/releases/v0.30.0.md` release note.
- critic-item-2: `answered` - Product is not keeping this closure-only. Because the scoped documentation evidence is not yet landed, the correct PO action is to reroute the ticket as implementation work instead of resubmitting it as closure-only.
- critic-item-3: `answered` - Answered by narrowing the contract to repository-backed remaining documentation work. The done sibling implementation tickets do not replace this ticket's own documentation deliverables, so closure-only approval is unsupported.
- critic-item-4: `answered` - Confirmed. `docs/releases/v0.30.0.md` is absent, `README.md:371-390` still lacks explicit refresh/recovery steps after bundle or fingerprint changes, and `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-181` still lacks the requested stale-input troubleshooting example. The analyzer README already documents the DMV196x baseline and should be reused as the wording source rather than reopened.

Clarifications
- Repository evidence fixes the baseline: typed helpers consume exactly one authoritative `dvault.support-bundle.v1`; raw `dvault.model.v1` is not a direct generator input; `DVaultTypedReadModelMetadataSourceFingerprint` drift is `DMV1961`; PIT and bridge helpers depend on request-bound `ReadShape` evidence supplied through `CreateSupportBundleDiagnostics`.
- The missing repository evidence is bounded to three concrete gaps: add `docs/releases/v0.30.0.md`, add refresh/recovery wording to `README.md:371-390`, and add stale-input troubleshooting guidance to `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-181`.
- `src/DCoding.Data.DVault.Analyzers/README.md:67-92` already carries the detailed DMV1960/<redacted>/1969 mapping; treat that as the v1 wording baseline and only edit it if needed to keep cross-document phrasing consistent.
- Sibling tickets `06F8KZP9XJ868GY6GT934QVFH4`, `06F8KZPN02NWFGMRC2Q1PKYKDR`, and `06F8KZPZZE8VZEBANP5MPN8HH8` are `done`, so this ticket is documentation follow-through only.
- The repository still contains `.gicket/relations/H8/0R/06F8KZPZZE8VZEBANP5MPN8HH8--06F8KZQAWZ7QRGB68KB21C9B0R--blocks.json` even though `.gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/ticket.json` reports `is-blocked=false`; treat that as non-blocking housekeeping, not closure evidence.

Scope In
- Update `README.md` typed-helper guidance to document the consumer recovery path after metadata changes: re-export the authoritative support bundle, update or remove a stale pinned `DVaultTypedReadModelMetadataSourceFingerprint`, and understand that stale or incompatible inputs surface through existing `DMV1960` and `DMV1961` diagnostics.
- Update `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` with an explicit stale-input troubleshooting example or checklist that shows re-running support-bundle export and re-supplying representative `CreateSupportBundleDiagnostics` requests when PIT or bridge helper generation depends on request-bound `ReadShape` evidence.
- Add `docs/releases/v0.30.0.md` as the current documentation baseline for typed-helper freshness, support-bundle refresh workflow, and stale-input troubleshooting.
- Keep `src/DCoding.Data.DVault.Analyzers/README.md` aligned with the shipped DMV196x baseline; only add wording if needed to make helper-specific skip behavior or freshness phrasing consistent with the README and architecture docs.

Scope Out
- New runtime diagnostics, source-generator behavior changes, or test-harness work already covered by done sibling tickets.
- Direct parsing of raw `dvault.model.v1`, source callbacks, or literal metadata objects by the generator.
- Support-bundle publication, attachment, routing, background refresh, or invented runtime requests.
- Rewriting historical release notes such as `docs/releases/v0.29.0.md` as if earlier behavior changed.
- Ticket-metadata housekeeping such as stale relation-file cleanup unless a separate bounded relation write is executed outside this documentation pass.

Open questions
- none

Follow-up questions
- After this documentation pass lands, should a shorter adopter-facing freshness checklist also be mirrored into `docs/production-adoption-checklist.md`?
- Should runtime or relation housekeeping separately reconcile the stale incoming `06F8KZPZZE8VZEBANP5MPN8HH8 --blocks--> 06F8KZQAWZ7QRGB68KB21C9B0R` file that still exists in `.gicket/relations/H8/0R/` even though the current ticket state is `is-blocked=false`?

Risks
- A partial update that touches the README or release notes without the design-time troubleshooting example can leave adopters without the documented recovery path for stale bundle or fingerprint inputs.
- If new wording diverges from the already-landed analyzer README and typed-helper contract, documentation may drift from the implemented DMV196x behavior.

Split recommendations
- No split recommended; remaining work is a single bounded documentation pass across the three verified repository gaps plus any minimal analyzer README wording alignment.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment