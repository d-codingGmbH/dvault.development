[gicket-bot] PO refinement contract

Summary
- Refined this as a tracking parent story: the strategy, implementation, proof/verifier, and documentation child tickets are done, repository evidence already ratifies the netstandard2.0 dual-SDK analyzer baseline, and the only live downstream dependency is ticket 06FH8RP1SBVZ7K3K48ERGZSMQC for the later 8.51.0 and 10.51.0 release-baseline roll-forward.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already fixes the bounded v1 baseline: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets netstandard2.0, docs/package-compatibility.md states one analyzer asset under analyzers/dotnet/cs/, and supported analyzer hosts are .NET 8 SDK and .NET 10 SDK for the current 8.50.0 and 10.50.0 package lines.
- Existing child tickets already cover the justified split: 06FH8QRPDP10ZBAF3A5RYQFFQM for strategy, 06FH8R33YACW00JA0GNVEDP1AM for implementation, 06FH8R4EF1QFF2E3ZWS3P1BWHM for smoke/verifier coverage, and 06FH8R733TZ6P8DFYCRV1M8RZ4 for documentation.
- The parent story still has one live outgoing blocks relation to 06FH8RP1SBVZ7K3K48ERGZSMQC for the v0.51.0 release-note and package-baseline roll-forward, and the live relation set also still contains stale incoming blocks from the done child tickets.

Scope In
- Track the completed analyzer-host delivery boundary as one netstandard2.0 DCoding.Data.DVault.Analyzers asset under analyzers/dotnet/cs/ with package-managed Roslyn, Workspaces, System.Composition, and System.Text.Json handling.
- Track dual-host proof for packaged analyzer consumption on pure .NET 8 SDK and .NET 10 SDK build hosts without runtime lib/<tfm> leakage and with PrivateAssets=all analyzer guidance preserved.
- Track coordinated verifier, smoke-test, CI, and documentation alignment for the analyzer-host support claim, plus the remaining release-baseline dependency that must carry the contract from the current 8.50.0 and 10.50.0 repository baseline to the planned 8.51.0 and 10.51.0 release surfaces.

Scope Out
- New public analyzer or code-fix package ids, target-specific analyzer asset trees, or runtime library assets for DCoding.Data.DVault.Analyzers.
- Analyzer-host compatibility claims beyond the repository-backed .NET 8 SDK and .NET 10 SDK boundary.
- Reopening the already-closed package-shape decision between one netstandard2.0 asset, dual target-specific assets, or a split code-fix package.

Open questions
- none

Follow-up questions
- Complete ticket 06FH8RP1SBVZ7K3K48ERGZSMQC so the current 8.50.0 and 10.50.0 analyzer-host contract is rolled forward into the planned 8.51.0 and 10.51.0 release-note and package-validation surfaces.
- Clean up the stale incoming child-to-parent blocks relations from done tickets 06FH8QRPDP10ZBAF3A5RYQFFQM, 06FH8R33YACW00JA0GNVEDP1AM, 06FH8R4EF1QFF2E3ZWS3P1BWHM, and 06FH8R733TZ6P8DFYCRV1M8RZ4 so the live relation graph matches the completed child state.
- If later editor or IDE hosts need explicit support claims beyond CLI SDK-host proof, schedule that as a separate validation follow-up instead of broadening this story.

Risks
- The parent story still depends on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, so the story description's 8.51.0 and 10.51.0 wording can outpace the current repository baseline until that release-baseline ticket lands.
- The live relation graph still shows stale incoming blocks from done child tickets, which can confuse downstream workflow or closure logic until cleaned.
- Future version-line updates can regress into stale net10-only or mixed-line guidance if release notes, package compatibility, verifier expectations, and install examples stop moving together.

Split recommendations
- No additional split is needed; the existing child tickets already cover strategy, implementation, smoke/verifier proof, and documentation.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment