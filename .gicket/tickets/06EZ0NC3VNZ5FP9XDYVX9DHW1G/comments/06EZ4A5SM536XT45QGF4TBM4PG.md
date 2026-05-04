[gicket-bot] PO refinement contract

Summary
- Refined the MySQL ticket to reuse the repository's existing external opt-in provider-test pattern and to keep MySQL on its current compatibility-only save path.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already provides default MySQL provider-registration smoke in unit tests and an external opt-in pattern for Postgres; this ticket should add the missing MySQL live opt-in path rather than invent a new testing model.
- MySQL remains a compatibility-only provider surface in v1: `AddDVaultMySql()` continues to call `AddDVault()` and this ticket does not require a MySQL-specific `IDataVaultProviderSaveStrategy` or new `DataVaultProviderCapabilityProfiles` entry.
- The opt-in contract for this ticket should follow the existing naming convention and use `DVAULT_TEST_MYSQL_CONNECTION_STRING` as the developer- or CI-supplied connection string.
- A single deterministic insert-only explicit save proof through a real MySQL-backed `DbContext` is the bounded v1 baseline for this ticket; broader provider behavior stays out of scope.

Scope In
- Add MySQL opt-in integration-test configuration and skip behavior in `tests/DCoding.Data.DVault.Tests/Integration` using the existing provider-category conventions.
- Add one live external MySQL smoke test that uses `AddDVaultMySql()` and `IDataVaultSaveService` to verify provider registration plus at least one insert-only explicit save scenario against a real MySQL database.
- Update provider test discovery expectations and repository documentation so local and CI users know how to enable the MySQL opt-in path.

Scope Out
- Any new MySQL-specific optimized save strategy, provider capability profile, upsert behavior, or concurrency contract.
- Repository-managed Docker/database provisioning, checked-in secrets, or a new CI workflow file.
- Broad MySQL parity coverage such as full schema snapshot validation, link/satellite matrices, or performance benchmarking.

Open questions
- none

Follow-up questions
- Should a later ticket add richer MySQL coverage such as link/satellite saves, reuse-path assertions, or schema translation checks once the live smoke path is stable?
- Should future automation add a repository-managed MySQL CI service job, or is documentation-only opt-in execution sufficient for the current stage?

Risks
- The live MySQL smoke depends on an external server and a chosen EF Core MySQL provider, so server-version or dialect differences should be contained by keeping the v1 scenario narrow and well documented.
- If MySQL test dependencies are conditionally restored like the Postgres path, local and CI instructions must set the opt-in environment variable consistently for restore/build/test or the live path can fail before execution.

Split recommendations
- If the work expands beyond one compatibility-path smoke test, split MySQL-specific optimized save behavior or capability-profile work into a separate provider ticket.
- If the team wants containerized provisioning or always-on CI execution for MySQL, split that automation from this ticket's test-contract and documentation scope.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment