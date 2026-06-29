<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh repository and .gicket inspection show that v0.48 already shipped alias-coverage and personal-data privacy diagnostics, and upstream ticket 06FGX5NTKQX87FWCZ2GDDVCXEW is done; this ticket can now be refined as an additive structured diagnostics/support-bundle contract and is ready for PO critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No planning writes, description updates, attachments, or relation changes were materialized in this run; refinement is based on live ticket, comment, relation, and repository evidence only.
- Upstream blocker 06FGX5NTKQX87FWCZ2GDDVCXEW is done and already fixes the provider-native encryption boundary; this ticket should consume that unmanaged guidance-only boundary instead of reopening provider capability scope.
- This ticket remains the diagnostics child under story 06FGX5KZHC9ZAKAT71C89MEYV8 and continues to block docs-alignment ticket 06FGX5S4FTGBE7YQ897BMY1974.
- Use the existing diagnostics-to-support-bundle flow as the single implementation path: once privacy adoption facts exist on DataVaultDiagnosticsResult, DataVaultSupportBundle should serialize the same facts under diagnostics rather than inventing a separate privacy-only export path.

### Scope In
- Add additive structured privacy adoption facts to the existing diagnostics and support-bundle surfaces.
- Expose alias-centric facts for registered encrypted-payload aliases, mapped EF properties, coverage status, and key-provider posture.
- Expose marker-centric facts for each personalData satellite payload field, its encryptedPayloadAlias, and its coverage status or cause against the analyzed EF model or metadata.
- Expose an active-provider guidance fact derived from the done provider-native boundary matrix that states the provider-native encryption boundary is unmanaged and guidance-only for DVault, without database probing.
- Add tests for object-model results and serialized support-bundle JSON across the bounded privacy coverage cases.

### Scope Out
- Do not implement provider-native encryption, SQL crypto dispatch, encrypted DDL, provider capability probing, or runtime branching based on native encryption availability.
- Do not take ownership of quickstart or example work; sibling ticket 06FGX5R67T2G0FEGMWE0JBEKJ8 keeps that scope.
- Do not broaden into repository-wide documentation alignment or release-note copy updates beyond minimal code-local comments or tests; sibling ticket 06FGX5S4FTGBE7YQ897BMY1974 owns that.
- Do not introduce a new standalone artifact format, CLI command, or schema fork beyond additive dvault.support-bundle.v1 and diagnostics changes.

## Acceptance Criteria
- The existing diagnostics result exposes additive machine-readable privacy adoption facts that support-bundle export reuses unchanged under diagnostics, so callers do not have to parse human-readable issue prose for alias coverage.
- Alias-centric facts preserve the repository-backed v0.48 baseline for registered aliases and key-provider posture, including the finite visible statuses covered and registered-but-unmapped plus posture values none, marker-only, and encrypted-payload-capable.
- Marker-centric facts report each marked satellite payload field and encryptedPayloadAlias, and distinguish the bounded visible coverage outcomes needed for this ticket: proof missing, alias unregistered, unusable key-provider posture, proof failure or no evaluation, no observable converter wiring, converter-alias mismatch, and covered.
- For the selected or active provider profile, diagnostics and support-bundle output include deterministic provider-native encryption boundary facts that state the boundary is unmanaged and guidance-only for DVault and never come from live database encryption probing.
- Structured privacy facts and related issue text remain redaction-safe and exclude plaintext payload values, ciphertext payload bodies, key material, secrets, provider connection details, and provider-specific encryption settings.
- Tests cover diagnostics and support-bundle JSON for configured, missing, mismatched, and unusable privacy coverage cases and verify additive compatibility with the existing dvault.support-bundle.v1 artifact.

## Definition of Done
- Core diagnostics and support-bundle code plus the optional privacy package keep the current dependency direction; no core public type depends directly on DCoding.Data.DVault.Privacy concrete types.
- The structured facts are additive to existing diagnostics and support-bundle consumers and keep the current support-bundle schema version and deterministic camelCase JSON behavior.
- Existing warning and error semantics remain aligned: proof-missing stays advisory, configured-but-unusable coverage stays fail-closed, and structured status or cause data matches those outcomes.
- Executable tests verify both object-model results and serialized support-bundle output for the accepted coverage and provider-boundary cases.
- Downstream docs-alignment work can cite the new structured facts without reopening provider scope or quickstart scope.

