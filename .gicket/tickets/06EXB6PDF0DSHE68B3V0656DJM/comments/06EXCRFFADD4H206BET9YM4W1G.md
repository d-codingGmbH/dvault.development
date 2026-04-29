[gicket-bot] PO-critic review contract

Summary
- Persisted contract is ready for developer handoff: open questions are explicitly closed, scope is repository-level, and AC/DoD define EditorConfig plus automated local/CI formatting checks for the current governance-only baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB6PDF0DSHE68B3V0656DJM/description.md contains the delivery contract with `## Open Questions` set to `none`.
- The delivery contract Scope In requires two-space indentation, LF, UTF-8, final newline, trailing whitespace handling, tab rejection, same-line opening braces where applicable, root EditorConfig, a local verification command/script, and a CI/build-time gate.
- find .gicket/tickets/06EXB6PDF0DSHE68B3V0656DJM -maxdepth 1 -type d lists only the ticket directory, comments, and events; no target-ticket attachments directory was present.
- .gicket/relations/CC/JM/06EXB6NWYVB37D7S74VB3PVTCC--06EXB6PDF0DSHE68B3V0656DJM--parentOf.json has type `parentOf`, source `06EXB6NWYVB37D7S74VB3PVTCC`, and target `06EXB6PDF0DSHE68B3V0656DJM`.
- git rev-parse observed HEAD `09392d7b95dc8d990b1acb003bc638a707e0dd9f` on branch `ticket/06EXB6PDF0DSHE68B3V0656DJM-task-define-repository-formatting-enforcement`.
- git log --oneline shows `8b25c9f [06EXB6PDF0DSHE68B3V0656DJM] handoff po->po-critic` followed by `09392d7 ... lease claim po-critic`.
- git ls-files | rg -v '^\.gicket(-bot)?/' produced no output, and git ls-files for src/test/tests/.github/build manifests/.editorconfig patterns also produced no output.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The CI gate is intentionally specified at planning level because no CI/build manifest exists yet; implementation must keep the check runnable without assuming a current workflow file.
- Same-line brace enforcement depends on future file types or a checker/formatter configuration beyond EditorConfig, which the contract already calls out as an implementation risk.

AC / test suggestions
- Verify the delivered root policy covers indent_style=space, indent_size=2, LF, UTF-8, final newline, and trailing whitespace trimming where appropriate.
- Verify the local and CI/build-time checks fail on tabs in governed text files and use the same rule source or equivalent results.

Implementation watchouts
- Do not rely on .gicket operational metadata as the product formatting source; the contract says it was only planning evidence.
- Keep the local check non-mutating by default for CI parity, with any formatter/fix command optional and separate.
- Document generated, binary, lock, vendor, and tool-required exceptions narrowly so default text-file policy remains intact.

Non-blocking notes
- Parent ticket 06EXB6NWYVB37D7S74VB3PVTCC is still broad and marked needs-po, but this child ticket has a concrete bounded formatting-enforcement contract and an existing parentOf relation.
- The worktree has bot/runtime metadata modifications visible in git status, but the observed product surface remains governance-only for this ticket review.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment