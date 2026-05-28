[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F5Q922T5B21GJN49FYN6DJH0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q922T5B21GJN49FYN6DJH0`.
- Optimistic claim succeeded (`expectedRevision=06F6R390ZV50BC0QGCPD77NTM4`, `currentRevision=06F6R3HJV9FZ3C80XQZAHHM3RW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q922T5B21GJN49FYN6DJH0-story-define-typed-read-model-generator-contract' from source '831ef955a8d83929e0c77ae61a573d95e0a14e3e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F5Q922T5B21GJN49FYN6DJH0-story-define-typed-read-model-generator-contract` as `26953f0d6aff`.

Open questions / Risiken
- Risky assumption: The developer implementing this story will carry forward the asymmetry from docs/architecture/dvault-v1-pit-bridge-boundary.md:30-36 and :91-106 instead of silently treating all PIT shapes as available from all input modes.
- Risky assumption: The eventual contract will treat `current` and `asOf` helpers as convenience forms over the latest-satellite pipeline, consistent with src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs:7-61, rather than inventing new satellite semantics.
- Risky assumption: The stale-fingerprint diagnostics can be specified against the existing `MetadataSourceKind` and `MetadataSourceFingerprint` annotations in src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:72-80 without requiring a separate new PO story first.
- Split recommendation: Keep the current split: this parent story defines the contract, 06F5Q92AHG0ZCTVQGC6NAYVP9C handles latest/as-of satellite projector implementation, and 06F5Q92R02HB7FCE1AWKXPTMRW handles PIT/bridge projector implementation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8017`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `120b8628f269414aab43f7d1b8499a1b`
- completed-at-utc: `<redacted>-28T00:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q922T5B21GJN49FYN6DJH0/runs/20260528T005522607Z-120b8628f269414aab43f7d1b8499a1b.json`