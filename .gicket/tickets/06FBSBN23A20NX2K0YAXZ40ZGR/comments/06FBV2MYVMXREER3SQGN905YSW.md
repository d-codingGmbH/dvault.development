[gicket-bot] PO refinement contract

Summary
- Ratified the target-specific dependency-line policy from current repository evidence, updated docs/plans/shared-implementation-standards.md on the approved planning surface, and reduced the remaining work to current-baseline documentation alignment.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already answers the policy question: each resolved target stays on the matching EF Core major line (`net8.0` -> EF Core 8, `net10.0` -> EF Core 10) instead of taking newer cross-major packages just because the target framework can restore them.
- Patch updates may move within the selected major line to the latest accepted repository baseline; the visible current baseline is provider-neutral EF `8.0.28` / Relational `8.0.28` / DI.Abstractions `8.0.2` for `net8.0` and `10.0.9` / `10.0.9` / `10.0.9` for `net10.0`.
- The old net8 cross-line posture is no longer the live baseline: net8 integration coverage now uses `MySql.EntityFrameworkCore` `8.0.26`, while net10 keeps `10.0.7`.
- Materialized planning update: `docs/plans/shared-implementation-standards.md` now codifies the current v0.36 dependency-line policy and version matrix on an approved planning surface.
- Live ticket relations still show outbound `blocks` links from this story to done tasks `06FBSBVGFERJGFF74Y5FC3G7B8` and `06FBSBVPAS4XV801DN3J8J3R40`; treat them as historical implementation evidence, not blockers for PO review.

Scope In
- Codify that each DVault consumer package line follows the matching EF Core major line for its target framework and may only advance patches within that line.
- Keep project files, matrix tests, package verification expectations, and current-baseline documentation consistent with the accepted target-specific dependency matrix.
- Explicitly capture the current accepted net8 and net10 baselines, including the visible MySQL provider versions per target.

Scope Out
- Rewriting historical v0.33-v0.35 release notes that describe earlier accepted baselines.
- Publishing packages, changing release automation, or changing runtime behavior beyond dependency references and documentation.
- Adding new provider capabilities such as DB2 live-schema support.

Open questions
- none

Follow-up questions
- Should the same delivery step that closes this story also update `docs/releases/v0.36.0.md` and `docs/production-adoption-checklist.md`, which still describe the older `8.0.27` / `10.0.8` matrix and the former cross-line MySQL exception?

Risks
- If current-baseline release notes and adoption guidance are not synced to the ratified policy, consumers and release operators will see contradictory dependency guidance even though code, tests, and verifier expectations already agree.
- Because live relation metadata still uses `blocks` for already-done implementation tasks, any workflow that interprets those links literally could misread delivery state until the relations are cleaned up.

Split recommendations
- No new split is needed. The repository already shows the net8 alignment task `06FBSBVGFERJGFF74Y5FC3G7B8` and the net10 alignment task `06FBSBVPAS4XV801DN3J8J3R40` as done; the remaining work is current-baseline documentation consistency.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment