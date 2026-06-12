[gicket-bot] PO refinement contract

Summary
- Refined the story around a net10.0-only analyzer baseline for 8.36.0/net8.0 consumers on the repository's .NET 10 SDK build-host contract, and wrote docs/plans/analyzer-package-compatibility-audit.md with the audit proof.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Checked-in evidence supports the 8.36.0 analyzer package as a net8.0 consumer-target story compiled on the documented .NET 10 SDK baseline; the repository does not currently prove a pure .NET 8 SDK analyzer-consumption baseline.
- The analyzer package project targets only net10.0 and packs analyzer assets under analyzers/dotnet/cs/, so the current 8.36.0 and 10.36.0 analyzer lines differ by package version, not by consumer-target-specific analyzer binaries.
- Created planning note docs/plans/analyzer-package-compatibility-audit.md capturing the decision, proof, and follow-up boundaries.

Scope In
- Audit the current analyzer package target, packaging layout, and validation story for net8.0 consumers using the 8.36.0 package line.
- Ratify whether the current net10.0 analyzer asset is acceptable as the supported baseline or whether retargeting is required for the claimed compatibility surface.
- Define the documentation and verification work needed so the accepted compatibility claim matches what the repository actually proves.

Scope Out
- Changing runtime or provider package target frameworks.
- Broad analyzer feature work unrelated to package-host compatibility.
- Claiming support for a pure .NET 8 SDK analyzer-consumption baseline unless the analyzer assets and verification lane are explicitly changed to prove it.

Open questions
- none

Follow-up questions
- If product guidance needs to support net8.0 projects built on a pure .NET 8 SDK, should that become a separate compatibility commitment instead of broadening the current net10.0-host baseline implicitly?
- Should live ticket relations be normalized so this story tracks both existing follow-up tasks explicitly, given that the current live relation state only blocks 06FBSBWBT33K7Y1Z6NM71GAQ68?

Risks
- Current public installation guidance shows the 8.36.0 analyzer package for net8.0 projects without an explicit build-host SDK requirement, so leaving the docs unchanged would overstate the verified compatibility baseline.
- Package verification currently proves version-aligned analyzer docs and asset presence but not a host-SDK compatibility lane, so future changes could silently drift from the accepted compatibility claim.
- The current live relation state is asymmetric: this story blocks the implementation task 06FBSBWBT33K7Y1Z6NM71GAQ68, while the documentation task 06FBSBWH9F415E12VRHRYQ2JJM exists without a live relation on the story.

Split recommendations
- Keep the current split: 06FBSBWBT33K7Y1Z6NM71GAQ68 owns any analyzer asset-target change or explicit SDK gate, and 06FBSBWH9F415E12VRHRYQ2JJM owns README and package-verification alignment.
- Do not create additional child tickets unless the team chooses to support a pure .NET 8 SDK analyzer-consumption baseline as a distinct compatibility promise.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment