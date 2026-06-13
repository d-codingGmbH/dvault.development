[gicket-bot] PO refinement contract

Summary
- Refined the ticket to ratify the audited compatibility outcome: keep DCoding.Data.DVault.Analyzers on the single net10.0 analyzer asset/.NET 10 SDK build-host baseline, align the root and analyzer README guidance to that boundary, and keep package verification enforcing it so net8 projects are not misled.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The compatibility outcome is already evidenced locally: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets only net10.0 and packs the analyzer under analyzers/dotnet/cs/ for both coordinated package lines.
- The ticket is a documentation-and-verification alignment task, not a request to retarget analyzer assets or broaden support beyond the verified .NET 10 SDK build-host baseline.
- Direct local proof for the intended supported lane already exists in tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, which multi-targets net8.0/net10.0 but forces the analyzer project reference to TargetFramework=net10.0.
- No child-ticket, relation, description, attachment, or planning-document write was needed for this refinement because docs/plans/analyzer-package-compatibility-audit.md already provides the authoritative compatibility evidence.

Scope In
- Keep root README installation guidance and src/DCoding.Data.DVault.Analyzers/README.md explicit that 8.36.0 is the net8.0 package line but analyzer consumption still uses a .NET 10 SDK build host.
- Keep analyzer package examples local with PrivateAssets="all" and aligned to the same 8.36.0 or 10.36.0 coordinated package line as the runtime/provider packages.
- Keep package verification aligned with the accepted compatibility claim so packaged README guidance cannot silently drift into broader unsupported promises.

Scope Out
- Retargeting the analyzer project or packaged analyzer asset from net10.0 to net8.0.
- Claiming or proving support for analyzer consumption from a pure .NET 8 SDK host baseline.
- Changing runtime/provider package targeting, EF Core line selection, or broader package family versioning decisions outside the analyzer-guidance contract.

Open questions
- none

Follow-up questions
- If product intent later expands to "net8 target project plus pure .NET 8 SDK host" support, should a follow-up ticket retarget the analyzer asset and add an explicit verification lane for that exact baseline?
- Should the same build-host caveat be echoed in secondary release/publication docs beyond the packaged README surfaces, or is the current root/analyzer README boundary sufficient for v0.36.x?

Risks
- As long as the analyzer remains a single net10.0 asset, any future documentation or package-metadata change that implies pure .NET 8 SDK support will overstate what the repository currently verifies.
- Live ticket relations still show this ticket blocked by 06FBSBWBT33K7Y1Z6NM71GAQ68 and blocking 06FBSBWPN112S4CGP0239K0ZT8, so delivery sequencing can still depend on external ticket flow even though PO refinement is complete.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment