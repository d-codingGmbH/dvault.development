[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F1XPS7KGKBP5SVMQPJC49J2G/description.md` persists `PO Handoff` = `ready_for_po_critic` and `## Open Questions` = `- none`.
- `git log --oneline --all --grep='06F1XPS7KGKBP5SVMQPJC49J2G|06F1XPSSFYJQS3BTGSYAX32198'` shows the child auto-integrated into `develop` at `0128c66c7` and the current story branch at `e18dd4bb2`.
- `src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs` contains 18 `new DataVaultDiagnosticDefinition(...)` entries spanning `DMV1001` through `DMV1801`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs` asserts the exact ordered code list plus the severity/category baseline.
- `src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs` resolves diagnostics via `DataVaultDiagnosticCatalog.GetModelArtifactDefinition(code)`, and `src/DCoding.Data.DVault/DataVaultModelImportResult.cs` resolves `DMV1801` through the same catalog, so the prerequisite catalog-backed validation path already exists in source.
- `src/DCoding.Data.DVault/DataVaultModelImportDiagnostic.cs` exposes public `Code`, `Category`, `JsonPointer`, and `LogicalSourcePath` accessors, matching the story's preserved affected-location behavior.
- `git diff --name-only develop..ticket/06F1XPS7KGKBP5SVMQPJC49J2G-story-establish-stable-dvault-diagnostic-codes` returns only `.gicket/tickets/06F1XPS7KGKBP5SVMQPJC49J2G/*`, so the story branch has not started unrelated implementation.
- `rg -n` across `README.md` and `docs/` found DMV diagnostic-contract references in `docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md`, but not in maintained docs such as `README.md` or `docs/model-first-governance.md`, which matches the story's remaining documentation scope.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Include one documentation example for a parse diagnostic such as `DMV1002` at `/schemaVersion` with its remediation text.
- Include one documentation example for a projection diagnostic such as `DMV1801` that shows both `LogicalSourcePath` and `JsonPointer` to make the affected-location contract explicit.

Risky assumptions
- The contract assumes 'repository documentation' means maintained docs beyond `docs/plans/`; current repo search shows the detailed DMV contract only in the planning document, so implementation should not satisfy the story by editing plan-only content.
- The story intentionally defers cross-family code-band allocation; downstream tickets can still drift if they start minting new diagnostics before that follow-up policy is written.

AC / test suggestions
- Prefer documentation evidence that mirrors the approved 18-code seed list already locked by `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs`.
- If the repo has doc-validation coverage later, add a lightweight check that published documentation still names the DMV1001-DMV1801 baseline and the required per-entry fields.

Implementation watchouts
- Do not rename the baseline to `DVLT####`; the persisted contract explicitly ratifies the existing `DMV####` family.
- Keep location details instance-bound: `Code`/`Category` stay on catalog definitions, while `JsonPointer` and `LogicalSourcePath` remain emitted-diagnostic context.
- `src/DCoding.Data.DVault/DataVaultDiagnosticDefinition.cs` is `internal`, so documentation should describe the stable diagnostic contract unless a separate ticket intentionally introduces a public catalog API.

Non-blocking notes
- The prerequisite catalog slice is already integrated into `develop`, so remaining story work is primarily documentation and story-level ratification rather than fresh catalog design.
- No additional child tickets or relation changes were introduced in the current refinement pass; the existing child relation file `.gicket/relations/2G/98/06F1XPS7KGKBP5SVMQPJC49J2G--06F1XPSSFYJQS3BTGSYAX32198--parentOf.json` remains the only implementation split referenced in the contract.

Split recommendations
- No additional split is needed for developer handoff; the remaining scope is bounded. If later publication work grows beyond repository-internal docs, make that a separate follow-up documentation ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment