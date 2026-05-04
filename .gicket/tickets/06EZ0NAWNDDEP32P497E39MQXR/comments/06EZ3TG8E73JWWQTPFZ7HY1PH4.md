[gicket-bot] PO refinement contract

Summary
- Refined ticket 06EZ0NAWNDDEP32P497E39MQXR by ratifying the existing integration-test and provider-strategy baselines; no split or extra planning artifact is needed before PO-critic handoff.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the implementation surface to the existing integration test project under tests/DCoding.Data.DVault.Tests/Integration.
- The repository already defines ProviderIntegration.ExternalOptIn and the SQL Server provider trait in ProviderTestCategories, so this ticket should reuse those categories instead of introducing new test classification.
- Opt-in means the SQL Server lane must stay out of default local and CI runs unless configuration is explicitly supplied, and missing configuration must produce a clean skip message.
- The ticket already has an incoming parentOf relation from 06EZ0NADTKZP9J1YCVNMDH60WC.

Scope In
- Add a SQL Server integration-test configuration helper and usage pattern for the existing integration test project.
- Document the required local SQL Server configuration and how to invoke the opt-in smoke lane.
- Add SQL Server smoke coverage for one representative hub save, one link save, and one satellite save scenario against the optimized save path.
- Ensure the unconfigured path skips deterministically instead of failing with provider-load, connection, or null-configuration errors.

Scope Out
- Always-on CI or mandatory local SQL Server setup for default test runs.
- Broader SQL Server coverage such as batching, concurrency, retry, duplicate-reuse, or performance validation beyond the three representative smoke scenarios.
- Changes to provider-neutral dispatcher contracts or to non-SQL Server provider behavior.

Open questions
- none

Follow-up questions
- Should a later ticket add a reproducible local SQL Server bootstrap recipe or CI-hosted SQL Server lane once the opt-in smoke baseline proves stable?
- After this smoke baseline lands, do we want broader SQL Server coverage for duplicate reuse, batching, or failure translation beyond the one-scenario-per-entity-type contract?

Risks
- Because the SQL Server lane is external and opt-in, regressions can escape default automation unless contributors run the documented smoke command.
- Different local SQL Server versions or connection defaults can create environment-specific failures unless the documentation pins the supported setup tightly.

Split recommendations
- No split recommended; the repository already has a single bounded integration-test lane and established provider-test conventions, so configuration plus three smoke scenarios fits one task.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment