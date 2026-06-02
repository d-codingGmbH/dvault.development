[gicket-bot] PO-critic review contract

Summary
- Contract is evidence-backed and ready for developer handoff; the persisted delivery contract is closed, the repo still reserves DMV1912-DMV1914, and the cited baselines match the proposed high-confidence scope.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted delivery contract has no unresolved gate items: `.gicket/tickets/06F8KZGC4NY41PRYB2RP00ZA1M/description.md` contains `## Open Questions` followed by `- none`.
- Local ticket comment history is automation-only and shows no blocking PO discussion: `.gicket/tickets/06F8KZGC4NY41PRYB2RP00ZA1M/comments` contains 10 comment files; `rg` over them shows handoff/lease messages plus `comments/06F8M7KGYPYQNBBPJZQHN856V8.md` reporting `blocking diagnostics: 0` and `comments/06F8M7HY547DBWFVH1WMF18P9M.md` stating the ticket is ready for handoff to `po-critic`.
- Branch-history evidence is clean: `git log --oneline --decorate --max-count=5 ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract` shows HEAD at `0ce2436f5` on the PO-critic claim commit, and `git diff --name-only 0ce2436f57931acda6026b56de993fbd66dae7ad..ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract` returned no files.
- The current analyzer catalog still stops at DMV1911: `src/DCoding.Data.DVault.Analyzers/README.md` lists `DMV1910` and `DMV1911` only, `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:17` asserts `SupportedDiagnostics` equals [`DMV1910`, `DMV1911`], and a repo-wide `rg -n "DMV1910|DMV1911|DMV1912|DMV1913|DMV1914" /mnt/c/Projects/DVault` produced hits for `DMV1910`/`DMV1911` only with no `DMV1912`-`DMV1914` occurrences.
- The referenced safe baselines are directly present in repo docs: `README.md:555-557` documents built-in `UseDataVaultMetadata(...)` model-cache isolation and caller-owned `IModelCacheKeyFactory` responsibility; `README.md:918` and `README.md:1004` say `UseModel(...)` and `AddDbContextPool<TContext>(...)` are guidance-only for one fixed realized model shape; `docs/releases/v0.24.0.md:68-72,173` repeats the same boundary.
- The cited proof fixtures exist and match the contract: `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs` covers registry/import-result model-cache participation plus a visible custom `IModelCacheKeyFactory` discriminator lane, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs` covers `UseModel(runtimeModel)` compiled-model compatibility, preserved DVault annotations, and read-only compiled-query access.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete AC example for the positive custom-cache-key lane could make implementation/test intent even tighter, e.g. a visible `ReplaceService<IModelCacheKeyFactory, ...>()` path that returns a key tuple including the varying member used by `OnModelCreating`.
- A concrete negative example for variable-shape pooling could help fixtures converge faster, e.g. `AddDbContextPool<TContext>(...)` against a context whose metadata source or table-shape discriminator varies by instance state.

Risky assumptions
- The contract assumes diagnostics stay high-confidence and skip opaque or indirect cache-key computations; if implementation starts inferring through helpers or cross-assembly abstractions, false positives will likely follow.
- The contract assumes `UseModel(...)` is only diagnosable when the same visible source scope proves variable model shape; the documented fixed-model compiled-compatibility lane must remain non-diagnostic.
- The pooling rule is intentionally bounded to `AddDbContextPool<TContext>(...)`; any extension to `AddPooledDbContextFactory<TContext>` or other entrypoints would need a separate ticket-level decision.

AC / test suggestions
- Keep one explicit non-diagnostic fixture tied to `DataVaultCompiledCompatibilitySqliteTests`: `UseModel(runtimeModel)` plus compiled query over `context.Set<Dictionary<string, object>>("HubOrder")` should stay clean when the model shape is fixed.
- Keep one explicit non-diagnostic fixture tied to `DataVaultMetadataRegistrationIntegrationTests`: registry-backed `UseDataVaultMetadata()` and a visible custom `IModelCacheKeyFactory` that includes the varying discriminator should not report.
- Add one diagnostic fixture each for DMV1912, DMV1913, and DMV1914 where the variation is source-visible and the matching cache-key or fixed-shape proof is absent.

Implementation watchouts
- Do not change DMV1910/DMV1911 semantics; this ticket is an additive contract and id-allocation authority only.
- Do not treat arbitrary `UseModel(...)` or `AddDbContextPool<TContext>(...)` occurrences as violations; the direct fixed-shape safe lane documented in `docs/architecture/dvault-ef-compiled-compatibility.md` must remain non-diagnostic.
- Do not guess that an opaque `IModelCacheKeyFactory` is incomplete; only direct source-visible key-shape evidence should satisfy or trigger the rule.

Non-blocking notes
- Prompt snapshot said recent comments were `<none>`, but persisted local ticket storage currently contains bot-generated handoff/lease comments only; I found no unresolved human PO discussion in them.

Split recommendations
- No further split is recommended; the persisted contract already separates contract 06F8KZGC4NY41PRYB2RP00ZA1M from implementation 06F8KZGNRG5FY4WWCY3FAX2NS4, fixtures 06F8KZGZND5ZCH147PVBRWXYN4, and docs 06F8KZHAB717MJJNAWWK7S0A5W.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment