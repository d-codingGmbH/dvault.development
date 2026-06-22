[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the delivery contract is specific, has no open questions, and matches repository-proven privacy seams and architecture boundaries.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FE4RASEQZN7XEYH1XR4H06PR/description.md:48-49` records `## Open Questions` as `none`, so the persisted contract has no unresolved open-question gate.
- `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06FE4RASEQZN7XEYH1XR4H06PR/...` files, so this branch is still ticket-metadata-only pre-development work rather than mixed product-code changes.
- `src/DCoding.Data.DVault.Privacy/DVaultPrivacyServiceCollectionExtensions.cs:26-36` layers `AddDVaultPrivacy(...)` on `AddDVault()`, and `src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:27-49` exposes the existing manual alias-registration and caller-owned key-provider seam the ticket references.
- `src/DCoding.Data.DVault.Privacy/IDataVaultPrivacyKeyProvider.cs:3-8` states the current key-provider contract is marker-only and does not yet define encryption/decryption or key lifecycle behavior, matching the ticket's implementation-note baseline.
- `rg -n ValueConverter|encrypted payload|encrypt|decrypt src/DCoding.Data.DVault.Privacy tests/DCoding.Data.DVault.Tests` found no privacy-package `ValueConverter` or encrypt/decrypt implementation beyond alias/configuration scaffolding, so the requested proof appears genuinely unimplemented.
- `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:65-75` requires alias-driven caller-owned key-provider resolution with fail-closed behavior, and `:89-101` approves an explicit provider-neutral helper/value-conversion proof while excluding provider-native encryption.
- `README.md:46-47`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs:10-37`, `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.Privacy.approved.txt:7-22`, and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:519-558` show the current public baseline is an optional privacy skeleton with registration tests, API snapshot coverage, and an enforced no-core/provider-reference boundary.
- Related architecture story `06FE4R9PP99G6Q1PTPK4TKD460` is `done` in `.gicket/tickets/06FE4R9PP99G6Q1PTPK4TKD460/ticket.json:3-20`, and `git log --grep='06FE4R9PP99G6Q1PTPK4TKD460' -n 5` shows `5714535d8e` auto-integrated it into `develop`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract leaves the exact representative payload/property/model choice to the implementer; that is acceptable, but the chosen proof shape should stay obviously narrow and reuse existing translator/schema test patterns.
- Ciphertext-corruption or unreadable provider-value behavior is not named as a concrete example, even though the fail-closed requirement implies it should not silently round-trip or downgrade.

Risky assumptions
- A narrow encrypt/decrypt request surface can be introduced around the marker-only `IDataVaultPrivacyKeyProvider` without reopening PO scope.
- One representative alias-mapped payload proof is sufficient to demonstrate provider-neutral viability before any `personalData` metadata ingestion work.
- SQLite-backed proof coverage will be accepted as the shared provider-neutral baseline without requiring provider-specific validation in this ticket.

AC / test suggestions
- Add an explicit round-trip test that proves the stored provider value differs from plaintext while the read path returns the original payload for one alias-registered property.
- Add fail-closed tests for missing alias registration, unavailable or declined key material, and unreadable or tampered encrypted provider values.
- Add a boundary test proving ordinary `AddDVault()` and default save/read flows remain unchanged when the privacy package is not referenced.

Implementation watchouts
- Keep all new runtime surface in `src/DCoding.Data.DVault.Privacy`; `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:519-558` currently enforce that the privacy project depends only on core plus DI abstractions and that core/provider projects do not reference the privacy package.
- Do not add provider-name branching, provider-native encryption fallback, automatic `SaveChanges` hooks, or implicit behavior on ordinary `AddDVault()`; the architecture contract explicitly excludes those lanes.
- If new public types are introduced, update the privacy API snapshot in `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.Privacy.approved.txt` and keep package-facing text aligned with the optional-proof and non-compliance messaging.

Non-blocking notes
- The branch history is workflow-only so far: `git log --oneline -n 12` ends with PO handoff/residual/lease-claim commits (`<redacted>`, `b659baa452`, `9ff71a71a9`) and no product-code commits for this ticket yet.

Split recommendations
- No split is needed while implementation stays limited to one manual-alias, one-payload, provider-neutral proof plus bounded docs/tests.
- Split immediately if work expands into `personalData` metadata projection, broader diagnostics, read/write privacy workflow helpers, or provider-specific encryption/optimization lanes.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment