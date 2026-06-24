[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi\u0027 at commit \u0027909d0259bc33\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi",
    "commitSha": "909d0259bc33",
    "pullRequestReference": null,
    "changeReference": null,
    "branchOwnerProvenance": {
      "ticketId": "06FF438KMPKSBT6KXZ5DBY85QC",
      "ownerBranch": "ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi",
      "sourceCommitSha": "909d0259bc33",
      "baseBranch": "develop",
      "producingRole": "test",
      "producingRunId": "6561731b41454b80b4686578a6f39bba",
      "producingInstanceId": "hp-ai-2026-001.1"
    }
  },
  "acceptanceCriteria": [
    {
      "expectation": "The authoritative provider evidence contract explicitly distinguishes PIT maintenance timing rows from PIT and bridge read rows so read evidence cannot be cited as maintenance evidence.",
      "satisfied": true,
      "reason": "Satisfied. docs/plans/provider-optimization-evidence-matrix.md now states that pit-full-rebuild-maintenance is a separate row family from pit-as-of-read and bridge-traversal-read, adds a dedicated PIT Full-Rebuild Maintenance Row Contract section, and includes a citation example that forbids citing pit-as-of-read rows as maintenance evidence; docs/architecture/dvault-v1-pit-bridge-boundary.md repeats the same separation."
    },
    {
      "expectation": "The documented maintenance slice is limited to PIT full-rebuild evidence for the provider-neutral comparator plus the PostgreSQL and SQL Server provider-specific lanes already owned by sibling benchmark tickets.",
      "satisfied": true,
      "reason": "Satisfied. The new maintenance contract section in docs/plans/provider-optimization-evidence-matrix.md lists exactly three lanes: provider-neutral comparator, PostgreSQL PIT full rebuild, and SQL Server PIT full rebuild, and docs/architecture/dvault-v1-pit-bridge-boundary.md says the bounded v1 maintenance timing slice is limited to those full-rebuild lanes."
    },
    {
      "expectation": "Maintenance timing claims require scenario, provider, baseline/comparator, selected strategy or provider-neutral fallback posture, bounded fallback causes when present, run context, and links to the supporting benchmark artifact triplet.",
      "satisfied": true,
      "reason": "Satisfied. The matrix usage rules, manifest field descriptions, and maintenance contract section require scenario identity, provider, baseline/comparator identity, selected strategy or provider-neutral fallback posture, bounded fallback causes when present, run context, and the benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json artifact triplet; docs/plans/performance-evidence-benchmark-artifact-contract.md aligns the same requirements through maintenanceScope=FullRebuild and shared executionDetail tokens."
    },
    {
      "expectation": "The contract reuses the existing supported-shape boundary: PostgreSQL full rebuilds on ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs; SQL Server full rebuilds on clean ordinary hub-parent PITs only.",
      "satisfied": true,
      "reason": "Satisfied. The PostgreSQL lane is limited to ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PIT full rebuilds, while the SQL Server lane is limited to clean ordinary hub-parent PIT full rebuilds; the same boundaries are restated in docs/architecture/dvault-v1-pit-bridge-boundary.md."
    },
    {
      "expectation": "Skipped, unconfigured, diagnostics-only, or docs-only guidance rows are not maintenance timing claims; completed maintenance timing claims require preserved artifact triplets and run context.",
      "satisfied": true,
      "reason": "Satisfied. The new maintenance section says skipped, unconfigured, diagnostics-only, smoke-only, docs-only, and placeholder rows are not maintenance timing claims, and it requires preserved artifact triplets plus run context for completed claims."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The authoritative evidence-contract document set is updated on the existing matrix and supporting-contract surfaces to describe PIT full-rebuild maintenance timing rows without creating a parallel document.",
      "satisfied": true,
      "reason": "Satisfied. git diff --name-only develop...909d0259bc33 -- docs shows changes only in docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/plans/performance-evidence-benchmark-artifact-contract.md, and docs/plans/provider-optimization-evidence-matrix.md, so the existing contract surfaces were updated without creating a parallel document."
    },
    {
      "expectation": "The updated contract stays consistent with the PIT maintenance boundary in docs/architecture/dvault-v1-pit-bridge-boundary.md and with the shared benchmark artifact contract.",
      "satisfied": true,
      "reason": "Satisfied. The same maintenance vocabulary and boundaries are aligned across the matrix, the shared benchmark artifact contract, and the PIT/bridge boundary doc: separate maintenance row family, maintenanceScope=FullRebuild, readShape=null, shared artifact triplet, run context, and bounded PIT maintenance fallback causes."
    },
    {
      "expectation": "Sibling benchmark tickets can add provider-neutral, PostgreSQL, and SQL Server PIT full-rebuild maintenance rows without reopening provider boundary, artifact, or non-goal decisions.",
      "satisfied": true,
      "reason": "Satisfied. docs/plans/provider-optimization-evidence-matrix.md now fixes the row family, lane table, artifact-triplet requirement, fallback vocabularies, and explicit non-goals, so sibling benchmark tickets can add provider-neutral, PostgreSQL, and SQL Server PIT full-rebuild maintenance rows without reopening boundary or artifact decisions."
    },
    {
      "expectation": "No blocking PO question remains about whether bridge maintenance, parent maintenance, or additional providers belong in this ticket.",
      "satisfied": true,
      "reason": "Satisfied. The updated matrix explicitly keeps bridge maintenance push-down out of scope, marks MaintainParentsUnsupported as a fallback rather than a completed maintenance timing row, and says provider expansion beyond PostgreSQL and SQL Server stays outside this slice, leaving no blocking scope question on those fronts."
    }
  ],
  "evidence": [
    "git log --oneline --max-count=8 shows the functional documentation change landed at commit 909d0259bc33; git diff --name-only 909d0259bc33...HEAD -- docs returns no output, so the verified docs content at branch HEAD matches the implementation handoff.",
    "git diff --name-only develop...909d0259bc33 -- docs lists only docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/plans/performance-evidence-benchmark-artifact-contract.md, and docs/plans/provider-optimization-evidence-matrix.md.",
    "docs/plans/provider-optimization-evidence-matrix.md adds PIT full-rebuild maintenance usage rules, manifest-field rules for workloadShape=pit-full-rebuild-maintenance and readShape=null, a dedicated PIT Full-Rebuild Maintenance Row Contract section, and a maintenance-specific citation example.",
    "That matrix section defines exactly three maintenance lanes: provider-neutral comparator, PostgreSQL PIT full rebuild, and SQL Server PIT full rebuild.",
    "The same matrix requires benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json under the sibling benchmark ticket\u0027s preserved artifact label, plus run context, before a completed maintenance timing claim is valid.",
    "docs/plans/performance-evidence-benchmark-artifact-contract.md now says maintenance rows must use scenario pit-full-rebuild-maintenance, maintenanceScope=FullRebuild, bounded fallback causes, run context, and the supporting artifact triplet, and it maps maintenance fields through the shared manifest without inventing a parallel schema.",
    "docs/architecture/dvault-v1-pit-bridge-boundary.md now states that pit-full-rebuild-maintenance is separate from pit-as-of-read and bridge-traversal-read and limits the completed maintenance timing slice to the provider-neutral comparator plus PostgreSQL and SQL Server full-rebuild lanes.",
    "docs/plans/provider-optimization-evidence-matrix.md stop and fallback rules now include DataVaultPitMaintenanceStrategyFallbackCauseKind and SqlServerPitMaintenanceFallbackCauseKind and explicitly keep MaintainParentsAsync(...), bridge maintenance push-down, and provider expansion beyond PostgreSQL and SQL Server out of this slice.",
    "rg -n finds SqlServerPitMaintenanceFallbackCauseKind.MaintainParentsUnsupported in src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs and PIT maintenance fallback enums in src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs, matching the matrix\u0027s cited bounded fallback vocabularies.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/documentation, area/performance, area/provider-support, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi\u0027.",
    "Ticket history references implementation commit \u0027909d0259bc33\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Proceed to integrator; no tester rework is required."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FF438KMPKSBT6KXZ5DBY85QC`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi' at commit '909d0259bc33'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi`
- implementation-commit: `909d0259bc33`
- implementation-pr: `<none>`
- implementation-change: `<none>`