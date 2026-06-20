[gicket-bot] conflict escalation (human-needed)

- operation: `runtime-environment-precondition`
- outcome: `failed`
- current-revision: `06FE9MKHSH0AYBR67QD68G95W4`
- cooldown-seconds: `21600`
- stop-further-auto-writes: `False`

Runtime blocked before SQL Server latest-satellite evidence or tuning could be completed: the SQL Server connection string is unset and the local restore cache is missing required EF Core analyzer packages. No repository files were changed.

Risk: Changing the SQL Server latest-satellite SQL shape without provider-configured evidence would be speculative and could regress current/as-of correctness or parameter-limit batching.
Risk: The root benchmark placeholder must not be promoted as measured SQL Server latest-satellite timing while the SQL Server connection string remains unset.
Risk: Validation remains incomplete until the missing EF Core analyzer packages are restored or otherwise available to the local build/test runtime.
Resolve runtime precondition: Changing the SQL Server latest-satellite SQL shape without provider-configured evidence would be speculative and could regress current/as-of correctness or parameter-limit batching.
Resolve runtime precondition: The root benchmark placeholder must not be promoted as measured SQL Server latest-satellite timing while the SQL Server connection string remains unset.
Resolve runtime precondition: Validation remains incomplete until the missing EF Core analyzer packages are restored or otherwise available to the local build/test runtime.



[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "runtime-environment-precondition",
  "role": "dev",
  "outcome": "failed",
  "observedAtUtc": "2026-06-20T11:43:48.1767365Z",
  "retryNotBeforeUtc": "2026-06-20T17:43:48.1767365Z",
  "cooldownSeconds": 21600,
  "errorFingerprint": "c8ec96b5fa907371306fcf53ee81bc34a416a55ba60eb2f6fb9f14fb6f1b8a20",
  "stopFurtherAutoWrites": false,
  "instanceId": "hp-ai-2026-001.1"
}
```