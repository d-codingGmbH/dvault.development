[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F0MECPFAVBFBNC5XMVDZRQ6M`.
- Role `po-critic` completed with outcome `po-critic-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `3`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `6fc6359a9f3b4ab1ac56c98a942ea9e8`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F0MEDJC732GDD77H60R259P0` via `blocks` path `06F0MECPFAVBFBNC5XMVDZRQ6M -> 06F0MEDJC732GDD77H60R259P0`
- [queued] `blocked-by-follow-up-comment` -> `06F0MEB634X6CTBZ00W108G3FG` via `blocks` path `06F0MECPFAVBFBNC5XMVDZRQ6M -> 06F0MEB634X6CTBZ00W108G3FG`
- [queued] `blocked-by-follow-up-comment` -> `06F0MEC7FEXAD069AJNYZW0DRM` via `blocks` path `06F0MECPFAVBFBNC5XMVDZRQ6M -> 06F0MEC7FEXAD069AJNYZW0DRM`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F0MECPFAVBFBNC5XMVDZRQ6M` owner `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p` base `develop` source-owner `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEDJC732GDD77H60R259P0` owner `ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u` base `develop` source-owner `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p`: Target ticket owner branch 'ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u' differs from source owner branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEB634X6CTBZ00W108G3FG` owner `ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a` base `develop` source-owner `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p`: Target ticket owner branch 'ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a' differs from source owner branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEC7FEXAD069AJNYZW0DRM` owner `ticket/06F0MEC7FEXAD069AJNYZW0DRM-task-define-typed-hub-link-and-satellite-mapper` base `develop` source-owner `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p`: Target ticket owner branch 'ticket/06F0MEC7FEXAD069AJNYZW0DRM-task-define-typed-hub-link-and-satellite-mapper' differs from source owner branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0MEDJC732GDD77H60R259P0` on owner branch `ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F0MEB634X6CTBZ00W108G3FG` on owner branch `ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F0MEC7FEXAD069AJNYZW0DRM` on owner branch `ticket/06F0MEC7FEXAD069AJNYZW0DRM-task-define-typed-hub-link-and-satellite-mapper` after that branch is refreshed/rebased.