[gicket-bot] relation automation follow-up

Summary
- Evaluated `2` selected relation flow(s) for source ticket `06F7Y0KGY29HHGZWHC470KVJBG`.
- Role `dev` completed with outcome `dev-workflow-apply` under guard profile `community-safe-selected-flows-v1`.
- Applied `0` follow-up comment(s); queued owner-branch task(s): `1`; dropped obsolete follow-up(s): `1`; blocking diagnostics: `0`; write failures: `0`.
- run-id: `7e8b5b91ec7d420680dba6ebfd3b86a5`

Action plan
- [queued] `blocked-follow-up-comment` -> `06F7Y0KVHGTTVS216ERSG4XNMM` via `blocks` path `06F7Y0KGY29HHGZWHC470KVJBG -> 06F7Y0KVHGTTVS216ERSG4XNMM`
- [dropped] `blocked-by-follow-up-comment` -> `06F7Y0HZKHBHMYX9EYDYFRYXZ0` via `blocks` path `06F7Y0KGY29HHGZWHC470KVJBG -> 06F7Y0HZKHBHMYX9EYDYFRYXZ0`

Branch/worktree plan
- [execute-now] `source-audit-summary` `06F7Y0KGY29HHGZWHC470KVJBG` owner `ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru` base `develop` source-owner `ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru`: Source summary is owned by the source ticket branch.
- [queue-for-owner-branch] `relation-audit-follow-up` `06F7Y0KVHGTTVS216ERSG4XNMM` owner `ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in` base `develop` source-owner `ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru`: Mutation targets 'ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in', not current branch 'ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru'; queue for target-branch replay.
- [base-terminal-dropped] `relation-audit-follow-up` `06F7Y0HZKHBHMYX9EYDYFRYXZ0` owner `develop` base `develop` source-owner `ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru`: Base branch 'develop' already contains ticket status 'done', so the relation follow-up is obsolete.

Queued owner-branch tasks
- `RELATION-AUTOMATION-BRANCH-OWNER-QUEUED`: apply `blocked-follow-up-target` to `06F7Y0KVHGTTVS216ERSG4XNMM` on owner branch `ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in` after that branch is refreshed/rebased.