## Implementation Notes
- The repository already has two partial inputs that this ticket should unify, not replace: DataVaultPrivacyCoverageReporter in the optional privacy package for alias-centric facts and DefaultDataVaultDiagnosticsService personal-data coverage checks in core for marker-centric validation.
- Keep diagnostics as the authoritative source and let DataVaultSupportBundleExporter serialize the same bounded privacy facts; avoid duplicating logic in a second support-bundle-only path.
- Preserve the optional privacy boundary by introducing any new diagnostics-facing abstraction in the core package and having DCoding.Data.DVault.Privacy implement and register it, rather than making core depend on privacy-package concrete report types.
- Use the done boundary already documented in docs/architecture/dvault-v1-optional-privacy-extension-boundary.md, docs/getting-started.md, docs/package-compatibility.md, and docs/production-adoption-checklist.md as the source of truth for the unmanaged provider-native encryption guidance fact.
- Do not claim converter coverage from metadata-only analysis. When no configured DbContext model exposes DataVaultEncryptedPayloadValueConverter, structured facts must say coverage is not observable or usable instead of silently treating metadata markers as covered.
- Model the current finite code-backed coverage branches explicitly in tests, especially the missing test lane where a marked payload field uses DataVaultEncryptedPayloadValueConverter with a different alias than the personalData marker.

## Open Questions
- none

## Follow-Up Questions
- After structured facts land, should ticket 06FGX5S4FTGBE7YQ897BMY1974 include a small JSON excerpt in the docs, or keep documentation at the prose and boundary level only?
- If future provider-specific native encryption work is approved, should each provider ticket extend this boundary vocabulary only for its named capability instead of widening the shared provider-neutral contract?

## Risks
- The current alias report lives in the optional privacy package while diagnostics and support-bundle live in core; a careless implementation could invert package dependencies or leak optional-package types into the core public API.
- If structured statuses are not clearly separated between alias-centric and marker-centric coverage, consumers may confuse registered-but-unmapped alias facts with fail-closed personalData coverage failures.
- Any non-additive JSON change or accidental inclusion of provider settings or connection details would break the redacted support-bundle contract and downstream consumers.

## Split Recommendations
- No further split is needed for this ticket: the parent story already isolates provider-boundary work in 06FGX5NTKQX87FWCZ2GDDVCXEW, quickstart work in 06FGX5R67T2G0FEGMWE0JBEKJ8, and documentation alignment in 06FGX5S4FTGBE7YQ897BMY1974.
- If later work moves beyond structured facts into actual native encryption behavior, create one provider-specific follow-up ticket per exact capability rather than widening this diagnostics task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Expose deterministic privacy-adoption facts through diagnostics or support bundles without leaking sensitive values.

Acceptance:
- Support-bundle or diagnostics output reports registered encrypted payload aliases, mapped personal-data aliases, coverage status, and unmanaged provider-native encryption boundary facts.
- Output does not include plaintext payload values, key material, secrets, or provider connection details.
- Tests cover configured, missing, mismatched, and unusable privacy coverage cases.
- The feature remains provider-neutral and does not probe database encryption settings.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- decision: implemented
- Added additive `DataVaultDiagnosticsResult.Privacy` facts for alias coverage, personal-data marker coverage, key-provider posture, and unmanaged guidance-only provider-native encryption boundary status.
- Kept diagnostics as the authoritative support-bundle source; `DataVaultSupportBundleExporter` serializes the same privacy facts under `diagnostics` without changing `dvault.support-bundle.v1`.
- Preserved the optional privacy package boundary by adding a core abstraction and registering the privacy-package implementation through `AddDVaultPrivacy`.
- No provider-native encryption, provider capability probing, provider-specific encryption settings, key material, payload values, or connection details were added to diagnostics or support bundles.

## Verification

- `dotnet build DVault.slnx --nologo --no-restore` passed with existing warning noise and 0 errors.
- `dotnet test DVault.slnx --nologo --no-restore --no-build` passed; external provider integration tests without local connection strings were skipped by existing test gates.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --no-restore --no-build` passed for `net8.0` and `net10.0`.
- `bash tools/check-format.sh` passed.

<!-- gicket-bot:developer-delivery:v1:end -->