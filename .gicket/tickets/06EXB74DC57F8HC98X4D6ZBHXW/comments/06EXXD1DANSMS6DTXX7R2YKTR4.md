[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'dev-test-ping-pong' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-04-30T14:04:02.3221726Z",
  "operationToken": "dev-test-ping-pong",
  "reason": "Reviewed ping-pong: tester blocker was the repository check-format script defect. tools/check-format.sh is fixed on develop and this active branch; bash tools/check-format.sh, dotnet build DVault.slnx --nologo, and dotnet test --nologo now pass. Continue automation with the dev/test chain reset.",
  "clearedBy": "codex"
}
```