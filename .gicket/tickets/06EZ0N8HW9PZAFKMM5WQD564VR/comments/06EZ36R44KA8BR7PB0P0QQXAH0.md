[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EZ0N8HW9PZAFKMM5WQD564VR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0N8HW9PZAFKMM5WQD564VR`.
- Optimistic claim succeeded (`expectedRevision=06EZ3523MDSYAGTVNFBQN1XQFR`, `currentRevision=06EZ35E3Y8HVN8V4C5TJTC0S7M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0N8HW9PZAFKMM5WQD564VR-story-define-provider-optimization-contract-and' from source '559f1b2f625a6f2902cc997ca4d85fc9567bb305'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EZ0N8HW9PZAFKMM5WQD564VR-story-define-provider-optimization-contract-and` as `4727d1a126a4`.

Open questions / Risiken
- Risky assumption: Equal-priority tie behavior currently depends on stable LINQ ordering plus DI registration order; source shows the sort, but the repo does not yet have a direct equal-priority proof outside the ticket contract.
- Risky assumption: DataVaultEfMetadataTranslator currently hardcodes DataVaultProviderCapabilityProfiles.Sqlite, so implementers could accidentally widen provider-aware EF metadata scope while touching provider contracts.
- Risky assumption: Because only SQLite currently has an optimized executor, some shared-contract or dispatch edge cases may stay latent until a second provider adopts the strategy boundary.
- Split recommendation: Keep the current split: one contract story, bounded docs/test child tasks, and separate provider-specific optimization stories.
- Split recommendation: Only split further if provider-capability-profile expansion or non-SQLite metadata translation is pulled into scope; both are explicitly deferred today.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8909`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `515671b9a6eb47659d9c182719611fc3`
- completed-at-utc: `<redacted>-04T06:09:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0N8HW9PZAFKMM5WQD564VR/runs/20260504T060920803Z-515671b9a6eb47659d9c182719611fc3.json`