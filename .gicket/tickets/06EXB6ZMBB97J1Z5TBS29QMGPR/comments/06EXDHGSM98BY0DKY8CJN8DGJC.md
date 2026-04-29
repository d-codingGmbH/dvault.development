[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is clearer than before, but current repository evidence still shows the required public API, solution/test scaffold, and standard test command are absent, while the ticket remains blocked for dev/test and has no concrete dependency relation to the prerequisite implementation/scaffold work.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted delivery contract .gicket/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/description.md states Open Questions: none, but also says the ticket is sequenced after a foundation/test-suite scaffold exists and should pause or route to setup if source/test scaffold is absent.
- git ls-files src tests returned no tracked source or test files; git ls-files '*.sln' '*.slnx' '*.csproj' returned no tracked solution or project files.
- rg for AddDVault, UseDataVault, and DCoding.Data.DVault found the API shape in docs/plans/06EXB6ZC4M7Q55PXTFBVWP34S0-adddvault-usedatavault-extension-shape.md and ticket metadata, not in product source.
- docs/plans/06EXB6ZC4M7Q55PXTFBVWP34S0-adddvault-usedatavault-extension-shape.md identifies ticket 06EXB6ZC4M7Q55PXTFBVWP34S0 as a design-artifact ticket and explicitly says not to implement AddDVault, UseDataVault, DVaultOptions, or DataVaultModelOptions there.
- find .gicket/relations -name '*06EXB6ZMBB97J1Z5TBS29QMGPR*' found only .gicket/relations/T4/PR/06EXB6Z3YMAPSRYRB8NQX3ZST4--06EXB6ZMBB97J1Z5TBS29QMGPR--parentOf.json; no concrete blocking/dependency relation from this task to API implementation or scaffold work was observed.
- git diff --name-status develop..HEAD lists ticket/comment/event metadata for this ticket and .gicket/.ticket.write.lock, with no src, tests, solution, or project files.

Blocking findings
- The ticket is not ready for developer handoff now because its own contract says implementation must wait for a public startup API and runnable test scaffold, and direct repository evidence shows those prerequisites are still absent.
- Compatibility depends on public AddDVault and UseDataVault APIs, but only planning/ticket text exists locally; no source implementation or project containing those public members is available to target.

Required PO actions
- Keep this ticket out of dev until the prerequisite public API implementation, solution/library project, DVault test project, and standard test command exist, or explicitly rescope the ticket to include that larger setup work.
- Add or verify concrete blocking/dependency relation(s) from this ticket to the prerequisite API implementation and repository test-scaffold work, not only a prose sequencing note.

Open issues ledger
- critic-item-1 [required-po-action] Keep this ticket out of dev until the prerequisite public API implementation, solution/library project, DVault test project, and standard test command exist, or explicitly rescope the ticket to include that larger setup work.
- critic-item-2 [required-po-action] Add or verify concrete blocking/dependency relation(s) from this ticket to the prerequisite API implementation and repository test-scaffold work, not only a prose sequencing note.
- critic-item-3 [blocking-finding] The ticket is not ready for developer handoff now because its own contract says implementation must wait for a public startup API and runnable test scaffold, and direct repository evidence shows those prerequisites are still absent.
- critic-item-4 [blocking-finding] Compatibility depends on public AddDVault and UseDataVault APIs, but only planning/ticket text exists locally; no source implementation or project containing those public members is available to target.

Missing examples / edge cases
- No additional smoke-test example is blocking at the ticket level; the core example of a tiny consuming DbContext using default AddDVault and UseDataVault is present.

Risky assumptions
- Assuming a developer can implement only the smoke test despite the absent source/test scaffold would likely turn this into hidden architecture and repository setup work.
- Assuming the API-shape planning document is sufficient compatibility evidence would violate the requirement to verify public API/type availability from source when implementation depends on it.

AC / test suggestions
- When prerequisites exist, AC should remain focused on one deterministic minimal-startup smoke test that uses public AddDVault and UseDataVault and runs through the repository standard DVault test command without external services.

Implementation watchouts
- Do not assert internal registrations or EF convention details; verify observable startup success through the public minimal path.
- Use the repository's established test layout and local fixture/provider choice once the scaffold exists; avoid adding solution or project setup inside this task unless PO explicitly rescope it.

Non-blocking notes
- The Open Questions section is resolved as 'none', so the remaining issue is readiness/sequencing, not unanswered product detail.
- The parentOf relation to story 06EXB6Z3YMAPSRYRB8NQX3ZST4 is present.

Split recommendations
- No split is needed for the smoke-test body after prerequisites exist.
- If no existing ticket owns the solution/library/test scaffold and public API implementation, create or link those prerequisite tickets before sending this task to dev.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment