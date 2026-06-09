[gicket-bot] PO refinement contract

Summary
- Verified directly from ticket and repository state that this epic's child split is already materialized and that the branch now carries the dual net8.0/net10.0 compatibility baseline across packable projects, provider matrix tests, verifier guidance, and v0.33 documentation, so the epic is refined and ready for PO-critic without additional PO writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current ticket state is stable: only bot claim comments are present, no attachments are present, and no new description update, attachment, relation change, child ticket, or planning document was needed in this refinement pass.
- The existing child split is already materialized and completed for the core epic scope: 06F9G8EQJGBRSWE96VE028HJYW, 06F9GF2Z4Y7A91ZHG4NW1YTNMC, 06F9G8EXXFJJ1SWWQXC2N9P2X8, 06F9G8F4RQ0T7RV82M3H2H3FVG, 06F9G8FBQTAPXXS1Y4NR5QKVG8, and 06F9G8FJMZ3AY43YG06W2V4T8G.
- Repository evidence now shows the packable runtime and provider projects targeting net8.0 and net10.0, while src/DCoding.Data remains a non-packable net10.0 source-root anchor outside NuGet publication scope.
- The analyzer package stays in the coordinated seven-package family with PrivateAssets="all" guidance, but the analyzer project itself remains a net10.0 build artifact; the compatibility contract is package-line alignment and local analyzer assets, not analyzer-project multi-targeting.
- README.md, docs/manual-nuget-publication.md, docs/releases/v0.33.0.md, docs/plans/shared-implementation-standards.md, and tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs consistently ratify 8.33.0 for net8.0 / EF Core 8 and 10.33.0 for net10.0 / EF Core 10, with no consumer-facing 0.33.0 package version and no mixed-line installs or approvals.

Scope In
- Epic-level coordination of the v0.33 compatibility contract, 8.33.0 and 10.33.0 package-line policy, dual-target runtime/provider packaging, provider matrix validation, verifier guidance, and consumer/release documentation.
- The finite EF/provider matrix bounded to SQLite 8.0.27/10.0.8, MySql.EntityFrameworkCore 10.0.7 for both lines, Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11/10.0.2, Oracle.EntityFrameworkCore 8.<redacted>.<redacted>, and Microsoft.EntityFrameworkCore.SqlServer 8.0.27/10.0.8.
- Preserving the unchanged seven DVault package IDs and the analyzer's local-only PrivateAssets guidance across both compatibility lines.

Scope Out
- New runtime behavior, persistence semantics, provider provisioning, container automation, release automation, or new package IDs.
- Expanding support beyond the bounded v0.33 net8.0 and net10.0 matrix or reopening intentionally net10.0-only helper/project baselines outside package publication scope.
- DB2 provider capability work beyond consuming this epic's compatibility baseline as downstream input.

Open questions
- none

Follow-up questions
- When runtime closure work runs for this epic, should it also clear the live blocks relation from 06F9G8EE7ZA666MW8YEB2QP8BW to 06F9G8GS08VNH0DT09Q4PC2HRC so board views stop showing the compatibility baseline as an active blocker?
- Does the downstream DB2 contract story 06F9G8GS08VNH0DT09Q4PC2HRC need an explicit citation back to the v0.33 matrix artifacts, or is the existing epic and relation context enough?

Risks
- The live relation graph still shows this epic blocking 06F9G8GS08VNH0DT09Q4PC2HRC, so workflow views may temporarily overstate an active blocker until relation cleanup is replayed.
- Helper projects and examples remain net10.0-only by design; if later documentation stops restating that boundary, contributors may misread them as accidental compatibility gaps.
- Because the same package IDs span historical 0.x versions and the new 8.x/10.x consumer lines, stale install guidance can still cause line-mixing confusion if docs and verifier rules drift.

Split recommendations
- No further split is recommended. The epic already has the needed materialized child decomposition: 06F9G8EQJGBRSWE96VE028HJYW, 06F9GF2Z4Y7A91ZHG4NW1YTNMC, 06F9G8EXXFJJ1SWWQXC2N9P2X8, 06F9G8F4RQ0T7RV82M3H2H3FVG, 06F9G8FBQTAPXXS1Y4NR5QKVG8, and 06F9G8FJMZ3AY43YG06W2V4T8G.
- Keep 06F9G8GS08VNH0DT09Q4PC2HRC as downstream follow-on work rather than expanding this epic further.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment