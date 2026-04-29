[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'dev-test-ping-pong' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-04-29T22:08:50.8442737Z",
  "operationToken": "dev-test-ping-pong",
  "reason": "Human repair completed after tester returned due missing direct verification evidence. Local policies in DVault and DVault2 now allow git grep and bash tools/check-format.sh, and tester test-commands now include dotnet build DVault.slnx --nologo, bash tools/check-format.sh, dotnet build --nologo, and dotnet test --nologo.",
  "clearedBy": "Codex"
}
```