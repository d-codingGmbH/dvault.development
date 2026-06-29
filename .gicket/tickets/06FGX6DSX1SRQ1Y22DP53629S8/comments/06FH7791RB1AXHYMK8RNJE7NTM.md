[gicket-bot] PO-critic review contract

Summary
- Closure-only audit failed: the ticket contract is concrete enough for normal development, but the current repository and branch still lack the v0.50.0 release-note and current-baseline doc updates, so this cannot be approved as already-satisfied work.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/description.md:8` marks the PO handoff as `ready_for_po_critic`, and `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/description.md:54-55` shows `## Open Questions` -> `none`.
- `git ls-files docs/releases/v0.50.0.md CHANGELOG.md README.md docs/package-compatibility.md docs/manual-nuget-publication.md docs/local-validation.md docs/plans/shared-implementation-standards.md docs/production-adoption-checklist.md tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` listed every in-scope file except `docs/releases/v0.50.0.md`, confirming the required release-note artifact is still absent.
- `CHANGELOG.md:5` still starts with `## v0.49.0 - Modeling and Generator Parity Refinement`, and `CHANGELOG.md:14` still links to `docs/releases/v0.49.0.md`, so the v0.50.0 top-entry acceptance criteria and definition of done are not yet satisfied.
- `README.md:187,191,197`, `docs/package-compatibility.md:57-60`, and `docs/manual-nuget-publication.md:98` still say the current release-note and changelog cross-references remain on v0.49.0 until the v0.50.0 update lands.
- `docs/plans/shared-implementation-standards.md:92,115,136,249` still describes the current compatibility contract as v0.49.0 and still forbids consumer-facing `0.49.0`, not `0.50.0`.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:39-40,87-88,132-133,634-645` already preserves the `8.50.0` / `10.50.0` package-line baseline, rejects stale `8.49.0` / `10.49.0`, and keeps the `.NET 10 SDK` analyzer-host guardrail, matching the ticket clarification that this is preserve-and-align work rather than an open version-selection problem.
- `git show --stat --summary --oneline --no-patch HEAD` reports `07d117dd0 [06FGX6DSX1SRQ1Y22DP53629S8] lease claim po-critic`, and `git diff --name-only HEAD^ HEAD` shows only `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/...` files changed, so the branch head contains ticket metadata and lease activity but no repository implementation for the closure-only deliverables.

Blocking findings
- This run is explicitly gated as a closure-only audit, but the repository still lacks required acceptance artifacts: `docs/releases/v0.50.0.md` is absent, `CHANGELOG.md` is still headed by v0.49.0, and the current-baseline cross-links remain parked on v0.49.0.
- The branch head only shows po-critic lease and ticket metadata changes, not the documentation updates required to satisfy the ticket, so the closure-only contract is unsupported by current repository state.

Required PO actions
- Fix the ticket routing and contract mismatch: either reclassify this as a normal pre-development developer-handoff ticket, or create and route a concrete follow-up developer ticket for the outstanding repository work instead of treating it as closure-only.
- If the ticket is rerouted for development, keep the acceptance surface explicit about whether ancillary v0.49.0 references such as `docs/production-adoption-checklist.md` remain follow-up-only or become in-scope.

Open issues ledger
- critic-item-1 [required-po-action] Fix the ticket routing and contract mismatch: either reclassify this as a normal pre-development developer-handoff ticket, or create and route a concrete follow-up developer ticket for the outstanding repository work instead of treating it as closure-only.
- critic-item-2 [required-po-action] If the ticket is rerouted for development, keep the acceptance surface explicit about whether ancillary v0.49.0 references such as `docs/production-adoption-checklist.md` remain follow-up-only or become in-scope.
- critic-item-3 [blocking-finding] This run is explicitly gated as a closure-only audit, but the repository still lacks required acceptance artifacts: `docs/releases/v0.50.0.md` is absent, `CHANGELOG.md` is still headed by v0.49.0, and the current-baseline cross-links remain parked on v0.49.0.
- critic-item-4 [blocking-finding] The branch head only shows po-critic lease and ticket metadata changes, not the documentation updates required to satisfy the ticket, so the closure-only contract is unsupported by current repository state.

Missing examples / edge cases
- The contract intentionally leaves ancillary docs outside the main acceptance surface, but `docs/production-adoption-checklist.md:11,139-140` still treats v0.49.0 as the current release-note baseline; PO should confirm whether that inconsistency is acceptable until a follow-up lands.
- The contract also leaves `src/DCoding.Data.DVault.Analyzers/README.md` out of scope; if it still carries v0.49.0 current-release wording, PO should decide whether that is acceptable package-local drift or deserves a separate cleanup ticket.

Risky assumptions
- Assuming closure-only status is appropriate even though the repository still needs real documentation work.
- Assuming packaged README and package-verifier guardrails are enough to cover non-packaged planning and adoption docs; the repository still shows stale v0.49.0 references outside the packaged surfaces.
- Assuming the existing verifier guardrails eliminate the need for a developer pass; they only preserve version and analyzer constraints and do not create the missing release-note, changelog, and document updates.

AC / test suggestions
- If rerouted to development, keep acceptance checks anchored to concrete file outcomes: new `docs/releases/v0.50.0.md`, a top-of-file v0.50.0 changelog entry, updated v0.50.0 links in `README.md`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md`, and v0.50.0 wording in `docs/plans/shared-implementation-standards.md`.
- Keep a manual PO verification step that the v0.50.0 release note contains only already-landed repository-backed value and does not introduce a provider-performance placeholder.

Implementation watchouts
- Do not expand analyzer compatibility during this ticket; repository evidence still supports only the `.NET 10 SDK` build-host baseline for `DCoding.Data.DVault.Analyzers`.
- Preserve the existing stale-version guardrails for `8.49.0` / `10.49.0`; this ticket is alignment work, not package-line re-selection.
- Do not blur the documentation release label with a consumer-facing package version; current guardrails correctly keep `v0.50.0` separate from `8.50.0` / `10.50.0`.

Non-blocking notes
- As a normal pre-development ticket, the contract is otherwise bounded and coherent: scope, acceptance criteria, definition of done, and implementation notes are concrete, and `## Open Questions` is explicitly `none`.
- No split recommendation is currently justified by repository evidence; the work remains one bounded documentation-and-verification alignment task once routing is corrected.

Split recommendations
- No split recommended after the closure-only routing mismatch is corrected.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment