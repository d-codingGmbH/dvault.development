[gicket-bot] PO refinement contract

Summary
- Verified the epic already has three done child stories for automated test strategy, public API quality, and manual NuGet release gating; no new child tickets, relations, attachments, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Relation state is already coherent: this epic has outgoing parentOf links to 06EXB807MN08HABHTHVPKKNFMG, 06EXB80ZNQTTGT6VN2DKEDGB0M, and 06EXB8202A88KJJP7WEGBESBYM, plus an incoming relates link from charter epic 06EXB4MDREV2T51VJNJEP6R0WR.
- All three child stories are already done: 06EXB807MN08HABHTHVPKKNFMG covers automated test strategy, 06EXB80ZNQTTGT6VN2DKEDGB0M covers public API quality, and 06EXB8202A88KJJP7WEGBESBYM covers the manual NuGet release gate.
- Recent comments contain only bot claim and lease metadata and do not add product-scope requirements.
- Repository evidence fixes the v1 release surface to six packable packages: DCoding.Data.DVault, DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer; src/DCoding.Data is explicitly non-packable and out of publication scope.
- No new child tickets, relation writes, attachments, or planning documents were needed in this PO pass because the existing split and repository documents already bound the epic.

Scope In
- The parent epic coordinates the existing three-track delivery split for automated test coverage, public API quality gates, and the manual NuGet release gate for the DVault package family.
- Public API quality scope includes XML documentation enforcement, package-specific API snapshots, and one-member-per-file enforcement for the six packable packages.
- Test-scope planning includes required local SQLite-backed coverage, default provider smoke coverage, and explicitly opt-in external provider integration coverage rather than treating all providers as mandatory in the default run.
- Release-governance scope includes the coordinated six-package publication rule, aligned version and dependency validation, auditable release-note evidence, and manual approval before the first package push.

Scope Out
- Publishing packages automatically, adding release credentials, or introducing CI-driven push automation in this epic.
- Treating src/DCoding.Data, tests, benchmarks, or helper tooling as NuGet publication artifacts.
- Provider-specific runtime behavior changes, save-strategy redesign, or broader post-MVP Data Vault capability work outside the documented quality and release gates.
- Pre-publication docs that present live dotnet add package commands or versioned NuGet examples as current guidance.

Open questions
- none

Follow-up questions
- After the first public release, should DVault add a separate story for NuGet-first installation guidance and versioned package examples that replace the current source-reference baseline?
- Should a later release-automation story wrap the validated manual gate in CI while preserving the explicit human approval step before any package push?
- Do SQL Server, Oracle, and MySQL need their own opt-in external integration harness tickets later, or should they remain limited to smoke coverage until provider priorities change?
- After public publication exists, should DVault add a second compatibility gate against the last published NuGet versions in addition to the repository-managed API baselines?

Risks
- Because release publication remains a coordinated manual process across six packages, any partial push or skipped verification step can create version or dependency drift if the documented gate is not followed exactly.
- If the default-versus-opt-in test boundary erodes, contributors may accidentally make external services a hidden prerequisite for normal validation.
- Documentation drift between README.md and docs/manual-nuget-publication.md could confuse maintainers about whether source-based or NuGet-based consumption is currently supported.

Split recommendations
- No additional split is needed; the epic is already bounded by the three existing child stories 06EXB807MN08HABHTHVPKKNFMG, 06EXB80ZNQTTGT6VN2DKEDGB0M, and 06EXB8202A88KJJP7WEGBESBYM.
- If CI-driven publication, credential handling, or public post-publication documentation is needed later, schedule those as separate follow-on stories rather than widening this epic.

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