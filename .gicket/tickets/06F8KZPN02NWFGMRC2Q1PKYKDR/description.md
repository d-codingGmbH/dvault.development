<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to the support-bundle-only typed read-model generator contract, clarifying the expected diagnostics for source resolution, fingerprint drift, request-bound PIT/bridge ReadShape failures, and model-first boundary handling; no persistent planning writes were applied.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The generator input boundary remains exactly one authoritative `dvault.support-bundle.v1` additional file; this story does not widen the generator to parse raw `dvault.model.v1`, Code-First callbacks, or literal metadata-first declarations directly.
- `DMV1960` is the authoritative source-resolution diagnostic for missing, invalid, ambiguous, or wrong-version support-bundle inputs.
- `DMV1961` covers `DVaultTypedReadModelMetadataSourceFingerprint` drift and suppresses helper generation until the configured fingerprint matches the authoritative bundle fingerprint.
- `DMV1963` and `DMV1964` are request-bound shape diagnostics: PIT and bridge helpers need matching `diagnostics.readShape` facts as well as compatible explain metadata.
- Under the current v1 baseline, raw or changed model-first artifacts outside the projected support-bundle contract are rejected through the same authoritative-source boundary rather than by adding a direct raw-model parsing lane.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this pass.

### Scope In
- Support-bundle source-resolution diagnostics for missing, invalid, ambiguous, or incompatible-version inputs.
- Fingerprint drift detection against the authoritative support-bundle metadata-source fingerprint.
- Entity-specific PIT and bridge diagnostics when request-bound `ReadShape` facts are missing, mismatched, or outside the bounded helper contract.
- Model-first boundary handling at the generator input edge without widening inputs beyond projected support bundles.
- Regression coverage and package-local documentation alignment for the affected diagnostics.

### Scope Out
- New generator parsing of raw `dvault.model.v1` files or source-visible Code-First/metadata-first declarations.
- Changes to runtime support-bundle export or orchestration, or to how representative PIT/bridge requests are captured.
- New typed helper families beyond the existing satellite, PIT, and bounded bridge v1 contract.
- Provider-specific SQL, maintenance, or query-planning behavior changes.

## Acceptance Criteria
- With `DVaultGenerateTypedReadModels=true`, resolving anything other than exactly one authoritative `dvault.support-bundle.v1` additional file results in `DMV1960` and no generated helpers.
- When `DVaultTypedReadModelMetadataSourceFingerprint` is configured and does not match the resolved bundle fingerprint, the generator reports `DMV1961` and suppresses generation.
- When PIT explain metadata or request-bound `diagnostics.readShape.pit` facts are missing, mismatched, or outside the bounded PIT helper contract, the generator reports `DMV1963` for the affected PIT helper while leaving unrelated supported helpers eligible.
- When bridge explain metadata or request-bound `diagnostics.readShape.bridge` facts are missing, mismatched, or outside the bounded bridge helper contract, the generator reports `DMV1964` or `DMV1967` as appropriate for the affected bridge helper while leaving unrelated supported helpers eligible.
- A projected model-first support bundle with matching fingerprint and required ReadShape facts continues to generate supported PIT and bridge helpers.
- Raw or residual `dvault.model.v1` artifacts presented outside the projected support-bundle contract report `DMV1960` under the current source-boundary baseline and do not widen generator inputs.

## Definition of Done
- Generator code paths and analyzer tests cover the `DMV1960`, `DMV1961`, `DMV1963`, `DMV1964`, and `DMV1967` paths touched by this story, plus the accepted raw-model rejection behavior.
- README and any in-repo generator contract text that mention these scenarios match the shipped diagnostic mapping.
- Supported satellite, PIT, and bridge helpers continue generating for unaffected entities in mixed bundles.
- No direct raw-model parsing path or unreviewed metadata-source fallback is introduced.

## Implementation Notes
- Repository evidence already establishes a support-bundle-driven generator baseline; refine diagnostics on that baseline instead of reopening generator inputs.
- Treat incompatible support-bundle versions as source-resolution failures because the authoritative schema version is `dvault.support-bundle.v1`.
- Keep model-first acceptance support-bundle-based: projected model-first bundles with matching fingerprint and required ReadShape evidence stay valid, while raw model-first artifacts remain outside the generator input contract.
- PIT and bridge validation should compare entity identity, parent references, snapshot/reference columns, endpoint columns, and bounded traversal facts against the specific entity so failures stay entity-specific.
- Current live relations show this ticket is a child of `06F8KZP0VKMXGE0JXPZRD1RQDG`, is blocked by `06F8KZP9XJ868GY6GT934QVFH4`, and blocks `06F8KZPZZE8VZEBANP5MPN8HH8`.

