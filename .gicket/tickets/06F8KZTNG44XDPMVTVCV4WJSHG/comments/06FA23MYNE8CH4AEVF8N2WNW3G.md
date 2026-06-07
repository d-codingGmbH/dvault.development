[gicket-bot] integrator-decision-v1

```json
{
  "decision": "ACCEPT",
  "reason": "Automatic integration recovered an integration-ready active ticket branch under the test role.",
  "returnTarget": null,
  "conditions": {
    "baseBranch": "develop",
    "mode": "squash",
    "sourceBranch": "ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a"
  }
}
```

[gicket-bot] runtime-orchestration template

- template: `integrator-decision`
- transaction-point: `TP5`
- ticket-id: `06F8KZTNG44XDPMVTVCV4WJSHG`
- target-role: `integrator`
- decision: `ACCEPT`
- reason: Automatic integration recovered an integration-ready active ticket branch under the test role.
- return-target: `<none>`
- conditions: `baseBranch, mode, sourceBranch`

<!-- gicket-semantic-idempotency-key: bot-writeback:06f8kztng44xdpmvtvcv4wjshg:audit-only:writeback:tp6:wg-close:integrator:d360745f0b8cdfda -->