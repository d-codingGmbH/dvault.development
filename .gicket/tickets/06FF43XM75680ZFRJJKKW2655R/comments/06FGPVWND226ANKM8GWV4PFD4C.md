[gicket-bot] PO-critic review contract

Summary
- Scope and repository evidence align, but the persisted parent ticket still contradicts itself about the description rewrite that just happened, so it is not clean enough for developer handoff yet.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF43XM75680ZFRJJKKW2655R/description.md` now contains the full delivery contract with Scope In/Out, Acceptance Criteria, Definition of Done, and `## Open Questions` set to `none`.
- `git diff develop..HEAD -- .gicket/tickets/06FF43XM75680ZFRJJKKW2655R/description.md` shows this branch replaced the old one-line legacy description with a 71-line contract block plus the retained legacy draft.
- Commit `f954a1d73da514310c312b3b53dc4c4613771428` and comment `.gicket/tickets/06FF43XM75680ZFRJJKKW2655R/comments/06FGPSD5V2XSGKYC5S307EZS74.md` both say the PO pass updated the ticket description and handed the ticket to PO-critic.
- That same persisted `description.md` still says `No planning document, relation rewrite, attachment, or description update was materialized in this run`, `No ticket description rewrite, planning document, attachment, or relation change was applied in this refinement pass`, and asks whether a follow-up should write the aggregate contract into the ticket body; those statements conflict with the actual diff.
- Core repo scope is otherwise aligned across code, docs, and tests: `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:167,176,205`, `src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs:144-153`, `docs/architecture/dvault-v1-typed-row-mapper-contract.md:40-42`, `docs/model-first-governance.md:262`, `docs/production-adoption-checklist.md:29,169,171`, `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs:90-105,123`, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs:183-218` all agree on explicit unique produced participant names, support-bundle-driven helper generation, and deferred dependent-child/effectivity expansion.

Blocking findings
- The ticket's own persisted contract is factually inconsistent about description changes. The branch diff, handoff commit, and PO run report all show that `description.md` was updated in this pass, but the current contract still claims no description update or rewrite happened.
- The persisted follow-up and risk text is stale after that rewrite. The live `description.md` already contains the aggregate contract block, yet the ticket still asks whether a later pass should write that contract into the ticket body and still describes the live description as the short legacy draft.

Required PO actions
- Rewrite the delivery-contract history text so it matches the persisted ticket state: the description was updated in this PO pass and the aggregate contract is already in the ticket body.
- Remove or restate the stale follow-up and risk wording so the handoff surface no longer contains mutually exclusive statements about the same description update.

Open issues ledger
- critic-item-1 [required-po-action] Rewrite the delivery-contract history text so it matches the persisted ticket state: the description was updated in this PO pass and the aggregate contract is already in the ticket body.
- critic-item-2 [required-po-action] Remove or restate the stale follow-up and risk wording so the handoff surface no longer contains mutually exclusive statements about the same description update.
- critic-item-3 [blocking-finding] The ticket's own persisted contract is factually inconsistent about description changes. The branch diff, handoff commit, and PO run report all show that `description.md` was updated in this pass, but the current contract still claims no description update or rewrite happened.
- critic-item-4 [blocking-finding] The persisted follow-up and risk text is stale after that rewrite. The live `description.md` already contains the aggregate contract block, yet the ticket still asks whether a later pass should write that contract into the ticket body and still describes the live description as the short legacy draft.

Missing examples / edge cases
- A small valid-versus-invalid same-hub example matrix in the parent contract would help future readers compare `SourceCustomer` and `MatchedCustomer` against duplicate `Customer` endpoints, although the repository tests already prove the behavior.
- A brief example tying declaration order to persisted `SourceCustomerHashKey` and `MatchedCustomerHashKey` column order would make cross-surface review faster.

Risky assumptions
- This review assumes the unreadable duplicate relation `06FF43Z97VRFNMVKPZ13CKPN1C` is historical noise only, because the real replacement child `06FF43YPV3WYDQHEGZSW4T296C` is locally present and `done`.

AC / test suggestions
- Keep negative acceptance coverage for repeated same-hub links without an explicit relationship name and for duplicate `DataVaultLinkParticipantBindingAttribute` produced names raising `DMV1955`.
- Keep one end-to-end assertion that `SourceCustomer` and `MatchedCustomer` stay in declaration order from generator output through persisted `LinkCustomerIdentityMatch` columns.
- Keep the contract text explicit that typed helper generation stays support-bundle-driven and does not parse raw `dvault.model.v1` inputs directly.

Implementation watchouts
- Do not widen this parent story into model-first same-hub typed mapper generation, dependent-child metadata, effectivity-specific APIs, SaveChanges-driven write paths, or provider-specific SQL; the current repo docs and ticket scope all exclude those.
- Public names such as `ParticipantHubName` and `ParticipantHubNames` remain part of the current documentation and API surface, so renaming them here would create extra compatibility scope that this ticket does not approve.

Non-blocking notes
- Aside from the stale description-history prose, the actual scope is well bounded across code, docs, and tests.
- No additional split appears necessary; the already-done child tickets cover support-bundle facts, mapper parity, documentation alignment, and the nearby defer-now decisions.

Split recommendations
- No additional split recommended once the ticket text is reconciled; the existing child-ticket breakdown already covers the bounded work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment