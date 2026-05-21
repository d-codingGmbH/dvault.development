[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492A8WV0EP2V03CWXXWH71G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492A8WV0EP2V03CWXXWH71G`.
- Optimistic claim succeeded (`expectedRevision=06F4P1EAV3NEXYD6GNT5PEPGXG`, `currentRevision=06F4Q8PG53AGHV2AB52AQ1QC74`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492A8WV0EP2V03CWXXWH71G-story-strengthen-migration-guardrail-reports' from source 'adfa29f3919d6c4d7d04104556d8852c165c6403'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492A8WV0EP2V03CWXXWH71G-story-strengthen-migration-guardrail-reports` as `f4b46e01d982`.

Open questions / Risiken
- Risky assumption: The contract implies, but does not literally spell out, that a per-operation summary becomes incompatible whenever any error-severity DVM finding exists, even if warning findings are also attached to the same operation.
- Risky assumption: The contract assumes provider-aware wording can be satisfied from the existing DataVaultDiagnosticsResult.Explain surface without introducing any new provider-discovery mechanism; repository evidence supports that assumption today via fields such as provider ...
- Split recommendation: No split recommended. The ticket is already bounded to strengthening one existing report lane while leaving the aggregator story 06F492BG6BZYYFMBE5WK7CB024 and documentation task 06F492BNDPWS9P4EDSV0W7G6VM as downstream consumers.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9104`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d9c3c5ab4cf64332996bf69e572133c8`
- completed-at-utc: `<redacted>-21T17:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492A8WV0EP2V03CWXXWH71G/runs/20260521T175105805Z-d9c3c5ab4cf64332996bf69e572133c8.json`