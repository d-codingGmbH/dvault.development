[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F7Y0KGY29HHGZWHC470KVJBG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0KGY29HHGZWHC470KVJBG`.
- Optimistic claim succeeded (`expectedRevision=06F8F7N3KY0REJMVWXHRKF8YE4`, `currentRevision=06F8FCJNZPPP7XS46D6ZVAPKNR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru' from source '664a23565e23b9b2f3b89e2310d363a7cd3a6759'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru` as `4bb0ef33e81e`.

Open questions / Risiken
- Risky assumption: This review assumes 'named generated constraints' means the currently evidenced generated primary-key family unless PO intends broader constraint kinds; DataVaultDiagnostics.CreateEntityExplain currently materializes only primary-key constraints in DataVaultE...
- Risky assumption: This review assumes 'explicit intentional evolution' is primarily evidenced by clear EF rename or evolution operations and preserved logical continuity, not by arbitrary drop-and-add sequences with manually explained intent.
- Risky assumption: This review assumes provider-specific scaffolding differences may still surface as 'suspicious' rather than 'safe' when continuity evidence is weak, which is consistent with the ticket's stated risk section.
- Split recommendation: No split recommended; the contract already keeps this as one cohesive guardrail-strengthening story on the existing guardrail --migration surface.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8272`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `5a6ed806e42d413e8edf80de029bd4e3`
- completed-at-utc: `<redacted>-02T09:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0KGY29HHGZWHC470KVJBG/runs/20260602T094558914Z-5a6ed806e42d413e8edf80de029bd4e3.json`