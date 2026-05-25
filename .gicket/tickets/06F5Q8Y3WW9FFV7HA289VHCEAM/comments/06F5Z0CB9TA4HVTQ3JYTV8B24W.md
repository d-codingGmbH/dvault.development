[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the ticket has a specific docs-only contract, `## Open Questions` is `none`, prerequisite stories are already done, and the repository already exposes the chunked-save API and benchmark evidence the docs must reference.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q8Y3WW9FFV7HA289VHCEAM/description.md` contains a bounded docs-only delivery contract, explicit scope-in/out, and `## Open Questions` -> `none`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` defines `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...)` and the public `DataVaultChunkedSaveRequest` / `DataVaultSaveChunk` types at the current source tip.
- `benchmark-summary.md:42-44`, `benchmark-summary.csv:11-13`, and `benchmark-summary.json` contain `customer-profile-streaming-save` rows for materialized explicit bulk, `chunked-save-bounded-10`, and `chunked-save-bounded-5`, including `chunkCount`, `processedChunkCount`, and `retainedStateHighWater` in `executionDetail`.
- `README.md:10-16`, `README.md:686`, and `README.md:705` still present `0.18.0` as the public baseline; `docs/production-adoption-checklist.md:9` still points at `releases/v0.18.0.md`.
- `README.md:219` still documents `DataVaultBulkSaveRequest` as the multi-request path, and `rg -n 'chunked|streaming|DataVaultChunkedSaveRequest|DataVaultSaveChunk' README.md docs/production-adoption-checklist.md docs/architecture/dvault-v1-explicit-save-service.md` returned no matches, so the doc gap is concrete and well scoped.
- `git ls-files docs/releases/v0.19.0.md` returned no tracked file, and the repo-visible `docs/releases/` set currently runs through `docs/releases/v0.18.0.md` only.
- `git log --oneline develop..HEAD` shows only ticket-metadata commits (`401ef95ff`, `728b1f9cd`, `21bf5ed5a`), and `git diff --name-only develop..HEAD` lists only `.gicket/tickets/...` paths, so this is still a clean pre-development handoff rather than partially implemented docs work.
- `rg -n '^\[gicket-bot\]' .gicket/tickets/06F5Q8Y3WW9FFV7HA289VHCEAM/comments` matches every stored comment file, so there is no stored human clarification thread superseding the current contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the eventual docs should make the empty-chunk and empty-chunk-sequence no-op behavior visible enough that readers do not infer chunked requests must always contain work.
- Non-blocking: migration guidance should explicitly distinguish callers that already materialize one ordered request set from callers that need bounded chunking under the same caller-owned transaction and cancellation rules.

Risky assumptions
- Assuming release prose will stay anchored to the visible root benchmark triplet only; there is no checked-in streaming-specific before/after artifact bundle visible today.
- Assuming only the touched current-baseline references move to `v0.19.0`; repo-visible docs such as `docs/model-first-governance.md` and `docs/plans/fluent-code-first-api-contract.md` still reference `v0.18.0` as the current baseline and should remain follow-up cleanup unless intentionally pulled into scope.
- Assuming the documentation will cross-link the authoritative streaming contract instead of duplicating behavior text that can drift from `docs/architecture/dvault-v1-streaming-explicit-save-contract.md`.

AC / test suggestions
- Acceptance review should verify that `README.md`, `docs/production-adoption-checklist.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and new `docs/releases/v0.19.0.md` all tell the same `v0.19.0` story while leaving `docs/releases/v0.18.0.md` historical.
- Validation should reuse the repository-visible command baselines already cited by the ticket: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `dotnet pack DVault.slnx --configuration Release --nologo`, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`.
- Final doc review should compare claims against the actual streaming benchmark rows and their `executionDetail` fields so the release notes mention only visible evidence.

Implementation watchouts
- Do not imply provider-native chunk execution has shipped; the current streaming benchmark rows show `saveStrategyStatus=ProviderNeutralFallback` and `selectedStrategy=<none>`.
- Do not invent a registry-backed chunked request API; `rg -n 'DataVaultRegistryChunkedSaveRequest' src tests README.md docs` returns no matches in the current repository.
- README already has the ordered-bulk guidance in place, so the chunked-save migration guidance should extend that existing path rather than create a parallel, competing documentation flow.

Non-blocking notes
- The branch currently carries only ticket-state changes, which is consistent with a PO-quality gate on a pre-development docs task.

Split recommendations
- No split recommended for this ticket as written; keep broader `v0.18.0` current-baseline cleanup outside this handoff unless a separate follow-up ticket is created.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment