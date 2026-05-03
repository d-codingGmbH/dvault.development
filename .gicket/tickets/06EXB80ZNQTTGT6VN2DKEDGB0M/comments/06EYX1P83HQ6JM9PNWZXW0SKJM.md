[gicket-bot] PO refinement contract

Summary
- Verified the parent quality story against the live repository and existing child tickets; the work is already split into three done child tasks covering XML-doc enforcement, package-specific API snapshots, and one-member-per-file enforcement, so the parent story is ready for PO-critic without new planning artifacts.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The story is already decomposed into done child tickets 06EXB817Q8RAXCQH5QQR5RFY34 (XML-doc enforcement), 06EXB81FSWAA6N1HMYQ0CM4S8G (package-specific API snapshot review), and 06EXB81QXE7XJPNM6NTPYCTP1M (one-member-per-file enforcement).
- Repository evidence fixes the v1 package boundary to six packable projects: src/DCoding.Data.DVault, src/DCoding.Data.DVault.MySql, src/DCoding.Data.DVault.Oracle, src/DCoding.Data.DVault.Postgres, src/DCoding.Data.DVault.Sqlite, and src/DCoding.Data.DVault.SqlServer; src/DCoding.Data is explicitly non-packable and out of this story's release-surface scope.
- No new child tickets, relations, attachments, or planning documents were created in this refinement pass because the existing split and repository documents already bound the story.

Scope In
- Enforcing XML documentation generation and missing-doc detection for public and protected APIs in each of the six packable DVault packages.
- Package-aware API surface approval or compatibility checks that keep core and provider package changes separately reviewable.
- One-public-or-protected-top-level-declaration-per-file enforcement for the same six packable source projects, with explicit documented exceptions where retained.

Scope Out
- src/DCoding.Data, test projects, and benchmark projects as direct enforcement targets because they are non-packable or test-only surfaces.
- Provider runtime behavior changes, save semantics, or new public API design beyond documenting and reviewing the existing visible surface.
- Post-v1 release governance such as published-NuGet backward-compatibility policy, broader repository-wide analyzer expansion, or future provider optimization work.

Open questions
- none

Follow-up questions
- After the first public package release, should DVault add a second compatibility check against the last published NuGet versions in addition to the repository-managed package baselines?
- If new packable provider packages or externally published examples are added later, should these gates auto-discover eligible projects or continue to rely on an explicit allowlist update?
- Once the public and protected baseline is stable, should the one-member-per-file policy remain limited to release-surface declarations or expand to internal top-level declarations as well?

Risks
- If shared MSBuild or shell-gate scope is broadened without packable-project conditions, non-packable tests, benchmarks, or build output could start failing on unrelated surfaces.
- A namespace-based or aggregated API snapshot would be misleading because the provider packages share the DCoding.Data.DVault namespace and could hide package-boundary regressions.
- Over-broad one-member-per-file exceptions or stale exception-list entries would weaken the source-layout gate enough for future public API drift to slip through review.

Split recommendations
- No additional split is recommended; the parent story is already bounded by the three existing child tickets for XML-doc enforcement, API snapshot review, and one-member-per-file enforcement.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment