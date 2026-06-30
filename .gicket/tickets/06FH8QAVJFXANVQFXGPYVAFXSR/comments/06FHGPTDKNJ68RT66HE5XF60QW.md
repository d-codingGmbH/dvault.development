[gicket-bot] manual lease cleanup

The previous dev claim was left locally after a bot writeback push failure. Manual recovery completed the requested ticket-text rework and releases the dev lease before routing back to test.

[gicket-bot] lease-state-v1 (event: released)

```json
{
  "owner": "hp-ai-2026-001.1",
  "role": "dev",
  "acquired": "2026-06-30T11:06:38.3540905Z",
  "expires": "2026-06-30T13:06:38.3540905Z",
  "version": 22,
  "state": "released",
  "runtime": {
    "runId": "manual-codex-recovery",
    "checkoutBranch": "ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp",
    "heartbeatObservedAt": "2026-06-30T11:47:31.5282985Z"
  }
}
```

<!-- gicket-semantic-idempotency-key: manual-lease-release:06fh8qavjfxanvqfxgpyvafxsr:dev:20260630T114731Z -->