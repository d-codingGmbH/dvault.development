[gicket-bot] PO-critic review contract

Summary
- Ticket is now clear enough for developer handoff: it explicitly binds the validator to the current dry-run manifest shape, defines how invalid fixtures are sourced, and leaves no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted delivery contract now states that the validator input is the current `dvault.hash-key-storage-migration.v1` dry-run artifact with top-level `schemaVersion`, `dryRun`, `source`, `target`, `comparison`, and `entries`, and that invalid-manifest tests use mutated current-shape fixtures instead of producer-emitted invalid files.
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs` directly matches that clarified contract: `ExportDryRunJson(...)` serializes `{ schemaVersion, dryRun, source, target, comparison, entries }`, and `ValidateSourceFacts`, `ValidateTargetFacts`, and `ValidatePairFacts` enforce the storage-only `HexString` to `Binary` semantics plus unchanged algorithm, digest length, and digest encoding.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs` exposes the built-in provider baseline named in the ticket: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`.
- `src/DCoding.Data.DVault/BuiltInStableHashService.cs` exposes the built-in stable-hash baseline named in the ticket: `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- `git rev-parse HEAD` returned `224f0befc743422a763bb541bb54942055952134`, matching the prompt branch-head identity, and `git status --short --branch` returned `## HEAD (no branch)` with no modified files, so the review surface is a clean detached scratch worktree.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The repository docs still describe conceptual top-level keys such as `selectedModelBoundary`, `reviewedSourceEvidence`, `expectedStorageProfiles`, `coverage`, and `validation`, so developers will need to follow the ticket's explicit semantic mapping rather than the older prose alone.
- The ticket does not include a concrete warning example payload for the supplemental live-schema-unavailable case, only the rule that warnings are limited to non-blocking evidence gaps.

Risky assumptions
- Future contributors may read `docs/hash-key-storage-migration.md` or `docs/plans/hash-key-storage-profile-contract.md` without the ticket context and assume the conceptual field names are the serialized v1 keys.
- Validator fixture helpers will need to stay aligned with any later exporter-shape change because invalid manifests are intentionally hand-built from a valid current-shape artifact.

AC / test suggestions
- Keep one acceptance case that validates an unmodified current producer artifact with the exact `schemaVersion`/`dryRun`/`source`/`target`/`comparison`/`entries` shape.
- Include deterministic negative fixtures for missing sections, missing per-entry facts, duplicate coverage identity, mixed source or target profiles, unsupported provider/profile/value-format/conversion/hash facts, and digest or encoding drift.
- Add at least one explicit test around the `sha1-v1` versus `sha256-160-v1` equal-size incompatibility because both the docs and code baselines call that case out as special.

Implementation watchouts
- Do not widen this task into exporter-schema changes; the refined contract explicitly keeps `DataVaultHashKeyStorageMigrationManifestExporter` and the `hash-key-storage-migration` command shape out of scope.
- Treat `entries` as complete boundary coverage and use the ticket's semantic mapping from `source`/`target`/`comparison`/`entries` to the conceptual v1 contract rather than inventing new serialized keys.
- Keep findings redacted and deterministic; the checked-in docs prohibit raw hash-key values, raw business keys, SQL text, credentials, connection strings, and provider exception text in validation output.

Non-blocking notes
- `gicket-read-ticket-comments` returned a prior PO-critic blocking assessment followed by a later PO refinement comment that answered the shape and fixture-sourcing issues, which is consistent with the updated delivery contract now persisted on the ticket.
- Related downstream ticket `06FGX6B9KQME0NJ8B810239DG0` remains `todo`, so keeping this ticket validator-only still preserves the intended upstream/downstream split.

Split recommendations
- No split is needed for this ticket as currently refined.
- If the team later wants a new serialized manifest shape, embedded validation payloads, or a successor manifest version, open a separate follow-up ticket rather than broadening this validator-only task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment