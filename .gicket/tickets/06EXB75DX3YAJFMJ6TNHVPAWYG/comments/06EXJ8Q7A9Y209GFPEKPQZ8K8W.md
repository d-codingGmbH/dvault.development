[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the persisted contract has no unresolved Open Questions, the prior PO-critic API-assumption findings were answered, and the current repository evidence supports the named source surfaces while allowing missing or changed configuration surfaces to be created by this story.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted .gicket/tickets/06EXB75DX3YAJFMJ6TNHVPAWYG/description.md contains PO Handoff decision ready_for_po_critic and ## Open Questions with only '- none'.
- gicket-read-ticket-comments returned 20 of 26 comments; the latest PO refinement comment states critic-item-1 through critic-item-3 were answered and restates that missing configuration/API surfaces may be introduced by the story.
- docs/naming/default-naming-policy.md defines the provider-neutral PascalCase v1 policy, including Hub/Link/Sat table formats, business-key and payload column rules, technical columns, normalization, finite singularization, reserved words, collision behavior, duplicate suffixes, and public DefaultNamingPolicy methods.
- src/DVault/Modeling/IDataVaultNamingPolicy.cs defines source-backed override methods for hub, link, and satellite table names, technical column names, index names, and constraint names, with corresponding context records and kind enums.
- src/DVault/Modeling/DefaultDataVaultNamingPolicy.cs implements IDataVaultNamingPolicy and currently composes placeholder-style names such as family + '__' + normalized parts, matching the contract risk that it must be updated where it conflicts with the v1 docs.
- src/DVault/Modeling/DataVaultModelOptions.cs exists and exposes IDataVaultNamingPolicy? NamingPolicy plus UseNamingPolicy, resolving to DefaultDataVaultNamingPolicy.Instance when unset.
- rg output shows src/DVault/Modeling/DataVaultConventions.cs uses DefaultNamingPolicy.Instance, and src/DVault/Modeling/DataVaultModelBuilderExtensions.cs UseDataVault applies DataVaultConventions.Default.
- rg output and ticket event files show existing parentOf relation evidence for 06EXB75DX3YAJFMJ6TNHVPAWYG -> 06EXB75NX7Z0DY7X0BD0YFZECM and 06EXB75DX3YAJFMJ6TNHVPAWYG -> 06EXB75XTWD7FTRAFE5GNDCS5R.
- git rev-parse HEAD returned ed98130e9199899fe157e490790ac8801f8662d5; git show --name-status HEAD showed the current HEAD commit is a po-critic lease claim touching only .gicket ticket/comment/event files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the contract requires participant-order fallback for unnamed links, while the current DataVaultModelBuilder.Link source requires a relationshipName; tests should cover the fallback through the policy API or any new model API if introduced.
- Non-blocking: docs/naming/default-naming-policy.md gives exact table and column examples but not exact index or constraint string examples; the contract's deterministic derived-from-table-and-columns rule is enough for handoff, but tests should lock the chosen produced names.

Risky assumptions
- The story intentionally leaves the implementation to reconcile the existing DefaultNamingPolicy and DefaultDataVaultNamingPolicy surfaces; source evidence shows both currently exist under src/DVault/Modeling.

AC / test suggestions
- Add tests for documented normalization examples, plural/singular equivalence, reserved object/property words, technical-column collisions, and duplicate property disambiguation within a column scope.
- Add custom-policy tests proving override behavior for hub/link/satellite table names, technical columns, indexes, and constraints via IDataVaultNamingPolicy.
- Add deterministic ordering tests over repeated model builds, including produced table, column, index, and constraint names.

Implementation watchouts
- src/DVault/Modeling/DataVaultModel.cs and src/DVault/Modeling/DataVaultModelBuilder.cs both declare public DataVaultModelBuilder; implementation should reconcile the model-declaration and UseDataVault convention-builder surfaces without losing required behavior.
- DefaultDataVaultNamingPolicy currently emits placeholder-style names like Hub__Customer and TechnicalColumn__HashKey__Customer, while docs/naming/default-naming-policy.md requires names like HubCustomer and CustomerHashKey.
- Keep PascalCase Data Vault modeling names separate from the lowercase snake_case dvault_* persistence convention documented in docs/plans/dvault-v1-default-persistence-convention-policy.md.

Non-blocking notes
- The previous PO-critic return reason was addressed by the PO comment and updated delivery contract: missing API names are no longer treated as pre-existing requirements unless found in source or intentionally created by this story.
- No new split is needed at ticket level; the current scope is cohesive around deterministic model naming and custom naming policy configuration.

Split recommendations
- No split recommended; keep existing child-ticket relations as context only and hand this story to dev as the deterministic naming implementation slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment