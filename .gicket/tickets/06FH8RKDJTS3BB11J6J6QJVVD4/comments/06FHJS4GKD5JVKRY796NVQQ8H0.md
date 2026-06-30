[gicket-bot] PO-critic review contract

Summary
- Approve for dev. The persisted delivery contract is specific enough for implementation handoff: it has no open questions, fixes the shared/custom default lane, keeps native selection provider-owned, and ties fail-closed behavior to already checked-in repo seams and capability facts.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted contract in `.gicket/tickets/06FH8RKDJTS3BB11J6J6QJVVD4/description.md:31-37` defines six acceptance criteria, and `:53-54` shows `## Open Questions` -> `- none`.
- Branch evidence: `git log --oneline --decorate -n 3` at `/mnt/c/Projects/DVault` shows HEAD `1de33483b06fa6e2fb62bb693302614634ee111c` on `ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or`; `git diff --name-only 7225287db..HEAD` lists only `.gicket/tickets/06FH8RKDJTS3BB11J6J6QJVVD4/...` files.
- The existing custom lane is directly present in source: `src/DCoding.Data.DVault.Privacy/DVaultPrivacyServiceCollectionExtensions.cs:15-38` exposes `AddDVaultPrivacy(...)`, `src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:28-50` registers encrypted-payload aliases plus `UseCallerOwnedKeyProvider(...)`, and `src/DCoding.Data.DVault.Privacy/IDataVaultEncryptedPayloadKeyProvider.cs:10-17` defines the caller-owned conversion seam.
- Fail-closed behavior is already anchored in repo code: `src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs:9-10,41-60,75-96` throws for unregistered aliases, missing providers, non-encrypted-payload providers, null results, and declined conversions; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs:51-106` covers those failure cases.
- Static provider capability facts already exist in `src/DCoding.Data.DVault/DataVaultProviderCryptoCapabilityCatalog.cs:11-116` for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2, while `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:51-53,73,91-105` forbids shared provider-name branching, implicit native selection, live probing, and shared provider-native runtime behavior.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not show a concrete sample of the future provider-package native opt-in call beside `AddDVaultPrivacy(...)`; the first implementation should keep that example narrow to one provider and one exact capability.
- An explicit example for a partially configured native request is still absent, for example a capability selection present while caller-owned prerequisites are missing; dev should cover that fail-closed path in contract tests.

Risky assumptions
- This approval assumes the shared ticket will not introduce a generic cross-provider native-selector API in `DCoding.Data.DVault` or `DCoding.Data.DVault.Privacy`, because the repo boundary documents explicitly push native selection into provider-owned seams.
- This approval assumes existing alias-driven diagnostics and capability-fact surfaces are sufficient inputs for native-selection failures without adding new provider-specific metadata to the shared model.

AC / test suggestions
- Add a contract test that an explicitly requested native capability never downgrades silently to plaintext persistence or to the existing custom lane.
- Add a package-boundary test that shared packages do not branch on provider identity and treat `DataVaultProviderCryptoCapabilityCatalog` as diagnostics input only.

Implementation watchouts
- Do not widen `DataVaultPrivacyOptions` into a generic provider-strategy selector; keep alias registration and caller-owned key-provider wiring as the shared default lane.
- Keep any native opt-in extension method in the matching provider package and bind it to one exact reviewed capability with redacted fail-closed diagnostics.

Non-blocking notes
- The current owner branch contains only ticket/comment/event changes under `.gicket/...`; no product code changed after PO handoff, which is consistent with a pre-development PO gate and not a blocker.
- The follow-up questions in the delivery contract are recorded under `## Follow-Up Questions`, not `## Open Questions`, so they do not invalidate dev handoff.

Split recommendations
- Keep provider-native execution proof and fallback testing in `06FH8RMFZSVNW0KKTZT9HMGM8G` and limit that follow-on to one provider/capability slice.
- Keep any consumer-facing runtime support matrix or preflight-diagnostics expansion in separate later tickets, as the current contract already recommends.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment