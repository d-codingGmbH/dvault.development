[gicket-bot] PO-critic review contract

Summary
- Stable hashing contract is sufficiently refined for developer handoff; no unresolved Open Questions remain in the persisted ticket contract.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted Delivery Contract states PO Handoff decision ready_for_po_critic and ## Open Questions contains '- none'.
- Persisted Acceptance Criteria require documentation of abstraction responsibilities, SHA-256 UTF-8 lowercase hex default behavior, replaceability through options/DI, examples/vectors for deterministic/null/empty/substitution cases, and explicit non-goals for password hashing/encryption/signatures.
- gicket-read-ticket-comments returned totalComments=9 and returnedComments=9; observed comments are bot claim/lease/refinement/handoff/run-report entries with no separate blocking PO discussion.
- repository-list-directory at . returned only .gicket and .gicket-bot at repository root; targeted git ls-files for docs/plans, .gicket-bot/planning, src, tests, .sln, and .csproj returned no tracked entries.
- git show HEAD:.gicket/milestones/06EXB6F6Z8HMH2BQKDY1ZKQCPC.json identifies the milestone as Foundation and architecture, status planned.
- git grep found one direct relation for this ticket: .gicket/relations/38/ZC/06EXB765S2X2MR2K18ZBV8RC38--06EXB76DNVSRBD12T4W03AWQZC--parentOf.json; the parent ticket is Story: Implement hash key and hash diff services.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Ensure the eventual design notes include concrete vectors for repeated deterministic hashing, normalization-sensitive values, null or empty handling, and alternate implementation substitution, matching the persisted AC.

Implementation watchouts
- No tracked source, test, docs/plans, or .gicket-bot/planning files were observed, so the implementation should keep the result to the agreed planning/project documentation area and avoid broad repository structure expansion unless the dev-role context authorizes it.
- Keep algorithm identity and normalization rules explicit because the contract calls out future persisted-hash compatibility risk.

Non-blocking notes
- git status showed local working-tree modifications under bot/ticket operational files, but the target description diff disappears with --ignore-space-at-eol and the latest persisted ticket was verified through gicket-read-ticket.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment