[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is scoped correctly as packaging/build configuration, but the target branch still lacks the prerequisite packageable DVault project, so developer handoff would immediately hit the documented blocker.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- git status shows current branch `ticket/06EXB6YVY0WHJYJ7ZNPE00K0AM-task-configure-xml-documentation-and-determinist` tracking origin; `git rev-parse HEAD` returned `821a89e81bcaffae11f14692ec7dfbe5864d0bba`.
- `.gicket/tickets/06EXB6YVY0WHJYJ7ZNPE00K0AM/description.md` Delivery Contract says to run this ticket only after the foundation source/test layout exists on the target branch and not to scaffold `src/DVault`, `tests/DVault.Tests`, a solution, or a new `.csproj` here.
- The same contract Definition of Done says: before implementation, confirm the target branch contains the packageable `src/DVault` project; if it does not, return the ticket as blocked by missing foundation layout rather than scaffolding it.
- `git ls-files 'src/**' 'tests/**' '*.sln' '*.slnx' '*.csproj' 'Directory.Build.props' 'Directory.Build.targets' '*.props' '*.targets'` produced no output.
- `find src tests -maxdepth 3 -type f -o -type d 2>/dev/null | sort` produced no output, confirming no current `src` or `tests` directory in the worktree.
- Comment `.gicket/tickets/06EXB6YVY0WHJYJ7ZNPE00K0AM/comments/06EXCXH3CY0DADC3KGZNZ22GD4.md` resolved the prior dev question by saying this ticket must wait for or target a branch that already contains the intended packageable `src/DVault` project and, when available, `tests/DVault.Tests`.
- Comment `.gicket/tickets/06EXB6YVY0WHJYJ7ZNPE00K0AM/comments/06EXC9D486GCXHY4406BDVR95R.md` records the previous dev workflow returning because no `src/**`, `tests/**`, solution, `.csproj`, `Directory.Build.*`, `NuGet.config`, or `global.json` files were present.
- Foundation work exists in backlog: `06EXB6XBV95E08R2W9ZQ1PRDPM` is `Story: Create .NET 10 solution skeleton using .slnx`; child task `06EXB6XVWBWZGN6MA3SFWGWKM4` is `Task: Add DCoding.Data.DVault library project targeting net10.0`; child task `06EXB6XKXCG27GYB88KKZVBR3G` covers `DVault.slnx` and source/test folders.
- `rg` over `.gicket/relations` for this ticket found only parent relation `06EXB6YBXPDBPWZPNV89A9F9AM--06EXB6YVY0WHJYJ7ZNPE00K0AM--parentOf.json`; no blocking relation from the foundation tasks to this packaging task was observed.

Blocking findings
- The target branch is still missing the packageable `src/DVault` project that this ticket explicitly requires before implementation. Sending it to dev now would violate the ticket's own Definition of Done and reproduce the prior dev blocker.
- The prerequisite foundation backlog items are present, but this ticket does not currently have an observed `blocks` relation from the solution/library foundation task(s), so automation can still pick it up before the required layout exists.

Required PO actions
- Return the ticket to PO refinement or blocked state until the target branch includes the packageable `src/DVault` project.
- Add or update ticket-level dependency/blocked-by relation(s) from the foundation work that creates `DVault.slnx`, `src/DVault`, and the main library project, especially `06EXB6XBV95E08R2W9ZQ1PRDPM` and/or `06EXB6XVWBWZGN6MA3SFWGWKM4`.
- Remove or withhold developer-ready routing labels/status until the prerequisite project layout is actually present on the target branch or the ticket is retargeted to a branch that contains it.

Open issues ledger
- critic-item-1 [required-po-action] Return the ticket to PO refinement or blocked state until the target branch includes the packageable `src/DVault` project.
- critic-item-2 [required-po-action] Add or update ticket-level dependency/blocked-by relation(s) from the foundation work that creates `DVault.slnx`, `src/DVault`, and the main library project, especially `06EXB6XBV95E08R2W9ZQ1PRDPM` and/or `06EXB6XVWBWZGN6MA3SFWGWKM4`.
- critic-item-3 [required-po-action] Remove or withhold developer-ready routing labels/status until the prerequisite project layout is actually present on the target branch or the ticket is retargeted to a branch that contains it.
- critic-item-4 [blocking-finding] The target branch is still missing the packageable `src/DVault` project that this ticket explicitly requires before implementation. Sending it to dev now would violate the ticket's own Definition of Done and reproduce the prior dev blocker.
- critic-item-5 [blocking-finding] The prerequisite foundation backlog items are present, but this ticket does not currently have an observed `blocks` relation from the solution/library foundation task(s), so automation can still pick it up before the required layout exists.

Missing examples / edge cases
- No package/sample artifact expectations can be validated yet because there is no packageable project on the branch.
- The contract permits skipping `tests/DVault.Tests` validation when the test project is absent, but it still requires the packageable source project before implementation.

Risky assumptions
- Assuming SourceLink verification will be possible locally remains conditional on eventual repository host/remote metadata, as the contract already notes.

AC / test suggestions
- Keep the current AC framed around an existing packageable `src/DVault` project, but add ticket-level gating that prevents dev handoff until that project exists.
- When unblocked, require local `dotnet build`/`dotnet pack` evidence and artifact inspection for XML documentation plus source/symbol metadata; run `tests/DVault.Tests` only if that project exists.

Implementation watchouts
- Do not scaffold `src/DVault`, `tests/DVault.Tests`, solution files, or new project files under this ticket.
- When the prerequisite project exists, prefer shared build configuration only if it matches the actual project layout; otherwise configure the existing packageable project directly.
- Avoid broad API or documentation-content changes solely to satisfy XML documentation warnings.

Non-blocking notes
- The persisted `## Open Questions` section says `none`, so the return is not due to unresolved PO questions; it is due to the explicitly documented prerequisite still being absent on the target branch.
- The current description diff appears line-ending-only for the ticket description content, not a substantive contract change.

Split recommendations
- Do not split this ticket to include scaffolding; keep scaffolding in the existing foundation tasks.
- If automation needs stronger sequencing, create or link a dependency relation rather than expanding this ticket's implementation scope.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment