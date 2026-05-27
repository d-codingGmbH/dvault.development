[gicket-bot] integrator-decision-v1

```json
{
  "decision": "ACCEPT",
  "reason": "Automatic integration via \u0027squash\u0027 accepted the verified branch after tester handoff.",
  "returnTarget": null,
  "conditions": {
    "baseBranch": "develop",
    "mode": "squash",
    "sourceBranch": "ticket/06F5Q91M0PM17RP43ZQRPBDXP0-task-update-v0-21-0-pit-and-bridge-completeness"
  }
}
```

[gicket-bot] runtime-orchestration template

- template: `integrator-decision`
- transaction-point: `TP5`
- ticket-id: `06F5Q91M0PM17RP43ZQRPBDXP0`
- target-role: `integrator`
- decision: `ACCEPT`
- reason: Automatic integration via 'squash' accepted the verified branch after tester handoff.
- return-target: `<none>`
- conditions: `baseBranch, mode, sourceBranch`

<!-- gicket-semantic-idempotency-key: bot-writeback:06f5q91m0pm17rp43zqrpbdxp0:audit-only:writeback:tp6:wg-close:integrator:be8ce020794b658d -->