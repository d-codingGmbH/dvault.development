[gicket-bot] PO-critic review contract

Summary
- Return to PO: the docs scope and API claims are mostly source-backed, and Open Questions is none, but the recorded package-validation success is contradicted by current repository package-verifier/readme-version evidence.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md has Open Questions: none and requires recorded successful dotnet pack plus bash tools/verify-packages.sh evidence.
- Manual evidence comment .gicket/tickets/06F0MEDJC732GDD77H60R259P0/comments/06F19YAYMYWFCFFSCTQE6FS8Q4.md records both commands as succeeded, but says validation ran for commit '$sha' rather than a concrete commit.
- Runtime escalation comment 06F19YT7KK76Q221J14QDHBT0W.md says the override was cleared because manual package validation was recorded on commit 688f0c7e.
- git show --stat 688f0c7e6 shows that commit only added ticket/comment/event evidence and edited ticket.json; git diff 688f0c7e6..HEAD for README.md, docs/releases/v0.6.0.md, tools/verify-packages.sh, and PackageVerifier.cs produced no output.
- README.md:10-15 documents all six packages at --version 0.6.0.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:354-360 still requires packaged README.md to contain 0.5.0 install guidance for DCoding.Data.DVault and DCoding.Data.DVault.Sqlite.
- tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:279-280 also seeds 0.5.0 README guidance for verifier tests.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:13 and :24, and provider csproj files such as src/DCoding.Data.DVault.Sqlite/DCoding.Data.DVault.Sqlite.csproj:13 and :24, package the repository root README.md as PackageReadmeFile.
- find artifacts/packages showed only 0.5.1-alpha.0.58 package and symbol artifacts, not v0.6.0 artifacts.
- git show --name-only 172cc1d4c shows the dev implementation changed README.md and docs/releases/v0.6.0.md only; git show --stat 27e32150b identifies the latest package-verifier alignment as v0.5.0.
- Direct source evidence supports README/release-note API claims: ApplyDataVaultMetadata exists in DataVaultCodeFirstModelBuilderExtensions.cs:16-31; Hub and Link builders exist in DataVaultCodeFirstModelBuilder.cs:23-55; BusinessKey/Satellite/Payload/DrivingKey/Participant exist in their builder files; typed read helpers exist in DataVaultReadServiceTypedProjectionExtensions.cs:48-73; raw ReadLatestSatelliteRowsAsync exists in IDataVaultReadService.cs:16-19.
- docs/releases/v0.6.0.md:51-61 keeps package validation separate from final audited NuGet publication approval.

Blocking findings
- The ticket cannot be approved on the current evidence because the successful tools/verify-packages.sh summary conflicts with the current verifier source: it still checks for 0.5.0 README install strings while the ticket's authoritative README now documents 0.6.0 packages.
- The manual validation evidence is too thin to resolve that contradiction: the evidence comment contains a literal '$sha' instead of the validated checkout hash, and the current artifacts/packages directory contains 0.5.1-alpha.0.58 artifacts rather than v0.6.0 artifacts.

Required PO actions
- Clarify the delivery contract with concrete validation evidence that resolves the verifier/readme-version mismatch: exact checkout hash, package artifact versions, package directory state, and the successful verify-packages output summary from the capable runner.
- If the capable-runner validation did not actually validate the current v0.6.0 README/package contents, route or split a concrete packaging-validation follow-up before resubmitting PO-critic.

Open issues ledger
- critic-item-1 [required-po-action] Clarify the delivery contract with concrete validation evidence that resolves the verifier/readme-version mismatch: exact checkout hash, package artifact versions, package directory state, and the successful verify-packages output summary from the capable runner.
- critic-item-2 [required-po-action] If the capable-runner validation did not actually validate the current v0.6.0 README/package contents, route or split a concrete packaging-validation follow-up before resubmitting PO-critic.
- critic-item-3 [blocking-finding] The ticket cannot be approved on the current evidence because the successful tools/verify-packages.sh summary conflicts with the current verifier source: it still checks for 0.5.0 README install strings while the ticket's authoritative README now documents 0.6.0 packages.
- critic-item-4 [blocking-finding] The manual validation evidence is too thin to resolve that contradiction: the evidence comment contains a literal '$sha' instead of the validated checkout hash, and the current artifacts/packages directory contains 0.5.1-alpha.0.58 artifacts rather than v0.6.0 artifacts.

Missing examples / edge cases
- No recorded evidence shows the package verifier passing against packaged README.md content that contains the v0.6.0 install commands.
- No recorded evidence lists the generated package versions or confirms that stale artifacts were absent before verification.

Risky assumptions
- Assuming the manual success summary validated the current v0.6.0 package contents despite PackageVerifier.cs still enforcing v0.5.0 README strings.
- Assuming stale 0.5.1-alpha.0.58 artifacts in artifacts/packages are unrelated without a captured clean package directory or exact verifier output.

AC / test suggestions
- Require the next validation evidence to include the verifier success line and artifact versions produced by dotnet pack.
- Acceptance evidence should explicitly show that README.md packaged into each NuGet artifact contains the intended v0.6.0 installation guidance.

Implementation watchouts
- Do not rely on the current artifacts/packages directory as v0.6.0 evidence; it contains 0.5.1-alpha.0.58 artifacts.
- Downstream validation should treat the package verifier README-version check as a gate to reconcile, not as historical prose.

Non-blocking notes
- The documentation scope itself is bounded to README.md and docs/releases/v0.6.0.md, and the source-backed API surface cited in those docs exists.
- Manual NuGet publication remains out of scope and still needs final audited release-operator approval per docs/releases/v0.6.0.md and docs/manual-nuget-publication.md.

Split recommendations
- No docs split is needed. Split only a concrete packaging-validation/verifier-evidence follow-up if PO confirms the recorded manual validation did not cover the current v0.6.0 package contents.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment