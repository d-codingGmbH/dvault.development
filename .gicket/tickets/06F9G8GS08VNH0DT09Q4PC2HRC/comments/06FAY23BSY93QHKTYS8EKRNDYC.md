[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F9G8GS08VNH0DT09Q4PC2HRC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9G8GS08VNH0DT09Q4PC2HRC`.
- Optimistic claim succeeded (`expectedRevision=06FAXZFD2D9GASEJNYJ0Q78PAW`, `currentRevision=06FAXZPQH2C866CB96PN5FF65G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F9G8GS08VNH0DT09Q4PC2HRC-story-define-db2-provider-capability-and-depende' from source 'e906db0ebf9a3b502b2ed798831f1dc246ea4d95'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F9G8GS08VNH0DT09Q4PC2HRC-story-define-db2-provider-capability-and-depende` as `7ca2002683e4`.

Open questions / Risiken
- Risky assumption: Assumes IBM.EntityFrameworkCore exposes a stable provider-name string that can be matched explicitly across capability selection, diagnostics, and registration without alias drift.
- Risky assumption: Assumes DB2 can reuse the current `DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported` and `DataVaultProviderConcurrencySupport.NoneInV1Unsupported` defaults unless the delivered contract records an exception.
- Risky assumption: Assumes DB2 can follow the existing `ProviderIntegration.ExternalOptIn` consumer-managed test posture without requiring DVault-owned provisioning, credentials, container lifecycle, or CI infrastructure.
- Split recommendation: No further split recommended; the current epic already separates contract definition, package wiring, schema/guardrail work, integration coverage, package verification, and documentation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9157`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `fde89a19a9174d47b41e13622fd919ba`
- completed-at-utc: `<redacted>-10T00:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9G8GS08VNH0DT09Q4PC2HRC/runs/20260610T005705224Z-fde89a19a9174d47b41e13622fd919ba.json`