[gicket-bot] relation automation follow-up

Summary
- Evaluated `1` selected relation flow(s) for source ticket `06F1XQ25KK4VY4MYJSDG9V4BZM`.
- Role `po` completed with outcome `po-refinement-ready` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `b1de8d46be3b436fac121b90754d36ee`

Action plan
- [queued] `blocked-by-follow-up-comment` -> `06F1XQ03MADSPQD0AJN6R50D44` via `blocks` path `06F1XQ25KK4VY4MYJSDG9V4BZM -> 06F1XQ03MADSPQD0AJN6R50D44`
- [queued] `blocked-by-follow-up-comment` -> `06F1XPX99KQRB09GRQG50Z75FM` via `blocks` path `06F1XQ25KK4VY4MYJSDG9V4BZM -> 06F1XPX99KQRB09GRQG50Z75FM`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F1XQ25KK4VY4MYJSDG9V4BZM` owner `ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample` base `develop` source-owner `ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F1XQ03MADSPQD0AJN6R50D44` owner `ticket/06F1XQ03MADSPQD0AJN6R50D44-story-add-optional-provider-bulk-insert-strategy` base `develop` source-owner `ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample`: Target ticket owner branch 'ticket/06F1XQ03MADSPQD0AJN6R50D44-story-add-optional-provider-bulk-insert-strategy' differs from source owner branch 'ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F1XPX99KQRB09GRQG50Z75FM` owner `ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics` base `develop` source-owner `ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample`: Target ticket owner branch 'ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics' differs from source owner branch 'ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F1XQ03MADSPQD0AJN6R50D44` on owner branch `ticket/06F1XQ03MADSPQD0AJN6R50D44-story-add-optional-provider-bulk-insert-strategy` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F1XPX99KQRB09GRQG50Z75FM` on owner branch `ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics` after that branch is refreshed/rebased.