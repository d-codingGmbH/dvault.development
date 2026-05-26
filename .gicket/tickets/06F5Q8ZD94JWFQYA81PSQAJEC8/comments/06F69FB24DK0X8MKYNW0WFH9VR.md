[gicket-bot] conflict escalation (human-needed)

- operation: `workflow-stop-the-line`
- outcome: `test-workflow-failed`
- current-revision: `06F69D56W5XH8RWK9D38WV4WZ0`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Tester workflow for ticket '06F5Q8ZD94JWFQYA81PSQAJEC8' blocked durable writeback because verification output echoed private prompt context.

Open questions or risks:
- Private prompt-context echo detected in tester verification writeback; blocked durable writeback for surface(s): tester.verification.evidence.

Next steps:
- Re-run tester verification after ensuring private prompt context is not copied into verification findings, evidence, or comments.



[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "workflow-stop-the-line",
  "role": "test",
  "outcome": "test-workflow-failed",
  "observedAtUtc": "2026-05-26T14:43:13.5694734Z",
  "retryNotBeforeUtc": "2026-05-26T14:58:13.5694734Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "513771d1a016c5508586d7bc1a926a91a725a7b6a1ffcf008c630c27ee1a1e0b",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1"
}
```