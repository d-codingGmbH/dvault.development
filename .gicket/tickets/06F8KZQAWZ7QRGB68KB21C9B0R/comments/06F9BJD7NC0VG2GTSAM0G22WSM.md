[gicket-bot] PO-critic review contract

Summary
- The ticket contract is specific enough for development, but it is not valid as a closure-only handoff because the repository still lacks the requested v0.30.0 documentation updates.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/description.md:30-36` defines acceptance criteria for refresh/troubleshooting doc updates plus a new `docs/releases/v0.30.0.md`, and `description.md:51-52` shows `## Open Questions` is `none`.
- `README.md:371-390` already documents exactly one authoritative `dvault.support-bundle.v1`, optional `DVaultTypedReadModelMetadataSourceFingerprint`, and partial PIT/bridge helper suppression, which confirms the baseline is repo-grounded.
- `src/DCoding.Data.DVault.Analyzers/README.md:67-91` already documents the typed-helper boundary and the shipped `DMV1960`/`DMV1961`/`DMV1963`/`DMV1964`/`DMV1967` diagnostic mapping.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-181` already documents `support-bundle` export and `CreateSupportBundleDiagnostics`, but the current text stops at bundle generation and request-bound evidence supply.
- Checking `/mnt/c/Projects/DVault/docs/releases/v0.30.0.md` returned `missing`.
- `git -C /mnt/c/Projects/DVault diff --name-status 42322633eba1e35ded4f62b5c499d55cf8774eab...HEAD -- README.md src/DCoding.Data.DVault.Analyzers/README.md docs/architecture/dvault-dotnet-ef-design-time-workflow.md docs/releases/v0.30.0.md` returned no output, so this review branch does not carry additional repo evidence for the requested doc pass.

Blocking findings
- The prompt marks this as a closure-only audit, but the persisted contract and current repository state still require net-new documentation work. Closure-only approval is therefore unsupported on the current routing.
- The repository does not yet satisfy the ticket's own closure evidence: `docs/releases/v0.30.0.md` is absent, `README.md:371-390` does not yet describe the refresh/recovery workflow after bundle or fingerprint changes, and `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-181` does not yet show the requested stale-bundle troubleshooting example.

Required PO actions
- Remove or correct the closure-only posture for `06F8KZQAWZ7QRGB68KB21C9B0R` and reroute it as a normal documentation implementation ticket for developer handoff.
- If Product wants this to remain closure-only, first land repository evidence for the scoped documentation changes, including a new `docs/releases/v0.30.0.md`, then resubmit the ticket with that landed evidence.

Open issues ledger
- critic-item-1 [required-po-action] Remove or correct the closure-only posture for `06F8KZQAWZ7QRGB68KB21C9B0R` and reroute it as a normal documentation implementation ticket for developer handoff.
- critic-item-2 [required-po-action] If Product wants this to remain closure-only, first land repository evidence for the scoped documentation changes, including a new `docs/releases/v0.30.0.md`, then resubmit the ticket with that landed evidence.
- critic-item-3 [blocking-finding] The prompt marks this as a closure-only audit, but the persisted contract and current repository state still require net-new documentation work. Closure-only approval is therefore unsupported on the current routing.
- critic-item-4 [blocking-finding] The repository does not yet satisfy the ticket's own closure evidence: `docs/releases/v0.30.0.md` is absent, `README.md:371-390` does not yet describe the refresh/recovery workflow after bundle or fingerprint changes, and `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-181` does not yet show the requested stale-bundle troubleshooting example.

Missing examples / edge cases
- none

Risky assumptions
- Assuming the ticket can close because the sibling freshness/fingerprint implementation tickets are done; those tickets do not replace this ticket's own documentation deliverables.
- Assuming relation cleanup is fully materialized already; `.gicket/relations/H8/0R/06F8KZPZZE8VZEBANP5MPN8HH8--06F8KZQAWZ7QRGB68KB21C9B0R--blocks.json` still exists even though `.gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/ticket.json` shows `is-blocked: false` and comment `06F9BGN6TPRGF0648F9D27SFYG.md` says the follow-up was dropped as obsolete.

AC / test suggestions
- Once rerouted as normal dev work, keep acceptance anchored to the already-observed source baseline rather than reopening analyzer behavior: README refresh guidance, analyzer README diagnostic mapping, support-bundle troubleshooting guidance, and an additive `docs/releases/v0.30.0.md`.

Implementation watchouts
- Do not imply raw `dvault.model.v1` is a direct generator input; current repo docs say model-first changes matter only after projection into the authoritative support bundle.
- Do not imply DVault invents representative PIT or bridge requests; the request-bound `ReadShape` evidence remains consumer-supplied through `CreateSupportBundleDiagnostics`.
- Keep the partial-generation boundary explicit: unsupported PIT or bridge facts suppress only the affected helper while unrelated supported helpers from the same bundle can still generate.

Non-blocking notes
- The ticket contract itself is otherwise developer-ready: `## Open Questions` is `none`, the sibling prerequisite tickets are `done`, and current source already grounds the diagnostic and request-bound evidence boundary.

Split recommendations
- No split recommendation; fix the routing mismatch first.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment