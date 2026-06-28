[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FGX69QJYHGNKBV8MJ1HG7MMG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX69QJYHGNKBV8MJ1HG7MMG`.
- Optimistic claim succeeded (`expectedRevision=06FGZQJEAB16JJ7SRQQZWPD39W`, `currentRevision=06FGZV54K3W88S2AS72CCSAHFR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife' from source '224f0befc743422a763bb541bb54942055952134'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife` as `54189ee9dc56`.

Open questions / Risiken
- Risky assumption: Future contributors may read `docs/hash-key-storage-migration.md` or `docs/plans/hash-key-storage-profile-contract.md` without the ticket context and assume the conceptual field names are the serialized v1 keys.
- Risky assumption: Validator fixture helpers will need to stay aligned with any later exporter-shape change because invalid manifests are intentionally hand-built from a valid current-shape artifact.
- Split recommendation: No split is needed for this ticket as currently refined.
- Split recommendation: If the team later wants a new serialized manifest shape, embedded validation payloads, or a successor manifest version, open a separate follow-up ticket rather than broadening this validator-only task.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8152`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `bfceabab33bd4017b037d666e38a124d`
- completed-at-utc: `<redacted>-28T20:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX69QJYHGNKBV8MJ1HG7MMG/runs/20260628T203754180Z-bfceabab33bd4017b037d666e38a124d.json`