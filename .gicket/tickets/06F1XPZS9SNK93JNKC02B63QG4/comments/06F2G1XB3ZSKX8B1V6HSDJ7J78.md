[gicket-bot] PO-critic review contract

Summary
- The ticket contract now matches the repository's four-role technical metadata baseline, has no unresolved Open Questions, and bounds the work to an explicit opt-in Added-row interceptor slice, so it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4/description.md now sets `## Open Questions` to `- none` and its acceptance criteria limit auto-population to `LoadTimestamp` and `RecordSource` while requiring `HashKey` and `HashDiff` to remain untouched.
- git diff 26ba8fa61c4f..70422d73012d -- .gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4/description.md shows the earlier two-role wording was replaced with the four-role baseline and explicit skip-`HashKey`/`HashDiff` language.
- src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs defines the closed public technical-role set as `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs exposes `DataVaultAnnotationNames.PropertyRole` and `DataVaultAnnotationNames.TechnicalColumnRole`, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs asserts generated hub/link/satellite metadata carries those annotations, including `HashDiff` on satellites.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers DVault services without adding any `ISaveChangesInterceptor`, and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs asserts `Assert.Empty(provider.GetServices<ISaveChangesInterceptor>())` on the default `AddDVault()` path.
- docs/architecture/dvault-v1-explicit-save-service.md states that SaveChanges interceptors remain outside the default v1 write path and the explicit `IDataVaultSaveService` boundary remains authoritative.
- .gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4/comments/06F2G068MV0FN268173H0HKYX0.md explicitly answers prior critic items 1-3, and commit `70422d73012d` is the ticket-level handoff commit that refreshed the description and ticket artifacts.
- .gicket/relations/V4/G4/06F1XPRY3ZDB6W1WQ9ABRRJ2V4--06F1XPZS9SNK93JNKC02B63QG4--blocks.json shows one incoming `blocks` relation, and .gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/ticket.json marks that epic `done`, so the relation is historical rather than a live blocker.
- git diff --name-only develop..HEAD and git log --oneline develop..HEAD show only `.gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4/...` artifacts and workflow commits (`8e2f87789`, `26ba8fa61`, `70422d730`, `47b06ec62`), with no `src/` or `tests/` code changes on this branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add an explicit partial-manual case where `LoadTimestamp` is preset and only `RecordSource` is filled.
- Add the mirror partial-manual case where `RecordSource` is preset and only `LoadTimestamp` is filled.
- A shared-type SQLite proof with an overridden effective technical column name, such as `RecordSource -> SourceSystem`, would strengthen the annotation-driven discovery story.

Risky assumptions
- Developers will treat the child ticket contract as authoritative even though the parent story still uses broader metadata language.
- SQLite proof will be read as the only promised provider baseline for this slice, not as a provider-neutral runtime guarantee.

AC / test suggestions
- Keep a unit test that the default `AddDVault()` path still resolves zero `ISaveChangesInterceptor` instances after the opt-in API lands.
- Add sync and async SQLite cases for the two partial-manual permutations where only one targeted metadata value is missing.
- Add a proof that annotation-driven selection ignores `HashKey` and `HashDiff` even though those properties also carry technical roles.
- If feasible, add one SQLite/shared-type case that uses an overridden effective column name for `RecordSource` or `LoadTimestamp`.

Implementation watchouts
- Do not treat every `DataVaultPropertyRole.Technical` property as auto-populatable; repository source/tests show hubs, links, and satellites also carry non-target technical roles.
- Keep the first slice on `EntityState.Added` rows only; widening into update behavior changes the semantics of this ticket.
- Do not broaden `IDataVaultLoadTimestampResolver` or `IDataVaultRecordSourceResolver` purely to fit interception; both interfaces are explicit-save-request based.
- Keep the default `AddDVault()` path interceptor-free so the explicit `IDataVaultSaveService` boundary stays the normal write path.

Non-blocking notes
- The incoming `parentOf` relation from story `06F1XPZAJBSSNN6HY1CHAQPH74` is present, but this task contract is specific enough to stand on its own for developer handoff.

Split recommendations
- No split recommended for the current ticket; the bounded Added-row interceptor slice is coherent as written.
- If work expands into `HashKey`/`HashDiff` population, non-Added behavior, or broader batch/correlation/tenant metadata, split that into follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment