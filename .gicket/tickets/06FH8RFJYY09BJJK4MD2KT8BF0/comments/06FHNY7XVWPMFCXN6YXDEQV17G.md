[gicket-bot] PO-critic review contract

Summary
- Delivery contract is bounded and ready for developer handoff: no open questions remain, the repository already has matching privacy/provider-capability seams, and the branch is a clean pre-development ticket handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git log --oneline --decorate -n 5 shows 314c3a502 as the po->po-critic handoff commit and HEAD b4d3a7670 as the current po-critic claim; git status -sb returned the ticket branch tracking origin cleanly, and git diff --name-only 95f9f4b65...HEAD listed only .gicket/tickets/06FH8RFJYY09BJJK4MD2KT8BF0/** files.
- .gicket/tickets/06FH8RFJYY09BJJK4MD2KT8BF0/description.md:32-55 defines the acceptance criteria and shows ## Open Questions = none, which satisfies the approve-for-dev gate.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:8,10,21,23-24 already establishes the same boundary the ticket relies on: opt-in privacy extension, caller-owned key-provider seam, redacted diagnostics, and provider-specific behavior behind provider packages rather than shared-core runtime branching.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs:15-184 and src/DCoding.Data.DVault/DataVaultProviderCryptoCapabilityCatalog.cs:11-117 already define the finite built-in capability profiles and reviewed provider-native crypto matrix for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- src/DCoding.Data.DVault/DataVaultPrivacyDiagnostics.cs:7,14,20 and src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:<redacted>,<redacted>,1963 already name the requested diagnostics surfaces (ProviderNativeEncryption, ProviderCryptoCapabilities, ProviderNativeCryptoSelections) and the fail-closed validation code provider-native-crypto-selection-unavailable.
- src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:39-69 already exposes AddDVaultSqlServerAlwaysEncryptedSelection(...), including duplicate-alias rejection; tests at tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs:56-104 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:744-974 cover the matrix, success path, fail-closed negative paths, and redacted support-bundle behavior referenced by the story.
- README.md:48-50, docs/getting-started.md:235-251, docs/package-compatibility.md:44-51, and docs/releases/v0.50.0.md:46-47 already use the same diagnostics-only / no-managed-provider-native-encryption wording the contract expects developers to preserve.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Duplicate AddDVaultSqlServerAlwaysEncryptedSelection(...) registration is in Definition of Done (.gicket/tickets/06FH8RFJYY09BJJK4MD2KT8BF0/description.md:42) and repo tests, but it is not called out as its own Acceptance Criterion.
- The contract does not name the capability-profile-defaulted / unknown-provider lane explicitly, although the existing repo tests already exercise the unavailable reviewed-capability path.

Risky assumptions
- Developers and reviewers will treat the delivery contract and scope-out sections as authoritative and not over-read the broader story title into multi-provider runtime encryption work.
- Any later provider capability-matrix edits will be kept synchronized across the static catalog, docs, and tests so the finite reviewed baseline does not drift.

AC / test suggestions
- If product wants tighter UAT wording, add an explicit Acceptance Criterion for duplicate-alias rejection on AddDVaultSqlServerAlwaysEncryptedSelection(...).
- Keep dedicated negative coverage for both reviewed-capability-unavailable/defaulted and reviewed-capability-unsupported paths, in addition to the success and incompatible-profile lanes already described.

Implementation watchouts
- Do not broaden this into provider-native encrypted DDL, provider SQL crypto calls, live capability probing, or shared runtime dispatch; the architecture contract explicitly keeps those out of scope.
- Keep the provider-owned selection surface in the SQL Server package and the shared reporting surface in the existing privacy diagnostics pipeline; avoid moving provider-specific behavior into the shared AddDVault() default path.
- When provider-native capability facts change, update the static catalog, docs, and tests together so the reviewed matrix stays consistent.

Non-blocking notes
- This branch is still a ticket-only pre-development handoff. The absence of product-code changes on the branch is not a PO blocker for this review.
- The repository already contains the target terminology across README/getting-started/package-compatibility/release-note surfaces, which should reduce implementation ambiguity for the developer handoff.

Split recommendations
- No mandatory split before dev; the current contract is already bounded to diagnostics guidance plus one SQL Server Always Encrypted selection surface.
- If product later wants additional provider-native selections or managed runtime behavior, open separate provider-owned tickets per capability family instead of broadening this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment