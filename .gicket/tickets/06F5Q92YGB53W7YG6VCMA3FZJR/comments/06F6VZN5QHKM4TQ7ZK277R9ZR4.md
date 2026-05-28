[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F5Q92YGB53W7YG6VCMA3FZJR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92YGB53W7YG6VCMA3FZJR`.
- Optimistic claim succeeded (`expectedRevision=06F6VXG729QBRE45RC1PA2CR0M`, `currentRevision=06F6VXSEK0Y2DSVJVRQTJ3A3G4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea' from source '51a9a856d62a93653976e04cd35058adfa7737c6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea` as `5f4f2ad71ddc`.

Open questions / Risiken
- Risky assumption: The developer will classify ambiguous non-satellite cases with the shared contract in docs/plans/typed-read-model-generator-contract.md rather than inventing provider-specific behavior, especially at the DMV1967 versus DMV1969 boundary.
- Risky assumption: Legacy fingerprint compatibility is assumed to remain in place during this ticket because DataVaultTypedReadModelSourceGenerator.cs still defines `LegacyExpectedFingerprintProperty` and the ticket defers any deprecation to a later follow-up.
- Split recommendation: No further split is needed if this ticket stays limited to residual diagnostics and tests.
- Split recommendation: Keep satellite work on 06F5Q92AHG0ZCTVQGC6NAYVP9C, PIT/bridge helper generation on 06F5Q92R02HB7FCE1AWKXPTMRW, and documentation rollup on 06F5Q93H60W6X8FJ88PWTR6NG4.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8334`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7426b850729c4532b76b4f97f90d1a0e`
- completed-at-utc: `<redacted>-28T09:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/runs/20260528T095105658Z-7426b850729c4532b76b4f97f90d1a0e.json`