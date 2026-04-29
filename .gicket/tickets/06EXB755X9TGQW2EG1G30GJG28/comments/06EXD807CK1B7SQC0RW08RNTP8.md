[gicket-bot] PO-critic review contract

Summary
- The refined contract is ready for developer handoff: prior PO-critic blockers were addressed with explicit four-role defaults, a scaffold dependency, and a documentation fallback while the repo still lacks source/test projects.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB755X9TGQW2EG1G30GJG28/description.md has ## Open Questions with '- none'.
- The delivery contract explicitly limits v1 roles to hash key, hash diff, load timestamp, and record source, and pins default effective names to HashKey, HashDiff, LoadTimestamp, and RecordSource.
- The delivery contract states implementation is ordered after foundation/project scaffolding and excludes creating .slnx, .csproj, src/DVault, or tests/DVault.Tests before foundation work provides them.
- Local inspection command output reported src missing, tests missing, src/DVault missing, and tests/DVault.Tests missing; git ls-files for *.sln, *.slnx, *.csproj, src/**, and tests/** returned no tracked files.
- docs/plans/06EXB6ZC4M7Q55PXTFBVWP34S0-adddvault-usedatavault-extension-shape.md establishes DCoding.Data.DVault as the v1 namespace baseline and says source/test scaffolding is out of scope until foundation tickets provide it.
- Comment .gicket/tickets/06EXB755X9TGQW2EG1G30GJG28/comments/06EXD2AQT7FRG6HSA0EA0PSQ3M.md records PO checklist responses addressing the previous scaffold and default-name blockers.
- Relation evidence shows .gicket/relations/PM/SW/06EXB6XBV95E08R2W9ZQ1PRDPM--06EXB74NRVRX18GD33CH1C12SW--blocks.json and .gicket/relations/SW/28/06EXB74NRVRX18GD33CH1C12SW--06EXB755X9TGQW2EG1G30GJG28--parentOf.json, giving this ticket an indirect foundation-order dependency through its parent modeling story.
- Branch history shows HEAD af661b5 on ticket/06EXB755X9TGQW2EG1G30GJG28-task-define-technical-metadata-column-contracts, with recent commits 20a090d handoff po->po-critic and af661b5 lease claim po-critic.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not specify null, empty, whitespace, or duplicate override-name behavior; this can remain a developer design choice unless PO wants those as acceptance cases.

Risky assumptions
- Because src/DVault and tests/DVault.Tests are absent, the first developer pass may need to produce the documented planning artifact rather than implementation code unless foundation scaffolding lands first.

AC / test suggestions
- When tests/DVault.Tests exists, verify all four defaults plus one explicit override per role, and assert override changes only the effective column name while preserving role identity and default name.
- If development proceeds as documentation-only, include the same cases as acceptance examples in the planning artifact.

Implementation watchouts
- Do not create solution, project, source, or test scaffolding in this ticket unless the foundation setup work has landed first.
- Use namespace DCoding.Data.DVault when implementation files exist, consistent with docs/plans/06EXB6ZC4M7Q55PXTFBVWP34S0-adddvault-usedatavault-extension-shape.md.
- Keep the v1 role set closed to HashKey, HashDiff, LoadTimestamp, and RecordSource and avoid structure-specific parallel role definitions.

Non-blocking notes
- The current repository root contains .gicket, .gicket-bot, .git, and docs, but no src or tests directories.
- The follow-up question about which foundation setup ticket should carry an explicit dependency can stay follow-up because the contract has no open questions and an indirect relation already exists through the parent modeling story.

Split recommendations
- No split needed for the four-role metadata contract; keep solution and test scaffolding in the existing foundation/test setup work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment