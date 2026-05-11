[gicket-bot] PO-critic review contract

Summary
- PO refinement is ready for developer handoff: the durable contract resolves the validation-routing ambiguity, keeps Open Questions at none, and scopes the remaining work to rerunning package validation in a capable runner without changing the docs unless validation exposes a real failure.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md lines 42-43 show ## Open Questions followed by '- none'.
- Persisted contract lines 13-19 state README.md and docs/releases/v0.6.0.md should remain unchanged unless capable-runner validation finds an actual failure, and require dotnet pack DVault.slnx --configuration Release --nologo plus bash tools/verify-packages.sh evidence from a network/cache-enabled runner.
- README.md lines 357-358 list dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh in Local Validation; README.md line 364 describes verify-packages.sh checking the six package set, symbols, metadata, and provider dependency alignment.
- docs/releases/v0.6.0.md lines 57-61 list dotnet pack and bash tools/verify-packages.sh and explicitly say the release operator must replace pending notes with final audited pass/fail evidence before NuGet publication.
- docs/manual-nuget-publication.md lines 62-63 and 90-91 require dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh as pre-publish evidence; lines 77 and 121 require provider dependency alignment evidence.
- git diff --name-status develop...HEAD shows product-scope repository changes are M README.md and A docs/releases/v0.6.0.md, with ticket metadata/comment/event changes under .gicket for the workflow history.
- git log --oneline -- README.md docs/releases/v0.6.0.md shows the docs update commit 172cc1d4 [06F0MEDJC732GDD77H60R259P0] handoff dev->test (DEV-IMPLEMENTATION implementation).
- gicket-read-ticket-comments returned 46 total comments, with recent tester history returning for package/build/test/format evidence gaps, dev history returning to PO because no-network restore/cache prevented pack verification, and PO refinement resolving that the remaining blocker is environmental validation capability.
- Direct source evidence exists for the public APIs named by the docs: src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilderExtensions.cs defines ApplyDataVaultMetadata; DataVaultCodeFirstModelBuilder.cs defines Hub<TEntity>() and Link(...); DataVaultCodeFirstHubBuilder.cs defines BusinessKey(...) and Satellite(...); DataVaultCodeFirstSatelliteBuilder.cs defines Payload(...) and DrivingKey(...); DataVaultCodeFirstLinkBuilder.cs defines Participant<TEntity>(); IDataVaultReadService.cs defines ReadLatestSatelliteRowsAsync; DataVaultReadServiceTypedProjectionExtensions.cs defines ReadLatestSatelliteAsync<TProjection>.
- Prior scratch comparison git diff --stat 293ca98976d9b4c71c15e50d8bc8724df6f798c6..HEAD over README.md, docs/releases/v0.6.0.md, docs/manual-nuget-publication.md, tools/verify-packages.sh, and DVault.slnx produced no output, supporting the PO instruction that docs were not further changed during refinement.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developer or release-validation orchestration must actually provide a network/cache-enabled mutable runner or complete NuGet cache; repeating the current no-network sandbox will not satisfy the ticket contract.
- If capable-runner package verification fails for a non-environmental reason, the result must come back with concrete command output rather than being treated as a PO clarification issue.

AC / test suggestions
- Dev handoff should capture successful output for dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh from the capable runner before tester signoff.
- If either capable-runner command fails, attach or comment the exact failing command and output so the failure can be routed as a concrete packaging/docs issue.

Implementation watchouts
- Do not edit README.md or docs/releases/v0.6.0.md merely to work around the previous sandbox restore failure.
- Do not weaken package verification into a release-operator-only deferral; the persisted contract keeps it as ticket completion evidence before tester review.
- Keep final NuGet publication approval out of this docs ticket; the release operator still replaces pending release-note placeholders before publication.

Non-blocking notes
- The contract is explicit enough for dev handoff and has no unresolved Open Questions.
- No split is needed unless capable-runner validation exposes a real non-environmental package failure.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment