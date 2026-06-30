[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8RGQZA7D9JZSTSAJEM9B3M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RGQZA7D9JZSTSAJEM9B3M`.
- Optimistic claim succeeded (`expectedRevision=06FHH4BKMWJ5WMRSYEGNE33MPM`, `currentRevision=06FHH4RY3PVZ4GFZWYS911D7E8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co' from source '3c264a9e77bb38c81f1bc336953f6ee6eb8e597b'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co` as `4a4b53226ac1`.

Open questions / Risiken
- Risky assumption: The current contract correctly treats `personalData[].encryptedPayloadAlias` as the approved downstream contract target; implementers should not assume that metadata is already landed as branch code just because the contract is now approved.
- Risky assumption: Implementers must treat `DataVaultProviderNativeEncryptionBoundaryFact` as evidence-only boundary reporting, not as approval for shared runtime provider-native encryption behavior.
- Split recommendation: Keep future provider-native encryption work split to one provider and one exact capability per ticket, with its own provider package surface, fallback rules, tests, and evidence.
- Split recommendation: Split broader privacy workflow APIs such as read-helper redaction, pseudonymization flows, or retention metadata review into separate tickets instead of widening this contract.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7586`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `13ff1a9c20fc4373a5e20c9016866c8c`
- completed-at-utc: `<redacted>-30T12:54:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RGQZA7D9JZSTSAJEM9B3M/runs/20260630T125449993Z-13ff1a9c20fc4373a5e20c9016866c8c.json`