[gicket-bot] conflict escalation (human-needed)

- operation: `workflow-stop-the-line`
- outcome: `test-workflow-failed`
- current-revision: `06F4SVXXXF12K2745G7S6JNAX8`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Tester workflow for ticket '06F492AYE4A3PKA2D20DDPQ37C' blocked durable writeback because the assessment echoed private prompt context.

Open questions or risks:
- Private prompt-context echo detected in tester assessment writeback; blocked durable writeback for surface(s): tester.assessment.next-steps.

Next steps:
- Re-run tester assessment after ensuring private prompt context is not copied into assessment findings, evidence, or comments.



[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "workflow-stop-the-line",
  "role": "test",
  "outcome": "test-workflow-failed",
  "observedAtUtc": "2026-05-21T23:54:07.8819231Z",
  "retryNotBeforeUtc": "2026-05-22T00:09:07.8819231Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "005a7b1dba9c50e7265bd4a37fd60e6ef7962fd1b046c8afbef2c3c862d05303",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```