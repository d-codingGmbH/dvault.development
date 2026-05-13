[gicket-bot] PO-critic review contract

Summary
- The ticket is directionally well-bounded and grounded in the existing importer/projection diagnostics path, but it still leaves the required diagnostic-documentation contract and the exact v1 seed set too implicit for unattended developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F1XPSSFYJQS3BTGSYAX32198/description.md` names the first integration path as the existing model-artifact importer/projection diagnostics path and its `## Open Questions` section is `- none`.
- `src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs` currently emits shipped model-artifact diagnostic codes `DMV1001` through `DMV1701`, and `src/DCoding.Data.DVault/DataVaultModelImportResult.cs` emits `DMV1801` for projection failures.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs` asserts that `DMV1002` preserves logical source path `models/invalid.json` and JSON pointer `/schemaVersion`, and that `DMV1801` preserves category `projection`, logical source path `models/customer.json`, and JSON pointer `/pits/0`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactParserTests.cs:424-441` verifies parser diagnostics by `Category`, `Code`, and `Path`, with observed categories including `schema-version`, `shape`, and `provider-choice` elsewhere in that file.
- A repository search for diagnostic-documentation guidance (`documentation coverage`, `diagnostic.*documentation`, `remediation`) found no local diagnostic-definition contract; the only non-ticket hit was generic XML-doc guidance in `docs/plans/shared-implementation-standards.md:87`.

Blocking findings
- The acceptance criteria require documentation coverage on each catalog entry, but neither the ticket contract nor repository sources define what fields or quality bar satisfy that coverage. With only generic XML-doc guidance in `docs/plans/shared-implementation-standards.md:87`, developers would have to invent the catalog documentation shape.
- The contract cites `DMV1002` and `DMV1801` as examples, but direct source inspection shows 18 currently emitted importer/projection-path codes across `DataVaultModelArtifactParser.cs` and `DataVaultModelImportResult.cs` (`DMV1001`-`DMV1801`). The ticket does not explicitly say whether v1 must catalog all of those emitted diagnostics or a smaller named subset.

Required PO actions
- Define the minimum documentation contract for one catalog entry in ticket language: required fields, where they live, and what the new tests must enforce.
- State the exact v1 seed rule for this ticket, for example 'catalog every diagnostic currently emitted by `DataVaultModelArtifactParser` and `DataVaultModelImportResult`' or provide an explicit smaller in-scope code list.

Open issues ledger
- critic-item-1 [required-po-action] Define the minimum documentation contract for one catalog entry in ticket language: required fields, where they live, and what the new tests must enforce.
- critic-item-2 [required-po-action] State the exact v1 seed rule for this ticket, for example 'catalog every diagnostic currently emitted by `DataVaultModelArtifactParser` and `DataVaultModelImportResult`' or provide an explicit smaller in-scope code list.
- critic-item-3 [blocking-finding] The acceptance criteria require documentation coverage on each catalog entry, but neither the ticket contract nor repository sources define what fields or quality bar satisfy that coverage. With only generic XML-doc guidance in `docs/plans/shared-implementation-standards.md:87`, developers would have to invent the catalog documentation shape.
- critic-item-4 [blocking-finding] The contract cites `DMV1002` and `DMV1801` as examples, but direct source inspection shows 18 currently emitted importer/projection-path codes across `DataVaultModelArtifactParser.cs` and `DataVaultModelImportResult.cs` (`DMV1001`-`DMV1801`). The ticket does not explicitly say whether v1 must catalog all of those emitted diagnostics or a smaller named subset.

Missing examples / edge cases
- Add one representative catalog-entry example for a parser diagnostic such as `DMV1002`, including the required stable fields and documentation fields.
- Add one explicit projection-path example showing how `DMV1801` remains tied to declaration path `/pits/0` while diagnostic metadata moves into the catalog.

Risky assumptions
- Assumes the first-slice catalog only needs current error diagnostics; the reviewed model-artifact path evidence did not show warning/info severities, so a broader catalog shape would otherwise be guessed.

AC / test suggestions
- Add a deterministic test that the catalog covers exactly the agreed importer/projection diagnostic code set and fails on missing or extra entries.
- Keep regression tests that prove catalog-backed emission preserves `Code`, `Category`, `LogicalSourcePath`, and `JsonPointer` for at least `DMV1002` and `DMV1801`.
- If documentation becomes structured, add a test that every catalog entry carries non-empty human-facing fields for the agreed documentation contract.

Implementation watchouts
- Projection failures are currently synthesized in `DataVaultModelImportResult.ApplyToCore(...)`, while parse diagnostics originate in `DataVaultModelArtifactParser`; centralization must preserve the current `ResolveDeclarationPath(...)` behavior.
- Current tests pin the public importer-facing surface in `DataVaultModelImportDiagnostic`, so the refactor cannot regress externally observed message/category/path behavior while moving metadata into a catalog.

Non-blocking notes
- The current ticket branch appears to contain review/workflow commits only; I did not find developer implementation commits on this branch yet.
- The child relation to story `06F1XPS7KGKBP5SVMQPJC49J2G` is present in `.gicket/relations/2G/98/06F1XPS7KGKBP5SVMQPJC49J2G--06F1XPSSFYJQS3BTGSYAX32198--parentOf.json`.

Split recommendations
- No split needed after the two contract gaps above are resolved; the implementation slice itself remains appropriately small.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment