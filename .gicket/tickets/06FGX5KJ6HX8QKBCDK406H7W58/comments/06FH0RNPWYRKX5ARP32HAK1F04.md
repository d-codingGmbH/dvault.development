[gicket-bot] PO refinement contract

Summary
- Repository evidence already ratifies the analyzer compatibility baseline: one net10.0 analyzer asset, PrivateAssets="all", and a .NET 10 SDK build host for both 8.50.0 and 10.50.0. This ticket is bounded to v0.50.0 documentation and package-verifier alignment.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current branch rejects pure .NET 8 SDK analyzer consumption for this release; the supported matrix is a .NET 10 SDK build host with either the 8.50.0/net8.0 or 10.50.0/net10.0 consumer package line.
- DCoding.Data.DVault.Analyzers remains a local build-time package only. Keep analyzer references local with PrivateAssets="all" and do not describe the analyzer as a runtime or transitive package dependency.
- Treat v0.50.0 as the release label and 8.50.0 / 10.50.0 as the consumer package versions. Install examples and PackageReference examples must not use 0.50.0.

Scope In
- Update README analyzer/package compatibility wording to the v0.50.0 release label while keeping package lines 8.50.0 and 10.50.0.
- Update src/DCoding.Data.DVault.Analyzers/README.md to the same release-label, build-host, and PrivateAssets guidance.
- Update docs/package-compatibility.md and docs/manual-nuget-publication.md to the v0.50.0 baseline, including the one net10.0 analyzer asset and .NET 10 SDK host statement.
- Update package-verifier guidance and tests so packaged README expectations preserve the .NET 10 SDK host baseline and reject 0.50.0 or mixed-line install claims.
- Normalize stale labels in the same touched surfaces, including README/manual-publication headings that still reference older release labels.

Scope Out
- Retargeting the analyzer package to net8.0 or netstandard2.0.
- Adding pure .NET 8 SDK CI, pack, or package-verification lanes.
- Changing analyzer/runtime package shape beyond documentation and verifier expectation updates.
- Runtime, provider, or analyzer feature-code changes unrelated to documentation/verifier alignment.

Open questions
- none

Follow-up questions
- If pure .NET 8 SDK analyzer-host support becomes a product requirement later, should it be owned as two follow-ups: analyzer retarget/split work first, then CI/package-verifier/documentation rollout?

Risks
- If the v0.50.0 release-note or changelog artifact lands separately from this ticket, README/package-compatibility/manual-publication cross-references can remain inconsistent even after analyzer wording is corrected.
- PackageVerifier guards packaged README content, but the broader documentation set can still drift unless the in-scope docs are reviewed together in the same change.

Split recommendations
- No split is needed for the current ticket; the current scope is bounded to documentation and verifier-alignment surfaces.
- If pure .NET 8 SDK analyzer-host support is later required, split it into one implementation ticket for analyzer asset/target/dependency changes and one follow-up ticket for CI, package verification, and documentation rollout.

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