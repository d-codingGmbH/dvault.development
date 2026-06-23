[gicket-bot] integrator-decision-v1

```json
{
  "decision": "ACCEPT",
  "reason": "Automatic integration via \u0027squash\u0027 accepted the verified branch after tester handoff.",
  "returnTarget": null,
  "conditions": {
    "baseBranch": "develop",
    "mode": "squash",
    "sourceBranch": "ticket/06FF43CJ9CJMG7J917RW22QKJC-task-evaluate-mysql-pit-full-rebuild-push-down-f"
  }
}
```

[gicket-bot] runtime-orchestration template

- template: `integrator-decision`
- transaction-point: `TP5`
- ticket-id: `06FF43CJ9CJMG7J917RW22QKJC`
- target-role: `integrator`
- decision: `ACCEPT`
- reason: Automatic integration via 'squash' accepted the verified branch after tester handoff.
- return-target: `<none>`
- conditions: `baseBranch, mode, sourceBranch`

<!-- gicket-semantic-idempotency-key: bot-writeback:06ff43cj9cjmg7j917rw22qkjc:audit-only:writeback:tp6:wg-close:integrator:30a5c046e7415b8b -->