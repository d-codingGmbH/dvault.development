[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the delivery contract has no open questions, the referenced preflight/live-schema/provider-capability surfaces exist in-repo, and the branch still contains only ticket metadata writes.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F7Y0KVHGTTVS216ERSG4XNMM/description.md` contains `## Open Questions` with `- none`, so the persisted delivery contract has no unresolved PO questions.
- `git -C /mnt/c/Projects/DVault log --oneline -n 5 ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in` shows only workflow commits after `develop` (`b12d69a68`, `d2cb6d93f`, `769fa4807`), and `git diff --name-only develop..ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in` lists only `.gicket/tickets/06F7Y0KVHGTTVS216ERSG4XNMM/**`, confirming this is still a pre-development ticket branch with no code or test implementation yet.
- On-disk relations confirm the scoped ticket graph: `.gicket/relations/TW/MM/06F7Y0J8PRFRSSWZ3GGT91S0TW--06F7Y0KVHGTTVS216ERSG4XNMM--parentOf.json` links the epic to this story, and `.gicket/relations/MM/V4/06F7Y0KVHGTTVS216ERSG4XNMM--06F7Y0NBHXQ6CK8R3AH4DEP9V4--blocks.json` keeps documentation on follow-up task `06F7Y0NBHXQ6CK8R3AH4DEP9V4`.
- `src/DCoding.Data.DVault/DataVaultPreflight.cs` already composes `validation-provider`, `artifact-drift`, `snapshot-drift`, `migration-guardrail`, and `request-diagnostics`, with omitted optional inputs returning deterministic skipped lanes via the `No ... was provided.` messages; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs` covers that current behavior.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` registers built-in readers for SQLite, PostgreSQL, SQL Server, Oracle, `MySql.EntityFrameworkCore`, and `Pomelo.EntityFrameworkCore.MySql`; `README.md` and `docs/releases/v0.11.0.md` both document live-schema checks as explicit opt-in rather than default behavior.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaDriftReporter.cs` classifies non-success live reads into stable blocking evidence codes `live-schema-provider-unsupported` and `live-schema-unavailable`, and `tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs` verifies both outcomes.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` exposes `DataVaultEntityExplain` with `PrimaryKey`, `Indexes`, and `Constraints`, and also exposes read-shape `ExpectedIndexBaseline` and `ExpectedTraversalIndexBaseline`, matching the ticket's proposed baseline sources.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` and `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` already encode the provider caveats this story depends on through `AllowsIndexesCoveredByPrimaryKey` and `UnsupportedIncludedIndexColumnMode`; `tests/DCoding.Data.DVault.Tests/Shared/LiveSchemaReaderContractFixture.cs` mirrors the included-index shape differences used by existing fixtures.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example where a provider legitimately suppresses a secondary index because it is fully covered by the primary key would make pass criteria for redundant-index providers even clearer.
- An explicit example distinguishing omitted live input (`Skipped`) from supplied-but-unavailable live input (`Unavailable`) would help keep lane semantics unambiguous.
- A concrete multi-active satellite example that exercises driving-key plus included-index fallback behavior would strengthen edge-case expectations across providers.
- A concrete example for metadata models that omit PIT or bridge tables would clarify that those operation families are skipped rather than treated as failures.

Risky assumptions
- The story assumes the new idempotency result surface can expose `UnsupportedProvider` and `Unavailable` cleanly even though the current `DataVaultPreflightSectionStatus` enum in `src/DCoding.Data.DVault/DataVaultPreflightSectionStatus.cs` only has `Passed`, `Blocked`, and `Skipped`.
- The story assumes operation-family mapping can be derived from existing diagnostics and translated-baseline surfaces without introducing a second, conflicting index vocabulary.

AC / test suggestions
- Add one default local SQLite path that covers pass, missing structure, and mismatched structure on the new idempotency surface, then layer provider-specific fixtures for include-column and redundant-index behavior.
- Mirror the current unavailable/unsupported live-schema coverage from `tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs` on the new idempotency-check surface.
- Add redaction assertions for unavailable live-schema findings similar to the support-bundle redaction checks in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs`.

Implementation watchouts
- Do not surface full `DataVaultLiveSchemaDriftReporter.Compare(...)` output directly if it would pull unrelated column or store-type drift into this idempotency-specific contract.
- Normalize expected index shape through provider capability rules before comparing, or providers with PK-covered-index suppression or non-native include handling will false-fail.
- Keep live-schema invocation explicit and consumer-owned; default preflight behavior must remain skipped when the caller does not supply live input or lane selection.
- If the new surface plugs into `DataVaultPreflight`, decide explicitly whether unsupported/unavailable is represented as lane-local status, report detail, or new enum values so aggregate behavior stays deterministic.

Non-blocking notes
- The existing branch content is ticket metadata only, which matches the contract note that refinement was repository-evidence-based rather than implementation-based.

Split recommendations
- No split is required for developer handoff; the repository already contains the preflight, live-schema, diagnostics, provider-capability, and test-fixture building blocks this story depends on.
- Keep documentation rollout and any future broader non-idempotency live-schema advisory work on separate tickets rather than widening this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment