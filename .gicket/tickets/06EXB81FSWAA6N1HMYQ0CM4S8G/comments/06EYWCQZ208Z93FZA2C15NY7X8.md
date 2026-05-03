[gicket-bot] PO refinement contract

Summary
- Verified the live ticket, comments, relations, and repository baseline; this work is bounded to six packable packages with package-specific API snapshot baselines and is ready for PO-critic without creating new planning artifacts.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Persisted comments added on 2026-05-03 are automation follow-up, claim, and lease notes only; there is no human scope change or attachment to incorporate into this refinement.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement pass.

Scope In
- A repository-enforced API review gate for the six packable packages: `DCoding.Data.DVault`, `DCoding.Data.DVault.Sqlite`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.SqlServer`, `DCoding.Data.DVault.Oracle`, and `DCoding.Data.DVault.MySql`.
- Committed approval, baseline, or compatibility snapshot artifacts that record each package's public API separately and require a deliberate update when that package surface changes.
- Coverage of the current consumer-facing public API, including `AddDVault*` registration entry points, `UseDataVault`, `ApplyDataVaultMetadata`, `IDataVaultSaveService`, provider save-strategy contracts, and provider capability/profile contracts.
- Contributor-facing documentation that explains how to run the API review locally, interpret package-specific output, and intentionally update approved baselines.

Scope Out
- `src/DCoding.Data/DCoding.Data.csproj`, test projects, and benchmarks as API-review targets, because they are non-packable and not current release surfaces.
- Provider-specific runtime behavior, persistence semantics, or new public API design beyond reviewing the surfaces that already exist.
- A release-history or published-NuGet backward-compatibility program beyond the v1 repository baseline for the current packable packages.
- The separate one-member-per-file analyzer work already tracked by `06EXB81QXE7XJPNM6NTPYCTP1M`.

Open questions
- none

Follow-up questions
- After the first public package release, should DVault add a second compatibility check against the last published NuGet versions in addition to the repository-managed baselines?
- If new packable provider packages are added later, should the API-review mechanism auto-discover packable `src/DCoding.Data.DVault.*` projects or require an explicit allowlist update?

Risks
- A namespace-based or single aggregated snapshot would be misleading because the provider packages share the `DCoding.Data.DVault` namespace and could hide package-boundary regressions.
- If the check inspects only source declarations and not built package or assembly output, it can miss packaging-level API drift or attribute public surface changes to the wrong package.

Split recommendations
- No additional split is recommended; the ticket is already bounded to one package-aware API review gate, with XML-doc enforcement upstream in `06EXB817Q8RAXCQH5QQR5RFY34` and one-member-per-file analyzer work downstream in `06EXB81QXE7XJPNM6NTPYCTP1M`.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment