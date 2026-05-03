<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the live ticket, comments, relations, and repository baseline; this work is bounded to six packable packages with package-specific API snapshot baselines and is ready for PO-critic without creating new planning artifacts.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Persisted comments added on 2026-05-03 are automation follow-up, claim, and lease notes only; there is no human scope change or attachment to incorporate into this refinement.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement pass.

### Scope In
- A repository-enforced API review gate for the six packable packages: `DCoding.Data.DVault`, `DCoding.Data.DVault.Sqlite`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.SqlServer`, `DCoding.Data.DVault.Oracle`, and `DCoding.Data.DVault.MySql`.
- Committed approval, baseline, or compatibility snapshot artifacts that record each package's public API separately and require a deliberate update when that package surface changes.
- Coverage of the current consumer-facing public API, including `AddDVault*` registration entry points, `UseDataVault`, `ApplyDataVaultMetadata`, `IDataVaultSaveService`, provider save-strategy contracts, and provider capability/profile contracts.
- Contributor-facing documentation that explains how to run the API review locally, interpret package-specific output, and intentionally update approved baselines.

### Scope Out
- `src/DCoding.Data/DCoding.Data.csproj`, test projects, and benchmarks as API-review targets, because they are non-packable and not current release surfaces.
- Provider-specific runtime behavior, persistence semantics, or new public API design beyond reviewing the surfaces that already exist.
- A release-history or published-NuGet backward-compatibility program beyond the v1 repository baseline for the current packable packages.
- The separate one-member-per-file analyzer work already tracked by `06EXB81QXE7XJPNM6NTPYCTP1M`.

## Acceptance Criteria
- Running the agreed validation path fails when the built public API for any one of the six packable packages differs from its committed approved baseline unless that package baseline is deliberately updated in the same change.
- Review output is package-aware and distinctly reports core, SQLite, PostgreSQL, SQL Server, Oracle, and MySQL surfaces so provider-package changes cannot mask core-package changes.
- The baseline covers the current consumer-visible API emitted by each packable package, including the core save, modeling, and provider-capability contracts plus each provider package's registration extensions.
- Contributor documentation explains the baseline artifact location, the command or test entry point used to regenerate it, and the expected workflow for approving intentional API changes.

## Definition of Done
- Committed baseline artifacts exist for each of the six packable packages and are stored in a deterministic repository location alongside the owning tests or contract checks.
- The chosen gate is wired into normal repository validation for this codebase, and unchanged baseline runs pass without manual intervention.
- A deliberate API change demonstrably requires both source changes and an explicit baseline update for the affected package surface.
- Implementation and documentation continue to follow shared repository standards, including the existing snapshot-style test conventions already used in DVault tests.

## Implementation Notes
- Repository evidence already fixes the v1 package baseline: the six packable projects are the core package plus provider packages `Sqlite`, `Postgres`, `SqlServer`, `Oracle`, and `MySql`; `src/DCoding.Data`, tests, and benchmarks stay out of the API-review target set.
- Provider packages currently expose the same `DCoding.Data.DVault` namespace as the core package, so the review mechanism must group by package or assembly output rather than by namespace or a single aggregated API file.
- No API approval tooling is present today, while `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` already uses committed text snapshots; a committed package-specific snapshot artifact is the safe v1 default unless an equivalent package-aware compatibility tool is added.
- Upstream ticket `06EXB817Q8RAXCQH5QQR5RFY34` is already `done`, so this ticket can assume the current documented public surface is the baseline to capture rather than reopening XML-doc coverage or package-boundary questions.

## Open Questions
- none

## Follow-Up Questions
- After the first public package release, should DVault add a second compatibility check against the last published NuGet versions in addition to the repository-managed baselines?
- If new packable provider packages are added later, should the API-review mechanism auto-discover packable `src/DCoding.Data.DVault.*` projects or require an explicit allowlist update?

## Risks
- A namespace-based or single aggregated snapshot would be misleading because the provider packages share the `DCoding.Data.DVault` namespace and could hide package-boundary regressions.
- If the check inspects only source declarations and not built package or assembly output, it can miss packaging-level API drift or attribute public surface changes to the wrong package.

## Split Recommendations
- No additional split is recommended; the ticket is already bounded to one package-aware API review gate, with XML-doc enforcement upstream in `06EXB817Q8RAXCQH5QQR5RFY34` and one-member-per-file analyzer work downstream in `06EXB81QXE7XJPNM6NTPYCTP1M`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Add a review mechanism for public API changes.

## Current Baseline
- Public API now includes the core API, provider capability/strategy contracts, and provider package registration extensions.
- API review should preserve package boundaries so provider package additions do not hide core API changes.

## Scope
- Use an approval, baseline, or compatibility test approach appropriate for the repo.
- Capture API snapshots per packable package or with equivalent package-aware grouping.

## Acceptance Criteria
- Public API changes require deliberate baseline updates.
- The mechanism is documented for contributors.
- API review output distinguishes core, SQLite, PostgreSQL, SQL Server, Oracle, and MySQL package surfaces.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.