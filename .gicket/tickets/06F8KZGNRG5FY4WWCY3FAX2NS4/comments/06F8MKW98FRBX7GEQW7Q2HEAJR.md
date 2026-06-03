[gicket-bot] PO-critic review contract

Summary
- Ticket 06F8KZGNRG5FY4WWCY3FAX2NS4 is ready for developer handoff: the persisted delivery contract is resolved, the repository already contains the lifecycle contract and safe-baseline evidence it depends on, and the remaining gap is implementation rather than PO refinement.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted delivery contract sets `decision: ready_for_po_critic` and `## Open Questions` to `- none` in `.gicket/tickets/06F8KZGNRG5FY4WWCY3FAX2NS4/description.md:7-9,48-49`.
- Current analyzer code exposes only DMV1910 and DMV1911 in `src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:45-49` and `src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:8-26`, matching the ticket clarification that DMV1912-DMV1914 are still to be implemented.
- Current analyzer tests also assert the supported IDs are exactly `["DMV1910", "DMV1911"]` in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:13-18`.
- Repository docs already define the lifecycle analyzer contract and safe/non-safe boundaries for DMV1912-DMV1914 in `docs/architecture/dvault-ef-compiled-compatibility.md:83-97`.
- Repository guidance and tests already prove the built-in non-diagnostic `UseDataVaultMetadata(...)` baseline and the caller-owned `IModelCacheKeyFactory` responsibility in `README.md:555-608` and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:14-149`.
- Repository integration tests already prove the fixed-shape compiled-model and read-only compiled-query baseline in `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs:25-129`.
- The upstream contract ticket `06F8KZGC4NY41PRYB2RP00ZA1M` is `done` in `.gicket/tickets/06F8KZGC4NY41PRYB2RP00ZA1M/ticket.json:7`, and it still blocks this implementation story via `.gicket/relations/1M/S4/06F8KZGC4NY41PRYB2RP00ZA1M--06F8KZGNRG5FY4WWCY3FAX2NS4--blocks.json:3-5`.
- Branch inspection is consistent with a pre-development PO gate: `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06F8KZGNRG5FY4WWCY3FAX2NS4/**`, and `git log --oneline -n 5 -- .gicket/tickets/06F8KZGNRG5FY4WWCY3FAX2NS4` shows ticket handoff/lease commits ending at `fa5b64be0` and `ac8304173` rather than analyzer source work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete non-diagnostic example for DMV1912 where caller-owned variable model shape is visible and the custom `IModelCacheKeyFactory` visibly includes the discriminator would reduce interpretation drift for implementers and fixture authors.
- A concrete pair for DMV1913 that contrasts safe fixed-shape `UseModel(runtimeModel)` with unsafe variable-shape `UseModel(...)` would make the positive/non-diagnostic boundary easier to fixture consistently.
- A concrete pair for DMV1914 that contrasts safe fixed-shape `AddDbContextPool<TContext>(...)` with unsafe variable-shape pooling would help keep pooling coverage aligned with the documented boundary.

Risky assumptions
- Assuming the analyzer should expand helper methods, infer DI graphs, or reason across assemblies would contradict the direct-source-only boundary in the ticket and docs.
- Assuming any `UseModel(...)` call is unsafe would conflict with the existing compiled-compatibility proof and documented fixed-shape safe lane.
- Assuming pooling coverage should already include `AddPooledDbContextFactory<TContext>` or other entrypoints would exceed the current ticket scope; the ticket leaves that explicitly as follow-up work.

AC / test suggestions
- Keep the ticket-local test scope to targeted analyzer unit coverage for each new rule, as stated in `.gicket/tickets/06F8KZGNRG5FY4WWCY3FAX2NS4/description.md:34-45`, and leave the broader matrix to sibling ticket `06F8KZGZND5ZCH147PVBRWXYN4`.
- For DMV1912, include at least one positive case and one safe custom-cache-key case where the varying members are directly visible in the returned key shape.
- For DMV1913, include a non-diagnostic fixed-shape compiled-model lane that mirrors `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs:25-129` plus one diagnostic variable-shape lane.
- For DMV1914, include a non-diagnostic fixed options-only pooling lane and a diagnostic variable-shape pooling lane so the rule does not collapse into flagging pooling by default.

Implementation watchouts
- Do not regress existing DMV1910/DMV1911 behavior or their current non-diagnostic lanes in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:116-160`.
- Keep the analyzer on direct syntax/semantic evidence only; opaque `IModelCacheKeyFactory` logic, helper-expanded registrations, and runtime-only state must stay skipped rather than guessed.
- Do not turn this story into runtime work, SaveChanges guard work, provider-specific validation, or documentation work; those are outside the ticket contract.
- Preserve the safe built-in `UseDataVaultMetadata(...)` isolation baseline and the documented fixed-shape compiled/pooling baselines when adding new descriptors.

Non-blocking notes
- The branch currently contains ticket metadata changes only; that is acceptable at this pre-development gate because the contract is what is being reviewed, not landed implementation.
- The current ticket split is coherent: contract is done, this story is implementation, and fixture/docs follow-up remains separated in sibling tickets.

Split recommendations
- No further split recommended. The existing separation across contract `06F8KZGC4NY41PRYB2RP00ZA1M`, implementation `06F8KZGNRG5FY4WWCY3FAX2NS4`, fixtures `06F8KZGZND5ZCH147PVBRWXYN4`, and docs `06F8KZHAB717MJJNAWWK7S0A5W` is already appropriate for developer handoff.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment