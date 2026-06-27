[gicket-bot] PO-critic review contract

Summary
- The delivery contract is bounded and has no open questions, but the closure-only framing is not supported by the current branch: the repository still documents the v0.48 baseline and recent commits only update `.gicket` ticket metadata.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF4430YGFJV43ZS54RXEJD5R/description.md` says `PO Handoff decision: ready_for_po_critic`, scopes future updates to v0.49 release docs, and has `## Open Questions - none`.
- `git show --stat --summary --oneline HEAD` for `fb98ff995b622131f436226b6dd69a1463637075` lists only `.gicket/tickets/06FF4430YGFJV43ZS54RXEJD5R/*` files; `git show --stat --summary --oneline --name-only 8bc6f09edf` likewise lists ticket metadata files, not docs.
- `rg -n "v0\.49\.0|8\.49\.0|10\.49\.0" CHANGELOG.md README.md docs/package-compatibility.md src/DCoding.Data.DVault.Analyzers/README.md docs/releases -g '*.md'` returned no matches, and `git ls-files docs/releases/v0.49.0.md` returned no tracked file.
- `rg -n "v0\.48\.0|8\.48\.0|10\.48\.0" CHANGELOG.md README.md docs/package-compatibility.md src/DCoding.Data.DVault.Analyzers/README.md docs/releases/v0.48.0.md` still finds the active baseline in `CHANGELOG.md`, `README.md`, `docs/package-compatibility.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, and `docs/releases/v0.48.0.md`.
- `src/DCoding.Data.DVault/IDataVaultLinkMapper.cs` documents repeated same-hub support only when produced participant names are unique by `StringComparer.Ordinal`, and `src/DCoding.Data.DVault/IDataVaultSaveService.cs` keeps the explicit save-service boundary.
- `docs/model-first-governance.md` and `docs/production-adoption-checklist.md` keep effectivity as link-parent satellite guidance and keep dependent child / effectivity-specific API expansion out of the current public claim set.

Blocking findings
- This cannot pass as a closure-only ticket: the named repository surfaces have not been rolled to v0.49 yet, no `docs/releases/v0.49.0.md` file exists, and the searched docs still advertise the v0.48 / 8.48.0 / 10.48.0 baseline.

Required PO actions
- Remove or override the closure-only routing for this ticket and hand it off as a normal developer documentation task.
- If Product really wants a closure-only outcome, replace the current claim with concrete landed-evidence references to the updated v0.49 doc paths; the present branch does not supply that evidence.
- Keep the existing delivery contract content, since it is otherwise bounded and has no open questions.

Open issues ledger
- critic-item-1 [required-po-action] Remove or override the closure-only routing for this ticket and hand it off as a normal developer documentation task.
- critic-item-2 [required-po-action] If Product really wants a closure-only outcome, replace the current claim with concrete landed-evidence references to the updated v0.49 doc paths; the present branch does not supply that evidence.
- critic-item-3 [required-po-action] Keep the existing delivery contract content, since it is otherwise bounded and has no open questions.
- critic-item-4 [blocking-finding] This cannot pass as a closure-only ticket: the named repository surfaces have not been rolled to v0.49 yet, no `docs/releases/v0.49.0.md` file exists, and the searched docs still advertise the v0.48 / 8.48.0 / 10.48.0 baseline.

Missing examples / edge cases
- No completed-repo example of the claimed v0.49 documentation state is cited: there is no tracked `docs/releases/v0.49.0.md` and no observed 8.49.0 / 10.49.0 doc surface to point at.

Risky assumptions
- Assuming a closure-only ticket can be approved before any of the named docs actually carry the v0.49 baseline.
- Assuming ticket metadata commits are sufficient evidence for a documentation rollover.
- Assuming the current v0.48 docs are already aligned enough to close a v0.49 release-doc ticket without developer work.

AC / test suggestions
- If this stays a normal doc task, keep closure evidence tied to exact updated paths: `CHANGELOG.md`, `README.md`, `docs/package-compatibility.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, the support-bundle guidance path, and `docs/releases/v0.49.0.md`.
- Require final closure evidence to show both version rollover consistency and the preserved boundary wording for same-hub mapper parity, support-bundle-only typed helpers, analyzer net10 build-host guidance, and deferred dependent-child / effectivity-specific APIs.

Implementation watchouts
- The same version baseline is repeated across multiple docs, so partial rollover is the main contradiction risk.
- Do not widen the docs beyond repository-backed scope: no pure .NET 8 SDK analyzer claim, no dependent-child support claim, and no effectivity-specific API claim.
- Keep typed read helpers distinct from typed save mappers; same-hub mapper parity and support-bundle-driven read-helper limits are separate boundaries.

Non-blocking notes
- The persisted contract itself is coherent: `description.md` already narrows scope, keeps implementation work out of scope, and records `Open Questions - none`.
- The repository does already contain direct source/doc evidence for the intended boundary language around explicit `IDataVaultSaveService`, same-hub unique participant names, support-bundle-only typed helpers, and deferred effectivity/dependent-child expansion.

Split recommendations
- If a no-work / already-satisfied audit is still desired, create a separate closure-only follow-up after the v0.49 doc edits land; keep ticket `06FF4430YGFJV43ZS54RXEJD5R` as the actual documentation implementation task.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment