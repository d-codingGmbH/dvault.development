[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FH8QAVJFXANVQFXGPYVAFXSR\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp\u0027 from source \u0027ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp\u0027.",
    "Evidence: git rev-parse HEAD returned 12cb645340c9b3b997ae4ef5a203d0de9dd83056 on branch ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp.",
    "Evidence: git diff --name-status develop...HEAD showed only .gicket ticket-metadata changes on this parent branch; no product repository paths were added or modified outside ticket metadata.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj contains \u003CTargetFramework\u003Enetstandard2.0\u003C/TargetFramework\u003E and target AddAnalyzerPackageAssets with PackagePath=\u0022analyzers/dotnet/cs/\u0022 entries for the analyzer DLL, XML docs, and companion assemblies.",
    "Evidence: artifacts/packages currently contains 18 .nupkg files and 16 .snupkg files. unzip -l on artifacts/packages/DCoding.Data.DVault.Analyzers.8.50.0.nupkg showed analyzers/dotnet/cs/DCoding.Data.DVault.Analyzers.dll, analyzers/dotnet/cs/DCoding.Data.DVault.Analyzers.xml, Microsoft.CodeAnalysis.Workspaces.dll, and System.Text.Json.dll under the single analyzer asset root.",
    "Evidence: tools/pack-release-packages.sh still contains pack_line \u00228.50.0\u0022 \u0022net8.0\u0022 and pack_line \u002210.50.0\u0022 \u0022net10.0\u0022. tools/run-analyzer-package-smoke.sh still maps SDK major 8 to package 8.50.0 and major 10 to package 10.50.0, and references DCoding.Data.DVault.Analyzers with PrivateAssets=all.",
    "Evidence: docs/package-compatibility.md still documents the visible package lines as 8.50.0/net8.0 and 10.50.0/net10.0, says consumers must not mix those lines, and says the analyzer package ships one netstandard2.0 asset under analyzers/dotnet/cs/.",
    "Evidence: .gicket/relations/SR/QC/06FH8QAVJFXANVQFXGPYVAFXSR--06FH8RP1SBVZ7K3K48ERGZSMQC--blocks.json is missing on the branch, while .gicket/relations/QM/SR/06FH8QRPDP10ZBAF3A5RYQFFQM--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json, .gicket/relations/AM/SR/06FH8R33YACW00JA0GNVEDP1AM--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json, .gicket/relations/HM/SR/06FH8R4EF1QFF2E3ZWS3P1BWHM--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json, and .gicket/relations/Z4/SR/06FH8R733TZ6P8DFYCRV1M8RZ4--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json still exist.",
    "Evidence: comment .gicket/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/comments/06FHCCE01QT7KAZKVBTZ6JCQVC.md states that each stale done-child blocks edge back to the parent now has a durable queued removal on its canonical owner branch.",
    "Evidence: .gicket/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/description.md still includes the legacy future-line sentence in the Original Ticket Draft section below the authoritative contract block.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/compatibility, area/package, area/source-generators, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 9 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027develop\u0027.",
    "Evidence: Ticket history references implementation commit \u00273bdfe30ecb49\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 2 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: The parent contract states only the implemented baseline visible in repository evidence: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets netstandard2.0, and the analyzer package remains one asset root under analyzers/dotnet/cs/. (The delivery-contract block states the netstandard2.0 baseline, src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets netstandard2.0, and its AddAnalyzerPackageAssets target packs one analyzer asset root under the package archive path analyzers/dotnet/cs/.).",
    "AC check passed: Repository validation evidence for both supported analyzer hosts remains the bounded proof surface for this story: tools/pack-release-packages.sh, tools/run-analyzer-package-smoke.sh 8, tools/run-analyzer-package-smoke.sh 10, and package verification. (The bounded proof surface is still the pack script, the SDK 8 and SDK 10 smoke script lanes, and package verification. tools/pack-release-packages.sh still packs 8.50.0/net8.0 and 10.50.0/net10.0, tools/run-analyzer-package-smoke.sh still maps majors 8 and 10 to those lines, and the current artifacts/packages inventory matches that surface.).",
    "AC check passed: Consumer guidance for this story stays on the current visible package lines 8.50.0 and 10.50.0, with local analyzer references using PrivateAssets=all and no mixed-line install guidance. (README.md, docs/package-compatibility.md, and docs/manual-nuget-publication.md still document only the 8.50.0 and 10.50.0 consumer lines, require PrivateAssets=all for analyzer references, and reject mixed-line installs.).",
    "AC check passed: Future 8.51.0 / 10.51.0 release-surface movement is explicitly excluded from this parent and handed to ticket 06FH8RP1SBVZ7K3K48ERGZSMQC. (The delivery-contract Scope Out section assigns 8.51.0 / 10.51.0 release-surface work to ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, and the parent-owned blocks relation file .gicket/relations/SR/QC/06FH8QAVJFXANVQFXGPYVAFXSR--06FH8RP1SBVZ7K3K48ERGZSMQC--blocks.json is absent on this branch.).",
    "DoD check passed: The strategy, implementation, smoke/verifier, and documentation child tickets for the analyzer-host baseline are complete and remain consistent with current repository evidence. (Child tickets 06FH8QRPDP10ZBAF3A5RYQFFQM, 06FH8R33YACW00JA0GNVEDP1AM, 06FH8R4EF1QFF2E3ZWS3P1BWHM, and 06FH8R733TZ6P8DFYCRV1M8RZ4 all show status done, and the current analyzer project, plan docs, package docs, and packaged analyzer artifacts remain aligned with the parent baseline.).",
    "DoD check passed: This parent no longer owns a live blocks dependency on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, and stale child-to-parent blocks removals have been materialized as applied or queued source-owner relation cleanups. (The parent no longer owns the future roll-forward blocks edge, and the persisted ticket evidence still states that stale child-to-parent blocks removals were queued on canonical owner branches. The contract allows those cleanups to be applied or queued, and comment 06FHCCE01QT7KAZKVBTZ6JCQVC.md explicitly records durable queued removal for each stale done-child edge.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: No ticket text for this parent reintroduces the superseded .NET 10 SDK-only analyzer-host assumption or mixes the landed 8.50.0 / 10.50.0 baseline with future 8.51.0 / 10.51.0 release wording. (The current ticket description still contains an Original Ticket Draft section with the sentence \u0027The 8.51.0 analyzer package must be usable from a pure .NET 8 SDK build host, while 10.51.0 keeps the existing .NET 10 path.\u0027 That persisted text still mixes the landed 8.50.0 / 10.50.0 baseline with future 8.51.0 / 10.51.0 wording.).",
    "Definition of Done 3 is still not satisfied because the persisted parent ticket text continues to include future 8.51.0 / 10.51.0 wording in the legacy draft section."
  ],
  "evidence": [
    "git rev-parse HEAD returned 12cb645340c9b3b997ae4ef5a203d0de9dd83056 on branch ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp.",
    "git diff --name-status develop...HEAD showed only .gicket ticket-metadata changes on this parent branch; no product repository paths were added or modified outside ticket metadata.",
    "src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj contains \u003CTargetFramework\u003Enetstandard2.0\u003C/TargetFramework\u003E and target AddAnalyzerPackageAssets with PackagePath=\u0022analyzers/dotnet/cs/\u0022 entries for the analyzer DLL, XML docs, and companion assemblies.",
    "artifacts/packages currently contains 18 .nupkg files and 16 .snupkg files. unzip -l on artifacts/packages/DCoding.Data.DVault.Analyzers.8.50.0.nupkg showed analyzers/dotnet/cs/DCoding.Data.DVault.Analyzers.dll, analyzers/dotnet/cs/DCoding.Data.DVault.Analyzers.xml, Microsoft.CodeAnalysis.Workspaces.dll, and System.Text.Json.dll under the single analyzer asset root.",
    "tools/pack-release-packages.sh still contains pack_line \u00228.50.0\u0022 \u0022net8.0\u0022 and pack_line \u002210.50.0\u0022 \u0022net10.0\u0022. tools/run-analyzer-package-smoke.sh still maps SDK major 8 to package 8.50.0 and major 10 to package 10.50.0, and references DCoding.Data.DVault.Analyzers with PrivateAssets=all.",
    "docs/package-compatibility.md still documents the visible package lines as 8.50.0/net8.0 and 10.50.0/net10.0, says consumers must not mix those lines, and says the analyzer package ships one netstandard2.0 asset under analyzers/dotnet/cs/.",
    ".gicket/relations/SR/QC/06FH8QAVJFXANVQFXGPYVAFXSR--06FH8RP1SBVZ7K3K48ERGZSMQC--blocks.json is missing on the branch, while .gicket/relations/QM/SR/06FH8QRPDP10ZBAF3A5RYQFFQM--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json, .gicket/relations/AM/SR/06FH8R33YACW00JA0GNVEDP1AM--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json, .gicket/relations/HM/SR/06FH8R4EF1QFF2E3ZWS3P1BWHM--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json, and .gicket/relations/Z4/SR/06FH8R733TZ6P8DFYCRV1M8RZ4--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json still exist.",
    "comment .gicket/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/comments/06FHCCE01QT7KAZKVBTZ6JCQVC.md states that each stale done-child blocks edge back to the parent now has a durable queued removal on its canonical owner branch.",
    ".gicket/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/description.md still includes the legacy future-line sentence in the Original Ticket Draft section below the authoritative contract block.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/compatibility, area/package, area/source-generators, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 9 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027develop\u0027.",
    "Ticket history references implementation commit \u00273bdfe30ecb49\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 2 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Remove or rewrite the Original Ticket Draft text in .gicket/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/description.md so the parent ticket text no longer carries 8.51.0 / 10.51.0 scope wording.",
    "Return the ticket to test after that ticket-text cleanup. The repository-backed analyzer package baseline and queued relation-cleanup evidence were otherwise consistent with the parent contract.",
    "If the next tester pass still needs policy executable proof for solution test or formatting in a read-only session, route that through legacy verification rather than treating sandbox write limits as a product failure."
  ],
  "branchName": "ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp",
  "commitSha": null
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FH8QAVJFXANVQFXGPYVAFXSR`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp`