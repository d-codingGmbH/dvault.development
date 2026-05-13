[gicket-bot] PO refinement contract

Summary
- Refined the story around the existing relation split: done child task 06F1XPW1N9PATP3R6YG53ZNGV0, completed prerequisite stories 06F1XPS7KGKBP5SVMQPJC49J2G and 06F1XPTCGWTJHHQVNPN13KANMG, and blocked downstream drift story 06F1XPWB8DZR4J8EZ00V8DT25G; no new planning documents, attachments, or relation writes were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Done story 06F1XPS7KGKBP5SVMQPJC49J2G (Story: Establish stable DVault diagnostic codes) is the completed diagnostic-id foundation for this ticket, so design-time validation here should reuse the stable DMV#### and DVM2xxx families instead of inventing a new naming scheme.
- Done story 06F1XPTCGWTJHHQVNPN13KANMG (Story: Add EF migration guardrails for Data Vault structures) already provides the reusable DataVaultMigrationOperationDiagnostics and DataVaultMigrationGuardrailReport baseline, so migration guardrail surfacing in this story is a design-time composition task rather than a new guardrail engine.
- Done child task 06F1XPW1N9PATP3R6YG53ZNGV0 (Task: Wire design-time validation into a sample workflow) is the existing proof slice for repository-backed design-time validation coverage and should be reused rather than re-split.
- The visible repository baseline already exposes provider-neutral DbContext analysis through IDataVaultDiagnosticsService.Analyze(DbContext) plus human-readable ToDisplayString rendering, so this story should compose those existing surfaces into an EF design-time path instead of inventing a parallel diagnostics model.
- The current repository does not reference Microsoft.EntityFrameworkCore.Design and does not yet expose IDesignTimeServices, so the safe v1 default is a documented opt-in dotnet ef composition path that keeps any EF design-time glue minimal and provider-neutral.
- No new child tickets, relation updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Document one bounded opt-in dotnet ef design-time path, adding only the minimal provider-neutral implementation needed to support that path.
- Reuse existing provider-neutral diagnostics surfaces to summarize model validation and migration guardrail findings during design-time workflows.
- Keep the baseline anchored on a DbContext that already projects DVault metadata, so both code-first ApplyDataVaultMetadata(...) and registry/model-first UseDataVaultMetadata(...) remain eligible inputs.
- Document the project-layout baseline and limitations that are actually evidenced in the repository, including that the current proof path is design-time-only and does not require a live database.

Scope Out
- No custom dotnet ef fork, IDE extension, or provider-specific online migration runner.
- No new ModelSnapshot or live database schema drift engine in this story; that expansion is already captured by story 06F1XPWB8DZR4J8EZ00V8DT25G and child tasks 06F1XPWNAWWMDBRK315S66P7AM and 06F1XPWYZTWE9E46GNPFB8F804.
- No provider-specific design-time package split or provider-name-specific output contracts.
- No promise of broader startup-project or target-project layouts beyond the layouts explicitly exercised and documented in-repo.

Open questions
- none

Follow-up questions
- After this provider-neutral design-time contract lands, should DVault later ship a first-party IDesignTimeServices package if consumer-owned guidance proves too heavy?
- Should a later docs or example ticket add an explicitly exercised startup-project versus target-project dotnet ef layout once the repository has a concrete proof case?
- When story 06F1XPWB8DZR4J8EZ00V8DT25G starts, should its ModelSnapshot and live-schema output plug into the same design-time reporting path established here?

Risks
- If this story tries to absorb ModelSnapshot or live-schema drift work now, it will duplicate already-created downstream tickets and blur the milestone split.
- If docs promise EF project-layout variants that the repository does not exercise, the design-time contract will overstate support.
- If the implementation adds a hard EF design-package dependency to the core library without a minimal justification, the provider-neutral surface and package boundary may grow unnecessarily.

Split recommendations
- No new split is needed for PO-critic readiness: existing done child task 06F1XPW1N9PATP3R6YG53ZNGV0 already captures the sample/workflow slice, and existing downstream story 06F1XPWB8DZR4J8EZ00V8DT25G plus tasks 06F1XPWNAWWMDBRK315S66P7AM and 06F1XPWYZTWE9E46GNPFB8F804 already capture post-boundary drift expansion.
- If first-party packaged EF design-time integration later needs its own delivery boundary, create a focused follow-up task rather than expanding this story beyond the provider-neutral validation and reporting contract.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment