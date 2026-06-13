[gicket-bot] PO refinement contract

Summary
- Refined the story to match the repo-visible v0.36 dependency matrix: code, tests, and package verification already enforce the target-matched EF lines, while docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md still need alignment.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now treats docs/plans/shared-implementation-standards.md as remaining in-scope work, not landed evidence. Repository evidence shows that file still carries the older 8.0.27 / 10.0.8 matrix and a cross-target MySQL 10.0.7 exception, while the source, test, and verifier baseline already moved to 8.0.28 / 10.0.9 with net8 MySQL 8.0.26.
- critic-item-2: `answered` - Current-baseline documentation scope is explicit for this story: update docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md. README.md, docs/manual-nuget-publication.md, and docs/local-validation.md already align with the dual package-line policy and are not reopened unless contradictory evidence appears during delivery.
- critic-item-3: `answered` - The remaining-work statement is now unambiguous: do not redesign the dependency matrix. The branch already landed the target-matched policy in project files, matrix tests, and PackageVerifier expectations; the delivery work left for this story is documentation alignment across the three named current-baseline surfaces.
- critic-item-4: `answered` - The blocking finding is resolved at contract level by moving docs/releases/v0.36.0.md and docs/production-adoption-checklist.md out of Follow-Up Questions and into explicit in-scope work with concrete acceptance criteria tied to the repo-visible versions.

Clarifications
- The repository already answers the policy question: each target framework follows the matching EF Core major line, and a resolved target must not mix 8.x and 10.x dependency lines.
- The current visible baseline is net8 EF 8.0.28 / Relational 8.0.28 / DI.Abstractions 8.0.2 with DB2 8.0.0.400, SQLite 8.0.28, MySQL 8.0.26, PostgreSQL 8.0.11, Oracle 8.<redacted>, SQL Server 8.0.28; and net10 EF 10.0.9 / Relational 10.0.9 / DI.Abstractions 10.0.9 with DB2 10.0.0.100, SQLite 10.0.9, MySQL 10.0.7, PostgreSQL 10.0.2, Oracle <redacted>, SQL Server 10.0.9.
- docs/plans/shared-implementation-standards.md has not yet been updated on this branch, so the contract keeps that planning surface in scope instead of treating it as landed evidence.
- README.md, docs/manual-nuget-publication.md, and docs/local-validation.md already reflect the dual 8.36.0 / 10.36.0 package-line posture and do not need new scope unless contradictory evidence is found.
- No child-ticket split or additional planning artifact is justified from the current branch evidence; the remaining work is a bounded documentation-alignment pass.

Scope In
- Update docs/plans/shared-implementation-standards.md V0.36 Compatibility Contract to the repo-visible net8/net10 dependency matrix and target-matched major-line rule.
- Update docs/releases/v0.36.0.md so the Compatibility Matrix and explanatory text use the current 8.0.28 / 10.0.9 baselines and net8 MySQL 8.0.26 instead of the carried-forward 8.0.27 / 10.0.8 and cross-target MySQL 10.0.7 story.
- Update docs/production-adoption-checklist.md so its v0.36 baseline bullets describe the same target-specific matrix and do not present MySQL 10.0.7 as general mixed-line permission.
- Use the current project files, matrix tests, and PackageVerifier expectations as the authoritative evidence source for those documentation updates.

Scope Out
- Re-opening the dependency-line policy itself; the branch already ratifies target-matched EF Core major lines.
- Changing package references, matrix tests, or PackageVerifier to a different version policy unless new contradictory repository evidence is found.
- Rewriting historical v0.33 through v0.35 release notes or other intentionally historical documentation.
- Updating README.md, docs/manual-nuget-publication.md, or docs/local-validation.md absent newly discovered contradictions, because those surfaces already align with the repo-visible package-line policy.

Open questions
- none

Follow-up questions
- none

Risks
- Until the three named documentation surfaces are aligned, release operators and consumers will continue to see guidance that contradicts the already-landed project, test, and package-verifier baseline.
- If historical done-task blocks relations remain in live ticket metadata, automation or humans may misread delivery state even after the documentation work is complete.

Split recommendations
- No split recommended. The remaining work is a bounded documentation-alignment pass across docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment