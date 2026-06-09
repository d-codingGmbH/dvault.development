[gicket-bot] PO refinement contract

Summary
- Refined the story around the already-visible dual-target repository baseline, keeping the work focused on deterministic provider-version and package-dependency-line proof while treating multitargeting as completed prerequisite scope and broader verifier/CI expansion as sibling follow-up work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already pins the v1 matrix in src/DCoding.Data.DVault/DCoding.Data.DVault.csproj and the Unit and Integration test projects, so this story should lock that visible matrix with deterministic tests instead of reopening provider or version selection.
- The supported-provider test baseline for this story is the finite five-provider set already documented in repository planning material: SQLite, PostgreSQL, Oracle, SQL Server, and MySQL.
- MySQL coverage for this story follows the checked-in MySql.EntityFrameworkCore 10.0.7 package on both target frameworks; Pomelo-specific proof is not part of this bounded v1 ticket.
- External-provider database execution remains opt-in behind the existing connection-string switches; default local validation must stay runnable without containers or live external databases.
- Broader package verifier metadata, symbols, README, XML docs, and CI/manual-guidance expansion remains with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8; this story only needs the EF/provider matrix and dependency-line proof.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass because the existing evidence already bounded the story.

Scope In
- Deterministic repository tests that assert the exact EF/provider package version matrix for net8.0 and net10.0 across the core and provider-support validation path.
- Deterministic package-artifact dependency checks proving packed outputs expose the intended 8.x or 10.x EF/provider dependency line for the corresponding target framework.
- Coverage that preserves the existing net8 helper exclusions and the existing external-provider opt-in gates while still validating the matrix.
- Clear failure diagnostics that identify the drifting provider, package version, and target framework when the matrix changes unexpectedly.

Scope Out
- Retargeting benchmarks, tools/DCoding.Data.DVault.PackageVerification, analyzers, or analyzer tests to net8.0.
- Broader package verification concerns such as README, symbols, XML docs, nuspec metadata, or CI/manual publication guidance already owned by 06F9G8FBQTAPXXS1Y4NR5QKVG8.
- New runtime provider behavior, new supported providers, or reopening the completed multitarget project-set decision from done ticket 06F9G8EXXFJJ1SWWQXC2N9P2X8.
- Mandatory live MySQL, PostgreSQL, Oracle, or SQL Server execution in the default local test lane.

Open questions
- none

Follow-up questions
- Should ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 later absorb these dependency-line assertions into the reusable package verifier and CI guidance once the focused matrix tests land?
- Should a later compatibility task add explicit Pomelo.EntityFrameworkCore.MySql matrix proof, since repository policy supports both MySQL provider names but the checked-in test matrix currently uses MySql.EntityFrameworkCore?
- After the matrix tests land, should release or adoption documentation call out the exact opt-in external-provider verification commands, or is the existing fixture guidance sufficient?

Risks
- Because several provider PackageReferences are conditioned on connection-string properties, matrix coverage that relies only on live external-provider execution could miss drift when those opt-in properties are absent; at least one deterministic non-live assertion path is required.
- Package-artifact dependency proof can overlap with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 if implementation expands beyond EF/provider dependency groups into broader nuspec, README, symbol, or metadata verification.
- Repository policy supports a broader MySQL provider-name baseline than this story's checked-in MySql.EntityFrameworkCore matrix, so Pomelo-specific drift would remain outside this ticket unless separately scheduled.

Split recommendations
- No additional split is required; the story is bounded if it stays focused on exact provider-version assertions and package dependency-line proof, while broader verifier and CI coverage remains with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment