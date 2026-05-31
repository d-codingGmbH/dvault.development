[gicket-bot] PO-critic review contract

Summary
- The child coverage and repository baseline are in place for this tracking-only epic, but the persisted ticket metadata still contradicts the verified closure-only handoff state.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/description.md` contains `## Open Questions` with `- none` and states the epic is tracking-only closure/no-work-required.
- A direct search under `.gicket/relations` found no relation JSON targeting ticket `06F5Q93R4633D41Z21WQW3SVGR`, while five `parentOf` relations remain from this epic to `06F5Q93YXHSKABD2SABWY85S78`, `06F5Q9463M0RSHAJJX0F3D1DB0`, `06F5Q94D0JDMMWDXSRGWX1E4F0`, `06F5Q94KX65TXQ8EC75FWSD01W`, and `06F5Q94SQ086B2DZ1AKFDXGV94`.
- The repository baseline named in the parent contract exists in Git: `docs/architecture/dvault-v1-activity-tracing-contract.md`, `src/DCoding.Data.DVault/DataVaultActivityTracing.cs`, `docs/performance-profiles.md`, `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, `docs/releases/v0.23.0.md`, and `README.md`.
- `docs/architecture/dvault-v1-activity-tracing-contract.md` and `src/DCoding.Data.DVault/DataVaultActivityTracing.cs` both use ActivitySource `DCoding.Data.DVault` and include the same span vocabulary such as `dvault.save.single_request`, `dvault.read.latest_satellite`, and `dvault.maintenance.pit.rebuild`.
- `docs/performance-profiles.md`, `docs/releases/v0.23.0.md`, `README.md`, and the root benchmark triplet all cross-reference the v0.23.0 performance baseline and checked-in benchmark artifacts.

Blocking findings
- Because this review is limited to ticket-level readiness, that metadata contradiction leaves the automated handoff state ambiguous and is not clean enough for unattended progression.

Required PO actions
- Rerun PO handoff after the persisted ticket metadata matches the already-verified relation graph and child-completion evidence.

Open issues ledger
- critic-item-1 [required-po-action] Rerun PO handoff after the persisted ticket metadata matches the already-verified relation graph and child-completion evidence.
- critic-item-2 [blocking-finding] Because this review is limited to ticket-level readiness, that metadata contradiction leaves the automated handoff state ambiguous and is not clean enough for unattended progression.

Missing examples / edge cases
- None at the epic-contract level; the remaining gap is metadata alignment, not missing scope examples.

Risky assumptions
- Closure still assumes no new incoming relation or child-ticket reopen occurs after this review; the contract already calls for a final eligibility check before close.

AC / test suggestions
- Add an explicit closure checklist item in ticket metadata or workflow notes that the persisted labels must match the clean relation state before the ticket advances from PO-critic.

Implementation watchouts
- No parent-owned implementation work remains; if the ticket advances later, the next role should treat it as closure verification only, not as a new development slice.

Non-blocking notes
- none

Split recommendations
- No additional split recommended once the persisted metadata is aligned; the existing five-child decomposition is sufficient.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment