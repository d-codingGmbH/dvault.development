[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F0MEF08AJ1K52STF42T74B04`.
- Role `po-critic` completed with outcome `po-critic-non-blocking-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `4`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `2117d261bd774af78ef4c3cd16ca482e`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F0MEFHKF04B746X7GJKRVT04` via `blocks` path `06F0MEF08AJ1K52STF42T74B04 -> 06F0MEFHKF04B746X7GJKRVT04`
- [queued] `blocked-follow-up-comment` -> `06F0MEFX5M9V9SA25N76CPGT4M` via `blocks` path `06F0MEF08AJ1K52STF42T74B04 -> 06F0MEFX5M9V9SA25N76CPGT4M`
- [queued] `blocked-by-follow-up-comment` -> `06F0MEE8T9PKPKQH8EPWNQ2CRW` via `blocks` path `06F0MEF08AJ1K52STF42T74B04 -> 06F0MEE8T9PKPKQH8EPWNQ2CRW`
- [queued] `blocked-by-follow-up-comment` -> `06F0MEEGJE9QCHC8YN4FEXYX10` via `blocks` path `06F0MEF08AJ1K52STF42T74B04 -> 06F0MEEGJE9QCHC8YN4FEXYX10`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F0MEF08AJ1K52STF42T74B04` owner `ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and` base `develop` source-owner `ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEFHKF04B746X7GJKRVT04` owner `ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry` base `develop` source-owner `ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and`: Target ticket owner branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' differs from source owner branch 'ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEFX5M9V9SA25N76CPGT4M` owner `ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat` base `develop` source-owner `ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and`: Target ticket owner branch 'ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat' differs from source owner branch 'ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEE8T9PKPKQH8EPWNQ2CRW` owner `ticket/06F0MEE8T9PKPKQH8EPWNQ2CRW-task-define-versioned-dvault-model-schema-and-va` base `develop` source-owner `ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and`: Target ticket owner branch 'ticket/06F0MEE8T9PKPKQH8EPWNQ2CRW-task-define-versioned-dvault-model-schema-and-va' differs from source owner branch 'ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and'.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F0MEEGJE9QCHC8YN4FEXYX10` owner `ticket/06F0MEEGJE9QCHC8YN4FEXYX10-task-implement-json-model-parser-and-validation` base `develop` source-owner `ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and`: Target ticket owner branch 'ticket/06F0MEEGJE9QCHC8YN4FEXYX10-task-implement-json-model-parser-and-validation' differs from source owner branch 'ticket/06F0MEF08AJ1K52STF42T74B04-task-project-imported-model-into-ef-metadata-and'.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0MEFHKF04B746X7GJKRVT04` on owner branch `ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F0MEFX5M9V9SA25N76CPGT4M` on owner branch `ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F0MEE8T9PKPKQH8EPWNQ2CRW` on owner branch `ticket/06F0MEE8T9PKPKQH8EPWNQ2CRW-task-define-versioned-dvault-model-schema-and-va` after that branch is refreshed/rebased.
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-by-follow-up-target` to `06F0MEEGJE9QCHC8YN4FEXYX10` on owner branch `ticket/06F0MEEGJE9QCHC8YN4FEXYX10-task-implement-json-model-parser-and-validation` after that branch is refreshed/rebased.