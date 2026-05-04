[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06EZ0NCAFFJSSRFFEG66AYG8XC`.
- Role `po-critic` completed with outcome `po-critic-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `a73fe35c094d4d4580b28b43f21cd926`

Action plan
- [queued] `blocked-by-follow-up-comment` -> `06EZ0N8HW9PZAFKMM5WQD564VR` via `blocks` path `06EZ0NCAFFJSSRFFEG66AYG8XC -> 06EZ0N8HW9PZAFKMM5WQD564VR`
- [queued] `child-follow-up-comment` -> `06EZ0NCGYCADKEYGR16J5PJFS0` via `parentOf` path `06EZ0NCAFFJSSRFFEG66AYG8XC -> 06EZ0NCGYCADKEYGR16J5PJFS0`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06EZ0NCAFFJSSRFFEG66AYG8XC` owner `ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting` base `develop` source-owner `ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06EZ0N8HW9PZAFKMM5WQD564VR` owner `ticket/06EZ0N8HW9PZAFKMM5WQD564VR-story-define-provider-optimization-contract-and` base `develop` source-owner `ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting`: Target ticket owner branch 'ticket/06EZ0N8HW9PZAFKMM5WQD564VR-story-define-provider-optimization-contract-and' differs from source owner branch 'ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06EZ0NCGYCADKEYGR16J5PJFS0` owner `ticket/06EZ0NCGYCADKEYGR16J5PJFS0-task-emit-provider-comparison-benchmark-artifact` base `develop` source-owner `ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting`: Target ticket owner branch 'ticket/06EZ0NCGYCADKEYGR16J5PJFS0-task-emit-provider-comparison-benchmark-artifact' differs from source owner branch 'ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06EZ0N8HW9PZAFKMM5WQD564VR` on owner branch `ticket/06EZ0N8HW9PZAFKMM5WQD564VR-story-define-provider-optimization-contract-and` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `child-follow-up-target` to `06EZ0NCGYCADKEYGR16J5PJFS0` on owner branch `ticket/06EZ0NCGYCADKEYGR16J5PJFS0-task-emit-provider-comparison-benchmark-artifact` after that branch is refreshed/rebased.