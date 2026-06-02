[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the persisted contract is specific, has no open questions, and the repository directly proves both the stale current docs and the implemented read-plan/PIT/bridge helper surface the docs must be updated to describe.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F7Y0HZKHBHMYX9EYDYFRYXZ0/description.md:7-9 marks PO handoff as `ready_for_po_critic`, and lines 57-58 record `## Open Questions` as `- none`.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:128-207 directly checks generated bridge helper/read-model output, including `ReadBridgeCustomerOrderFromAsync`, `ReadBridgeCustomerOrderToAsync`, `ReadBridgeSalesRegionHierarchyAncestorAsync`, `ReadBridgeSalesRegionHierarchyDescendantAsync`, and required `int maximumDepth` for hierarchy helpers.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:210-298 verifies generated bridge helpers delegate through the runtime read boundary and pass `MaximumDepth` for hierarchy requests.
- src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:<redacted> and <redacted> directly generate bridge read-model code and `Read...FromAsync`/`Read...ToAsync`/`Read...AncestorAsync`/`Read...DescendantAsync` methods over `IDataVaultReadService`, with `maximumDepth` only for hierarchy shapes.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:459-473 defines `DataVaultReadShapeKind` as `LatestSatellite`, `PitAsOf`, and `Bridge`; lines 626-655 place request-bound `ReadShape` on `DataVaultDiagnosticsResult`; lines <redacted> populate `ReadShape` for satellite, PIT, and bridge requests.
- src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs:116-119 uses `DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics(dbContext)` when supplied, and src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs:20-41 exports deterministic redacted support-bundle JSON from `DataVaultDiagnosticsResult`.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:177-205 asserts bridge `ReadShape` diagnostics for many-to-many and hierarchy shapes, and lines 956-975 assert support-bundle JSON includes `readShape` while omitting forbidden request values.
- Current baseline docs are visibly stale: README.md:369-390 and 924 say typed helpers are satellite-only and PIT/bridge stay runtime-only; src/DCoding.Data.DVault.Analyzers/README.md:12, 58-66, 76 says the generator is satellite/PIT-only and that bridge helpers are unsupported; docs/production-adoption-checklist.md:9, 11, 55, 77, 127 still describe the baseline as v0.24/satellite-only.
- docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:3-10 still labels the PIT/bridge helper contract as additive/future even though the source generator and tests above now prove bridge helper generation exists.
- `test -f /mnt/c/Projects/DVault/docs/releases/v0.25.0.md; echo $?` returned `1`, confirming the v0.25.0 release note deliverable does not yet exist and is part of the ticket scope.
- `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F7Y0HZKHBHMYX9EYDYFRYXZ0/...` metadata files; no README/docs/source files are changed yet. For this pre-development doc ticket, that is a handoff watchout, not a PO blocker.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- None blocking at ticket level; the contract already requires at least one redacted `ReadShape`/support-bundle example plus PIT and bridge helper call examples.
- Historical satellite-only documents outside the current baseline may still need explicit historical framing later, but the ticket already records that as a follow-up question rather than an open question.

Risky assumptions
- Developers must treat current code and tests, not stale prose, as the authority when reconciling helper support; the repo currently contains contradictory documentation.
- Historical documents such as docs/releases/v0.22.0.md and docs/plans/typed-read-model-generator-contract.md can remain satellite-only only if the updated baseline clearly frames them as historical context.

AC / test suggestions
- When updating docs, cite the existing repository evidence already named in the contract: `DataVaultTypedReadModelSourceGeneratorTests.cs` for PIT/bridge helpers and `DataVaultDiagnosticsTests.cs` for `ReadShape` export/redaction.
- Include validation command references in the new v0.25.0 release note, since the contract explicitly requires validation evidence/commands and the repository already standardizes those commands in current docs.

Implementation watchouts
- Do not let updated docs imply unbounded traversal, automatic PIT/bridge maintenance, provider-specific SQL generation, or a new query API; the source still bounds bridge helpers to `From`/`To` and `Ancestor`/`Descendant` with explicit `maximumDepth` for hierarchy.
- Update current-baseline docs consistently; README, analyzer README, checklist, and the v1 typed PIT/bridge contract currently disagree with the generator/tests and will stay contradictory if only partially edited.

Non-blocking notes
- The branch is still at scratch ref `12a37764f1a764ffb6b157a45625897ae84e3b54`; no documentation implementation work has started yet.
- The ticket remains a clean documentation-only handoff because scope-out excludes runtime/analyzer/source-generator/test changes and the persisted contract already reflects that boundary.

Split recommendations
- Keep this ticket doc-only as written. If the team wants runnable consumer samples or additional historical-document cleanup beyond the active baseline, track those as separate follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment