[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F8KZKFTCC0YXAPRTXA53DNEC`.
- Role `po-critic` completed with outcome `po-critic-non-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; dropped obsolete follow-up(s): `1`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `5a41933dd3314fa9a95369e31b05d33c`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F8KZHNYE6PAGC74BSF70WZ3W` via `blocks` path `06F8KZKFTCC0YXAPRTXA53DNEC -> 06F8KZHNYE6PAGC74BSF70WZ3W`
- [queued] `blocked-follow-up-comment` -> `06F8KZMRXRHRKHV56Y96M4S90G` via `blocks` path `06F8KZKFTCC0YXAPRTXA53DNEC -> 06F8KZMRXRHRKHV56Y96M4S90G`
- [dropped] `blocked-by-follow-up-comment` -> `06F8KZK2MSFQP9G2DBM61ZVGD4` via `blocks` path `06F8KZKFTCC0YXAPRTXA53DNEC -> 06F8KZK2MSFQP9G2DBM61ZVGD4`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F8KZKFTCC0YXAPRTXA53DNEC` owner `ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d` base `develop` source-owner `ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZHNYE6PAGC74BSF70WZ3W` owner `ticket/06F8KZHNYE6PAGC74BSF70WZ3W-epic-provider-read-optimization-evidence-and-exp` base `develop` source-owner `ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d`: Mutation targets 'ticket/06F8KZHNYE6PAGC74BSF70WZ3W-epic-provider-read-optimization-evidence-and-exp', not current branch 'ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d'; queue for target-branch replay.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZMRXRHRKHV56Y96M4S90G` owner `ticket/06F8KZMRXRHRKHV56Y96M4S90G-story-define-provider-identifier-and-ddl-guardra` base `develop` source-owner `ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d`: Mutation targets 'ticket/06F8KZMRXRHRKHV56Y96M4S90G-story-define-provider-identifier-and-ddl-guardra', not current branch 'ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06F8KZK2MSFQP9G2DBM61ZVGD4` owner `develop` base `develop` source-owner `ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZHNYE6PAGC74BSF70WZ3W` on owner branch `ticket/06F8KZHNYE6PAGC74BSF70WZ3W-epic-provider-read-optimization-evidence-and-exp` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZMRXRHRKHV56Y96M4S90G` on owner branch `ticket/06F8KZMRXRHRKHV56Y96M4S90G-story-define-provider-identifier-and-ddl-guardra` after that branch is refreshed/rebased.