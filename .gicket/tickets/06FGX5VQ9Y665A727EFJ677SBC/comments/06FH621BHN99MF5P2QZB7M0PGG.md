[gicket-bot] PO-critic review contract

Summary
- Return to PO: the child coverage and implemented validator/preflight/doc surfaces exist, but the checked-in contract is not internally consistent about whether validation findings are serialized manifest input or validator output.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FGX5VQ9Y665A727EFJ677SBC/description.md:50-51` has `## Open Questions` with `- none`, so the blocker is not unresolved ticket questions.
- `git -C /mnt/c/Projects/DVault log --oneline --grep='06FGX67TZV1F6S949F96ZE201W\|06FGX69QJYHGNKBV8MJ1HG7MMG\|06FGX6B9KQME0NJ8B810239DG0\|06FGX6CRPG02ZWGE62QWSG42EC' -n 20` shows develop already contains `d4e341bef`, `3d580b90e`, `c9fbbac79`, and `20b6aed8c` auto-integration commits for the four child tickets.
- `git -C /mnt/c/Projects/DVault diff --name-only develop..HEAD` lists only `.gicket/tickets/06FGX5VQ9Y665A727EFJ677SBC/...` files, so this owner branch is a tracking-only ticket branch and adds no repository code/doc changes beyond ticket metadata.
- Direct implementation evidence exists: `src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs:471-477` defines the serialized manifest as exactly `SchemaVersion`, `DryRun`, `Source`, `Target`, `Comparison`, and `Entries`; `src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs` exposes `DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(...)`; `src/DCoding.Data.DVault/DataVaultPreflightRequest.cs` exposes `HashKeyStorageMigrationManifestJson`; and `src/DCoding.Data.DVault/DataVaultPreflight.cs:141-155` keeps this as a separate `hash-key-storage-migration-manifest` preflight section.
- The public guidance and tests match that six-key shape: `docs/hash-key-storage-migration.md:87-149`, `docs/getting-started.md:243`, `docs/releases/v0.49.0.md:44-52`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs:159-258` all treat findings as validator/preflight output, not serialized manifest input.
- Conflicting repository contract text remains in `docs/plans/hash-key-storage-profile-contract.md:69-77`, which still says a valid manifest declares top-level facts including `deterministic validation findings grouped as error, warning, and info`; that conflicts with the exporter shape above and with the parent contract statements in `.gicket/tickets/06FGX5VQ9Y665A727EFJ677SBC/description.md:13` and `:39`.

Blocking findings
- Repository contract inconsistency: `docs/plans/hash-key-storage-profile-contract.md:69-77` still defines validation findings as part of the manifest's top-level facts, while the implemented exporter (`src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs:471-477`), validator/preflight surfaces, tests, and the parent ticket contract all define the serialized v1 artifact as only `schemaVersion`, `dryRun`, `source`, `target`, `comparison`, and `entries`, with findings produced as output. That directly breaks the parent Definition of Done claim that the visible repo baseline is consistent.

Required PO actions
- Reopen `06FGX67TZV1F6S949F96ZE201W` or create one bounded follow-up ticket to reconcile `docs/plans/hash-key-storage-profile-contract.md` with the implemented six-key v1 manifest shape.
- Keep the parent ticket in PO until the ticket contract and cited repository contract both say the same thing about findings being validator output rather than serialized manifest input.

Open issues ledger
- critic-item-1 [required-po-action] Reopen `06FGX67TZV1F6S949F96ZE201W` or create one bounded follow-up ticket to reconcile `docs/plans/hash-key-storage-profile-contract.md` with the implemented six-key v1 manifest shape.
- critic-item-2 [required-po-action] Keep the parent ticket in PO until the ticket contract and cited repository contract both say the same thing about findings being validator output rather than serialized manifest input.
- critic-item-3 [blocking-finding] Repository contract inconsistency: `docs/plans/hash-key-storage-profile-contract.md:69-77` still defines validation findings as part of the manifest's top-level facts, while the implemented exporter (`src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs:471-477`), validator/preflight surfaces, tests, and the parent ticket contract all define the serialized v1 artifact as only `schemaVersion`, `dryRun`, `source`, `target`, `comparison`, and `entries`, with findings produced as output. That directly breaks the parent Definition of Done claim that the visible repo baseline is consistent.

Missing examples / edge cases
- A redacted sample `dvault.hash-key-storage-migration.v1` manifest or sample preflight output is still only a follow-up question in the parent contract; that is non-blocking after the contract mismatch is fixed.

Risky assumptions
- Assuming `docs/plans/hash-key-storage-profile-contract.md` is merely historical or ignorable is risky because the parent ticket implementation notes still cite it as part of the bounded contract surface.

AC / test suggestions
- Add a doc-contract assertion or equivalent regression test that the serialized v1 manifest contract names only `schemaVersion`, `dryRun`, `source`, `target`, `comparison`, and `entries`, and keeps validation findings out of the input artifact.
- If the team ever wants embedded findings in a later manifest, capture that in a separate versioned successor ticket instead of changing `dvault.hash-key-storage-migration.v1` in place.

Implementation watchouts
- This owner branch is tracking-only: `git diff --name-only develop..HEAD` shows only `.gicket/tickets/06FGX5VQ9Y665A727EFJ677SBC/...` files. Any repository contract correction must land through a reopened or new child slice, not be assumed already present here.
- Do not collapse this lane into migration guardrails while fixing the contract; `src/DCoding.Data.DVault/DataVaultPreflight.cs:141-155` currently preserves `hash-key-storage-migration-manifest` as its own preflight section.

Non-blocking notes
- Direct source evidence for the built-in stable-hash baseline still matches the parent contract: `src/DCoding.Data.DVault/BuiltInStableHashService.cs` declares `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- The decomposition itself looks adequate: the four child tickets cover contract, validator, preflight integration, and documentation, and develop already contains auto-integration commits for all four child ids.

Split recommendations
- No broader split is needed; one narrow contract-alignment reopen/follow-up for `docs/plans/hash-key-storage-profile-contract.md` is sufficient.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment