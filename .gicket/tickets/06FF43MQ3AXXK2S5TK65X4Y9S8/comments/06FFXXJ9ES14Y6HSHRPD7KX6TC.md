[gicket-bot] conflict escalation (human-needed)

- operation: `model-execution`
- outcome: `failed`
- current-revision: `06FFXMPJ1W0J7W0YJRNXPX0QG8`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Developer workflow for ticket '06FF43MQ3AXXK2S5TK65X4Y9S8' failed during model execution.

Diagnostics:
- Provider returned a tool invocation outcome even though the request did not declare any tools.
- Normalized model execution failure category: `tool-loop-runtime`.
- Failure code: `MODEL-TOOL-INVOCATION-RESULT-WITHOUT-REQUEST`.
- External tool exit code: `<not available>`.
- External tool stderr tail: `<absent>`.
- External tool stdout tail: `<absent>`.
- Diagnostic artifact path: `<not available>`.
- Repository changes were not preserved in a failure-snapshot commit.

Suggested recovery:
- Inspect the tool-loop contract failure and retry after correcting the tool/provider interaction.

Model/tool-loop failure diagnostics:
- normalized-failure-category: `tool-loop-runtime`
- failure-code: `MODEL-TOOL-INVOCATION-RESULT-WITHOUT-REQUEST`
- exit-code: `<not available>`
- transient-external-tool-detected: `false`
- diagnostic-artifact-path: `<not available>`
- repository-changes-preserved: `false`
- failure-snapshot-commit: `<none>`
- stderr-tail: `<absent>`
- stdout-tail: `<absent>`

Operator recovery guidance:
- Inspect the model/tool-loop diagnostics above, resolve the external condition, then retry ticket processing.
- After investigation, clear the durable stop with `gicket-bot runtime-escalation resolve --id 06FF43MQ3AXXK2S5TK65X4Y9S8 --role dev --operation-token model-execution --reason "External model/tool condition cleared."`.

[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "model-execution",
  "role": "dev",
  "outcome": "failed",
  "observedAtUtc": "2026-06-25T13:26:54.0662738Z",
  "retryNotBeforeUtc": "2026-06-25T13:41:54.0662738Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "05730b866c0cae606fafbad1cfc45e8744b3a0cde33a492241af791134b3e8d1",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1",
  "diagnostics": {
    "model-failure.category": "tool-loop-runtime",
    "model-failure.code": "MODEL-TOOL-INVOCATION-RESULT-WITHOUT-REQUEST",
    "model-failure.external-transient-detected": "false",
    "model-failure.exit-code": null,
    "model-failure.stdout-tail": null,
    "model-failure.stderr-tail": null,
    "model-failure.diagnostic-artifact-path": null,
    "model-failure.failure-snapshot-preserved": "false",
    "model-failure.failure-snapshot-commit": null
  }
}
```