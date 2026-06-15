[gicket-bot] PO refinement contract

Summary
- Resolved the PO-critic version-line blocker by re-scoping this as a docs-only `v0.39.0` documentation update: the new release note and changelog entry may document the provider-evidence baseline, but they must not assert new consumer package-version lines because repository evidence still exposes only `8.38.0` and `10.38.0`. No child tickets, relation changes, attachments, description writes, or additional planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Yes. This ticket is allowed to produce a docs-only `v0.39.0` release note and matching changelog entry. For this ticket, `v0.39.0` is a documentation/release-note label for the provider-evidence baseline, not a claim that new consumer package-version lines exist.
- critic-item-2: `answered` - `v0.39.0` in this ticket must not mention consumer package lines. No authoritative `8.39.0` or `10.39.0` release-planning/version-alignment surface is visible in the repository, so any such claim would be invented. If package lines move later, that requires separate release-planning/version-alignment work outside this ticket.
- critic-item-3: `answered` - The developer should proceed with a docs-only `v0.39.0` note and explicitly avoid package-line claims. This refinement supersedes the earlier unbacked `8.39.0` / `10.39.0` assumption, so the developer does not need to invent release-version facts or wait on undefined alignment work before writing the bounded docs.

Clarifications
- This ticket is explicitly allowed to ship a docs-only `v0.39.0` release note and matching changelog entry even though the repository still exposes `8.38.0` / `10.38.0` consumer package lines.
- The `v0.39.0` surfaces in this ticket must not claim, imply, or document `8.39.0`, `10.39.0`, or a consumer-facing `0.39.0` package version.
- The earlier planning-note sentence that preserved an assumed `8.39.0` / `10.39.0` release-note pattern is superseded by this refinement decision because no repo-backed version-alignment evidence is visible.
- No bounded child-ticket creation, relation changes, attachment writes, description writes, or additional planning-document writes were materialized in this pass because repository evidence already resolved the blocker.

Scope In
- Update `docs/performance-profiles.md` so measured provider evidence is clearly separated from follow-up recommendations and readers are directed to the evidence and gap matrices for canonical details.
- Create `docs/releases/v0.39.0.md` as a docs-only release record for the provider-evidence baseline, caveats, and follow-up posture, without consumer package-version claims.
- Add the matching `CHANGELOG.md` entry that points to the new `v0.39.0` release note and stays consistent with the docs-only scope.

Scope Out
- Changing `README.md`, `docs/package-compatibility.md`, `docs/production-adoption-checklist.md`, `docs/manual-nuget-publication.md`, `tools/pack-release-packages.sh`, package verification code, or any other package-version surface to `8.39.0` or `10.39.0`.
- Documenting or publishing new consumer package versions, package approval, package hashes, or release automation outcomes.
- Rerunning benchmarks, generating new benchmark artifact triplets, changing benchmark schemas, or widening provider claims beyond the checked-in evidence baseline.
- Provider implementation work, diagnostics behavior changes, or DB2 claim expansion beyond the current evidence posture.

Open questions
- none

Follow-up questions
- Should a later release-planning/version-alignment ticket move the repository package-version surfaces to a future `8.39.0` / `10.39.0` baseline before any consumer-facing installation guidance changes?
- After the docs-only `v0.39.0` note lands, should other adopter-facing docs such as `docs/production-adoption-checklist.md` receive the same evidence-matrix and gap-matrix cross-links if drift appears there?
- When provider-configured benchmark bundles are added later, which gap-matrix rows should be promoted first from follow-up recommendations into release-note-ready completed timing claims?

Risks
- If another ticket later introduces repo-backed `8.39.0` / `10.39.0` version alignment, the docs-only `v0.39.0` wording may need a follow-up update to stay aligned with the new release baseline.
- Live ticket comment and relation reads were trust-policy blocked earlier in the session, so duplicate and relation conclusions still rely on the provided ticket snapshot; no blocking duplicate evidence is present in that snapshot.

Split recommendations
- No split recommended. The remaining work is one bounded docs-only task across `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, and `CHANGELOG.md`.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment