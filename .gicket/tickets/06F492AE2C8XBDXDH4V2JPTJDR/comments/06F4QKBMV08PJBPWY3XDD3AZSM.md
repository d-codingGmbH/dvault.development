[gicket-bot] PO refinement contract

Summary
- Verified the ticket, comments, relations, and repo-local package/API boundaries. The authoritative contract now fixes the snapshot input to a consumer-materialized IReadOnlyModel, keeps src/DCoding.Data.DVault design-package-free, and is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The additive preflight takes one authoritative snapshot input boundary: a consumer-materialized IReadOnlyModel. EF ModelSnapshot types or generated snapshot-derived classes stay outside the src/DCoding.Data.DVault public API and remain consumer-owned conversion details.
- critic-item-2: `answered` - Repo-local evidence supports the chosen boundary without adding Microsoft.EntityFrameworkCore.Design. The core package already depends on Microsoft.EntityFrameworkCore and Microsoft.EntityFrameworkCore.Relational, and the design-time workflow document explicitly keeps EF design tooling in the consumer project. If a consumer owns an EF ModelSnapshot, converting it into the required IReadOnlyModel remains consumer-owned code outside src/DCoding.Data.DVault.
- critic-item-3: `answered` - The feasibility boundary is now grounded on existing repo-local APIs: src/DCoding.Data.DVault already publishes IReadOnlyModel-based comparison overloads, so the new story can compose metadata-versus-runtime and metadata-versus-snapshot-model comparisons without taking a direct dependency on EF ModelSnapshot or Microsoft.EntityFrameworkCore.Design. The runtime lane is additive over DbContext.Model, while existing DbContext drift APIs keep their design-time semantics.
- critic-item-4: `answered` - The contract no longer mixes snapshot-input shapes. Scope In, Acceptance Criteria, Definition of Done, and Implementation Notes consistently refer to a consumer-materialized snapshot-model IReadOnlyModel, while existing Compare(..., DbContext) behavior remains a separate design-time path and is explicitly not redefined.

Clarifications
- gicket-read-ticket-comments returned only bot workflow artifacts; recent human comments remain none, so the ticket body and referenced repository documents are the authoritative scope inputs.
- gicket-read-ticket-relations confirms that this ticket remains a child of epic 06F492A3MPSGP3KXDNZECN01QM and still blocks 06F492BG6BZYYFMBE5WK7CB024 and 06F492BNDPWS9P4EDSV0W7G6VM; no relation cleanup or relinking was needed.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj stays design-package-free with Microsoft.EntityFrameworkCore and Microsoft.EntityFrameworkCore.Relational only, matching docs/architecture/dvault-dotnet-ef-design-time-workflow.md.
- No checked-in *ModelSnapshot.cs files are present in the repository outside build output, so DVault must not assume a repo-owned migrations snapshot file, fixed path, or automatic discovery boundary.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs uses compiledContext.Model and preserves DVault annotations, which is sufficient repo-local evidence for the additive runtime-model lane.

Scope In
- An additive library-owned preflight/report API that evaluates drift across DVault metadata, DbContext.Model, and an explicit consumer-materialized snapshot IReadOnlyModel.
- Support for both existing expected-model authorities: DataVaultMetadataModel and successful DataVaultModelImportResult.
- Use within the documented single-project consumer boundary where the same project owns the configured DbContext, migrations, design-time factory, and any consumer code that materializes snapshot-model input.
- Deterministic structured output with overall blocking status plus per-comparison detail suitable for CI tests or app-startup checks without a live database connection.
- Additive unit and integration coverage for matching and drifted runtime and snapshot-model lanes without requiring checked-in migration snapshot files in this repository.

Scope Out
- A DVault-owned dotnet ef shim, IDesignTimeServices, automatic migration execution, or automatic drift repair.
- Any core-package requirement to reference Microsoft.EntityFrameworkCore.Design, accept EF ModelSnapshot as a public input type, instantiate snapshot classes, or discover migrations automatically.
- A new top-level preflight command aggregator or orchestration UX; that remains with 06F492BG6BZYYFMBE5WK7CB024.
- Live-schema drift as a default gate, provider-wide online checks, or database connectivity requirements.
- Repo scanning, fixed snapshot file paths, or DVault-owned migration discovery heuristics.
- Standalone release-note and broad documentation rollout work; that remains with 06F492BNDPWS9P4EDSV0W7G6VM.

Open questions
- none

Follow-up questions
- When 06F492BG6BZYYFMBE5WK7CB024 is implemented, should snapshot-model drift surface through the existing design-time command host, a new facade, or both?
- Should the later documentation task recommend snapshot-model preflight only for applications that ship migrations, or as a broader optional startup check whenever an authoritative snapshot-model input is available?
- Should support-bundle export eventually record a snapshot-preflight section once this reusable result surface exists?

Risks
- False positives are possible if runtime model, snapshot-model, and metadata are not materialized under the same provider/profile or if consumer model-cache behavior is wrong; this story should surface that drift, while cache-key hardening remains with 06F492AKGMKPCRJYF4Z1EC9WY4.
- Reintroducing direct EF ModelSnapshot or design-package coupling into src/DCoding.Data.DVault would violate the documented package boundary and recreate the feasibility problem raised in PO-critic.
- Redefining existing artifact or design-time drift APIs instead of adding a new composite preflight surface would create compatibility risk for current tests, docs, and the blocked aggregator story.

Split recommendations
- No additional split is recommended; command aggregation and documentation are already separated into blocked follow-on tickets, so this story stays bounded to reusable runtime and consumer-materialized snapshot-model drift APIs and tests.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment