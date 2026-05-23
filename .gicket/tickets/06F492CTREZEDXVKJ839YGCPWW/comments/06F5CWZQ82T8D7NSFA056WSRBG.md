[gicket-bot] conflict escalation (human-needed)

- operation: `workflow-stop-the-line`
- outcome: `test-workflow-failed`
- current-revision: `06F5CVZEA9S32D2XJSAWBKFVY8`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Tester workflow for ticket '06F492CTREZEDXVKJ839YGCPWW' blocked durable writeback because the assessment echoed private prompt context.

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
  "observedAtUtc": "2026-05-23T20:08:21.0539507Z",
  "retryNotBeforeUtc": "2026-05-23T20:23:21.0539507Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "81c47141dc02249136d5848738a8f99008ea057d5b5a09ce7d0d3632170e1cb6",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```