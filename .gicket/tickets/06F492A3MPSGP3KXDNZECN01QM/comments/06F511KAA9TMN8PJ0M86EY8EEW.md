[gicket-bot] tracking-epic-closure-v1

Summary
- Closed tracking-only epic '06F492A3MPSGP3KXDNZECN01QM' because all parentOf child tickets are done and no parent-owned implementation slice remains.
- PO-critic closure audit approved that the completed child set satisfies the parent tracking-only epic.

Evidence
- parent ticket: `06F492A3MPSGP3KXDNZECN01QM`
- parentOf child `06F492A8WV0EP2V03CWXXWH71G` status `done`
- parentOf child `06F492AE2C8XBDXDH4V2JPTJDR` status `done`
- parentOf child `06F492AKGMKPCRJYF4Z1EC9WY4` status `done`
- parentOf child `06F492ARW2N6SNYJH15RHMZEN8` status `done`
- parentOf child `06F492AYE4A3PKA2D20DDPQ37C` status `done`
- parentOf child `06F492B40K7B0WWPKH8N3PPG3G` status `done`
- parentOf child `06F492B9PR036PDNN52S06S9BC` status `done`
- parentOf child `06F492BG6BZYYFMBE5WK7CB024` status `done`
- parentOf child `06F492BNDPWS9P4EDSV0W7G6VM` status `done`

PO-critic audit evidence
- `.gicket/tickets/06F492A3MPSGP3KXDNZECN01QM/description.md` contains the authoritative delivery contract and shows `## Open Questions` as `- none`.
- Nine persisted `parentOf` relations exist for the epic under `.gicket/relations/QM/{1G,24,3G,7C,BC,DR,N8,VM,Y4}/06F492A3MPSGP3KXDNZECN01QM--...--parentOf.json`, matching the claimed child set.
- Each child contract also shows `## Open Questions` as `- none` in `.gicket/tickets/<id>/description.md`, so the epic is not carrying unresolved child-level contract questions.
- `git log --oneline --all --grep <child-id>` finds AUTO-INTEGRATION commits for every child: `7f42e0bc1`, `f90a96394`, `60a78c8b2`, `ab6f88e8f`, `d7647adbc`, `c0558e1c2`, `3c001a2c3`, `a8990a4e8`, and `5e455b93b`.
- `src/DCoding.Data.DVault/DataVaultPreflight.cs` defines `DataVaultPreflight.Run(...)` as a caller-owned aggregate over validation, artifact drift, snapshot drift, migration guardrail, and request diagnostics, with omitted lanes marked skipped rather than auto-discovered.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs` exposes `UseDataVaultSaveChangesGuardInterceptor(...)`, and `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs` defines `DMV1910` and `DMV1911`.
- `docs/releases/v0.17.0.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, and `README.md` consistently describe the v0.17.0 baseline, consumer-owned design-time/preflight workflow, opt-in runtime guard, opt-in telemetry/support-bundle enrichment, and non-goals such as no standalone DVault CLI or automatic migration/snapshot discovery.

PO-critic non-blocking notes
- The review branch is `ticket/06F492A3MPSGP3KXDNZECN01QM-epic-ef-core-safety-and-preflight`, `git rev-parse HEAD` is `7b56b411dba5e64379312da79d91ecd625a9ea67`, and `git diff --quiet 7b56b411dba5e64379312da79d91ecd625a9ea67 HEAD` returned exit `0`.

PO-critic closure watchouts
- Keep `DataVaultPreflight.Run(...)` additive and caller-owned; `src/DCoding.Data.DVault/DataVaultPreflight.cs` deliberately skips omitted lanes instead of discovering snapshots, migrations, or representative requests.
- Keep runtime guard opt-in only; `docs/releases/v0.17.0.md` and `docs/production-adoption-checklist.md` both state that `AddDVault()` does not enable `UseDataVaultSaveChangesGuardInterceptor(...)` automatically.
- Keep analyzer scope bounded to EF misuse around generated shared-type tables; `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs` and `src/DCoding.Data.DVault.Analyzers/README.md` do not justify broad runtime/provider inference.

<!-- gicket-semantic-idempotency-key: bot-closure:06f492a3mpsgp3kxdnzecn01qm:tracking-epic:done:done -->