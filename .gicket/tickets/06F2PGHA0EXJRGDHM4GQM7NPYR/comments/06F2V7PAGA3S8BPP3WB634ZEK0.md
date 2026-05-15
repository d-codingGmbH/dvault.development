[gicket-bot] PO-critic review contract

Summary
- Closure-only gate failed: the ticket is well-bounded for development, but the target branch still lacks the required v0.11.0 documentation changes and validation evidence.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGHA0EXJRGDHM4GQM7NPYR/description.md` has `## Open Questions` = `none`, so the contract itself is refined; the problem is closure-only routing, not missing clarification.
- `docs/releases/v0.11.0.md` is absent in the repository (`test -f` reported missing), and `docs/releases/` currently contains `v0.10.0.md`, `v0.9.0.md`, `v0.8.0.md`, `v0.7.0.md`, `v0.6.0.md`, and `v0.5.0.md`.
- `README.md:10-16` and `examples/README.md:17-23` still pin the coordinated package family to `0.10.0`.
- `README.md:475-493` still points readers to `docs/releases/v0.10.0.md` and says live-schema drift is `SQLite-first`; `docs/production-adoption-checklist.md:31` still says SQLite is the supported v1 live-schema reader.
- `docs/model-first-governance.md:224-226` still says the current branch lacks first-party CLI commands, CI gate snippets, and broad multi-provider live drift beyond SQLite, while `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs:57-67` implements `validate`/`export`/`drift`/`guardrail` and `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:13-33` registers built-in readers for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL/Pomelo.
- `git log develop..HEAD` on `ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no` shows only ticket-metadata commits (`d99c9f9e7`, `e4719b5b0`, `e4914a45f`, `dd60336df`), and `git diff develop..HEAD` changes only `.gicket/tickets/06F2PGHA0EXJRGDHM4GQM7NPYR/**`, with no README/docs paths.
- Current comments under `.gicket/tickets/06F2PGHA0EXJRGDHM4GQM7NPYR/comments/*.md` are claim/lease/PO automation plus the PO refinement comment; there is no human clarification blocking a later dev handoff.

Blocking findings
- This review is running under a closure-only gate, but the repository still needs the ticket's actual documentation work: `docs/releases/v0.11.0.md` is missing and the current public docs remain on the `0.10.0` / `SQLite-first` baseline.
- The target branch contains no documentation implementation or doc-verification evidence; the branch-only changes are `.gicket` ticket metadata and handoff comments.
- Approving closure would require assuming the missing README/examples/adoption/model-first/release-note updates landed elsewhere, and the bounded branch/repository inspection does not support that.

Required PO actions
- Fix the routing mismatch: either send this ticket through the normal pre-development `po-critic -> dev` path, or create a separate developer-owned follow-up ticket and reserve closure-only review for after the docs land.
- If closure-only is intentional, update the persisted contract to cite the exact commit(s) and paths that already satisfy the acceptance criteria and include the promised documentation-level verification evidence.
- Keep the bounded scope, but make the next handoff name the required path set explicitly: `docs/releases/v0.11.0.md`, `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`.

Open issues ledger
- critic-item-1 [required-po-action] Fix the routing mismatch: either send this ticket through the normal pre-development `po-critic -> dev` path, or create a separate developer-owned follow-up ticket and reserve closure-only review for after the docs land.
- critic-item-2 [required-po-action] If closure-only is intentional, update the persisted contract to cite the exact commit(s) and paths that already satisfy the acceptance criteria and include the promised documentation-level verification evidence.
- critic-item-3 [required-po-action] Keep the bounded scope, but make the next handoff name the required path set explicitly: `docs/releases/v0.11.0.md`, `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`.
- critic-item-4 [blocking-finding] This review is running under a closure-only gate, but the repository still needs the ticket's actual documentation work: `docs/releases/v0.11.0.md` is missing and the current public docs remain on the `0.10.0` / `SQLite-first` baseline.
- critic-item-5 [blocking-finding] The target branch contains no documentation implementation or doc-verification evidence; the branch-only changes are `.gicket` ticket metadata and handoff comments.
- critic-item-6 [blocking-finding] Approving closure would require assuming the missing README/examples/adoption/model-first/release-note updates landed elsewhere, and the bounded branch/repository inspection does not support that.

Missing examples / edge cases
- If this later returns as closure-only, state explicitly whether documentation verification is only repository validation/formatting or also includes rendered-link or docs-surface checks.

Risky assumptions
- Assuming closure-ready state from the PO handoff text would be unsafe; the inspected branch does not contain the promised doc files.
- Assuming SQLite-only live-schema guidance is still current would contradict `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:13-33`.
- Assuming the design-time command surface is still absent would contradict `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` and `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs`.

AC / test suggestions
- When this ticket is re-submitted for closure, require the completion evidence to cite the final changed-path set and the exact validation performed, or explicitly say why no doc-specific check beyond repository validation applied.
- Keep one acceptance note that current-doc text must align both the implemented verbs (`validate`, `export`, `drift`, `guardrail`) and the provider-support distinction: built-in readers exist for five providers, but external live-schema execution remains opt-in.

Implementation watchouts
- Do not let README/examples/adoption/model-first docs drift into different baselines; the current repo already mixes `v0.10.0`, `v0.8.0`, `SQLite-first`, and `no first-party CLI commands` wording.
- Keep the wording precise: built-in multi-provider readers are source-backed, but live-schema execution for external providers still depends on adopter-managed connection strings and should remain optional.
- Do not recast historical release notes; `docs/releases/v0.10.0.md` stays historical and `docs/releases/v0.11.0.md` becomes the public summary only when the work actually lands.

Non-blocking notes
- The delivery contract is otherwise bounded: `## Open Questions` is `none`, upstream dependency tickets are `done`, and the PO comments show no unresolved human clarification.
- Absent the closure-only audit context, this contract looks suitable for developer handoff.

Split recommendations
- No feature split is needed for the documentation rollout itself.
- If automation must keep a closure-only audit, split routing from implementation: let this docs task go to `dev` first and review closure only after the documentation commit and validation evidence exist.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment