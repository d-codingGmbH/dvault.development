[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/description.md:32-36` defines the parent acceptance criteria around the three child tickets, current source/tests/public API snapshot, zero-config `AddDVault()` defaults, and a diff limited to `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R` metadata; `:51-52` shows `## Open Questions` = `none`.
- `git diff --name-only develop...ticket/06EZ0NWKC9ZME5BSCJFSQEQ02R-story-expose-advanced-configuration-hooks-needed` returned only files under `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/`; the scoped check `git diff --name-only ... -- 'src/**' 'tests/**' 'docs/**'` returned no paths.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-47` exposes both `AddDVault()` and `AddDVault(Action<DataVaultOptions>)`; `src/DCoding.Data.DVault/DataVaultOptions.cs:18-80` exposes `UseLoadTimestampResolver`, `UseRecordSourceResolver`, and `UseProviderBehavior` on the advanced configuration surface.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:41-91` keeps `loadTimestamp` and `recordSource` on `DataVaultSaveRequest`; `:397-505` resolves them once per request and fails on ambiguous, null, non-UTC, or empty hook outputs before provider strategy execution.
- `src/DCoding.Data.DVault/DefaultDataVaultProviderBehaviorSelector.cs:29-45` selects the first applicable explicit provider behavior and otherwise falls back to `DataVaultProviderBehaviorProfiles.ProviderNeutral`.
- `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:14-166` covers the zero-config explicit save-service boundary, optional resolver configuration, per-request resolution, and invalid hook outputs; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderBehaviorTests.cs:15-117` covers provider-neutral default behavior, explicit override selection, and provider-package registrations.
- `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:8-9,59-66,176-198` snapshots the public `AddDVault` overloads, `DataVaultOptions` methods, resolver/provider-behavior interfaces, and `IDataVaultSaveService` boundary the parent contract cites.
- A bounded scan of `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/comments/*.md` found `total_comments=42` and `non_bot_count=0`; this matches the contract's qualitative 'bot-authored comment history' wording and confirms the legacy `Recent comments: <none>` text is stale.
- `rg` over `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R` found relation event files `06EZ0NYEF4C1MWW19RH44BS208.json`, `06EZ0NYGF7B9JRTSVS6GMEJ8PR.json`, `06EZ0NYJFF4V8B7VM9C5NFQ5MG.json`, `06EZ0NZ9PHM07FC96RPA1N7GYC.json`, `06EZ0NZBW7RKPA3V85HNB7RZR0.json`, and `06EZ0NZE3K3FZJFRYMHPCPRSNR.json`, matching the parent's existing `parentOf` and `blocks` relations called out in the contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Approval assumes reviewers continue to treat current source/tests/public API snapshot as authoritative, because `docs/plans/optional-advanced-configuration-hooks.md:59-61` still says provider behavior is not an implemented public API even though current source and the API snapshot show `UseProviderBehavior(...)`.
- Approval assumes the live outgoing `blocks` relations called out in `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/description.md:55-60` are a closure-cleanup concern rather than a pre-dev blocker for this ratification-only parent.

AC / test suggestions
- If closure automation is tightened later, add a parent acceptance criterion that formal closure either removes or explicitly preserves the outgoing `blocks` relations to `06EZ0NSXY2Y1JZ8SSCX177C770`, `06EZ0NTV4SVAKV98C418T8A3CC`, and `06EZ0NVN71BN0QWJDCWGVZ2PYG`.
- If this umbrella is reused again, add an acceptance criterion that parent-level proof must cite current source/tests/public API snapshot whenever architecture/planning prose lags implemented advanced-hook APIs.

Implementation watchouts
- Treat dev handoff on this parent as ratification/closure only; the current `develop...ticket/06EZ0NWKC9ZME5BSCJFSQEQ02R-story-expose-advanced-configuration-hooks-needed` diff contains no product-code, test, or docs changes to implement on the parent branch.
- Do not use `docs/plans/optional-advanced-configuration-hooks.md` as the API source of truth for provider behavior on this umbrella; current source and `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` are newer and directly verifiable.
- If closure work later cleans relations, avoid reopening the three done child tickets just to restate already-evidenced hook behavior.

Non-blocking notes
- `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/ticket.json:7-16` is still `todo` with `critic-needed`, `blocked/dev`, and `blocked/test`; that is consistent with a pending critic decision rather than a missing refinement artifact.
- The PO refinement comment `06EZPFFJ4P5WFG0X6FCKDYRJ28.md` cited 34 persisted comments, but the current bounded scan already sees 42; the durable contract correctly switched to qualitative comment-history wording to avoid this drift.

Split recommendations
- Existing split remains sufficient: `06EZ0NWTM3EPBJS0SWVHXGDGTM` for timestamp/record-source hooks, `06EZ0NX282R80VF5VBKS6ARFZC` for provider behavior, and `06EZ0NX9SVP7MSB1R4PJ50EHGW` for validation/failure-mode documentation.
- No further split is warranted for this parent umbrella unless future naming or hashing customization becomes new implementation scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment