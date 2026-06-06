[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F8KZSYCVZ21MS983501BZG18`.
- Role `test` completed with outcome `test-workflow-returned` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; dropped obsolete follow-up(s): `2`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `cf1f8e0f23954c959ae467880f8a8550`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F8KZQNH8CCMTJW9P95W1N388` via `blocks` path `06F8KZSYCVZ21MS983501BZG18 -> 06F8KZQNH8CCMTJW9P95W1N388`
- [queued] `blocked-follow-up-comment` -> `06F8KZTNG44XDPMVTVCV4WJSHG` via `blocks` path `06F8KZSYCVZ21MS983501BZG18 -> 06F8KZTNG44XDPMVTVCV4WJSHG`
- [dropped] `blocked-by-follow-up-comment` -> `06F8KZSCGZBKAC4YZH5SY3NX68` via `blocks` path `06F8KZSYCVZ21MS983501BZG18 -> 06F8KZSCGZBKAC4YZH5SY3NX68`
- [dropped] `blocked-by-follow-up-comment` -> `06F8KZSNDXXEEHF53HN14QFK14` via `blocks` path `06F8KZSYCVZ21MS983501BZG18 -> 06F8KZSNDXXEEHF53HN14QFK14`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F8KZSYCVZ21MS983501BZG18` owner `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release` base `develop` source-owner `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZQNH8CCMTJW9P95W1N388` owner `ticket/06F8KZQNH8CCMTJW9P95W1N388-epic-performance-decision-guidance-and-observabi` base `develop` source-owner `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release`: Mutation targets 'ticket/06F8KZQNH8CCMTJW9P95W1N388-epic-performance-decision-guidance-and-observabi', not current branch 'ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release'; queue for target-branch replay.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZTNG44XDPMVTVCV4WJSHG` owner `ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a` base `develop` source-owner `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release`: Mutation targets 'ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a', not current branch 'ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06F8KZSCGZBKAC4YZH5SY3NX68` owner `develop` base `develop` source-owner `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F8KZSNDXXEEHF53HN14QFK14` owner `develop` base `develop` source-owner `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZQNH8CCMTJW9P95W1N388` on owner branch `ticket/06F8KZQNH8CCMTJW9P95W1N388-epic-performance-decision-guidance-and-observabi` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZTNG44XDPMVTVCV4WJSHG` on owner branch `ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a` after that branch is refreshed/rebased.