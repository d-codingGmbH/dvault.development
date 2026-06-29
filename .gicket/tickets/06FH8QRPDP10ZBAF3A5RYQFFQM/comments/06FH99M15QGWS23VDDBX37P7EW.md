[gicket-bot] PO refinement contract

Summary
- Verified the current analyzer packaging constraints and added `docs/plans/analyzer-dotnet8-host-strategy-refinement.md`, which turns the v0.50 audit into one concrete design: keep one `DCoding.Data.DVault.Analyzers` package id, retarget the analyzer asset to `netstandard2.0`, and require explicit dependency, verifier, validation, and documentation updates before claiming pure `.NET 8 SDK` analyzer-host support.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository baseline is still one `net10.0` analyzer asset packed into `analyzers/dotnet/cs/`, with README and package-verifier guidance that explicitly require a `.NET 10 SDK` host for both `8.50.0` and `10.50.0` package lines.
- A ticket-bound planning note was materialized at `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` and is the authoritative refinement artifact for this ticket.
- No child tickets, relation changes, or ticket-description writes were materialized in this run; the current ticket remains a bounded design/planning item.

Scope In
- Choose one supported analyzer package shape for future pure `.NET 8 SDK` host support and reject the unresolved alternatives.
- Document the required dependency strategy for Roslyn, `Microsoft.CodeAnalysis.Workspaces`, `System.Composition`, and `System.Text.Json`.
- Define the required analyzer package path, pack-script, package-verifier, test-lane, and documentation updates needed before the repository may claim pure `.NET 8 SDK` analyzer-host support.

Scope Out
- Retargeting or editing product code, test projects, pack targets, or package verifier implementation in this ticket.
- Adding a second public analyzer package id or widening the coordinated nine-package family.
- Publishing packages or updating release-claim docs to state pure `.NET 8 SDK` analyzer-host support before both `.NET 8 SDK` and `.NET 10 SDK` proof lanes exist.

Open questions
- none

Follow-up questions
- If real `.NET 8 SDK` host proof shows that companion analyzer dependencies still do not load cleanly for the code-fix slice, should a later delivery ticket split code fixes into an optional package or asset set after this design baseline is implemented?
- If the team wants explicit IDE-host proof beyond CLI `.NET 8 SDK` and `.NET 10 SDK` build lanes, should that be scheduled as a separate validation follow-up rather than broaden this design ticket?

Risks
- Retargeting to `netstandard2.0` is not a csproj-only change: analyzer sources currently use modern BCL APIs and framework assumptions that will need bounded compatibility work.
- The package-verifier and README baselines currently hard-code the `.NET 10 SDK` host claim and a flat single-analyzer-asset expectation; those guardrails must change in lockstep with implementation or they will misreport the new package shape.
- If the reviewed analyzer companion-assembly strategy under `analyzers/dotnet/cs/` proves insufficient on actual `.NET 8 SDK` or IDE hosts, the later implementation may still need a narrower asset split despite this design decision.

Split recommendations
- No additional split is justified inside this design ticket; use `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` as the bounded handoff artifact for the later implementation ticket that changes project references, packing, verifier coverage, tests, and docs.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment