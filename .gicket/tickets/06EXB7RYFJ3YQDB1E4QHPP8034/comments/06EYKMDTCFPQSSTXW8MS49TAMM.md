[gicket-bot] PO-critic review contract

Summary
- Earlier scenario-gap blockers are resolved; the ticket now contains an exact two-event shared comparison contract and is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git symbolic-ref --short HEAD` returned `ticket/06EXB7RYFJ3YQDB1E4QHPP8034-task-implement-normal-ef-baseline-for-customer-p`; `git rev-parse HEAD`, the branch ref, and scratch ref `0f533b541cae545e9d79c6044d728515c22f800c` all matched, and `git diff --name-only 0f533b541cae545e9d79c6044d728515c22f800c..HEAD -- .gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034 docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md` returned no paths.
- Previous blocking PO-critic comment `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/comments/06EYJ6XVX2MCKZD06RME77X88R.md:15-22` required a concrete second state and a shared comparison contract; the later PO refinement comment `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/comments/06EYK1DCTADBT5G8E5QMQCAQRC.md:10-15` records critic-items 1-5 as answered.
- The current persisted contract in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:12-16,31-36` now names the authoritative shared artifact, fixes the exact `C-100` two-event sequence, requires exactly two persisted history rows, and requires proof that no extra unchanged replay row is inserted.
- `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:50-51` shows `## Open Questions` as `- none`.
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md:32-46` defines the exact plain-EF row contract, and `:48-57` binds the paired DVault ticket to the same two business events with its own exact persisted outcome.
- `README.md:7-23` and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-27` confirm the repository-level `DVault.slnx`/`dotnet test` execution surface and existing SQLite test dependency.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Approval assumes the existing shared contract file can serve as the 'comparison notes' artifact referenced in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:36,41`; the ticket does not name a second required documentation file.

AC / test suggestions
- Optional: replace 'comparison notes' in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:36,41` with explicit wording like 'the shared contract doc plus automated assertions' if you want to remove any residual ambiguity about deliverable location.

Implementation watchouts
- The scope-out is concrete, not abstract: `IDataVaultSaveService` exists in `src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21`, `ApplyDataVaultMetadata` exists in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29-38`, and `AddDVault` exists in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-25`, but this ticket explicitly wants ordinary EF `DbContext`/`DbSet` history modeling instead of those DVault surfaces.
- The existing SQLite integration harness is already present in `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-27`, `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs:22-34`, and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:18-30`; the baseline should fit that `dotnet test` path rather than introduce a new execution surface.

Non-blocking notes
- This ticket already blocks the paired DVault ticket via `.gicket/relations/34/X8/06EXB7RYFJ3YQDB1E4QHPP8034--06EXB7S6DB97GVVTS2GGZ3CCX8--blocks.json:3-5`, which matches the intended comparison sequencing.

Split recommendations
- Keep any runnable example or broader demo separate; this ticket is now specific enough to stay focused on the automated plain-EF baseline and the locked comparison contract.
- Keep broader change-history variants or replay/dedup cases as follow-up tickets instead of widening this v1 baseline.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment