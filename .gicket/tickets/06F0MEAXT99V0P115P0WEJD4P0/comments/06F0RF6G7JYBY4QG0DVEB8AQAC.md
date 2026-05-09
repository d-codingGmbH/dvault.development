[gicket-bot] PO-critic review contract

Summary
- Approved: the persisted immutable registry contract is source-backed, has no unresolved Open Questions, and is sufficiently bounded for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEAXT99V0P115P0WEJD4P0/description.md:7-16 records PO handoff decision `ready_for_po_critic`, states the registry is additive over existing metadata, and lists `## Open Questions` as `- none`.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:113-151 already exposes `Hubs`, `Links`, `Satellites`, `PointInTimeTables`, `Bridges`, and `Pits`, matching the contract's in-scope metadata families.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:852-944 defines both `DataVaultPointInTimeMetadata` and `DataVaultPitMetadata`, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:268-289 proves PIT declaration order and multi-active satellite flags are part of the current source-backed baseline.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:233-280 validates point-in-time references with `StringComparer.Ordinal` and parent-aware satellite checks, which directly supports the contract's exact-name and parent-scoped lookup requirements.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:722-840 shows satellite metadata carries a parent reference plus ordered `DrivingKeyNames`, backing the ticket's no-loss adaptation requirement for parent-scoped and multi-active satellite metadata.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:192-247 and :414-558 define public provider capability profile metadata and built-in exact profile names (`sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `mysql-pomelo-v1`).
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:16-45 iterates the metadata model collections in declaration order, and `git show --stat 626e74e5` plus `git log --oneline` on `ticket/06F0MEAXT99V0P115P0WEJD4P0-task-define-immutable-model-registry-and-lookup` show this branch only contains ticket-workflow commits after `develop`, so the review is against the persisted contract rather than hidden code changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A worked example with two satellites sharing the same logical name under different parents is not written out, even though parent-scoped lookup is a key requirement.
- The contract names ambiguous CLR lookup as a failure mode but does not include a concrete collision example showing the expected diagnostic shape.
- Provider capability profiles are in scope, but there is no concrete example of registry lookup and deterministic iteration over multiple profile entries.

Risky assumptions
- No-loss adaptation should be read as adapting representative existing `DataVaultMetadataModel` instances, not as requiring one current public constructor to combine PointInTimeTables, Bridges, and Pits in a single model instance.
- Future code-first work must populate CLR mappings explicitly; current modeling metadata types do not expose CLR members, so metadata-first adaptation should default to no match rather than inferred associations.
- PointInTimeTables and Pits need to remain separate lookup domains because the current public source exposes them as distinct types and collections.

AC / test suggestions
- Add a no-loss adaptation test with duplicate satellite names under different parents to prove parent-scoped satellite lookup works without global-name rejection.
- Add explicit CLR ambiguity tests that assert diagnostics include metadata kind, logical name, and CLR type.
- Add provider-profile registry tests that preserve declaration or registration order and exact `ProfileName` lookup for the built-in profiles.
- Add adaptation tests that cover both `DataVaultPointInTimeMetadata` and `DataVaultPitMetadata` baselines so both public surfaces remain first-class.

Implementation watchouts
- Do not collapse `PointInTimeTables` and `Pits` into one collection or silently rename one behind the adapter; the source currently exposes both independently.
- Do not impose global satellite-name uniqueness; current source and validation logic rely on parent context.
- Keep lookup comparison ordinal and case-sensitive to match the existing `StringComparer.Ordinal` and `StringComparison.Ordinal` usage in source.
- Treat CLR lookup as optional metadata only; the current metadata-first source does not provide enough information to infer CLR associations.
- Keep provider capability profiles as named immutable registry entries rather than special-casing only one active profile.

Non-blocking notes
- The persisted contract stays within modeling-layer contract work and explicitly excludes DI wiring and save/read service refactors, which aligns with the four downstream blocked tickets already related to this gate.

Split recommendations
- No split recommended; `.gicket/relations/.../06F0MEAXT99V0P115P0WEJD4P0--*--blocks.json` shows four downstream tickets already block on this shared registry contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment