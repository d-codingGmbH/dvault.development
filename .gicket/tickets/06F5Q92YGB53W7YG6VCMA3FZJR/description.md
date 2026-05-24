Goal: Add analyzer guidance around generated typed read model usage.

Acceptance criteria:
- Reports stale generated code, unsupported metadata inputs, unsafe dynamic assumptions, and direct generated-table misuse where applicable.
- Provides safe code fixes only where deterministic and local.
- Keeps analyzer package optional with PrivateAssets guidance.