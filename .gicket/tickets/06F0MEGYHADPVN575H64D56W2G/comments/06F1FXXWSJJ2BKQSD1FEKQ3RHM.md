[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F0MEGYHADPVN575H64D56W2G\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract\u0027 and commit \u0027030ad7545a7a\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract\u0027 from source \u0027030ad7545a7a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract\u0027.",
    "Evidence: \u0060git rev-parse HEAD\u0060 returned \u0060030ad7545a7a5432849adcf0e4aed3f47f7122dc\u0060 on branch \u0060ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract\u0060.",
    "Evidence: \u0060git diff --name-status develop...030ad7545a7a\u0060 showed only \u0060.gicket/tickets/06F0MEGYHADPVN575H64D56W2G/...\u0060 additions/modifications and no \u0060docs/\u0060 or \u0060tests/DCoding.Data.DVault.Tests/Unit/\u0060 PIT contract files.",
    "Evidence: \u0060repository-list-directory docs\u0060 enumerated 30 entries under \u0060docs/\u0060, including multiple \u0060docs/plans/*.md\u0060 files, but did not include \u0060docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md\u0060.",
    "Evidence: \u0060repository-list-directory tests/DCoding.Data.DVault.Tests/Unit\u0060 enumerated 33 entries, including \u0060Snapshots/PublicApi/...\u0060, but there was no \u0060Snapshots/Contracts/\u0060 subtree and no \u0060PitAsOfReadContractSnapshotTests.cs\u0060 entry.",
    "Evidence: \u0060git ls-files\u0060 for the three developer-claimed artifact paths returned no tracked files.",
    "Evidence: \u0060git grep -n \u002206F0MEGYHADPVN575H64D56W2G|PitBackedAsOfReadContract|PitAsOfReadContractSnapshotTests\u0022 -- docs tests src\u0060 returned no matches.",
    "Evidence: \u0060README.md:167-208\u0060 documents only latest/as-of satellite reads on \u0060IDataVaultReadService\u0060 and says PIT-backed read models remain future extension points.",
    "Evidence: \u0060docs/plans/deferred-data-vault-capabilities.md:73-101\u0060 documents the existing \u0060DataVaultPitMetadata\u0060 baseline and unsupported shapes, but it is pre-existing baseline documentation rather than a ticket-specific PIT read contract fixture/test delivery.",
    "Evidence: \u0060docs/releases/v0.6.0.md:25,39\u0060 keeps \u0060IDataVaultReadService\u0060 scoped to latest/as-of satellite rows and does not supply the new PIT contract artifacts claimed in developer handoff.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/api, area/docs, area/pit, area/read-models, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F0MEH660Y5QTNR5P8JPS2QXC-task-implement-provider-neutral-pit-snapshot-rea\u0027.",
    "Evidence: Ticket history references implementation commit \u0027030ad7545a7a\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The contract defines a provider-neutral PIT read request on \u0060IDataVaultReadService\u0060 that accepts one \u0060DataVaultPitMetadata\u0060 declaration, one or more parent hash keys, and an \u0060asOf\u0060 instant, and it states that the service resolves the latest PIT row visible at or before that instant per requested parent. (The persisted ticket contract explicitly keeps the v1 public read boundary on \u0060IDataVaultReadService\u0060, requires one \u0060DataVaultPitMetadata\u0060 declaration plus one or more parent hash keys and a \u0060DateTimeOffset asOf\u0060, and states that the latest PIT row visible at or before that instant is resolved per requested parent.).",
    "AC check passed: The contract defines a raw PIT read-record shape that exposes the parent hash key, PIT load timestamp, and per-satellite snapshot data keyed by declared satellite name and ordered by the \u0060DataVaultPitMetadata\u0060 declaration so a caller-owned projector can build typed read models. (The persisted ticket contract and implementation notes define a raw PIT read-record shape with parent hash key, PIT load timestamp, and ordered per-satellite snapshot access aligned to the \u0060DataVaultPitMetadata\u0060 declaration for caller-owned projectors.).",
    "AC check passed: The contract states that a missing PIT row yields no result for that parent, while unsupported or inconsistent PIT metadata shapes fail deterministically through diagnostics instead of silently falling back to latest-satellite logic. (The persisted clarifications state that a missing PIT row yields no projected result for that parent, missing satellite snapshots stay absent inside a matched PIT row, and unsupported or inconsistent metadata shapes fail deterministically instead of falling back to latest-satellite reads.).",
    "AC check passed: The contract explicitly rejects unsupported v1 shapes, including multi-active satellite references, bridge-driven reads, link-based PIT parents, and any request that tries to read outside the bounded \u0060DataVaultPitMetadata\u0060 baseline. (The persisted contract explicitly rejects multi-active satellite references, bridge-driven reads, link-based PIT parents, and requests outside the bounded \u0060DataVaultPitMetadata\u0060 baseline.).",
    "AC check passed: The contract and examples show that timestamp storage modes remain internal and do not change the caller-facing \u0060DateTimeOffset\u0060 API. (The persisted clarifications keep timestamp handling provider-neutral: callers pass \u0060DateTimeOffset\u0060 and provider storage modes remain internal implementation detail.).",
    "DoD check passed: The contract cross-references the current latest/as-of satellite read baseline and confirms PIT reads extend it without changing existing latest-satellite behavior. (The persisted contract explicitly extends the existing latest/as-of projector pattern on \u0060IDataVaultReadService\u0060, and the current baseline is directly documented in \u0060README.md\u0060 and \u0060docs/releases/v0.6.0.md\u0060 without changing existing latest-satellite behavior.).",
    "DoD check passed: Unsupported multi-active, bridge, and legacy \u0060PointInTime\u0060 cases are called out as diagnostics or out-of-scope behavior in the final contract text. (The persisted contract text, together with the existing PIT baseline documentation, calls out unsupported multi-active, bridge, and legacy \u0060PointInTime\u0060 cases as diagnostics or out-of-scope behavior.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Documentation or fixture examples cover at least one multi-satellite typed projection example and one missing-PIT-row example before implementation starts. (No repository-hosted contract examples or fixtures were found. The claimed \u0060docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/Contracts/PitBackedAsOfReadContract.approved.txt\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/PitAsOfReadContractSnapshotTests.cs\u0060 are absent, and the bounded directory scans found no substitute PIT contract example files.).",
    "DoD check failed: A planning-level contract is written in ticket or repository documentation with the bounded v1 PIT read surface, examples, and non-goals. (The ticket text provides bounded scope and non-goals, but the claimed repository plan document is missing and I found no repository-hosted worked PIT examples. That leaves the documented contract deliverable incomplete.).",
    "DoD check failed: Expected request and raw-record/projection shapes are captured in API fixtures, snapshots, or equivalent tests so downstream implementation has a stable contract target. (No API fixture, snapshot, or equivalent PIT contract test is present in the reviewed repository state, so downstream implementation does not yet have the promised stable contract target in repository artifacts.).",
    "The developer handoff claims three repository artifacts, but none of those files exist in the reviewed branch state and no alternate repository files were found that deliver the PIT contract examples/fixtures required by AC6 and DoD1-2.",
    "The repository still documents PIT-backed reads as future extension work on top of the current latest/as-of read baseline, so the promised stable downstream contract target has not been materialized in repo docs/tests yet."
  ],
  "evidence": [
    "\u0060git rev-parse HEAD\u0060 returned \u0060030ad7545a7a5432849adcf0e4aed3f47f7122dc\u0060 on branch \u0060ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract\u0060.",
    "\u0060git diff --name-status develop...030ad7545a7a\u0060 showed only \u0060.gicket/tickets/06F0MEGYHADPVN575H64D56W2G/...\u0060 additions/modifications and no \u0060docs/\u0060 or \u0060tests/DCoding.Data.DVault.Tests/Unit/\u0060 PIT contract files.",
    "\u0060repository-list-directory docs\u0060 enumerated 30 entries under \u0060docs/\u0060, including multiple \u0060docs/plans/*.md\u0060 files, but did not include \u0060docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md\u0060.",
    "\u0060repository-list-directory tests/DCoding.Data.DVault.Tests/Unit\u0060 enumerated 33 entries, including \u0060Snapshots/PublicApi/...\u0060, but there was no \u0060Snapshots/Contracts/\u0060 subtree and no \u0060PitAsOfReadContractSnapshotTests.cs\u0060 entry.",
    "\u0060git ls-files\u0060 for the three developer-claimed artifact paths returned no tracked files.",
    "\u0060git grep -n \u002206F0MEGYHADPVN575H64D56W2G|PitBackedAsOfReadContract|PitAsOfReadContractSnapshotTests\u0022 -- docs tests src\u0060 returned no matches.",
    "\u0060README.md:167-208\u0060 documents only latest/as-of satellite reads on \u0060IDataVaultReadService\u0060 and says PIT-backed read models remain future extension points.",
    "\u0060docs/plans/deferred-data-vault-capabilities.md:73-101\u0060 documents the existing \u0060DataVaultPitMetadata\u0060 baseline and unsupported shapes, but it is pre-existing baseline documentation rather than a ticket-specific PIT read contract fixture/test delivery.",
    "\u0060docs/releases/v0.6.0.md:25,39\u0060 keeps \u0060IDataVaultReadService\u0060 scoped to latest/as-of satellite rows and does not supply the new PIT contract artifacts claimed in developer handoff.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/api, area/docs, area/pit, area/read-models, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F0MEH660Y5QTNR5P8JPS2QXC-task-implement-provider-neutral-pit-snapshot-rea\u0027.",
    "Ticket history references implementation commit \u0027030ad7545a7a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add the repository-hosted PIT contract document with concrete worked examples, including a multi-satellite typed projection example and a missing-PIT-row example, or point the handoff to the actual persisted path if it differs from the claim.",
    "Add a PIT contract fixture/snapshot and a verifying test under \u0060tests/DCoding.Data.DVault.Tests/Unit/...\u0060 that captures request shape, raw PIT record shape, missing satellite snapshot behavior, and unsupported-shape diagnostics.",
    "After the missing artifacts are present, rerun \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060, then return the ticket to tester review."
  ],
  "branchName": "ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract",
  "commitSha": "030ad7545a7a"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F0MEGYHADPVN575H64D56W2G`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract`