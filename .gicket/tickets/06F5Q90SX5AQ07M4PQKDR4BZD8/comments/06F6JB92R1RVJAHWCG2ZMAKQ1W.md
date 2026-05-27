[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F5Q90SX5AQ07M4PQKDR4BZD8`.
- Role `po-critic` completed with outcome `po-critic-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `1`; dropped obsolete follow-up(s): `1`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `e0215a9a92e5443dac79247884bd62f9`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F5Q91DR1555RSBQT7KDST684` via `blocks` path `06F5Q90SX5AQ07M4PQKDR4BZD8 -> 06F5Q91DR1555RSBQT7KDST684`
- [dropped] `blocked-by-follow-up-comment` -> `06F5Q90KC6JGQPSP285XQYSPK8` via `blocks` path `06F5Q90SX5AQ07M4PQKDR4BZD8 -> 06F5Q90KC6JGQPSP285XQYSPK8`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F5Q90SX5AQ07M4PQKDR4BZD8` owner `ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re` base `develop` source-owner `ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F5Q91DR1555RSBQT7KDST684` owner `ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma` base `develop` source-owner `ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re`: Mutation targets 'ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma', not current branch 'ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06F5Q90KC6JGQPSP285XQYSPK8` owner `develop` base `develop` source-owner `ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F5Q91DR1555RSBQT7KDST684` on owner branch `ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma` after that branch is refreshed/rebased.