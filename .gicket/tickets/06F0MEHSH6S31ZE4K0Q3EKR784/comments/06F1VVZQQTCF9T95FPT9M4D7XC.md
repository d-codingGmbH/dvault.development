[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0MEHSH6S31ZE4K0Q3EKR784'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEHSH6S31ZE4K0Q3EKR784`.
- Optimistic claim succeeded (`expectedRevision=06F1VT8VZH43G5KY1V889NGQY8`, `currentRevision=06F1VTMRPG0R0AQFVQ713ZQ874`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo' from source '638b3d021fe4f89c54369d1831370c6f4a8889b6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo` as `d38459e4bcca`.

Open questions / Risiken
- Risky assumption: Benchmark timings remain machine-specific; downstream review should preserve command, provider filter, timestamp storage, run context, and measured rows rather than relying on absolute times alone.
- Risky assumption: Non-SQLite provider benchmark rows may be skipped on machines without configured provider dependencies or connection strings; this is acceptable only if skipReason remains deterministic and visible.
- Split recommendation: No additional split is recommended. The existing split is already materialized and done through 06F0MEJ0NE80R7CNS982S3PKVR, 06F0MEJ7NANHCP64VR1SH3S3G8, 06F0MEJE5WC51MFQ3CWDRATCWC, and 06F0MEJPGG7JBFEXD693BHY07W.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9516`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `bd70a35834c143a48d373eca1683ec6b`
- completed-at-utc: `<redacted>-12T20:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEHSH6S31ZE4K0Q3EKR784/runs/20260512T204525469Z-bd70a35834c143a48d373eca1683ec6b.json`