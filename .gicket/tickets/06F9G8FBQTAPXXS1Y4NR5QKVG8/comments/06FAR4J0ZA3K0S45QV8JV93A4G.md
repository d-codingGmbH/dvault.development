[gicket-bot] PO refinement contract

Summary
- Bounded this ticket as the reusable dual-line package-verifier and manual/CI-guidance task behind epic 06F9G8EE7ZA666MW8YEB2QP8BW: the matrix-test story is already done, this task still blocks broader v0.33.0 compatibility docs, and no additional PO blocker remains.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- docs/plans/shared-implementation-standards.md already fixes the v0.33 package-line contract: keep the existing seven package IDs, publish 8.33.0 for net8.0 and EF Core 8 and 10.33.0 for net10.0 and EF Core 10, do not introduce a consumer-facing 0.33.0 package line, and do not use mixed-line examples.
- Current repository evidence already shows the runtime and provider packages multitarget net8.0 and net10.0, while DCoding.Data.DVault.Analyzers and tools/DCoding.Data.DVault.PackageVerification remain net10-only helper projects; this ticket owns broader verifier and guidance behavior, not helper-project retargeting.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs already owns the reusable package-artifact gate for nuspec metadata, README presence and content, XML docs, analyzer assets, symbols, and dependency groups; this ticket should make that gate authoritative for both target-framework dependency groups and mixed-line rejection.
- .github/workflows/ci.yml already runs restore, format, build, filtered test, pack, and bash tools/verify-packages.sh with no publish step, and docs/manual-nuget-publication.md already preserves manual publication as a separate release activity.
- Relation context is coherent: epic 06F9G8EE7ZA666MW8YEB2QP8BW is the parent, done story 06F9G8F4RQ0T7RV82M3H2H3FVG is historical prerequisite evidence for exact matrix assertions, and this ticket still blocks task 06F9G8FJMZ3AY43YG06W2V4T8G so broader compatibility docs consume the finalized verifier and guidance contract.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass because the existing ticket and repository evidence already bounded the work.

Scope In
- Extend the reusable package-verifier path so each produced package validates the expected net8.0 and net10.0 nuspec dependency groups, including provider-to-core version alignment and rejection of mixed EF Core lines inside one target group.
- Update local validation and manual release guidance for the dual package lines, including the required build, test, pack, verify, and check-format evidence and artifact inspection expectations for package metadata, README content, XML docs, analyzer assets, and symbols.
- Update CI guidance so the blocking repository validation path explicitly includes pack plus bash tools/verify-packages.sh for the dual-line package family while preserving manual publication separation and avoiding any publish automation.
- Update packaged install-guidance expectations so README.md and src/DCoding.Data.DVault.Analyzers/README.md distinguish 8.33.0 and net8.0 and EF Core 8 from 10.33.0 and net10.0 and EF Core 10 and keep analyzer guidance local with PrivateAssets=all.

Scope Out
- Automatic NuGet publishing, release credentials, package push tooling, or any CI step that publishes artifacts.
- Reopening the already-selected dual-target project baseline or retargeting tools/DCoding.Data.DVault.PackageVerification or analyzer projects to net8.0 as part of this ticket.
- New runtime or provider behavior, new supported providers, or new mandatory live external-provider database execution in the default validation lane.
- Broader release-note or adopter-documentation ownership beyond the verifier and manual-guidance boundary already blocked downstream in 06F9G8FJMZ3AY43YG06W2V4T8G.

Open questions
- none

Follow-up questions
- After this ticket lands, should 06F9G8FJMZ3AY43YG06W2V4T8G fold the same dual-line examples into broader v0.33.0 compatibility and adopter documentation, or is a narrower consumer-doc slice still needed?
- If later guidance wants explicit Pomelo.EntityFrameworkCore.MySql or multi-SDK-lane consumer advice, should that be scheduled as post-v0.33 follow-up rather than broaden this verifier and manual-guidance task?

Risks
- Current root and analyzer README examples are still single-line 0.32.0 guidance, so verifier and documentation updates can drift apart unless this ticket treats packaged README content as part of the same dual-line contract.
- Because tools/DCoding.Data.DVault.PackageVerification stays net10-only, guidance must be explicit that the .NET 10 SDK validation lane still proves both net8.0 and net10.0 package outputs; otherwise maintainers may incorrectly narrow validation to one package line.
- Broader consumer-facing compatibility documentation remains blocked by 06F9G8FJMZ3AY43YG06W2V4T8G, so stale public install examples can persist even after matrix tests and verifier checks are in place unless the downstream docs task is completed promptly.

Split recommendations
- No additional split is recommended: done story 06F9G8F4RQ0T7RV82M3H2H3FVG already carved out exact provider and version matrix proof, while this ticket remains the bounded home for reusable package verification, README and manual guidance, and CI documentation.
- Keep broader release-note and compatibility-prose updates in blocked task 06F9G8FJMZ3AY43YG06W2V4T8G rather than expanding this ticket into full v0.33.0 documentation ownership.

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