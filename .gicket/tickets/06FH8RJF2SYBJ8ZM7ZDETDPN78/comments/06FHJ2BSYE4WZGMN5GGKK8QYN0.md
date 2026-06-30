[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8RJF2SYBJ8ZM7ZDETDPN78'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RJF2SYBJ8ZM7ZDETDPN78`.
- Optimistic claim succeeded (`expectedRevision=06FHJ0D33BETQR5YK9VTN2MRCW`, `currentRevision=06FHJ0SCBJQQ4S7PPWASQA6X18`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro' from source '0d74e06aedfb71faa9e2ac8500515dfb3c06f2b3'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro` as `afcdca84c661`.

Open questions / Risiken
- Risky assumption: Implementation can bypass or compensate for the existing SQLite fallback in DataVaultProviderCapabilityProfileSelection.Select(...) so unknown or unregistered providers do not inherit reviewed crypto facts.
- Risky assumption: Using the existing shared MySQL profile name 'mysql-pomelo-v1' for both MySQL provider names will not confuse consumers if the emitted facts also make the actual provider or shared-profile semantics clear.
- Risky assumption: The downstream docs ticket 06FH8RMZPSZ7H3AQRP8FX72S08 will publish the same reviewed matrix so diagnostics and documentation do not drift.
- Split recommendation: Keep provider-specific execution or runtime crypto behavior out of this task; that belongs in later per-provider follow-on tickets.
- Split recommendation: Keep consumer-facing selection or configuration API work in ticket 06FH8RKDJTS3BB11J6J6QJVVD4.
- Split recommendation: Keep matrix publication and broader documentation rollout in ticket 06FH8RMZPSZ7H3AQRP8FX72S08.
- Split recommendation: If opt-in runtime probing is ever wanted, split it into a separate diagnostics ticket with its own redaction and secret-handling review.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9025`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `eb66690a959246c5b47f33a703b41bf5`
- completed-at-utc: `<redacted>-30T14:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RJF2SYBJ8ZM7ZDETDPN78/runs/20260630T145759279Z-eb66690a959246c5b47f33a703b41bf5.json`