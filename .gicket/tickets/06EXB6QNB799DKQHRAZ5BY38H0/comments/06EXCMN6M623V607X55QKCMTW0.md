[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB6QNB799DKQHRAZ5BY38H0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6QNB799DKQHRAZ5BY38H0`.
- Optimistic claim succeeded (`expectedRevision=06EXCKS282R46SS291SHGW4V64`, `currentRevision=06EXCKW1DWTC7KJMQZDC4AC5BG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB6QNB799DKQHRAZ5BY38H0-task-draft-default-convention-policy' from source '5ef9d64c28a165ac1202b99965a3ba20f8d3c70c'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB6QNB799DKQHRAZ5BY38H0-task-draft-default-convention-policy` as `8946cc6e1987`.

Open questions / Risiken
- Risky assumption: The ticket leaves exact logical names and hash algorithm selection to the policy author; this is acceptable for handoff because the Acceptance Criteria require the produced policy to settle deterministic v1 defaults.
- Risky assumption: Provider-neutral wording can hide provider constraints; the contract already calls out this risk and requires logical defaults that adapters can map without provider-specific commitments.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8988`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c8ccf25736ad473ead4635918fc60a4f`
- completed-at-utc: `<redacted>-28T23:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6QNB799DKQHRAZ5BY38H0/runs/20260428T230033487Z-c8ccf25736ad473ead4635918fc60a4f.json`