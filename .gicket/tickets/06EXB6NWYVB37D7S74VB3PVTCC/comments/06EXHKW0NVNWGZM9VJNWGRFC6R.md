[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EXB6NWYVB37D7S74VB3PVTCC\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards\u0027 and commit \u0027b2c3707e901b\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards\u0027 from source \u0027b2c3707e901b\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards\u0027.",
    "Evidence: git rev-parse --verify b2c3707e901b^{commit} returned b2c3707e901b5c1adb46171b647420146a193ffb.",
    "Evidence: git rev-parse ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards returned 2ad37d0f94a77fdfccc0180a5d95e1503abd8665; git merge-base --is-ancestor b2c3707e901b target exited 0, and git diff b2c3707e901b..2ad37d0f94a77fdfccc0180a5d95e1503abd8665 -- . \u0027:!.gicket\u0027 produced no output.",
    "Evidence: git diff --name-status develop...b2c3707e901b -- . \u0027:!.gicket\u0027 listed D DVault.Build.csproj, D DVault.csproj, D DVault.sln, M docs/formatting.md, and A docs/plans/shared-implementation-standards.md.",
    "Evidence: git ls-tree -r --name-only b2c3707e901b included docs/plans/shared-implementation-standards.md, docs/formatting.md, docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, docs/plans/dvault-v1-default-persistence-convention-policy.md, src/DCoding.Data.DVault/.gitkeep, src/DVault/DVault.csproj, and tools/check-format.sh.",
    "Evidence: git show b2c3707e901b:docs/plans/shared-implementation-standards.md showed sections for Relationship Context, Source Of Truth Documents, Formatting And Encoding, Repository Layout, .NET Project Baseline, Namespaces And Naming, Data Vault Concepts, Stable Hashing, V1 Persistence Conventions, Documentation Standards, V1 Defaults, Deferred Follow-Up Work, and Acceptance Baseline.",
    "Evidence: git show b2c3707e901b:src/DVault/DVault.csproj showed TargetFramework net10.0, ImplicitUsings enable, Nullable enable, and GenerateDocumentationFile true.",
    "Evidence: git status --short -- . \u0027:!.gicket\u0027 \u0027:!.gicket-bot\u0027 produced no output for non-operational paths.",
    "Evidence: bash tools/check-format.sh exited 1 and reported missing final newlines for governed files including .editorconfig, .gitattributes, DVault.slnx, README.md, docs/formatting.md, docs/plans/shared-implementation-standards.md, src/DVault/DVault.csproj, tests/DVault.Tests/DVault.Tests.csproj, and tools/check-format.sh.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/governance, automation/bot-ready, backlog/initial-dvault, needs-test, type/story, bot/lease:hp-ai-2026-001.2].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards\u0027.",
    "Evidence: Ticket history references implementation commit \u0027b2c3707e901b\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: The standards explicitly cover formatting, encoding, line endings, tab policy, final newline, trailing whitespace, brace placement, and the required local formatting command bash tools/check-format.sh. (The Formatting And Encoding section explicitly covers spaces/two-space indentation, LF, UTF-8 without BOM, final newline, trailing whitespace, tab exceptions, same-line braces, and bash tools/check-format.sh.).",
    "AC check passed: The standards explicitly cover current repository layout defaults for src/, tests/, docs/, examples/, benchmarks/, root solution files, and tracked placeholder folders. (The Repository Layout section explicitly covers DVault.slnx, src/, tests/, docs/, examples/, benchmarks/, and .gitkeep-tracked placeholder folders.).",
    "AC check passed: The standards explicitly cover the current .NET implementation baseline: net10.0, nullable enabled, implicit usings enabled, XML documentation generation, and same-line C# braces. (The .NET Project Baseline section states net10.0, nullable enable, implicit usings enable, GenerateDocumentationFile true, and same-line C# braces; src/DVault/DVault.csproj contains those properties.).",
    "AC check passed: The standards reference existing repository decisions for Data Vault concepts, default naming policy, stable hashing contract, and v1 persistence convention policy instead of duplicating their full content. (The Source Of Truth Documents and later sections reference Data Vault concepts, docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md rather than duplicating their full content.).",
    "AC check passed: The standards identify which decisions are v1 defaults and which categories remain deferred follow-up work. (The V1 Defaults and Deferred Follow-Up Work sections separate current v1 defaults from deferred work such as CI wiring, provider-specific persistence, schema generation, and project-layout reconciliation.).",
    "DoD check passed: The refined standards artifact is present in an approved planning or docs surface and can be attached or referenced by the charter epic. (The standards artifact is present under the approved docs/plans surface at docs/plans/shared-implementation-standards.md.).",
    "DoD check passed: The artifact names the existing child-ticket relations and downstream dependency enough that related work can reference the shared source of truth. (The Relationship Context section names charter ticket 06EXB4MDREV2T51VJNJEP6R0WR, child tickets 06EXB6P4ZNYA46MSYRGAJ9ZEPM and 06EXB6PDF0DSHE68B3V0656DJM, and blocked ticket 06EXB6XBV95E08R2W9ZQ1PRDPM.).",
    "DoD check passed: No unresolved PO-level architecture questions remain for formatting, encoding, file layout, documentation baseline, or current v1 naming/hash/persistence defaults. (The artifact states no unresolved PO-level architecture questions remain for formatting, encoding, repository layout, documentation baseline, current .NET baseline, v1 naming defaults, v1 hashing defaults, or v1 persistence defaults.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: A shared standards document or charter-attached planning artifact exists and is suitable for downstream tickets to reference directly. (docs/plans/shared-implementation-standards.md exists and is directly referenceable by content, but it is not ready as the shared source of truth because bash tools/check-format.sh reports the artifact itself is missing a final newline.).",
    "DoD check failed: The final contract keeps implementation details bounded to governance documentation and does not require product-code edits. (The non-.gicket diff is not bounded to governance documentation: it deletes DVault.Build.csproj, DVault.csproj, and DVault.sln in addition to adding the standards document and editing docs/formatting.md.).",
    "Blocking: the required non-mutating formatting gate fails on the delivered tree, including the new shared standards artifact.",
    "Blocking: the implementation changes root build/solution files outside the governance-documentation surface called for by this story."
  ],
  "evidence": [
    "git rev-parse --verify b2c3707e901b^{commit} returned b2c3707e901b5c1adb46171b647420146a193ffb.",
    "git rev-parse ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards returned 2ad37d0f94a77fdfccc0180a5d95e1503abd8665; git merge-base --is-ancestor b2c3707e901b target exited 0, and git diff b2c3707e901b..2ad37d0f94a77fdfccc0180a5d95e1503abd8665 -- . \u0027:!.gicket\u0027 produced no output.",
    "git diff --name-status develop...b2c3707e901b -- . \u0027:!.gicket\u0027 listed D DVault.Build.csproj, D DVault.csproj, D DVault.sln, M docs/formatting.md, and A docs/plans/shared-implementation-standards.md.",
    "git ls-tree -r --name-only b2c3707e901b included docs/plans/shared-implementation-standards.md, docs/formatting.md, docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, docs/plans/dvault-v1-default-persistence-convention-policy.md, src/DCoding.Data.DVault/.gitkeep, src/DVault/DVault.csproj, and tools/check-format.sh.",
    "git show b2c3707e901b:docs/plans/shared-implementation-standards.md showed sections for Relationship Context, Source Of Truth Documents, Formatting And Encoding, Repository Layout, .NET Project Baseline, Namespaces And Naming, Data Vault Concepts, Stable Hashing, V1 Persistence Conventions, Documentation Standards, V1 Defaults, Deferred Follow-Up Work, and Acceptance Baseline.",
    "git show b2c3707e901b:src/DVault/DVault.csproj showed TargetFramework net10.0, ImplicitUsings enable, Nullable enable, and GenerateDocumentationFile true.",
    "git status --short -- . \u0027:!.gicket\u0027 \u0027:!.gicket-bot\u0027 produced no output for non-operational paths.",
    "bash tools/check-format.sh exited 1 and reported missing final newlines for governed files including .editorconfig, .gitattributes, DVault.slnx, README.md, docs/formatting.md, docs/plans/shared-implementation-standards.md, src/DVault/DVault.csproj, tests/DVault.Tests/DVault.Tests.csproj, and tools/check-format.sh.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/governance, automation/bot-ready, backlog/initial-dvault, needs-test, type/story, bot/lease:hp-ai-2026-001.2].",
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
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards\u0027.",
    "Ticket history references implementation commit \u0027b2c3707e901b\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Restore final newlines for all governed files so bash tools/check-format.sh exits 0 on the target branch.",
    "Remove, justify in the authoritative contract, or split the DVault.Build.csproj, DVault.csproj, and DVault.sln deletions into an owning build/project-layout ticket.",
    "After rework, rerun the formatting gate and perform dotnet test --nologo in a write-enabled or legacy verification environment if executable verification remains required."
  ],
  "branchName": "ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards",
  "commitSha": "b2c3707e901b"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EXB6NWYVB37D7S74VB3PVTCC`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards`