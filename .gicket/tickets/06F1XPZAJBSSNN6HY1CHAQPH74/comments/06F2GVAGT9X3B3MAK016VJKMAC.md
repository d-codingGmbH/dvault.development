[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F1XPZAJBSSNN6HY1CHAQPH74`.
- Role `po` completed with outcome `po-refinement-ready` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `17b5b5314d4249608660266b02b780e7`

Action plan
- [queued] `blocked-by-follow-up-comment` -> `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` via `blocks` path `06F1XPZAJBSSNN6HY1CHAQPH74 -> 06F1XPRY3ZDB6W1WQ9ABRRJ2V4`
- [queued] `child-follow-up-comment` -> `06F1XPZS9SNK93JNKC02B63QG4` via `parentOf` path `06F1XPZAJBSSNN6HY1CHAQPH74 -> 06F1XPZS9SNK93JNKC02B63QG4`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F1XPZAJBSSNN6HY1CHAQPH74` owner `ticket/06F1XPZAJBSSNN6HY1CHAQPH74-story-add-opt-in-load-metadata-interceptors` base `develop` source-owner `ticket/06F1XPZAJBSSNN6HY1CHAQPH74-story-add-opt-in-load-metadata-interceptors`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` owner `ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails` base `develop` source-owner `ticket/06F1XPZAJBSSNN6HY1CHAQPH74-story-add-opt-in-load-metadata-interceptors`: Target ticket owner branch 'ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails' differs from source owner branch 'ticket/06F1XPZAJBSSNN6HY1CHAQPH74-story-add-opt-in-load-metadata-interceptors'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F1XPZS9SNK93JNKC02B63QG4` owner `ticket/06F1XPZS9SNK93JNKC02B63QG4-task-implement-savechanges-metadata-interceptor` base `develop` source-owner `ticket/06F1XPZAJBSSNN6HY1CHAQPH74-story-add-opt-in-load-metadata-interceptors`: Target ticket owner branch 'ticket/06F1XPZS9SNK93JNKC02B63QG4-task-implement-savechanges-metadata-interceptor' differs from source owner branch 'ticket/06F1XPZAJBSSNN6HY1CHAQPH74-story-add-opt-in-load-metadata-interceptors'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` on owner branch `ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `child-follow-up-target` to `06F1XPZS9SNK93JNKC02B63QG4` on owner branch `ticket/06F1XPZS9SNK93JNKC02B63QG4-task-implement-savechanges-metadata-interceptor` after that branch is refreshed/rebased.