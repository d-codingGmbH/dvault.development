[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract has no open questions, but direct repository and branch-diff evidence show the DB2 package-verification work is already present on develop, so this ticket no longer identifies a concrete remaining developer delta.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9G8HJJDJH4KF9VK6TZ8B1Z0/description.md:18-46 scopes this ticket to DB2 package-verification counts, IBM.EntityFrameworkCore dependency assertions, packaged README/XML checks, and symbol expectations; description.md:48-49 records Open Questions as none.
- tools/pack-release-packages.sh:8-16,57-58 already packs src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj as part of the runtime package family for package lines 8.34.0 and 10.34.0.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:18-21,32-55,120-125,458-479 already expects package lines 8.34.0 and 10.34.0, includes DCoding.Data.DVault.Db2 in the expected package set, derives artifact counts from that eight-package set, and checks packaged README install commands for every runtime package including DB2.
- tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:10-23,25-48,650-702 already models DB2 package verification and per-target IBM.EntityFrameworkCore versions 8.0.0.400 and 10.0.0.100.
- tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs:31-65,85-102 already asserts the DB2 integration-project package matrix and the DB2 provider project package references.
- git show --name-only 3f272aec69dd lists only .gicket/tickets/06F9G8HJJDJH4KF9VK6TZ8B1Z0 description/comment/event/ticket files in the po->po-critic handoff commit; no verifier source, tool, or test files were part of the handoff commit.
- git diff --name-status develop..HEAD shows only .gicket relation/ticket metadata differences, and git diff --name-only develop..HEAD -- tools/pack-release-packages.sh tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs returned no paths.
- .gicket/tickets/06F9G8GZ384VKA7RVF039WKX1M/description.md:23-27 says comprehensive package verifier updates stay with ticket 06F9G8HJJDJH4KF9VK6TZ8B1Z0 and docs stay with 06F9G8HRZ72XP5Z7FNWM6MBMQC, which conflicts with the landed repository state above and shows this ticket has drifted from reality.

Blocking findings
- The ticket is still positioned as a normal pre-development handoff, but the branch history and develop..HEAD diff show no remaining implementation delta for the verifier surfaces; handing this to a developer would not point to concrete unfinished work.
- The repository already contains the DB2 package-verification implementation on develop across the pack script, package verifier tool, verifier tests, and version-matrix tests, so the current scope/acceptance criteria need PO re-triage before any developer handoff.

Required PO actions
- Decide whether this ticket should be closed as already satisfied / duplicate / closure-only, or rewritten to the exact remaining work item.
- If the intended remaining work is verification-adjacent documentation only, retarget the ticket explicitly to docs/manual-nuget-publication.md alignment or fold that work into task 06F9G8HRZ72XP5Z7FNWM6MBMQC.

Open issues ledger
- critic-item-1 [required-po-action] Decide whether this ticket should be closed as already satisfied / duplicate / closure-only, or rewritten to the exact remaining work item.
- critic-item-2 [required-po-action] If the intended remaining work is verification-adjacent documentation only, retarget the ticket explicitly to docs/manual-nuget-publication.md alignment or fold that work into task 06F9G8HRZ72XP5Z7FNWM6MBMQC.
- critic-item-3 [blocking-finding] The ticket is still positioned as a normal pre-development handoff, but the branch history and develop..HEAD diff show no remaining implementation delta for the verifier surfaces; handing this to a developer would not point to concrete unfinished work.
- critic-item-4 [blocking-finding] The repository already contains the DB2 package-verification implementation on develop across the pack script, package verifier tool, verifier tests, and version-matrix tests, so the current scope/acceptance criteria need PO re-triage before any developer handoff.

Missing examples / edge cases
- If the ticket is retargeted, state whether the only remaining gap is current manual-publication checklist wording versus the already-landed verifier/tooling baseline.
- If docs/manual-nuget-publication.md is still in scope, specify whether the change is limited to current checklist/baseline text or also includes historical release-note families that intentionally remain seven-package records.

Risky assumptions
- Assuming no hidden remaining verifier delta exists outside the inspected pack script, verifier tool, verifier tests, version-matrix tests, README, and manual publication checklist, because the relevant develop..HEAD implementation diffs were empty.

AC / test suggestions
- If PO keeps the ticket open for remaining checklist work, add acceptance criteria that docs/manual-nuget-publication.md reflects the DB2-inclusive package family and the 8.34.0 / 10.34.0 lines without reopening unrelated runtime or historical release-note scope.
- If PO closes the ticket as already satisfied, add closure evidence citing the landed verifier paths and the empty develop..HEAD implementation diff so future reviewers do not reopen the same scope.

Implementation watchouts
- Do not send a developer back into PackageVerifier.cs, PackageVerifierTests.cs, or EfCoreProviderVersionMatrixTests.cs unless PO can name a concrete unmet behavior; those verifier surfaces already contain the DB2 matrix on develop.
- docs/manual-nuget-publication.md still carries seven-package and 8.33.0 / 10.33.0 wording, so any remaining work should be explicitly framed as checklist/document alignment rather than re-implementing package verification.

Non-blocking notes
- .gicket/tickets/06F9G8HJJDJH4KF9VK6TZ8B1Z0/description.md:48-49 records Open Questions as none, so the return is not caused by unresolved product questions.
- docs/manual-nuget-publication.md:9-28,42-49,96-114 still describes a seven-package family and 8.33.0 / 10.33.0 lines, which explains the stale narrative around already-landed verifier changes.

Split recommendations
- No new child-ticket split is needed.
- If the only remaining task is checklist/document wording, consolidate it with 06F9G8HRZ72XP5Z7FNWM6MBMQC instead of keeping a separate developer ticket whose original verifier scope is already landed.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment