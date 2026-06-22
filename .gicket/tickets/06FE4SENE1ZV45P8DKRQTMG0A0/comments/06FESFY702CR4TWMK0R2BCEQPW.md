[gicket-bot] PO-critic review contract

Summary
- Ticket contract is clear, repo-aligned, and has no unresolved open questions; approve for developer handoff with scope-drift watchouts.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4SENE1ZV45P8DKRQTMG0A0/description.md contains `## Open Questions` with `- none` and acceptance criteria that explicitly limit v0.44 to caller-invoked provider-neutral encrypted payload mapping, not provider-native cell/column/row encryption.
- .gicket/releases/06FE4PMT7V0NN9Y6RKD5WDTY58.json names `v0.44.0 - Optional Privacy Extension Foundation`, and .gicket/milestones/06FE4PN9EX787F4DHNJVN86YK0.json names `v0.44.0 - Privacy extension contract and compliance boundary`, matching the ticket's scoped planning baseline.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md states the privacy boundary is explicit, opt-in, provider-neutral in shared core, and that provider-specific behavior must sit behind `AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, and `AddDVaultDb2()` seams.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs and src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs define the finite built-in provider baseline as SQLite, PostgreSQL, SQL Server, Oracle, DB2, and MySQL (`MySql.EntityFrameworkCore` and `Pomelo.EntityFrameworkCore.MySql`), with no separate MariaDB capability profile.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs and src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs already provide the cited provider-neutral mapping precedent via provider storage annotations, `ValueConverter<string, byte[]>`, and canonical hex<->bytes conversion.
- .gicket/tickets/06FE4SENE1ZV45P8DKRQTMG0A0/events/06FE4SFHVSXWQQDQ593KNDDQEM.json, 06FE4SFM47MKC800SH5GQSARVR.json, 06FE4SFPEYQ87GNP86QZ8B3QX0.json, and 06FE4SFRVWM3ZSREHCQMGED95W.json record `blocks` relations from this ticket to 06FE4RA88AV7ZRRPMDS8YADEX4, 06FE4RASEQZN7XEYH1XR4H06PR, 06FE4RB219AXVF2535MFF36PN4, and 06FE4RBK2MJBS5K3C15JTB8Z9W; those child tickets exist and are currently `todo`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocker, but downstream docs/examples should show one explicit caller-owned encrypted-payload flow and one explicit non-example such as TDE or Always Encrypted so adopters do not infer native-provider runtime support.
- No blocker, but downstream guidance should call out that MySQL baseline evidence does not create a MariaDB-specific guarantee.

Risky assumptions
- Downstream implementers will treat `mysql-pomelo-v1` and `MySql.EntityFrameworkCore` support as MySQL-only evidence and will not market MariaDB as equivalently supported without a separate ticket.
- Future privacy work will keep key material caller-owned and explicit instead of adding ambient key lookup, hidden interception, or automatic database-feature negotiation.
- Provider-specific encryption features will remain guidance-only until a dedicated provider ticket lands with package ownership, diagnostics, fallback behavior, tests, and evidence.

AC / test suggestions
- Add a downstream acceptance/test check that shared-core privacy work does not branch on provider-native encryption capability and stays behind explicit provider-neutral contracts.
- Add one representative proof that encrypted payload mapping uses caller-supplied key material and explicit save/read flow activation, not `SaveChanges` interception.
- Add docs/tests that explicitly classify SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, TDE, SQLite encrypted-file variants, and DB2 native encryption as guidance/non-goals for v0.44.

Implementation watchouts
- Do not infer native encryption behavior from EF provider name inside `DCoding.Data.DVault`; keep shared-core behavior provider-neutral and explicit.
- Do not turn this ticket into provider-specific DDL, SQL function generation, migration automation, deployment automation, or key-management ownership.
- Preserve explicit diagnostics and fallback behavior when a provider package cannot support a requested privacy shape.

Non-blocking notes
- The current branch history is workflow/ticket metadata only; that is acceptable for a pre-development PO gate and is not a reason to return the ticket to PO.
- The `## Follow-Up Questions` section contains strategic backlog ideas, but `## Open Questions` is explicitly `none`, so developer handoff is not blocked.

Split recommendations
- No additional split is needed now; the existing blocks split to design, proof, tests, and documentation tickets is already in place.
- If DVault later approves a native provider lane, keep it to one provider and one exact capability per ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment