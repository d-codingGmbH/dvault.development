[gicket-bot] relation automation follow-up

Summary
- Evaluated `1` selected relation flow(s) for source ticket `06F0MEHSH6S31ZE4K0Q3EKR784`.
- Role `po-critic` completed with outcome `po-critic-non-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `4`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `bd70a35834c143a48d373eca1683ec6b`

Action plan
- [queued] `child-follow-up-comment` -> `06F0MEJ0NE80R7CNS982S3PKVR` via `parentOf` path `06F0MEHSH6S31ZE4K0Q3EKR784 -> 06F0MEJ0NE80R7CNS982S3PKVR`
- [queued] `child-follow-up-comment` -> `06F0MEJ7NANHCP64VR1SH3S3G8` via `parentOf` path `06F0MEHSH6S31ZE4K0Q3EKR784 -> 06F0MEJ7NANHCP64VR1SH3S3G8`
- [queued] `child-follow-up-comment` -> `06F0MEJE5WC51MFQ3CWDRATCWC` via `parentOf` path `06F0MEHSH6S31ZE4K0Q3EKR784 -> 06F0MEJE5WC51MFQ3CWDRATCWC`
- [queued] `child-follow-up-comment` -> `06F0MEJPGG7JBFEXD693BHY07W` via `parentOf` path `06F0MEHSH6S31ZE4K0Q3EKR784 -> 06F0MEJPGG7JBFEXD693BHY07W`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F0MEHSH6S31ZE4K0Q3EKR784` owner `ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo` base `develop` source-owner `ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEJ0NE80R7CNS982S3PKVR` owner `ticket/06F0MEJ0NE80R7CNS982S3PKVR-task-benchmark-latest-pit-and-bridge-reads-acros` base `develop` source-owner `ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo`: Target ticket owner branch 'ticket/06F0MEJ0NE80R7CNS982S3PKVR-task-benchmark-latest-pit-and-bridge-reads-acros' differs from source owner branch 'ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEJ7NANHCP64VR1SH3S3G8` owner `ticket/06F0MEJ7NANHCP64VR1SH3S3G8-task-add-provider-specific-read-strategy-selecti` base `develop` source-owner `ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo`: Target ticket owner branch 'ticket/06F0MEJ7NANHCP64VR1SH3S3G8-task-add-provider-specific-read-strategy-selecti' differs from source owner branch 'ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEJE5WC51MFQ3CWDRATCWC` owner `ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti` base `develop` source-owner `ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo`: Target ticket owner branch 'ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti' differs from source owner branch 'ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEJPGG7JBFEXD693BHY07W` owner `ticket/06F0MEJPGG7JBFEXD693BHY07W-task-update-docs-and-release-notes-for-v0-7-0-mo` base `develop` source-owner `ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo`: Target ticket owner branch 'ticket/06F0MEJPGG7JBFEXD693BHY07W-task-update-docs-and-release-notes-for-v0-7-0-mo' differs from source owner branch 'ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `child-follow-up-target` to `06F0MEJ0NE80R7CNS982S3PKVR` on owner branch `ticket/06F0MEJ0NE80R7CNS982S3PKVR-task-benchmark-latest-pit-and-bridge-reads-acros` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `child-follow-up-target` to `06F0MEJ7NANHCP64VR1SH3S3G8` on owner branch `ticket/06F0MEJ7NANHCP64VR1SH3S3G8-task-add-provider-specific-read-strategy-selecti` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `child-follow-up-target` to `06F0MEJE5WC51MFQ3CWDRATCWC` on owner branch `ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `child-follow-up-target` to `06F0MEJPGG7JBFEXD693BHY07W` on owner branch `ticket/06F0MEJPGG7JBFEXD693BHY07W-task-update-docs-and-release-notes-for-v0-7-0-mo` after that branch is refreshed/rebased.