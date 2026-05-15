[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the delivery contract is bounded, the ticket has no open questions, and direct repository evidence matches the documented stale surfaces and source-of-truth references the developer needs.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `274e69aa83900fbc94d80b4a9b39bcc8f9d69ae5`, matching the supplied scratch ref; `git -C /mnt/c/Projects/DVault diff --stat 274e69aa83900fbc94d80b4a9b39bcc8f9d69ae5..HEAD` returned no output.
- `ls /mnt/c/Projects/DVault/docs/releases` lists `v0.10.0.md`, `v0.5.0.md`, `v0.6.0.md`, `v0.7.0.md`, `v0.8.0.md`, and `v0.9.0.md`; `docs/releases/v0.11.0.md` is absent.
- `README.md:10-16` and `examples/README.md:17-23` still use `--version 0.10.0` package commands.
- `README.md:35` still points to `DVault EF Design-Time Workflow` as `v0.8.0` guardrails, and `README.md:475-493` still presents `v0.10.0` as the current release baseline with SQLite-first live-schema wording.
- `docs/production-adoption-checklist.md:28-32` documents `validate`, `drift`, and `guardrail`, but line 31 still says SQLite is the supported live-schema reader and other providers are external/unsupported.
- `docs/model-first-governance.md:3-5` still says `Status: v0.7.0 branch documentation` and treats `v0.7.0` as the current baseline.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:8-10` states DVault does not ship a `dotnet ef` shim or intercept EF CLI commands, and `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:140-206` is the repository source of truth for the `export`, `validate`, `drift`, and `guardrail` command surface.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:14-32` registers built-in readers for SQLite, PostgreSQL, SQL Server, Oracle, `MySql.EntityFrameworkCore`, and `Pomelo.EntityFrameworkCore.MySql`.
- The ticket contract in the prompt marks `## Open Questions` as `none`, limits scope to `docs/releases/v0.11.0.md`, `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`, and the prompt snapshot lists `Recent comments: <none>`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The docs should explicitly cover the MySQL edge case that both `MySql.EntityFrameworkCore` and Pomelo map to the same built-in reader without implying extra DVault-managed infrastructure.
- The docs should make the difference explicit between runnable quickstarts today (SQLite and PostgreSQL) and broader built-in live-schema reader coverage (also SQL Server, Oracle, and MySQL).
- The docs should restate that `drift --live-schema` is optional and non-default even when a built-in reader exists.

Risky assumptions
- The contract assumes the named five files are the only current-baseline doc surfaces that need v0.11.0 wording; other public docs may still contain `0.10.0` or SQLite-only statements.
- The release note must derive every v0.11.0 claim from repository-visible behavior; if a desired release claim is not directly supported by the repo, the developer will need to narrow wording rather than infer it.

AC / test suggestions
- Completion evidence should cite all five changed documentation paths and the exact verification performed against them, even if the result is only repository inspection and formatting validation.
- Verify the final docs consistently name the four design-time verbs as `validate`, `export`, `drift`, and `guardrail`.
- Verify the final docs remove remaining `0.10.0` current-baseline references and reconcile live-schema wording with `DataVaultLiveSchemaReader.cs` while keeping live execution opt-in and consumer-managed.

Implementation watchouts
- Keep the consumer-owned design-time boundary consistent everywhere: DVault provides reusable library-hosted commands, does not ship a standalone CLI, and does not intercept `dotnet ef`.
- Do not replace one overstatement with another: built-in provider readers are supported in code, but operational live-database execution remains optional and adopter-managed.
- Use `docs/releases/v0.10.0.md` only as a shape/template reference; v0.11.0 content still has to be written from current repository evidence.

Non-blocking notes
- The branch currently has no implementation delta from the supplied scratch ref; that is normal for a pre-development handoff ticket.
- No ticket-comment discussion was provided in the snapshot, so the decision relies on the delivery contract plus direct repository inspection.

Split recommendations
- No split recommended. One missing release-note file plus four stale current-baseline documents is still a bounded documentation rollout suitable for the normal `po-critic -> dev` path.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment