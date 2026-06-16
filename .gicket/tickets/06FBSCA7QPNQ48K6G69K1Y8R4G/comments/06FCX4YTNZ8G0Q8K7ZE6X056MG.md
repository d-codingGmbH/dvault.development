[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract is bounded, but the ticket is routed as new implementation work even though the branch contains only `.gicket` metadata changes and the repository already carries the PostgreSQL code, tests, docs, and benchmark evidence it cites.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCA7QPNQ48K6G69K1Y8R4G/description.md` says the visible repository baseline already resolved the spike in favor of the existing `AddDVaultPostgres()` / `PostgresDataVaultSaveStrategy` implementation, and `## Open Questions` is `none`.
- `git -C /mnt/c/Projects/DVault diff --stat develop..HEAD` showed 27 changed files, all under `.gicket/tickets/06FBSCA7QPNQ48K6G69K1Y8R4G/...`; the filtered non-`.gicket` diff returned no files, so this branch carries no non-ticket repository delta.
- `src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs` contains `MinimumStagedBulkOperationCount = 60`, `NpgsqlProviderName = Npgsql.EntityFrameworkCore.PostgreSQL`, staged-provider diagnostics, and PostgreSQL `COPY` command generation.
- `tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs` covers `AddDVaultPostgres()` registration, provider-profile selection, the 60-operation boundary, and generated staging/COPY SQL.
- `tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs` already contains configured-PostgreSQL tests for ordered hub/link/satellite persistence plus `AddDVaultPostgresStagedBulkStrategyRollsBackFailureAndCleansUpWhenConfigured`.
- `benchmark-summary.csv` rows 30-32 preserve PostgreSQL provider-native bulk rows as skipped placeholders when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset, while `artifacts/benchmarks/v0.32.0-06F9XD33MNNVHHW232TC7T1CN8-scale-evidence-<redacted>/README.md` documents completed PostgreSQL retained-direct/UNNEST and staged-bulk timings.

Blocking findings
- The ticket is still framed and routed as pre-development implementation work, but the branch contains no non-`.gicket` delta and the delivery contract explicitly ratifies implementation and evidence that is already present in the repository. A developer handoff does not currently have a concrete remaining work product.
- Ticket lineage is unresolved. The current scope relies on already-landed PostgreSQL code and evidence and references the earlier done ticket and evidence trail, but the ticket is not marked `closure-only` or `no-work-required`, and the follow-up explicitly asks for lineage reconciliation.

Required PO actions
- Decide the correct lifecycle for `06FBSCA7QPNQ48K6G69K1Y8R4G`: convert it to `closure-only` or `no-work-required` if it only ratifies landed work, or redefine it with an explicit remaining repository delta that a developer must produce.
- Reconcile the ticket's lineage against the earlier PostgreSQL evidence work, at minimum clarifying the relationship to done ticket `06F9XD33MNNVHHW232TC7T1CN8` and whether this ticket is duplicate, absorbed, or follow-up verification.
- If the ticket is meant to stay open for dev, rewrite the title, handoff, and acceptance text so it states the remaining output unambiguously, such as a required benchmark rerun, explicit closure evidence, or another concrete non-`.gicket` deliverable.

Open issues ledger
- critic-item-1 [required-po-action] Decide the correct lifecycle for `06FBSCA7QPNQ48K6G69K1Y8R4G`: convert it to `closure-only` or `no-work-required` if it only ratifies landed work, or redefine it with an explicit remaining repository delta that a developer must produce.
- critic-item-2 [required-po-action] Reconcile the ticket's lineage against the earlier PostgreSQL evidence work, at minimum clarifying the relationship to done ticket `06F9XD33MNNVHHW232TC7T1CN8` and whether this ticket is duplicate, absorbed, or follow-up verification.
- critic-item-3 [required-po-action] If the ticket is meant to stay open for dev, rewrite the title, handoff, and acceptance text so it states the remaining output unambiguously, such as a required benchmark rerun, explicit closure evidence, or another concrete non-`.gicket` deliverable.
- critic-item-4 [blocking-finding] The ticket is still framed and routed as pre-development implementation work, but the branch contains no non-`.gicket` delta and the delivery contract explicitly ratifies implementation and evidence that is already present in the repository. A developer handoff does not currently have a concrete remaining work product.
- critic-item-5 [blocking-finding] Ticket lineage is unresolved. The current scope relies on already-landed PostgreSQL code and evidence and references the earlier done ticket and evidence trail, but the ticket is not marked `closure-only` or `no-work-required`, and the follow-up explicitly asks for lineage reconciliation.

Missing examples / edge cases
- The contract does not say what the developer should do when the repository already satisfies the cited code, test, doc, and evidence surfaces and the branch has no product-code changes.
- If a fresh PostgreSQL benchmark rerun is actually required, the ticket does not define the exact trigger, expected artifact path, or whether the outcome is additive evidence versus closure evidence.
- The ticket does not state how to close a no-delta outcome: duplicate or absorbed, `no-work-required`, or closure-only verification.

Risky assumptions
- It assumes a developer will infer the intended no-op or closure path from repository state without the ticket being explicitly reclassified.
- It assumes the checked-in v0.32 PostgreSQL bundle tied to earlier ticket `06F9XD33MNNVHHW232TC7T1CN8` can serve as this ticket's delivery evidence without additional lineage cleanup.
- It assumes the blocking relationship noted in `.gicket/tickets/06FBSCA7QPNQ48K6G69K1Y8R4G/comments/06FCX03CVFZ4C78TG406R41T8G.md` remains correct even if this ticket has no remaining implementation delta.

AC / test suggestions
- If the ticket stays open, add one acceptance criterion that states the exact remaining deliverable on this branch rather than re-stating already-landed repository state.
- If the intended outcome is evidence refresh, add explicit acceptance text for the benchmark source, provider filter, artifact location, and whether root skipped-placeholder rows remain unchanged.
- If the intended outcome is ticket hygiene only, move implementation-oriented acceptance criteria off this ticket and replace them with closure and lineage criteria.

Implementation watchouts
- Do not treat root `benchmark-summary.*` PostgreSQL skipped-placeholder rows as completed timing evidence; the completed timing surface is the checked-in v0.32 provider bundle.
- Keep PostgreSQL latest-satellite optimization out of scope; the docs matrix still records no provider-specific latest-satellite strategy for PostgreSQL.
- Any future work must preserve the observed 60-operation boundary between retained direct/UNNEST execution and staged `COPY`, plus provider-neutral fallback on dirty-context or unsupported-shape gates.

Non-blocking notes
- The PO run report in `.gicket/tickets/06FBSCA7QPNQ48K6G69K1Y8R4G/comments/06FCX0380DPJYZX7RS7SXG4SD4.md` shows the refinement activity was ticket description and label handoff work, not repository implementation work.
- Open Questions are resolved as `none`; the blocker is lifecycle clarity and duplicate or absorbed-work handling, not missing technical scope bullets.

Split recommendations
- No split is needed if PO converts this ticket to closure-only or no-work-required.
- If PO wants new work, separate lineage and housekeeping from any fresh benchmark or code delta so the developer handoff describes one concrete objective.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment