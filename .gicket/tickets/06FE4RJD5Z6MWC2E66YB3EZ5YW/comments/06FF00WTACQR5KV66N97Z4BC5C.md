[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06FE4RJD5Z6MWC2E66YB3EZ5YW`.
- Role `po` completed with outcome `po-refinement-ready` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; dropped obsolete follow-up(s): `1`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `9c1ef8db218047b0a35b8a191ad23e67`

Action plan
- [queued] `blocked-follow-up-comment` -> `06FE4RJP5KG02DF7AEMCQYGNVW` via `blocks` path `06FE4RJD5Z6MWC2E66YB3EZ5YW -> 06FE4RJP5KG02DF7AEMCQYGNVW`
- [queued] `blocked-follow-up-comment` -> `06FE4RJZ4PA0DZ3HXDSEG2BQMM` via `blocks` path `06FE4RJD5Z6MWC2E66YB3EZ5YW -> 06FE4RJZ4PA0DZ3HXDSEG2BQMM`
- [dropped] `blocked-by-follow-up-comment` -> `06FE4RJ4CC2YRVK0P98NBSXRKC` via `blocks` path `06FE4RJD5Z6MWC2E66YB3EZ5YW -> 06FE4RJ4CC2YRVK0P98NBSXRKC`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06FE4RJD5Z6MWC2E66YB3EZ5YW` owner `ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r` base `develop` source-owner `ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06FE4RJP5KG02DF7AEMCQYGNVW` owner `ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel` base `develop` source-owner `ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r`: Mutation targets 'ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel', not current branch 'ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r'; queue for target-branch replay.
- [queue-for-owner-branch] `relation-audit-follow-up` `06FE4RJZ4PA0DZ3HXDSEG2BQMM` owner `ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel` base `develop` source-owner `ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r`: Mutation targets 'ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel', not current branch 'ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06FE4RJ4CC2YRVK0P98NBSXRKC` owner `develop` base `develop` source-owner `ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06FE4RJP5KG02DF7AEMCQYGNVW` on owner branch `ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06FE4RJZ4PA0DZ3HXDSEG2BQMM` on owner branch `ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel` after that branch is refreshed/rebased.