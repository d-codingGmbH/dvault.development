[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff. The persisted contract has no unresolved Open Questions and gives a bounded documentation-only story with enough repository-backed source evidence for formatting, layout, .NET baseline, namespace, naming, hashing, persistence, and Data Vault concept references.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB6NWYVB37D7S74VB3PVTCC/description.md contains PO Handoff decision ready_for_po_critic and ## Open Questions with '- none'.
- Comment 06EXE60EVNT1TFYXS2CCCH3TG0.md records the PO refinement contract; comment 06EXE60XC3XZG6V5P1W99VVHM8.md records handover to po-critic; comment 06EXE6162Q03ECSMTNCANAEMV4.md links handoff commit 6849ae2c5de7.
- Comment 06EXE61WYN8HR6ET28CN6GETMW.md says relation follow-up comments were applied to blocked ticket 06EXB6XBV95E08R2W9ZQ1PRDPM and child tickets 06EXB6P4ZNYA46MSYRGAJ9ZEPM and 06EXB6PDF0DSHE68B3V0656DJM.
- git show --stat 6849ae2c5de7 showed only .gicket ticket/comment/event metadata changes for the PO handoff, not product-code changes.
- git ls-files docs/plans/shared-implementation-standards.md returned empty, so the shared standards artifact remains the developer deliverable rather than pre-existing work.
- docs/formatting.md, .editorconfig, .gitattributes, and tools/check-format.sh exist and together cover LF, UTF-8 without BOM, final newline, trailing whitespace, tab exceptions, same-line braces, and the bash tools/check-format.sh gate.
- README.md defines the repository layout baseline for DVault.slnx, src/, tests/, examples/, benchmarks/, docs/, and tracked placeholder folders.
- repository-list-directory src observed src/DCoding.Data.DVault/.gitkeep, src/DCoding.Data/.gitkeep, src/DVault/DVault.csproj, and src/DVault/Modeling/DefaultNamingPolicy.cs.
- src/DVault/DVault.csproj directly sets TargetFramework net10.0, ImplicitUsings enable, Nullable enable, and GenerateDocumentationFile true.
- git grep found src/DVault/Modeling/DefaultNamingPolicy.cs:4: namespace DVault.Modeling; confirming the namespace evidence referenced by the contract.
- git ls-files confirmed docs/architecture/mvp-data-vault-concepts.md, docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md are tracked reference sources.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract intentionally treats the README-reserved src/DCoding.Data.DVault path and the visible src/DVault project as transitional; the standards artifact must name that mismatch instead of silently choosing a new convention.
- The future artifact is expected to reference existing standards instead of copying them; downstream drift is still possible if later tickets duplicate policy text.

AC / test suggestions
- Acceptance review should verify the produced standards artifact explicitly references docs/formatting.md, README.md, docs/architecture/mvp-data-vault-concepts.md, docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md.
- Acceptance review should verify the artifact names the existing child/dependency ticket ids and clearly separates v1 defaults from deferred provider-specific, CI-specific, and layout-reconciliation follow-up work.
- The implementation should run the non-mutating bash tools/check-format.sh gate after adding or updating the documentation artifact.

Implementation watchouts
- Keep the work documentation-only: no product-code refactor, provider behavior, migrations, schema generation, runtime configuration APIs, or CI workflow creation is required by this story.
- Use docs/formatting.md as canonical formatting policy and do not create a competing policy in the shared standards artifact.
- If docs/plans/shared-implementation-standards.md is used, keep it suitable for downstream direct references and charter-epic attachment.

Non-blocking notes
- The existing Follow-Up Questions are framed as later governance/provider/CI/reference cleanup and are not unresolved Open Questions.

Split recommendations
- No additional split is needed before developer handoff; the contract already records two child tickets and keeps provider-specific conventions, CI wiring, and project-layout reconciliation as future separate work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment