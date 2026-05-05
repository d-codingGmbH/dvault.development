[gicket-bot] PO refinement contract

Summary
- Refined the ticket to use the existing per-package API snapshot approval test as the guardrail for future deferred capability public contracts, while explicitly allowing internal-only implementations to leave snapshots unchanged.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already has the public API approval mechanism for this work: `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` plus one approved snapshot file per packable package under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/`.
- `docs/plans/deferred-data-vault-capabilities.md` is the architecture guardrail for this ticket: do not infer or add placeholder public APIs for PIT tables, bridge tables, multi-active satellites, or advanced hooks.
- Current visible source keeps the v1 default boundary at optionless `AddDVault()`, convention-first `UseDataVault()` and `ApplyDataVaultMetadata()`, SQLite-default capability selection, and the explicit `IDataVaultSaveService` write boundary; no concrete PIT, bridge, multi-active, or hook runtime API is yet visible in source.
- Guardrails apply to any affected packable package, not just the core package, and snapshot updates should stay package-specific so a deferred-capability API change is isolated to the package that exports it.
- If a deferred capability implementation stays internal, the expected outcome is no new public snapshot baseline for that surface, plus an explicit note that the contract remains internal instead of silently widening the public API.

Scope In
- Refine or extend the existing API snapshot approval test and approved snapshot files so newly exported deferred-capability contracts are covered in the affected packable package.
- Preserve or improve failure clarity so snapshot diffs identify the changed package and exported type or member signature when deferred-capability API drift occurs.
- Document the internal-only outcome when a deferred capability implementation does not create a public contract, so reviewers know why approved snapshots did or did not change.
- Keep the work compatible with the existing repository quality gate driven by `dotnet test DVault.slnx --nologo` and the documented snapshot regeneration workflow.

Scope Out
- Designing or implementing PIT, bridge, multi-active, or advanced-hook runtime behavior beyond the guardrail work needed for public API review.
- Inventing concrete deferred-capability API names or placeholder public types solely to satisfy snapshot coverage.
- Changing the v1 default setup boundary, including optionless `AddDVault()`, convention-first model configuration, SQLite-default behavior, or the explicit save-service contract.
- Adding a second approval framework or replacing the existing `ApiSurfaceSnapshotTests` mechanism.
- Provider-specific feature matrices, DDL, or optimization commitments that belong to the separate PIT, bridge, multi-active, hook, or provider tickets.

Open questions
- none

Follow-up questions
- When the first concrete hook or other deferred-capability public surface lands, should it be marked as stable immediately or treated as an experimental public API in its first release pass?
- After the first deferred-capability public contract is introduced, is the current plain-text snapshot format still sufficient for reviewer ergonomics, or should later work add richer grouping or commentary for experimental surfaces?
- Which deferred-capability family is expected to introduce the first public contract so the guardrail can be proven against a concrete example in a follow-on ticket?

Risks
- Because the governing decision record deliberately avoids concrete deferred-capability API names, an implementation may overfit the snapshot guardrail to speculative shapes instead of guarding only real exported contracts.
- Future tickets may move a capability boundary between internal and public more than once, which can create noisy snapshot churn unless the public or internal decision is stated explicitly in each change.
- Shared namespace usage across packable packages can confuse review if snapshot diffs or documentation stop making the affected package obvious.

Split recommendations
- No additional split is recommended. PIT, bridge, multi-active, and hook implementation already lives in separate owning stories, so this task should stay focused on the shared API snapshot guardrail.

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