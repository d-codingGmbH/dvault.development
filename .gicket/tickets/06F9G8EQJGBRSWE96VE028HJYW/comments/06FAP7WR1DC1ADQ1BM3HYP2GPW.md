[gicket-bot] PO refinement contract

Summary
- Ratified the v0.33.0 compatibility contract as a dual package-line release: the current repository is still net10.0-only, all seven DVault package IDs stay unchanged, 8.33.0 maps to net8.0 and EF Core 8, 10.33.0 maps to net10.0 and EF Core 10, and the contract now fixes the provider-version matrix, analyzer handling, and allowed conditional reference boundaries for downstream implementation tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence is a net10.0-only baseline across the packable projects, analyzer project, test projects, and shared implementation standards; this ticket defines the additive v0.33.0 compatibility pivot rather than a backport of earlier 0.x releases.
- The coordinated family remains the existing seven package IDs: DCoding.Data.DVault, DCoding.Data.DVault.Analyzers, DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer; compatibility lines are expressed through package version major, not package ID splits.
- Upstream task 06F9GF2Z4Y7A91ZHG4NW1YTNMC is already done and settles the version-line policy: planning release v0.33.0 maps to package versions 8.33.0 for the net8.0 and EF Core 8 line and 10.33.0 for the net10.0 and EF Core 10 line, while v0.32.0 and earlier remain on the historical 0.x line.
- Each published compatibility line must resolve one dependency line only: a net8.0 target resolves the 8.x contract, a net10.0 target resolves the 10.x contract, and no build, pack output, or consumer example may mix 8.x and 10.x dependencies in the same resolved target.
- DCoding.Data.DVault.Analyzers remains optional build-time tooling in the coordinated family. Its analyzer and source-generator assets stay local to the declaring project through PrivateAssets=all and must not become a runtime or transitive exported dependency as part of the dual-line work.
- The live relation graph still shows incoming blocks edges from done tickets 06F8KZVRARQPG482YKCQ686PNM and 06F9GF2Z4Y7A91ZHG4NW1YTNMC; treat them as historical workflow residue, not active scope blockers for this ticket.

Scope In
- Define which existing DVault packages participate in the net8.0 and net10.0 compatibility lines and how their published package versions map to those lines.
- Define the required EF provider-version matrix for both targets, including the explicit SQLite, MySQL, PostgreSQL, Oracle, and SQL Server package versions named in the ticket draft.
- Define allowed conditional package-reference patterns for runtime, provider, and opt-in integration and test projects so each resolved target uses exactly one compatible dependency line.
- Define the analyzer and source-generator packaging boundary for the optional analyzer package under the new dual-line release model.
- Define the verification and documentation expectations that distinguish planning release numbers from consumer-facing NuGet package versions and distinguish required package evidence from opt-in external-provider execution.

Scope Out
- Editing project files, verifier code, tests, README, release notes, or CI guidance; those implementation changes belong to the already-split sibling tickets.
- Adding new runtime features, new provider behaviors, provider provisioning, container automation, or automatic publication and release automation.
- Republishing v0.32.0-or-earlier packages onto new 8.x or 10.x majors or inventing new line-specific package IDs.
- Expanding the required v0.33.0 acceptance matrix into a separate Pomelo package-version lane; the required package evidence in this ticket is the named MySql.EntityFrameworkCore 10.0.7 exception for both targets.

Open questions
- none

Follow-up questions
- Should a later ticket publish an explicit Pomelo.EntityFrameworkCore.MySql package-version compatibility matrix in addition to preserving the current runtime support-name baseline for Pomelo and MySql provider names?
- After the 8.x and 10.x lines are established, should a later policy ticket define how future 11.x or additional compatibility lines are introduced without another full contract rewrite?
- Should build and release automation later standardize a named line-selection property or artifact-directory convention for separate 8.x and 10.x pack runs, or leave that as build-implementation detail?

Risks
- The repository currently hard-codes a net10.0 baseline in project files, tests, verifier expectations, and shared standards text, so missed constants or docs can leave one compatibility line only partially updated.
- Keeping the same package IDs across the historical 0.x line and the new 8.x and 10.x lines is the least disruptive continuation of current package naming, but it increases the chance of consumer confusion if examples or release notes blur planning release numbers and package versions.
- MySql.EntityFrameworkCore 10.0.7 being the required package-evidence exception for both targets can be misread as an accidental mixed-line dependency unless tests and docs call it out explicitly.
- The current live relation graph still contains historical incoming blocks edges from done tickets, so workflow views may overstate active blockers until relation cleanup is replayed.

Split recommendations
- No additional split is recommended. The epic is already decomposed into the version-line policy task, this compatibility-contract story, the multitarget implementation story, the provider-matrix test story, the verifier and CI guidance task, and the v0.33 documentation task.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment