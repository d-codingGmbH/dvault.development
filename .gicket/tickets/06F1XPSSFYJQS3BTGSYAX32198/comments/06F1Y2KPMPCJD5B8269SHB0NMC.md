[gicket-bot] PO-critic review contract

Summary
- Prior PO-critic gaps are resolved; the persisted contract now gives an explicit 18-code importer/projection seed set, a concrete per-entry documentation contract, clear scope boundaries, and source-backed regression expectations, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPSSFYJQS3BTGSYAX32198/description.md now explicitly enumerates the 18 in-scope codes DMV1001-DMV1801 in both `Clarifications` and `Acceptance Criteria`, requires per-entry fields `code`, `severity`, `category`, `summary/title`, `explanation`, and `remediation`, and its `## Open Questions` section is `- none`.
- .gicket/tickets/06F1XPSSFYJQS3BTGSYAX32198/comments/06F1Y0SNSD585D942015XEG0Y8.md marks prior PO-critic `critic-item-1` through `critic-item-4` as `answered` and restates the seed rule as catalog every diagnostic currently emitted by `DataVaultModelArtifactParser` and `DataVaultModelImportResult`, and no others.
- `rg -o "DMV[0-9]{4}" -N src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs src/DCoding.Data.DVault/DataVaultModelImportResult.cs | sort -u | wc -l` returned `18`; the parser file contains DMV1001-DMV1701 and `src/DCoding.Data.DVault/DataVaultModelImportResult.cs` contains DMV1801.
- `src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:8` and `:1273` show parser diagnostics are emitted through `SeverityError = "error"` and `AddIssue(...)`; `src/DCoding.Data.DVault/DataVaultModelImportResult.cs:98-100` emits DMV1801 with severity `error` and category `projection`, giving a direct source baseline for the AC drift checks.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs:41-55` asserts DMV1002 keeps logical source path `models/invalid.json` and JSON pointer `/schemaVersion`, and `:130-138` asserts DMV1801 keeps category `projection`, logical source path `models/customer.json`, and JSON pointer `/pits/0`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactParserTests.cs:437-441` verifies parser diagnostics by `Category`, `Code`, and `Path`; `git log --oneline 135c9e808..HEAD -- .gicket/tickets/06F1XPSSFYJQS3BTGSYAX32198` shows the substantive refinement handoff commit `a56a66269` followed only by workflow residual/claim commits, with no later ticket evidence reopening the prior blockers.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The approved v1 slice assumes the catalog remains limited to the 18 currently emitted importer/projection error diagnostics in `DataVaultModelArtifactParser.cs` and `DataVaultModelImportResult.cs`; if those files gain additional importer/projection diagnostics before development starts, the ticket contract will need a PO refresh.
- The ticket intentionally keeps any consumer-facing published diagnostic catalog artifact out of scope; implementation should not expand this work into a public documentation or distribution surface without a follow-up ticket.

AC / test suggestions
- Keep one deterministic test that discovers the catalog in ascending code order and compares it to the exact approved 18-code list.
- Keep explicit regression tests that catalog-backed emission preserves `Code`, `Category`, `LogicalSourcePath`, and `JsonPointer` for representative parse and projection cases such as DMV1002 and DMV1801.
- Keep explicit required-field tests for non-blank `summary/title`, `explanation`, and `remediation` on every catalog definition.

Implementation watchouts
- `DMV1001`-`DMV1701` originate in `DataVaultModelArtifactParser.cs`, while `DMV1801` is synthesized in `DataVaultModelImportResult.ApplyToCore(...)`; centralization must preserve both emission paths without changing externally observed diagnostics.
- `DataVaultModelArtifactParser.ResolveDeclarationPath(...)` currently drives projection-path JSON-pointer selection for DMV1801; moving metadata into a catalog must not regress that path resolution.

Non-blocking notes
- `.gicket/relations/2G/98/06F1XPS7KGKBP5SVMQPJC49J2G--06F1XPSSFYJQS3BTGSYAX32198--parentOf.json` shows the ticket remains attached to story `06F1XPS7KGKBP5SVMQPJC49J2G`, and no additional child-ticket structure was added in this refinement pass.
- `git log --oneline 135c9e808..HEAD -- .gicket/tickets/06F1XPSSFYJQS3BTGSYAX32198` shows only PO/PO-critic workflow commits on this branch so far; no developer implementation commits are present yet.

Split recommendations
- No split recommended; the 18-code importer/projection catalog slice remains the smallest coherent developer handoff.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment