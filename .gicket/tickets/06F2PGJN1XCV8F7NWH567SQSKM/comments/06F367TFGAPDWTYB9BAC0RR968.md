[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F2PGJN1XCV8F7NWH567SQSKM`.
- Role `po-critic` completed with outcome `po-critic-non-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `1`; dropped obsolete follow-up(s): `2`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `d63f76cd14cd49c1a68ce6f5fc8cc8e2`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F2PGJSXP18VKKV52QZA4NP30` via `blocks` path `06F2PGJN1XCV8F7NWH567SQSKM -> 06F2PGJSXP18VKKV52QZA4NP30`
- [dropped] `blocked-by-follow-up-comment` -> `06F2PGJBRXFCP038CN6XVAYSZM` via `blocks` path `06F2PGJN1XCV8F7NWH567SQSKM -> 06F2PGJBRXFCP038CN6XVAYSZM`
- [dropped] `blocked-by-follow-up-comment` -> `06F2PGFT8Z406HFBJGQSY7YRJ0` via `blocks` path `06F2PGJN1XCV8F7NWH567SQSKM -> 06F2PGFT8Z406HFBJGQSY7YRJ0`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F2PGJN1XCV8F7NWH567SQSKM` owner `ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co` base `develop` source-owner `ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F2PGJSXP18VKKV52QZA4NP30` owner `ticket/06F2PGJSXP18VKKV52QZA4NP30-task-generate-metadata-and-row-factory-helpers` base `develop` source-owner `ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co`: Target ticket owner branch 'ticket/06F2PGJSXP18VKKV52QZA4NP30-task-generate-metadata-and-row-factory-helpers' differs from source owner branch 'ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co'.
- [base-terminal-dropped] `relation-audit-follow-up` `06F2PGJBRXFCP038CN6XVAYSZM` owner `<base-terminal>` base `develop` source-owner `ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.
- [base-terminal-dropped] `relation-audit-follow-up` `06F2PGFT8Z406HFBJGQSY7YRJ0` owner `<base-terminal>` base `develop` source-owner `ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F2PGJSXP18VKKV52QZA4NP30` on owner branch `ticket/06F2PGJSXP18VKKV52QZA4NP30-task-generate-metadata-and-row-factory-helpers` after that branch is refreshed/rebased.