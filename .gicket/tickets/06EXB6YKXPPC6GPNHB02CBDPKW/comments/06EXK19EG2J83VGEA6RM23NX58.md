[gicket-bot] PO-critic review contract

Summary
- The delivery contract is ready for developer handoff: scope is bounded to NuGet metadata on the visible library project, the license decision is resolved, and Open Questions contains only 'none'.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted delivery contract has PO Handoff decision ready_for_po_critic.
- The contract's Open Questions section says 'none'.
- Repository branch snapshot shows src/DVault/DVault.csproj exists and targets net10.0 with GenerateDocumentationFile enabled.
- Repository branch snapshot lists src-roots including src/DVault, src/DCoding.Data, and src/DCoding.Data.DVault.
- README.md identifies DVault as the repository for the DCoding.Data.DVault .NET library.
- The contract explicitly selects PackageLicenseExpression Apache-2.0 and the branch snapshot shows path Apache-2.0 is missing, aligning with the instruction not to add PackageLicenseFile.
- The contract scopes out publishing, credentials, API keys, release tokens, push commands, SourceLink policy, and package verification automation beyond local manual inspection.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract uses src/DVault/DVault.csproj as the package target even though README.md also mentions a reserved src/DCoding.Data.DVault path; this is acceptable because the branch snapshot visibly includes src/DVault/DVault.csproj and the contract explicitly names it as the target.

AC / test suggestions
- During development, local package inspection should confirm nuspec metadata, README inclusion at package root, license expression Apache-2.0, repository URL/type, and snupkg generation/readiness without uploading anything.

Implementation watchouts
- Keep metadata on src/DVault/DVault.csproj unless an existing repo convention clearly requires central package props.
- Do not introduce any NuGet publish workflow, token, endpoint, or documented push command.
- Do not add PackageLicenseFile for Apache-2.0 because the contract requires PackageLicenseExpression and the branch snapshot shows no Apache-2.0 file.

Non-blocking notes
- Follow-up release details such as NuGet owner/profile, project URL, icon, release notes, signing, and final package README placement are correctly deferred.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment