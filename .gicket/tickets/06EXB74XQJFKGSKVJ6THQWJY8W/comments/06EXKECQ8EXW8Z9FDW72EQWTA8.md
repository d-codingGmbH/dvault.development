[gicket-bot] PO-critic review contract

Summary
- Persisted PO contract is ready for developer handoff: open questions are closed, foundation structure is present on the target branch, and the scope/AC are bounded to v1 metadata abstractions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git rev-parse --abbrev-ref HEAD returned ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst; git rev-parse HEAD returned 8278e40307c86a6d127105a96c9d09a40d2a3749.
- .gicket/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/description.md has PO Handoff decision ready_for_po_critic and ## Open Questions with '- none'.
- The persisted Scope In requires hub/link/satellite metadata abstractions under src/DVault, documented public/protected members, minimum relationships, and focused tests under tests/DVault.Tests; Scope Out excludes scaffolding, persistence, SQL, serialization, runtime discovery, and advanced Data Vault variants.
- Comment .gicket/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/comments/06EXK0F1N5T118E5XHSKMMW0XC.md records the prior foundation blocker as answered and says DVault.slnx, src/DVault, and tests/DVault.Tests are present; it also records BOT-LOCAL-TOOL-TRUST-BLOCKED for the attempted blocks relation and says that is no longer a development blocker.
- git ls-files confirmed DVault.slnx, src/DVault/DVault.csproj, src/DVault/Modeling/*.cs, tests/DVault.Tests/DVault.Tests.csproj, and tests/DVault.Tests/Modeling/*.cs are tracked.
- src/DVault/DVault.csproj targets net10.0 and has GenerateDocumentationFile true; tests/DVault.Tests/DVault.Tests.csproj references ../../src/DVault/DVault.csproj and runs the executable test target.
- src/DVault/Modeling/DataVaultModel.cs shows existing public modeling API in namespace DVault.Modeling, including DataVaultModel, DataVaultModelBuilder, DataVaultHubBuilder, DataVaultSatelliteBuilder, DataVaultTable, DataVaultColumn, DataVaultIndex, and DataVaultConstraint, with XML documentation and ArgumentException/ArgumentNullException validation conventions.
- docs/architecture/mvp-data-vault-concepts.md says the MVP concept set is limited to hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources; it also says links connect two or more hubs and satellites reference a parent hub or link.
- docs/plans/deferred-data-vault-capabilities.md says PIT table generation, bridge table generation, multi-active satellites, and provider-specific optimizations are deferred and must not be treated as MVP requirements.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking PO gap observed. A developer/tester should still cover both the zero-endpoint and one-endpoint link cases because the contract's minimum relationship is two or more endpoints.
- No blocking PO gap observed. Satellite examples should make clear whether v1 tests cover a hub parent, a link parent, or both; the contract permits a parent hub or link.

Risky assumptions
- The ticket intentionally leaves concrete metadata type and member names to implementation while requiring a small documented public API; this is acceptable for handoff but should be reviewed against existing DVault.Modeling naming conventions.
- Existing DataVaultModelBuilder.Link in src/DVault/Modeling/DataVaultModel.cs currently checks only for zero participants, while this ticket's metadata contract requires at least two endpoints; dev must not infer the existing one-participant minimum is sufficient for the new metadata abstraction behavior.
- The absent persisted blocks relation is accepted as sequencing history, based on the refreshed contract and comment evidence that foundation paths now exist.

AC / test suggestions
- Add focused tests under tests/DVault.Tests that construct valid hub metadata and assert identifying properties are retained.
- Add focused tests that construct valid link metadata with at least two endpoints and assert endpoint relationships are retained.
- Add focused tests that construct satellite metadata with a required parent relationship and descriptive metadata, including the missing-parent failure case.
- Add deterministic negative tests for null, empty, and whitespace required names, plus link endpoint counts below the required minimum.

Implementation watchouts
- Keep the new abstractions in src/DVault, likely under DVault.Modeling unless nearby source establishes a more specific convention.
- Keep the public surface small and XML-documented; the source project already emits documentation files.
- Do not introduce persistence, SQL rendering, migrations, serialization, configuration loading, runtime discovery, PIT, bridge, multi-active satellite, or provider-specific behavior in this ticket.
- Use the repository's existing argument validation style, visible in DataVaultModel.cs as ArgumentException.ThrowIfNullOrWhiteSpace and ArgumentNullException.ThrowIfNull.
- Fit tests into the existing tests/DVault.Tests executable-test convention or the nearby Modeling test pattern.

Non-blocking notes
- The dirty worktree entries observed by git status are in gicket metadata for other tickets and do not change the contract evidence for this ticket.

Split recommendations
- No split recommended; the persisted contract explicitly says the metadata abstraction scope remains valid for v1 as one focused modeling task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment