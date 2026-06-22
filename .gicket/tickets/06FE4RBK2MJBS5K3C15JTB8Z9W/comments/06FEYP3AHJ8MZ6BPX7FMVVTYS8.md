[gicket-bot] runtime escalation resolved

The persistent stop for `model-execution` was caused by a technical model-output parsing failure: the captured response began with terminal escape/control bytes (`0x1B`, bracketed-paste toggles) instead of JSON. This is not a semantic blocker in the ticket.

Current branch review:
- Relation readiness is green for `06FE4RBK2MJBS5K3C15JTB8Z9W`.
- The previous PO-critic concern about missing privacy APIs is stale in this branch: the branch contains `DataVaultPrivacyOptions`, `RegisterEncryptedPayloadAlias`, `UseCallerOwnedKeyProvider`, `IDataVaultEncryptedPayloadKeyProvider`, and `DataVaultEncryptedPayloadValueConverter` as source-backed seams.
- Stale `blocked/dev`, `blocked/test`, and `needs-po` labels were removed; `critic-needed` was added so PO-Critic can re-evaluate the current branch state.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "operationToken": "model-execution",
  "role": "po",
  "outcome": "resolved",
  "observedAtUtc": "2026-06-22T11:40:39.1180529Z",
  "resolvedAtUtc": "2026-06-22T12:39:41.9362725Z",
  "reason": "Model response contained ANSI terminal escape/control sequences and was not parseable JSON; no ticket-content or relation blocker remains after branch review.",
  "returnToRole": "po-critic",
  "stopFurtherAutoWrites": false
}
```