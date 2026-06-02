[gicket-bot] relation automation follow-up

Summary
- Evaluated `1` selected relation flow(s) for source ticket `06F8KZGC4NY41PRYB2RP00ZA1M`.
- Role `po-critic` completed with outcome `po-critic-non-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `1`; dropped obsolete follow-up(s): `0`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `0471e9f7d0cd4fb785bdec79b7ada76a`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F8KZGNRG5FY4WWCY3FAX2NS4` via `blocks` path `06F8KZGC4NY41PRYB2RP00ZA1M -> 06F8KZGNRG5FY4WWCY3FAX2NS4`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F8KZGC4NY41PRYB2RP00ZA1M` owner `ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract` base `develop` source-owner `ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F8KZGNRG5FY4WWCY3FAX2NS4` owner `ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault` base `develop` source-owner `ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract`: Mutation targets 'ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault', not current branch 'ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract'; queue for target-branch replay.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F8KZGNRG5FY4WWCY3FAX2NS4` on owner branch `ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault` after that branch is refreshed/rebased.