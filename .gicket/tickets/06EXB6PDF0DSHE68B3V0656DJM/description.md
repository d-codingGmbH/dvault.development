<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the persisted ticket, comments, relations, attachment state, and visible repository baseline. The ticket is ready for PO critic with a bounded v1 enforcement plan: repository-level formatting defaults via EditorConfig plus local and CI check entrypoints that are independent of any future application stack.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The ticket has no human refinement comments and no attached documents at this time; the referenced repository metadata and branch snapshot are treated as the current planning evidence.
- The ticket is already a child of 06EXB6NWYVB37D7S74VB3PVTCC through an incoming parentOf relation, so no additional split or relation write is needed for this refinement.
- The visible repository baseline contains only project governance metadata and no source roots, test roots, build manifest, or CI workflow yet; v1 formatting enforcement should therefore be repository-level and bootstrap-friendly rather than tied to a specific language stack.

### Scope In
- Define repository formatting standards for two-space indentation, LF line endings, UTF-8 text encoding, final newline behavior, trailing whitespace handling, tab rejection, and same-line opening braces where the target file type has brace syntax.
- Add or specify a root EditorConfig policy that applies to repository text files and establishes the default formatting contract for future source, test, docs, configuration, and workflow files.
- Define a local formatting verification command or script that developers can run before committing and that fails on violations instead of silently accepting drift.
- Define the CI/build-time formatting gate so the same checks run automatically once CI/build infrastructure exists, without depending on application-specific source roots that are not present in the current branch.
- Document any intentional exceptions for generated, binary, lock, or vendor artifacts so the enforcement surface is clear and bounded.

### Scope Out
- Choosing the final application runtime, language framework, or source/test directory layout.
- Implementing product code, application build behavior, or feature-specific formatting rules beyond the shared repository policy.
- Reformatting large future code surfaces that do not exist in the current repository baseline.
- Adding broad linting, static analysis, security scanning, or style rules unrelated to the stated formatting requirements.

## Acceptance Criteria
- A root formatting policy is defined for text files with indent_style=space, indent_size=2, end_of_line=lf, charset=utf-8, insert_final_newline=true, and trailing whitespace trimming where appropriate.
- Tabs in governed text files are rejected by an explicit check unless a documented file-type exception requires tabs.
- Same-line opening brace style is covered by the enforcement plan for brace-based file types, using formatter or checker configuration appropriate to the eventual file type rather than ad hoc manual review.
- The enforcement design includes one local developer command and one CI/build-time gate that use the same rule source or produce equivalent results.
- The plan remains valid for the current repository baseline, which has no source roots, test roots, build manifest, or CI workflow yet, and describes how future source/test files inherit the policy.

## Definition of Done
- Formatting enforcement requirements are captured in the ticket contract or an approved planning artifact with enough detail for implementation without reopening baseline decisions.
- The implementation path includes EditorConfig plus an automated verification mechanism for local and CI use.
- Exceptions are explicitly scoped to generated, binary, lock, vendor, or tool-required files and do not weaken the default text-file policy.
- A developer can determine from the ticket which repository-level files or scripts to add and what behavior must fail the check.
- No unresolved PO-level blockers remain for PO critic review.

## Implementation Notes
- Use the current empty source/test baseline as the v1 default: place enforcement at repository root so future project files inherit it.
- Prefer .editorconfig as the canonical editor-facing rule source, with a repository-local check script or tool configuration for rules EditorConfig alone cannot reliably enforce, especially tab rejection and brace placement.
- The local check should be non-mutating by default for CI parity; an optional formatter/fix command may be added separately if the chosen toolchain supports it.
- Because there is no build manifest yet, the CI/build-time gate can initially be specified as a named script or documented command to be wired into the first CI workflow/build definition when that ticket exists.
- Do not depend on .gicket operational metadata as a product formatting source; those files were used only as planning evidence and repository baseline context.

## Open Questions
- none

## Follow-Up Questions
- When the application stack is introduced, decide whether to add language-specific formatter integrations such as dotnet format, Prettier, clang-format, or an equivalent tool while preserving the repository-level defaults.
- When CI infrastructure is added, bind the formatting check into the concrete workflow and decide whether violations should block all branches or only protected branch merges.
- If generated or vendor directories are later introduced, confirm their exact ignore patterns before broad formatting scans are enabled.

## Risks
- EditorConfig alone may not enforce same-line opening braces or fail builds, so implementation must include an automated checker or formatter configuration beyond editor hints.
- Future language-specific formatters may have brace or indentation defaults that conflict with the repository standard if they are introduced without updating the shared formatting policy.
- Running a future formatting check across bot-operational or generated files could create noisy failures unless exceptions are clearly maintained.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Specify enforcement for two-space indentation, LF endings, UTF-8, and same-line opening braces.

## Scope
- Plan EditorConfig and build-time checks for formatting requirements.

## Acceptance Criteria
- Formatting rules are enforceable locally and in CI.
- Tabs are rejected or normalized to spaces.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.