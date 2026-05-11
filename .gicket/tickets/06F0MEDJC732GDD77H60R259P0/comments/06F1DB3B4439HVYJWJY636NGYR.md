[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md records PO Handoff decision ready_for_po_critic, Scope In accepting capable-runner validation at 3967d99c57977b65770dff03c79b0f938ade059d, and ## Open Questions -> none.
- Comment .gicket/tickets/06F0MEDJC732GDD77H60R259P0/comments/06F1A604MFZMN20088VWHKS20R.md records PackageVerifierTests passing 226 tests, dotnet pack succeeding, bash tools/verify-packages.sh succeeding, exactly six .nupkg and six .snupkg files at 0.5.1-alpha.0.69, and a successful verifier summary.
- git merge-base --is-ancestor 3967d99c57977b65770dff03c79b0f938ade059d HEAD returned exit 0; git diff --stat from that commit to HEAD over README.md, docs/releases/v0.6.0.md, docs/manual-nuget-publication.md, PackageVerifier.cs, PackageVerifierTests.cs, tools/verify-packages.sh, and DVault.slnx produced no output.
- README.md:10-15 lists all six v0.6.0 dotnet add package commands; README.md:24 and 50-77 present the Code-First happy path; README.md:301-303 preserves metadata-first migration guidance; README.md:352-364 lists build/test/pack/verify/check-format validation and verifier behavior.
- docs/releases/v0.6.0.md:8-17 documents the six-package v0.6.0 scope; lines 21-28 cover Code-First, registry, typed reads, diagnostics, and quickstarts; lines 32-39 record compatibility boundaries; lines 43-49 list deferred PIT/bridge/model-first limitations; lines 53-61 preserve final validation as release-operator work.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:14 expects README install version 0.6.0 and lines 362-365 verify dotnet add package commands for every expected package; tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:12 and 268-283 build the same 0.6.0 README guidance into tests.
- Direct source evidence exists for the documented APIs: DataVaultModelBuilderExtensions.cs:95-104 exposes ApplyDataVaultMetadata(vault => ...); DataVaultCodeFirstModelBuilder.cs:23-55 exposes Hub<TEntity>() and Link(...); DataVaultCodeFirstHubBuilder.cs:25-55 exposes BusinessKey(...) and Satellite(...); DataVaultCodeFirstSatelliteBuilder.cs:22-39 exposes DrivingKey(...) and Payload(...); DataVaultCodeFirstLinkBuilder.cs:18 exposes Participant<TEntity>().
- Direct typed-read evidence exists: DataVaultReadServiceTypedProjectionExtensions.cs:48-53 exposes ReadLatestSatelliteAsync<TProjection> over DataVaultLatestSatelliteReadRequest; DataVaultReadServiceRegistryExtensions.cs:85-106 exposes the registry-backed overload; DataVaultSatelliteProjectionRow.cs:36,65,90 exposes RequiredString, NullableString, and RequiredDateTimeOffset.
- examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs:12-17 and examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs:17-22 use AddDVault(...UseMetadataModel(...)) plus UseDataVaultMetadata(); QuickstartHistoryFlow.cs:101-117 uses registry-backed typed latest/as-of reads.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Reviewers must keep pre-tag MinVer artifact version 0.5.1-alpha.0.69 separate from final tagged v0.6.0 publication artifacts.

AC / test suggestions
- For any future rerun, use a capable mutable dev or release-validation runner, clear artifacts/packages first, run dotnet pack DVault.slnx --configuration Release --nologo, then bash tools/verify-packages.sh, and record exact output.
- Final publication should still follow docs/manual-nuget-publication.md:57-77 from the tagged checkout before package push approval.

Implementation watchouts
- Do not change README.md, docs/releases/v0.6.0.md, package metadata, verifier rules, or release automation merely to hide runner capability failures.
- Keep provider dependency verification tied to the packed DCoding.Data.DVault package version, not a hard-coded final release filename.

Non-blocking notes
- Read-only review did not rerun build/test/pack; approval relies on persisted capable-runner validation evidence and unchanged validated surfaces.

Split recommendations
- No split recommended now; create a follow-up only for a future concrete non-MinVer packaging or verifier defect with capable-runner output and artifact state.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment