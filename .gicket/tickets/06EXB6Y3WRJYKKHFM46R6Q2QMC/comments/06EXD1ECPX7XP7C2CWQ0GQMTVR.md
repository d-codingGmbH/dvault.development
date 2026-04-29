[gicket-bot] PO-critic review contract

Summary
- The revised contract matches the inspected empty implementation baseline and resolves the earlier PO-critic blockers; approve for developer handoff, with routing labels needing normal handoff cleanup.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB6Y3WRJYKKHFM46R6Q2QMC/description.md contains the persisted Delivery Contract with PO Handoff decision ready_for_po_critic and Open Questions set to none.
- The current contract states there is no existing src/DVault, tests/DVault.Tests, solution, or .NET project baseline, and Scope In now creates tests/DVault.Tests, unit/integration test projects, minimal dotnet test entry-point wiring, Sqlite utilities, and smoke/sample tests.
- git rev-parse HEAD returned 5aa0605fbbdca2a6ce52c4f97382e75294b05cff on target branch ticket/06EXB6Y3WRJYKKHFM46R6Q2QMC-task-add-test-projects-and-shared-test-utilities.
- git ls-tree -r --name-only HEAD -- src tests returned no output, and git ls-files for *.sln, *.slnx, *.csproj, Directory.Build.*, Directory.Packages.props, and global.json returned no output.
- Comment 06EXD06KHVN4FNWVDD0Y1M55KC records PO responses marking critic-item-1 through critic-item-4 answered, including that this ticket does not depend on existing .NET scaffolding and must not require a ProjectReference to a DVault source project.
- Earlier PO-critic comment 06EXCA9JFJ4MMHZ1VJ497SEED0 blocked the prior contract for assuming existing src/DVault and tests/DVault.Tests; the current description removes that incorrect baseline.
- rg found the .NET 10 planning basis in .gicket/tickets/06EXB4MDREV2T51VJNJEP6R0WR/description.md and in the current ticket contract, while no tracked package or project convention files exist.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Exact test project names and test framework are left to developer discretion; this is acceptable because the AC specify repository root test discovery, independent compilation, Sqlite helper behavior, and tests/DVault.Tests placement.
- Integration-test category or filter conventions are explicitly deferred to follow-up and are not required for this bootstrap ticket.

Risky assumptions
- Initial package versions remain a developer choice because no central package management files are visible on the branch.
- Local verification depends on .NET 10 SDK availability, which the ticket already calls out as a risk.

AC / test suggestions
- Keep the root-level dotnet test verification as the primary acceptance check.
- Ensure smoke/sample tests prove both unit and integration project discovery plus Sqlite helper setup and cleanup using in-memory or temporary-file databases.

Implementation watchouts
- Do not create src/DVault or the production DVault library project under this ticket.
- Do not add a ProjectReference to src/DVault while no source project exists on the branch.
- Keep shared Sqlite helpers in tests/DVault.Tests and use Microsoft.Data.Sqlite unless an implementation-time repository package convention is discovered.

Non-blocking notes
- The working tree has broad .gicket line-ending/modification noise and an untracked docs/plans file, but HEAD tree evidence still confirms no tracked src, tests, solution, project, or package convention files.
- No existing public API/type compatibility is required by the current contract because production source creation and ProjectReference wiring are explicitly out of scope.

Split recommendations
- No split recommended; the revised scope is bounded to test infrastructure plus minimal test-entry-point wiring, with production source-project creation left to downstream foundation work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment