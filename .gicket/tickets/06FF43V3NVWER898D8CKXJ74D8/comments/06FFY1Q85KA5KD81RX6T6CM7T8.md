[gicket-bot] PO-critic review contract

Summary
- The ticket contract is clear, repository-backed, and has no unresolved open questions; it is ready for developer handoff as a bounded contract-ratification task.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0` and packs output under `analyzers/dotnet/cs/`, which supports the single analyzer-asset baseline described by the ticket.
- `tools/pack-release-packages.sh` packs the same analyzer project once for `8.47.0` and once for `10.47.0` without changing analyzer target framework, so both coordinated package lines currently ship the same analyzer binary shape.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0` and references the analyzer project as an `Analyzer` with `SetTargetFramework` fixed to `net10.0`, which is direct local proof of the intended host baseline.
- `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/manual-nuget-publication.md`, `docs/package-compatibility.md`, and `docs/local-validation.md` all describe the `.NET 10 SDK` analyzer-host baseline for both package lines and explicitly say pure `.NET 8 SDK` analyzer consumption is not validated.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` hard-codes the expected analyzer build-host guidance text and runs README checks that reject contradictory pure `.NET 8 SDK` claims in packaged README content.
- `git diff --name-only develop...HEAD` shows only `.gicket/tickets/06FF43V3NVWER898D8CKXJ74D8/**` changes, while `git log --oneline` on the branch shows recent ticket-state commits `048267dfaf` for the PO handoff and `d56b52ae10` for the PO-critic claim; this is a pre-development contract branch, not an implementation branch.
- Comment `.gicket/tickets/06FF43V3NVWER898D8CKXJ74D8/comments/06FFXZSC5ZNKFR0NJVG5J4E65G.md` records PO refinement outcome `po-refinement-ready`, states no new planning writes were needed, and preserves the same bounded recommendation and risks now present in the persisted contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If the product requirement changes to `net8.0` projects built on a pure `.NET 8 SDK` host, the current ticket intentionally does not define the needed asset-target change, smoke lane, or package-verifier expansion; that remains separate scope rather than a blocker here.
- The contract names the authoritative proof files, but any later copied analyzer-install snippets outside those surfaces would still need the same host-SDK caveat to avoid overstating support.

Risky assumptions
- The product decision really is to keep the current `.NET 10 SDK` build-host baseline for both `8.47.0` and `10.47.0`; teams pinned to pure `.NET 8 SDK` toolchains remain intentionally unsupported in this ticket.
- Package-verifier README checks remain the primary anti-drift control; broadening analyzer-host claims anywhere else without matching verifier changes would recreate documentation-versus-verification skew.

AC / test suggestions
- In developer closure evidence, cite the exact `PackageVerifier.cs` README checks together with the integration-project analyzer reference pinned to `net10.0` so the host-baseline claim stays repository-backed.
- Keep closure evidence tied to the proof paths already named in Acceptance Criteria plus the aligned audit note at `docs/plans/analyzer-package-compatibility-audit.md`.

Implementation watchouts
- Do not broaden this ticket into retargeting or multi-targeting `DCoding.Data.DVault.Analyzers`; the contract explicitly ratifies one `net10.0` analyzer asset.
- Do not claim pure `.NET 8 SDK` analyzer consumption support unless a separate ticket lands both an asset-target change and an explicit verification or smoke lane.
- Keep packaged README wording aligned with `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` so package verification does not regress on the analyzer-host guidance.

Non-blocking notes
- `docs/plans/analyzer-package-compatibility-audit.md` still carries prior ticket id `06FBSBW6HDT15D1KGVD7XBQXM8`; the current ticket description explicitly treats that note as existing evidence to ratify, so this is traceability context rather than a blocker.
- Comment `06FFXZV0908G6S6YE7SYMM2Z4R.md` already queues follow-up ticket `06FF43W243BZM340V86CAXQC00` for any future analyzer-retargeting work.

Split recommendations
- Do not split the current ratification ticket further; the repository-backed baseline and follow-up boundary are already explicit.
- If the team wants to promise pure `.NET 8 SDK` analyzer-host support, handle it in a separate additive ticket with its own asset-target and verification contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment