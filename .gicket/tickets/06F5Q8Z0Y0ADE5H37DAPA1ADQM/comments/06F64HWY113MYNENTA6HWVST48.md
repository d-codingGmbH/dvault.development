[gicket-bot] conflict escalation (human-needed)

- operation: `workflow-stop-the-line`
- outcome: `test-workflow-failed`
- current-revision: `06F64GQW18P34Y07BDNQXSNT2M`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Tester workflow for ticket '06F5Q8Z0Y0ADE5H37DAPA1ADQM' blocked durable writeback because the assessment echoed private prompt context.

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
  "observedAtUtc": "2026-05-26T03:15:21.2216180Z",
  "retryNotBeforeUtc": "2026-05-26T03:30:21.2216180Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "1154099cb8f323497115cdbce570b36b94972f301348da23af11c461545939c1",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```