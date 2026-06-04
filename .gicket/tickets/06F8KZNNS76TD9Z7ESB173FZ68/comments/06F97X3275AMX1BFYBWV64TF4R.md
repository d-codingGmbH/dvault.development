[gicket-bot] PO refinement contract

Summary
- Refined the ticket around creating the missing `docs/releases/v0.29.0.md` baseline and updating public documentation to explain provider schema guardrail behavior, examples, adoption workflow, and limitations, with no child-ticket or relation materialization in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already provides the planning anchor in `docs/plans/provider-identifier-ddl-guardrail-contract.md`; this ticket should document that contract rather than reopen provider-baseline decisions.
- `docs/releases/v0.29.0.md` is currently missing, while `README.md` and `docs/production-adoption-checklist.md` still treat v0.28.0 as the current public documentation baseline.
- The supported-provider baseline is finite and already ratified as SQLite, Oracle, PostgreSQL, SQL Server, and MySQL with their existing DVault provider profiles; unrecognized providers must not inherit provider-specific DDL safety claims.
- No ticket comments or closure evidence amendments were present in the supplied context.
- Live ticket, relation, comment, and attachment reads through gicket were trust-policy blocked in this run, so no relation cleanup, attachment reuse, child-ticket creation, or description update was materialized.

Scope In
- Create the public v0.29.0 release notes document for the coordinated seven-package DVault release without claiming package publication.
- Update public adopter-facing docs to make v0.29.0 the current documentation baseline and route readers to the new provider schema guardrail guidance.
- Document provider DDL guardrail behavior for the existing five supported provider profiles, including identifier-safety boundaries, included-index and duplicate-index caveats, load-timestamp storage implications, and fail-fast handling for unsafe provider-specific DDL shapes.
- Document the expected adopter workflow around reviewed artifacts, design-time validation/drift/guardrail commands, and migration review before applying provider-specific DDL.
- Add concrete examples that show how provider-specific identifier or migration constraints affect generated schema or guardrail outcomes.

Scope Out
- Changing provider capability profiles, annotations, diagnostics, or migration guardrail runtime behavior in source code.
- Adding support claims for providers beyond the existing SQLite, Oracle, PostgreSQL, SQL Server, and MySQL profiles.
- Recording actual NuGet publication evidence, package hashes, or release distribution tasks.
- Broader documentation rewrites unrelated to the v0.29.0 provider schema guardrail slice.

Open questions
- none

Follow-up questions
- After v0.29.0 docs land, should a separate discoverability pass add more cross-links from design-time workflow or migration guidance if adopters still miss the guardrail entry points?
- Who will record final publication evidence once packages are actually shipped, since the release notes themselves should remain publication-neutral?

Risks
- Live relation and attachment state could not be re-verified through gicket because the provided ticket read tools were trust-policy blocked, so hidden coordination context may still exist outside the prompt snapshot.
- If provider capability names or guardrail diagnostic terms change before implementation lands, the documentation could drift from the current planning contract.
- The main content risk is overclaiming provider support or automatic remediation; reviewer attention should stay on keeping the guardrail guidance bounded to the existing supported profiles and review workflow.

Split recommendations
- No split recommended; current evidence keeps this as one bounded documentation ticket spanning the missing v0.29.0 release notes and coordinated public-doc updates.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment