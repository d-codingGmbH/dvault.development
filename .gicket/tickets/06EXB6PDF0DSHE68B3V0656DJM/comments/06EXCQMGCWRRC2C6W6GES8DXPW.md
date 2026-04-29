[gicket-bot] PO refinement contract

Summary
- Verified the persisted ticket, comments, relations, attachment state, and visible repository baseline. The ticket is ready for PO critic with a bounded v1 enforcement plan: repository-level formatting defaults via EditorConfig plus local and CI check entrypoints that are independent of any future application stack.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The ticket has no human refinement comments and no attached documents at this time; the referenced repository metadata and branch snapshot are treated as the current planning evidence.
- The ticket is already a child of 06EXB6NWYVB37D7S74VB3PVTCC through an incoming parentOf relation, so no additional split or relation write is needed for this refinement.
- The visible repository baseline contains only project governance metadata and no source roots, test roots, build manifest, or CI workflow yet; v1 formatting enforcement should therefore be repository-level and bootstrap-friendly rather than tied to a specific language stack.

Scope In
- Define repository formatting standards for two-space indentation, LF line endings, UTF-8 text encoding, final newline behavior, trailing whitespace handling, tab rejection, and same-line opening braces where the target file type has brace syntax.
- Add or specify a root EditorConfig policy that applies to repository text files and establishes the default formatting contract for future source, test, docs, configuration, and workflow files.
- Define a local formatting verification command or script that developers can run before committing and that fails on violations instead of silently accepting drift.
- Define the CI/build-time formatting gate so the same checks run automatically once CI/build infrastructure exists, without depending on application-specific source roots that are not present in the current branch.
- Document any intentional exceptions for generated, binary, lock, or vendor artifacts so the enforcement surface is clear and bounded.

Scope Out
- Choosing the final application runtime, language framework, or source/test directory layout.
- Implementing product code, application build behavior, or feature-specific formatting rules beyond the shared repository policy.
- Reformatting large future code surfaces that do not exist in the current repository baseline.
- Adding broad linting, static analysis, security scanning, or style rules unrelated to the stated formatting requirements.

Open questions
- none

Follow-up questions
- When the application stack is introduced, decide whether to add language-specific formatter integrations such as dotnet format, Prettier, clang-format, or an equivalent tool while preserving the repository-level defaults.
- When CI infrastructure is added, bind the formatting check into the concrete workflow and decide whether violations should block all branches or only protected branch merges.
- If generated or vendor directories are later introduced, confirm their exact ignore patterns before broad formatting scans are enabled.

Risks
- EditorConfig alone may not enforce same-line opening braces or fail builds, so implementation must include an automated checker or formatter configuration beyond editor hints.
- Future language-specific formatters may have brace or indentation defaults that conflict with the repository standard if they are introduced without updating the shared formatting policy.
- Running a future formatting check across bot-operational or generated files could create noisy failures unless exceptions are clearly maintained.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment