[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract does not yet choose the canonical PIT public surface for docs and developer scope, while the repository exposes conflicting public PointInTime and PIT APIs.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NSXY2Y1JZ8SSCX177C770/description.md and comments/06EZSWY497GM2DA3SASS4NG1SC.md both persist ready_for_po_critic and Open Questions = none.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs defines both DataVaultPointInTimeMetadata and DataVaultPitMetadata, and src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs exposes both PointInTimeTables and Pits.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt exposes public DataVaultPointInTimeMetadata, public DataVaultPitMetadata, and public DataVaultModelBuilder.PointInTime(...).
- src/DCoding.Data.DVault/Modeling/DataVaultModel.cs and src/DCoding.Data.DVault/Modeling/DefaultDataVaultNamingPolicy.cs keep a public PointInTime path whose generated load column is PitLoadTimestamp; tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs asserts PitCustomerHistory and PitLoadTimestamp.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs creates DataVaultTableKind.Pit projections with Pit<Hub><Satellite...> table names and LoadTimestamp snapshot keys; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs asserts CustomerHashKey, LoadTimestamp, ProfileLoadTimestamp, and StatusLoadTimestamp.
- docs/plans/deferred-data-vault-capabilities.md says the architecture record is a guardrail and should not be used to infer concrete PIT API names.
- .gicket/tickets/06EZ0NSBM3GD7DY11Y4PZMXD28/ticket.json and .gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/ticket.json are done, and child tickets 06EZ0NT4FDPC7XTQH40PQS942M, 06EZ0NTB26CCYQ7FCN2REEGDGW, and 06EZ0NTJZEMVA5RPR01V0KNVMR are also done.
- git diff --name-only develop...HEAD excluding .gicket returned no output, so this story branch currently has ticket-metadata changes only.

Blocking findings
- The contract does not explicitly choose whether the story's minimal example and developer scope are anchored on the new DataVaultPitMetadata EF-translation path or on the older public PointInTime/DataVaultPointInTimeMetadata path.
- The repository's two public PIT surfaces currently imply different naming semantics (LoadTimestamp on the EF PIT path versus PitLoadTimestamp on the public PointInTime model path), but the ticket acceptance criteria only describe the EF PIT names and do not say how docs/examples must handle the existing public PointInTime contract.

Required PO actions
- State explicitly which public surface is canonical for this story's example and acceptance boundary: DataVaultMetadataModel/DataVaultPitMetadata only, or both with defined coexistence behavior.
- If the older public PointInTime/DataVaultPointInTimeMetadata surface is out of scope, say that directly in Clarifications or Scope Out and require the docs to call it out.
- If the older surface must be reconciled, materialize the already-mentioned API-shape follow-up as a tracked ticket or add explicit acceptance text for how both surfaces coexist.

Open issues ledger
- critic-item-1 [required-po-action] State explicitly which public surface is canonical for this story's example and acceptance boundary: DataVaultMetadataModel/DataVaultPitMetadata only, or both with defined coexistence behavior.
- critic-item-2 [required-po-action] If the older public PointInTime/DataVaultPointInTimeMetadata surface is out of scope, say that directly in Clarifications or Scope Out and require the docs to call it out.
- critic-item-3 [required-po-action] If the older surface must be reconciled, materialize the already-mentioned API-shape follow-up as a tracked ticket or add explicit acceptance text for how both surfaces coexist.
- critic-item-4 [blocking-finding] The contract does not explicitly choose whether the story's minimal example and developer scope are anchored on the new DataVaultPitMetadata EF-translation path or on the older public PointInTime/DataVaultPointInTimeMetadata path.
- critic-item-5 [blocking-finding] The repository's two public PIT surfaces currently imply different naming semantics (LoadTimestamp on the EF PIT path versus PitLoadTimestamp on the public PointInTime model path), but the ticket acceptance criteria only describe the EF PIT names and do not say how docs/examples must handle the existing public PointInTime contract.

Missing examples / edge cases
- A concrete minimal example showing the exact declaration surface the docs should use.
- A docs/example note covering the existing PointInTime public surface and whether its PitLoadTimestamp naming is unaffected or intentionally not part of this story.
- An explicit statement about whether users should expect the older PointInTime public path and the newer PIT EF path to coexist after this story.

Risky assumptions
- Assuming developers will infer DataVaultPitMetadata as the canonical public story without an explicit ticket statement.
- Assuming the docs can omit the older public PointInTime surface without creating compatibility confusion.
- Assuming the architecture note can stand in for a public API decision even though it explicitly avoids concrete PIT API naming.

AC / test suggestions
- Add an acceptance criterion or clarification that the minimal example must use the chosen canonical API surface and must mention whether DataVaultPointInTimeMetadata/PointInTime(...) are unchanged, deferred, or separately tracked.
- Add a docs/test expectation that this story does not silently redefine the existing PitLoadTimestamp PointInTime naming contract unless a separate public-API change is intentionally in scope.

Implementation watchouts
- Do not let docs/example work silently expand this story from translator-only PIT support into public API reconciliation unless the ticket explicitly approves that scope.
- The existing public PointInTime path and the EF PIT translation path currently name the load column differently; developers need an explicit scope boundary around that difference.
- Because the story branch has no product-code delta outside .gicket, downstream dev work will be packaging already-landed child capability behavior rather than extending a story-branch prototype.

Non-blocking notes
- The persisted contract's Open Questions section is resolved to none; the blocker is missing canonical public-surface guidance, not unresolved open questions.
- Blocking prerequisite stories are done, and the three child PIT tasks are done.

Split recommendations
- Keep this story translator/documentation-scoped only if PO clarifies the canonical public PIT surface now; otherwise split public API consolidation or deprecation of DataVaultPointInTimeMetadata/PointInTime into the follow-up API-shape ticket the contract already hints at.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment