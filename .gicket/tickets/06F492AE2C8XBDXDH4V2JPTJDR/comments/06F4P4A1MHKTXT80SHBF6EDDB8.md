[gicket-bot] PO refinement contract

Summary
- Verified from repository code, docs, and repository-local `.gicket` metadata that DVault currently supports artifact-versus-design-time drift while model-snapshot drift is still explicitly out of scope; this story is now refined as an additive library-local preflight/report surface for runtime-model and consumer-supplied `ModelSnapshot` comparison, with command aggregation and broad docs left to the already blocked follow-on tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Runtime `gicket-read-*` fallbacks were trust-blocked, so ticket, comment, and relation verification used repository-local `.gicket` files instead; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.
- The only stored ticket comment is the bot claim template; there are no human comments adding scope beyond the ticket body and referenced repository documents.
- This ticket remains a child of epic `06F492A3MPSGP3KXDNZECN01QM` and currently blocks `06F492BG6BZYYFMBE5WK7CB024` (consumer-owned preflight command aggregator) and `06F492BNDPWS9P4EDSV0W7G6VM` (v0.17 docs and release notes).
- Repository evidence shows `DataVaultModelDriftReporter` already compares metadata or import results against `IReadOnlyModel` and `DbContext`, while compiled-model coverage proves DVault annotations survive runtime-model initialization; the missing gap is a first-class preflight that also incorporates EF `ModelSnapshot` output.
- Current design-time documentation and tests still treat model snapshot drift comparison as unsupported, and `DataVaultDesignTimeCommand` only exposes artifact-based `drift --artifact`; this story should close the reusable library gap without taking ownership of command aggregation.
- The repository currently has no checked-in `*ModelSnapshot.cs` files, so the bounded v1 default is a consumer-supplied snapshot type or instance from the owning migrations project rather than DVault-owned repo scanning or migration discovery.

Scope In
- An additive library-owned preflight/report API that evaluates drift across DVault metadata, the EF runtime model, and a consumer-supplied EF `ModelSnapshot`.
- Support for both existing expected-model authorities: `DataVaultMetadataModel` and successful `DataVaultModelImportResult`.
- Use within the existing single-project consumer boundary where the same project owns the configured `DbContext`, migrations, design-time factory, and snapshot input.
- Deterministic structured output with overall blocking status plus per-comparison detail suitable for CI tests or app-startup checks without requiring a live database.
- Additive unit and integration coverage for matching and drifted runtime and snapshot lanes without requiring checked-in migration snapshot files in this repository.

Scope Out
- A DVault-owned `dotnet ef` shim, `IDesignTimeServices`, automatic migration execution, or automatic drift repair.
- A new top-level preflight command aggregator or orchestration UX; that remains with `06F492BG6BZYYFMBE5WK7CB024`.
- Live-schema drift as a default gate, provider-wide online checks, or database connectivity requirements.
- Model-cache isolation, runtime save interception, provider capability explainers, and query-shape diagnostics already scoped to sibling epic stories.
- Standalone release-note and broad documentation rollout work; that remains with `06F492BNDPWS9P4EDSV0W7G6VM`.

Open questions
- none

Follow-up questions
- When `06F492BG6BZYYFMBE5WK7CB024` is implemented, should snapshot drift surface through the existing design-time command host, a new facade, or both?
- Should the later documentation task recommend snapshot preflight only for applications that ship migrations, or as a broader optional startup check whenever a snapshot is present?
- Should support-bundle export eventually record a snapshot-preflight section once this reusable result surface exists?

Risks
- False positives are possible if runtime model, snapshot, and metadata are not all created with the same provider/profile or if consumer model-cache behavior is wrong; this story should detect that drift, while cache-key hardening remains with `06F492AKGMKPCRJYF4Z1EC9WY4`.
- Auto-discovery of snapshots or migrations would over-expand scope and reintroduce repo-layout coupling that the current consumer-owned design-time boundary explicitly avoids.
- Redefining existing artifact or design-time drift APIs instead of adding new ones would create unnecessary compatibility risk for current tests, docs, and the blocked aggregator story.

Split recommendations
- No additional split is recommended; command aggregation and documentation are already separated into blocked follow-on tickets, so this story should stay bounded to reusable runtime and snapshot drift APIs and tests.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment