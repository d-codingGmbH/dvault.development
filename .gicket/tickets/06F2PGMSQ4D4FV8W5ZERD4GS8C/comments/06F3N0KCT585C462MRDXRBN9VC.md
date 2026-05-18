[gicket-bot] PO-critic review contract

Summary
- The ticket contract is internally consistent, backed by current source/docs/snapshot evidence, and has no unresolved open questions; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract is current and unblocked: .gicket/tickets/06F2PGMSQ4D4FV8W5ZERD4GS8C/description.md:7-9 records PO handoff `ready_for_po_critic`, and :55-56 records `## Open Questions` = `none`.
- The explicit bulk SPI already exists in source: src/DCoding.Data.DVault/DataVaultSaveService.cs:12-35 defines `IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest)`, :93-110 resolves `DataVaultRegistryBulkSaveRequest` into the same bulk pipeline, :230-244 defines `DataVaultRegistryBulkSaveRequest`, and :482-496 defines `DataVaultBulkSaveRequest`.
- The provider-native SPI and ordered-batch context already exist in source: src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:10-33 defines `IDataVaultProviderSaveStrategy` with `Priority`, `CanSave`, and `SaveAsync`, and :68-99 exposes `DataVaultProviderSaveStrategyContext.ResolvedRequests`.
- Current implementation matches the ratified semantics: src/DCoding.Data.DVault/DataVaultSaveService.cs:859-875 dispatches whole request batches through provider strategies, and :<redacted> plus :<redacted> keep latest `HashDiff` state across ordered satellite batches with chronological advancement.
- Diagnostics/docs/API snapshot are aligned: src/DCoding.Data.DVault/DataVaultDiagnostics.cs:433-451 has bulk Analyze overloads; README.md:204 and :390 document ordered bulk saves and request-bound diagnostics; docs/releases/v0.9.0.md:54-58 states no second bulk-insert SPI was added; tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:58-60, :607, :933-934, :967-970, and :987-988 include the public bulk save and provider strategy surfaces.
- Typed bulk helpers feed the same registry-backed batch contract: src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs:107-207 public bulk helpers call `Create*RegistryBulkSaveRequest`, and :282-336 assemble `DataVaultRegistryBulkSaveRequest`; tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs:255-282 verifies caller order is preserved.
- Split/ownership evidence is present in ticket data: .gicket/relations/8C/D0/06F2PGMSQ4D4FV8W5ZERD4GS8C--06F2PGN4GPQCGC5WHZQBGP4SD0--parentOf.json makes 06F2PGN4GPQCGC5WHZQBGP4SD0 the child fallback task, .gicket/relations/8C/K4, 8C/EM, and 8C/P8 show blocks to 06F2PGNGVQ3TZZWSABAK5SNFK4, 06F2PGNT7DF4DVNKYWDFZC8DEM, and 06F2PGP2B2RZGGK3CVKK5WRRP8, and .gicket/tickets/06F2PGN4GPQCGC5WHZQBGP4SD0/ticket.json:5-19 shows the fallback child is already `done`.
- Branch-history evidence shows this is still a pre-dev contract ticket: `git diff --name-only develop..HEAD` lists only .gicket/tickets/06F2PGMSQ4D4FV8W5ZERD4GS8C/* files, and `git show --stat d6a6db8f5` shows the HEAD commit is ticket metadata/comment activity rather than repo code changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No consumer-facing example for `DataVaultRegistryBulkSaveRequest` or typed bulk helper usage is carried by this ticket; the contract explicitly leaves broader consumer docs to 06F2PGP2B2RZGGK3CVKK5WRRP8.
- Benchmark crossover guidance between provider-neutral fallback and provider-native strategies is intentionally not defined here and remains a follow-up for 06F2PGNZBRNCQ1SV2KKP6F3BA8.
- Streaming or non-materialized ingestion is explicitly out of scope here, so any caller that needs it still needs a separate follow-on story rather than an in-ticket expansion.

Risky assumptions
- Sibling implementation tickets will preserve the ordered-batch and `ResolvedRequests` semantics ratified here; the current contract already notes drift risk if provider-native work diverges.
- Broader v0.14.0 consumer docs and release-note packaging will be supplied by 06F2PGP2B2RZGGK3CVKK5WRRP8; the repository currently has release notes only through docs/releases/v0.13.0.md.
- Performance guidance will wait for 06F2PGNZBRNCQ1SV2KKP6F3BA8 instead of treating this contract story as proof of faster provider-native behavior.

AC / test suggestions
- Keep the strategy-dispatch regression checks that prove descending `Priority` evaluation and equal-priority registration-order tie-breaks (tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:286-366).
- Keep request-bound diagnostics coverage that proves ordered bulk requests are passed to strategy evaluation (tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:153-178).
- Keep bulk satellite regression coverage for in-batch latest-state suppression and chronological replay (tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:601-798).
- Keep typed bulk helper order-preservation coverage (tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs:255-282).

Implementation watchouts
- Do not add a second public bulk-insert API or widen this story into implicit `SaveChanges` ingestion; the persisted contract, README, and docs/releases/v0.9.0.md:54-58 all ratify one explicit bulk boundary.
- Treat provider-name-specific SQL or acceptance logic as sibling-ticket work under 06F2PGNGVQ3TZZWSABAK5SNFK4, not as scope to reopen here.
- Keep v0.14.0 release-note packaging and broader docs in 06F2PGP2B2RZGGK3CVKK5WRRP8; this ticket should only guard the already-visible contract wording.
- Because the branch currently differs from `develop` only in .gicket ticket files, any developer action should stay narrowly scoped and should not invent unrelated repo changes just to create implementation evidence.

Non-blocking notes
- PO comment .gicket/tickets/06F2PGMSQ4D4FV8W5ZERD4GS8C/comments/06F3MY88RVYMCSBN89F868PHYW.md already records that no persistent planning artifact, relation edit, or new child ticket was required in refinement.
- Sibling tickets 06F2PGNGVQ3TZZWSABAK5SNFK4, 06F2PGNT7DF4DVNKYWDFZC8DEM, 06F2PGNZBRNCQ1SV2KKP6F3BA8, and 06F2PGP2B2RZGGK3CVKK5WRRP8 are still `todo`; that does not make this ticket unclear, but those downstream stories/tasks are not independently PO-cleared yet.

Split recommendations
- No additional split is recommended; the current graph already separates fallback implementation (child 06F2PGN4GPQCGC5WHZQBGP4SD0), provider-native strategies (06F2PGNGVQ3TZZWSABAK5SNFK4), provider integration coverage (06F2PGNT7DF4DVNKYWDFZC8DEM), benchmarks (06F2PGNZBRNCQ1SV2KKP6F3BA8), and docs (06F2PGP2B2RZGGK3CVKK5WRRP8).
- If future work needs streaming/non-materialized ingestion or transport-specific batching, create a separate follow-on story instead of widening 06F2PGMSQ4D4FV8W5ZERD4GS8C.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment