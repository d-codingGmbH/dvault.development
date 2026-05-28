[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded typed read-model analyzer/generator guidance slice: authoritative support-bundle inputs, fingerprint-drift diagnostics for stale generated helpers, bounded unsupported-shape outcomes, existing generated-table misuse rules, and deterministic-only fixer policy.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For this story, 'stale generated code' means configured typed-read metadata fingerprint drift against the authoritative projected metadata source, not diffing checked-in generated files.
- The authoritative input for typed read generation is exactly one projected `dvault.support-bundle.v1` additional file with `diagnostics.explain`; raw Code-First callbacks, raw `dvault.model.v1` files, and unprojected metadata objects are not direct generator inputs.
- The bounded v1 diagnostic family is the reserved `DMV1960`-`DMV1969` typed read range plus the existing `DMV1910`-`DMV1911` generated-table misuse warnings where typed read adoption would otherwise encourage unsafe direct table access.
- Code fixes are only in scope when the remediation is deterministic and local; diagnostics whose remedy depends on runtime-produced metadata, API redesign, or ambiguous project intent remain diagnostic-only.
- `DCoding.Data.DVault.Analyzers` remains optional developer tooling and must continue to be documented with `PrivateAssets="all"` guidance and explicit `DVaultGenerateTypedReadModels=true` opt-in.

Scope In
- Typed read-model analyzer/generator diagnostics for authoritative metadata-source resolution, fingerprint drift, unsupported metadata shapes, deterministic name collisions, nullability fallback, dynamic-query-required cases, model-first out-of-contract cases, and helper-skipped outcomes within the reserved `DMV1960`-`DMV1969` range.
- Bounded detection of source-visible DVault generated shared-type table misuse through existing `DMV1910` and `DMV1911` patterns when code exposes or mutates generated tables directly.
- Documentation for optional analyzer-package installation, `PrivateAssets="all"`, typed-read opt-in configuration, and the supported/non-supported typed read generator boundary.
- Safe code fixes only for cases where one deterministic local edit can be proven without changing runtime architecture or guessing developer intent.
- Unit and analyzer/generator coverage for true positives, false-positive guards, and no-source-emission behavior on blocking typed read diagnostics.

Scope Out
- A new runtime read engine, provider-specific SQL generator, migration/runtime maintenance automation, or any change to the `IDataVaultReadService` execution boundary.
- Direct parsing of raw Code-First lambdas, raw `dvault.model.v1` additional files, or arbitrary metadata objects as generator inputs instead of the projected support-bundle explain descriptor.
- Whole-application DI inference, global dataflow analysis, provider-specific SQL validation, or complete model validation beyond the bounded analyzer rules.
- Auto-rewriting applications from generated shared-type table usage to `IDataVaultReadService` or `IDataVaultSaveService` when that would require non-local design changes.
- Making the analyzer package a runtime dependency or a transitive default for downstream projects.

Open questions
- none

Follow-up questions
- Should PIT/bridge generated helper parity ship in the same release as the full `DMV1963`/`DMV1964`/`DMV1967`/`DMV1969` surface, or stay staged after the satellite slice remains stable?
- Should long-term documentation keep supporting the legacy `DVaultReadModelMetadataSourceFingerprint` property, or should a later compatibility ticket deprecate it explicitly?

Risks
- If the analyzer tries to infer authoritative metadata from anything broader than the projected support-bundle explain descriptor, false positives and contract drift are likely.
- Unsafe or low-confidence code fixes could mis-edit project files or conceal required design changes; most typed-read and generated-table misuse diagnostics should remain diagnostic-only unless one exact edit target is provable.
- Consumer docs can overpromise PIT/bridge/helper coverage unless the root README, analyzer README, and typed read contract language stay synchronized.

Split recommendations
- Keep the satellite typed read analyzer/generator slice aligned with existing downstream ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Keep PIT/bridge-specific `DMV1963`/`DMV1964`/`DMV1967`/`DMV1969` behavior and tests aligned with existing downstream ticket `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Do not widen this story into broader EF Core dataflow or runtime write-boundary enforcement beyond the existing source-visible `DMV1910`/`DMV1911` patterns; treat that as a separate future ticket if needed.

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