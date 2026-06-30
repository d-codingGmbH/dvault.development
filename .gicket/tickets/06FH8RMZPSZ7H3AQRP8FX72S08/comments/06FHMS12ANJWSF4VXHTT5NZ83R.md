[gicket-bot] PO-critic review contract

Summary
- Contract is concrete, repo-backed, and has no unresolved PO questions; approve for developer handoff as a bounded documentation-alignment task.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06FH8RMZPSZ7H3AQRP8FX72S08 shows `## Open Questions` = `none`, `Recent comments` = `<none>`, and scope explicitly names `README.md`, `docs/getting-started.md`, `docs/production-adoption-checklist.md`, `docs/package-compatibility.md`, and the current release-note/changelog surface.
- Direct branch inspection confirmed HEAD `8227c80c971dd916c932e0e2fb147e75e840bf7f` on `ticket/06FH8RMZPSZ7H3AQRP8FX72S08-task-document-provider-native-crypto-capabilitie` (`git log -1 --oneline --decorate`).
- `src/DCoding.Data.DVault/DataVaultProviderCryptoCapabilityCatalog.cs` defines the finite reviewed provider capability matrix for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 with `conditional` or `unsupported` statuses.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` exposes `AddDVaultSqlServerAlwaysEncryptedSelection(...)`, giving the ticket an exact SQL Server opt-in API to document.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs` contains `AnalyzeMetadataModelReportsProviderNativeCryptoSelectionForReviewedCapability`, `AnalyzeMetadataModelFailsClosedForMissingProviderNativeCryptoPrerequisites`, and `AnalyzeMetadataModelFailsClosedForProviderNativeCryptoSelectionWithIncompatibleProfile`, which directly back the fail-closed diagnostics story.
- `src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs` and `docs/getting-started.md` already anchor the caller-owned alias-driven converter path and explicit fail-closed behavior.
- A targeted search across `README.md`, `docs/getting-started.md`, `docs/production-adoption-checklist.md`, `docs/package-compatibility.md`, `CHANGELOG.md`, and `docs/releases/v0.50.0.md` found the provider-baseline caveats already published, while the exact diagnostics surface names and `AddDVaultSqlServerAlwaysEncryptedSelection(...)` are not yet broadly surfaced there, so the remaining work is a bounded documentation-alignment pass rather than open-ended discovery.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Show one adopter-facing example of where `ProviderNativeEncryption`, `ProviderCryptoCapabilities`, and `ProviderNativeCryptoSelections` appear in diagnostics or support-bundle review.
- Show one negative example for `AddDVaultSqlServerAlwaysEncryptedSelection(...)` with missing caller-owned prerequisite proof names, because tests already show `provider-native-rejected-missing-prerequisite` fail-closed behavior.
- Show one incompatible-profile/provider example so docs do not imply SQL Server selection auto-routes on PostgreSQL or SQLite paths.

Risky assumptions
- The contract allows updating either the current release notes or the changelog; implementation should pick one current release surface and keep its wording aligned with the named docs.
- The docs must stay at the same abstraction level as the redaction-safe diagnostics tests and must not drift into key-store, provider-provisioning, deletion, or compliance promises.

AC / test suggestions
- Add a documentation review check that all named public surfaces use the same six-provider baseline and the same `guidance-only` and `fail-closed` ownership language.
- Verify the published example names the exact API `AddDVaultSqlServerAlwaysEncryptedSelection(...)` and the diagnostics surfaces `ProviderNativeEncryption`, `ProviderCryptoCapabilities`, and `ProviderNativeCryptoSelections`.
- Verify at least one user-facing doc tells adopters where to inspect redaction-safe diagnostics/support-bundle facts for provider capability and native-selection review.

Implementation watchouts
- Do not describe SQL Server Always Encrypted as active shared-runtime execution today; repo evidence shows a reviewed selection and diagnostics seam, not provider auto-routing.
- Do not blur provider-native selection with the existing caller-owned `DataVaultEncryptedPayloadValueConverter` path; the contract correctly keeps both lanes distinct.
- Do not widen the provider baseline to MariaDB or other providers that are not in `DataVaultProviderCryptoCapabilityCatalog.cs`.

Non-blocking notes
- Current public docs already carry much of the guidance-only boundary language, so this looks like an alignment and naming pass more than a discovery task.
- The ticket is already tightly anchored to concrete repo symbols and public doc paths, which reduces developer ambiguity without needing more PO refinement.

Split recommendations
- No split recommended; the scope is already bounded to one documentation-alignment task across named public docs and one current release surface.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment