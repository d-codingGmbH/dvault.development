[gicket-bot] PO refinement contract

Summary
- Audit evidence rejects analyzer package retargeting for the current v0.47.0 baseline, so this ticket should be refined as an audit-backed no-work closure that preserves the existing single net10.0 analyzer asset and .NET 10 SDK build-host baseline for both visible package lines.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository baseline is already bounded: DCoding.Data.DVault.Analyzers packs one net10.0 analyzer asset under analyzers/dotnet/cs/ for both 8.47.0 and 10.47.0 package lines.
- The audit does not approve retargeting to a net8.0 analyzer asset or claiming pure .NET 8 SDK analyzer consumption, so this ticket should not request a package target or asset change.
- Referenced repository documents already align on a .NET 10 SDK host requirement for analyzer consumption across both visible package lines, so the safe PO decision is to ratify that baseline rather than reopen it.

Scope In
- Record the audit-backed no-work decision for analyzer retargeting on the current v0.47.0 baseline.
- Keep analyzer compatibility guidance, verifier expectations, and test assumptions aligned with the current single net10.0 analyzer asset.
- Treat the current repository baseline as authoritative: net8.0 consumer projects may use the 8.47.0 package line, but validated analyzer use stays on a .NET 10 SDK build host.

Scope Out
- Retargeting DCoding.Data.DVault.Analyzers to net8.0 or shipping separate analyzer assets per package line.
- Claiming or validating pure .NET 8 SDK analyzer consumption for the analyzer package.
- Broader package-line, EF dependency, or runtime library retargeting outside this analyzer compatibility decision.

Open questions
- none

Follow-up questions
- Does product want a future compatibility commitment for net8.0 consumer projects built on a pure .NET 8 SDK host? If yes, open a separate ticket for analyzer retargeting plus an explicit verification lane.
- Should release-facing compatibility guidance call out the analyzer build-host requirement even more prominently for adopters who only look at the 8.47.0 package line?

Risks
- If future docs or release messaging imply that the 8.47.0 analyzer package is validated on a pure .NET 8 SDK host, they will exceed the repository-backed proof accepted here.
- A later requirement to support pure .NET 8 SDK analyzer consumption will need explicit implementation and verification work; treating it as incidental follow-on to this ticket would under-scope the change.

Split recommendations
- No split is recommended; current evidence supports a bounded no-work closure on the existing ticket.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment