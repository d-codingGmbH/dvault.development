[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FE4R1C96NBSNMM7AFDTHJ7A4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R1C96NBSNMM7AFDTHJ7A4`.
- Optimistic claim succeeded (`expectedRevision=06FEH1VW7CJC48FTS4HWSS2F0R`, `currentRevision=06FEH7DE1P9N3MMBS9M402JYGG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4R1C96NBSNMM7AFDTHJ7A4-task-improve-code-first-binary-first-profile-erg' from source 'bb5e81b9ec5e7b5c782cba5a50873b7707baad5d'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FE4R1C96NBSNMM7AFDTHJ7A4-task-improve-code-first-binary-first-profile-erg` as `684858a95a71`.

Open questions / Risiken
- Risky assumption: The ticket assumes one additive call-site convenience can be introduced without widening into a general configuration DSL even though the current public surface already includes `ApplyDataVaultMetadata(..., providerCapabilities)` and `DataVaultProviderCapabil...
- Risky assumption: The ticket assumes any minimal discoverability guidance touched for the new entry point can stay local to this ergonomics change while broader documentation consolidation remains with `06FE4R2EGQ444EGPKZBRZCDEV8`.
- Split recommendation: No additional split is justified from the current repository and ticket evidence; this remains the dedicated code-first ergonomics slice separate from analyzer work and downstream docs consolidation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8696`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `78a0dc3025ce46f3b94a9d9ae2a98c4d`
- completed-at-utc: `<redacted>-21T05:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R1C96NBSNMM7AFDTHJ7A4/runs/20260621T052601743Z-78a0dc3025ce46f3b94a9d9ae2a98c4d.json`