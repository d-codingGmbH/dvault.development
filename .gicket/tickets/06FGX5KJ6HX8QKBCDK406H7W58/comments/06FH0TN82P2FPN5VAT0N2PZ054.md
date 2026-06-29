[gicket-bot] PO-critic review contract

Summary
- Analyzer-baseline evidence and acceptance criteria are strong, but the ticket still leaves v0.50.0 release-note/changelog cross-reference ownership ambiguous.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `d863a3e51f8ced2b6844fdc17eabb510f8cf0ebd`; `git -C /mnt/c/Projects/DVault log --oneline --decorate -3 -- .gicket/tickets/06FGX5KJ6HX8QKBCDK406H7W58` shows only PO handoff/claim commits (`d863a3e51`, `48869e7e8`, `a9809b38b`), so this is still a pre-development ticket-quality review.
- `.gicket/tickets/06FGX5KJ6HX8QKBCDK406H7W58/description.md` has `## Open Questions` = `- none`, and its implementation notes explicitly say `README` and `docs/package-compatibility.md` currently link to `docs/releases/v0.49.0.md` and must stay internally consistent with the `v0.50.0` baseline when this ticket lands.
- `README.md:187,191,197,222`, `docs/package-compatibility.md:16,57`, `docs/manual-nuget-publication.md:25,32,38,53,85,98`, `CHANGELOG.md:5-14`, and `docs/releases/v0.49.0.md:1-6` all still describe the current baseline as `v0.49.0`.
- `rg --files /mnt/c/Projects/DVault/docs/releases` lists `docs/releases/v0.49.0.md` but no `docs/releases/v0.50.0.md`.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0`; `tools/pack-release-packages.sh` packs analyzer versions `8.50.0` and `10.50.0` without a TFM override; `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` enforces the `.NET 10 SDK` analyzer-host sentence and currently disallows stale install fragments through `0.49.0` / `10.49.0`.

Blocking findings
- The ticket requires touched cross-references to stop carrying stale `v0.49.0` labels, but it does not say whether this ticket must also create or update `docs/releases/v0.50.0.md` and `CHANGELOG.md`, or whether those links should remain deferred to the separate release-note ticket.
- A separate release-note ticket already exists and is still `todo` / `needs-po`, so the current ticket cannot safely assume the target `v0.50.0` release artifact is available; that dependency needs explicit ownership in this ticket.

Required PO actions
- Decide ownership for `v0.50.0` release-note/changelog alignment: either add `CHANGELOG.md` and `docs/releases/v0.50.0.md` to this ticket's in-scope surfaces, or explicitly state that README/package-compatibility/manual-publication must not retarget those links until ticket `06FGX6DSX1SRQ1Y22DP53629S8` lands.
- Add one acceptance criterion that defines the exact intended target for README/package-compatibility release-note references during this ticket.
- If the work stays split, add an explicit dependency note naming ticket `06FGX6DSX1SRQ1Y22DP53629S8` so the developer is not forced to guess the correct cross-reference behavior.

Open issues ledger
- critic-item-1 [required-po-action] Decide ownership for `v0.50.0` release-note/changelog alignment: either add `CHANGELOG.md` and `docs/releases/v0.50.0.md` to this ticket's in-scope surfaces, or explicitly state that README/package-compatibility/manual-publication must not retarget those links until ticket `06FGX6DSX1SRQ1Y22DP53629S8` lands.
- critic-item-2 [required-po-action] Add one acceptance criterion that defines the exact intended target for README/package-compatibility release-note references during this ticket.
- critic-item-3 [required-po-action] If the work stays split, add an explicit dependency note naming ticket `06FGX6DSX1SRQ1Y22DP53629S8` so the developer is not forced to guess the correct cross-reference behavior.
- critic-item-4 [blocking-finding] The ticket requires touched cross-references to stop carrying stale `v0.49.0` labels, but it does not say whether this ticket must also create or update `docs/releases/v0.50.0.md` and `CHANGELOG.md`, or whether those links should remain deferred to the separate release-note ticket.
- critic-item-5 [blocking-finding] A separate release-note ticket already exists and is still `todo` / `needs-po`, so the current ticket cannot safely assume the target `v0.50.0` release artifact is available; that dependency needs explicit ownership in this ticket.

Missing examples / edge cases
- No explicit example shows the expected README/package-compatibility link target when the documentation baseline moves to `v0.50.0` before release notes/changelog are updated.
- The contract asks PackageVerifier/tests to reject mixed-line install claims, but it does not give one concrete mixed-line example such as `DCoding.Data.DVault` on `8.50.0` with `DCoding.Data.DVault.Analyzers` on `10.50.0`.

Risky assumptions
- Assumes a `v0.50.0` release-note/changelog artifact either already exists or can be referenced without clarifying which ticket owns it.
- Assumes a developer will infer the correct fallback behavior for stale `v0.49.0` links without breaking scope boundaries.

AC / test suggestions
- Add an AC that says exactly one of: `this ticket also updates CHANGELOG + docs/releases/v0.50.0.md`, or `this ticket leaves versioned release-note links unchanged pending ticket 06FGX6DSX1SRQ1Y22DP53629S8`.
- Add a verifier/test example for a mixed-line failure case so `8.50.0` runtime plus `10.50.0` analyzer/provider text is clearly rejected alongside `0.50.0` fragments.

Implementation watchouts
- PackageVerifier currently hardcodes stale-version fragments through `0.49.0` / `10.49.0`; the ticket should not say `reject 0.50.0` without clarifying whether that means raw `0.50.0` only or also future stray mixed-line examples.
- The `.NET 10 SDK` analyzer-host baseline is directly evidenced in the csproj, pack script, audit doc, READMEs, and verifier; any wording drift across those surfaces will create inconsistent package-verification behavior.

Non-blocking notes
- `## Open Questions` is explicitly `none`, and the analyzer compatibility baseline itself is well evidenced and consistent with the current repository.
- The ticket already scopes out retargeting the analyzer package and pure `.NET 8 SDK` CI/package-validation work, which keeps the intended implementation bounded.

Split recommendations
- Keep the release-note/package-validation work separate only if this ticket explicitly states how versioned README/package-compatibility links should behave before ticket `06FGX6DSX1SRQ1Y22DP53629S8` lands.
- If PO wants the developer to update versioned release-note links now, merge that ownership into this ticket instead of leaving it implicit.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment