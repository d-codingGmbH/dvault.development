[gicket-bot] dev-test ping-pong escalation (human-needed)

- operation: `dev-test-ping-pong`
- current-revision: `06F3VBY1Q6JTGPJDQCK8ATP11C`
- cooldown-seconds: `900`
- max-consecutive-dev-test-handoffs: `6`

Direct developer/tester handoffs would reach 7 consecutive steps, exceeding the configured limit of 6.

- source-role: `dev`
- target-role: `test`
- observed-consecutive-handoffs: `6`
- prospective-consecutive-handoffs: `7`
- chain: `dev->test -> test->dev -> dev->test -> test->dev -> dev->test -> test->dev -> dev->test`

[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "dev-test-ping-pong",
  "role": "dev",
  "outcome": "failed",
  "observedAtUtc": "2026-05-19T00:52:27.8711331Z",
  "retryNotBeforeUtc": "2026-05-19T01:07:27.8711331Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "8e61fff35e3375bc0111062c0b76a73bc8ae1bfb8e0c6b8dd69006d7e828bf99",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```