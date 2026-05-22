[gicket-bot] relation automation follow-up (human-needed)

Summary
- Evaluated `1` selected relation flow(s) for source ticket `06F492A3MPSGP3KXDNZECN01QM`.
- Role `po` completed with outcome `po-refinement-ready` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `0`; dropped obsolete follow-up(s): `8`; blocking diagnostics: `1`; write failures: `0`.
- run-id: `696e5317f53a418294a3f089ddb046cc`

Action plan
- [blocked] `child-follow-up-comment` -> `06F492A8WV0EP2V03CWXXWH71G` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492A8WV0EP2V03CWXXWH71G`
- [blocked] `child-follow-up-comment` -> `06F492AE2C8XBDXDH4V2JPTJDR` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492AE2C8XBDXDH4V2JPTJDR`
- [blocked] `child-follow-up-comment` -> `06F492AKGMKPCRJYF4Z1EC9WY4` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492AKGMKPCRJYF4Z1EC9WY4`
- [blocked] `child-follow-up-comment` -> `06F492ARW2N6SNYJH15RHMZEN8` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492ARW2N6SNYJH15RHMZEN8`
- [blocked] `child-follow-up-comment` -> `06F492AYE4A3PKA2D20DDPQ37C` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492AYE4A3PKA2D20DDPQ37C`
- [blocked] `child-follow-up-comment` -> `06F492B40K7B0WWPKH8N3PPG3G` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492B40K7B0WWPKH8N3PPG3G`
- [blocked] `child-follow-up-comment` -> `06F492B9PR036PDNN52S06S9BC` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492B9PR036PDNN52S06S9BC`
- [blocked] `child-follow-up-comment` -> `06F492BG6BZYYFMBE5WK7CB024` via `parentOf` path `06F492A3MPSGP3KXDNZECN01QM -> 06F492BG6BZYYFMBE5WK7CB024`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F492A3MPSGP3KXDNZECN01QM` owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Source summary is owned by the source ticket branch.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492A8WV0EP2V03CWXXWH71G` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492AE2C8XBDXDH4V2JPTJDR` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492AKGMKPCRJYF4Z1EC9WY4` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492ARW2N6SNYJH15RHMZEN8` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492AYE4A3PKA2D20DDPQ37C` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492B40K7B0WWPKH8N3PPG3G` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492B9PR036PDNN52S06S9BC` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F492BG6BZYYFMBE5WK7CB024` owner `<base-terminal>` base `develop` source-owner `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Blocking diagnostics
- `RELATION-AUTOMATION-FANOUT-CAP-EXCEEDED`: Flow 'child-follow-up-comment' would exceed max follow-up actions 8 at ticket '06F492BNDPWS9P4EDSV0W7G6VM'. Relation automation is blocked by fanout policy.