## Open Questions
- none

## Follow-Up Questions
- After this story lands, do we want to activate or retire the unused `DMV1968` catalog/README slot so the public diagnostic list matches the shipped behavior?
- Is README-level documentation enough for these diagnostics, or should a later release-note pass add concrete support-bundle failure examples?

## Risks
- Current repository evidence is internally inconsistent: the diagnostic catalog and README reserve `DMV1968`, but the executable generator tests currently expect raw `dvault.model.v1` additional files to fall into `DMV1960`.
- Because PIT and bridge evidence is request-bound, incomplete fixture data can accidentally exercise the wrong diagnostic lane and hide regressions.
- This ticket is still a child of `06F8KZP0VKMXGE0JXPZRD1RQDG`, is blocked by `06F8KZP9XJ868GY6GT934QVFH4`, and blocks `06F8KZPZZE8VZEBANP5MPN8HH8`, so dependency drift can delay downstream delivery even after refinement.

## Split Recommendations
- No split recommended; the remaining work stays bounded to generator diagnostics, tests, and package-local documentation alignment.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement source-generator diagnostics for stale fingerprints, missing ReadShape facts, incompatible support-bundle versions, missing metadata sources, and changed model-first artifacts.

<!-- gicket-bot:developer-delivery:06F8KZPN02NWFGMRC2Q1PKYKDR:v1:start -->
## Developer Delivery

Summary
- Implemented source-boundary DMV1960 handling for incompatible support-bundle schema versions and raw or residual dvault.model.v1 additional files, including mixed valid support-bundle plus raw-model input suppression.
- Added regression coverage for ambiguous support bundles, incompatible bundle versions, raw model residual inputs, and entity-specific PIT or bridge diagnostic skip behavior while preserving unrelated satellite helper generation.
- Aligned package README, typed helper contract, and the reserved DMV1968 catalog description with the shipped DMV1960 raw-model boundary.

Verification
- dotnet test tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj --nologo passed: 89 succeeded.
- bash tools/check-format.sh passed.
- dotnet build DVault.slnx --nologo passed with existing NU1900 vulnerability-cache warnings and existing analyzer warnings.
- dotnet test DVault.slnx --nologo passed; external provider integration tests skipped where local connection strings were absent.

<!-- gicket-bot:developer-delivery:06F8KZPN02NWFGMRC2Q1PKYKDR:v1:end -->

<!-- gicket-bot:developer-delivery:06F8KZPN02NWFGMRC2Q1PKYKDR:v1:start -->
## Developer Delivery

Summary
- Reworked the tester-returned documentation gap by updating `docs/plans/typed-read-model-generator-contract.md` so the superseded v0.22 planning contract points to the current v1 typed PIT/bridge helper contract and no longer conflicts with shipped diagnostics.
- Preserved the historical satellite-only context while recording current `DMV1960` source-boundary behavior for incompatible support bundles and raw or residual `dvault.model.v1` files, current `DMV1963`/`DMV1964`/`DMV1967` PIT and bridge ReadShape mapping, and reserved `DMV1968` behavior.
- Prior branch implementation remains in place for generator diagnostics, analyzer regression tests, analyzer README, diagnostic catalog, and the typed helper contract.

Verification
- `dotnet test tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj --nologo` passed: 89 succeeded.
- `bash tools/check-format.sh` passed.
- `DOTNET_CLI_TELEMETRY_OPTOUT=1 dotnet build DVault.slnx --nologo` passed with existing `NU1900` read-only vulnerability-cache warnings and existing analyzer/test warnings.
- `DOTNET_CLI_TELEMETRY_OPTOUT=1 dotnet test DVault.slnx --nologo` passed; integration tests reported 191 succeeded and 21 external-provider tests skipped because local connection strings were absent, and unit tests reported 446 succeeded. The analyzer test project was also verified separately as above.

<!-- gicket-bot:developer-delivery:06F8KZPN02NWFGMRC2Q1PKYKDR:v1:end -->