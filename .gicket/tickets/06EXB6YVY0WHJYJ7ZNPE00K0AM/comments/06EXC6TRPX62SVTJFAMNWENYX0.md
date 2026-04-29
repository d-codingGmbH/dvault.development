[gicket-bot] PO refinement contract

Summary
- Refined the packaging task to target the current DVault source and test layout, with XML documentation, deterministic build settings, SourceLink, and locally inspectable symbols in scope. No blocking PO questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 scope applies to the visible repository baseline: the main source project under src/DVault and related validation through tests/DVault.Tests.
- XML documentation should be generated for packageable build output, and missing documentation on public/protected APIs should be surfaced by the build as warnings or errors according to the repository's existing warning policy.
- Deterministic and reproducible package-support settings include deterministic compilation and SourceLink where supported by the current project type and package tooling.

Scope In
- Enable XML documentation output for the packageable DVault project build.
- Configure deterministic build settings appropriate for the existing .NET project structure.
- Configure SourceLink/package metadata needed for source-linked symbol packages when supported by the repository layout.
- Ensure local package/symbol output can be produced and inspected by a developer without publishing externally.
- Keep changes aligned with existing repository-level build configuration conventions if shared build props/targets already exist.

Scope Out
- Publishing packages or symbols to an external feed.
- Changing public API shape solely to satisfy documentation warnings.
- Adding broad documentation content beyond what is needed to make the configured warnings actionable.
- Introducing a multi-project packaging strategy beyond the current visible src/DVault and tests/DVault.Tests layout.
- Changing workflow labels or ticket status as part of implementation.

Open questions
- none

Follow-up questions
- Decide later whether XML documentation warnings should become hard errors across all projects once additional projects are added.
- Decide later whether package verification should be automated in CI after the repository's CI workflow exists.
- Decide later whether separate public API documentation quality standards are needed beyond compiler XML documentation coverage.

Risks
- SourceLink configuration may depend on the eventual repository host/remote metadata; if that metadata is absent locally, implementation should configure the standard settings and document the verification limit.
- Enforcing missing documentation warnings too aggressively could surface existing undocumented APIs; the implementation should avoid broad API changes and document only what is necessary for this packaging baseline.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment