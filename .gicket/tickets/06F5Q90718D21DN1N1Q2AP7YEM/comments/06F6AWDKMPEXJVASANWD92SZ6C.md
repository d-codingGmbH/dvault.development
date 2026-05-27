[gicket-bot] PO-critic review contract

Summary
- Return to PO: the refined contract has useful scope and no open questions, but the authoritative text is stale about blocker state and over-broad about the staged-bulk story versus current repository evidence.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q90718D21DN1N1Q2AP7YEM/description.md` contains `## Open Questions` -> `- none`, so the contract has no unresolved open-question gate.
- The same `description.md` risk section says the ticket "remains relation-blocked" by `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`.
- `git diff develop..HEAD -- .gicket/tickets/06F5Q90718D21DN1N1Q2AP7YEM/description.md` shows the delivery-contract block was added on this branch, even though the contract summary says no description edits were materialized.
- `docs/releases/v0.19.0.md:24-29`, `docs/production-adoption-checklist.md:79-80`, and `docs/releases/v0.19.0/README.md:17-18` establish v0.19.0 as the current public baseline and keep staged provider bulk outside the v0.19.0 public claim set.
- `docs/architecture/dvault-v1-explicit-save-service.md:56-78` and `benchmark-summary.md:60-69` show provider-optimized bulk behavior is mixed: PostgreSQL staged COPY, SQL Server native bulk, MySQL staged bulk, and Oracle direct optimized batching with `stagedOracleBulk=not-selected-no-measured-win`.
- `find docs/releases -maxdepth 2 -type f` shows release-note files through `docs/releases/v0.19.0.md`; no `docs/releases/v0.20.0.md` exists yet.
- `rg -n "stored procedure|stored-procedure|stored procedures" README.md docs benchmarks src tests` returned no matches, so local repo evidence does not show a documented or public stored-procedure feature surface.

Blocking findings
- The authoritative contract is factually inconsistent about its own refinement output: it claims no description edits were materialized, but the branch diff shows the delivery contract itself was added to `.gicket/tickets/06F5Q90718D21DN1N1Q2AP7YEM/description.md`.
- The requested v0.20.0 narrative overgeneralizes staged bulk as the preferred provider-optimized path without accounting for source-proven provider exceptions, especially Oracle's retained direct optimized path; this would let docs overclaim the current write-path hierarchy.

Required PO actions
- Update the delivery contract summary/risk text to match current persisted state: remove the claim that the ticket remains relation-blocked by `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`, or restate that item as historical/conditional rather than current status.
- Correct the refinement audit text that says no description edits were materialized.
- Clarify the provider-specific v0.20.0 hierarchy in the contract: staged bulk where supported/measured, SQL Server native-bulk wording, Oracle direct-optimized exception, provider-neutral explicit-save baseline, and stored procedures remaining non-default escape-hatch guidance only.
- Name the expected benchmark-facing deliverable file(s) if more than one document is intended, since the repo already has `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/releases/v0.19.0/README.md` as benchmark-facing documentation surfaces.

Open issues ledger
- critic-item-1 [required-po-action] Update the delivery contract summary/risk text to match current persisted state: remove the claim that the ticket remains relation-blocked by `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`, or restate that item as historical/conditional rather than current status.
- critic-item-2 [required-po-action] Correct the refinement audit text that says no description edits were materialized.
- critic-item-3 [required-po-action] Clarify the provider-specific v0.20.0 hierarchy in the contract: staged bulk where supported/measured, SQL Server native-bulk wording, Oracle direct-optimized exception, provider-neutral explicit-save baseline, and stored procedures remaining non-default escape-hatch guidance only.
- critic-item-4 [required-po-action] Name the expected benchmark-facing deliverable file(s) if more than one document is intended, since the repo already has `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/releases/v0.19.0/README.md` as benchmark-facing documentation surfaces.
- critic-item-5 [blocking-finding] The authoritative contract is factually inconsistent about its own refinement output: it claims no description edits were materialized, but the branch diff shows the delivery contract itself was added to `.gicket/tickets/06F5Q90718D21DN1N1Q2AP7YEM/description.md`.
- critic-item-6 [blocking-finding] The requested v0.20.0 narrative overgeneralizes staged bulk as the preferred provider-optimized path without accounting for source-proven provider exceptions, especially Oracle's retained direct optimized path; this would let docs overclaim the current write-path hierarchy.

Missing examples / edge cases
- Oracle edge case: current repo evidence keeps Oracle on direct optimized batching and explicitly says staged Oracle is not selected without a measured win.
- Optional-provider evidence edge case: benchmark artifacts currently keep PostgreSQL, SQL Server, MySQL, and Oracle rows visible as `skipped` when connection strings are absent; docs should say that skipped rows are still authoritative evidence under the contract.
- Release-note edge case: the repo has no `docs/releases/v0.20.0.md` yet, so the ticket should say whether the task creates it or updates another pending release-notes surface.
- Stored-procedure edge case: no repo-local stored-procedure docs/API references were found, so wording must avoid implying built-in generation or runtime management.

Risky assumptions
- Assumes `preferred provider-optimized path` can be written as a single staged-bulk story across all providers despite mixed provider implementations in current source and benchmark evidence.
- Assumes the lack of current relation blockers is obvious to a developer even though the authoritative contract still says the ticket remains blocked.
- Assumes stored procedures can be mentioned without accidentally implying a supported DVault public feature surface; current repo search found no stored-procedure references.
- Assumes benchmark-facing target docs are self-evident even though multiple repository docs already serve that role.

AC / test suggestions
- Add an acceptance criterion that v0.20.0 docs preserve provider-specific truth: PostgreSQL/MySQL staged-bulk guidance, SQL Server native-bulk guidance, Oracle direct-optimized exception, and provider-neutral fallback/explicit-save baselines remain accurate.
- Add an acceptance criterion that benchmark-facing docs keep pointing to `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, and preserve skipped optional-provider rows plus provider/runtime context per `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Add an acceptance criterion that stored-procedure wording explicitly says DVault does not auto-generate or auto-manage stored procedures as the standard runtime path.

Implementation watchouts
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` already documents provider-native bulk evidence and Oracle's `stagedOracleBulk=not-selected-no-measured-win`; any v0.20.0 docs need to stay aligned with that file and `docs/architecture/dvault-v1-explicit-save-service.md`.
- `docs/releases/v0.20.0.md` is not present yet under `docs/releases`, so the task likely includes creating a new release-notes file unless PO specifies another target.
- Current benchmark artifacts show optional provider-native bulk rows as skipped when external connection strings are absent; docs must not convert skipped rows into universal performance claims.

Non-blocking notes
- The contract's `## Open Questions` section is `none`, so that specific approval gate is satisfied.
- The branch diff from `develop..HEAD` is ticket metadata/comments/description only; there is no expectation of product-code or test evidence yet for this pre-development documentation task.
- Parent relation `.gicket/relations/AR/EM/06F5Q8YBVRS2EZVMJK5EATV9AR--06F5Q90718D21DN1N1Q2AP7YEM--parentOf.json` exists, so higher-level tracking is already present.

Split recommendations
- none

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment