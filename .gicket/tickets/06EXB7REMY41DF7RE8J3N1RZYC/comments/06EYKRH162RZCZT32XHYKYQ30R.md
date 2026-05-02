[gicket-bot] PO-critic review contract

Summary
- Refinement is developer-ready: repository and ticket evidence confirm the current README documentation gap, the correct project/package targets, and no unresolved PO questions remain.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket `06EXB7REMY41DF7RE8J3N1RZYC` (revision `06EYKQSE7B0S2VGG51YKYMDPVG`) contains `## Open Questions` = `none` and PO handoff `ready_for_po_critic`.
- `README.md` currently starts with a Quickstart that assumes the consumer project already references `DCoding.Data.DVault`; the observed file has no installation or project-reference section before that quickstart.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` declares `<PackageId>DCoding.Data.DVault</PackageId>` and packs `../../README.md` as the package README via `<None Include="../../README.md" Pack="true" PackagePath="/" />`.
- `DVault.slnx` includes `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, matching the contract's required current project-reference target.
- Source-backed quickstart APIs exist as described: `AddDVault` in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`, `ApplyDataVaultMetadata` in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, and `IDataVaultSaveService` in `src/DCoding.Data.DVault/DataVaultSaveService.cs`.
- Direct branch inspection command `git show --stat --oneline c91b93ab3dea62dfc4c25f13f52269669ba60c4c -- README.md src/DCoding.Data.DVault/DCoding.Data.DVault.csproj DVault.slnx` exited 0 with no output, so no additional scoped file-change evidence contradicted the persisted contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract intentionally leaves the `<ProjectReference>` snippet relative path representative rather than universal; final docs should avoid implying one path fits every consumer solution layout.
- Because the root README is also the packaged README, the pre-publication/from-source framing is an edge case that must stay explicit in the final wording.

Risky assumptions
- This refinement assumes `DCoding.Data.DVault` is still unpublished when the documentation work is implemented.
- This refinement assumes `README.md` remains the primary consumer discovery surface for install guidance during the pre-publication phase.

AC / test suggestions
- Validate that the final README introduces project-reference installation guidance before or adjacent to Quickstart and clearly targets `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`.
- Reject any implementation that adds executable NuGet install commands, version numbers, or claims that `DCoding.Data.DVault` is already available on NuGet.
- Run `bash tools/check-format.sh` as part of review because the ticket Definition of Done explicitly references repository formatting-gate expectations.

Implementation watchouts
- The root README is packed into the future NuGet package, so project-reference instructions must be clearly labeled as pre-publication or from-source guidance.
- Keep the change scoped to installation/consumption framing; the existing Quickstart API flow already matches observed public source symbols and should not need behavioral expansion from PO.

Non-blocking notes
- none

Split recommendations
- No split recommended; the scope remains one bounded README/documentation refinement, consistent with the persisted contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment