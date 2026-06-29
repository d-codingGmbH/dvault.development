[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD returns only .gicket/tickets/06FGX5KZHC9ZAKAT71C89MEYV8/*, so the story branch carries ticket metadata only and no extra repository source/doc deltas beyond develop.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:89-105 keeps provider-native encryption unmanaged/guidance-only, fixes the finite provider baseline to SQLite/PostgreSQL/SQL Server/MySQL/Oracle/DB2, and forbids provider-native DDL, SQL crypto calls, capability probing, and runtime routing.
- src/DCoding.Data.DVault/DataVaultDiagnosticsResult.cs:41-43, src/DCoding.Data.DVault/DataVaultPrivacyDiagnostics.cs:4-26, and src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:<redacted> expose additive Privacy diagnostics in core and populate guidance-only with UsesDatabaseCapabilityProbing=false.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:460-688 verifies the advisory-vs-fail-closed split, alias coverage statuses, support-bundle diagnostics.privacy serialization, guidanceStatus == guidance-only, usesDatabaseCapabilityProbing == false, and that the exported JSON does not contain Data Source.
- examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs:13-31 and examples/DCoding.Data.DVault.SqliteQuickstart/SqliteQuickstartVaultContext.cs:16-25 show the checked-in SQLite proof using AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), and DataVaultEncryptedPayloadValueConverter.
- docs/getting-started.md:176-235, examples/README.md:90-96, README.md:46-48, docs/package-compatibility.md:34-36, docs/production-adoption-checklist.md:9-10, and docs/releases/v0.48.0.md:17-34 consistently describe the privacy seam as explicit opt-in, provider-neutral, fail-closed, and not a compliance/provider-native encryption feature.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No PO-level gap found. The remaining non-SQLite provider-native encryption cases are intentionally out of scope and already routed to separate future provider-specific tickets/contracts.

Risky assumptions
- The story relies on ticket relations as the source of truth for the four implementation slices; the delivery contract summary names the slices but does not list the four child ids inline.
- Because develop...HEAD differs only in .gicket metadata, developer handoff assumes no additional story-level repository changes are needed beyond the already-completed child tasks.

AC / test suggestions
- Keep regression coverage that personal-data-privacy-proof-missing stays advisory while configured-but-unusable privacy coverage remains fail-closed.
- Keep support-bundle assertions that diagnostics.privacy remains redaction-safe, includes guidance-only, and never reports live database capability probing.
- Keep quickstart/converter proof coverage for missing alias registration, missing provider wiring, marker-only providers, and declined conversion paths.

Implementation watchouts
- Do not reopen provider-native encryption, encrypted DDL, provider SQL crypto calls, or runtime capability probing inside this story; those remain separate provider-specific future work.
- Preserve the core-to-privacy dependency direction so diagnostics facts stay in core without depending on DCoding.Data.DVault.Privacy concrete types.
- Preserve fail-closed behavior for alias mismatches or unusable key-provider posture; no plaintext fallback and no silent ciphertext-as-plaintext behavior.

Non-blocking notes
- The story reads as a tracking/umbrella handoff over already-landed child outcomes; that is unusual but not a PO blocker because the contract is clear and ## Open Questions is already closed.

Split recommendations
- No further split is needed. Keep any future native-encryption implementation as one separate provider-specific ticket per exact capability.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment