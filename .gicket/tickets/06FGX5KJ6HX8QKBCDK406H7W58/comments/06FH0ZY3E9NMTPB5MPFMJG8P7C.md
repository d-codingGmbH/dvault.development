[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Ownership is now explicit. This ticket owns analyzer-compatibility documentation and package-verifier alignment only. CHANGELOG.md and docs/releases/v0.50.0.md stay owned by ticket 06FGX6DSX1SRQ1Y22DP53629S8, and README/package-compatibility/manual-publication must not retarget release-note or changelog links to v0.50.0 before that ticket lands.
- critic-item-2: `answered` - An explicit acceptance criterion now defines the target: during this ticket, README, docs/package-compatibility.md, and docs/manual-nuget-publication.md keep any release-note or changelog cross-reference on the existing v0.49.0 artifact and do not introduce a docs/releases/v0.50.0.md or CHANGELOG.md retarget before ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands.
- critic-item-3: `answered` - The split-work dependency note is now explicit in the contract and persisted ticket state. Implementation notes name ticket 06FGX6DSX1SRQ1Y22DP53629S8 as the separate follow-on owner for CHANGELOG.md, docs/releases/v0.50.0.md, and later release-note/changelog link retargeting, and the current ticket has a live outgoing relation to that ticket.
- critic-item-4: `answered` - The contract now resolves the stale-label ambiguity by separating wording cleanup from release-artifact ownership. This ticket may normalize stale v0.49.0/v0.47 headings and analyzer wording inside the touched surfaces, but CHANGELOG.md and docs/releases/v0.50.0.md remain out of scope here and v0.49.0 release-note/changelog cross-references stay intentionally preserved until the separate release-note ticket lands.
- critic-item-5: `answered` - The separate release-note ticket is still not landed, so the current ticket no longer assumes v0.50.0 release artifacts already exist. The contract records split ownership instead of guessing availability, and ticket 06FGX6DSX1SRQ1Y22DP53629S8 remains the named owner for those artifacts.

Clarifications
- Supported analyzer consumption remains a .NET 10 SDK build host with either the 8.50.0/net8.0 or 10.50.0/net10.0 consumer package line; pure .NET 8 SDK analyzer-host support is not part of this ticket.
- DCoding.Data.DVault.Analyzers remains a local build-time package only; analyzer references stay local with PrivateAssets="all" and must not be described as runtime or transitive dependencies.
- This ticket updates analyzer-compatibility documentation to the v0.50.0 documentation baseline while intentionally leaving release-note and changelog cross-references on the current v0.49.0 targets until ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands.
- Persisted relation state currently links this ticket to 06FGX6DSX1SRQ1Y22DP53629S8 through outgoing relation 06FGX5KJ6HX8QKBCDK406H7W58--06FGX6DSX1SRQ1Y22DP53629S8--blocks.

Scope In
- Update README analyzer/package compatibility wording to the v0.50.0 documentation baseline while keeping consumer package lines 8.50.0 and 10.50.0.
- Update src/DCoding.Data.DVault.Analyzers/README.md to the same release-label, build-host, and PrivateAssets="all" guidance.
- Update docs/package-compatibility.md and docs/manual-nuget-publication.md to the v0.50.0 analyzer compatibility baseline, including the one net10.0 analyzer asset and .NET 10 SDK host statement.
- Update package-verifier guidance and tests so packaged README expectations preserve the .NET 10 SDK host baseline and reject 0.50.0 or mixed-line install claims.
- Normalize stale labels in the touched surfaces, including stale README/manual-publication headings, while keeping release-note/changelog links on their current v0.49.0 targets during this ticket.

Scope Out
- Creating or updating CHANGELOG.md or docs/releases/v0.50.0.md; that work remains owned by ticket 06FGX6DSX1SRQ1Y22DP53629S8.
- Retargeting README, docs/package-compatibility.md, or docs/manual-nuget-publication.md release-note/changelog links to v0.50.0 before ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands.
- Retargeting the analyzer package to net8.0 or netstandard2.0.
- Adding pure .NET 8 SDK CI, pack, or package-verification lanes.
- Changing analyzer/runtime package shape beyond documentation and verifier expectation updates.
- Runtime, provider, or analyzer feature-code changes unrelated to documentation/verifier alignment.

Open questions
- none

Follow-up questions
- After ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands, should the release-note owner run one final documentation sweep to move all remaining v0.49.0 release-note/changelog cross-references to the new v0.50.0 artifacts together?
- If pure .NET 8 SDK analyzer-host support becomes a product requirement later, should it be owned as two follow-up tickets: analyzer asset/target work first, then CI/package-verifier/documentation rollout?

Risks
- Until ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands, the repository will intentionally keep v0.49.0 release-note/changelog links next to v0.50.0 analyzer wording in the touched documentation surfaces; reviewers need to treat that as planned split ownership, not an accidental regression.
- PackageVerifier guards packaged README content, but the broader documentation set can still drift unless the in-scope docs are reviewed together in the same change.

Split recommendations
- Keep the current split: this ticket owns analyzer-compatibility documentation and verifier alignment, while ticket 06FGX6DSX1SRQ1Y22DP53629S8 owns CHANGELOG.md, docs/releases/v0.50.0.md, and the eventual release-note/changelog link retarget.
- If pure .NET 8 SDK analyzer-host support is later required, split it into one implementation ticket for analyzer asset/target/dependency changes and one follow-up ticket for CI, package verification, and documentation rollout.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment