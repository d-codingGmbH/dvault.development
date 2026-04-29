[gicket-bot] PO-critic review contract

Summary
- Persisted contract is bounded and ready for a documentation-only developer handoff; no unresolved Open Questions remain.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Read .gicket/tickets/06EXB6QNB799DKQHRAZ5BY38H0/description.md: PO handoff is 'ready_for_po_critic', scope is documentation-only, implementation providers/schema generators/migrations/hashing code/runtime APIs are Scope Out, and Open Questions contains '- none'.
- The persisted Acceptance Criteria require deterministic v1 defaults for names, metadata fields, hashing behavior, provider-neutral mapping, explicit required defaults vs optional overrides, deterministic-preserving overrides, and recording deferred decisions as follow-up items.
- Comment 06EXCKQSKQ3KNTZK6CTGJPRP2W.md records the PO refinement contract with Open questions 'none'; comment 06EXCKRKQTAE0NGM0PHJ6DWQC8.md reports outcome 'po-refinement-ready' and says the durable refinement contract was updated.
- Comments 06EXCKVY2QJGQ8DZYP3KY8GSHW.md and 06EXCKW1DWZCDYM5Z6N3X9PJ6G.md show the po-critic claim and active lease acquired at <redacted>-28T22:57:<redacted>+00:00.
- Repository layout check: root ls showed only .gicket, .gicket-bot, and .git; git ls-files docs .gicket-bot src test tests returned only .gicket-bot/.gitignore; ls docs reported no such file, supporting the foundation-stage/no source-or-test-root premise.
- Relation .gicket/relations/V8/H0/06EXB6QD5Y9XVVZDVZEN4M6EV8--06EXB6QNB799DKQHRAZ5BY38H0--parentOf.json links parent story 06EXB6QD5Y9XVVZDVZEN4M6EV8 to this task; the parent story scopes convention-first defaults and optional advanced configuration hooks.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No PO-blocking gaps. During delivery, the policy should include concrete rules or examples for logical names, metadata field semantics, hash canonicalization, mutable vs immutable timestamp handling, and override behavior so deterministic review is possible.

Risky assumptions
- The ticket leaves exact logical names and hash algorithm selection to the policy author; this is acceptable for handoff because the Acceptance Criteria require the produced policy to settle deterministic v1 defaults.
- Provider-neutral wording can hide provider constraints; the contract already calls out this risk and requires logical defaults that adapters can map without provider-specific commitments.

AC / test suggestions
- Review the delivered artifact with a documentation checklist covering approved path, names, metadata fields, hashing defaults, provider-neutral mapping, override categories, and deferred decisions.
- No code test suite is required for this documentation-only ticket; acceptance should verify that two implementers could derive the same logical persistence shape from the artifact.

Implementation watchouts
- Keep the work to a planning/documentation artifact under docs/plans or .gicket-bot/planning; do not implement persistence providers, schema generators, migrations, hashing code, or runtime configuration APIs.
- Use normative language to distinguish MUST defaults from SHOULD/MAY extension points, and label examples as illustrative unless they are required defaults.
- Avoid public API, type, class, helper, or package-layout commitments because the contract explicitly scopes those out and no source roots currently exist.

Non-blocking notes
- The chosen documentation path may need to be created because the current root has no docs directory and .gicket-bot currently contains .gitignore, logs, and policy.json.
- The worktree has ticket and bot metadata modifications, but the persisted ticket file and comments directly show the refined contract and po-critic claim used for this review.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment