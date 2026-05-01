[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7HPGW3Y9MSP10DEC8RBK4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7HPGW3Y9MSP10DEC8RBK4`.
- Optimistic claim succeeded (`expectedRevision=06EY2K3R23BF8V4KSPGF4N3VQR`, `currentRevision=06EY2K7BQFEZ9SWEGZJM4Q2058`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7HPGW3Y9MSP10DEC8RBK4-task-persist-satellite-rows-only-when-hash-diff' from source '580583d8ef88b2a6a053d761190dbe8a566564fe'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7HPGW3Y9MSP10DEC8RBK4-task-persist-satellite-rows-only-when-hash-diff` as `ba9327088ad9`.

Open questions / Risiken
- Blocking finding: The ticket asks developers to extend the existing explicit save-service boundary but never fixes the caller-visible satellite request shape. Current source exposes only hub/link operations on DataVaultSaveRequest, so developers still lack a source-backed prod...
- Blocking finding: The ticket also leaves visible result semantics unresolved. Current DataVaultSaveResult/DataVaultSavedRecord only summarize hub/link hash-key outputs, but the contract does not say whether satellite saves must appear in SavedRecords, be omitted intentionally,...
- Blocking finding: Hash-diff ownership is still ambiguous at the ticket level: description.md:48 allows either computing the hash diff inside the save path or carrying a deterministic hash diff in the request, while the repository hashing docs intentionally leave satellite fiel...
- Required PO action: Specify the satellite request contract on the explicit save-service boundary: required caller inputs, how the parent hub/link is identified, and whether callers submit HashDiff explicitly or submit only payload values.
- Required PO action: Specify the caller-visible save result behavior for satellite operations: whether DataVaultSaveResult.SavedRecords must include satellite outcomes, whether satellite saves intentionally do not surface there, or whether another result contract is expected.
- Required PO action: Promote the chosen hash-diff strategy into acceptance criteria or examples so developers are not forced to make a public API decision during implementation.
- Risky assumption: Assuming developers may choose the hash-diff source themselves without changing the intended public API.
- Risky assumption: Assuming latest persisted version always means the row with the greatest LoadTimestamp and that caller timestamps are monotonic per parent.
- Risky assumption: Assuming DataVaultSavedRecord.HashKey can represent a satellite outcome even though the schema keys satellite history by parent hash key plus load timestamp, not by a satellite hash key.
- Split recommendation: No split is needed if the PO tightens the satellite request/result contract and hash-diff ownership within this ticket.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9414`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `4cb6740e287542eab22ae7b3ee18d587`
- completed-at-utc: `<redacted>-01T02:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/runs/20260501T021615186Z-4cb6740e287542eab22ae7b3ee18d587.json`