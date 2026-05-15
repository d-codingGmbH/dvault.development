[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F2PGGW8ZBW80V6B8RPWNVM70'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGGW8ZBW80V6B8RPWNVM70`.
- Optimistic claim succeeded (`expectedRevision=06F2TQ5GHBDVGA2SXWVQKS6GGM`, `currentRevision=06F2TQDQBNPPVQV8AS37ZB60QG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce' from source 'f7c4aa54980ebe8124b4065c4fc7af21d823ec2e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce` as `d042533ff44d`.

Open questions / Risiken
- Risky assumption: The story assumes current CreateTableOperation plus existing column/index/primary-key/drop-table coverage is sufficient for a blocking CI baseline without table-rename or prior-schema inference.
- Risky assumption: The handoff assumes the missing docs/releases/v0.11.0.md file remains acceptable here because that documentation rollout is explicitly separated into ticket 06F2PGHA0EXJRGDHM4GQM7NPYR.
- Split recommendation: Keep implementation-level rule coverage in existing child 06F2PGH42B6BT1708MYGMXP5GM.
- Split recommendation: Keep broader v0.11 documentation and release-note work in 06F2PGHA0EXJRGDHM4GQM7NPYR.
- Split recommendation: Create separate future tickets for RenameTableOperation, missing-table/prior-schema inference, or provider-specific facet checks if they become necessary.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9361`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `a7af65d874744f0280b3a7e6e4495aa3`
- completed-at-utc: `<redacted>-15T20:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGGW8ZBW80V6B8RPWNVM70/runs/20260515T204446895Z-a7af65d874744f0280b3a7e6e4495aa3.json`