[gicket-bot] PO refinement contract

Summary
- Refined the epic contract to explicitly mark 06EXB7ZZDZH7ADQ12R2MGY60A0 as a tracking-only closure epic with no parent-owned implementation slice; verified the three child stories are done, and no new child tickets, relations, attachments, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Resolved the closure audit by defining this parent as a tracking-only closure epic: it closes when the three existing child stories remain done, the parentOf and relates topology remains intact, and the shared six-package validation and release documents still define the coordinated contract; the parent itself does not carry a separate implementation slice.
- critic-item-2: `answered` - The delivery contract is now explicitly ratified as tracking-only and closure-focused: the parent epic owns no parent-level repository implementation, code, test, or release-automation slice; any further work must be created as child or follow-on tickets instead of being assigned directly to the epic.

Clarifications
- This epic is a tracking-only closure wrapper. It owns no direct repository implementation slice and exists to coordinate and close the already-delivered child stories.
- The child-story implementation split is already complete and done: 06EXB807MN08HABHTHVPKKNFMG owns automated test strategy, 06EXB80ZNQTTGT6VN2DKEDGB0M owns public API quality, and 06EXB8202A88KJJP7WEGBESBYM owns the manual NuGet release gate.
- Relation topology is already coherent: this epic has outgoing parentOf links to the three child stories and an incoming relates link from charter epic 06EXB4MDREV2T51VJNJEP6R0WR.
- Repository evidence fixes the v1 coordinated release family to six packable packages: DCoding.Data.DVault, DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer; src/DCoding.Data is explicitly non-packable and outside publication scope.
- No new child tickets, relation writes, attachments, or planning documents were needed in this PO pass.

Scope In
- Tracking and closure of the existing three-story delivery split for automated test coverage, public API quality gates, and manual NuGet release governance.
- Verification that the six-package validation and publication baseline remains defined by README.md, docs/quality/api-surface-snapshots.md, docs/quality/one-member-per-file.md, and docs/manual-nuget-publication.md.
- Epic-level closeout rules: all three child stories done, relation topology intact, and no added parent-owned implementation scope.

Scope Out
- Any new repository implementation, tests, packaging behavior, release automation, or documentation work owned directly by this parent epic.
- Automatic publication, credentials, or CI-driven push automation in this epic.
- Treating src/DCoding.Data, tests, benchmarks, or helper tooling as NuGet publication artifacts.
- Provider-specific runtime behavior changes, save-strategy redesign, or broader post-MVP Data Vault feature work.

Open questions
- none

Follow-up questions
- After the first public release, should DVault add a separate story for NuGet-first installation guidance and versioned package examples that replace the current source-reference baseline?
- Should a later release-automation story wrap the validated manual gate in CI while preserving the explicit human approval step before any package push?
- Do SQL Server, Oracle, and MySQL need their own opt-in external integration harness tickets later, or should they remain limited to smoke coverage until provider priorities change?
- After public publication exists, should DVault add a second compatibility gate against the last published NuGet versions in addition to the repository-managed API baselines?

Risks
- If future implementation work is added directly to the parent epic instead of a child or follow-on ticket, the tracking-only closure contract will drift and closure audits will fail again.
- Because release publication remains a coordinated manual process across six packages, any partial push or skipped verification step can create version or dependency drift if the documented gate is not followed exactly.
- If the default-versus-opt-in test boundary erodes, contributors may accidentally make external services a hidden prerequisite for normal validation.
- Documentation drift between README.md and docs/manual-nuget-publication.md could confuse maintainers about whether source-based or NuGet-based consumption is currently supported.

Split recommendations
- No additional split is needed; the epic is already bounded by done child stories 06EXB807MN08HABHTHVPKKNFMG, 06EXB80ZNQTTGT6VN2DKEDGB0M, and 06EXB8202A88KJJP7WEGBESBYM.
- If CI-driven publication, credential handling, provider-specific live integration harnesses, post-publication installation guidance, or NuGet compatibility gates are needed later, schedule them as separate follow-on tickets because the parent epic owns no direct implementation slice.

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