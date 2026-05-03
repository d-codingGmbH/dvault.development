[gicket-bot] PO refinement contract

Summary
- Refined the story into a manual coordinated NuGet release gate for the six-package DVault family, with explicit pre-publish evidence, approval controls, and source-based pre-publication guidance.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 release gate covers exactly six packable packages: DCoding.Data.DVault plus MySql, Oracle, Postgres, Sqlite, and SqlServer; the non-packable src/DCoding.Data project is explicitly out of publication scope.
- Manual publication remains the only supported path in this story; no package push may occur until final publish approval is recorded.
- The current documentation and example baseline is the repository's source-consumption guidance and README quickstart content; live NuGet install commands and versioned package examples remain post-publication follow-up work.
- Local pre-publish evidence is bounded to the repo-root validation flow already defined in the repository: build, test, release pack, package verification, and formatting verification against the same checkout and intended release version.

Scope In
- Define the coordinated release gate for the six-package DVault NuGet family as one synchronized publication unit.
- Document the required pre-publish evidence, including build, test, release pack, package verification, formatting verification, and auditable release-note review.
- Define package validation expectations for each packable package, including aligned versions, dependency alignment, readme presence, XML docs, symbols, and exclusion of unintended test/helper/benchmark packages.
- Document manual release steps, publish-order and stop-condition expectations, and the approval boundary before the first package push.
- Preserve and reference the current source/project-reference consumer guidance as the pre-publication baseline.

Scope Out
- Adding CI/CD publish automation, release credentials, secret handling, or package push tooling.
- Changing product code, provider implementations, or NuGet metadata beyond what is needed to describe the release gate.
- Publishing only a subset of the six-package family or redefining the coordinated release as provider-by-provider.
- Introducing live NuGet installation instructions or versioned dotnet add package examples before the packages are publicly published.
- Treating the non-packable src/DCoding.Data anchor project as a publication artifact.

Open questions
- none

Follow-up questions
- After the packages are publicly available, what NuGet-first installation guidance and versioned examples should replace or supplement the current source-based README instructions?
- Should a later story automate the same validated manual release gate in CI while preserving the explicit human approval boundary before package push?
- Does the team want a separate post-MVP artifact for public-facing release notes or changelog publication beyond the auditable internal release evidence required here?

Risks
- Because publication remains manual across six coordinated packages, a missed checklist step or partial-family push would create version and dependency drift unless the documented gate is followed strictly.
- If package verification does not actually inspect all required artifacts for every package, the release gate could appear complete while still shipping incomplete or unintended package contents.
- Documentation drift between the manual publication guide and README consumer guidance could confuse maintainers about whether source-based setup or NuGet-based setup is currently supported.

Split recommendations
- If release credential handling, package push tooling, or CI-driven publication is needed, schedule that as a separate follow-on story after the manual release gate is accepted.
- If public NuGet consumer documentation is needed immediately after first publication, schedule a separate documentation story for post-publication installation guidance and examples.

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