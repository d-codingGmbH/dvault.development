[gicket-bot] conflict escalation (human-needed)

- operation: `workflow-stop-the-line`
- outcome: `test-workflow-failed`
- current-revision: `06F8XFG8KKGDRPS6867CPR1Y00`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Tester workflow for ticket '06F8KZJAKN7Q2QXXP9PRK2V94G' blocked durable writeback because the interactive assessment echoed private prompt context.

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
  "observedAtUtc": "2026-06-03T18:47:12.2451179Z",
  "retryNotBeforeUtc": "2026-06-03T19:02:12.2451179Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "52680a51130d91227d04b7f60addc550cbf74f11e9f95dee7e64ea6ab22c11e0",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```