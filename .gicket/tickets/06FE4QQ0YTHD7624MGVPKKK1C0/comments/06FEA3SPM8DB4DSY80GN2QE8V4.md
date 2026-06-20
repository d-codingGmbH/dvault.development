[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'runtime-environment-precondition' for role 'dev'.

SQL Server provider-configured benchmark evidence was produced in this ticket branch. The run completed the latest-satellite row with `selectedStrategy=SqlServerDataVaultReadStrategy` and supports retaining the existing bounded SQL Server latest-satellite query shape.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-06-20T12:43:52.8645220Z",
  "operationToken": "runtime-environment-precondition",
  "reason": "SQL Server benchmark evidence materialized; latest-satellite row completed with SqlServerDataVaultReadStrategy selected.",
  "clearedBy": "Codex"
}
```