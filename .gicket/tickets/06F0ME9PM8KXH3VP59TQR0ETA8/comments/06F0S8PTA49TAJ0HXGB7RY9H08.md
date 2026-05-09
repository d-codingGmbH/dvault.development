[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F0ME9PM8KXH3VP59TQR0ETA8`.
- Role `po` completed with outcome `po-refinement-ready` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `3`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `002f9492cefa434e93f4d43d0150cecb`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F0MEAD1BAA5QEVM3F9QJA38G` via `blocks` path `06F0ME9PM8KXH3VP59TQR0ETA8 -> 06F0MEAD1BAA5QEVM3F9QJA38G`
- [queued] `blocked-follow-up-comment` -> `06F0MEB634X6CTBZ00W108G3FG` via `blocks` path `06F0ME9PM8KXH3VP59TQR0ETA8 -> 06F0MEB634X6CTBZ00W108G3FG`
- [queued] `blocked-by-follow-up-comment` -> `06F0ME976PM5455JK04S6GPNNW` via `blocks` path `06F0ME9PM8KXH3VP59TQR0ETA8 -> 06F0ME976PM5455JK04S6GPNNW`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F0ME9PM8KXH3VP59TQR0ETA8` owner `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata` base `develop` source-owner `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEAD1BAA5QEVM3F9QJA38G` owner `ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity` base `develop` source-owner `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata`: Target ticket owner branch 'ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity' differs from source owner branch 'ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEB634X6CTBZ00W108G3FG` owner `ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a` base `develop` source-owner `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata`: Target ticket owner branch 'ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a' differs from source owner branch 'ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0ME976PM5455JK04S6GPNNW` owner `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co` base `develop` source-owner `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata`: Target ticket owner branch 'ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co' differs from source owner branch 'ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0MEAD1BAA5QEVM3F9QJA38G` on owner branch `ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0MEB634X6CTBZ00W108G3FG` on owner branch `ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F0ME976PM5455JK04S6GPNNW` on owner branch `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co` after that branch is refreshed/rebased.