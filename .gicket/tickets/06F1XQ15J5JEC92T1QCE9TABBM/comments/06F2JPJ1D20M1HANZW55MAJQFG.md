[gicket-bot] PO-critic review contract

Summary
- Cannot complete PO-critic review without direct tool evidence. The provided snapshot is seed context only, and the role requires repository, branch-history, and ticket/comment verification before deciding readiness.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- No interactive repository or gicket tool results were executed in this response.
- The prompt states the persisted delivery contract has ## Open Questions: none.
- The prompt snapshot shows src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj has <IsPackable>false</IsPackable>, which keeps packability as an explicit risk unless directly verified or documented in the current repository state.

Blocking findings
- PO-critic cannot approve from prompt seed context alone; direct ticket/comment state and repository/branch evidence are required by the task instructions.

Required PO actions
- Re-run the PO-critic review with access to the declared local tools so the reviewer can verify the latest persisted ticket, comments, branch history, analyzer project packaging metadata, solution inclusion, tests, and documentation guidance.

Open issues ledger
- critic-item-1 [required-po-action] Re-run the PO-critic review with access to the declared local tools so the reviewer can verify the latest persisted ticket, comments, branch history, analyzer project packaging metadata, solution inclusion, tests, and documentation guidance.
- critic-item-2 [blocking-finding] PO-critic cannot approve from prompt seed context alone; direct ticket/comment state and repository/branch evidence are required by the task instructions.

Missing examples / edge cases
- none

Risky assumptions
- Assuming the prompt snapshot reflects current repository state would violate the instruction to treat seed branch snapshots as context rather than the source of truth.

AC / test suggestions
- none

Implementation watchouts
- Verify whether IsPackable=false is intentionally documented or changed before developer handoff, because the delivery contract calls out analyzer package pack readiness or a documented deferral.

Non-blocking notes
- none

Split recommendations
- none

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment