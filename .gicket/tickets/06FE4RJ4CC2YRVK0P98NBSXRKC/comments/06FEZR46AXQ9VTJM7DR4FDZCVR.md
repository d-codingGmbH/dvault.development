[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06FE4RJ4CC2YRVK0P98NBSXRKC`.
- Role `po` completed with outcome `po-refinement-ready` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; dropped obsolete follow-up(s): `1`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `f6565b93a4934163ac4bd1a24db490b9`

Action plan
- [queued] `blocked-follow-up-comment` -> `06FE4RJD5Z6MWC2E66YB3EZ5YW` via `blocks` path `06FE4RJ4CC2YRVK0P98NBSXRKC -> 06FE4RJD5Z6MWC2E66YB3EZ5YW`
- [queued] `blocked-follow-up-comment` -> `06FE4RK80ZXGCZ62CMSAYP164W` via `blocks` path `06FE4RJ4CC2YRVK0P98NBSXRKC -> 06FE4RK80ZXGCZ62CMSAYP164W`
- [dropped] `blocked-by-follow-up-comment` -> `06FE4RBK2MJBS5K3C15JTB8Z9W` via `blocks` path `06FE4RJ4CC2YRVK0P98NBSXRKC -> 06FE4RBK2MJBS5K3C15JTB8Z9W`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06FE4RJ4CC2YRVK0P98NBSXRKC` owner `ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena` base `develop` source-owner `ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06FE4RJD5Z6MWC2E66YB3EZ5YW` owner `ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r` base `develop` source-owner `ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena`: Mutation targets 'ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r', not current branch 'ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena'; queue for target-branch replay.
- [queue-for-owner-branch] `relation-audit-follow-up` `06FE4RK80ZXGCZ62CMSAYP164W` owner `ticket/06FE4RK80ZXGCZ62CMSAYP164W-task-evaluate-bridge-rebuild-push-down-feasibili` base `develop` source-owner `ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena`: Mutation targets 'ticket/06FE4RK80ZXGCZ62CMSAYP164W-task-evaluate-bridge-rebuild-push-down-feasibili', not current branch 'ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06FE4RBK2MJBS5K3C15JTB8Z9W` owner `develop` base `develop` source-owner `ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06FE4RJD5Z6MWC2E66YB3EZ5YW` on owner branch `ticket/06FE4RJD5Z6MWC2E66YB3EZ5YW-task-add-dry-run-sql-shape-diagnostics-for-pit-r` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06FE4RK80ZXGCZ62CMSAYP164W` on owner branch `ticket/06FE4RK80ZXGCZ62CMSAYP164W-task-evaluate-bridge-rebuild-push-down-feasibili` after that branch is refreshed/rebased.