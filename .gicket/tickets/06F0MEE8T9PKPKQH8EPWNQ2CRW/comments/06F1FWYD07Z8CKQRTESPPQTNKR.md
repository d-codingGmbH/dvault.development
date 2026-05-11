Developer delivery rework

- Added repository artifact `docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md` as the durable `dvault.model.v1` schema and validation contract.
- The contract fixes top-level field names, supported tokens, defaults, version compatibility, unknown-field policy, diagnostics categories/codes, valid fixture expectations, and invalid fixture expectations.
- Verification: `git diff --check -- docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md` passed.
- Verification: `bash tools/check-format.sh` exited 0 with `Formatting check passed.` The script also reported its existing solution-workspace warning and successful folder whitespace fallback.

Downstream parser/diagnostics, YAML boundary, projection, and governance tickets can now validate this repository path directly instead of relying on ticket-only prose.