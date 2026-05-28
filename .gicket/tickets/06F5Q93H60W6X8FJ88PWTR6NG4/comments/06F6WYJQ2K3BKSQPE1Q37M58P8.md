[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F5Q93H60W6X8FJ88PWTR6NG4' for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93H60W6X8FJ88PWTR6NG4`.
- Optimistic claim succeeded (`expectedRevision=06F6WWTB8MFMMV6V4NV9SVHVT0`, `currentRevision=06F6WX2PKMA14KFNZGKEWC9EEC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan' and commit '05b8e276fe19' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan' from source '05b8e276fe19'.
- Interactive tester tool loop completed review for branch 'ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan'.
- Evidence: git diff --name-only develop...05b8e276fe19 shows the repository-side delivery is confined to README.md, docs/model-first-governance.md, docs/production-adoption-checklist.md, docs/releases/v0.22.0.md, and src/DCoding.Data.DVault.Analyzers/README.md outside .gicket m...
- Evidence: git ls-files confirms all required evidence paths exist, including docs/quality/api-surface-snapshots.md, tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, docs/plans/stable-hashing-contract.md, tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTes...
- Evidence: README.md adds a typed satellite helper section with DVaultGenerateTypedReadModels, DVaultTypedReadModelMetadataSourceFingerprint, exactly one authoritative support-bundle workflow, dynamic IDataVaultReadService alternatives, compiled EF query guidance, and README lo...
- Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs defines build_property.DVaultGenerateTypedReadModels, build_property.DVaultTypedReadModelMetadataSourceFingerprint, and dvault.support-bundle.v1; tests/DCoding.Data.DVault.Tests/Analyzers/Dat...
- Evidence: docs/model-first-governance.md documents reviewed dvault.model.v1 import, projection, and consumer-owned support-bundle export, and states raw dvault.model.v1 files are not direct generator inputs.
- Evidence: docs/releases/v0.22.0.md links DataVaultTypedReadModelSourceGeneratorTests.cs, StableHashServiceTests.cs, docs/quality/api-surface-snapshots.md, ApiSurfaceSnapshotTests.cs, the six PublicApi approved files, docs/architecture/dvault-ef-compiled-compatibility.md, and t...
- 41 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- DoD check failed: The docs tell one consistent v0.22.0 story for support-bundle-driven typed satellite helpers and sha256-v1 compatibility without reintroducing unsourced API or snapshot claims. (The v0.22.0 story is not fully consistent: README.md and src/DCoding.Data.DVault....
- DoD check failed: The docs keep package publication, support-bundle transport, and approval-snapshot generation explicitly manual and consumer-owned. (Package publication is not kept consistently manual and published-only across the targeted docs because README.md and src/DCod...
- README.md and src/DCoding.Data.DVault.Analyzers/README.md currently imply immediate NuGet availability of 0.22.0 by hardcoding install snippets, which conflicts with the same branch's manual-publication posture in docs/releases/v0.22.0.md and docs/production-adoption-checklist...

Next steps
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Remove or explicitly qualify the 0.22.0 NuGet installation snippets in README.md and src/DCoding.Data.DVault.Analyzers/README.md so the docs stop implying published availability before the manual publication step.
- After the publication-posture wording is corrected, re-run tester review; if command evidence is still required from the read-only tester path, obtain deterministic verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8581`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `0c36a1a44e7f4814bacfdab08ddc66dc`
- completed-at-utc: `<redacted>-28T12:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/runs/20260528T120611984Z-0c36a1a44e7f4814bacfdab08ddc66dc.json`