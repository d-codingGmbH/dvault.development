[gicket-bot] conflict escalation (human-needed)

- operation: `model-execution`
- outcome: `failed`
- current-revision: `06EXKCJSJY1DNVX1QVX546PB34`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Developer workflow for ticket '06EXB6XVWBWZGN6MA3SFWGWKM4' failed during model execution.

Unhandled external-program execution failure for protocol 'openai-codex-cli-v1': ArgumentException: An item with the same key has already been added. Key: clarification_category (Parameter 'key')

[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "model-execution",
  "role": "dev",
  "outcome": "failed",
  "observedAtUtc": "2026-04-29T14:50:20.5819962Z",
  "retryNotBeforeUtc": "2026-04-29T15:05:20.5819962Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "314cc3dcb0748a0aef1e2b964e0abb07f78cc322b3e4db4396ae1af231a74423",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.2"
}
```