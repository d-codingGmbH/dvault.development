[gicket-bot] relation automation follow-up

Summary
- Evaluated `1` selected relation flow(s) for source ticket `06F8KZSNDXXEEHF53HN14QFK14`.
- Role `po-critic` completed with outcome `po-critic-non-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `1`; dropped obsolete follow-up(s): `0`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `e7a791e3c3bc473682d0a875daabe571`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F8KZSYCVZ21MS983501BZG18` via `blocks` path `06F8KZSNDXXEEHF53HN14QFK14 -> 06F8KZSYCVZ21MS983501BZG18`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F8KZSNDXXEEHF53HN14QFK14` owner `ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with` base `develop` source-owner `ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZSYCVZ21MS983501BZG18` owner `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release` base `develop` source-owner `ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with`: Mutation targets 'ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release', not current branch 'ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with'; queue for target-branch replay.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZSYCVZ21MS983501BZG18` on owner branch `ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release` after that branch is refreshed/rebased.