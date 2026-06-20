[gicket-bot] PO-critic review contract

Summary
- The delivery contract fixes the earlier documentation-surface blocker and has no open questions, but it still misstates the live child-ticket relation state: current repository evidence shows removed parentOf links rather than active ones.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/description.md lines 23-24 and 38-45 now explicitly include docs/production-adoption-checklist.md in scope, map planning release v0.42.0 to package lines 8.42.0 and 10.42.0, and keep ## Open Questions as - none at line 52.
- .gicket/releases/06FE4PMQ8GNKY6X54F8D16AVGC.json describes v0.42.0 - Provider Performance Evidence and Tuning and says the planning release maps to package versions 8.42.0 and 10.42.0.
- Current repository surfaces are still on the v0.41 baseline: docs/production-adoption-checklist.md lines 9 and 12-14 and 122-123, README.md lines 18-41, docs/package-compatibility.md lines 11-16, docs/manual-nuget-publication.md lines 22-35 and 84-110, docs/local-validation.md lines 17-18, tools/pack-release-packages.sh lines 57-58, and tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs lines 28-29 and 534-545 all reference 8.41.0 and 10.41.0.
- benchmark-summary.json lines 31-56 mark the optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 lanes as executionStatus=skipped when the matching DVAULT_TEST_* connection string is unset, and lines <redacted> preserve selectedStrategy or plannedReadStrategy with persistedOutcome=not executed for latest-satellite, PIT, and bridge guidance rows.
- docs/plans/provider-optimization-evidence-matrix.md lines 16-19, 216-221, and 234-245 plus docs/plans/provider-optimization-gap-matrix.md lines 71-79 and 87-90 already define the completed-vs-skipped or diagnostics or smoke posture and the current threshold and fallback boundaries the story is intended to ratify.
- git log --oneline -n 8 on ticket/06FE4QNWP9606HTB92MTVQMYDG-story-define-v0-42-provider-evidence-and-tuning ends at 414e22629 and 5051a08aa, and git diff --stat develop..HEAD shows only .gicket ticket artifacts and description changes, so this branch is still refinement-only, which is expected for a pre-development story.
- Active relation files under .gicket/relations for this ticket include blocks and relates, for example .gicket/relations/DG/SR/06FE4QNWP9606HTB92MTVQMYDG--06FE4QP6FB892E7TJMB47A3MSR--blocks.json and ...--relates.json; no matching ...--parentOf.json files were found, while .gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/events/06FE4VB050VSHAB5WTPD56SPSC.json, 06FE4VB3XTFC082F0ZD6K486QR.json, and 06FE4VC3M3MEFSGX06ESR95EHM.json are TicketRelationRemoved events for parentOf links.

Blocking findings
- The persisted contract still says live .gicket relation files already show parentOf links for the downstream split, but direct repository inspection shows those parentOf relations are not live: the active relation files are blocks or relates, and the ticket event history records parentOf removals.
- Because the PO summary, clarifications, and implementation notes use that incorrect live-state claim to conclude that no new relation write is needed, the authoritative child-ticket relation model for the already-materialized split is still unclear at ticket level.

Required PO actions
- Correct the delivery contract and latest PO wording to match the observed relation state. Either restore the intended active parentOf relations in .gicket/relations, or rewrite the contract to say the split is currently represented by active blocks or relates links plus historical parentOf removal events.

Open issues ledger
- critic-item-1 [required-po-action] Correct the delivery contract and latest PO wording to match the observed relation state. Either restore the intended active parentOf relations in .gicket/relations, or rewrite the contract to say the split is currently represented by active blocks or relates links plus historical parentOf removal events.
- critic-item-2 [blocking-finding] The persisted contract still says live .gicket relation files already show parentOf links for the downstream split, but direct repository inspection shows those parentOf relations are not live: the active relation files are blocks or relates, and the ticket event history records parentOf removals.
- critic-item-3 [blocking-finding] Because the PO summary, clarifications, and implementation notes use that incorrect live-state claim to conclude that no new relation write is needed, the authoritative child-ticket relation model for the already-materialized split is still unclear at ticket level.

Missing examples / edge cases
- If only three downstream tickets should remain blocker-style links, clarify how the other seven split tickets are expected to appear in live relations when parentOf is absent.
- If the removed parentOf relations are intentional, clarify whether closure should leave them removed permanently or restore a parent-child hierarchy before later workflow stages.

Risky assumptions
- The current contract assumes historical event evidence is enough to claim live parentOf state even though the corresponding relation files are absent.
- The current contract assumes active blocks or relates links are semantically equivalent to the claimed parent-child split without stating that explicitly.

AC / test suggestions
- Add a ticket-level verification item that the authoritative downstream-split relations exist in .gicket/relations after refinement and match the contract wording.
- Keep the current acceptance wording that skipped placeholder rows with persistedOutcome=not executed and plannedReadStrategy guidance cannot be promoted to measured timing claims without a provider-configured artifact triplet.

Implementation watchouts
- Repository delivery work is still downstream: docs/releases/v0.42.0.md is currently absent, and the current docs and tooling remain pinned to 8.41.0 and 10.41.0.
- Optional-provider benchmark rows in benchmark-summary.json remain skipped placeholders even when strategy names are preserved; downstream work must not turn those rows into measured timing claims.
- This branch is still refinement-only, so any repository doc or tooling updates belong to the downstream implementation tickets rather than this story branch.

Non-blocking notes
- The earlier documentation-surface blocker is addressed: docs/production-adoption-checklist.md is now explicitly in the coordinated v0.42 scope.

Split recommendations
- No new split is needed beyond the existing downstream tickets; correct the authoritative relation-state wording first, then re-run PO-critic.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment