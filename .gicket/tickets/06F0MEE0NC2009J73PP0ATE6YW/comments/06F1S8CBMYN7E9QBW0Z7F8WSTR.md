[gicket-bot] PO-critic review contract

Summary
- Approve: the story-level contract has no unresolved open questions, and the verified parentOf child set is done and covers schema, parser/diagnostics, YAML boundary, and import/projection scope.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The delivery contract states governance ticket 06F0MEGAGJCEHQ8QRHGH8W7804 remains a separate todo consumer and does not reopen import story scope.
- ParentOf relation files exist from 06F0MEE0NC2009J73PP0ATE6YW to 06F0MEE8T9PKPKQH8EPWNQ2CRW, 06F0MEEGJE9QCHC8YN4FEXYX10, 06F0MEERJ7D5Q4WYBQAJD3GFVC, and 06F0MEF08AJ1K52STF42T74B04.
- docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md defines the JSON-first dvault.model.v1 envelope, exact schemaVersion, defaults, token registry, unknown-field rejection, diagnostic codes, valid fixtures, and invalid fixture expectations.
- src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs and src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs are present and expose the model-first parse/import path into DataVaultMetadataModel and DataVaultMetadataRegistry.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs and src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs include import-result overloads for ApplyDataVaultMetadata and UseDataVaultMetadata, matching the story's existing DVault path requirement.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactParserTests.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs include visible coverage for valid artifacts, strict version handling, unknown/provider fields, references, duplicates, unsupported capabilities, PIT/bridge scenarios, registry use, and EF projection parity.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- none

Implementation watchouts
- For any downstream follow-up, preserve source-oriented diagnostics through parser, registry build, and EF projection stages.
- Keep governance documentation on 06F0MEGAGJCEHQ8QRHGH8W7804 separate from the import story closure.

Non-blocking notes
- none

Split recommendations
- No new split recommended; the existing child set covers schema, parser/diagnostics, YAML boundary, import/projection, with governance already tracked separately.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment