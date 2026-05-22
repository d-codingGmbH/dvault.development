[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F492BNDPWS9P4EDSV0W7G6VM\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F492BNDPWS9P4EDSV0W7G6VM-task-update-v0-17-0-documentation-and-release-no\u0027 and commit \u0027fbaf938d551c\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F492BNDPWS9P4EDSV0W7G6VM-task-update-v0-17-0-documentation-and-release-no\u0027 from source \u0027fbaf938d551c\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F492BNDPWS9P4EDSV0W7G6VM-task-update-v0-17-0-documentation-and-release-no\u0027.",
    "Evidence: \u0060git diff --name-only develop...fbaf938d551c -- . \u0027:(exclude).gicket/**\u0027\u0060 shows these repository changes: \u0060README.md\u0060, \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060, \u0060docs/model-first-governance.md\u0060, \u0060docs/plans/fluent-code-first-api-contract.md\u0060, \u0060docs/production-adoption-checklist.md\u0060, \u0060docs/releases/v0.17.0.md\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/README.md\u0060.",
    "Evidence: \u0060docs/releases/v0.17.0.md:8-18\u0060 states the seven-package coordinated release and that publication remains a separate manual activity; \u0060docs/releases/v0.17.0.md:22-27\u0060 lists the EF safety/preflight highlights.",
    "Evidence: \u0060docs/production-adoption-checklist.md:9-11\u0060, \u0060README.md:10-21\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/README.md:19-39\u0060 advance the baseline to v0.17.0 and document \u0060DMV1910\u0060/\u0060DMV1911\u0060 plus project-local analyzer installation guidance.",
    "Evidence: \u0060README.md:141-152\u0060, \u0060docs/production-adoption-checklist.md:45-49\u0060, and \u0060docs/releases/v0.17.0.md:47-64\u0060 document the explicit SaveChanges guard opt-in, mode selection, coexistence with the metadata interceptor, and the continued default \u0060IDataVaultSaveService\u0060 write boundary.",
    "Evidence: \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060 and \u0060docs/releases/v0.17.0.md\u0060 both document consumer-owned snapshot-model drift, migration guardrails, aggregate preflight, and no \u0060dotnet ef\u0060 interception / no standalone DVault CLI boundaries.",
    "Evidence: \u0060git diff --unified=20 develop...fbaf938d551c -- docs/plans/fluent-code-first-api-contract.md\u0060 shows a ticket-branch edit to that planning document, and \u0060docs/plans/fluent-code-first-api-contract.md:4\u0060 now points its current shipped reference at \u0060docs/releases/v0.17.0.md\u0060.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/documentation, area/ef-core, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F492BNDPWS9P4EDSV0W7G6VM-task-update-v0-17-0-documentation-and-release-no\u0027.",
    "Evidence: Ticket history references implementation commit \u0027fbaf938d551c\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A new or updated \u0060docs/releases/v0.17.0.md\u0060 presents the coordinated seven-package release, identifies the EF safety and preflight highlights ratified by the completed prerequisite stories, and keeps publication, manual-release, and non-goal boundaries explicit. (\u0060docs/releases/v0.17.0.md\u0060 was added and documents the coordinated seven-package release, EF safety/preflight highlights, manual publication boundary, and explicit non-goals.).",
    "AC check passed: The public docs surfaces that currently define adoption, setup, and design-time guidance are updated to treat v0.17.0 as the current baseline and to align installation snippets, analyzer guidance, and preflight, guard, and drift workflow wording with the checked-in APIs. (\u0060README.md\u0060, \u0060docs/production-adoption-checklist.md\u0060, \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060, and \u0060docs/model-first-governance.md\u0060 were updated to treat v0.17.0 as the current baseline and to align setup, adoption, drift, guard, and preflight guidance with the documented APIs.).",
    "AC check passed: Release notes and adoption docs name the shipped EF misuse analyzer ids \u0060DMV1910\u0060 and \u0060DMV1911\u0060, explain their supported and non-supported patterns at a bounded level, and keep \u0060DCoding.Data.DVault.Analyzers\u0060 as project-local tooling. (\u0060docs/releases/v0.17.0.md\u0060, \u0060README.md\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/README.md\u0060 name \u0060DMV1910\u0060 and \u0060DMV1911\u0060, describe their bounded analyzer scope, and keep \u0060DCoding.Data.DVault.Analyzers\u0060 as project-local tooling with \u0060PrivateAssets=\u0022all\u0022\u0060.).",
    "AC check passed: Runtime guard documentation explains that \u0060UseDataVaultSaveChangesGuardInterceptor(...)\u0060 is explicit opt-in, separate from \u0060AddDVault()\u0060, supports warning and blocking modes, coexists with \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060, and does not replace \u0060IDataVaultSaveService\u0060 as the default write boundary. (\u0060README.md\u0060, \u0060docs/production-adoption-checklist.md\u0060, and \u0060docs/releases/v0.17.0.md\u0060 describe \u0060UseDataVaultSaveChangesGuardInterceptor(...)\u0060 as explicit opt-in, separate from \u0060AddDVault()\u0060, available in warning and blocking modes, able to coexist with \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060, and not a replacement for \u0060IDataVaultSaveService\u0060.).",
    "AC check passed: Preflight and drift documentation shows the consumer-owned workflow around \u0060IDataVaultDiagnosticsService.Analyze(DbContext)\u0060, \u0060DataVaultModelDriftPreflightReporter.Compare(...)\u0060, \u0060DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)\u0060, and \u0060DataVaultPreflight.Run(...)\u0060 without implying \u0060ModelSnapshot\u0060 coupling, repository scanning, or a DVault-owned CLI. (The updated docs show consumer-owned workflows around \u0060IDataVaultDiagnosticsService.Analyze(DbContext)\u0060, \u0060DataVaultModelDriftPreflightReporter.Compare(...)\u0060, \u0060DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)\u0060, and \u0060DataVaultPreflight.Run(...)\u0060, while explicitly rejecting \u0060ModelSnapshot\u0060 as a DVault public contract, repository scanning, and a DVault-owned CLI.).",
    "AC check passed: Provider explainability and support-bundle guidance documents capability profile, provider-behavior profile, save and read strategy diagnostics, and request-bound read-shape diagnostics as deterministic redacted explain surfaces rather than raw SQL or provider-magic claims. (\u0060README.md\u0060, \u0060docs/production-adoption-checklist.md\u0060, and \u0060docs/releases/v0.17.0.md\u0060 document provider capability/profile, provider-behavior profile, save/read strategy diagnostics, and request-bound read-shape diagnostics as deterministic redacted explain output rather than raw SQL or provider-magic claims.).",
    "AC check passed: At least one migration example and one drift or preflight example are updated so readers can distinguish safe, risky, and incompatible guardrail outcomes plus artifact-versus-design-time and snapshot-model preflight lanes. (The release notes and public guidance include migration guardrail outcome descriptions (\u0060Safe\u0060, \u0060Risky\u0060, \u0060Incompatible\u0060) plus snapshot-model drift and aggregate preflight examples that distinguish artifact, snapshot-model, and representative-diagnostics lanes.).",
    "AC check passed: The documentation keeps non-goals explicit across release notes and public guidance: no automatic migration execution, no automatic schema repair, no automatic live-schema gate, no dashboards, and no standalone DVault platform. (The updated release notes and guidance keep non-goals explicit, including no automatic migration execution, no automatic schema repair, no automatic live-schema gate, no dashboards, and no standalone DVault platform.).",
    "DoD check passed: All affected public documentation surfaces and the v0.17.0 release notes are internally consistent on version numbers, API names, diagnostic ids, and default-versus-opt-in behavior. (The affected public docs consistently use v0.17.0, the same API names, the same analyzer ids, and the same default-versus-opt-in guard/telemetry boundaries.).",
    "DoD check passed: The docs use the completed ticket contracts and checked-in repository docs as the authoritative source for feature scope instead of inventing new APIs, relation semantics, or broader provider guarantees. (The documentation updates stay aligned with the checked-in documentation/source-backed feature scope and do not invent a broader CLI, relation semantics, or provider guarantees.).",
    "DoD check passed: Examples and snippets remain bounded to consumer-owned EF Core workflows and do not require unsupported repository discovery, \u0060ModelSnapshot\u0060 public contracts, or provider-specific magic. (Examples remain consumer-owned EF Core workflows and explicitly avoid unsupported repository discovery, \u0060ModelSnapshot\u0060 as a DVault public contract, and provider-specific magic.).",
    "DoD check passed: The current v0.16.0 baseline references in public guidance are advanced to v0.17.0 wherever this ticket owns the public current-release posture. (Public current-baseline references owned by this ticket were advanced from v0.16.0 to v0.17.0 in the README, adoption checklist, model-first guidance, and new release notes.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: The documentation pass completes without child-ticket creation, relation rewrites, description updates, attachments, or planning-document materialization. (The branch diff also modifies \u0060docs/plans/fluent-code-first-api-contract.md\u0060, which is a planning document. That conflicts with the explicit requirement that this documentation pass complete without planning-document materialization.).",
    "The branch includes an out-of-scope planning-document change in \u0060docs/plans/fluent-code-first-api-contract.md\u0060. That violates Definition of Done index 5, which explicitly disallows planning-document materialization for this ticket."
  ],
  "evidence": [
    "\u0060git diff --name-only develop...fbaf938d551c -- . \u0027:(exclude).gicket/**\u0027\u0060 shows these repository changes: \u0060README.md\u0060, \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060, \u0060docs/model-first-governance.md\u0060, \u0060docs/plans/fluent-code-first-api-contract.md\u0060, \u0060docs/production-adoption-checklist.md\u0060, \u0060docs/releases/v0.17.0.md\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/README.md\u0060.",
    "\u0060docs/releases/v0.17.0.md:8-18\u0060 states the seven-package coordinated release and that publication remains a separate manual activity; \u0060docs/releases/v0.17.0.md:22-27\u0060 lists the EF safety/preflight highlights.",
    "\u0060docs/production-adoption-checklist.md:9-11\u0060, \u0060README.md:10-21\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/README.md:19-39\u0060 advance the baseline to v0.17.0 and document \u0060DMV1910\u0060/\u0060DMV1911\u0060 plus project-local analyzer installation guidance.",
    "\u0060README.md:141-152\u0060, \u0060docs/production-adoption-checklist.md:45-49\u0060, and \u0060docs/releases/v0.17.0.md:47-64\u0060 document the explicit SaveChanges guard opt-in, mode selection, coexistence with the metadata interceptor, and the continued default \u0060IDataVaultSaveService\u0060 write boundary.",
    "\u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060 and \u0060docs/releases/v0.17.0.md\u0060 both document consumer-owned snapshot-model drift, migration guardrails, aggregate preflight, and no \u0060dotnet ef\u0060 interception / no standalone DVault CLI boundaries.",
    "\u0060git diff --unified=20 develop...fbaf938d551c -- docs/plans/fluent-code-first-api-contract.md\u0060 shows a ticket-branch edit to that planning document, and \u0060docs/plans/fluent-code-first-api-contract.md:4\u0060 now points its current shipped reference at \u0060docs/releases/v0.17.0.md\u0060.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/documentation, area/ef-core, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F492BNDPWS9P4EDSV0W7G6VM-task-update-v0-17-0-documentation-and-release-no\u0027.",
    "Ticket history references implementation commit \u0027fbaf938d551c\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Remove the \u0060docs/plans/fluent-code-first-api-contract.md\u0060 change from the ticket branch so the delivery stays limited to the owned public documentation and release-note surfaces.",
    "Resubmit for test review after that planning-document edit is removed; the public documentation content already has direct repository evidence for the stated acceptance criteria."
  ],
  "branchName": "ticket/06F492BNDPWS9P4EDSV0W7G6VM-task-update-v0-17-0-documentation-and-release-no",
  "commitSha": "fbaf938d551c"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F492BNDPWS9P4EDSV0W7G6VM`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F492BNDPWS9P4EDSV0W7G6VM-task-update-v0-17-0-documentation-and-release-no`