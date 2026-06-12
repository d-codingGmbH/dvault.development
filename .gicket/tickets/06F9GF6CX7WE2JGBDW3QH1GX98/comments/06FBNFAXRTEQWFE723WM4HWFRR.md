[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F9GF6CX7WE2JGBDW3QH1GX98\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F9GF6CX7WE2JGBDW3QH1GX98-task-document-binary-hash-storage-adoption-guida\u0027 and commit \u00273203c60aa944\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F9GF6CX7WE2JGBDW3QH1GX98-task-document-binary-hash-storage-adoption-guida\u0027 from source \u00273203c60aa944\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F9GF6CX7WE2JGBDW3QH1GX98-task-document-binary-hash-storage-adoption-guida\u0027.",
    "Evidence: \u0060git diff --name-only develop...3203c60aa944\u0060 shows the claimed change set updates \u0060README.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, \u0060docs/production-adoption-checklist.md\u0060, \u0060docs/releases/v0.36.0.md\u0060, and \u0060hash-key-footprint.md\u0060, but no \u0060artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/...\u0060 files.",
    "Evidence: \u0060git show 3203c60aa944:docs/releases/v0.36.0.md\u0060 and \u0060git show 3203c60aa944:hash-key-footprint.md\u0060 both reference \u0060artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.*\u0060 and \u0060hash-key-footprint.*\u0060 as the checked-in SQLite-local evidence bundle.",
    "Evidence: \u0060git show 3203c60aa944:artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.md\u0060 exited 128 with \u0060fatal: path ... exists on disk, but not in \u00273203c60aa944\u0027\u0060, confirming the cited bundle is not committed in the claimed implementation.",
    "Evidence: \u0060git grep -n -E \u00270\\.35\\.0|8\\.35\\.0|10\\.35\\.0|0\\.36\\.0|8\\.36\\.0|10\\.36\\.0\u0027 3203c60aa944 -- README.md docs/production-adoption-checklist.md docs/manual-nuget-publication.md docs/releases/v0.36.0.md\u0060 shows the current-baseline docs moved to v0.36.0 / 8.36.0 / 10.36.0 wording and do not leave current-baseline 0.35 package lines behind.",
    "Evidence: \u0060git grep\u0060 over README, \u0060docs/production-adoption-checklist.md\u0060, and \u0060docs/releases/v0.36.0.md\u0060 shows the committed docs explicitly document HexString default, Binary opt-in, digest-length-based provider storage types, DB2 live-schema unsupported status, and the redacted diagnostics/support-bundle fields.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/documentation, area/hashing, area/performance, area/provider-support, area/schema, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F9GF6CX7WE2JGBDW3QH1GX98-task-document-binary-hash-storage-adoption-guida\u0027.",
    "Evidence: Ticket history references implementation commit \u00273203c60aa944\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: README current-baseline guidance is advanced from v0.35.0 / 8.35.0 / 10.35.0 to v0.36.0 / 8.36.0 / 10.36.0, and the docs explicitly keep v0.36.0 as the planning or release-note label rather than a consumer NuGet version. (README and the new release/publication guidance move current-baseline wording to v0.36.0 with consumer package lines 8.36.0 and 10.36.0, and they explicitly say v0.36.0 is the planning/release-note label rather than a consumer NuGet version.).",
    "AC check passed: User-facing docs state that logical hash keys remain lowercase hexadecimal strings, HexString remains the default compatible storage profile, Binary is explicit opt-in only, and DVault does not automatically migrate, backfill, dual-write, or repair persisted keys. (README, the production adoption checklist, and the v0.36.0 release note all state that logical hash keys stay lowercase hexadecimal strings, HexString remains the default compatible profile, Binary is explicit opt-in, and DVault does not automatically migrate, backfill, dual-write, or repair persisted keys.).",
    "AC check passed: The updated documentation includes the bounded built-in provider column-type guidance for HashKey and ParticipantReference storage, with widths derived from the active digest length and DB2 live-schema support still documented as unsupported. (The updated docs include digest-length-based HashKey and ParticipantReference storage guidance for SQLite, Oracle, PostgreSQL, SQL Server, DB2, and MySQL, and they keep DB2 live-schema reading documented as unsupported.).",
    "AC check passed: Diagnostics and support-bundle guidance explains how algorithm and storage choices are surfaced through algorithmId, digestByteLength, digestEncoding, hashKeyStorageProfile, and store-type, value-format, or conversion facts while preserving the redacted output boundary. (README, the production adoption checklist, and the release note document \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, \u0060hashKeyStorageProfile\u0060, provider store type, provider value format, and conversion behavior while preserving the redaction boundary.).",
    "AC check passed: Package verification and manual-publication guidance is refreshed so dual package-line install examples, README validation language, and hash-storage documentation all align with the v0.36.0 baseline. (README installation/local-validation wording, the manual publication checklist, and the hash-storage guidance are aligned on the v0.36.0 baseline and the dual 8.36.0 / 10.36.0 consumer package lines.).",
    "DoD check passed: README, docs/production-adoption-checklist.md, docs/manual-nuget-publication.md, and docs/releases/v0.36.0.md tell one consistent v0.36.0 story without leaving current-baseline references on 0.35 / 8.35 / 10.35. (README, \u0060docs/production-adoption-checklist.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, and \u0060docs/releases/v0.36.0.md\u0060 present a consistent v0.36.0 current-baseline story; the remaining v0.35.0 mentions are historical carry-forward references rather than current-baseline package guidance.).",
    "DoD check passed: Historical v0.35.0 material remains historical; only the current baseline and release-line guidance move forward. (The reviewed docs keep older v0.35.0 material in historical sections instead of presenting 0.35 / 8.35 / 10.35 as the current baseline.).",
    "DoD check passed: No documentation claim implies new runtime behavior, package publication, automatic migration, DB2 live-schema support, or cross-provider measured wins that the repository does not currently prove. (The documentation keeps the behavior claims bounded: HexString stays default, Binary stays opt-in, no automatic migration is claimed, DB2 live-schema remains unsupported, and performance wording stays scoped to SQLite-local evidence.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Production adoption and release guidance cite the landed SQLite benchmark bundle and footprint sidecars for storage-footprint and lookup or read tradeoff evidence, and keep any performance claims scoped to that checked-in evidence. (The docs cite \u0060artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/\u0060 and its sidecars as checked-in evidence, but commit \u00603203c60aa944\u0060 does not contain that bundle, so the release/adoption guidance does not actually point to landed committed benchmark evidence.).",
    "DoD check failed: Documentation links point to the existing hash-key storage contract, stable-hashing contract, benchmark-summary triplet, and hash-key-footprint sidecars instead of duplicating unsupported implementation detail. (The new documentation links to the \u006006F9GF66...\u0060 benchmark-summary and hash-key-footprint sidecars, but those linked files are not part of commit \u00603203c60aa944\u0060, so not all evidence links resolve to existing committed repository content.).",
    "DoD check failed: The implementation can be completed as a documentation-only change set without further PO decisions. (The claimed delivery is not self-contained as a documentation-only completion because it depends on a benchmark bundle that is referenced by the docs but not committed in the claimed implementation.).",
    "The new v0.36.0 docs and \u0060hash-key-footprint.md\u0060 cite a checked-in SQLite benchmark bundle under \u0060artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/\u0060, but that bundle is not part of commit \u00603203c60aa944\u0060, leaving the documented evidence links unresolved in the claimed implementation."
  ],
  "evidence": [
    "\u0060git diff --name-only develop...3203c60aa944\u0060 shows the claimed change set updates \u0060README.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, \u0060docs/production-adoption-checklist.md\u0060, \u0060docs/releases/v0.36.0.md\u0060, and \u0060hash-key-footprint.md\u0060, but no \u0060artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/...\u0060 files.",
    "\u0060git show 3203c60aa944:docs/releases/v0.36.0.md\u0060 and \u0060git show 3203c60aa944:hash-key-footprint.md\u0060 both reference \u0060artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.*\u0060 and \u0060hash-key-footprint.*\u0060 as the checked-in SQLite-local evidence bundle.",
    "\u0060git show 3203c60aa944:artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.md\u0060 exited 128 with \u0060fatal: path ... exists on disk, but not in \u00273203c60aa944\u0027\u0060, confirming the cited bundle is not committed in the claimed implementation.",
    "\u0060git grep -n -E \u00270\\.35\\.0|8\\.35\\.0|10\\.35\\.0|0\\.36\\.0|8\\.36\\.0|10\\.36\\.0\u0027 3203c60aa944 -- README.md docs/production-adoption-checklist.md docs/manual-nuget-publication.md docs/releases/v0.36.0.md\u0060 shows the current-baseline docs moved to v0.36.0 / 8.36.0 / 10.36.0 wording and do not leave current-baseline 0.35 package lines behind.",
    "\u0060git grep\u0060 over README, \u0060docs/production-adoption-checklist.md\u0060, and \u0060docs/releases/v0.36.0.md\u0060 shows the committed docs explicitly document HexString default, Binary opt-in, digest-length-based provider storage types, DB2 live-schema unsupported status, and the redacted diagnostics/support-bundle fields.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/documentation, area/hashing, area/performance, area/provider-support, area/schema, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F9GF6CX7WE2JGBDW3QH1GX98-task-document-binary-hash-storage-adoption-guida\u0027.",
    "Ticket history references implementation commit \u00273203c60aa944\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Commit the referenced \u0060artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/\u0060 benchmark-summary and hash-key-footprint sidecars with the documentation update, or revise the new v0.36.0 docs so they cite only committed repository evidence while still satisfying the ticket contract.",
    "After the evidence-link blocker is fixed, rerun tester verification and the supported repository validation path for \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06F9GF6CX7WE2JGBDW3QH1GX98-task-document-binary-hash-storage-adoption-guida",
  "commitSha": "3203c60aa944"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F9GF6CX7WE2JGBDW3QH1GX98`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F9GF6CX7WE2JGBDW3QH1GX98-task-document-binary-hash-storage-adoption-guida`