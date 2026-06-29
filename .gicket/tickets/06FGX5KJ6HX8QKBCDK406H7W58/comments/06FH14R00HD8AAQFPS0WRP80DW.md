[gicket-bot] PO-critic review contract

Summary
- Delivery contract ambiguity around v0.50.0 release-note ownership is now explicitly resolved, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `22dd2c7772c45a103b131fe78cc2932c69d81342`; `git -C /mnt/c/Projects/DVault log --oneline --decorate -5 -- .gicket/tickets/06FGX5KJ6HX8QKBCDK406H7W58` shows the latest PO handoff commit `ce323bee5` for this ticket, followed only by automation/claim commits, so this remains a pre-development ticket-quality review.
- `.gicket/tickets/06FGX5KJ6HX8QKBCDK406H7W58/description.md` now makes ownership explicit: `Scope Out` excludes `CHANGELOG.md` and `docs/releases/v0.50.0.md`, and Acceptance Criteria require `README`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md` to keep release-note/changelog links on `v0.49.0` until ticket `06FGX6DSX1SRQ1Y22DP53629S8` lands.
- The same description has `## Open Questions` with `- none`, so there is no unresolved delivery-contract question blocking developer handoff.
- Prior PO-critic return-to-PO evidence exists in `.gicket/tickets/06FGX5KJ6HX8QKBCDK406H7W58/comments/06FH0TN82P2FPN5VAT0N2PZ054.md` (`decision: return_to_po` for ambiguous release-note ownership); the follow-up PO refinement comment `.gicket/tickets/06FGX5KJ6HX8QKBCDK406H7W58/comments/06FH0ZY3E9NMTPB5MPFMJG8P7C.md` marks critic-items 1-5 as `answered` and records the exact split ownership and preserved `v0.49.0` link target.
- `.gicket/relations/58/S8/06FGX5KJ6HX8QKBCDK406H7W58--06FGX6DSX1SRQ1Y22DP53629S8--blocks.json` exists, and `.gicket/tickets/06FGX6DSX1SRQ1Y22DP53629S8/ticket.json` shows related ticket `06FGX6DSX1SRQ1Y22DP53629S8` still `todo` with `needs-po`, matching the documented split.
- `git -C /mnt/c/Projects/DVault ls-files docs/releases/v0.50.0.md CHANGELOG.md` returned only `CHANGELOG.md`, and `test -e /mnt/c/Projects/DVault/docs/releases/v0.50.0.md; echo $?` returned `1`, confirming the repo currently has no `docs/releases/v0.50.0.md`; the contract now explicitly treats that as out of scope here.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0`; `tools/pack-release-packages.sh` packs analyzer lines `8.50.0` and `10.50.0`; `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` still enforces the `.NET 10 SDK` analyzer-host sentence and stale install-fragment guardrails, matching the delivery contract's compatibility baseline.
- `README.md`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md` still contain `v0.49.0` references, and `docs/manual-nuget-publication.md` still contains `## Current v0.47 Dependency Matrix`; those are directly observed stale surfaces the ticket intentionally assigns to developer cleanup or preservation per the refined scope.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not show one concrete mixed-line bad example such as runtime `8.50.0` plus analyzer `10.50.0`; the developer should rely on verifier/test coverage rather than prose alone.

Risky assumptions
- Assumes downstream ticket `06FGX6DSX1SRQ1Y22DP53629S8` will later create or retarget `docs/releases/v0.50.0.md` and release-note/changelog links as documented by the live `blocks` relation.
- Assumes reviewers will treat intentionally preserved `v0.49.0` release-note/changelog links as planned split ownership, not as accidental stale wording.

AC / test suggestions
- Keep at least one verifier/test case that rejects raw `0.50.0` install or `PackageReference` fragments and one that rejects mixed package-line guidance.
- When updating docs, verify all human-facing analyzer guidance repeats the same three facts: local `PrivateAssets="all"`, one `net10.0` analyzer asset, and `.NET 10 SDK` build-host requirement for both `8.50.0` and `10.50.0` lines.

Implementation watchouts
- Do not retarget README/package-compatibility/manual-publication release-note or changelog links to `v0.50.0` on this ticket; the refined contract explicitly keeps those on `v0.49.0` until ticket `06FGX6DSX1SRQ1Y22DP53629S8` lands.
- Normalize stale headings like `## Current v0.47 Dependency Matrix` without pulling `CHANGELOG.md` or `docs/releases/v0.50.0.md` into scope.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` currently hardcodes stale-version fragments through `0.49.0` / `10.49.0`; the developer handoff needs to carry that guardrail forward consistently with the `v0.50.0` wording change.

Non-blocking notes
- The repository still lacks a `docs/releases/v0.50.0.md` file today, but the refined contract and related-ticket dependency now make that absence intentional rather than ambiguous.

Split recommendations
- Keep the current split: this ticket owns analyzer-compatibility docs and verifier alignment, while ticket `06FGX6DSX1SRQ1Y22DP53629S8` owns `CHANGELOG.md`, `docs/releases/v0.50.0.md`, and the later link retarget.
- After ticket `06FGX6DSX1SRQ1Y22DP53629S8` lands, run one release-note/changelog cross-reference sweep across `README.md`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md` to remove the intentionally preserved `v0.49.0` links.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment