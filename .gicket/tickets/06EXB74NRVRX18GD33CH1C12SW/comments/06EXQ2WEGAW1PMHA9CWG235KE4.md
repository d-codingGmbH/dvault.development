[gicket-bot] PO-critic review contract

Summary
- PO-critic review finds the delivery contract ready for developer handoff: scope is bounded, Open Questions are explicitly closed, and repository evidence supports the stated implementation home, standards, and existing technical metadata baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- repository-list-directory on `src/DCoding.Data.DVault/Modeling` found existing modeling source files including `DefaultNamingPolicy.cs`, `IDataVaultNamingPolicy.cs`, `DataVaultConventions.cs`, `DataVaultMetadata.cs`, `DataVaultModel.cs`, `DataVaultModelBuilder.cs`, and related builder/concept/options files.
- repository-list-directory on `tests/DCoding.Data.DVault.Tests/Modeling` found modeling test files `DefaultNamingPolicyTests.cs` and `NamingPolicyTests.cs`, matching the contract's preferred test area.
- Seeded repository source shows `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` targets `net10.0`, enables `GenerateDocumentationFile`, and treats `CS1591` as an error, supporting the XML documentation DoD.
- Seeded source evidence shows `TechnicalMetadataColumnRole` exists with the closed v1 values `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`, and `TechnicalMetadataColumnContract` exposes defaults for those roles.
- Repository documents cited by the contract exist in seed/read evidence: `docs/architecture/mvp-data-vault-concepts.md`, `docs/naming/default-naming-policy.md`, `docs/plans/stable-hashing-contract.md`, `docs/plans/dvault-v1-default-persistence-convention-policy.md`, and `docs/plans/shared-implementation-standards.md`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The delivery contract intentionally leaves child-ticket closure sequencing as follow-up governance rather than a prerequisite; developers should verify whether the two child slices are dependencies before closing the parent story.

AC / test suggestions
- Keep the listed acceptance criteria as the developer handoff target; they already require hub, link, satellite, business-key, participant, payload, technical metadata, naming/default, and provider-neutral tests without a database provider.

Implementation watchouts
- Reuse the existing `TechnicalMetadataColumnRole` baseline instead of creating a parallel role model.
- Keep hash key/hash diff as metadata only; do not add hash computation or normalization behavior under this story.
- Keep implementation in `src/DCoding.Data.DVault/Modeling` and tests under `tests/DCoding.Data.DVault.Tests/Modeling` unless nearby source establishes a narrower placement.
- Avoid provider-specific Sqlite/Postgres APIs, schema generation, migrations, generated columns, sequences, triggers, PIT tables, bridges, and multi-active satellite behavior.

Non-blocking notes
- Two shell-command attempts in the tool loop were blocked because the runtime rejects chained or redirected shell commands; the review still has sufficient ticket, comment, directory, and seeded source evidence for ticket-level PO-critic approval.

Split recommendations
- No additional split is required for PO handoff. If implementation grows too large, split by concept family: hub/business-key metadata, link/participant metadata, and satellite/payload metadata, sharing the same technical metadata role set.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment