[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSC3V8NQS032B8MK84FMGVC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC3V8NQS032B8MK84FMGVC`.
- Optimistic claim succeeded (`expectedRevision=06FCP09F4AE8P9J9JPFF9JVZAW`, `currentRevision=06FCP0FVTMK838C3ZPX97ACYWW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSC3V8NQS032B8MK84FMGVC-task-add-provider-evidence-manifest-shape' from source 'bba2049331e7b5cbad987d674ee473a9914952ec'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSC3V8NQS032B8MK84FMGVC-task-add-provider-evidence-manifest-shape` as `25611a0b7bb4`.

Open questions / Risiken
- Risky assumption: The ticket assumes one manifest identity can cover both benchmark-backed rows and docs-owned evidence-matrix rows without needing extra identity fields beyond the bounded provider-evidence family.
- Risky assumption: The ticket assumes current deterministic executionDetail fragments are sufficient interim source material for planned-path mapping until a shared mapper replaces ad hoc prose parsing.
- Split recommendation: No split recommended. Defining the manifest shape, pinning the source vocabularies, and proving the row-to-manifest mapping still fits one bounded developer ticket.
- Split recommendation: If the team later wants a generated or checked-in provider-evidence manifest artifact, track that as the separate follow-up already called out in the ticket.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9044`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `8dc18f5941c145afad1919a438b9f08a`
- completed-at-utc: `<redacted>-15T11:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC3V8NQS032B8MK84FMGVC/runs/20260615T112703460Z-8dc18f5941c145afad1919a438b9f08a.json`