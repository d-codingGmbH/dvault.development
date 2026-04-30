[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB74NRVRX18GD33CH1C12SW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB74NRVRX18GD33CH1C12SW`.
- Optimistic claim succeeded (`expectedRevision=06EXS8Q5J7SQH7XEAW561NFC9M`, `currentRevision=06EXS8XF4S1K4GPYF1ZA4Z830M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB74NRVRX18GD33CH1C12SW-story-model-data-vault-building-blocks' from source '5941e2a0b3a79c8409bedbdf379170933d0f9b13'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB74NRVRX18GD33CH1C12SW-story-model-data-vault-building-blocks` as `43375a63e419`.

Open questions / Risiken
- Risky assumption: Hash key and hash diff metadata could be mistaken for hash computation; the ticket contract keeps hash algorithms and normalization out of scope.
- Risky assumption: The parent story spans hub, link, and satellite concepts, so implementation needs scope discipline to avoid drifting into provider persistence or schema generation.
- Split recommendation: No PO split is required before dev; comment evidence records existing parentOf child tickets 06EXB74XQJFKGSKVJ6THQWJY8W and 06EXB755X9TGQW2EG1G30GJG28.
- Split recommendation: If dev finds the parent too broad, split by hub/business-key metadata, link/participant metadata, and satellite/payload metadata while keeping the shared technical metadata role set common.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8318`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `900e700d253f41819054508d67e4b439`
- completed-at-utc: `<redacted>-30T04:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB74NRVRX18GD33CH1C12SW/runs/20260430T043155641Z-900e700d253f41819054508d67e4b439.json`