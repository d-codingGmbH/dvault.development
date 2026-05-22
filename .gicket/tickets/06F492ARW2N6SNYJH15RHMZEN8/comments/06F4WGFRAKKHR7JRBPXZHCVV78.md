[gicket-bot] conflict escalation (human-needed)

- operation: `workflow-stop-the-line`
- outcome: `test-workflow-failed`
- current-revision: `06F4WF4AR9CXWK98Y865V2M6ZM`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Tester workflow for ticket '06F492ARW2N6SNYJH15RHMZEN8' blocked durable writeback because the assessment echoed private prompt context.

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
  "observedAtUtc": "2026-05-22T05:56:46.8022234Z",
  "retryNotBeforeUtc": "2026-05-22T06:11:46.8022234Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "3177f1a0fcaaaba9fde513c3d8d6ac8bf4993c72281b6bb9a5e1fc23b2fee7f1",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```