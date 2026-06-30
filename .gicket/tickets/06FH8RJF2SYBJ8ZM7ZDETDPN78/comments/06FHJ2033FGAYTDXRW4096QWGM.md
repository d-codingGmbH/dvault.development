[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the delivery contract is bounded, Open Questions are closed, and direct ticket/repo evidence confirms the existing diagnostics surfaces and finite provider baseline that this additive capability-fact task will extend.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FH8RJF2SYBJ8ZM7ZDETDPN78/description.md contains PO Handoff decision 'ready_for_po_critic', Open Questions '- none', and explicit AC/DoD for deterministic static capability facts, MySQL dual-provider mapping, unknown-provider handling, redaction safety, and no live database probing.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs maps SQLite, PostgreSQL, SQL Server, Oracle, DB2, MySql.EntityFrameworkCore, and Pomelo.EntityFrameworkCore.MySql to built-in capability profiles; both MySQL provider names resolve to the same DataVaultProviderCapabilityProfiles.MySql entry.
- src/DCoding.Data.DVault/KnownProviderNames.cs and src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs define the checked-in finite provider baseline in code, including one shared MySQL profile ('mysql-pomelo-v1') plus SQLite, PostgreSQL, SQL Server, Oracle, and DB2.
- docs/getting-started.md, docs/package-compatibility.md, docs/architecture/dvault-v1-optional-privacy-extension-boundary.md, and docs/releases/v0.50.0.md all repeat the same bounded provider-native crypto posture: SQLite/PostgreSQL/SQL Server/MySQL/Oracle/DB2 are guidance-only and outside shared runtime behavior.
- src/DCoding.Data.DVault/DataVaultPrivacyDiagnostics.cs, DataVaultProviderNativeEncryptionBoundaryFact.cs, DefaultDataVaultDiagnosticsService.cs, and DataVaultSupportBundleExporter.cs already provide the intended additive delivery lane: redaction-safe diagnostics/support-bundle JSON with boundaryStatus 'unmanaged', guidanceStatus 'guidance-only', and usesDatabaseCapabilityProbing false.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs already asserts privacy facts serialize through support-bundle diagnostics and do not leak connection-string text, so this ticket extends an existing checked-in test surface.
- git diff --name-only develop...HEAD lists only .gicket/tickets/06FH8RJF2SYBJ8ZM7ZDETDPN78/**/*, and git show --stat 0d74e06aedfb71faa9e2ac8500515dfb3c06f2b3 shows the current head is the po-critic lease-claim commit, so the branch is still pre-development ticket-state only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not show a concrete example row for a provider that has both deployment/at-rest guidance and SQL-function or driver-mediated guidance (for example SQL Server or PostgreSQL), so tests should lock that distinction down explicitly.
- The contract does not illustrate how emitted facts should present the shared MySQL capability set when the runtime EF provider is MySql.EntityFrameworkCore rather than Pomelo.EntityFrameworkCore.MySql.

Risky assumptions
- Implementation can bypass or compensate for the existing SQLite fallback in DataVaultProviderCapabilityProfileSelection.Select(...) so unknown or unregistered providers do not inherit reviewed crypto facts.
- Using the existing shared MySQL profile name 'mysql-pomelo-v1' for both MySQL provider names will not confuse consumers if the emitted facts also make the actual provider or shared-profile semantics clear.
- The downstream docs ticket 06FH8RMZPSZ7H3AQRP8FX72S08 will publish the same reviewed matrix so diagnostics and documentation do not drift.

AC / test suggestions
- Add per-provider assertions for SQLite, PostgreSQL, SQL Server, Oracle, DB2, and both MySQL EF provider names, not just shared-profile coverage.
- Add an unknown or unregistered provider test that proves diagnostics keep the unmanaged/guidance-only boundary without inheriting SQLite crypto rows.
- Add support-bundle JSON assertions that capability-family typing distinguishes deployment or at-rest guidance from SQL-function or driver-mediated guidance.
- If the implementation adds a new public diagnostics record or collection, update the public API snapshot and checked-in support-bundle baseline in the same change.

Implementation watchouts
- Keep DataVaultProviderNativeEncryptionBoundaryFact intact as the shared unmanaged guidance-only boundary; the new capability rows should be additive, not a replacement.
- Do not open database connections, probe provider encryption capabilities, or route runtime behavior from these facts.
- Keep emitted facts redaction-safe: no plaintext, ciphertext, SQL text, keys, provider secrets, connection strings, or live probe outputs.
- Be careful with unknown providers because current capability-profile selection already falls back to SQLite for some explain paths.

Non-blocking notes
- Recent ticket comments are orchestration and PO-refinement only; there is no separate human comment thread altering scope after refinement.
- The current branch contains only ticket metadata changes, which is normal for this pre-development PO gate.

Split recommendations
- Keep provider-specific execution or runtime crypto behavior out of this task; that belongs in later per-provider follow-on tickets.
- Keep consumer-facing selection or configuration API work in ticket 06FH8RKDJTS3BB11J6J6QJVVD4.
- Keep matrix publication and broader documentation rollout in ticket 06FH8RMZPSZ7H3AQRP8FX72S08.
- If opt-in runtime probing is ever wanted, split it into a separate diagnostics ticket with its own redaction and secret-handling review.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment