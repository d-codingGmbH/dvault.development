[gicket-bot] PO-critic review contract

Summary
- Ticket 06FF43PCN26C70DXX326B9VYA4 is ready for developer handoff: the delivery contract is specific, Open Questions is none, and the repository already provides direct source evidence for the privacy seam, finite provider baseline, fail-closed behavior, and provider-native-encryption non-goals.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF43PCN26C70DXX326B9VYA4/description.md contains the current delivery contract, names docs/architecture/dvault-v1-optional-privacy-extension-boundary.md as the canonical source, lists the expected consumer-facing doc surfaces, and has '## Open Questions' -> 'none'.
- .gicket/tickets/06FF43PCN26C70DXX326B9VYA4/comments/06FFVWXBA135243084NN2ATWP4.md records the PO refinement handoff as ready_for_po_critic and already captures the bounded scope, risks, and no-split posture.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md directly states that the shared privacy lane is alias-driven encrypted payload conversion, fixes the finite provider baseline to SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2, lists provider-native features such as SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL/MariaDB SQL crypto and file encryption, SQLite encrypted-file builds, and DB2 native database encryption as out of scope, and says any future provider-native lane requires a separate provider-specific ticket.
- docs/getting-started.md says the privacy package uses ordinary EF Core value conversion, documents fail-closed behavior when alias registration or conversion approval is missing, and keeps provider caveats bounded to ordinary EF Core mapping; src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs and src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs directly confirm the IDataVaultPrivacyKeyProvider versus IDataVaultEncryptedPayloadKeyProvider boundary and throw on missing alias/provider/approval.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs maps both MySql.EntityFrameworkCore and Pomelo.EntityFrameworkCore.MySql to the same MySQL capability profile, matching the contract's MySQL precision requirement.
- Git history on branch ticket/06FF43PCN26C70DXX326B9VYA4-task-document-provider-native-encryption-caveats shows only ticket workflow commits 53860bc9e2, 0f31b077f0, and 390de78f32 for this ticket, and git diff --name-only 53860bc9e2 0f31b077f0 returns only .gicket/tickets/06FF43PCN26C70DXX326B9VYA4 files, so the branch is still at pre-development handoff state rather than carrying hidden implementation work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Keep the docs explicit about the three-way distinction between DVault field-level encrypted payload conversion, provider-native column/cell/row features, and database-at-rest encryption.
- Keep MySQL wording precise: the repository maps MySql.EntityFrameworkCore and Pomelo.EntityFrameworkCore.MySql to one MySQL profile and does not establish a separate MariaDB capability profile.

Risky assumptions
- The implementation notes mention 'current release guidance'; development should treat docs/releases/v0.44.0.md as the authoritative source for the caveat wording, but align any consumer-facing update with the current documentation baseline rather than rewriting history inconsistently.

AC / test suggestions
- Review every touched consumer-facing doc named in the contract and confirm each either reuses or links to the canonical wording from docs/architecture/dvault-v1-optional-privacy-extension-boundary.md.
- Check that no updated doc implies provider-native encryption DDL, SQL crypto calls, runtime capability probing, or automatic provider dispatch.
- Check that no updated doc broadens support beyond SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 or implies a separate MariaDB capability profile.

Implementation watchouts
- This is a documentation-alignment handoff, not an approval to change runtime behavior; the branch history currently shows only ticket workflow commits.
- The docs should preserve the concrete type boundary where UseCallerOwnedKeyProvider accepts IDataVaultPrivacyKeyProvider but actual encrypted payload conversion requires IDataVaultEncryptedPayloadKeyProvider.

Non-blocking notes
- The main remaining work is alignment across public docs: README.md, docs/package-compatibility.md, and docs/production-adoption-checklist.md already say the privacy package is optional and not provider-native encryption, while the full provider-specific caveat list currently lives in docs/architecture/dvault-v1-optional-privacy-extension-boundary.md and docs/releases/v0.44.0.md.

Split recommendations
- No split recommended; the contract is already bounded to one documentation-alignment task and the durable ticket description already says no split recommended.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment