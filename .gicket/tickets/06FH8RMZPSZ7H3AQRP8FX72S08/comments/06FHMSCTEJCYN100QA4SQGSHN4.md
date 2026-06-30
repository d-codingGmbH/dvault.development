[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8RMZPSZ7H3AQRP8FX72S08'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RMZPSZ7H3AQRP8FX72S08`.
- Optimistic claim succeeded (`expectedRevision=06FHMQEXTBN5S3XCCTY7B0Y0Y0`, `currentRevision=06FHMQWC02C6DVTM8C89P5W6MM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8RMZPSZ7H3AQRP8FX72S08-task-document-provider-native-crypto-capabilitie' from source '8227c80c971dd916c932e0e2fb147e75e840bf7f'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8RMZPSZ7H3AQRP8FX72S08-task-document-provider-native-crypto-capabilitie` as `660345ae5e97`.

Open questions / Risiken
- Risky assumption: The contract allows updating either the current release notes or the changelog; implementation should pick one current release surface and keep its wording aligned with the named docs.
- Risky assumption: The docs must stay at the same abstraction level as the redaction-safe diagnostics tests and must not drift into key-store, provider-provisioning, deletion, or compliance promises.
- Split recommendation: No split recommended; the scope is already bounded to one documentation-alignment task across named public docs and one current release surface.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7634`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `9d7610a4116848e2906e1dc1ef88f3e2`
- completed-at-utc: `<redacted>-30T21:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RMZPSZ7H3AQRP8FX72S08/runs/20260630T211814128Z-9d7610a4116848e2906e1dc1ef88f3e2.json`