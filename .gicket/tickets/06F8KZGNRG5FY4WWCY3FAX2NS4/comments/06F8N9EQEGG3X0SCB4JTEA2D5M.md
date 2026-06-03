[gicket-bot] conflict escalation (human-needed)

- operation: `model-execution`
- outcome: `failed`
- current-revision: `06F8N96GQ7KRHPJN42ESXY2VFW`
- cooldown-seconds: `900`
- stop-further-auto-writes: `True`

Developer workflow for ticket '06F8KZGNRG5FY4WWCY3FAX2NS4' failed during model execution.

Diagnostics:
- Codex CLI role execution exited with code 1.
- Normalized model execution failure category: `external-tool-nonzero-exit`.
- Failure code: `BOT-EXTERNAL-PROGRAM-NONZERO-EXIT`.
- External tool exit code: `1`.
- External tool stderr tail: `<absent>`.
- External tool stdout tail: ojectionInvocations.IsEmpty) {\n          continue;\n        }\n\n        hasDataVaultProjection = true;\n\n        foreach (var projectionInvocation in projectionInvocations) {\n          AddVaryingContextMemberReferences(\n              semanticModel,\n              projectionInvocation.Syntax,\n              typeSymbol,\n              varyingMembers,\n              ref reportLocation,\n              cancellationToken);\n\n          foreach (var controlExpression in GetProjectionControlExpressions(projectionInvocation.Syntax)) {\n            AddVaryingContextMemberReferences(\n                semanticModel,\n","exit_code":0,"status":"completed"}}
{"type":"item.completed","item":{"id":"item_3","type":"command_execution","command":"/bin/bash -lc 'git diff -- src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs'","aggregated_output":"","exit_code":0,"status":"completed"}}
{"type":"error","message":"{\n  \"type\": \"error\",\n  \"error\": {\n    \"type\": \"image_generation_user_error\",\n    \"code\": \"invalid_value\",\n    \"message\": \"The model 'gpt-image-2' does not exist.\",\n    \"param\": \"tools\"\n  },\n  \"status\": 400\n}"}
{"type":"turn.failed","error":{"message":"{\n  \"type\": \"error\",\n  \"error\": {\n    \"type\": \"image_generation_user_error\",\n    \"code\": \"invalid_value\",\n    \"message\": \"The model 'gpt-image-2' does not exist.\",\n    \"param\": \"tools\"\n  },\n  \"status\": 400\n}"}}
- Diagnostic artifact path: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/status/role-execution-boundary.dev.role-execution-boundary.v1.json`.
- Repository changes were not preserved in a failure-snapshot commit.

Suggested recovery:
- Inspect the external tool configuration/output and retry after the external condition is resolved.

Model/tool-loop failure diagnostics:
- normalized-failure-category: `external-tool-nonzero-exit`
- failure-code: `BOT-EXTERNAL-PROGRAM-NONZERO-EXIT`
- exit-code: `1`
- transient-external-tool-detected: `false`
- diagnostic-artifact-path: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/status/role-execution-boundary.dev.role-execution-boundary.v1.json`
- repository-changes-preserved: `false`
- failure-snapshot-commit: `<none>`
- stderr-tail: `<absent>`
- stdout-tail:
```text
ojectionInvocations.IsEmpty) {\n          continue;\n        }\n\n        hasDataVaultProjection = true;\n\n        foreach (var projectionInvocation in projectionInvocations) {\n          AddVaryingContextMemberReferences(\n              semanticModel,\n              projectionInvocation.Syntax,\n              typeSymbol,\n              varyingMembers,\n              ref reportLocation,\n              cancellationToken);\n\n          foreach (var controlExpression in GetProjectionControlExpressions(projectionInvocation.Syntax)) {\n            AddVaryingContextMemberReferences(\n                semanticModel,\n","exit_code":0,"status":"completed"}}
{"type":"item.completed","item":{"id":"item_3","type":"command_execution","command":"/bin/bash -lc 'git diff -- src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs'","aggregated_output":"","exit_code":0,"status":"completed"}}
{"type":"error","message":"{\n  \"type\": \"error\",\n  \"error\": {\n    \"type\": \"image_generation_user_error\",\n    \"code\": \"invalid_value\",\n    \"message\": \"The model 'gpt-image-2' does not exist.\",\n    \"param\": \"tools\"\n  },\n  \"status\": 400\n}"}
{"type":"turn.failed","error":{"message":"{\n  \"type\": \"error\",\n  \"error\": {\n    \"type\": \"image_generation_user_error\",\n    \"code\": \"invalid_value\",\n    \"message\": \"The model 'gpt-image-2' does not exist.\",\n    \"param\": \"tools\"\n  },\n  \"status\": 400\n}"}}
```

Operator recovery guidance:
- Inspect the model/tool-loop diagnostics above, resolve the external condition, then retry ticket processing.
- After investigation, clear the durable stop with `gicket-bot runtime-escalation resolve --id 06F8KZGNRG5FY4WWCY3FAX2NS4 --role dev --operation-token model-execution --reason "External model/tool condition cleared."`.

[gicket-bot] runtime-escalation-v1

```json
{
  "operationToken": "model-execution",
  "role": "dev",
  "outcome": "failed",
  "observedAtUtc": "2026-06-02T23:23:04.9445393Z",
  "retryNotBeforeUtc": "2026-06-02T23:38:04.9445393Z",
  "cooldownSeconds": 900,
  "errorFingerprint": "fb0b7910d3243b44944d613433f05d343c29692936d6943eab13b0254440fa09",
  "stopFurtherAutoWrites": true,
  "instanceId": "hp-ai-2026-001.1",
  "diagnostics": {
    "model-failure.category": "external-tool-nonzero-exit",
    "model-failure.code": "BOT-EXTERNAL-PROGRAM-NONZERO-EXIT",
    "model-failure.external-transient-detected": "false",
    "model-failure.exit-code": "1",
    "model-failure.stdout-tail": "ojectionInvocations.IsEmpty) {\\n          continue;\\n        }\\n\\n        hasDataVaultProjection = true;\\n\\n        foreach (var projectionInvocation in projectionInvocations) {\\n          AddVaryingContextMemberReferences(\\n              semanticModel,\\n              projectionInvocation.Syntax,\\n              typeSymbol,\\n              varyingMembers,\\n              ref reportLocation,\\n              cancellationToken);\\n\\n          foreach (var controlExpression in GetProjectionControlExpressions(projectionInvocation.Syntax)) {\\n            AddVaryingContextMemberReferences(\\n                semanticModel,\\n\u0022,\u0022exit_code\u0022:0,\u0022status\u0022:\u0022completed\u0022}}\n{\u0022type\u0022:\u0022item.completed\u0022,\u0022item\u0022:{\u0022id\u0022:\u0022item_3\u0022,\u0022type\u0022:\u0022command_execution\u0022,\u0022command\u0022:\u0022/bin/bash -lc \u0027git diff -- src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027\u0022,\u0022aggregated_output\u0022:\u0022\u0022,\u0022exit_code\u0022:0,\u0022status\u0022:\u0022completed\u0022}}\n{\u0022type\u0022:\u0022error\u0022,\u0022message\u0022:\u0022{\\n  \\\u0022type\\\u0022: \\\u0022error\\\u0022,\\n  \\\u0022error\\\u0022: {\\n    \\\u0022type\\\u0022: \\\u0022image_generation_user_error\\\u0022,\\n    \\\u0022code\\\u0022: \\\u0022invalid_value\\\u0022,\\n    \\\u0022message\\\u0022: \\\u0022The model \u0027gpt-image-2\u0027 does not exist.\\\u0022,\\n    \\\u0022param\\\u0022: \\\u0022tools\\\u0022\\n  },\\n  \\\u0022status\\\u0022: 400\\n}\u0022}\n{\u0022type\u0022:\u0022turn.failed\u0022,\u0022error\u0022:{\u0022message\u0022:\u0022{\\n  \\\u0022type\\\u0022: \\\u0022error\\\u0022,\\n  \\\u0022error\\\u0022: {\\n    \\\u0022type\\\u0022: \\\u0022image_generation_user_error\\\u0022,\\n    \\\u0022code\\\u0022: \\\u0022invalid_value\\\u0022,\\n    \\\u0022message\\\u0022: \\\u0022The model \u0027gpt-image-2\u0027 does not exist.\\\u0022,\\n    \\\u0022param\\\u0022: \\\u0022tools\\\u0022\\n  },\\n  \\\u0022status\\\u0022: 400\\n}\u0022}}",
    "model-failure.stderr-tail": null,
    "model-failure.diagnostic-artifact-path": "C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/status/role-execution-boundary.dev.role-execution-boundary.v1.json",
    "model-failure.failure-snapshot-preserved": "false",
    "model-failure.failure-snapshot-commit": null
  }
}
```