[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492BTNHRPBC7D24E13ECFKM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492BTNHRPBC7D24E13ECFKM`.
- Optimistic claim succeeded (`expectedRevision=06F5EBGG27DPBDZ360PG583V1M`, `currentRevision=06F5ECM351D6R2MR9J1RG4VW5G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492BTNHRPBC7D24E13ECFKM-epic-performance-analysis-and-query-tuning' from source 'abf13e1d86fb5f2a15a541721d3dbe23be7ea8f4'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492BTNHRPBC7D24E13ECFKM-epic-performance-analysis-and-query-tuning` as `5ee97bbf178e`.

Open questions / Risiken
- Risky assumption: This approval assumes the epic is a rollup/handoff ticket over already-landed child work, not a request for separate epic-level implementation beyond the seven child tickets.
- Risky assumption: Optional external-provider lanes are assumed acceptable as skipped when unconfigured because the contract, `benchmark-summary.md`, and `benchmark-summary.json` all preserve visible skipped rows with normalized reasons instead of omitting them.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9012`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `84596659e5aa46b7a9c0eac61e8118ff`
- completed-at-utc: `<redacted>-23T23:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492BTNHRPBC7D24E13ECFKM/runs/20260523T234401634Z-84596659e5aa46b7a9c0eac61e8118ff.json`