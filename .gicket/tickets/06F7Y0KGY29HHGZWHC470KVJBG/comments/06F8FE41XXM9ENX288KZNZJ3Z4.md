[gicket-bot] PO-critic review contract

Summary
- Evidence-backed contract is ready for developer handoff; repository, ticket, and branch-history checks align with the stated scope, with only non-blocking ambiguity around constraint scope and multi-operation rename heuristics.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 10 comments for 06F7Y0KGY29HHGZWHC470KVJBG; the visible comments are bot claim, handoff, runtime, and PO-refinement messages, with no human scope-change discussion.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md documents the existing consumer-owned preflight command 'dotnet run --project ... -- guardrail --migration <name>' and says it passes migration UpOperations into DataVaultMigrationOperationDiagnostics.AnalyzeReport(...) before apply.
- src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs RunGuardrail(...) resolves migration operations and calls DataVaultMigrationOperationDiagnostics.AnalyzeReport(host.Diagnostics, dbContext, operations), confirming the ticket is strengthening an existing entrypoint rather than adding a new CLI surface.
- src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs currently analyzes AddColumn, DropColumn, AlterColumn, RenameColumn, CreateIndex, DropIndex, RenameIndex, AddPrimaryKey, DropPrimaryKey, and DropTable against hub/link/satellite/pit/bridge baselines.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs already covers PIT and bridge findings such as migration/DropColumn/PitCustomerContact/ContactLoadTimestamp, migration/DropColumn/BridgeSalesRegionHierarchy/TraversalDepth, and migration/DropTable/BridgeCustomerOrder, matching the ticket's stated structure scope.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs defines provider-neutral ProducedName, EntityKind, MetadataName, and PropertyRole annotations, and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs applies ProducedName, EntityKind, and MetadataName to entities, ProducedName to keys and indexes, and ProducedName, PropertyRole, and MetadataName to properties.
- git log --oneline on ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru shows only ticket orchestration commits above develop, and git show --stat 6b9cee0c4 plus git diff --stat develop...ticket/06F7Y0KGY29HHGZWHC470KVJBG-story-strengthen-migration-guardrails-for-destru show .gicket comment, description, event, and ticket metadata changes only, which is consistent with a pre-development handoff branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example for intentional RenameTable-style evolution of a generated hub, link, satellite, PIT, or bridge table is not spelled out in the contract, and current repository source does not show RenameTable handling in DataVaultMigrationOperationDiagnostics.
- Primary-key or other named-constraint replacement cases where EF emits drop-plus-add rather than a dedicated rename operation are not illustrated with a concrete expected outcome.
- A mixed migration example that adds safe generated structure changes and also performs one suspicious replacement on the same logical shape would help pin the report behavior and blocking threshold.

Risky assumptions
- This review assumes 'named generated constraints' means the currently evidenced generated primary-key family unless PO intends broader constraint kinds; DataVaultDiagnostics.CreateEntityExplain currently materializes only primary-key constraints in DataVaultEntityExplain.Constraints.
- This review assumes 'explicit intentional evolution' is primarily evidenced by clear EF rename or evolution operations and preserved logical continuity, not by arbitrary drop-and-add sequences with manually explained intent.
- This review assumes provider-specific scaffolding differences may still surface as 'suspicious' rather than 'safe' when continuity evidence is weak, which is consistent with the ticket's stated risk section.

AC / test suggestions
- Add explicit acceptance examples for RenameColumn, RenameIndex, and any intended table-rename or evolution flow so 'intentional' vs 'suspicious' is testable per object type.
- Cover named-constraint cases explicitly, at least for generated primary keys, because current guardrail tests already cover AddPrimaryKey and DropPrimaryKey while the new story raises the bar to destructive and suspicious classification.
- Include one provider-decomposed rename scenario where EF emits multiple operations so the guardrail's suspicious-drift behavior is proven rather than inferred.

Implementation watchouts
- Current guardrail code ignores DataVaultEntityExplain.Constraints and builds its baseline from entity columns, the primary key, and indexes, so constraint-scope expectations need to stay aligned with the actually available explain model.
- The current report contract exposes deterministic operation paths and Safe, Risky, and Incompatible summaries; stronger destructive-drift behavior should preserve that automation-friendly shape because docs and unit tests already depend on it.
- The ticket requires provider-neutral heuristics based on metadata and produced-name evidence; relying on raw SQL text or provider-specific migration patterns would contradict the documented guardrail boundary.

Non-blocking notes
- The current owner branch is still pre-development: compared with develop it contains ticket and comment metadata only, and the prompt explicitly allows that at this gate.
- No human comment thread or attachment evidence changed scope after PO refinement, so the durable contract remains the authoritative handoff artifact.

Split recommendations
- No split recommended; the contract already keeps this as one cohesive guardrail-strengthening story on the existing guardrail --migration surface.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment