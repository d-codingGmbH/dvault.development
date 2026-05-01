[gicket-bot] PO refinement contract

Summary
- Refined the Entity Framework and persistence epic against current repository evidence; no new planning writes were needed because the epic already has four child tickets and the v1 SQLite-first explicit-save baseline is already visible in repository docs and code.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This epic is an orchestration ticket over the existing child-ticket split `06EXB7FF1J9NR2849WKDR8DKPG`, `06EXB7G6YE4X0GA0CT7EPEFMPR`, `06EXB7GYQKBZ8FMQN6YDYCKATG`, and `06EXB7HYG17X73GH0K535GYJH8`; no additional split was justified from the current evidence.
- The v1 provider baseline is SQLite-first. `DataVaultProviderCapabilityProfiles.Sqlite` is the only built-in capability profile in current code, and Postgres remains an opt-in readiness hook rather than a full parity requirement.
- The default write boundary is the explicit `IDataVaultSaveService` and `DataVaultSaveRequest` path documented in `docs/architecture/dvault-v1-explicit-save-service.md`; `SaveChanges` interception is not part of this epic's MVP scope.
- This epic should consume the shared standards, MVP Data Vault concept document, default naming policy, stable hashing contract, and v1 persistence convention policy already referenced by repository planning docs instead of reopening those baselines.

Scope In
- Entity Framework model integration through `UseDataVault()` and `ApplyDataVaultMetadata()` and translation of provider-neutral hub, link, and satellite metadata into EF model metadata.
- Provider-neutral EF annotations, deterministic table and column naming, keys, indexes, and SQLite v1 type mappings required for the first persistence MVP.
- Explicit persistence orchestration for hub, link, and satellite rows through `IDataVaultSaveService`, including load timestamp, record source, hash-key, and hash-diff handling consistent with current contracts.
- SQLite integration-test proof for hub, link, and satellite persistence behavior, plus non-default Postgres test readiness hooks consistent with the README baseline.

Scope Out
- Full Postgres provider implementation, provider-specific optimization, and cross-provider parity guarantees.
- `SaveChanges` interception, hidden automatic persistence, or other alternative default write entry points.
- PIT tables, bridge tables, multi-active satellites, and other deferred Data Vault capabilities listed in `docs/plans/deferred-data-vault-capabilities.md`.
- Migration tooling, schema-generation automation promises beyond the EF model surface, and unrelated CI or workflow expansion.

Open questions
- none

Follow-up questions
- After the SQLite-first MVP closes, should a separate provider epic formalize a first-class Postgres capability profile and broader provider-specific integration coverage?
- Should a later convenience ticket evaluate optional wrappers or interceptors on top of the explicit save service without changing the v1 default write boundary?
- Which deferred Data Vault expansion, if any, should be scheduled first after MVP: PIT tables, bridge tables, multi-active satellites, or provider optimizations?

Risks
- The biggest scope-creep risk is treating Postgres readiness hooks as a requirement for full provider support instead of the narrower opt-in hook already documented in README.
- The epic touches naming, hashing, EF metadata projection, and persistence semantics at once; if downstream work reopens those shared contracts instead of consuming them, epic closure will drift.
- Satellite history behavior depends on the current hash-diff and latest-load-timestamp interpretation, so weak integration coverage here could allow silent regressions across child tickets.

Split recommendations
- No additional split is recommended at the epic level right now; the existing `parentOf` children `06EXB7FF1J9NR2849WKDR8DKPG`, `06EXB7G6YE4X0GA0CT7EPEFMPR`, `06EXB7GYQKBZ8FMQN6YDYCKATG`, and `06EXB7HYG17X73GH0K535GYJH8` are the bounded delivery path for closing this epic.
- If a child ticket starts absorbing full Postgres support or deferred Data Vault capabilities, split that work into a separate follow-up ticket or epic instead of expanding this MVP epic.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment