[gicket-bot] relation automation follow-up

Summary
- Evaluated `1` selected relation flow(s) for source ticket `06F0ME976PM5455JK04S6GPNNW`.
- Role `po-critic` completed with outcome `po-critic-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `3`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `7399d8a682ee4fe9bb42f777621d14d7`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F0ME9PM8KXH3VP59TQR0ETA8` via `blocks` path `06F0ME976PM5455JK04S6GPNNW -> 06F0ME9PM8KXH3VP59TQR0ETA8`
- [queued] `blocked-follow-up-comment` -> `06F0MEA1FF743S14XQW02H4A3W` via `blocks` path `06F0ME976PM5455JK04S6GPNNW -> 06F0MEA1FF743S14XQW02H4A3W`
- [queued] `blocked-follow-up-comment` -> `06F0MEAD1BAA5QEVM3F9QJA38G` via `blocks` path `06F0ME976PM5455JK04S6GPNNW -> 06F0MEAD1BAA5QEVM3F9QJA38G`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F0ME976PM5455JK04S6GPNNW` owner `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co` base `develop` source-owner `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0ME9PM8KXH3VP59TQR0ETA8` owner `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata` base `develop` source-owner `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co`: Target ticket owner branch 'ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata' differs from source owner branch 'ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEA1FF743S14XQW02H4A3W` owner `ticket/06F0MEA1FF743S14XQW02H4A3W-task-implement-fluent-link-and-relationship-proj` base `develop` source-owner `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co`: Target ticket owner branch 'ticket/06F0MEA1FF743S14XQW02H4A3W-task-implement-fluent-link-and-relationship-proj' differs from source owner branch 'ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEAD1BAA5QEVM3F9QJA38G` owner `ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity` base `develop` source-owner `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co`: Target ticket owner branch 'ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity' differs from source owner branch 'ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0ME9PM8KXH3VP59TQR0ETA8` on owner branch `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0MEA1FF743S14XQW02H4A3W` on owner branch `ticket/06F0MEA1FF743S14XQW02H4A3W-task-implement-fluent-link-and-relationship-proj` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0MEAD1BAA5QEVM3F9QJA38G` on owner branch `ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity` after that branch is refreshed/rebased.