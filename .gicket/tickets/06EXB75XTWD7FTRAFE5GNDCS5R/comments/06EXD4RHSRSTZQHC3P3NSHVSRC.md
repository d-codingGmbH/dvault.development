[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the refreshed contract removes prior PO-critic blockers by enumerating the v1 naming-policy override surface and explicitly bounding sibling-owned default naming semantics.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/description.md has ## Open Questions with '- none'.
- .gicket/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/description.md Scope In requires a public naming-policy abstraction, an optional DataVaultModelOptions-aligned hook, default-policy plumbing, and default/custom tests.
- .gicket/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/description.md clarifies the six v1 override families: hub table names, link table names, satellite table names, Data Vault technical column names, index names, and constraint names.
- .gicket/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/description.md Scope Out assigns detailed default naming convention semantics to sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM and excludes casing, singular/plural handling, reserved-word handling, and detailed examples.
- Relation files .gicket/relations/YG/5R/...--06EXB75XTWD7FTRAFE5GNDCS5R--parentOf.json and .gicket/relations/YG/CM/...--06EXB75NX7Z0DY7X0BD0YFZECM--parentOf.json show both tickets share parent story 06EXB75DX3YAJFMJ6TNHVPAWYG.
- .gicket/tickets/06EXB75DX3YAJFMJ6TNHVPAWYG/description.md parent story scope covers naming for hubs, links, satellites, technical columns, indexes, and constraints, matching this ticket's override families.
- Comment .gicket/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/comments/06EXBQJG0WNKE9ANRYAD2HH39G.md recorded prior blockers around sibling default-policy boundaries and under-specified override targets.
- Comment .gicket/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/comments/06EXCZBK3FGX463EHM52QVW3YR.md records PO answers for those critic items, including the sibling boundary and six-family custom-policy coverage.
- git show --stat --oneline 0fb7c02fe830 shows the PO handoff commit updated the target ticket description and added the PO refinement comment; git log shows current HEAD bd8a187 is the po-critic lease claim on the target branch.
- docs/plans/06EXB6ZC4M7Q55PXTFBVWP34S0-adddvault-usedatavault-extension-shape.md defines DataVaultModelOptions as the model-convention options type and includes UseDataVault overloads accepting Action<DataVaultModelOptions>.
- git ls-tree -r --name-only HEAD filtered for src/test roots, .sln/.slnx, and project files produced no matches; the target contract explicitly acknowledges no visible source/test roots and directs implementation to the first owning modeling layout.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking gap: concrete default naming examples and edge cases are intentionally scoped to sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM, while this ticket now specifies the override families and required default/custom test paths.

Risky assumptions
- Sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM is still needs-po, so parallel implementation assumes active coordination to avoid conflicting default naming semantics.
- DataVaultModelOptions is evidenced in the planning document, not current source; once source exists, developers must verify the actual public type/API before treating compatibility as established.

AC / test suggestions
- Keep the existing AC expectation that the custom-policy test proves caller-provided output for each produced v1 family in the modeled scenario: hub table, link table, satellite table, technical column, index, and constraint.
- Keep the existing default-path AC that the modeling flow succeeds without a naming policy and produces deterministic built-in names automatically.

Implementation watchouts
- Do not finalize casing, pluralization, reserved-word, or detailed default naming semantics in this ticket; preserve or delegate to sibling-owned behavior when available.
- Because the current branch has no tracked implementation roots, use the first established modeling project/test layout rather than creating an unrelated structure.
- Treat the naming-policy contract as public API and keep it minimal while still covering the six enumerated v1 name families.

Non-blocking notes
- No additional split is needed for the override abstraction, options hook, default plumbing, and tests.

Split recommendations
- No split recommended; the sibling default naming policy ticket remains the separate boundary for detailed default naming rules.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment