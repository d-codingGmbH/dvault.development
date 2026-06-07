[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F8KZVCVRPS3NAGQA7J55EAA4`.
- Role `po` completed with outcome `po-refinement-ready` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `2`; dropped obsolete follow-up(s): `1`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `5b8a927d0caa451aadc7431efc446d16`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F8KZVRARQPG482YKCQ686PNM` via `blocks` path `06F8KZVCVRPS3NAGQA7J55EAA4 -> 06F8KZVRARQPG482YKCQ686PNM`
- [queued] `blocked-follow-up-comment` -> `06F9XD26D2MHVAKZ2GCZ67BEFC` via `blocks` path `06F8KZVCVRPS3NAGQA7J55EAA4 -> 06F9XD26D2MHVAKZ2GCZ67BEFC`
- [dropped] `blocked-by-follow-up-comment` -> `06F8KZV18BQ0GN3CE4G02ATVA0` via `blocks` path `06F8KZVCVRPS3NAGQA7J55EAA4 -> 06F8KZV18BQ0GN3CE4G02ATVA0`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F8KZVCVRPS3NAGQA7J55EAA4` owner `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari` base `develop` source-owner `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZVRARQPG482YKCQ686PNM` owner `ticket/06F8KZVRARQPG482YKCQ686PNM-task-update-v0-32-0-artifact-lane-documentation` base `develop` source-owner `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari`: Mutation targets 'ticket/06F8KZVRARQPG482YKCQ686PNM-task-update-v0-32-0-artifact-lane-documentation', not current branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari'; queue for target-branch replay.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F9XD26D2MHVAKZ2GCZ67BEFC` owner `ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma` base `develop` source-owner `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari`: Mutation targets 'ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma', not current branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06F8KZV18BQ0GN3CE4G02ATVA0` owner `develop` base `develop` source-owner `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZVRARQPG482YKCQ686PNM` on owner branch `ticket/06F8KZVRARQPG482YKCQ686PNM-task-update-v0-32-0-artifact-lane-documentation` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F9XD26D2MHVAKZ2GCZ67BEFC` on owner branch `ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma` after that branch is refreshed/rebased.