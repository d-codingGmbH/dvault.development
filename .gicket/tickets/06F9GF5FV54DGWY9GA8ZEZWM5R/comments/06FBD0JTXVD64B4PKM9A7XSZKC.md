[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F9GF5FV54DGWY9GA8ZEZWM5R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9GF5FV54DGWY9GA8ZEZWM5R`.
- Optimistic claim succeeded (`expectedRevision=06FBCYK8SV54ZZ5BGKWGNVHDQC`, `currentRevision=06FBCYWXH1MWPAGNF6HRG6M57R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract' from source 'a9f9d2de8e7ecf51393949858723005027d3279f'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract` as `5096ea7a0a9f`.

Open questions / Risiken
- Risky assumption: docs/plans/provider-identifier-ddl-guardrail-contract.md still reflects an older five-provider baseline, so downstream readers must treat this ticket contract plus DataVaultProviderCapabilityProfiles.cs as the current six-profile source of truth.
- Risky assumption: The drift-check story assumes teams actually regenerate and review a fresh dvault.support-bundle.v1 baseline when algorithm or storage-profile inputs change.
- Split recommendation: If scope has to shrink later, keep this ticket as the contract parent and split provider-profile/EF-annotation storage-shape work from migration, live-schema, and explain/preflight guardrail work, matching the current Delivery Contract.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9189`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `3dfe79fd9dd94455b6653b6f0af53358`
- completed-at-utc: `<redacted>-11T11:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/runs/20260611T114736809Z-3dfe79fd9dd94455b6653b6f0af53358.json`