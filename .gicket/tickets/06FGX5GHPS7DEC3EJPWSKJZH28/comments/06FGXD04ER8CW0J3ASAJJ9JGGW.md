[gicket-bot] PO refinement contract

Summary
- Refined this as an evidence-backed audit ticket. Current repository state already ratifies a single net10.0 analyzer asset and a .NET 10 SDK build-host baseline for both package lines, so the ticket should document the exact Roslyn/source-generator/code-fix/package couplings and decide whether a separate implementation ticket is warranted. No child tickets, relation writes, description writes, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- docs/plans/analyzer-package-compatibility-audit.md already establishes the current v0.49.0 baseline: DCoding.Data.DVault.Analyzers ships one net10.0 analyzer asset and the repository does not validate pure .NET 8 SDK analyzer consumption.
- This ticket should refine that baseline into a dependency audit, not reopen the current support claim. The expected outcome is a file-backed go/no-go assessment for pure .NET 8 SDK hosts and the minimal bounded follow-up if support is required.
- Fresh repository inspection confirmed the baseline is consistent across packaging, tests, README/docs, package verification, and CI rather than being an isolated project-file quirk.

Scope In
- Audit src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj for target framework, Roslyn assembly references, Workspaces/System.Composition references, and packaging behavior under analyzers/dotnet/cs/.
- Separate dependency findings for the diagnostic analyzers, the two source generators, and the code-fix provider instead of treating the package as one undifferentiated slice.
- Document the current proof points for the .NET 10 SDK host baseline across tests, package scripts, package verification, README/docs, and CI.
- Produce a clear next-step recommendation: keep the current baseline with documentation only, or raise bounded implementation follow-up for retargeting plus validation.

Scope Out
- Retargeting or multi-targeting the analyzer package in this ticket.
- Changing runtime package target frameworks, EF Core version lines, or provider package dependency policy.
- Adding a new .NET 8 SDK CI lane, package verifier behavior, or packaging layout as part of this audit ticket.
- Claiming pure .NET 8 SDK analyzer support without a later implementation and validation lane.

Open questions
- none

Follow-up questions
- If pure .NET 8 SDK analyzer consumption becomes a product requirement, is the preferred compatibility target one netstandard2.0 analyzer asset, one net8.0 analyzer asset, or a multi-targeted or split-asset strategy?
- If the code-fix provider remains the only Workspaces/System.Composition-coupled slice, should a future implementation keep it in the same package asset or allow a separate asset/package boundary from the analyzers and source generators?
- Should a future implementation ticket add a dedicated .NET 8 SDK CI and package-verification lane before any README claim changes?

Risks
- The visible 8.49.0 package line can be misread as .NET 8 SDK host support unless the audit preserves the current .NET 10 SDK host wording already enforced in README and PackageVerifier.
- The SDK-local HintPath references to MSBuildToolsPath and DotnetTools/dotnet-format make analyzer and test resolution sensitive to SDK layout, so a retargeting effort can fail even before source-level API issues are addressed.
- Current validation proves a net8.0 consumer target compiled with the net10.0 analyzer asset; it does not prove pure .NET 8 SDK host compatibility.

Split recommendations
- If implementation follow-up is approved, split it into one ticket for analyzer target and asset strategy plus Roslyn reference normalization, and a second ticket for CI, package-verifier, packaging, and documentation updates required by the chosen host baseline.
- If the audit finds the code-fix provider to be the only hard blocker for a lower analyzer target, consider a separate follow-up slice for code-fix packaging rather than forcing the analyzer and source-generator paths to move together.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment