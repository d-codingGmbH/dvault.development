[gicket-bot] integrator-decision-v1

```json
{
  "decision": "ACCEPT",
  "reason": "Automatic integration via \u0027squash\u0027 accepted the verified branch after tester handoff.",
  "returnTarget": null,
  "conditions": {
    "baseBranch": "develop",
    "mode": "squash",
    "sourceBranch": "ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi"
  }
}
```

[gicket-bot] runtime-orchestration template

- template: `integrator-decision`
- transaction-point: `TP5`
- ticket-id: `06FF439ETZKD6WBB5G2MPS9EG8`
- target-role: `integrator`
- decision: `ACCEPT`
- reason: Automatic integration via 'squash' accepted the verified branch after tester handoff.
- return-target: `<none>`
- conditions: `baseBranch, mode, sourceBranch`

<!-- gicket-semantic-idempotency-key: bot-writeback:06ff439etzkd6wbb5g2mps9eg8:audit-only:writeback:tp6:wg-close:integrator:977a312f72110a64 -->