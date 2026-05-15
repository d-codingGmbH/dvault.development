[gicket-bot] PO-critic review contract

Summary
- The persisted contract is repo-grounded, has no unresolved PO questions, and is ready for developer handoff as a narrowly scoped docs/examples task.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGGR30XXCDKCZ8W2J2WX8C/description.md` contains PO handoff decision `ready_for_po_critic` and `## Open Questions` set to `- none`.
- `.gicket/tickets/06F2PGGR30XXCDKCZ8W2J2WX8C/comments/06F2SP44THWKJ19XH8KKHNQ6XG.md` records the bounded scope as GitHub Actions examples for `validate`, artifact-based `drift`, and `guardrail`, with no new command verbs and no broad README/release-note rollout.
- On branch `ticket/06F2PGGR30XXCDKCZ8W2J2WX8C-task-add-ci-workflow-examples-for-design-time-ch`, `git log --oneline --decorate --max-count=20 --all --grep='06F2PGGR30XXCDKCZ8W2J2WX8C'` showed only workflow commits `b4f6eb00a`, `0e7f7978b`, `09c9afedc`, and `af0f8dcfb`.
- `git diff --stat af0f8dcfbb865a86a59ad7d561bd1e01a7507418..HEAD` returned no file changes, confirming this branch is still pre-development handoff state rather than partially implemented work.
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` implements reusable verbs `validate`, `export`, `drift`, and `guardrail`; its usage text is `dvault validate`, `dvault drift [--live-schema] (--artifact <path>|<path>)`, and `dvault guardrail (--migration <name>|<name>)`.
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs` makes both the design-time `DbContext` factory and migration-operation resolver consumer-owned, matching the contract's single-project ownership boundary.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, `docs/production-adoption-checklist.md`, and `examples/README.md` already establish the consumer-owned design-time workflow, artifact-vs-model drift posture, and SQLite-first optional live-schema boundary, but they do not yet provide the GitHub Actions CI snippet this ticket is scoped to add.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Clarify the conditional case when no reviewed `dvault.model.v1` artifact exists yet, so the default CI lane does not imply every adopter already has a committed artifact.
- Show whether `guardrail` is conditional on a newly scaffolded migration already existing in the change, or document it as a separate post-scaffold lane.
- If an optional `--live-schema` example is shown, state unsupported/unavailable behavior for non-SQLite providers so readers do not mistake it for the default gate.

Risky assumptions
- Readers will understand that `dotnet run --project <consumer-project> -- validate|drift|guardrail` is a consumer-owned host pattern, not a built-in DVault executable shipped by the package.
- Adopters already have a stable artifact path and review convention for `dvault.model.v1` when enabling the artifact-based drift lane.
- The design-time workflow anchor can absorb CI examples without leaving conflicting older wording about schema-drift/reporting on the same doc path.

AC / test suggestions
- Acceptance evidence should include one exact GitHub Actions YAML example plus exact rerunnable command lines that match the real verb/option shapes in `DataVaultDesignTimeCommand.cs`.
- Keep existing docs/format validation as the default completion evidence unless the ticket introduces executable sample code; only then should extra sample validation be required.
- Verify the final docs explicitly distinguish blocking `validate` and artifact-based `drift` from non-default `export` or optional live-schema flows.

Implementation watchouts
- Update the design-time workflow anchor or adjacent linked docs together so the new CI example does not contradict the current `Unsupported In V1` wording in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`.
- Do not imply DVault intercepts `dotnet ef` or ships a standalone CLI; `DataVaultDesignTimeCommandHost` keeps `CreateDbContext` and `ResolveMigrationOperations` in the consumer application.
- Keep artifact-versus-design-time-model drift as the default blocking lane; `--live-schema` must stay explicitly optional and SQLite-first/external-opt-in.
- Keep `export` framed as artifact maintenance or refresh, not as the default blocking CI check.

Non-blocking notes
- The current ticket branch has no code/docs diff beyond PO workflow commits, which is expected at this pre-development gate.

Split recommendations
- No further split is needed for this ticket as written.
- Keep non-GitHub CI templates and provider-specific secret-backed live-schema CI examples as separate follow-up tickets rather than widening this task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment