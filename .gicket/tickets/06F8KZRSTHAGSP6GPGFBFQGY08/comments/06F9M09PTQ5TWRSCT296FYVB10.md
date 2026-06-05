[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket contract is structured, but the repository already contains the practical performance-profile examples, fallback guidance, rerun triggers, and checklist routing that this ticket says still need to be added, so the scope is stale and no clear developer delta remains.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZRSTHAGSP6GPGFBFQGY08/description.md` says the remaining work is to add practical examples, fallback cases, and rerun/non-optimization guidance to `docs/performance-profiles.md`, and its `## Open Questions` section is `none`.
- `docs/performance-profiles.md` already contains the requested content on the inspected branch baseline: stop/fallback/rerun guidance around `:140-152`, concrete chunked/materialized/async save examples around `:198-225`, staged provider-ingestion guidance around `:235-297`, and SQLite latest-satellite plus PIT/bridge read examples with fallback/rerun guidance around `:318-352`.
- `docs/production-adoption-checklist.md:94` already routes adopters to `Performance Profiles`, and `:115` already records the SQLite-only latest-satellite and diagnostics-gated PIT/bridge evidence posture the ticket treats as optional checklist follow-up.
- `git diff --name-only develop...ticket/06F8KZRSTHAGSP6GPGFBFQGY08-task-add-bounded-performance-decision-tree-docum` returned only `.gicket/tickets/06F8KZRSTHAGSP6GPGFBFQGY08/**` paths; there is no current non-`.gicket` docs delta on the ticket branch.

Blocking findings
- The ticket no longer describes a clear repository change: the examples, fallback branches, and rerun/stop-condition guidance it asks a developer to add are already present in `docs/performance-profiles.md`, so a dev handoff would duplicate landed documentation or force the developer to guess at an unstated residual gap.
- The ticket's Definition of Done requires a repository diff that updates `docs/performance-profiles.md` outside `.gicket`, but the current branch diff against `develop` contains only ticket-metadata files under `.gicket`, which is consistent with the scope already being satisfied elsewhere rather than pending implementation on this ticket branch.

Required PO actions
- Reconcile ticket `06F8KZRSTHAGSP6GPGFBFQGY08` against current `develop` and either close it as already satisfied/no-work-required or rewrite it around a specific remaining documentation gap that is not already present in `docs/performance-profiles.md`.
- If the ticket stays open, replace the current broad acceptance criteria with delta-based criteria that name the exact missing section(s), example(s), or wording still absent on `develop`, and remove expectations that are already landed in `docs/performance-profiles.md` and `docs/production-adoption-checklist.md`.

Open issues ledger
- critic-item-1 [required-po-action] Reconcile ticket `06F8KZRSTHAGSP6GPGFBFQGY08` against current `develop` and either close it as already satisfied/no-work-required or rewrite it around a specific remaining documentation gap that is not already present in `docs/performance-profiles.md`.
- critic-item-2 [required-po-action] If the ticket stays open, replace the current broad acceptance criteria with delta-based criteria that name the exact missing section(s), example(s), or wording still absent on `develop`, and remove expectations that are already landed in `docs/performance-profiles.md` and `docs/production-adoption-checklist.md`.
- critic-item-3 [blocking-finding] The ticket no longer describes a clear repository change: the examples, fallback branches, and rerun/stop-condition guidance it asks a developer to add are already present in `docs/performance-profiles.md`, so a dev handoff would duplicate landed documentation or force the developer to guess at an unstated residual gap.
- critic-item-4 [blocking-finding] The ticket's Definition of Done requires a repository diff that updates `docs/performance-profiles.md` outside `.gicket`, but the current branch diff against `develop` contains only ticket-metadata files under `.gicket`, which is consistent with the scope already being satisfied elsewhere rather than pending implementation on this ticket branch.

Missing examples / edge cases
- No concrete missing adopter example is locally evident from the current repository state; if PO believes a gap remains, identify the exact heading or decision branch still missing from `docs/performance-profiles.md` on `develop`.
- If the real remaining work is release-baseline cross-linking or public-baseline cleanup rather than performance-profile examples, that should be restated explicitly instead of leaving this ticket framed as an example-addition task.

Risky assumptions
- This review assumes `develop` is the correct pre-development baseline for developer handoff; all inspected branch-history evidence points to that baseline.
- This review assumes the already-landed `docs/performance-profiles.md` sections satisfy the currently written contract; if PO sees a qualitative gap, that gap is not expressed concretely enough in the ticket or branch diff to hand to a developer safely.

AC / test suggestions
- For any retargeted ticket, require a concrete non-`.gicket` diff against `develop` and name the exact section headings expected to change, rather than using generic language like `add examples`.
- If the ticket is reopened with real residual scope, add reviewer evidence criteria that the new wording introduces a decision point or fallback case not already covered by the current `docs/performance-profiles.md` sections around the runtime summary, chunked-ingestion, staged-provider, and read-model profiles.

Implementation watchouts
- Do not reopen this as a vague documentation-touch task; any residual work must preserve the existing SQLite-only latest-satellite claim and diagnostics-gated PIT/bridge/provider-ingestion boundaries already documented.
- Avoid duplicating the existing checklist routing or restating the decision tree as a second competing model outside `docs/performance-profiles.md`.

Non-blocking notes
- The contract's `## Open Questions` section is `none`; the return decision is driven by stale scope, not unresolved contract questions.

Split recommendations
- No split recommended. Reconcile or close this ticket first; only create a new follow-up if PO can name a specific residual documentation gap against the current `develop` baseline.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment