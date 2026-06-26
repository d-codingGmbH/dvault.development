[gicket-bot] PO-critic review contract

Summary
- Persisted contract is clear, repository-backed, and has no open questions; the ticket is ready for developer handoff with only normal watchouts about keeping the privacy proof opt-in and inside the existing SQLite quickstart surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket-comments` returned 10 comments, all bot orchestration/refinement history; no newer human comment or closure amendment reopens scope or raises unresolved PO questions.
- `examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs` already wires `AddDVault(options => options.UseBinaryFirstProfile().UseMetadataModel(QuickstartHistoryFlow.MetadataModel))`, `AddDVaultSqlite()`, and `UseDataVaultMetadata()`, which matches the existing SQLite binary-first metadata-first baseline named in the ticket.
- `examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs` directly shows the current bounded runnable flow: one `Customer` hub, one `CustomerProfile` satellite, explicit `DataVaultRegistrySaveRequest` writes, latest/as-of reads, and sanitized diagnostics output, so there is an existing quickstart surface to extend rather than a need for a new example family.
- `docs/getting-started.md` already contains an `## Optional Privacy Proof` section with `personalData[].encryptedPayloadAlias`, `AddDVaultPrivacy(...)`, `RegisterEncryptedPayloadAlias(...)`, `UseCallerOwnedKeyProvider(...)`, `IDataVaultEncryptedPayloadKeyProvider`, and `DataVaultEncryptedPayloadValueConverter`, plus explicit fail-closed, no-compliance, and no-provider-native-encryption caveats.
- Direct source inspection of `src/DCoding.Data.DVault.Privacy/DVaultPrivacyServiceCollectionExtensions.cs`, `DataVaultPrivacyOptions.cs`, `IDataVaultEncryptedPayloadKeyProvider.cs`, and `DataVaultEncryptedPayloadValueConverter.cs` confirms the named public privacy seams exist and that conversion fails closed for unregistered aliases, missing providers, wrong provider type, or declined conversions.
- `git show --stat --oneline HEAD` at `b57d58de9` shows only PO-critic lease-claim and `.gicket/...` ticket-metadata changes; there is no implementation work on the branch yet, which is acceptable for this pre-development review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No PO blocker: the contract intentionally keeps this ticket to one happy-path SQLite proof and leaves broader declined-conversion or provider-matrix examples out of scope.

Risky assumptions
- The combined sample can add a small ordinary EF-mapped privacy row beside the existing registry-backed DVault quickstart without readers inferring that DVault metadata or `IDataVaultSaveService` performs automatic encryption.

AC / test suggestions
- Show one visible round trip where the SQLite provider value is caller-converted for storage and the mapped property materializes back through `DataVaultEncryptedPayloadValueConverter`.
- Keep one verification path for fail-closed behavior when alias registration or encrypted-payload provider wiring is missing, but keep that as test/docs evidence rather than widening the quickstart into a negative-case matrix.

Implementation watchouts
- Do not create a new `SqlitePrivacyQuickstart` project; the contract and current repo surfaces point to `examples/DCoding.Data.DVault.SqliteQuickstart` plus small shared/example additions only.
- Keep privacy conversion on an ordinary EF-mapped property and out of `IDataVaultSaveService`, `IDataVaultReadService`, and automatic `SaveChanges` behavior.
- Preserve the ticket's explicit `AddDVault(options => options.UseBinaryFirstProfile().UseMetadataModel(...))` setup even though `AddDVaultPrivacy()` currently layers `AddDVault()` underneath; the sample should not imply that privacy registration alone selects the binary-first metadata model.
- Keep the demo provider visibly caller-owned and non-production; the current docs and converter source both make DVault fail closed instead of owning cryptography or key lifecycle.

Non-blocking notes
- The latest ticket snapshot is consistent with PO-critic gate posture: `todo`, `automation/bot-ready`, `critic-needed`, `provider/sqlite`, and no assignee.
- `examples/README.md` already points readers to the optional privacy proof, and the broader README/release-doc linking cleanup is already deferred in follow-up ticket `06FF43WMMC8R3T4ZKVR4312NJC`.

Split recommendations
- No split recommended; the repository already has one bounded SQLite quickstart surface and one bounded privacy-proof surface, and the persisted contract limits this ticket to bridging them in a single slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment