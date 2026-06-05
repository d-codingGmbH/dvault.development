[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F8KZPN02NWFGMRC2Q1PKYKDR' for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZPN02NWFGMRC2Q1PKYKDR`.
- Optimistic claim succeeded (`expectedRevision=06F9A8ZC24EACCK3BWT3GVBA38`, `currentRevision=06F9A964RZ3YRJN0NWAK4D8RZ4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc' and commit 'ab2d0a0649af' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc' from source 'ab2d0a0649af'.
- Interactive tester tool loop completed review for branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc'.
- Evidence: Branch diff `develop...ab2d0a0649af` touches `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs`, `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/architect...
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:42-67` adds `metadataSourceResolutionFailed` and returns before generation when a raw model or incompatible support-bundle source is detected.
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:194-214` reports `DMV1960` for unsupported `dvault.support-bundle.*` schema versions and raw `dvault.model.*` additional files.
- Evidence: `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:495-583` adds analyzer coverage for raw model plus valid bundle, incompatible support-bundle version, and ambiguous authoritative bundles; each test asserts `DMV1960` and no gen...
- Evidence: `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:683-719` and `:745-778` prove unsupported PIT or bridge helpers emit `DMV1963` or `DMV1964` while `DVault.GeneratedReadModels.SatCustomerProfile.g.cs` still generates.
- Evidence: `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:211-246` and `:903-944` still generate bridge and PIT helpers from model-first support bundles with request-bound read-shape data and embedded source fingerprint metadata.
- 41 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- DoD check failed: README and any in-repo generator contract text that mention these scenarios match the shipped diagnostic mapping. (`src/DCoding.Data.DVault.Analyzers/README.md:83-91`, `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:18,134-140`, and `src/DCod...
- Blocking: `docs/plans/typed-read-model-generator-contract.md:111-119` still documents raw or residual model-first source-boundary failures as `DMV1968`, while the updated generator, catalog, README, and architecture contract now ship those failures as `DMV1960`.
- Related drift: `docs/releases/v0.25.0.md:102-110` still repeats the old `DMV1968` mapping for raw or residual model-first evidence.

Next steps
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Update `docs/plans/typed-read-model-generator-contract.md` so its diagnostics table matches the shipped mapping: `DMV1960` covers incompatible-version and raw-residual source-boundary failures, and `DMV1968` remains reserved.
- Decide whether the stale historical note in `docs/releases/v0.25.0.md` should be corrected or explicitly called out as historical to avoid repeating the old `DMV1968` mapping.
- After the documentation mismatch is fixed, rerun the tester gate.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8874`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `9ccad9c804764aa29e6f34821360d32f`
- completed-at-utc: `<redacted>-05T00:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZPN02NWFGMRC2Q1PKYKDR/runs/20260605T002610009Z-9ccad9c804764aa29e6f34821360d32f.json`