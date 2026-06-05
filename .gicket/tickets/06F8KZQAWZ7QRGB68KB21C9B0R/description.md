<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Rerouted the ticket away from unsupported closure-only handling and refined it as a normal documentation implementation task backed by three verified repository gaps: missing `docs/releases/v0.30.0.md`, missing refresh/recovery wording in `README.md`, and missing stale-input troubleshooting guidance in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`. No new child tickets, attachments, planning documents, or relation writes were materialized in this refinement run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence fixes the baseline: typed helpers consume exactly one authoritative `dvault.support-bundle.v1`; raw `dvault.model.v1` is not a direct generator input; `DVaultTypedReadModelMetadataSourceFingerprint` drift is `DMV1961`; PIT and bridge helpers depend on request-bound `ReadShape` evidence supplied through `CreateSupportBundleDiagnostics`.
- The missing repository evidence is bounded to three concrete gaps: add `docs/releases/v0.30.0.md`, add refresh/recovery wording to `README.md:371-390`, and add stale-input troubleshooting guidance to `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-181`.
- `src/DCoding.Data.DVault.Analyzers/README.md:67-92` already carries the detailed DMV1960/1961/1963/1964/1967/1969 mapping; treat that as the v1 wording baseline and only edit it if needed to keep cross-document phrasing consistent.
- Sibling tickets `06F8KZP9XJ868GY6GT934QVFH4`, `06F8KZPN02NWFGMRC2Q1PKYKDR`, and `06F8KZPZZE8VZEBANP5MPN8HH8` are `done`, so this ticket is documentation follow-through only.
- The repository still contains `.gicket/relations/H8/0R/06F8KZPZZE8VZEBANP5MPN8HH8--06F8KZQAWZ7QRGB68KB21C9B0R--blocks.json` even though `.gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/ticket.json` reports `is-blocked=false`; treat that as non-blocking housekeeping, not closure evidence.

### Scope In
- Update `README.md` typed-helper guidance to document the consumer recovery path after metadata changes: re-export the authoritative support bundle, update or remove a stale pinned `DVaultTypedReadModelMetadataSourceFingerprint`, and understand that stale or incompatible inputs surface through existing `DMV1960` and `DMV1961` diagnostics.
- Update `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` with an explicit stale-input troubleshooting example or checklist that shows re-running support-bundle export and re-supplying representative `CreateSupportBundleDiagnostics` requests when PIT or bridge helper generation depends on request-bound `ReadShape` evidence.
- Add `docs/releases/v0.30.0.md` as the current documentation baseline for typed-helper freshness, support-bundle refresh workflow, and stale-input troubleshooting.
- Keep `src/DCoding.Data.DVault.Analyzers/README.md` aligned with the shipped DMV196x baseline; only add wording if needed to make helper-specific skip behavior or freshness phrasing consistent with the README and architecture docs.

### Scope Out
- New runtime diagnostics, source-generator behavior changes, or test-harness work already covered by done sibling tickets.
- Direct parsing of raw `dvault.model.v1`, source callbacks, or literal metadata objects by the generator.
- Support-bundle publication, attachment, routing, background refresh, or invented runtime requests.
- Rewriting historical release notes such as `docs/releases/v0.29.0.md` as if earlier behavior changed.
- Ticket-metadata housekeeping such as stale relation-file cleanup unless a separate bounded relation write is executed outside this documentation pass.

## Acceptance Criteria
- `README.md` explicitly says the authoritative support bundle must be refreshed after metadata changes, stale fingerprint pins must be updated or removed, `DMV1961` is drift on a pinned fingerprint, and `DMV1960` covers missing, invalid, incompatible, non-authoritative, or ambiguous bundle input.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` includes at least one consumer-owned troubleshooting example or checklist for re-exporting the support bundle and re-running representative `CreateSupportBundleDiagnostics` when stale PIT or bridge helper inputs or missing request-bound `ReadShape` evidence block generation; the wording must not imply that DVault invents requests or routes bundles.
- A new `docs/releases/v0.30.0.md` file exists and records this documentation boundary as the current baseline, pointing readers at the current v1 typed-helper contract instead of rewriting historical release-note claims.
- The targeted docs consistently state that raw `dvault.model.v1` artifacts are not direct generator inputs and that PIT or bridge helper generation depends on request-bound `ReadShape` facts inside the authoritative support bundle.
- The final documentation set preserves the existing DMV196x baseline from the analyzer README and makes clear that unsupported PIT or bridge facts suppress only the affected helper while other supported helpers from the same bundle can still generate.

## Definition of Done
- Repository-visible documentation evidence exists for each verified gap: the README recovery wording, the design-time troubleshooting addition, and a new `docs/releases/v0.30.0.md` file.
- Documentation changes stay within `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md` if needed for wording alignment, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, and `docs/releases/v0.30.0.md`.
- The updated docs reuse the already-implemented typed-helper contract and DMV196x behavior from `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md` and `src/DCoding.Data.DVault.Analyzers/README.md`; they do not reopen architecture or change product behavior.
- No additional PO clarification is required before developer handoff once those bounded documentation changes land.

## Implementation Notes
- Use `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:18-22,134-143` and `src/DCoding.Data.DVault.Analyzers/README.md:67-92` as the authoritative wording source for DMV1960, DMV1961, DMV1963, DMV1964, DMV1967, DMV1969, and skip-only-the-affected-helper behavior.
- Treat the current ticket as a documentation implementation task, not a closure audit; the missing repository evidence is already known and should not be reopened as an architecture question.
- The README change should add a concrete recovery sequence: metadata changes -> regenerate the reviewed `dvault.support-bundle.v1` -> update or remove any stale pinned fingerprint -> rebuild; for PIT and bridge helper gaps, regenerate representative request-bound diagnostics so the support bundle carries the needed `readShape` evidence.
- Keep the release-note change additive at `docs/releases/v0.30.0.md`; do not rewrite `docs/releases/v0.29.0.md` beyond any minimal forward link consistent with existing conventions.
- No child-ticket split is justified from current evidence.
- No child tickets, attachments, planning documents, or relation writes were materialized in this refinement run; the stale incoming `.gicket/relations/H8/0R/...--blocks.json` edge remains separate non-blocking housekeeping because `ticket.json` already reports `is-blocked=false`.

## Open Questions
- none

## Follow-Up Questions
- After this documentation pass lands, should a shorter adopter-facing freshness checklist also be mirrored into `docs/production-adoption-checklist.md`?
- Should runtime or relation housekeeping separately reconcile the stale incoming `06F8KZPZZE8VZEBANP5MPN8HH8 --blocks--> 06F8KZQAWZ7QRGB68KB21C9B0R` file that still exists in `.gicket/relations/H8/0R/` even though the current ticket state is `is-blocked=false`?

## Risks
- A partial update that touches the README or release notes without the design-time troubleshooting example can leave adopters without the documented recovery path for stale bundle or fingerprint inputs.
- If new wording diverges from the already-landed analyzer README and typed-helper contract, documentation may drift from the implemented DMV196x behavior.

## Split Recommendations
- No split recommended; remaining work is a single bounded documentation pass across the three verified repository gaps plus any minimal analyzer README wording alignment.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update analyzer README, support-bundle guidance, README, release notes, and troubleshooting examples for stale typed helper inputs.