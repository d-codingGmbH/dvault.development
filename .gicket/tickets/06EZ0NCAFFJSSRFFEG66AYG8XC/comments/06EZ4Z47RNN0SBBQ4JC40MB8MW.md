[gicket-bot] PO refinement contract

Summary
- Refined the story to require one stable benchmark reporting surface that compares the provider-neutral fallback path with provider-specific strategies, keeps SQLite-only local runs useful, and records unavailable external providers explicitly.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For this story, the comparison baseline is the existing provider-neutral fallback save path versus any compatible provider-specific save strategy selected through the current strategy-dispatch architecture.
- SQLite is the required v1 benchmark baseline because it is the visible built-in provider capability profile and current proof baseline in repository planning documents.
- Configured external providers means providers from the existing package family that are actually enabled and reachable in the benchmark run environment; unavailable providers must be reported as skipped rather than omitted.
- This ticket is about benchmark reporting and run guidance, not about introducing new provider optimizations or changing the save-service public contract.

Scope In
- Consolidate benchmark output so each reported scenario shows provider, strategy, dataset size, change ratio, and the fallback-versus-optimized comparison context in one stable artifact.
- Support direct comparison for SQLite and any configured external providers using the current explicit save-service and provider-strategy architecture.
- Keep local benchmark execution useful when only the SQLite baseline is available.
- Emit explicit skipped benchmark entries, including provider identity and skip reason, when an external provider is not configured, not reachable, or otherwise unavailable for the run.
- Document how to run provider-specific benchmarks, what counts as a configured provider, and how to interpret skipped rows and comparison results.

Scope Out
- Implementing new provider-specific save optimizations, SQL capabilities, or concurrency behaviors.
- Changing the public `IDataVaultSaveService`, `IDataVaultProviderSaveStrategy`, or provider capability profile contracts.
- Requiring every external provider package in the repository family to be installed or runnable in a normal local developer environment.
- Building CI orchestration, environment provisioning, or release-pipeline automation for all external database engines unless that work is trivial within this ticket.
- Adding broader performance dashboards or long-term benchmark history tooling beyond the consolidated report needed for this story.

Open questions
- none

Follow-up questions
- Should a later ticket standardize one canonical serialized artifact format for published release evidence, such as a specific CSV or JSON contract, if external consumers will ingest benchmark results automatically?
- Should a later infrastructure ticket provision any external providers in CI so release evidence includes more than the SQLite baseline by default?

Risks
- If skip reasons are not normalized, benchmark artifacts may still be hard to compare across machines because unavailable-provider cases will look inconsistent.
- Absolute timings across different database engines can be noisy; the report must emphasize scenario metadata and comparison context so the evidence remains interpretable even when environments differ.
- If benchmark discovery of configured providers is ambiguous, developers may misread missing optimized rows as regressions rather than environment gaps, so documentation and output labeling need to be explicit.

Split recommendations
- If external database provisioning or CI matrix work grows beyond straightforward benchmark reporting, keep this ticket focused on the consolidated artifact plus local-run behavior and defer environment automation to a follow-up infrastructure ticket.
- If release publishing later needs machine-ingestable benchmark contracts or historical trend storage, separate that from this ticket once the stable single-run reporting surface exists.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment