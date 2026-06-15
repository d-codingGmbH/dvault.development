[gicket-bot] PO-critic review contract

Summary
- Persisted contract is repository-backed, has no open questions, and is specific enough for a documentation-only developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSC4HSXFJ5FM6GWECH2CTGG/description.md sets docs/plans/provider-optimization-gap-matrix.md as the output path, defines the row shape, and records ## Open Questions -> none.
- git diff --name-only develop..HEAD lists only .gicket/tickets/06FBSC4HSXFJ5FM6GWECH2CTGG/...; rg --files docs/plans | rg provider-optimization-gap-matrix returned no match, so the branch is a metadata-only pre-dev handoff and the target doc is not already present.
- docs/plans/provider-optimization-evidence-matrix.md:229-270 and benchmark-summary.md:63-89 already enumerate the requested provider/scenario rows, skipped-placeholder posture, external-provider not executed rows, and the non-SQLite latest-satellite providerSpecificReadStrategy=not registered for latest satellite reads facts.
- docs/architecture/dvault-v1-pit-bridge-boundary.md:13,60 says SQLite is the only optimized latest-satellite read path and that PostgreSQL/SQL Server/MySQL/Oracle/DB2 register PIT/bridge candidates only; the service collection extensions confirm this because src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs registers IDataVaultProviderReadStrategy, while the Postgres/SqlServer/MySql/Oracle/Db2 extensions register only PIT/bridge read strategies plus save strategies.
- docs/releases/v0.34.0.md:41-43,82,154 and tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs:29,130,307,337 bound DB2 to clean-context save plus PIT/bridge read evidence and explicitly exclude DB2 latest-satellite optimization, staged bulk, and provider-native chunk execution.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the contract defines category ordering (non-SQLite latest-satellite capability gaps before save/PIT/bridge evidence gaps) but does not explicitly spell out a provider tie-break rule inside each category; using repository/source order would keep the final matrix deterministic.
- Non-blocking: relevant release notes is not enumerated as exact paths in the contract; the repository baseline currently points most directly to docs/releases/v0.32.0.md, docs/releases/v0.34.0.md, and docs/releases/v0.28.0.md.

Risky assumptions
- A developer could incorrectly treat all non-SQLite rows as evidence gaps; repository evidence says latest-satellite is a capability gap outside SQLite because no provider-specific latest-satellite read strategy is registered.
- A developer could overstate DB2 from smoke/diagnostics evidence; the current baseline allows clean-context save and PIT/bridge candidate wording only, not completed DB2 timing, latest-satellite optimization, staged bulk, or provider-native chunk execution.

AC / test suggestions
- Verify the finished matrix row-for-row against docs/plans/provider-optimization-evidence-matrix.md:229-270 and benchmark-summary.md:63-89 so each provider/scenario entry carries posture, planned strategy or explicit non-strategy, and a finite stop/fallback boundary.
- Prefer explicit source-link citations to docs/releases/v0.32.0.md, docs/releases/v0.34.0.md, and docs/releases/v0.28.0.md rather than a generic relevant release notes label.
- Check final ordering puts all non-SQLite latest-satellite-read rows ahead of external-provider save/PIT/bridge timing-evidence rows.

Implementation watchouts
- Start from a new docs/plans/provider-optimization-gap-matrix.md; the file does not exist on the branch yet.
- Do not convert skipped-placeholder, diagnostics-only, or smoke-only rows into timing claims.
- Keep the SQLite rows as baselines and stop conditions, not as new backlog gaps unless the matrix is explicitly using them as reference comparisons.

Non-blocking notes
- This branch is refinement-only right now: git diff --name-only develop..HEAD changes only .gicket ticket metadata.
- The PO contract and the persisted PO refinement comment are aligned on ready_for_po_critic, bounded documentation scope, and Open Questions: none.

Split recommendations
- No split is needed for this story before developer handoff.
- If follow-up implementation tickets are created from the published matrix, split by gap family: non-SQLite latest-satellite capability work, external-provider read evidence work, and external-provider save evidence work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment