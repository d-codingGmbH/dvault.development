[gicket-bot] PO refinement contract

Summary
- Repository evidence narrows this ticket to the explicit SDK-gate path: keep one `net10.0` analyzer asset for both package lines, document the `.NET 10 SDK` build-host requirement for `8.36.0` and `10.36.0` consumers, and keep verification aligned with that contract.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository already ratifies the audit outcome from ticket `06FBSBW6HDT15D1KGVD7XBQXM8`: `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0` and packs the analyzer under `analyzers/dotnet/cs/`, so this ticket should not reopen a dual-asset or `net8.0` analyzer decision.
- The validated v1 consumer story is `net8.0` or `net10.0` application targets built on a `.NET 10 SDK` host when they reference `DCoding.Data.DVault.Analyzers`; pure `.NET 8 SDK` analyzer consumption is not part of the current contract.
- The repository already carries the required build-host gate in `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/manual-nuget-publication.md`, and the package verification lane.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this run because the provided ticket context and repository evidence already narrow the scope sufficiently for PO review.

Scope In
- Keep the analyzer package as a single `net10.0` analyzer asset across the `8.36.0` and `10.36.0` coordinated package lines.
- Make or keep the `.NET 10 SDK` build-host requirement explicit anywhere analyzer installation guidance is given for both package lines, especially the `8.36.0` / `net8.0` story.
- Verify the selected compatibility contract through package verification and test coverage that proves the supported `net8.0` consumer target plus `net10.0` analyzer-host lane and the `net10.0` consumer lane.

Scope Out
- Retargeting the analyzer project to `net8.0` or adding a second analyzer asset to satisfy a pure `.NET 8 SDK` build-host scenario.
- Broadening the claim to pure `.NET 8 SDK` analyzer compatibility without new asset layout changes and new verification lanes.
- Changing runtime/provider package target frameworks, package ids, or non-analyzer release-family behavior.

Open questions
- none

Follow-up questions
- If product direction later requires pure `.NET 8 SDK` analyzer consumption, should that be approved as a separate compatibility expansion with analyzer retargeting and a dedicated verification lane?

Risks
- If any packaged README or installation surface drops the `.NET 10 SDK` host requirement, `8.36.0` consumers may reasonably assume unsupported pure `.NET 8 SDK` analyzer compatibility.
- A future attempt to advertise pure `.NET 8 SDK` analyzer compatibility without changing the analyzer asset target/framework would create a documentation-to-verification mismatch.

Split recommendations
- No split is needed for the current ticket. If pure `.NET 8 SDK` analyzer compatibility becomes a requirement, create a dedicated follow-up ticket for analyzer retargeting or package-layout changes plus a new verification lane.

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