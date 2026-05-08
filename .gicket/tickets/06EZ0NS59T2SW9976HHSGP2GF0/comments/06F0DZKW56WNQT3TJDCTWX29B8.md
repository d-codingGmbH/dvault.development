[gicket-bot] PO-critic review contract

Summary
- Repository evidence shows the deferred-capability baseline is largely present, but the epic and bridge-story contracts still conflict with current source and do not cleanly mark this parent as a tracking-only closure umbrella, so the ticket should return to PO for reconciliation.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NS59T2SW9976HHSGP2GF0/description.md:48-49 has Open Questions -> none.
- .gicket/tickets/06EZ0NS59T2SW9976HHSGP2GF0/description.md:36-40 requires reviewers to trace child deliverables without conflicting guidance and with no PO-level ambiguity.
- A bounded rg over .gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/comments found .gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/comments/06F0DB1C84HB2PAGTQ5YE4EKGW.md:5 with decision return_to_po and no later approve_for_dev bridge review comment.
- Current source and branch history show the cited bridge gap is already fixed: git show 47bef894a ([06EZ0NTV4SVAKV98C418T8A3CC] AUTO-INTEGRATION squash into develop) changed src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:485-493 now rejects hierarchy links unless they are exactly a two-participant self-link over one hub type, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:643-687 now cover Employee-Employee-Department and Employee-Employee-Employee rejection.
- git diff --name-only develop...HEAD listed only .gicket/tickets/06EZ0NS59T2SW9976HHSGP2GF0/**, so this epic branch contributes ticket metadata only and the repository baseline under review is the current develop-aligned source/docs/tests.
- The claimed baseline is present in repository source/docs: docs/plans/deferred-data-vault-capabilities.md:17-26 keeps PIT/bridge/multi-active/hooks opt-in; src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-47 keeps AddDVault() optionless; src/DCoding.Data.DVault/DataVaultSaveService.cs:9-35 keeps the explicit IDataVaultSaveService boundary; src/DCoding.Data.DVault/DataVaultOptions.cs:18-80 exposes additive resolver/provider-behavior hooks; src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:400-519 implements the PIT baseline and rejects multi-active/link-based PIT shapes.

Blocking findings
- The epic DoD is not satisfied because ticket evidence and repository evidence conflict. The epic risk at .gicket/tickets/06EZ0NS59T2SW9976HHSGP2GF0/description.md:56-59 and the bridge child contract at .gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/description.md:5-15 still say a hierarchy-validation gap remains, but current source/tests/branch history show that exact gap landed in commit 47bef894a.
- The parent contract still does not explicitly say this is a tracking-only or closure umbrella with no parent-owned implementation slice. .gicket/tickets/06EZ0NS59T2SW9976HHSGP2GF0/description.md:17-21 and :42-46 describe ratification/guardrail work, but the legacy draft at :71-89 still reads like feature implementation scope.

Required PO actions
- Update the epic contract to state explicitly whether ticket 06EZ0NS59T2SW9976HHSGP2GF0 is a tracking-only or closure umbrella with no parent-owned implementation slice, and align the scope and legacy-draft wording to that intent.
- Reconcile the bridge-story and epic contract language with current repository state. If the current source/test fix in commit 47bef894a is accepted as the closure of the hierarchy-validation gap, remove the stale remaining-gap language and re-run PO-critic on the bridge story so the child contract matches the integrated code.
- If PO believes bridge work is still missing despite the current source/tests, open or reopen one narrow child or follow-up with that exact remaining gap instead of leaving the done bridge story and the tracking epic in contradictory states.

Open issues ledger
- critic-item-1 [required-po-action] Update the epic contract to state explicitly whether ticket 06EZ0NS59T2SW9976HHSGP2GF0 is a tracking-only or closure umbrella with no parent-owned implementation slice, and align the scope and legacy-draft wording to that intent.
- critic-item-2 [required-po-action] Reconcile the bridge-story and epic contract language with current repository state. If the current source/test fix in commit 47bef894a is accepted as the closure of the hierarchy-validation gap, remove the stale remaining-gap language and re-run PO-critic on the bridge story so the child contract matches the integrated code.
- critic-item-3 [required-po-action] If PO believes bridge work is still missing despite the current source/tests, open or reopen one narrow child or follow-up with that exact remaining gap instead of leaving the done bridge story and the tracking epic in contradictory states.
- critic-item-4 [blocking-finding] The epic DoD is not satisfied because ticket evidence and repository evidence conflict. The epic risk at .gicket/tickets/06EZ0NS59T2SW9976HHSGP2GF0/description.md:56-59 and the bridge child contract at .gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/description.md:5-15 still say a hierarchy-validation gap remains, but current source/tests/branch history show that exact gap landed in commit 47bef894a.
- critic-item-5 [blocking-finding] The parent contract still does not explicitly say this is a tracking-only or closure umbrella with no parent-owned implementation slice. .gicket/tickets/06EZ0NS59T2SW9976HHSGP2GF0/description.md:17-21 and :42-46 describe ratification/guardrail work, but the legacy draft at :71-89 still reads like feature implementation scope.

Missing examples / edge cases
- The epic contract does not include a persisted example of what counts as sufficient closure evidence for a tracking-only parent after all child work lands.
- The bridge child contract does not yet include a closure note tying the previously blocked hierarchy shapes to the now-landed rejection cases in tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:643-687.

Risky assumptions
- Assuming reviewers will infer tracking-only epic from context is unsafe because the persisted contract never says that explicitly and the legacy draft still reads like a parent implementation scope.
- Assuming repository code alone can override stale child contracts is unsafe for this epic because its acceptance criteria and DoD require traceable child contracts and no conflicting scope guidance.

AC / test suggestions
- Add one epic-level acceptance or DoD note describing the expected closure bundle for this tracking parent: child PO-critic outcomes, current docs, current source/tests, and no remaining parent-owned implementation.
- Add one short contract note on the bridge story referencing the fixed Employee-Employee-Department and Employee-Employee-Employee rejection cases so future reviewers can trace the formerly blocked hierarchy gap quickly.

Implementation watchouts
- Do not reopen source work blindly: the specific hierarchy-validation fix previously called out as missing is already present in src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:485-493 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:643-687.
- If the bridge story is re-reviewed, use the integrated develop baseline from commit 47bef894a, not only the older blocking PO-critic comment, as the source-of-truth implementation evidence.

Non-blocking notes
- Other child stories do show non-blocking PO-critic approvals in their comment histories: architecture comment 06EZMR2NYDSZBJHRF04PJVE6S0.md, PIT comment 06EZTWHTDD9GJW0E4MJASAE6J0.md, multi-active comment 06F0C4Q1MNMFZ35JP5618Z01JR.md, and hooks comment 06EZPJ378KTX14YGM4BJVTQG8R.md each record decision approve_for_dev.
- Repository evidence broadly matches the epic technical baseline claims even though the ticket-state evidence is not yet clean enough for handoff.

Split recommendations
- No new epic-level split is needed if the current bridge validation fix is accepted; the immediate need is contract/state reconciliation.
- If more bridge work truly remains, keep it as one narrow bridge follow-up child rather than leaving the parent epic and the done bridge story in a contradictory partially-open state.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment