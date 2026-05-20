[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F2PGQJ7THHNSYYBFFPBG4174'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGQJ7THHNSYYBFFPBG4174`.
- Optimistic claim succeeded (`expectedRevision=06F440YSC7RG47RHTY5QFBZKBG`, `currentRevision=06F442KHSNYYF1PZQPM43DQ4BW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGQJ7THHNSYYBFFPBG4174-story-add-diagnostics-support-bundle-export' from source '6422959a045d142761847a0c75d5c1f6df4f6d73'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F2PGQJ7THHNSYYBFFPBG4174-story-add-diagnostics-support-bundle-export` as `3360513eb27a`.

Open questions / Risiken
- Risky assumption: The developer will need an explicit consumer-owned way to supply request-bound save or read diagnostics into the support-bundle flow because the current `DataVaultDesignTimeCommandHost` only carries diagnostics service, DbContext factory, export source, migra...
- Risky assumption: Redaction can be implemented provider-neutrally enough to protect secrets without stripping provider names, diagnostic codes, profile names, or other troubleshooting-relevant identifiers.
- Split recommendation: No split recommended; the live relation set already separates prerequisite strategy diagnostics, this support-bundle story, sibling telemetry work, and downstream v0.16 documentation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6722`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7748337114d84158a9c0ddaaeb95a84f`
- completed-at-utc: `<redacted>-19T21:07:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGQJ7THHNSYYBFFPBG4174/runs/20260519T210715044Z-7748337114d84158a9c0ddaaeb95a84f.json`