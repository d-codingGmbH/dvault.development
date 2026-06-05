[gicket-bot] PO-critic review contract

Summary
- Return to PO: the repo proves the generator/test contract, but the epic's documentation closure coverage is not landed. Child task `06F8KZQAWZ7QRGB68KB21C9B0R` is marked `done` even though the repo still lacks its promised v0.30.0 release note and freshness/troubleshooting doc updates.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Epic contract `.gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/description.md:26-37` requires repo-facing docs/tests alignment and says README, architecture, and release-note surfaces must describe the same support-bundle/readShape boundary.
- `ls /mnt/c/Projects/DVault/docs/releases` shows `v0.29.0.md` as the newest release note present; `docs/releases/v0.30.0.md` is absent.
- `README.md:25` still names `docs/releases/v0.29.0.md` as the current coordinated release baseline.
- `README.md:371-390` documents exactly one authoritative `dvault.support-bundle.v1` input and helper-shape limits, but it does not contain the child contract's required recovery sequence for stale bundles or stale `DVaultTypedReadModelMetadataSourceFingerprint` pins; `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-181` documents support-bundle export and `CreateSupportBundleDiagnostics`, but not the requested stale-input troubleshooting example/checklist.
- `git -C /mnt/c/Projects/DVault diff --name-status develop...HEAD -- README.md docs/architecture/dvault-dotnet-ef-design-time-workflow.md docs/releases src/DCoding.Data.DVault.Analyzers/README.md tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs` returned no repo-surface changes on the epic branch.
- `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:70-85` enforces exactly one authoritative `dvault.support-bundle.v1`; `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:<redacted>` directly covers `DMV1961`, missing-bundle `DMV1960`, and helper-specific skip/partial-generation outcomes `DMV1963`, `DMV1964`, `DMV1967`, and `DMV1969`.

Blocking findings
- The epic Definition of Done is not met on documentation surfaces: the required current release-note surface does not exist (`docs/releases/v0.30.0.md` is missing), README still points to v0.29.0 as the current baseline, and the required freshness/troubleshooting examples are not present in README/workflow docs.
- Tracking coverage is inconsistent: child task `06F8KZQAWZ7QRGB68KB21C9B0R` is already `done`, but its own acceptance criteria and DoD are not satisfied by current repository evidence, so remaining work is no longer represented by an active delivery ticket.

Required PO actions
- Reopen `06F8KZQAWZ7QRGB68KB21C9B0R` or create one bounded replacement documentation ticket for the still-missing repo work: README freshness/recovery wording, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` troubleshooting guidance, and a new `docs/releases/v0.30.0.md`.
- Update the epic's tracking state so remaining documentation work is represented by an active child/follow-up instead of leaving the epic with all children marked done.
- Before resubmitting this epic for closure-style review, provide landed repo evidence that the documentation carrier's acceptance criteria are satisfied and, if closure is intended, reconcile or explicitly supersede the stale incoming `blocks` relation from `06F8KZQAWZ7QRGB68KB21C9B0R`.

Open issues ledger
- critic-item-1 [required-po-action] Reopen `06F8KZQAWZ7QRGB68KB21C9B0R` or create one bounded replacement documentation ticket for the still-missing repo work: README freshness/recovery wording, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` troubleshooting guidance, and a new `docs/releases/v0.30.0.md`.
- critic-item-2 [required-po-action] Update the epic's tracking state so remaining documentation work is represented by an active child/follow-up instead of leaving the epic with all children marked done.
- critic-item-3 [required-po-action] Before resubmitting this epic for closure-style review, provide landed repo evidence that the documentation carrier's acceptance criteria are satisfied and, if closure is intended, reconcile or explicitly supersede the stale incoming `blocks` relation from `06F8KZQAWZ7QRGB68KB21C9B0R`.
- critic-item-4 [blocking-finding] The epic Definition of Done is not met on documentation surfaces: the required current release-note surface does not exist (`docs/releases/v0.30.0.md` is missing), README still points to v0.29.0 as the current baseline, and the required freshness/troubleshooting examples are not present in README/workflow docs.
- critic-item-5 [blocking-finding] Tracking coverage is inconsistent: child task `06F8KZQAWZ7QRGB68KB21C9B0R` is already `done`, but its own acceptance criteria and DoD are not satisfied by current repository evidence, so remaining work is no longer represented by an active delivery ticket.

Missing examples / edge cases
- A consumer-facing recovery sequence is still missing for metadata changes: regenerate the reviewed `dvault.support-bundle.v1`, update or remove a stale pinned `DVaultTypedReadModelMetadataSourceFingerprint`, then rebuild.
- A troubleshooting example is still missing for PIT/bridge helper gaps caused by missing request-bound `readShape` evidence, including re-running representative `CreateSupportBundleDiagnostics` inputs before regenerating typed helpers.

Risky assumptions
- Assuming child `06F8KZQAWZ7QRGB68KB21C9B0R` is satisfied because `ticket.json` says `done`, even though its own contract and current repo state still show the three documentation gaps as open.
- Assuming the stale relation file `.gicket/relations/0R/DG/06F8KZQAWZ7QRGB68KB21C9B0R--06F8KZP0VKMXGE0JXPZRD1RQDG--blocks.json` is harmless housekeeping because epic `ticket.json` says `is-blocked=false` and comment `.gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/comments/06F9EFYZ8KJ0FDVQ861NB1ZATG.md:18-22` marks follow-up obsolete.

AC / test suggestions
- Keep any reopened/replacement documentation ticket anchored to the already-observed bounded AC in `.gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/description.md:31-36`; do not broaden it into generator/runtime redesign.
- Treat the existing source/test evidence in `DataVaultTypedReadModelSourceGenerator.cs` and `DataVaultTypedReadModelSourceGeneratorTests.cs` as already-landed implementation proof for the epic's diagnostics/partial-generation behavior.

Implementation watchouts
- Do not reopen architecture or change generator behavior; reuse wording from `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md` and `src/DCoding.Data.DVault.Analyzers/README.md` for DMV1960/<redacted>/1969 and helper-specific skip behavior.
- Keep the epic as a tracking parent; the remaining gap is a bounded documentation follow-through task, not new runtime or analyzer scope.

Non-blocking notes
- Epic `## Open Questions` is `none` in `.gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/description.md:46-47`, so ambiguity is not the reason for return.
- The core support-bundle/fingerprint/readShape contract is directly evidenced in source, analyzer README, architecture docs, and tests; the unresolved gap is documentation closure coverage and status consistency.
- All four current child tickets read as `done` in `.gicket/tickets/*/ticket.json`; the issue is not missing decomposition, it is that one done child still lacks repo-visible completion evidence.

Split recommendations
- No further split beyond a single bounded documentation carrier. Prefer reopening `06F8KZQAWZ7QRGB68KB21C9B0R`; if that is not acceptable, create one replacement task and keep the epic tracking-only.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment