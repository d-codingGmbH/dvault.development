[gicket-bot] PO refinement contract

Summary
- Refined the ticket against the current six-package DVault family, current source-based installation docs, and the existing build/test/pack/package-verification baseline; no split or planning artifact was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence fixes the v1 release family as six packable packages: DCoding.Data.DVault plus DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer; src/DCoding.Data is non-packable and out of publication scope.
- README already establishes source/project-reference consumption as the current baseline and defers live NuGet install commands until packages are published, so this ticket should document that distinction rather than rewrite installation guidance.
- The current validation baseline is already visible in repo docs: dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo, dotnet pack DVault.slnx --configuration Release --nologo, bash tools/verify-packages.sh, and bash tools/check-format.sh.
- Existing relations already place this task under story 06EXB8202A88KJJP7WEGBESBYM and show done upstream blocker tickets 06EXB82RW6PV2NFG088G6BPFHC and 06EXB7TP9PF2XFRQ9MG7CJQR10.

Scope In
- Document one manual release checklist for the coordinated DVault NuGet family rather than per-package publication instructions.
- Document required pre-publish quality evidence, package creation and validation, version alignment, publish ordering, stop conditions, and final publish approval.
- Document how maintainers confirm provider packages reference the aligned core package version before any push.
- Document the current source-consumption guidance versus future post-publication NuGet-first consumer guidance.

Scope Out
- Automating publishing, adding CI/CD release credentials, or introducing package push tooling.
- Changing product code, package metadata, or provider implementation behavior.
- Publishing only a subset of the package family for the planned coordinated release.
- Replacing current README source-installation guidance with live NuGet commands before packages exist.

Open questions
- none

Follow-up questions
- After the first public publication, should a separate ticket switch the README installation section from source/project references to NuGet-first examples while preserving pre-release contributor guidance elsewhere?
- Should a later release-management ticket introduce automation or a machine-readable release checklist once the manual publication flow has stabilized?
- Should the project later standardize a dedicated changelog or release-notes file, or keep release-note recording inside release documentation and ticket artifacts?

Risks
- If the documentation leaves the release-note or changelog location implicit, manual releases may still diverge even though the rest of the checklist is explicit.
- Because publishing remains manual, any checklist that does not force full-family validation before the first push still leaves room for accidental partial publication.
- Future provider-specific release needs could pressure the coordinated family-release rule, so the documentation should state that the current v1 baseline is synchronized publication across all six packages.

Split recommendations
- No split recommended; the work is a single bounded documentation task for the current manual six-package NuGet release process.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment