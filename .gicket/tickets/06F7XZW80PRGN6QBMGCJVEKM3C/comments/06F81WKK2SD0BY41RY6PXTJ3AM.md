[gicket-bot] PO-critic review contract

Summary
- The refined contract is technically clear and backed by landed repository evidence, but this epic is not a valid developer handoff as written because the work is already integrated and the ticket still carries stale pre-refinement scope.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:12-17 says this epic is a tracking/refinement boundary over the already-landed v0.24.0 repository surface, limits EF safety to guidance only, and keeps analyzer scope to DMV1910/DMV1911; :55-56 shows Open Questions is none.
- git diff --name-only develop...HEAD lists only .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/*, so the owner branch has no src/, docs/, tests/, or benchmark deltas beyond ticket metadata.
- git merge-base HEAD develop returned 5dba3a68f82713ec1542f9055378795debf2b1ef and git rev-list --left-right --count develop...HEAD returned 0 4, so the branch is just four workflow commits ahead of develop.
- git log --oneline --decorate develop --grep 06F7Y0 shows AUTO-INTEGRATION squashes on develop for all six child tickets: ea67fe2c5, 42e49e889, 1708faaf5, 90ed07497, fb98d0404, and 5dba3a68f.
- Landed repository evidence already matches the refined boundary: src/DCoding.Data.DVault/DataVaultSaveService.cs:50-60 exposes the async chunk-source overload; src/DCoding.Data.DVault/DataVaultSaveServiceAsyncExtensions.cs:12-30 and 63-91 implement bounded async helpers; src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs:30 and 86-100 keys registry-backed contexts by source kind and fingerprint; src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:8-22 only defines DMV1910/DMV1911; benchmark-summary.md:42-44 and 59-60 contains the async streaming and fixed-model pooling rows.

Blocking findings
- Stale conflicting scope remains in the same description. .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:81-84 still says Scope In includes analyzer/runtime diagnostics for EF Core model-cache, compiled-model, and DbContext pooling risks, which conflicts with the authoritative contract at :14-17 and :37.

Required PO actions
- Reclassify this epic to closure-only, done, or no-work-required, or explicitly restate a concrete remaining developer deliverable that is not already landed and reopen only the ticket or tickets that actually carry that work.
- Remove, archive, or explicitly neutralize the stale legacy draft section in .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:78-92 so the ticket no longer appears to promise new model-cache or pooling diagnostics.

Open issues ledger
- critic-item-1 [required-po-action] Reclassify this epic to closure-only, done, or no-work-required, or explicitly restate a concrete remaining developer deliverable that is not already landed and reopen only the ticket or tickets that actually carry that work.
- critic-item-2 [required-po-action] Remove, archive, or explicitly neutralize the stale legacy draft section in .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:78-92 so the ticket no longer appears to promise new model-cache or pooling diagnostics.
- critic-item-3 [blocking-finding] Stale conflicting scope remains in the same description. .gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:81-84 still says Scope In includes analyzer/runtime diagnostics for EF Core model-cache, compiled-model, and DbContext pooling risks, which conflicts with the authoritative contract at :14-17 and :37.

Missing examples / edge cases
- No blocking technical example or edge-case gap was found inside the refined v0.24.0 boundary; the blocker is the ticket's workflow classification after the work already landed.

Risky assumptions
- Assuming a developer should act on this epic despite git evidence showing no repo delta beyond ticket metadata on the owner branch.
- Assuming readers will ignore the legacy draft's broader analyzer/runtime-diagnostics wording because the contract block says it is non-authoritative.

AC / test suggestions
- If PO keeps any follow-on work open, add one acceptance item that names the specific not-yet-landed artifact or child ticket; otherwise mark the epic closure-only instead of sending it to dev.
- Keep acceptance text tied to the already-observed evidence surfaces that exist in-repo today: the async save API and public snapshot, the streaming contract docs, the analyzer catalog limit, and the benchmark-summary triplet.

Implementation watchouts
- The epic title plus the stale legacy draft can be misread as a request for new model-cache or pooling diagnostics even though the authoritative contract excludes them.
- Because develop already contains the six child integrations, any reopened engineering handoff risks duplicate or scope-creep work unless the PO rewrites the ticket to identify a truly remaining delta.

Non-blocking notes
- The refined contract itself is internally consistent and satisfies the hard gate on open questions.
- The repository evidence is strong and already aligned across code, docs, analyzer README, compiled-compatibility guidance, and benchmark artifacts; the return is about ticket state and residual scope text, not missing technical grounding.

Split recommendations
- Keep any future provider-native async write or provider-specific async execution claims in a separate follow-on ticket rather than reopening this epic.
- Keep any future model-cache, compiled-model, or pooling analyzer/runtime guardrails in a separate follow-on ticket if guidance-only EF safety proves insufficient.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment