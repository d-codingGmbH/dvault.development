[gicket-bot] conflict escalation (human-needed)

- operation: `workflow-stop-the-line`
- outcome: `test-workflow-failed`
- current-revision: `06FBEY001X21FNCCDV6K04ZF74`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Tester workflow for ticket '06F9GF5FV54DGWY9GA8ZEZWM5R' blocked durable writeback because the interactive assessment echoed private prompt context.

Open questions or risks:
- Private prompt-context echo detected in tester interactive assessment writeback; blocked durable writeback for surface(s): tester.assessment.next-steps.

Next steps:
- Re-run tester assessment after ensuring private prompt context is not copied into assessment findings, evidence, or comments.



[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "workflow-stop-the-line",
  "role": "test",
  "outcome": "test-workflow-failed",
  "observedAtUtc": "2026-06-11T16:38:47.2660699Z",
  "retryNotBeforeUtc": "2026-06-11T16:53:47.2660699Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "b0c3d6823afa0d40e657d510486f62f4c702d9528d589ed9c7af612795cef891",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```