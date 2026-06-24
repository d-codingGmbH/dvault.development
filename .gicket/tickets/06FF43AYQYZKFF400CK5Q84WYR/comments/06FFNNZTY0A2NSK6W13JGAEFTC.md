[gicket-bot] integrator-decision-v1

```json
{
  "decision": "ACCEPT",
  "reason": "Automatic integration via \u0027squash\u0027 accepted the verified branch after tester handoff.",
  "returnTarget": null,
  "conditions": {
    "baseBranch": "develop",
    "mode": "squash",
    "sourceBranch": "ticket/06FF43AYQYZKFF400CK5Q84WYR-task-add-sql-server-pit-full-rebuild-benchmark-l"
  }
}
```

[gicket-bot] runtime-orchestration template

- template: `integrator-decision`
- transaction-point: `TP5`
- ticket-id: `06FF43AYQYZKFF400CK5Q84WYR`
- target-role: `integrator`
- decision: `ACCEPT`
- reason: Automatic integration via 'squash' accepted the verified branch after tester handoff.
- return-target: `<none>`
- conditions: `baseBranch, mode, sourceBranch`

<!-- gicket-semantic-idempotency-key: bot-writeback:06ff43ayqyzkff400ck5q84wyr:audit-only:writeback:tp6:wg-close:integrator:45d30f5fd34d32fc -->