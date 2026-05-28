[gicket-bot] PO refinement contract

Summary
- Refined this as a docs-only v0.22.0 baseline update for typed read-model generation and stable-hash governance, anchored on the current support-bundle-driven satellite-helper implementation, existing public API snapshot evidence, published hash vectors, and the repository validation command baseline.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The shipped typed read-model generator currently consumes exactly one authoritative `dvault.support-bundle.v1` additional file and emits satellite `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` helpers; it does not parse raw `dvault.model.v1` artifacts or source-visible Code-First declarations directly.
- Current generated-helper scope is satellite-only: hub-parent, link-parent, and deterministic multi-active satellites with string payload and driving-key members. PIT and bridge shapes remain documented runtime read-service or direct EF alternatives, not shipped generated helpers.
- Metadata-source fingerprint governance is already part of the generator contract and diagnostics surface and should be documented as compatibility governance for generated helpers, not as a new runtime behavior.
- Stable hash compatibility guidance should use the existing `sha256-v1` contract, canonical normalization rules, and published vectors as the baseline. Provider-specific save strategies must preserve those .NET-side semantics unless a separate future contract and evidence gate says otherwise.
- Public API snapshot evidence already exists under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` for `DCoding.Data.DVault` and the provider packages. Generator evidence currently lives in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`, not in separate committed generator approval snapshots.
- The v0.22.0 release note should remain a documentation-boundary record and keep the manual publication boundary explicit; it must not imply that package publication, hashes, or approval records are automatic or already completed.

Scope In
- Update `README.md` to describe opt-in generated satellite read helpers, the support-bundle input workflow, metadata-source fingerprint governance, stable-hash compatibility references, and the documented alternatives for non-generated read shapes.
- Update `src/DCoding.Data.DVault.Analyzers/README.md` so the analyzer package documentation matches the current typed read-model generator behavior, configuration, diagnostics, and non-goals.
- Update `docs/model-first-governance.md` to explain the model-first path for generated read helpers and to route hash-governed consumers to the stable-hashing compatibility contract and support-bundle workflow.
- Update `docs/production-adoption-checklist.md` with adopter guidance for generated read helpers, support-bundle production, stable-hash compatibility expectations, and evidence links.
- Add `docs/releases/v0.22.0.md` as the new coordinated documentation boundary for this topic and switch the targeted docs from v0.21.0 current-baseline wording to v0.22.0 where this ticket explicitly owns that roll-forward.
- Link the existing evidence surfaces for public API snapshots, generator output evidence, stable-hash compatibility vectors, compiled-query alternatives, and repository validation commands.

Scope Out
- No runtime, analyzer, or source-generator code changes.
- No new generated-helper shapes beyond the current satellite-only implementation.
- No PIT or bridge generated-helper documentation that would overstate the current shipped behavior.
- No new stable-hash algorithm, provider-side hashing path, migration framework, or compatibility format.
- No new standalone CLI, automatic support-bundle routing, or workflow automation claims.
- No requirement to add new generator approval-snapshot infrastructure; this ticket can use the existing generator test surface as the bounded evidence baseline.

Open questions
- none

Follow-up questions
- When PIT or bridge generated-helper implementation actually ships, should a later ticket widen the public docs and release-note evidence beyond the current satellite-only generator boundary?
- If the team wants dedicated committed generator approval snapshots instead of assertion-based generator tests, should that be handled as a separate quality/evidence ticket rather than folded into this docs-only release pass?

Risks
- The biggest documentation risk is overstating the generator boundary by implying PIT or bridge helpers, direct raw `dvault.model.v1` parsing, or runtime request compilation that the current implementation does not ship.
- Hash-governance wording can become misleading if metadata-source fingerprint drift and stable-hash compatibility are blended together; the docs should keep those as separate governance topics.
- Linking to non-existent generator snapshot artifacts or analyzer public API snapshot files would create false evidence claims, because the visible repo evidence uses generator tests and runtime/provider public API snapshots instead.
- If the targeted docs do not all move to the same v0.22.0 current-baseline wording, readers will get mixed release guidance between README, the checklist, model-first guidance, and release notes.

Split recommendations
- If the work expands into new quality infrastructure such as dedicated generator approval snapshots or analyzer API snapshot coverage, split that into a separate quality/evidence ticket.
- If the release also needs public docs for PIT or bridge generated helpers, split that into a later ticket tied to the actual shipped implementation rather than broadening this documentation ticket beyond current behavior.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment