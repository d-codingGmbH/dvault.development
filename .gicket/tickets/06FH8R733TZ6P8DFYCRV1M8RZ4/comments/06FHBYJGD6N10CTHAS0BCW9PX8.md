[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already shows the target documentation surfaces updated: README.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/package-compatibility.md, docs/local-validation.md, docs/manual-nuget-publication.md, docs/releases/v0.50.0.md, and the v0.50.0 changelog entry all describe the dual-host analyzer baseline.
- The current ratified boundary is one netstandard2.0 analyzer asset under analyzers/dotnet/cs/, analyzer references kept local with PrivateAssets=all, and supported consumption on .NET 8 SDK and .NET 10 SDK build hosts for the 8.50.0 and 10.50.0 package lines.

Scope In
- Ratify and preserve the repository-backed documentation baseline for analyzer compatibility in README, analyzer README, package compatibility, local validation, manual publication, and release notes.
- Keep the support statement exact: one analyzer package id, one analyzers/dotnet/cs/ asset root, one netstandard2.0 analyzer asset set, and supported .NET 8 SDK and .NET 10 SDK hosts only.
- Preserve current package-line guidance that 8.50.0 maps to net8.0 and EF Core 8, 10.50.0 maps to net10.0 and EF Core 10, and v0.50.0 is a documentation release label rather than a consumer package version.

Scope Out
- No new analyzer package shape, second package id, split code-fix package, or lib/<tfm> runtime asset contract.
- No broader analyzer-host claim beyond the proved .NET 8 SDK and .NET 10 SDK boundary.
- No mixed package-line guidance, no consumer-facing 0.50.0 package version, and no claim that documentation alone confirms package publication.

Open questions
- none

Follow-up questions
- Confirm replay of outbox mutation-95b9dd5e1ee8609f so the live relation state no longer shows done ticket 06FH8R4EF1QFF2E3ZWS3P1BWHM as a blocker.
- When the coordinated package lines move beyond 8.50.0 and 10.50.0, carry the same dual-host analyzer wording and verifier-backed guards forward in lockstep.

Risks
- Future version-line updates can reintroduce stale net10-only or mixed-line wording if README, analyzer README, package compatibility, release notes, and validation guidance stop moving together.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment