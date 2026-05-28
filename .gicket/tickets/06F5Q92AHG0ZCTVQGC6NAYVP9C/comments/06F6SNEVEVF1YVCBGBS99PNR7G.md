[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C' for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Optimistic claim succeeded (`expectedRevision=06F6S88W0REX0NTRGEMS4J400C`, `currentRevision=06F6SKYVDBCAVGNFHG030P8988`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' and commit 'd85f41d6c614' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' from source 'd85f41d6c614'.
- Interactive tester tool loop completed review for branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Evidence: git -C /mnt/c/Projects/DVault diff --name-only develop...d85f41d6c614 shows the implementation changes are concentrated in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs, src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGe...
- Evidence: docs/plans/typed-read-model-generator-contract.md:22-38 requires one authoritative normalized metadata source and preservation of MetadataSourceKind, MetadataSourceFingerprint, ProducedName, MetadataName, PropertyRole, TechnicalColumnRole, ProviderLogicalPropertyKind...
- Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:43-66 builds declarations only from compilation syntax trees and additional texts; it does not resolve an authoritative EF/DVault annotated model or registry-backed descriptor.
- Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:549-624 computes fingerprints and produced names locally, and :677-703 emits those synthesized values into generated constants and DataVaultSatelliteMetadata construction.
- Evidence: src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:13-80 and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:693-699 plus :852-859 show the repository already exposes authoritative ProducedName, MetadataName, ParentReference, PropertyRole, ProviderLogicalPr...
- Evidence: src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-75 plus tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs:17-25 and tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs:33-6...
- 34 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: For each supported satellite metadata declaration, the consuming compilation receives generated `ReadModel` and `ReadExtensions` source under the documented namespace and naming rules, with `Current`, `Latest`, and `AsOf` methods bound to that satellite. (Not ...
- AC check failed: Generated satellite row types preserve exact produced table/column bindings and expose the parent hash key, driving keys in metadata order, `HashDiff`, `LoadTimestamp`, `RecordSource`, and payload properties with nullability derived from authoritative CLR/EF m...
- AC check failed: When authoritative metadata cannot be resolved deterministically, fingerprints drift, bindings or normalized public names collide, or the requested shape falls outside the bounded satellite contract, generation stops or skips with the documented `DMV196x` diag...
- AC check failed: Repository tests cover positive generation for representative hub-parent, link-parent, and multi-active satellite shapes plus negative diagnostics for stale fingerprints, unsupported bindings, nullability fallback, and naming-collision edge cases. (Repository ...
- DoD check failed: Developer-facing analyzer/generator documentation is updated enough to explain the typed satellite read-model generator boundary, supported inputs, and `DMV196x` failure cases. (Documentation was updated, but src/DCoding.Data.DVault.Analyzers/README.md:54-58 ...
- DoD check failed: Regression coverage protects deterministic naming, metadata-source and fingerprint handling, payload nullability, multi-active driving-key ordering, and unsupported-shape diagnostics. (Regression coverage does not protect authoritative metadata-source resolut...
- The generator does not resolve the authoritative translated EF/DVault metadata source required by the contract, so supported UseDataVaultMetadata(...) and compiled-model inputs are not implemented.
- Produced table names, produced column names, and metadata source fingerprints are recomputed inside the generator instead of being preserved from authoritative annotations, which can drift from the real runtime metadata contract.
- The regression suite does not cover authoritative-source ambiguity or annotated-model binding preservation, so the contract miss is currently unprotected.

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Rework the generator to normalize one authoritative translated EF/DVault descriptor for code-first, metadata-first, and model-first inputs, including UseDataVaultMetadata(...) and compiled-model annotated paths.
- Drive generated names, bindings, source kind/fingerprint, property roles, ordinals, provider logical metadata, and payload nullability from authoritative DVault annotations instead of ComputeFingerprint/GetSatelliteTableName/GetColumnNames heuristics.
- Add regression tests for authoritative-source ambiguity, UseDataVaultMetadata/UseModel inputs, and exact produced-name/fingerprint preservation, then rerun deterministic verification with dotnet test DVault.slnx --nologo and bash tools/check-format.sh.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9177`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `1df868072e8e4e139be7d47f9b902578`
- completed-at-utc: `<redacted>-28T04:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/runs/20260528T042655215Z-1df868072e8e4e139be7d47f9b902578.json`