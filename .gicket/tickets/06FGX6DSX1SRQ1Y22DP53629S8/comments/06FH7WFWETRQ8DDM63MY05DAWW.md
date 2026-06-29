[gicket-bot] PO-critic review contract

Summary
- Updated PO refinement resolves the earlier closure-only mismatch and now defines a bounded pre-development documentation-alignment ticket with no open questions, so it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/description.md:8` marks the handoff as `ready_for_po_critic`, and `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/description.md:56-57` shows `## Open Questions` -> `none`, so the persisted delivery contract is eligible for developer approval.
- `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/description.md:15,24-39` explicitly scopes `docs/production-adoption-checklist.md` and `src/DCoding.Data.DVault.Analyzers/README.md` out, and limits the required edit surface to `docs/releases/v0.50.0.md`, `CHANGELOG.md`, `README.md`, `docs/package-compatibility.md`, `docs/manual-nuget-publication.md`, `docs/plans/shared-implementation-standards.md`, plus preserving `docs/local-validation.md` and existing verifier guidance.
- `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/comments/06FH7R69755120T53KM05QH8HG.md` answers the prior critic items and states that this existing ticket, not a closure-only audit or child split, is the vehicle for the remaining developer documentation work.
- `if [ -f docs/releases/v0.50.0.md ]; then echo present; else echo missing; fi` returned `missing`; `CHANGELOG.md:5,14`, `README.md:187,191,197`, `docs/package-compatibility.md:57-59`, and `docs/manual-nuget-publication.md:98` still point at v0.49.0, which matches the contract's description of the outstanding developer task instead of leaving hidden scope.
- `docs/plans/shared-implementation-standards.md:92,115,136,249` still uses the v0.49.0 compatibility wording the ticket calls out, so that acceptance item is concrete and locally verifiable.
- `docs/local-validation.md:3,17-18` and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:17,39-40,87-88,132-133,634-645` already preserve the `.NET 10 SDK` analyzer-host baseline, the `8.50.0` / `10.50.0` package lines, and rejection of stale `8.49.0` / `10.49.0` guidance, supporting the contract's preserve-and-align posture.
- `git log --oneline --decorate -n 6` shows the refinement loop `7a92fdab7` -> `58bcf314e` -> `d1340ef12` -> `17dc346cf`, and `git diff --name-only 58bcf314eaf5d4310c7ce1cbc99ece01bdad01d3..17dc346cf8f8a251341f4f7ada4517a53b7ff6d7` touches only `.gicket/...` files, confirming this is still a pre-development handoff and that missing repository edits belong to the next developer pass rather than a new PO refinement cycle.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract intentionally leaves `docs/production-adoption-checklist.md` and similar ancillary v0.49.0 references stale until a follow-up ticket; developers should not treat those files as completion blockers for this ticket.
- `src/DCoding.Data.DVault.Analyzers/README.md` is explicitly out of scope for this pass unless PO later widens acceptance.

Risky assumptions
- Current-release alignment remains bounded to the explicitly named files; if PO later wants all ancillary v0.49.0 references updated in the same pass, the acceptance surface will need another refinement cycle.
- The v0.50.0 release note will be assembled only from already-landed repository-backed value, as stated in the implementation notes; if new release claims are introduced, the ticket would need fresh PO review.

AC / test suggestions
- During developer completion review, verify that `docs/releases/v0.50.0.md` contains only completed repository-backed value and excludes provider-performance placeholder language.
- Check the finished diff with a targeted search for `docs/releases/v0.49.0.md`, `0.50.0`, `8.49.0`, and `10.49.0` across the in-scope files to confirm the current-release cross-links moved forward without regressing package-line guidance.

Implementation watchouts
- Do not expand analyzer compatibility beyond the existing `.NET 10 SDK` build-host baseline documented in `README.md:50`, `docs/package-compatibility.md:53`, `docs/manual-nuget-publication.md:38,94`, and enforced by `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:17,634-645`.
- Preserve the current `8.50.0` / `10.50.0` package lines and the stale-version guardrails for `8.49.0` / `10.49.0`; this ticket is documentation alignment, not version reselection.
- Keep `docs/production-adoption-checklist.md` and `src/DCoding.Data.DVault.Analyzers/README.md` out of scope unless PO explicitly reopens them.

Non-blocking notes
- The remaining work is still one documentation-alignment task; no child split or relation cleanup is justified by the current evidence.

Split recommendations
- No split recommended; keep the remaining work on ticket `06FGX6DSX1SRQ1Y22DP53629S8`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment