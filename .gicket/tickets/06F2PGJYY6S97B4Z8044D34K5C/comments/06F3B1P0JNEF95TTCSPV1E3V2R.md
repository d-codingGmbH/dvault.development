[gicket-bot] dev-test ping-pong escalation (human-needed)

- operation: `dev-test-ping-pong`
- current-revision: `06F3B0HW1YJ088EP4RQ8QKV3F8`
- cooldown-seconds: `900`
- max-consecutive-dev-test-handoffs: `6`

Direct developer/tester handoffs would reach 7 consecutive steps, exceeding the configured limit of 6.

- source-role: `test`
- target-role: `dev`
- observed-consecutive-handoffs: `6`
- prospective-consecutive-handoffs: `7`
- chain: `test->dev -> dev->test -> test->dev -> dev->test -> test->dev -> dev->test -> test->dev`

[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "dev-test-ping-pong",
  "role": "test",
  "outcome": "failed",
  "observedAtUtc": "2026-05-17T10:41:23.9650629Z",
  "retryNotBeforeUtc": "2026-05-17T10:56:23.9650629Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "7234afc5a850db5e13e491a78c0b340f7571207181b5a51753b2a46892715f2e",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```