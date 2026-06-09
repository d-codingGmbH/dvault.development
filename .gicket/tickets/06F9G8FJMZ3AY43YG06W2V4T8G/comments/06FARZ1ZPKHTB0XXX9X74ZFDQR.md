[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already fixes the v0.33 package-line contract: keep the seven package ids unchanged, use 8.33.0 for net8.0 and EF Core 8, use 10.33.0 for net10.0 and EF Core 10, do not document a consumer-facing 0.33.0 package version, and do not mix both lines in one consumer example or install path.
- README.md and src/DCoding.Data.DVault.Analyzers/README.md already carry dual-line install examples and analyzer PrivateAssets=all guidance, so this ticket should not reopen installation-snippet policy; it should align the remaining baseline, release-note, compatibility, and limitation prose around that already-landed contract.
- docs/manual-nuget-publication.md already treats v0.33 as a dual-line manual-publication release and remains the authoritative publish-flow document; this ticket should reference and stay consistent with that manual boundary rather than inventing publish automation.
- Current repository evidence still lacks docs/releases/v0.33.0.md and still names v0.32.0 as the current baseline in README.md and docs/production-adoption-checklist.md, so the bounded remaining documentation gap is current-baseline rollover rather than package-version selection.
- The supported provider/version evidence is finite and already visible in docs/plans/shared-implementation-standards.md plus tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs; document that matrix as the v1 default instead of reopening provider-version choices.
- Default repository validation already separates package-tested local evidence from external-provider opt-in evidence: build, test, pack, verify-packages, check-format, required SQLite-backed local integration, provider registration smoke coverage, and opt-in Postgres, SQL Server, Oracle, and MySQL live database lanes behind DVAULT_TEST_* connection-string gates.
- The ticket snapshot was stale on comment facts: the live ticket has only bot claim/lease comments and no human scope comments.
- Live relation context is coherent: epic 06F9G8EE7ZA666MW8YEB2QP8BW is the parent via parentOf, this task still blocks that epic, and done task 06F9G8FBQTAPXXS1Y4NR5QKVG8 still appears as a blocking relation but should be treated as completed prerequisite evidence, not a remaining blocker.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass because existing repository and ticket evidence already bounded the work.

Scope In
- Make v0.33.0 the current documentation baseline by adding docs/releases/v0.33.0.md and updating README.md plus docs/production-adoption-checklist.md references, compatibility guidance, and limitations wording that still point at v0.32.0.
- Document the supported consumer package/version matrix for v0.33.0: 8.33.0 with net8.0 and EF Core 8, 10.33.0 with net10.0 and EF Core 10, unchanged package ids, no consumer-facing 0.33.0 package line, and no mixed-line consumer installs.
- Document the bounded provider/version evidence behind those two lines, including the finite provider matrix already codified in shared standards and version-matrix tests, rather than leaving provider/version combinations open-ended.
- Explain package-update and adoption caveats: choose one package line per consumer project, keep runtime/provider/analyzer packages aligned to that line, keep the analyzer local with PrivateAssets=all, and treat MySql.EntityFrameworkCore 10.0.7 as the documented evidence exception rather than general mixed-line permission.
- State what is package-tested or default-local versus what remains external-provider opt-in, using the existing xUnit categories, pack plus verify-packages gate, SQLite local baseline, and DVAULT_TEST_* live provider lanes.
- Refresh the v0.33 limitations and non-goals so the docs explicitly say there is no new runtime behavior, no provider provisioning, no standalone platform tooling or CLI addition, no release automation, and no new default requirement for live external databases.

Scope Out
- Changing DVault runtime behavior, provider strategy behavior, supported provider set, package ids, target frameworks, or EF Core dependency selection logic.
- Reopening package verifier, CI workflow, or manual publication implementation work already bounded by done task 06F9G8FBQTAPXXS1Y4NR5QKVG8 except where this ticket must reference that landed contract.
- Retargeting helper projects such as DCoding.Data.DVault.Analyzers, tools/DCoding.Data.DVault.PackageVerification, or src/DCoding.Data beyond the already-selected v0.33 compatibility scope.
- Provisioning Docker, databases, users, credentials, schemas, or other external platform/tooling assets for provider validation lanes.
- Broad cross-document cleanup outside the README, release-note, checklist, compatibility, and limitations surfaces needed to make the v0.33 compatibility story consistent.

Open questions
- none

Follow-up questions
- After v0.33 compatibility docs land, should topic-specific documents that still intentionally cite older releases as their topic baseline be audited separately, or is the bounded README, release-note, and checklist rollover sufficient for this release?
- Once publication actually occurs, should a release approval record or attachment capture final published package links and hashes separately from the planning release note, per the manual publication checklist?
- If future consumer docs want broader provider-package selection examples or multi-SDK support narratives beyond the current bounded compatibility matrix, should that be scheduled as a separate post-v0.33 documentation follow-up?

Risks
- README install snippets are already dual-line but README and production-checklist baseline references still point at v0.32.0, so partial edits can leave contradictory current-baseline messaging even if the version examples look correct.
- If the v0.33 prose does not clearly separate package-tested and default-local evidence from external-provider opt-in database runs, the documentation can overstate repository proof or imply that external databases are mandatory in the default validation path.
- The MySQL 10.0.7 provider pin across both target lines can be misread as general permission for mixed dependency lines unless the docs call it out as a bounded evidence exception.
- Because the live relation set still includes a historical blocks edge from done ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 into this task, downstream readers may misread the dependency state unless the refinement contract explicitly treats that relation as completed prerequisite context.

Split recommendations
- No additional split is recommended: done task 06F9G8FBQTAPXXS1Y4NR5QKVG8 already isolated verifier, CI, and manual-release guidance, and this ticket remains the bounded home for broader compatibility prose, release-note rollover, and limitation updates.
- If broader cross-document baseline normalization is desired beyond the README, release note, production checklist, and closely linked compatibility surfaces, schedule that as a later documentation follow-up instead of expanding this ticket.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment