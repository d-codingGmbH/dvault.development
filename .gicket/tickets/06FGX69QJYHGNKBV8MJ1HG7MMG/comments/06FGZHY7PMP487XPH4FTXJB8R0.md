[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket contract conflicts with the currently emitted dvault.hash-key-storage-migration.v1 manifest shape and does not define how invalid manifests are sourced for validation.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs` defines `dvault.hash-key-storage-migration.v1` as `{ schemaVersion, dryRun, source, target, comparison, entries }`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs` asserts the `hash-key-storage-migration` command writes `dryRun`, `source`, `target`, `comparison`, and `entries`, and its `HashKeyStorageMigrationFailsClosedForAlgorithmAndDigestDrift` case expects exit code `1` with no output file on blocking drift.
- `docs/hash-key-storage-migration.md` and `docs/plans/hash-key-storage-profile-contract.md` describe the v1 validator input as requiring `selectedModelBoundary`, `reviewedSourceEvidence`, `providerProfileId`, `modelHashFacts`, `expectedStorageProfiles`, `coverage`, and `validation`.
- Repository baselines do align on provider/hash vocabularies: `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs` lists `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`, and `src/DCoding.Data.DVault/BuiltInStableHashService.cs` lists `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- Dependency state is stale in the contract text: `.gicket/tickets/06FGX67TZV1F6S949F96ZE201W/ticket.json` is `done` while `.gicket/relations/1W/MG/06FGX67TZV1F6S949F96ZE201W--06FGX69QJYHGNKBV8MJ1HG7MMG--blocks.json` still records the relation and the description says this ticket is 'currently blocked by' it.

Blocking findings
- The authoritative contract and the checked-in producer disagree on the `dvault.hash-key-storage-migration.v1` top-level shape. The ticket does not say whether the validator must consume the current emitted `dryRun/source/target/comparison/entries` manifest, replace that shape, or introduce a new version.
- The ticket expects deterministic `error`/`warning`/`info` validation findings for invalid manifests, but the only checked-in producer currently fails closed and writes no manifest on blocking drift. The input source for invalid-manifest scenarios is unspecified.

Required PO actions
- Reconcile the authoritative v1 manifest schema across `docs/hash-key-storage-migration.md`, `docs/plans/hash-key-storage-profile-contract.md`, and the checked-in `hash-key-storage-migration` exporter/tests. State clearly whether this ticket preserves the current emitted shape, changes it, or needs a versioned successor.
- State whether this ticket also owns updates to the existing dry-run manifest producer and its tests/docs, or whether producer-shape changes belong to a separate ticket.
- Specify how invalid-manifest fixtures are expected to exist for this validator when the current producer exits with an error and writes no output file.
- Refresh the stale dependency wording in the delivery contract so it reflects that ticket `06FGX67TZV1F6S949F96ZE201W` is already `done`.

Open issues ledger
- critic-item-1 [required-po-action] Reconcile the authoritative v1 manifest schema across `docs/hash-key-storage-migration.md`, `docs/plans/hash-key-storage-profile-contract.md`, and the checked-in `hash-key-storage-migration` exporter/tests. State clearly whether this ticket preserves the current emitted shape, changes it, or needs a versioned successor.
- critic-item-2 [required-po-action] State whether this ticket also owns updates to the existing dry-run manifest producer and its tests/docs, or whether producer-shape changes belong to a separate ticket.
- critic-item-3 [required-po-action] Specify how invalid-manifest fixtures are expected to exist for this validator when the current producer exits with an error and writes no output file.
- critic-item-4 [required-po-action] Refresh the stale dependency wording in the delivery contract so it reflects that ticket `06FGX67TZV1F6S949F96ZE201W` is already `done`.
- critic-item-5 [blocking-finding] The authoritative contract and the checked-in producer disagree on the `dvault.hash-key-storage-migration.v1` top-level shape. The ticket does not say whether the validator must consume the current emitted `dryRun/source/target/comparison/entries` manifest, replace that shape, or introduce a new version.
- critic-item-6 [blocking-finding] The ticket expects deterministic `error`/`warning`/`info` validation findings for invalid manifests, but the only checked-in producer currently fails closed and writes no manifest on blocking drift. The input source for invalid-manifest scenarios is unspecified.

Missing examples / edge cases
- There is no example showing whether a manifest emitted today by `hash-key-storage-migration` should be accepted or rejected by the validator.
- There is no concrete example of the expected `validation` finding payload and stable codes for the non-blocking live-schema-unavailable warning path.
- There is no explicit example covering duplicate coverage identity when provider-specific casing normalization does or does not apply.

Risky assumptions
- Assuming `dvault.hash-key-storage-migration.v1` can change top-level JSON shape without breaking the existing design-time command, tests, or any consumer already reading the emitted manifest.
- Assuming invalid manifests will come from some external or future producer even though the current checked-in producer does not emit them.
- Assuming developers can infer whether both the current `comparison` summary and the proposed `validation` findings must coexist in the same artifact.

AC / test suggestions
- Add one acceptance case that feeds the validator an actual current `hash-key-storage-migration` output artifact and states explicitly whether that artifact is valid input.
- Add fixture-based validation cases for missing coverage, duplicate coverage, unsupported provider/profile, and `sha1-v1` versus `sha256-160-v1`, with deterministic finding ordering across `error`, `warning`, and `info`.
- Add an acceptance case for the authoritative-support-bundle-complete plus live-schema-unavailable warning lane so the intended warning payload is locked down.

Implementation watchouts
- Do not reuse `schemaVersion = dvault.hash-key-storage-migration.v1` for two incompatible manifest shapes without an explicit compatibility decision.
- Keep findings redacted; the contract and existing docs both prohibit raw hash-key values, raw business keys, SQL text, connection strings, and provider exception text in diagnostics.
- Keep coverage identity and any provider-specific casing normalization rule deterministic and bounded to explicitly documented provider behavior only.

Non-blocking notes
- The checked-in provider-profile and stable-hash baselines in `DataVaultProviderCapabilityProfiles.cs`, `DataVaultProviderCapabilityProfileTests.cs`, and `BuiltInStableHashService.cs` are consistent with the refined vocabulary in the ticket.
- The related downstream wiring ticket `06FGX6B9KQME0NJ8B810239DG0` is still `todo`, which is consistent with keeping this ticket validator-focused once the schema question is resolved.

Split recommendations
- No scope split is needed if PO simply reconciles the manifest contract. If PO decides the existing dry-run producer must change shape under this work, consider separating producer-schema migration from validator-only logic because downstream wiring already has its own ticket `06FGX6B9KQME0NJ8B810239DG0`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment