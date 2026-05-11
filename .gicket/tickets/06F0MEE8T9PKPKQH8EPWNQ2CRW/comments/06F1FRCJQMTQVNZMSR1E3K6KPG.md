Developer delivery

- Created durable repository planning/spec document `docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md` for the JSON-first `dvault.model.v1` schema and validation contract.
- The document covers schema version compatibility, top-level fields, defaults, supported tokens, unknown-field handling, provider-choice limits, diagnostics taxonomy, valid fixture expectations, and invalid fixture expectations.
- Validation evidence: `bash tools/check-format.sh` passed with `Formatting check passed.`

Follow-up implementation tickets can now consume this contract for parser/diagnostics, YAML boundary, projection, and governance work.