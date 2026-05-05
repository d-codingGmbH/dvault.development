[gicket-bot] PO refinement contract

Summary
- Replaced unsupported inferred deferred-capability API assumptions with source-backed snapshot evidence and restated the ticket so future public deferred-capability contracts may be introduced explicitly by owning work and then guarded package-by-package.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Current branch evidence shows an existing per-package public API approval mechanism, but it does not show any existing PIT, bridge, multi-active, or hook-specific public type. The contract is therefore restated to rely on the visible snapshot gate and to allow owning capability work to introduce a new public deferred-capability type or member explicitly when needed; if no public type is introduced, snapshots stay unchanged and the change notes must say the surface remained internal.
- critic-item-2: `answered` - The persisted contract no longer infers an already-existing deferred-capability public API. The visible public baseline is the current core snapshot contents plus one provider registration extension surface per provider package; deferred-capability APIs remain future work and become part of this ticket only when an owning change intentionally exports them.
- critic-item-3: `answered` - The repository already contains the API snapshot approval test and approved snapshot files for each packable package. The contract is refined to use that existing mechanism for any real new deferred-capability public surface, preserving the per-package snapshot boundary and the existing package-header plus signature-line diff clarity instead of inventing placeholder APIs or a second approval path.

Clarifications
- Current branch evidence for the guardrail is the existing `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` approval test plus the six approved files under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/`.
- `DVault.slnx` and the snapshot directory establish the bounded packable-package baseline for this ticket: `DCoding.Data.DVault`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- The visible public baseline in `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` covers existing registration, modeling, provider-profile, metadata, and explicit `IDataVaultSaveService` contracts; the provider snapshot files currently expose provider registration extensions only.
- No PIT, bridge, multi-active-satellite, or advanced-hook-specific public type or member is visible in the supplied branch evidence, so this ticket must not assume one already exists.
- If an owning deferred-capability change intentionally exports a new public type or member in one of the six packable packages, update only that package's approved snapshot in the same change.
- If a deferred capability remains internal, leave approved snapshots unchanged and explicitly record that the implementation stayed internal so the unchanged public baseline is understood as intentional.
- The existing snapshot text already carries `# Package:` header lines and exported signature lines, so package-specific diffs are the current failure-review mechanism that must be preserved or improved, not replaced.

Scope In
- Use the existing `ApiSurfaceSnapshotTests` gate and matching approved snapshot file to guard any real new public deferred-capability type or member exported from a packable package.
- Keep the per-package snapshot boundary so a change in one package updates only that package's approved file when other package public surfaces remain untouched.
- Preserve or improve the current package-and-signature failure readability supplied by snapshot header lines and diffable exported member text.
- Require an explicit note when deferred-capability work remains internal and therefore makes no snapshot update.
- Keep the work aligned with the documented `dotnet test DVault.slnx --nologo` validation path and the `DVAULT_UPDATE_API_SNAPSHOTS=1` approval workflow.

Scope Out
- Designing PIT, bridge, multi-active-satellite, or advanced-hook runtime behavior beyond the public API guardrail work.
- Inventing placeholder public types, members, or names solely to exercise the snapshot gate.
- Replacing the existing `ApiSurfaceSnapshotTests` approval mechanism or adding a second approval framework.
- Changing the current default boundary built around optionless `AddDVault()`, convention-first model configuration, SQLite-default capability selection, or the explicit `IDataVaultSaveService` write boundary.
- Provider-specific feature matrices, DDL, rollout, or release-governance work that belongs to separate capability or release tickets.

Open questions
- none

Follow-up questions
- When the first concrete deferred-capability public contract lands, should release notes present it as stable immediately or call out first-pass consumer guidance?
- After the first such contract arrives, is the current plain-text snapshot format still sufficient for reviewer ergonomics, or should later work add richer grouping or commentary?
- Which owning deferred-capability story is expected to introduce the first public API so this guardrail can be exercised against a concrete change?

Risks
- An implementation may still try to create placeholder public APIs to prove the guardrail instead of waiting for a real exported contract.
- Shared namespace usage across packages can confuse review if change notes fail to name the affected package even though the snapshot files are package-specific.
- Moving a capability between internal and public across successive tickets can create noisy snapshot churn unless each change explicitly records the chosen boundary.

Split recommendations
- No split is recommended; this remains a shared guardrail task, while concrete deferred-capability API shape stays with the existing PIT, bridge, multi-active, or hook owning stories.

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