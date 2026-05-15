[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F2PGGJQMKH2T5948VJH93M5R`.
- Role `po-critic` completed with outcome `po-critic-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `1b65ea1d39254a52a8785f27c2cd4684`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F2PGGR30XXCDKCZ8W2J2WX8C` via `blocks` path `06F2PGGJQMKH2T5948VJH93M5R -> 06F2PGGR30XXCDKCZ8W2J2WX8C`
- [queued] `blocked-by-follow-up-comment` -> `06F2PGFZWC5PXSDH46RCZPN1CG` via `blocks` path `06F2PGGJQMKH2T5948VJH93M5R -> 06F2PGFZWC5PXSDH46RCZPN1CG`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F2PGGJQMKH2T5948VJH93M5R` owner `ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c` base `develop` source-owner `ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F2PGGR30XXCDKCZ8W2J2WX8C` owner `ticket/06F2PGGR30XXCDKCZ8W2J2WX8C-task-add-ci-workflow-examples-for-design-time-ch` base `develop` source-owner `ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c`: Target ticket owner branch 'ticket/06F2PGGR30XXCDKCZ8W2J2WX8C-task-add-ci-workflow-examples-for-design-time-ch' differs from source owner branch 'ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F2PGFZWC5PXSDH46RCZPN1CG` owner `ticket/06F2PGFZWC5PXSDH46RCZPN1CG-story-add-provider-live-schema-drift-readers` base `develop` source-owner `ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c`: Target ticket owner branch 'ticket/06F2PGFZWC5PXSDH46RCZPN1CG-story-add-provider-live-schema-drift-readers' differs from source owner branch 'ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F2PGGR30XXCDKCZ8W2J2WX8C` on owner branch `ticket/06F2PGGR30XXCDKCZ8W2J2WX8C-task-add-ci-workflow-examples-for-design-time-ch` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F2PGFZWC5PXSDH46RCZPN1CG` on owner branch `ticket/06F2PGFZWC5PXSDH46RCZPN1CG-story-add-provider-live-schema-drift-readers` after that branch is refreshed/rebased.