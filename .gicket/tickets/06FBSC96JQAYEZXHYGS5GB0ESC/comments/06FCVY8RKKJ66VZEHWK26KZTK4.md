[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06FBSC96JQAYEZXHYGS5GB0ESC`.
- Role `po-critic` completed with outcome `po-critic-non-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `1`; dropped obsolete follow-up(s): `2`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `b3c8acd97a3b45e99844a76ee355b73e`

Action plan
- [queued] `blocked-follow-up-comment` -> `06FBSCA23YR3P9XRQA6MMYKV7C` via `blocks` path `06FBSC96JQAYEZXHYGS5GB0ESC -> 06FBSCA23YR3P9XRQA6MMYKV7C`
- [dropped] `blocked-by-follow-up-comment` -> `06FBSC8TS7R98ZEBDKE5XG2KTC` via `blocks` path `06FBSC96JQAYEZXHYGS5GB0ESC -> 06FBSC8TS7R98ZEBDKE5XG2KTC`
- [dropped] `blocked-by-follow-up-comment` -> `06FBSC4HSXFJ5FM6GWECH2CTGG` via `blocks` path `06FBSC96JQAYEZXHYGS5GB0ESC -> 06FBSC4HSXFJ5FM6GWECH2CTGG`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06FBSC96JQAYEZXHYGS5GB0ESC` owner `ticket/06FBSC96JQAYEZXHYGS5GB0ESC-task-evaluate-sql-server-bulk-strategy-gaps` base `develop` source-owner `ticket/06FBSC96JQAYEZXHYGS5GB0ESC-task-evaluate-sql-server-bulk-strategy-gaps`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06FBSCA23YR3P9XRQA6MMYKV7C` owner `ticket/06FBSCA23YR3P9XRQA6MMYKV7C-task-implement-accepted-sql-server-bulk-improvem` base `develop` source-owner `ticket/06FBSC96JQAYEZXHYGS5GB0ESC-task-evaluate-sql-server-bulk-strategy-gaps`: Mutation targets 'ticket/06FBSCA23YR3P9XRQA6MMYKV7C-task-implement-accepted-sql-server-bulk-improvem', not current branch 'ticket/06FBSC96JQAYEZXHYGS5GB0ESC-task-evaluate-sql-server-bulk-strategy-gaps'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06FBSC8TS7R98ZEBDKE5XG2KTC` owner `develop` base `develop` source-owner `ticket/06FBSC96JQAYEZXHYGS5GB0ESC-task-evaluate-sql-server-bulk-strategy-gaps`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06FBSC4HSXFJ5FM6GWECH2CTGG` owner `develop` base `develop` source-owner `ticket/06FBSC96JQAYEZXHYGS5GB0ESC-task-evaluate-sql-server-bulk-strategy-gaps`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06FBSCA23YR3P9XRQA6MMYKV7C` on owner branch `ticket/06FBSCA23YR3P9XRQA6MMYKV7C-task-implement-accepted-sql-server-bulk-improvem` after that branch is refreshed/rebased.