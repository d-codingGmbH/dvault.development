[gicket-bot] PO-critic review contract

Summary
- Return to PO: the epic currently points only to already-completed child work and is not a clean developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket shows 06EXB7FF1J9NR2849WKDR8DKPG is done and already reframed as an umbrella/tracking story whose repo evidence is src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs.
- gicket-read-ticket shows 06EXB7G6YE4X0GA0CT7EPEFMPR is done and already tied to the SQLite schema baseline under src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests.
- gicket-read-ticket shows 06EXB7GYQKBZ8FMQN6YDYCKATG is done and already owns the explicit IDataVaultSaveService write-pipeline slice.
- gicket-read-ticket shows 06EXB7HYG17X73GH0K535GYJH8 is done and already owns the provider-readiness / Postgres opt-in slice.
- repository-read-text of src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs shows AddDVault() already registers IDataVaultSaveService, IStableHashService, and IStableHashNormalizer on the default path.
- repository-read-text of tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs directly covers SQLite hub/link persistence, cross-context hub/link reuse, and satellite change-only insertion/history behavior.
- repository-read-text of README.md documents Postgres as opt-in via DVAULT_TEST_POSTGRES_CONNECTION_STRING and explicitly says normal dotnet test does not require Postgres, Docker, or checked-in machine-specific configuration.

Blocking findings
- The parent epic does not expose a live developer-owned slice anymore. Its own contract says the bounded delivery path is the four listed child tickets, and direct ticket reads show all four are already done.
- Approving this ticket would hand a tracking/orchestration epic to dev even though the repo already contains the referenced EF surface, explicit save surface, SQLite tests, and Postgres opt-in contract. That is a workflow/status problem, not an implementation-handoff problem.

Required PO actions
- Reframe 06EXB7F6WNWSJJV14EXTPSFDRG explicitly as a tracking/closure item or move it onto the appropriate completion path instead of sending it to dev.
- If any implementation is still intended, identify the specific still-open child ticket or create one; do not hand the current parent epic to dev while the listed child delivery path is already complete.
- Align ticket-level workflow state with that decision by updating the parent epic status/labels/comment handoff so automation does not dispatch a completed umbrella back to development.

Open issues ledger
- critic-item-1 [required-po-action] Reframe 06EXB7F6WNWSJJV14EXTPSFDRG explicitly as a tracking/closure item or move it onto the appropriate completion path instead of sending it to dev.
- critic-item-2 [required-po-action] If any implementation is still intended, identify the specific still-open child ticket or create one; do not hand the current parent epic to dev while the listed child delivery path is already complete.
- critic-item-3 [required-po-action] Align ticket-level workflow state with that decision by updating the parent epic status/labels/comment handoff so automation does not dispatch a completed umbrella back to development.
- critic-item-4 [blocking-finding] The parent epic does not expose a live developer-owned slice anymore. Its own contract says the bounded delivery path is the four listed child tickets, and direct ticket reads show all four are already done.
- critic-item-5 [blocking-finding] Approving this ticket would hand a tracking/orchestration epic to dev even though the repo already contains the referenced EF surface, explicit save surface, SQLite tests, and Postgres opt-in contract. That is a workflow/status problem, not an implementation-handoff problem.

Missing examples / edge cases
- If the epic is kept open for additional validation, clarify whether link-attached satellite persistence needs an explicit representative example; ExplicitDataVaultSaveServiceSqliteTests.cs persists a hub-attached Contact satellite, while CreateMetadataModel() also defines a link-attached State satellite that is not exercised in the observed save scenarios.

Risky assumptions
- none

AC / test suggestions
- Add an explicit parent-epic acceptance line that the epic becomes non-executable / ready for closure once 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 are all done and the cited repo evidence still matches.
- If PO wants extra representative coverage called out at epic level, state whether link-parent satellites are required alongside the existing hub-satellite history example in tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs.

Implementation watchouts
- Do not reopen Postgres parity, SaveChanges interception, migration tooling, PIT/bridge/multi-active scope, or other deferred capabilities while resolving this parent ticket.
- Do not send developers back to the parent epic for work that is already bounded to the done child tickets; surface any truly remaining work as a separate still-open child.

Non-blocking notes
- Open Questions is already none, so the review block is not unresolved question debt.
- The technical intent itself is well-supported by observed repo evidence: AddDVault() registration exists, SQLite persistence tests exist, and README keeps Postgres opt-in.

Split recommendations
- No additional split is needed for the current scope; the four-child decomposition on 06EXB7F6WNWSJJV14EXTPSFDRG remains sufficient.
- If residual work exists after PO cleanup, reopen or create a concrete child ticket instead of treating the parent epic as a generic dev handoff.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment