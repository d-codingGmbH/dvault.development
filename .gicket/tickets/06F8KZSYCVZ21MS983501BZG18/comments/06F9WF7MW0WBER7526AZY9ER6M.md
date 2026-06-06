[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release\u0027 at commit \u00274b9b9e12ba2f\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release",
    "commitSha": "4b9b9e12ba2f",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "docs/releases/v0.31.0.md exists and follows the coordinated release-note pattern with package scope, a boundary shift from v0.30.0, evidence anchors, validation evidence, and explicit non-goals.",
      "satisfied": true,
      "reason": "docs/releases/v0.31.0.md exists at commit 4b9b9e12ba2f and follows the established release-note structure with package scope, a boundary shift from v0.30.0, evidence anchors, validation evidence, documentation updates, and explicit non-goals."
    },
    {
      "expectation": "The release note links to the landed v0.31 sources it summarizes, including docs/performance-profiles.md, examples/README.md, the root benchmark-summary.md / benchmark-summary.csv / benchmark-summary.json triplet, and the existing observability contract surfaces rather than duplicating their full detail.",
      "satisfied": true,
      "reason": "The release note links the summarized source surfaces instead of duplicating them, including docs/performance-profiles.md, examples/README.md, README.md observability guidance, the activity-tracing and explain contracts, and benchmark-summary.md / benchmark-summary.csv / benchmark-summary.json."
    },
    {
      "expectation": "The release note summarizes the realistic quickstart evidence at release-note level: fixed CRM import/change timestamps, explicit load-timestamp and record-source saves, typed latest/as-of customer-profile reads, and bounded save/read diagnostics, without copying full console output or unsanitized values.",
      "satisfied": true,
      "reason": "The Quickstart Evidence section summarizes the realistic customer-profile flow at release-note level: fixed 2026-04-29 CRM timestamps, crm-import/crm-change record sources, explicit save-service writes, typed latest/as-of reads, and bounded diagnostics, without pasting raw console output or unsanitized values."
    },
    {
      "expectation": "Any touched current-baseline or version-example text in README.md and examples/README.md is internally aligned to v0.31.0, and docs/production-adoption-checklist.md no longer leaves v0.29.0 labeled as the current public baseline.",
      "satisfied": true,
      "reason": "README.md and examples/README.md align their package/version examples to 0.31.0, README.md marks v0.31.0 as the current coordinated baseline, and docs/production-adoption-checklist.md marks v0.31.0 as the current public baseline while keeping v0.29.0 historical."
    },
    {
      "expectation": "The final wording explicitly keeps observability application-owned and excludes dashboards, exporters, collectors, hosting, automatic PIT or bridge maintenance, ingestion orchestration, provider-specific SQL artifact workflow, benchmark reruns, and package-publication claims.",
      "satisfied": true,
      "reason": "The release wording keeps observability application-owned and explicitly excludes dashboards, exporters, collectors, hosting, automatic PIT or bridge maintenance, ingestion orchestration, provider-specific SQL artifact workflow, benchmark reruns, and package-publication claims."
    },
    {
      "expectation": "The v0.32 artifact-lane work is mentioned only as a future boundary and is not specified or implemented inside the v0.31 release note.",
      "satisfied": true,
      "reason": "The release note mentions the provider-specific SQL artifact lane only as future v0.32 scope and does not specify or implement that workflow inside the v0.31 note."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "docs/releases/v0.31.0.md and any touched navigation docs are updated outside .gicket and remain documentation-only changes.",
      "satisfied": true,
      "reason": "The non-.gicket diff against develop for commit 4b9b9e12ba2f changes only README.md, docs/production-adoption-checklist.md, docs/releases/v0.31.0.md, and examples/README.md, so the ticket remains documentation-only outside .gicket."
    },
    {
      "expectation": "README.md, docs/production-adoption-checklist.md, and any touched example doc no longer disagree about which release is the current public documentation baseline.",
      "satisfied": true,
      "reason": "The touched baseline surfaces no longer disagree: README.md and docs/production-adoption-checklist.md point to v0.31.0 as current, and examples/README.md package examples were advanced to 0.31.0 to match."
    },
    {
      "expectation": "The new release note\u0027s evidence anchors and non-goals stay consistent with the already-landed guidance and example sources in the repository.",
      "satisfied": true,
      "reason": "The new release note stays consistent with the already-landed repository sources it cites, including docs/performance-profiles.md, examples/README.md, QuickstartHistoryFlow.cs, benchmark-summary.* artifacts, and observability contract documents."
    },
    {
      "expectation": "No code/runtime changes, benchmark artifacts, ticket relations, child tickets, attachments, or planning documents are introduced by this ticket.",
      "satisfied": true,
      "reason": "No code/runtime files, benchmark artifacts, ticket-planning documents, or other non-documentation repository outputs were introduced in the branch delta; the only non-.gicket changes are the four documentation files."
    }
  ],
  "evidence": [
    "\u0060git rev-parse --verify 4b9b9e12ba2f^{commit}\u0060 resolved commit \u00604b9b9e12ba2f6e31b616ce639e22b2c097959687\u0060 on branch \u0060ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release\u0060.",
    "\u0060git diff --name-status develop...4b9b9e12ba2f -- . \u0027:(exclude).gicket\u0027 \u0027:(exclude).gicket-bot\u0027\u0060 showed only \u0060M README.md\u0060, \u0060M docs/production-adoption-checklist.md\u0060, \u0060A docs/releases/v0.31.0.md\u0060, and \u0060M examples/README.md\u0060.",
    "\u0060git diff --name-only 4b9b9e12ba2f..HEAD -- . \u0027:(exclude).gicket\u0027 \u0027:(exclude).gicket-bot\u0027\u0060 returned no paths, so later branch movement after the claimed commit is limited to .gicket metadata and does not change repository deliverables.",
    "\u0060git ls-files\u0060 confirmed the linked evidence surfaces exist in the repository, including \u0060benchmark-summary.md\u0060, \u0060benchmark-summary.csv\u0060, \u0060benchmark-summary.json\u0060, \u0060docs/performance-profiles.md\u0060, \u0060docs/architecture/dvault-v1-activity-tracing-contract.md\u0060, \u0060examples/README.md\u0060, and \u0060examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0060.",
    "\u0060git ls-files docs/README.md\u0060 returned no match, which is consistent with the authoritative delivery contract note that \u0060docs/README.md\u0060 does not exist on this branch and is not a required edit surface for this ticket.",
    "\u0060git show 4b9b9e12ba2f:docs/releases/v0.31.0.md\u0060 contains the expected coordinated release-note sections and explicitly links the benchmark triplet, performance profiles, observability contracts, quickstart docs, and v0.32 future-boundary statement.",
    "\u0060git show 4b9b9e12ba2f:README.md\u0060, \u0060git show 4b9b9e12ba2f:examples/README.md\u0060, and \u0060git show 4b9b9e12ba2f:docs/production-adoption-checklist.md\u0060 show the 0.31.0 baseline alignment across installation snippets and current-baseline wording.",
    "The supplied ticket context already records executable verification for the claimed commit: comments 17-18 state prior tester evidence for \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060, and comment 14 records \u0060dotnet build DVault.slnx --nologo\u0060 succeeding with operational NU1900 warnings only.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/developer-experience, area/documentation, area/ef-core, area/observability, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027integrator\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027integrator\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, integrator, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release\u0027.",
    "Ticket history references implementation commit \u00274b9b9e12ba2f\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Proceed on the normal test-success path so integrator automation can evaluate commit 4b9b9e12ba2f."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F8KZSYCVZ21MS983501BZG18`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release' at commit '4b9b9e12ba2f'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release`
- implementation-commit: `4b9b9e12ba2f`
- implementation-pr: `<none>`
- implementation-change: `<none>`