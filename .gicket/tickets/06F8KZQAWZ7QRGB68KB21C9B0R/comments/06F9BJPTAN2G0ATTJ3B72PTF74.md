[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F8KZQAWZ7QRGB68KB21C9B0R`.
- Role `po-critic` completed with outcome `po-critic-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; dropped obsolete follow-up(s): `1`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `97397827906348888c658e04dd81ec71`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F8KZP0VKMXGE0JXPZRD1RQDG` via `blocks` path `06F8KZQAWZ7QRGB68KB21C9B0R -> 06F8KZP0VKMXGE0JXPZRD1RQDG`
- [queued] `blocked-follow-up-comment` -> `06F8KZR38EDSVZBCTC0XYR4R80` via `blocks` path `06F8KZQAWZ7QRGB68KB21C9B0R -> 06F8KZR38EDSVZBCTC0XYR4R80`
- [dropped] `blocked-by-follow-up-comment` -> `06F8KZPZZE8VZEBANP5MPN8HH8` via `blocks` path `06F8KZQAWZ7QRGB68KB21C9B0R -> 06F8KZPZZE8VZEBANP5MPN8HH8`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F8KZQAWZ7QRGB68KB21C9B0R` owner `ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum` base `develop` source-owner `ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZP0VKMXGE0JXPZRD1RQDG` owner `ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag` base `develop` source-owner `ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum`: Mutation targets 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag', not current branch 'ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum'; queue for target-branch replay.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZR38EDSVZBCTC0XYR4R80` owner `ticket/06F8KZR38EDSVZBCTC0XYR4R80-story-define-performance-decision-tree-contract` base `develop` source-owner `ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum`: Mutation targets 'ticket/06F8KZR38EDSVZBCTC0XYR4R80-story-define-performance-decision-tree-contract', not current branch 'ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06F8KZPZZE8VZEBANP5MPN8HH8` owner `develop` base `develop` source-owner `ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZP0VKMXGE0JXPZRD1RQDG` on owner branch `ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZR38EDSVZBCTC0XYR4R80` on owner branch `ticket/06F8KZR38EDSVZBCTC0XYR4R80-story-define-performance-decision-tree-contract` after that branch is refreshed/rebased.