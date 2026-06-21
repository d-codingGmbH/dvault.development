[gicket-bot] PO-critic review contract

Summary
- Approve for dev. The delivery contract is bounded, has no open questions, and is grounded in checked-in storage/adoption evidence; the only inconsistency I found is a stale blocked-by note that conflicts with current relation state.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4R0H98K42XJY1NEDQX8KB4/description.md:28-33 defines five concrete acceptance criteria covering Binary vs HexString semantics, preflight metadata checks, fail-closed migration/rollback sequencing, provider caveats, and discoverability cross-links.
- .gicket/tickets/06FE4R0H98K42XJY1NEDQX8KB4/description.md:41-47 anchors the guide to checked-in sources: docs/plans/hash-key-storage-profile-contract.md, docs/getting-started.md, docs/production-adoption-checklist.md, and hash-key-footprint.md.
- .gicket/tickets/06FE4R0H98K42XJY1NEDQX8KB4/description.md:49-50 shows `## Open Questions` = `- none`, so there is no unresolved PO contract item blocking dev handoff.
- docs/plans/hash-key-storage-profile-contract.md:23-24,52-78 already define `HexString` vs `Binary`, the required metadata facts, and the fail-closed compatibility rule including the `sha1-v1` vs `sha256-160-v1` same-width incompatibility case that the guide must reuse.
- docs/getting-started.md:17-27,71-73 and README.md:64 already distinguish binary-first for new projects from reviewed migration for existing persisted storage; docs/production-adoption-checklist.md:100-106 already scopes storage claims to the SQLite bundle and names the support-bundle facts to compare.
- `git log --oneline -5 ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val` shows only PO/PO-critic workflow commits, and `git diff --stat develop..ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val -- .gicket/tickets/06FE4R0H98K42XJY1NEDQX8KB4 docs README.md hash-key-footprint.md` lists only `.gicket/tickets/06FE4R0H98K42XJY1NEDQX8KB4/...` changes with no `docs/` or `README.md` edits, which matches a pre-development ticket-quality branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Include an explicit same-width incompatibility example such as `sha1-v1` vs `sha256-160-v1` so column width alone is not misread as compatibility.
- Show the fail-closed path when support-bundle, translated metadata, and live-schema facts drift during preflight or between dry-run and cutover.
- Clarify rollback expectations if a consumer converts schema/data but post-cutover validation finds mismatched store type, value format, or conversion behavior.

Risky assumptions
- The guide assumes consumer-visible support-bundle or translated-metadata surfaces will continue exposing `algorithmId`, `digestByteLength`, provider store type, provider value format, and conversion behavior under the current terminology.

AC / test suggestions
- Add a reviewer checklist item that the new guide links from both `docs/getting-started.md` and `docs/production-adoption-checklist.md`, since those are the explicit discoverability entry points in the contract.
- Require one worked validation example that compares persisted facts before and after cutover and fails closed when an algorithm or digest-length change is mixed into the same change.
- Require wording review against `docs/plans/hash-key-storage-profile-contract.md` so the guide never implies public `byte[]` hash keys or automatic DVault migration tooling.

Implementation watchouts
- Do not widen provider claims beyond the checked-in SQLite footprint bundle in `hash-key-footprint.md` and `artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-<redacted>/...`.
- Keep `Binary` framed as physical-storage opt-in only; public, request, and diagnostic hash-key values must remain lowercase hexadecimal strings.
- Avoid mixing stable-hash algorithm changes into the same documented migration flow unless the guide explicitly says that combination fails closed and needs a separate reviewed plan.

Non-blocking notes
- The ticket is ready for dev from a PO-quality perspective even though the branch currently contains only `.gicket` metadata writes; the prompt explicitly treats missing implementation evidence as non-blocking for pre-development tickets.
- The relation sentence in description.md:16 looks stale relative to `ticket.json` and the done upstream story, but it does not materially blur the documentation scope or acceptance criteria.
- The follow-up questions at description.md:52-54 about provider-specific examples and extra release-note/package-compatibility links are sensible future work, not current blockers.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment