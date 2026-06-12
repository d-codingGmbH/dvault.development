[gicket-bot] PO-critic review contract

Summary
- Repository and ticket evidence support the chosen `net10.0` SDK-gate contract, but this ticket does not identify a concrete remaining implementation delta and overlaps with another open docs/verification ticket.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket ticket `06FBSBWBT33K7Y1Z6NM71GAQ68` revision `06FBVKG03WDCRS78TEV9SBS4B4` persists `## Open Questions - none` and scopes the work to the explicit SDK-gate path.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` line 3 targets only `net10.0`, and lines 43-46 pack analyzer files under `analyzers/dotnet/cs/`.
- `README.md` line 44 and `src/DCoding.Data.DVault.Analyzers/README.md` line 21 already require a `.NET 10 SDK` host for analyzer consumers, including `net8.0` on `8.36.0`.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` defines `ExpectedAnalyzerBuildHostGuidance` at line 17 and invokes README/analyzer checks at lines 441, 504-507, and 529.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` line 46 references the analyzer project with `PrivateAssets=all` and forces the analyzer TFM to `net10.0` for the multi-targeted test project.
- Branch inspection shows `HEAD` on `ticket/06FBSBWBT33K7Y1Z6NM71GAQ68-task-add-compatible-analyzer-asset-or-explicit-s` at `0d6b19b5717fd523775b0b63dc8fa48e18f903d0`, and `git diff --name-only develop...HEAD -- . ':(exclude).gicket/**'` returned no non-ticket files.
- There is a persisted `blocks` relation file `.gicket/relations/68/JM/06FBSBWBT33K7Y1Z6NM71GAQ68--06FBSBWH9F415E12VRHRYQ2JJM--blocks.json`, and related ticket `06FBSBWH9F415E12VRHRYQ2JJM` is still `todo` with title `Task: Update analyzer packaging docs and verification for compatibility outcome`.

Blocking findings
- The current contract does not name a concrete remaining delta for this ticket: direct repository evidence already matches the selected single-asset `net10.0` plus `.NET 10 SDK` gate path, and the branch contains no non-ticket changes. Without a residual gap or an explicit closure-only/no-work-required posture, developer handoff is ambiguous.
- Scope ownership is unclear because this ticket still keeps documentation and verification work in scope while open todo ticket `06FBSBWH9F415E12VRHRYQ2JJM` already exists specifically for analyzer packaging docs and verification and is blocked by this ticket.

Required PO actions
- Clarify whether `06FBSBWBT33K7Y1Z6NM71GAQ68` still has any residual implementation work that is not already satisfied on `develop`; if not, convert it to closure/no-work-required or close it.
- Clarify ownership between `06FBSBWBT33K7Y1Z6NM71GAQ68` and `06FBSBWH9F415E12VRHRYQ2JJM`: either merge/supersede one ticket or narrow each ticket so docs/verification work lives in exactly one open ticket.
- If residual work does exist, add one concrete missing artifact, failing verifier expectation, or missing validation surface that developers must change instead of restating the already-landed baseline.

Open issues ledger
- critic-item-1 [required-po-action] Clarify whether `06FBSBWBT33K7Y1Z6NM71GAQ68` still has any residual implementation work that is not already satisfied on `develop`; if not, convert it to closure/no-work-required or close it.
- critic-item-2 [required-po-action] Clarify ownership between `06FBSBWBT33K7Y1Z6NM71GAQ68` and `06FBSBWH9F415E12VRHRYQ2JJM`: either merge/supersede one ticket or narrow each ticket so docs/verification work lives in exactly one open ticket.
- critic-item-3 [required-po-action] If residual work does exist, add one concrete missing artifact, failing verifier expectation, or missing validation surface that developers must change instead of restating the already-landed baseline.
- critic-item-4 [blocking-finding] The current contract does not name a concrete remaining delta for this ticket: direct repository evidence already matches the selected single-asset `net10.0` plus `.NET 10 SDK` gate path, and the branch contains no non-ticket changes. Without a residual gap or an explicit closure-only/no-work-required posture, developer handoff is ambiguous.
- critic-item-5 [blocking-finding] Scope ownership is unclear because this ticket still keeps documentation and verification work in scope while open todo ticket `06FBSBWH9F415E12VRHRYQ2JJM` already exists specifically for analyzer packaging docs and verification and is blocked by this ticket.

Missing examples / edge cases
- The ticket does not describe the already-satisfied/no-op path: what should happen if `develop` already meets every acceptance criterion before developer work starts?
- The contract does not describe how responsibility splits if this ticket owns only the compatibility decision while `06FBSBWH9F415E12VRHRYQ2JJM` owns docs and verification edits.

Risky assumptions
- It assumes there is still developer work on this ticket even though the current repository and verifier already reflect the selected compatibility contract.
- It assumes developers will infer the intended boundary between this ticket and blocked ticket `06FBSBWH9F415E12VRHRYQ2JJM` without an explicit supersession or split statement.

AC / test suggestions
- If the ticket remains open, add one residual acceptance criterion that names the exact missing surface or failing verification condition that still needs work; otherwise reclassify the ticket as closure/no-work-required.
- Add an explicit routing criterion stating whether this ticket owns only the compatibility decision or also owns the docs/package-verifier work now described in `06FBSBWH9F415E12VRHRYQ2JJM`.
- If the blocked child ticket remains the implementation vehicle, add a completion criterion that this ticket closes once the decision is ratified and the child is the sole open execution ticket.

Implementation watchouts
- Do not broaden the claim to pure `.NET 8 SDK` analyzer consumption without a separate asset and verification expansion.
- Keep `PrivateAssets=all`, the `.NET 10 SDK` host wording, and packaged README/package-verifier expectations aligned across root README, analyzer README, and publication guidance.
- Preserve the `net8.0` consumer target plus `net10.0` analyzer-host validation lane already expressed by the integration test project.

Non-blocking notes
- The persisted delivery contract is otherwise well structured: scope in/out, risks, and `## Open Questions - none` are explicit.
- The repository evidence cited from audit ticket `06FBSBW6HDT15D1KGVD7XBQXM8` is directly present in the current tree, so the technical compatibility conclusion itself is not in doubt.

Split recommendations
- No new split is needed if PO closes or supersedes one of the overlapping tickets; otherwise narrow `06FBSBWBT33K7Y1Z6NM71GAQ68` to the decision/closure path and leave docs plus verification implementation in `06FBSBWH9F415E12VRHRYQ2JJM`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment