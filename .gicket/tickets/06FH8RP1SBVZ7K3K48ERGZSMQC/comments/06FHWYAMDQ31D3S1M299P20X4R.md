[gicket-bot] PO refinement contract

Summary
- Refinement stays bounded to the v0.51.0 release-surface roll-forward; repository evidence confirms the current baseline is still 8.50.0 / 10.50.0, the release-note convention currently ends at docs/releases/v0.50.0.md, and live relation state now shows three incoming blockers.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence still hardcodes the active consumer package lines as 8.50.0 for net8.0 / EF Core 8 and 10.50.0 for net10.0 / EF Core 10 across README.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/package-compatibility.md, docs/local-validation.md, docs/manual-nuget-publication.md, CHANGELOG.md, tools/pack-release-packages.sh, tools/run-analyzer-package-smoke.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs.
- The analyzer boundary remains fixed for this follow-up: one netstandard2.0 analyzer asset under analyzers/dotnet/cs/, analyzer references stay local with PrivateAssets='all', and supported analyzer build hosts remain .NET 8 SDK and .NET 10 SDK only.
- The repository release-note baseline currently ends at docs/releases/v0.50.0.md, so the v0.51.0 update should follow that established release-note location and naming pattern alongside the changelog update.
- Live relation evidence currently shows incoming blocks from 06FH8QAVJFXANVQFXGPYVAFXSR, 06FH8R9DPSKTNYB46HHVJMZ9P8, and 06FH8RFJYY09BJJK4MD2KT8BF0; follow-up comments from the latter two show upstream workflow progress, but the blocking relations still need to be cleared or intentionally updated in live state.

Scope In
- Roll forward all current v0.50.0 release-surface guidance that should advance with this baseline, including README.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/package-compatibility.md, docs/local-validation.md, docs/manual-nuget-publication.md, CHANGELOG.md, and the v0.51.0 release-note artifact under docs/releases/.
- Update packaging and validation baselines in tools/pack-release-packages.sh, tools/run-analyzer-package-smoke.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs from 8.50.0 / 10.50.0 to 8.51.0 / 10.51.0.
- Keep the existing coordinated-family rules intact: nine packable packages, one selected package-version line per consumer project, analyzer package alignment with the chosen runtime line, and no consumer-facing 0.51.0 NuGet version.

Scope Out
- Changing analyzer package shape, asset layout, or package ids.
- Broadening analyzer-host support beyond the reviewed .NET 8 SDK and .NET 10 SDK boundary.
- Changing dependency-major baselines, provider scope, or the manual-publication workflow beyond the coordinated version-line roll-forward.

Open questions
- none

Follow-up questions
- When the three upstream blocker tickets are complete, clear or downgrade any remaining incoming blocks relations so comments and live relation state do not diverge.
- If any additional release-surface file outside the evidenced set still advertises the active consumer baseline, update it in the same change rather than leaving a partial 8.50.0 / 10.50.0 trail.

Risks
- Because three live incoming blocks relations currently target this ticket, workflow comments alone are not enough to prove dependency clearance; stale relations can keep the ticket artificially blocked.
- Partial version bumps across docs, scripts, verifier logic, and verifier tests can leave the release baseline internally inconsistent even though the underlying analyzer implementation is already settled.
- Any mixed-line guidance or consumer-facing 0.51.0 package claim would publish incorrect installation and approval instructions.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment